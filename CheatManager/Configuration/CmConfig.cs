using System;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Common.ConfigurationParser;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace CheatManager.Configuration
{
    public static class CmConfig
    {
        public static string PROGRAM_VERSION = string.Empty;
        public static string CONFIG_VERSION = string.Empty;

        private static readonly string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string FILENAME = $"{modFolder}/config.txt";

        internal static int overPowerMultiplier;
        internal const float ASPECT = 4.8f;
        internal static int hungerAndThirstInterval;
        
        private static readonly string[] SECTIONS = { "Hotkeys", "Program", "Settings", "ToggleButtons", "UserWarpTargets" };
        internal static Dictionary<string, KeyCode> KEYBINDINGS;
        internal static Dictionary<string, string> Section_hotkeys;
        internal static Dictionary<string, string> Section_program;
        internal static Dictionary<string, string> Section_settings;
        internal static Dictionary<string, string> Section_toggleButtons;
        internal static Dictionary<string, string> Section_userWarpTargets;
        
        internal static bool isConsoleEnabled { get; set; }
        internal static bool isInfoBarEnabled { get; set; }

        internal static readonly string[] SECTIONKEYS_Hotkeys =
        {
            "ToggleWindow",
            "ToggleMouse",
            "ToggleConsole"
        };

        internal static readonly string[] SECTIONKEYS_Program =
        {
            "EnableConsole",
            "EnableInfoBar"
        };

        internal static readonly string[] SECTIONKEYS_Settings =
        {
            "OverPowerMultiplier",
            "HungerAndThirstInterval"
        };

        internal static readonly string[] SECTIONKEYS_ToggleButtons =
        {
            "fastbuild",
            "fastscan",
            "fastgrow",
            "fasthatch",
            "filterfast",
            "radiation",
            "invisible",
            "nodamage",
            "alwaysday",
            "noinfect",
            "overpower"
        };

        private static readonly List<ConfigData> DEFAULT_CONFIG = new List<ConfigData>
        {
            new ConfigData(SECTIONS[0], SECTIONKEYS_Hotkeys[0], KeyCode.F5.ToString()),
            new ConfigData(SECTIONS[0], SECTIONKEYS_Hotkeys[1], KeyCode.F4.ToString()),
            new ConfigData(SECTIONS[0], SECTIONKEYS_Hotkeys[2], KeyCode.Delete.ToString()),

            new ConfigData(SECTIONS[1], SECTIONKEYS_Program[0], true.ToString()),
            new ConfigData(SECTIONS[1], SECTIONKEYS_Program[1], false.ToString()),

            new ConfigData(SECTIONS[2], SECTIONKEYS_Settings[0], 2.ToString()),
            new ConfigData(SECTIONS[2], SECTIONKEYS_Settings[1], 10.ToString()),

            new ConfigData(SECTIONS[3], SECTIONKEYS_ToggleButtons[0], false.ToString()),
            new ConfigData(SECTIONS[3], SECTIONKEYS_ToggleButtons[1], false.ToString()),
            new ConfigData(SECTIONS[3], SECTIONKEYS_ToggleButtons[2], false.ToString()),
            new ConfigData(SECTIONS[3], SECTIONKEYS_ToggleButtons[3], false.ToString()),
            new ConfigData(SECTIONS[3], SECTIONKEYS_ToggleButtons[4], false.ToString()),
            new ConfigData(SECTIONS[3], SECTIONKEYS_ToggleButtons[5], false.ToString()),
            new ConfigData(SECTIONS[3], SECTIONKEYS_ToggleButtons[6], false.ToString()),
            new ConfigData(SECTIONS[3], SECTIONKEYS_ToggleButtons[7], false.ToString()),
            new ConfigData(SECTIONS[3], SECTIONKEYS_ToggleButtons[8], false.ToString()),
            new ConfigData(SECTIONS[3], SECTIONKEYS_ToggleButtons[9], false.ToString()),
            new ConfigData(SECTIONS[3], SECTIONKEYS_ToggleButtons[10], false.ToString()),

            new ConfigData(SECTIONS[4], string.Empty, string.Empty)
        };

        /*
        public override string ProgramName => "CheatManager";
        public override List<ConfigDataNew> DEFAULT_CONFIG => DEFAULT;
        
        private static List<ConfigDataNew> DEFAULT = new List<ConfigDataNew>
        {
            new ConfigDataNew("Hotkeys",
                new Dictionary<string, string>
                {
                    { "ToggleWindow", KeyCode.F5.ToString() },
                    { "ToggleMouse",  KeyCode.F4.ToString() },
                    { "ToggleConsole", KeyCode.Delete.ToString() }
                }),

            new ConfigDataNew("Program",
                new Dictionary<string, string>
                {
                    { "EnableConsole", false.ToString() },
                    { "EnableInfoBar", false.ToString() }                    
                }),

            new ConfigDataNew("Settings",
                new Dictionary<string, string>
                {
                    { "OverPowerMultiplier", 2.ToString() },
                    { "HungerAndThirstInterval", 1.ToString() }
                }),

            new ConfigDataNew("ToggleButtons",
                new Dictionary<string, string>
                {
                    { "fastbuild",  false.ToString() },
                    { "fastscan",   false.ToString() },
                    { "fastgrow",   false.ToString() },
                    { "fasthatch",  false.ToString() },
                    { "filterfast", false.ToString() },
                    { "radiation",  false.ToString() },
                    { "invisible",  false.ToString() },
                    { "nodamage",   false.ToString() },
                    { "alwaysday",  false.ToString() },
                    { "noinfect",   false.ToString() },
                    { "overpower",  false.ToString() }
                }),

             new ConfigDataNew("UserWarpTargets",
                new Dictionary<string, string>
                {
                    { string.Empty, string.Empty },                    
                }),
        };
        */


        internal static void Load()
        {          
            PROGRAM_VERSION = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

            if (!Check())
            {
                CreateDefault();
            }            

            Section_hotkeys = ParserHelper.GetAllKeyValuesFromSection(FILENAME, "Hotkeys", SECTIONKEYS_Hotkeys);
            Section_program = ParserHelper.GetAllKeyValuesFromSection(FILENAME, "Program", SECTIONKEYS_Program);
            Section_settings = ParserHelper.GetAllKeyValuesFromSection(FILENAME, "Settings", SECTIONKEYS_Settings);
            Section_toggleButtons = ParserHelper.GetAllKeyValuesFromSection(FILENAME, "ToggleButtons", SECTIONKEYS_ToggleButtons);
            Section_userWarpTargets = ParserHelper.GetAllKVPFromSection(FILENAME, "UserWarpTargets");
        }
        
        internal static void Write()
        {
            ParserHelper.SetAllKeyValuesInSection(FILENAME, "Hotkeys", Section_hotkeys);
            ParserHelper.SetAllKeyValuesInSection(FILENAME, "Program", Section_program);
            ParserHelper.SetAllKeyValuesInSection(FILENAME, "Settings", Section_settings);
            ParserHelper.SetAllKeyValuesInSection(FILENAME, "ToggleButtons", Section_toggleButtons);

            if (Main.Instance != null)
            {
                SyncWarpTargets();
            }
        }

        internal static void Set()
        {
            isConsoleEnabled = bool.Parse(Section_program["EnableConsole"]);
            isInfoBarEnabled = bool.Parse(Section_program["EnableInfoBar"]);

            int.TryParse(Section_settings["OverPowerMultiplier"], out overPowerMultiplier);

            if (overPowerMultiplier < 0 && overPowerMultiplier > 10)
            {
                overPowerMultiplier = 2;
                ParserHelper.SetKeyValue(FILENAME, "Settings", "OverPowerMultiplier", 2.ToString());
            }

            int.TryParse(Section_settings["HungerAndThirstInterval"], out hungerAndThirstInterval);

            if (hungerAndThirstInterval < 0 && hungerAndThirstInterval > 10)
            {
                hungerAndThirstInterval = 10;
                ParserHelper.SetKeyValue(FILENAME, "Settings", "HungerAndThirstInterval", 1.ToString());
            }

            SetKeyBindings();

        }

        internal static void SyncWarpTargets()
        {
            Dictionary<IntVector, string> warpTargets_User = Main.Instance.WarpTargets_User;

            Section_userWarpTargets.Clear();

            ParserHelper.ClearSection(FILENAME, "UserWarpTargets");

            if (warpTargets_User.Count > 0)
            {
                foreach (KeyValuePair<IntVector, string> kvp in warpTargets_User)
                {
                    UnityEngine.Debug.Log($"key: {kvp.Key}, value: {kvp.Value}");

                    Section_userWarpTargets.Add(kvp.Key.ToString(), kvp.Value);
                }
            }

            ParserHelper.SetAllKeyValuesInSection(FILENAME, "UserWarpTargets", Section_userWarpTargets);
        }

        internal static void SyncConfig()
        {
            foreach (string key in SECTIONKEYS_Hotkeys)
            {
                Section_hotkeys[key] = KEYBINDINGS[key].ToString();
            }

            //Write();
        }

        internal static void SetKeyBindings()
        {
            KEYBINDINGS = new Dictionary<string, KeyCode>();

            bool sync = false;

            foreach (KeyValuePair<string, string> kvp in Section_hotkeys)
            {
                try
                {
                    KeyCode keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), kvp.Value);
                    KEYBINDINGS.Add(kvp.Key, keyCode);
                }
                catch (ArgumentException)
                {
                    SNLogger.Warn($"({kvp.Value}) is not a valid KeyCode! Setting default value!");

                    for (int i = 0; i < DEFAULT_CONFIG.Count; i++)
                    {
                        if (DEFAULT_CONFIG[i].Key.Equals(kvp.Key))
                        {
                            KEYBINDINGS.Add(kvp.Key, (KeyCode)Enum.Parse(typeof(KeyCode), DEFAULT_CONFIG[i].Value, true));
                            sync = true;
                        }
                    }
                }
            }

            if (sync)
                SyncConfig();
        }

        internal static void CreateDefault()
        {
            SNLogger.Trace("Method call: CmConfig.Config_CreateDefault()");

            SNLogger.Warn("Configuration file is missing or wrong version. Trying to create a new one.");

            try
            {
                ParserHelper.CreateDefaultConfigFile(FILENAME, "CheatManager", PROGRAM_VERSION, DEFAULT_CONFIG);

                ParserHelper.AddInfoText(FILENAME, "OverPowerMultiplier and HungerAndThirstInterval possible values", "1 to 10");

                SNLogger.Log("The new configuration file was successfully created.");
            }
            catch
            {
                SNLogger.Error("An error occured while creating the new configuration file!");
            }
        }

        private static bool Check()
        {
            SNLogger.Trace("Method call: CmConfig.Config_Check()");

            if (!File.Exists(FILENAME))
            {
                SNLogger.Error("Configuration file open error!");
                return false;
            }

            CONFIG_VERSION = ParserHelper.GetKeyValue(FILENAME, "CheatManager", "Version");

            if (!CONFIG_VERSION.Equals(PROGRAM_VERSION))
            {
                SNLogger.Error("Configuration file version error!");
                return false;
            }

            if (!ParserHelper.CheckSectionKeys(FILENAME, "Hotkeys", SECTIONKEYS_Hotkeys))
            {
                SNLogger.Error("Configuration file [Hotkeys] section error!");
                return false;
            }

            if (!ParserHelper.CheckSectionKeys(FILENAME, "Program", SECTIONKEYS_Program))
            {
                SNLogger.Error("Configuration file [Program] section error!");
                return false;
            }

            if (!ParserHelper.CheckSectionKeys(FILENAME, "Settings", SECTIONKEYS_Settings))
            {
                SNLogger.Error("Configuration file [Settings] section error!");
                return false;
            }

            if (!ParserHelper.CheckSectionKeys(FILENAME, "ToggleButtons", SECTIONKEYS_ToggleButtons))
            {
                SNLogger.Error("Configuration file [ToggleButtons] section error!");
                return false;
            }

            return true;
        }
    }
}
