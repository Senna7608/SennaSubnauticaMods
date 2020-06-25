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
    internal static class SEConfig
    {        
        internal static string PROGRAM_VERSION = string.Empty;
        internal static string CONFIG_VERSION = string.Empty;

        internal static Dictionary<string, KeyCode> KEYBINDINGS;
        private static readonly string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string FILENAME = $"{modFolder}/config.txt";
        internal static Dictionary<string, string> Section_hotkeys;
        public static Dictionary<string, string> SLOTKEYS = new Dictionary<string, string>();
        public static List<string> SLOTKEYSLIST = new List<string>();
        internal static int MAXSLOTS;
        public static Color TEXTCOLOR;

        private static readonly string[] SECTIONS =
        {
            "Hotkeys",
            "Settings"
        };

        private static readonly string[] SECTION_Hotkeys =
        {
            "Upgrade",
            "Storage",
            "Slot_6",
            "Slot_7",
            "Slot_8",
            "Slot_9",
            "Slot_10",
            "Slot_11",
            "Slot_12",
            "SeamothArmLeft",
            "SeamothArmRight"
        };


        private static readonly string[] SECTION_Settings =
        {
            "MaxSlots",
            "TextColor"
        };

        private static readonly List<ConfigData> DEFAULT_CONFIG = new List<ConfigData>
        {
            new ConfigData(SECTIONS[1], SECTION_Settings[0], 12.ToString()),
            new ConfigData(SECTIONS[1], SECTION_Settings[1], COLORS.Green.ToString()),
            new ConfigData(SECTIONS[0], SECTION_Hotkeys[0], InputHelper.GetKeyCodeAsInputName(KeyCode.T)),
            new ConfigData(SECTIONS[0], SECTION_Hotkeys[1], InputHelper.GetKeyCodeAsInputName(KeyCode.R)),
            new ConfigData(SECTIONS[0], SECTION_Hotkeys[2], InputHelper.GetKeyCodeAsInputName(KeyCode.Alpha6)),
            new ConfigData(SECTIONS[0], SECTION_Hotkeys[3], InputHelper.GetKeyCodeAsInputName(KeyCode.Alpha7)),
            new ConfigData(SECTIONS[0], SECTION_Hotkeys[4], InputHelper.GetKeyCodeAsInputName(KeyCode.Alpha8)),
            new ConfigData(SECTIONS[0], SECTION_Hotkeys[5], InputHelper.GetKeyCodeAsInputName(KeyCode.Alpha9)),
            new ConfigData(SECTIONS[0], SECTION_Hotkeys[6], InputHelper.GetKeyCodeAsInputName(KeyCode.Alpha0)),
            new ConfigData(SECTIONS[0], SECTION_Hotkeys[7], InputHelper.GetKeyCodeAsInputName(KeyCode.Slash)),
            new ConfigData(SECTIONS[0], SECTION_Hotkeys[8], InputHelper.GetKeyCodeAsInputName(KeyCode.Equals)),
            new ConfigData(SECTIONS[0], SECTION_Hotkeys[9], InputHelper.GetKeyCodeAsInputName(KeyCode.O)),
            new ConfigData(SECTIONS[0], SECTION_Hotkeys[10], InputHelper.GetKeyCodeAsInputName(KeyCode.P))
        };

        internal static void InitSLOTKEYS()
        {

            SLOTKEYS.Clear();
            SLOTKEYSLIST.Clear();

            SLOTKEYS.Add("Slot1", GameInput.GetBindingName(GameInput.Button.Slot1, GameInput.BindingSet.Primary));
            SLOTKEYS.Add("Slot2", GameInput.GetBindingName(GameInput.Button.Slot2, GameInput.BindingSet.Primary));
            SLOTKEYS.Add("Slot3", GameInput.GetBindingName(GameInput.Button.Slot3, GameInput.BindingSet.Primary));
            SLOTKEYS.Add("Slot4", GameInput.GetBindingName(GameInput.Button.Slot4, GameInput.BindingSet.Primary));
            SLOTKEYS.Add("Slot5", GameInput.GetBindingName(GameInput.Button.Slot5, GameInput.BindingSet.Primary));
            SLOTKEYS.Add("Slot6", InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[SECTION_Hotkeys[2]]));
            SLOTKEYS.Add("Slot7", InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[SECTION_Hotkeys[3]]));
            SLOTKEYS.Add("Slot8", InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[SECTION_Hotkeys[4]]));
            SLOTKEYS.Add("Slot9", InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[SECTION_Hotkeys[5]]));
            SLOTKEYS.Add("Slot10", InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[SECTION_Hotkeys[6]]));
            SLOTKEYS.Add("Slot11", InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[SECTION_Hotkeys[7]]));
            SLOTKEYS.Add("Slot12", InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[SECTION_Hotkeys[8]]));
            SLOTKEYS.Add("SeamothArmLeft", InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[SECTION_Hotkeys[9]]));
            SLOTKEYS.Add("SeamothArmRight", InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[SECTION_Hotkeys[10]]));

            foreach (KeyValuePair<string, string> kvp in SLOTKEYS)
            {
                SLOTKEYSLIST.Add(kvp.Value);
            }
        }

        internal static void LoadConfig()
        {
            PROGRAM_VERSION = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

            if (!CheckConfig())
            {
                CreateDefaultConfigFile();
            }            

            try
            {
                Section_hotkeys = Helper.GetAllKeyValuesFromSection(FILENAME, SECTIONS[0], SECTION_Hotkeys);                

                int.TryParse(Helper.GetKeyValue(FILENAME, SECTIONS[1], SECTION_Settings[0]), out int result);
                MAXSLOTS = result < 5 || result > 12 ? 12 : result;

                TEXTCOLOR = Modules.GetColor(Helper.GetKeyValue(FILENAME, SECTIONS[1], SECTION_Settings[1]));

                SNLogger.Log("SlotExtender", "Configuration loaded.");
            }
            catch
            {
                SNLogger.Error("SlotExtender", "An error occurred while loading the configuration file!");
            }
        }

        internal static void CreateDefaultConfigFile()
        {
            SNLogger.Warn("SlotExtender", "Configuration file is missing or wrong version. Trying to create a new one.");

            try
            {
                Helper.CreateDefaultConfigFile(FILENAME, "SlotExtender", PROGRAM_VERSION, DEFAULT_CONFIG);
                Helper.AddInfoText(FILENAME, SECTIONS[1], "TextColor possible values: Red, Green, Blue, Yellow, White, Magenta, Cyan, Orange, Lime, Amethyst, Default");

                SNLogger.Log("SlotExtender", "The new configuration file was successfully created.");
            }
            catch
            {
                SNLogger.Error("SlotExtender", "An error occured while creating the new configuration file!");
            }
        }

        internal static void InitConfig()
        {
            SetKeyBindings();            

            SNLogger.Log("SlotExtender", "Configuration initialized.");
        }

        internal static void WriteConfig()
        {
            Helper.SetAllKeyValuesInSection(FILENAME, SECTIONS[0], Section_hotkeys);
            Helper.SetKeyValue(FILENAME, SECTIONS[1], SECTION_Settings[0], MAXSLOTS.ToString());
            Helper.SetKeyValue(FILENAME, SECTIONS[1], SECTION_Settings[1], Modules.GetColorName(TEXTCOLOR));

            SNLogger.Log("SlotExtender", "Configuration saved.");            
        }

        internal static void SyncConfig()
        {
            foreach (string key in SECTION_Hotkeys)
            {
                Section_hotkeys[key] = InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[key]);                
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
                SyncConfig();
            }
        }

        private static bool CheckConfig()
        {
            if (!File.Exists(FILENAME))
            {
                SNLogger.Error("SlotExtender", "Configuration file open error!");
                return false;
            }

            CONFIG_VERSION = Helper.GetKeyValue(FILENAME, "SlotExtender", "Version");            

            if (!CONFIG_VERSION.Equals(PROGRAM_VERSION))            
            {
                SNLogger.Error("SlotExtender", "Configuration file version error!");
                return false;
            }            

            if (!Helper.CheckSectionKeys(FILENAME, SECTIONS[0], SECTION_Hotkeys))            
            {
                SNLogger.Error("SlotExtender", $"Configuration {SECTIONS[0]} section error!");
                return false;
            }

            if (!Helper.CheckSectionKeys(FILENAME, SECTIONS[1], SECTION_Settings))            
            {
                SNLogger.Error("SlotExtender", $"Configuration {SECTIONS[1]} section error!");
                return false;
            }

            return true;
        }
    }
}
