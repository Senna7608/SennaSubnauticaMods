using System;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Common.ConfigurationParser;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace RuntimeHelper.Configuration
{
    public static class RuntimeHelper_Config
    {        
        internal static string PROGRAM_VERSION = string.Empty;
        internal static string CONFIG_VERSION = string.Empty;
        public static bool AUTOSTART = false;
        private static readonly string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string FILENAME = $"{modFolder}/config.txt";

        public static string TitleText;

        private static readonly string[] SECTIONS = { "Settings", "Hotkeys"};

        internal static Dictionary<string, KeyCode> KEYBINDINGS;
        internal static Dictionary<string, string> Section_hotkeys;

        private static readonly string[] SECTION_SETTINGS =
        {
            "Autostart"            
        };

        private static readonly string[] SECTION_HOTKEYS =
        {            
            "ToggleMouse",
            "ToggleRaycastMode",
            "ToggleColliderDrawing"
        };      
        
        
        private static readonly List<ConfigData> DEFAULT_CONFIG = new List<ConfigData>
        {
            new ConfigData(SECTIONS[0], SECTION_SETTINGS[0], false.ToString()),
            new ConfigData(SECTIONS[1], SECTION_HOTKEYS[0], KeyCode.X.ToString()),
            new ConfigData(SECTIONS[1], SECTION_HOTKEYS[1], KeyCode.R.ToString()),
            new ConfigData(SECTIONS[1], SECTION_HOTKEYS[2], KeyCode.Y.ToString()),
        };

        internal static void LoadConfig()
        {
            PROGRAM_VERSION = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

            TitleText = $"Runtime Helper v.{PROGRAM_VERSION}";

            if (!File.Exists(FILENAME))
            {
                CreateDefaultConfigFile();
            }
            else
            {
                CONFIG_VERSION = ParserHelper.GetKeyValue(FILENAME, "RuntimeHelper", "Version");

                if (CONFIG_VERSION.Equals(PROGRAM_VERSION))
                {
                    SNLogger.Log("RuntimeHelper", "Configuration file version match with program version.");
                }
                else
                {
                    CreateDefaultConfigFile();
                }
            }

            Section_hotkeys = ParserHelper.GetAllKeyValuesFromSection(FILENAME, SECTIONS[1], SECTION_HOTKEYS);

            string autostart = ParserHelper.GetKeyValue(FILENAME, SECTIONS[0], SECTION_SETTINGS[0]);

            if (!bool.TryParse(autostart, out AUTOSTART))
            {
                AUTOSTART = false;
            }

            SNLogger.Log("RuntimeHelper", "Configuration loaded.");
        }

        internal static void CreateDefaultConfigFile()
        {
            SNLogger.Warn("RuntimeHelper", "Configuration file is missing or wrong version. Trying to create a new one.");

            try
            {
                ParserHelper.CreateDefaultConfigFile(FILENAME, "RuntimeHelper", PROGRAM_VERSION, DEFAULT_CONFIG);
                SNLogger.Log("RuntimeHelper", "The new configuration file was successfully created.");
            }
            catch
            {
                SNLogger.Error("RuntimeHelper", "An error occured while creating the new configuration file!");
            }
        }

        internal static void InitConfig()
        {
            SetKeyBindings();

            SNLogger.Log("RuntimeHelper", "Configuration initialized.");
        }

        internal static void WriteConfig()
        {
            ParserHelper.SetKeyValue(FILENAME, SECTIONS[0], SECTION_SETTINGS[0], AUTOSTART.ToString());
            ParserHelper.SetAllKeyValuesInSection(FILENAME, SECTIONS[1], Section_hotkeys);            
        }

        internal static void SyncConfig()
        {
            foreach (string key in SECTION_HOTKEYS)
            {
                Section_hotkeys[key] = KEYBINDINGS[key].ToString();
            }

            WriteConfig();
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
                    SNLogger.Warn("RuntimeHelper", $"({kvp.Value}) is not a valid KeyCode! Setting default value!");

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
    }
}
