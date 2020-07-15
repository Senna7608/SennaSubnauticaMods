using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using UnityEngine;
using Common.ConfigurationParser;
using Common;

namespace SlotExtender.Configuration
{
    public static class SEConfig
    {
        public static string PROGRAM_VERSION = string.Empty;
        public static string CONFIG_VERSION = string.Empty;
        
        private static readonly string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string FILENAME = $"{modFolder}/config.txt";

        public static Dictionary<string, string> Hotkeys_Config;
        public static Dictionary<SlotConfigID, string> SLOTKEYBINDINGS = new Dictionary<SlotConfigID, string>();
        public static Dictionary<string, KeyCode> KEYBINDINGS;

        public static int MAXSLOTS;
        public static int EXTRASLOTS;
        public static Color TEXTCOLOR;
        public static int STORAGE_SLOTS_OFFSET = 4;
        public static SlotLayout SLOT_LAYOUT = SlotLayout.Grid;
        public static bool isSeamothArmsExists = false;

        private static readonly string[] SECTION_HOTKEYS =
        {
            "Upgrade",
            "Storage",
            SlotConfigID.Slot_6.ToString(),
            SlotConfigID.Slot_7.ToString(),
            SlotConfigID.Slot_8.ToString(),
            SlotConfigID.Slot_9.ToString(),
            SlotConfigID.Slot_10.ToString(),
            SlotConfigID.Slot_11.ToString(),
            SlotConfigID.Slot_12.ToString(),
            SlotConfigID.SeamothArmLeft.ToString(),
            SlotConfigID.SeamothArmRight.ToString()
        };
        
        private static readonly string[] SECTION_SETTINGS =
        {
            "MaxSlots",
            "TextColor",
            "SeamothStorageSlotsOffset",
            "SlotLayout"
        };

        private static readonly List<ConfigData> DEFAULT_CONFIG = new List<ConfigData>
        {
            new ConfigData("Settings", SECTION_SETTINGS[0], 12.ToString()),
            new ConfigData("Settings", SECTION_SETTINGS[1], COLORS.Green.ToString()),
            new ConfigData("Settings", SECTION_SETTINGS[2], 4.ToString()),
            new ConfigData("Settings", SECTION_SETTINGS[3], SlotLayout.Circle.ToString()),
            new ConfigData("Hotkeys", SECTION_HOTKEYS[0], InputHelper.GetKeyCodeAsInputName(KeyCode.T)),
            new ConfigData("Hotkeys", SECTION_HOTKEYS[1], InputHelper.GetKeyCodeAsInputName(KeyCode.R)),
            new ConfigData("Hotkeys", SECTION_HOTKEYS[2], InputHelper.GetKeyCodeAsInputName(KeyCode.Alpha6)),
            new ConfigData("Hotkeys", SECTION_HOTKEYS[3], InputHelper.GetKeyCodeAsInputName(KeyCode.Alpha7)),
            new ConfigData("Hotkeys", SECTION_HOTKEYS[4], InputHelper.GetKeyCodeAsInputName(KeyCode.Alpha8)),
            new ConfigData("Hotkeys", SECTION_HOTKEYS[5], InputHelper.GetKeyCodeAsInputName(KeyCode.Alpha9)),
            new ConfigData("Hotkeys", SECTION_HOTKEYS[6], InputHelper.GetKeyCodeAsInputName(KeyCode.Alpha0)),
            new ConfigData("Hotkeys", SECTION_HOTKEYS[7], InputHelper.GetKeyCodeAsInputName(KeyCode.Slash)),
            new ConfigData("Hotkeys", SECTION_HOTKEYS[8], InputHelper.GetKeyCodeAsInputName(KeyCode.Equals)),
            new ConfigData("Hotkeys", SECTION_HOTKEYS[9], InputHelper.GetKeyCodeAsInputName(KeyCode.O)),
            new ConfigData("Hotkeys", SECTION_HOTKEYS[10], InputHelper.GetKeyCodeAsInputName(KeyCode.P))
        };

        internal static void SLOTKEYBINDINGS_Update()
        {
            SNLogger.Debug("SlotExtender", "Method call: SEConfig.Update_SLOTKEYBINDINGS()");

            SLOTKEYBINDINGS.Clear();            

            SLOTKEYBINDINGS.Add(SlotConfigID.Slot_1, GameInput.GetBindingName(GameInput.Button.Slot1, GameInput.BindingSet.Primary));
            SLOTKEYBINDINGS.Add(SlotConfigID.Slot_2, GameInput.GetBindingName(GameInput.Button.Slot2, GameInput.BindingSet.Primary));
            SLOTKEYBINDINGS.Add(SlotConfigID.Slot_3, GameInput.GetBindingName(GameInput.Button.Slot3, GameInput.BindingSet.Primary));
            SLOTKEYBINDINGS.Add(SlotConfigID.Slot_4, GameInput.GetBindingName(GameInput.Button.Slot4, GameInput.BindingSet.Primary));
            SLOTKEYBINDINGS.Add(SlotConfigID.Slot_5, GameInput.GetBindingName(GameInput.Button.Slot5, GameInput.BindingSet.Primary));
            SLOTKEYBINDINGS.Add(SlotConfigID.Slot_6, Hotkeys_Config[SlotConfigID.Slot_6.ToString()]);
            SLOTKEYBINDINGS.Add(SlotConfigID.Slot_7, Hotkeys_Config[SlotConfigID.Slot_7.ToString()]);
            SLOTKEYBINDINGS.Add(SlotConfigID.Slot_8, Hotkeys_Config[SlotConfigID.Slot_8.ToString()]);
            SLOTKEYBINDINGS.Add(SlotConfigID.Slot_9, Hotkeys_Config[SlotConfigID.Slot_9.ToString()]);
            SLOTKEYBINDINGS.Add(SlotConfigID.Slot_10, Hotkeys_Config[SlotConfigID.Slot_10.ToString()]);
            SLOTKEYBINDINGS.Add(SlotConfigID.Slot_11, Hotkeys_Config[SlotConfigID.Slot_11.ToString()]);
            SLOTKEYBINDINGS.Add(SlotConfigID.Slot_12, Hotkeys_Config[SlotConfigID.Slot_12.ToString()]);
            SLOTKEYBINDINGS.Add(SlotConfigID.SeamothArmLeft, Hotkeys_Config[SlotConfigID.SeamothArmLeft.ToString()]);
            SLOTKEYBINDINGS.Add(SlotConfigID.SeamothArmRight, Hotkeys_Config[SlotConfigID.SeamothArmRight.ToString()]);
        }

        internal static void Config_Load()
        {
            SNLogger.Debug("SlotExtender", "Method call: SEConfig.LoadConfig()");

            PROGRAM_VERSION = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

            if (!Config_Check())
            {
                Config_CreateDefault();
            }

            try
            {
                Hotkeys_Config = ParserHelper.GetAllKeyValuesFromSection(FILENAME, "Hotkeys", SECTION_HOTKEYS);

                int.TryParse(ParserHelper.GetKeyValue(FILENAME, "Settings", SECTION_SETTINGS[0]), out int result);
                MAXSLOTS = result < 5 || result > 12 ? 12 : result;

                EXTRASLOTS = SEConfig.MAXSLOTS - 4;

                TEXTCOLOR = Modules.GetColor(ParserHelper.GetKeyValue(FILENAME, "Settings", SECTION_SETTINGS[1]));

                if (RefHelp.IsNamespaceExists("SeamothStorageSlots"))
                {
                    STORAGE_SLOTS_OFFSET = 0; // don't patch storages stuff if SeamothStorageSlots mod is active
                    const string msg = "<i>SeamothStorageSlots</i> mod is now merged into SlotExtender, you can safely delete it.";

                    Type qmmServices = Type.GetType("QModManager.API.QModServices, QModInstaller", false);
                    MethodInfo qmmMain = qmmServices?.GetProperty("Main")?.GetGetMethod();
                    qmmServices?.GetMethod("AddCriticalMessage")?.Invoke(qmmMain.Invoke(null, null), new object[] { msg, 25, "yellow", true });
                }
                else
                {
                    int.TryParse(ParserHelper.GetKeyValue(FILENAME, "Settings", SECTION_SETTINGS[2]), out result);
                    STORAGE_SLOTS_OFFSET = result < 3 ? 0 : result > 8 ? 8 : result;
                }

                SLOT_LAYOUT = ParserHelper.GetKeyValue(FILENAME, "Settings", SECTION_SETTINGS[3]) == "Circle"? SlotLayout.Circle : SlotLayout.Grid;

                isSeamothArmsExists = RefHelp.IsNamespaceExists("SeamothArms");

                SNLogger.Log("SlotExtender", "Configuration loaded.");
            }
            catch
            {
                SNLogger.Error("SlotExtender", "An error occurred while loading the configuration file!");
            }
        }

        internal static void Config_CreateDefault()
        {
            SNLogger.Debug("SlotExtender", "Method call: SEConfig.CreateDefaultConfigFile()");

            SNLogger.Warn("SlotExtender", "Configuration file is missing or wrong version. Trying to create a new one.");

            try
            {
                ParserHelper.CreateDefaultConfigFile(FILENAME, "SlotExtender", PROGRAM_VERSION, DEFAULT_CONFIG);

                ParserHelper.AddInfoText(FILENAME, "MaxSlots possible values", "5 to 12");
                ParserHelper.AddInfoText(FILENAME, "TextColor possible values", "Red, Green, Blue, Yellow, White, Magenta, Cyan, Orange, Lime, Amethyst, Default");
                ParserHelper.AddInfoText(FILENAME, "SeamothStorageSlotsOffset possible values", "0 to 8");
                ParserHelper.AddInfoText(FILENAME, "SlotLayout possible values", "Grid, Circle");
                

                SNLogger.Log("SlotExtender", "The new configuration file was successfully created.");
            }
            catch
            {
                SNLogger.Error("SlotExtender", "An error occured while creating the new configuration file!");
            }
        }

        internal static void Config_Init()
        {
            SNLogger.Debug("SlotExtender", "Method call: SEConfig.InitConfig()");

            SLOTKEYBINDINGS_Update();

            KEYBINDINGS_Set();

            SLOTKEYBINDINGS_SyncToAll();

            SNLogger.Log("SlotExtender", "Configuration initialized.");
        }

        internal static void Config_Write()
        {
            SNLogger.Debug("SlotExtender", "Method call: SEConfig.WriteConfig()");

            ParserHelper.SetAllKeyValuesInSection(FILENAME, "Hotkeys", Hotkeys_Config);
            ParserHelper.SetKeyValue(FILENAME, "Settings", SECTION_SETTINGS[0], MAXSLOTS.ToString());
            ParserHelper.SetKeyValue(FILENAME, "Settings", SECTION_SETTINGS[1], Modules.GetColorName(TEXTCOLOR));
            ParserHelper.SetKeyValue(FILENAME, "Settings", SECTION_SETTINGS[2], STORAGE_SLOTS_OFFSET.ToString());
            ParserHelper.SetKeyValue(FILENAME, "Settings", SECTION_SETTINGS[3], SLOT_LAYOUT.ToString());

            SNLogger.Log("SlotExtender", "Configuration saved.");
        }

        internal static void KEYBINDINGS_ToConfig()
        {
            SNLogger.Debug("SlotExtender", "Method call: SEConfig.SyncConfig()");

            foreach (string key in SECTION_HOTKEYS)
            {
                Hotkeys_Config[key] = InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[key]);
            }

            Config_Write();
        }

        internal static void SLOTKEYBINDINGS_SyncToAll()
        {
            SNLogger.Debug("SlotExtender", "Method call: SEConfig.SyncSlotKeyBindings()");            

            foreach (KeyValuePair<SlotConfigID, string> kvp in SLOTKEYBINDINGS)
            {
                SNLogger.Debug("SlotExtender", $"key: {kvp.Key.ToString()}, Value: {kvp.Value}");

                string key = kvp.Key.ToString();
                
                if (Hotkeys_Config.ContainsKey(key))
                    Hotkeys_Config[key] = kvp.Value;

                KEYBINDINGS[key] = InputHelper.GetInputNameAsKeyCode(kvp.Value);
            }

            Config_Write();
        }


        internal static void KEYBINDINGS_Set()
        {
            SNLogger.Debug("SlotExtender", "Method call: SEConfig.SetKeyBindings()");

            KEYBINDINGS = new Dictionary<string, KeyCode>();

            bool sync = false;

            foreach (KeyValuePair<string, string> kvp in Hotkeys_Config)
            {
                try
                {                    
                    KEYBINDINGS.Add(kvp.Key, InputHelper.GetInputNameAsKeyCode(kvp.Value));
                }
                catch (ArgumentException)
                {
                    SNLogger.Warn("SlotExtender", $"[{kvp.Value}] is not a valid KeyCode! Setting default value!");

                    for (int i = 0; i < DEFAULT_CONFIG.Count; i++)
                    {
                        if (DEFAULT_CONFIG[i].Key.Equals(kvp.Key))
                        {
                            KEYBINDINGS.Add(kvp.Key, InputHelper.GetInputNameAsKeyCode(DEFAULT_CONFIG[i].Value));
                            sync = true;
                        }
                    }
                }
            }

            if (sync)
            {
                KEYBINDINGS_ToConfig();
            }
        }

        private static bool Config_Check()
        {
            SNLogger.Debug("SlotExtender", "Method call: SEConfig.CheckConfig()");

            if (!File.Exists(FILENAME))
            {
                SNLogger.Error("SlotExtender", "Configuration file open error!");
                return false;
            }

            CONFIG_VERSION = ParserHelper.GetKeyValue(FILENAME, "SlotExtender", "Version");

            if (!CONFIG_VERSION.Equals(PROGRAM_VERSION))
            {
                SNLogger.Error("SlotExtender", "Configuration file version error!");
                return false;
            }

            if (!ParserHelper.CheckSectionKeys(FILENAME, "Hotkeys", SECTION_HOTKEYS))
            {
                SNLogger.Error("SlotExtender", "Configuration file [Hotkeys] section error!");
                return false;
            }

            if (!ParserHelper.CheckSectionKeys(FILENAME, "Settings", SECTION_SETTINGS))
            {
                SNLogger.Error("SlotExtender", "Configuration file [Settings] section error!");
                return false;
            }

            return true;
        }
    }
}
