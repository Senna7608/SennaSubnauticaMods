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
        public const string PROGRAM_NAME = "SlotExtender";
        internal static string PROGRAM_VERSION = string.Empty;
        internal static string CONFIG_VERSION = string.Empty;

        internal static Dictionary<string, KeyCode> KEYBINDINGS;        
        private static readonly string[] SECTIONS = { "Hotkeys", "Settings" };
        private static readonly string FILENAME = $"{Environment.CurrentDirectory}/QMods/{PROGRAM_NAME}/config.txt";
        internal static Dictionary<string, string> Section_hotkeys;
        public static Dictionary<string, string> SLOTKEYS = new Dictionary<string, string>();
        public static List<string> SLOTKEYSLIST = new List<string>();
        internal static int MAXSLOTS;
        public static Color TEXTCOLOR;

        private static readonly string[] SECTION_HOTKEYS =
        {
            "Upgrade",
            "Storage",
            "Slot_6",
            "Slot_7",
            "Slot_8",
            "Slot_9",
            "Slot_10",
            "Slot_11",
            "Slot_12"
        };


        private static readonly string[] SECTION_SETTINGS =
        {
            "MaxSlots",
            "TextColor"
        };

        private static readonly List<ConfigData> DEFAULT_CONFIG = new List<ConfigData>
        {
            new ConfigData(SECTIONS[1], SECTION_SETTINGS[0], 12.ToString()),
            new ConfigData(SECTIONS[1], SECTION_SETTINGS[1], COLORS.Green.ToString()),
            new ConfigData(SECTIONS[0], SECTION_HOTKEYS[0], InputHelper.GetKeyCodeAsInputName(KeyCode.T)),
            new ConfigData(SECTIONS[0], SECTION_HOTKEYS[1], InputHelper.GetKeyCodeAsInputName(KeyCode.R)),
            new ConfigData(SECTIONS[0], SECTION_HOTKEYS[2], InputHelper.GetKeyCodeAsInputName(KeyCode.Alpha6)),
            new ConfigData(SECTIONS[0], SECTION_HOTKEYS[3], InputHelper.GetKeyCodeAsInputName(KeyCode.Alpha7)),
            new ConfigData(SECTIONS[0], SECTION_HOTKEYS[4], InputHelper.GetKeyCodeAsInputName(KeyCode.Alpha8)),
            new ConfigData(SECTIONS[0], SECTION_HOTKEYS[5], InputHelper.GetKeyCodeAsInputName(KeyCode.Alpha9)),
            new ConfigData(SECTIONS[0], SECTION_HOTKEYS[6], InputHelper.GetKeyCodeAsInputName(KeyCode.Alpha0)),
            new ConfigData(SECTIONS[0], SECTION_HOTKEYS[7], InputHelper.GetKeyCodeAsInputName(KeyCode.Slash)),
            new ConfigData(SECTIONS[0], SECTION_HOTKEYS[8], InputHelper.GetKeyCodeAsInputName(KeyCode.Equals))
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
            SLOTKEYS.Add("Slot6", InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[SECTION_HOTKEYS[2]]));
            SLOTKEYS.Add("Slot7", InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[SECTION_HOTKEYS[3]]));
            SLOTKEYS.Add("Slot8", InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[SECTION_HOTKEYS[4]]));
            SLOTKEYS.Add("Slot9", InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[SECTION_HOTKEYS[5]]));
            SLOTKEYS.Add("Slot10", InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[SECTION_HOTKEYS[6]]));
            SLOTKEYS.Add("Slot11", InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[SECTION_HOTKEYS[7]]));
            SLOTKEYS.Add("Slot12", InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[SECTION_HOTKEYS[8]]));

            foreach (KeyValuePair<string, string> kvp in SLOTKEYS)
            {
                SLOTKEYSLIST.Add(kvp.Value);
            }
        }

        internal static void LoadConfig()
        {
            PROGRAM_VERSION = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

            if (!File.Exists(FILENAME))
            {
                CreateDefaultConfigFile();
            }
            else
            {
                CONFIG_VERSION = Helper.GetKeyValue(FILENAME, PROGRAM_NAME, "Version");

                if (CONFIG_VERSION.Equals(PROGRAM_VERSION))
                {
                    SNLogger.Log($"[{PROGRAM_NAME}] Configuration file version match with program version.");
                }
                else
                {
                    CreateDefaultConfigFile();
                }
            }

            Section_hotkeys = Helper.GetAllKeyValuesFromSection(FILENAME, SECTIONS[0], SECTION_HOTKEYS);

            int.TryParse(Helper.GetKeyValue(FILENAME, SECTIONS[1], SECTION_SETTINGS[0]), out int result);
            MAXSLOTS = result < 5 || result > 12 ? 12 : result;

            TEXTCOLOR = Modules.GetColor(Helper.GetKeyValue(FILENAME, SECTIONS[1], SECTION_SETTINGS[1]));

            SNLogger.Log($"[{PROGRAM_NAME}] Configuration loaded.");
        }

        internal static void CreateDefaultConfigFile()
        {
            SNLogger.Log($"[{PROGRAM_NAME}] Warning! Configuration file is missing or wrong version. Creating a new one.");

            try
            {
                Helper.CreateDefaultConfigFile(FILENAME, PROGRAM_NAME, PROGRAM_VERSION, DEFAULT_CONFIG);
                Helper.AddInfoText(FILENAME, SECTIONS[1], "TextColor possible values: Red, Green, Blue, Yellow, White, Magenta, Cyan, Orange, Lime, Amethyst, Default");
            }
            catch
            {
                SNLogger.Log($"[{PROGRAM_NAME}] Error! Creating new configuration file has failed!");
            }
        }

        internal static void InitConfig()
        {
            SetKeyBindings();            

            SNLogger.Log($"[{PROGRAM_NAME}] Configuration initialized.");
        }

        internal static void WriteConfig()
        {
            Helper.SetAllKeyValuesInSection(FILENAME, SECTIONS[0], Section_hotkeys);
            Helper.SetKeyValue(FILENAME, SECTIONS[1], SECTION_SETTINGS[0], MAXSLOTS.ToString());
            Helper.SetKeyValue(FILENAME, SECTIONS[1], SECTION_SETTINGS[1], Modules.GetColorName(TEXTCOLOR));

            SNLogger.Log($"[{PROGRAM_NAME}] Configuration saved.");            
        }

        internal static void SyncConfig()
        {
            foreach (string key in SECTION_HOTKEYS)
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
                    SNLogger.Log($"[{PROGRAM_NAME}] Warning! ({kvp.Value}) is not a valid KeyCode! Setting default value!");

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
    }
}
