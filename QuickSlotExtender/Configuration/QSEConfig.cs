using System;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Common.ConfigurationParser;
using Common.Helpers;

namespace QuickSlotExtender.Configuration
{
    internal static class QSEConfig
    {
        internal static string PROGRAM_VERSION = string.Empty;
        internal static string CONFIG_VERSION = string.Empty;

        internal static Dictionary<string, KeyCode> KEYBINDINGS;

        private static readonly string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string FILENAME = $"{modFolder}/config.txt";

        internal static Dictionary<string, string> Section_hotkeys;
        public static Dictionary<string, string> SLOTKEYS = new Dictionary<string, string>();
        public static List<string> SLOTKEYSLIST = new List<string>();
        public static int MAXSLOTS;
        public static Color TEXTCOLOR;

        private static readonly string[] SECTIONS =
        {
            "Hotkeys",
            "Settings"
        };

        private static readonly string[] SECTION_Hotkeys =
        {
            "Slot_6",
            "Slot_7",
            "Slot_8",
            "Slot_9",
            "Slot_10",
            "Slot_11",
            "Slot_12"
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
            new ConfigData(SECTIONS[0], SECTION_Hotkeys[0], InputHelper.GetKeyCodeAsInputName(KeyCode.Alpha6)),
            new ConfigData(SECTIONS[0], SECTION_Hotkeys[1], InputHelper.GetKeyCodeAsInputName(KeyCode.Alpha7)),
            new ConfigData(SECTIONS[0], SECTION_Hotkeys[2], InputHelper.GetKeyCodeAsInputName(KeyCode.Alpha8)),
            new ConfigData(SECTIONS[0], SECTION_Hotkeys[3], InputHelper.GetKeyCodeAsInputName(KeyCode.Alpha9)),
            new ConfigData(SECTIONS[0], SECTION_Hotkeys[4], InputHelper.GetKeyCodeAsInputName(KeyCode.Alpha0)),
            new ConfigData(SECTIONS[0], SECTION_Hotkeys[5], InputHelper.GetKeyCodeAsInputName(KeyCode.Slash)),
            new ConfigData(SECTIONS[0], SECTION_Hotkeys[6], InputHelper.GetKeyCodeAsInputName(KeyCode.Equals))
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
            SLOTKEYS.Add("Slot6", InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[SECTION_Hotkeys[0]]));
            SLOTKEYS.Add("Slot7", InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[SECTION_Hotkeys[1]]));
            SLOTKEYS.Add("Slot8", InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[SECTION_Hotkeys[2]]));
            SLOTKEYS.Add("Slot9", InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[SECTION_Hotkeys[3]]));
            SLOTKEYS.Add("Slot10", InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[SECTION_Hotkeys[4]]));
            SLOTKEYS.Add("Slot11", InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[SECTION_Hotkeys[5]]));
            SLOTKEYS.Add("Slot12", InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[SECTION_Hotkeys[6]]));


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
                Section_hotkeys = ParserHelper.GetAllKeyValuesFromSection(FILENAME, SECTIONS[0], SECTION_Hotkeys);

                int.TryParse(ParserHelper.GetKeyValue(FILENAME, SECTIONS[1], SECTION_Settings[0]), out int result);
                MAXSLOTS = result < 5 || result > 12 ? 12 : result;

                TEXTCOLOR = ColorHelper.GetColor(ParserHelper.GetKeyValue(FILENAME, SECTIONS[1], SECTION_Settings[1]));

                SNLogger.Log("Configuration loaded.");
            }
            catch
            {
                SNLogger.Log("An error occurred while loading the configuration file!");
            }
        }

        internal static void CreateDefaultConfigFile()
        {
            SNLogger.Warn("Configuration file is missing or wrong version. Trying to create a new one.");

            try
            {
                ParserHelper.CreateDefaultConfigFile(FILENAME, "QuickSlotExtender", PROGRAM_VERSION, DEFAULT_CONFIG);
                ParserHelper.AddInfoText(FILENAME, "TextColor possible values", "LightBlue, Red, Green, Blue, Yellow, White, Magenta, Cyan, Orange, Lime, Amethyst");

                SNLogger.Log("The new configuration file was successfully created.");
            }
            catch
            {
                SNLogger.Error("An error occured while creating the new configuration file!");
            }
        }

        internal static void InitConfig()
        {
            SetKeyBindings();
            InitSLOTKEYS();
            SNLogger.Log("Configuration initialized.");
        }

        internal static void WriteConfig()
        {
            ParserHelper.SetAllKeyValuesInSection(FILENAME, SECTIONS[0], Section_hotkeys);
            ParserHelper.SetKeyValue(FILENAME, SECTIONS[1], SECTION_Settings[0], MAXSLOTS.ToString());
            ParserHelper.SetKeyValue(FILENAME, SECTIONS[1], SECTION_Settings[1], ColorHelper.GetColorName(TEXTCOLOR));
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
                    SNLogger.Warn($"({kvp.Value}) is not a valid KeyCode! Setting default value!");

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
                SNLogger.Error("Configuration file open error!");
                return false;
            }

            CONFIG_VERSION = ParserHelper.GetKeyValue(FILENAME, "QuickSlotExtender", "Version");

            if (!CONFIG_VERSION.Equals(PROGRAM_VERSION))
            {
                SNLogger.Error("Configuration file version error!");
                return false;
            }

            if (!ParserHelper.CheckSectionKeys(FILENAME, SECTIONS[0], SECTION_Hotkeys))
            {
                SNLogger.Error($"Configuration {SECTIONS[0]} section error!");
                return false;
            }

            if (!ParserHelper.CheckSectionKeys(FILENAME, SECTIONS[1], SECTION_Settings))
            {
                SNLogger.Error($"Configuration {SECTIONS[1]} section error!");
                return false;
            }

            return true;
        }


    }
}
