using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using UnityEngine;
using Common.ConfigurationParser;
using Common;
using Common.Helpers;
using BepInEx.Bootstrap;

namespace SlotExtender.Configuration
{
    public static class SEConfig
    {
        public static string PROGRAM_VERSION = string.Empty;
        public static string CONFIG_VERSION = string.Empty;
        
        private static readonly string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string FILENAME = $"{modFolder}/config.txt";

        public static Dictionary<string, string> Section_Hotkeys;
        public static Dictionary<string, string> Section_Settings;
        public static Dictionary<SlotConfigID, string> SLOTKEYBINDINGS = new Dictionary<SlotConfigID, string>();
        public static Dictionary<string, KeyCode> KEYBINDINGS;

        public static List<string> SLOTKEYSLIST = new List<string>();

        public static int MAXSLOTS;
        public static int EXTRASLOTS;
        public static Color TEXTCOLOR;
        public static int STORAGE_SLOTS_OFFSET = 4;
        public static SlotLayout SLOT_LAYOUT = SlotLayout.Grid;
        public static bool isSeamothArmsExists = false;

        private static readonly string[] SECTIONKEYS_HOTKEYS =
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
        
        private static readonly string[] SECTIONKEYS_SETTINGS =
        {
            "MaxSlots",
            "TextColor",
            "SeamothStorageSlotsOffset",
            "SlotLayout"
        };

        private static readonly List<ConfigData> DEFAULT_CONFIG = new List<ConfigData>
        {
            new ConfigData("Settings", SECTIONKEYS_SETTINGS[0], 12.ToString()),
            new ConfigData("Settings", SECTIONKEYS_SETTINGS[1], COLORS.Green.ToString()),
            new ConfigData("Settings", SECTIONKEYS_SETTINGS[2], 4.ToString()),
            new ConfigData("Settings", SECTIONKEYS_SETTINGS[3], SlotLayout.Circle.ToString()),
            new ConfigData("Hotkeys", SECTIONKEYS_HOTKEYS[0], InputHelper.GetKeyCodeAsInputName(KeyCode.T)),
            new ConfigData("Hotkeys", SECTIONKEYS_HOTKEYS[1], InputHelper.GetKeyCodeAsInputName(KeyCode.R)),
            new ConfigData("Hotkeys", SECTIONKEYS_HOTKEYS[2], InputHelper.GetKeyCodeAsInputName(KeyCode.Alpha6)),
            new ConfigData("Hotkeys", SECTIONKEYS_HOTKEYS[3], InputHelper.GetKeyCodeAsInputName(KeyCode.Alpha7)),
            new ConfigData("Hotkeys", SECTIONKEYS_HOTKEYS[4], InputHelper.GetKeyCodeAsInputName(KeyCode.Alpha8)),
            new ConfigData("Hotkeys", SECTIONKEYS_HOTKEYS[5], InputHelper.GetKeyCodeAsInputName(KeyCode.Alpha9)),
            new ConfigData("Hotkeys", SECTIONKEYS_HOTKEYS[6], InputHelper.GetKeyCodeAsInputName(KeyCode.Alpha0)),
            new ConfigData("Hotkeys", SECTIONKEYS_HOTKEYS[7], InputHelper.GetKeyCodeAsInputName(KeyCode.Slash)),
            new ConfigData("Hotkeys", SECTIONKEYS_HOTKEYS[8], InputHelper.GetKeyCodeAsInputName(KeyCode.Equals)),
            new ConfigData("Hotkeys", SECTIONKEYS_HOTKEYS[9], InputHelper.GetKeyCodeAsInputName(KeyCode.O)),
            new ConfigData("Hotkeys", SECTIONKEYS_HOTKEYS[10], InputHelper.GetKeyCodeAsInputName(KeyCode.P))
        };

        internal static void SLOTKEYBINDINGS_Update()
        {
            SLOTKEYBINDINGS.Clear();
            SLOTKEYSLIST.Clear();

            SLOTKEYBINDINGS.Add(SlotConfigID.Slot_1, GameInput.GetBindingName(GameInput.Button.Slot1, GameInput.BindingSet.Primary));
            SLOTKEYBINDINGS.Add(SlotConfigID.Slot_2, GameInput.GetBindingName(GameInput.Button.Slot2, GameInput.BindingSet.Primary));
            SLOTKEYBINDINGS.Add(SlotConfigID.Slot_3, GameInput.GetBindingName(GameInput.Button.Slot3, GameInput.BindingSet.Primary));
            SLOTKEYBINDINGS.Add(SlotConfigID.Slot_4, GameInput.GetBindingName(GameInput.Button.Slot4, GameInput.BindingSet.Primary));
            SLOTKEYBINDINGS.Add(SlotConfigID.Slot_5, GameInput.GetBindingName(GameInput.Button.Slot5, GameInput.BindingSet.Primary));
            SLOTKEYBINDINGS.Add(SlotConfigID.Slot_6, Section_Hotkeys[SlotConfigID.Slot_6.ToString()]);
            SLOTKEYBINDINGS.Add(SlotConfigID.Slot_7, Section_Hotkeys[SlotConfigID.Slot_7.ToString()]);
            SLOTKEYBINDINGS.Add(SlotConfigID.Slot_8, Section_Hotkeys[SlotConfigID.Slot_8.ToString()]);
            SLOTKEYBINDINGS.Add(SlotConfigID.Slot_9, Section_Hotkeys[SlotConfigID.Slot_9.ToString()]);
            SLOTKEYBINDINGS.Add(SlotConfigID.Slot_10, Section_Hotkeys[SlotConfigID.Slot_10.ToString()]);
            SLOTKEYBINDINGS.Add(SlotConfigID.Slot_11, Section_Hotkeys[SlotConfigID.Slot_11.ToString()]);
            SLOTKEYBINDINGS.Add(SlotConfigID.Slot_12, Section_Hotkeys[SlotConfigID.Slot_12.ToString()]);
            SLOTKEYBINDINGS.Add(SlotConfigID.SeamothArmLeft, Section_Hotkeys[SlotConfigID.SeamothArmLeft.ToString()]);
            SLOTKEYBINDINGS.Add(SlotConfigID.SeamothArmRight, Section_Hotkeys[SlotConfigID.SeamothArmRight.ToString()]);

            foreach (KeyValuePair<SlotConfigID, string> kvp in SLOTKEYBINDINGS)
            {
                SLOTKEYSLIST.Add(kvp.Value);
            }            
        }

        internal static void Load()
        {
            PROGRAM_VERSION = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

            if (!Check())
            {
                CreateDefault();
            }

            Section_Settings = ParserHelper.GetAllKeyValuesFromSection(FILENAME, "Settings", SECTIONKEYS_SETTINGS);                        
            Section_Hotkeys = ParserHelper.GetAllKeyValuesFromSection(FILENAME, "Hotkeys", SECTIONKEYS_HOTKEYS);

            int.TryParse(Section_Settings["MaxSlots"], out int maxslots);
            MAXSLOTS = maxslots < 5 || maxslots > 12 ? 12 : maxslots;
            EXTRASLOTS = SEConfig.MAXSLOTS - 4;

            TEXTCOLOR = ColorHelper.GetColor(Section_Settings["TextColor"]);

            int.TryParse(Section_Settings["SeamothStorageSlotsOffset"], out int slotOffset);
            STORAGE_SLOTS_OFFSET = slotOffset < 3 ? 0 : slotOffset > 8 ? 8 : slotOffset;              

            SLOT_LAYOUT = Section_Settings["SlotLayout"] == "Circle"? SlotLayout.Circle : SlotLayout.Grid;

            isSeamothArmsExists = true;

            SNLogger.Log("Configuration loaded.");            
        }

        internal static void CreateDefault()
        {
            SNLogger.Warn("Configuration file is missing or wrong version. Trying to create a new one.");
            
            ParserHelper.CreateDefaultConfigFile(FILENAME, "SlotExtender", PROGRAM_VERSION, DEFAULT_CONFIG);

            ParserHelper.AddInfoText(FILENAME, "MaxSlots possible values", "5 to 12");
            ParserHelper.AddInfoText(FILENAME, "TextColor possible values", "Red, Green, Blue, Yellow, White, Magenta, Cyan, Orange, Lime, Amethyst, LightBlue");
            ParserHelper.AddInfoText(FILENAME, "SeamothStorageSlotsOffset possible values", "0 to 8");
            ParserHelper.AddInfoText(FILENAME, "SlotLayout possible values", "Grid, Circle");                

            SNLogger.Log("The new configuration file was successfully created.");            
        }

        internal static void Init()
        {
            SLOTKEYBINDINGS_Update();

            KEYBINDINGS_Set();

            SLOTKEYBINDINGS_SyncToAll();

            SNLogger.Log("Configuration initialized.");
        }

        internal static void Save()
        {
            ParserHelper.SetAllKeyValuesInSection(FILENAME, "Hotkeys", Section_Hotkeys);
            ParserHelper.SetAllKeyValuesInSection(FILENAME, "Settings", Section_Settings);
            /*
            ParserHelper.SetKeyValue(FILENAME, "Settings", SECTIONKEYS_SETTINGS[0], MAXSLOTS.ToString());
            ParserHelper.SetKeyValue(FILENAME, "Settings", SECTIONKEYS_SETTINGS[1], ColorHelper.GetColorName(TEXTCOLOR));
            ParserHelper.SetKeyValue(FILENAME, "Settings", SECTIONKEYS_SETTINGS[2], STORAGE_SLOTS_OFFSET.ToString());
            ParserHelper.SetKeyValue(FILENAME, "Settings", SECTIONKEYS_SETTINGS[3], SLOT_LAYOUT.ToString());
            */
            SNLogger.Log("Configuration saved.");
        }

        internal static void KEYBINDINGS_ToConfig()
        {
            foreach (string key in SECTIONKEYS_HOTKEYS)
            {
                Section_Hotkeys[key] = InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[key]);
            }

            Save();
        }

        internal static void SLOTKEYBINDINGS_SyncToAll()
        {
            foreach (KeyValuePair<SlotConfigID, string> kvp in SLOTKEYBINDINGS)
            {
                SNLogger.Debug($"key: {kvp.Key.ToString()}, Value: {kvp.Value}");

                string key = kvp.Key.ToString();
                
                if (Section_Hotkeys.ContainsKey(key))
                    Section_Hotkeys[key] = kvp.Value;

                KEYBINDINGS[key] = InputHelper.GetInputNameAsKeyCode(kvp.Value);
            }

            Save();
        }


        internal static void KEYBINDINGS_Set()
        {
            KEYBINDINGS = new Dictionary<string, KeyCode>();

            bool sync = false;

            foreach (KeyValuePair<string, string> kvp in Section_Hotkeys)
            {
                try
                {                    
                    KEYBINDINGS.Add(kvp.Key, InputHelper.GetInputNameAsKeyCode(kvp.Value));
                }
                catch (ArgumentException)
                {
                    SNLogger.Warn($"[{kvp.Value}] is not a valid KeyCode! Setting default value!");

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

        private static bool Check()
        {
            if (!File.Exists(FILENAME))
            {
                SNLogger.Error("Configuration file open error!");
                return false;
            }

            CONFIG_VERSION = ParserHelper.GetKeyValue(FILENAME, "SlotExtender", "Version");

            if (!CONFIG_VERSION.Equals(PROGRAM_VERSION))
            {
                SNLogger.Error("Configuration file version error!");
                return false;
            }

            if (!ParserHelper.CheckSectionKeys(FILENAME, "Hotkeys", SECTIONKEYS_HOTKEYS))
            {
                SNLogger.Error("Configuration file [Hotkeys] section error!");
                return false;
            }

            if (!ParserHelper.CheckSectionKeys(FILENAME, "Settings", SECTIONKEYS_SETTINGS))
            {
                SNLogger.Error("Configuration file [Settings] section error!");
                return false;
            }

            return true;
        }
    }
}
