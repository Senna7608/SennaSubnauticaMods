using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using UnityEngine;
using ConfigurationParser;
using Common;

namespace SlotExtender.Config
{
    public static class Config
    {
        public static string VERSION = string.Empty;
        public static Dictionary<string,KeyCode> KEYBINDINGS;
        private const string PROGRAM_NAME = "SlotExtender";
        private static readonly string[] SECTIONS = { "Hotkeys" };
        private static readonly string FILENAME = $"{Environment.CurrentDirectory}\\QMods\\SlotExtender\\config.txt";
        internal static Dictionary<string, string> Section_hotkeys;
        public static Dictionary<string, string> SLOTKEYS = new Dictionary<string, string>();

        private static readonly string[] SECTION_HOTKEYS =
        {
            "Show",            
            "Slot_6",
            "Slot_7",
            "Slot_8",
            "Slot_9",
            "Slot_10",
            "Slot_11",
            "Slot_12"
        };

        private static readonly List<ConfigData> DEFAULT_CONFIG = new List<ConfigData>
        {
            new ConfigData(SECTIONS[0], SECTION_HOTKEYS[0], InputHelper.GetKeyCodeAsInputName(KeyCode.R)),            
            new ConfigData(SECTIONS[0], SECTION_HOTKEYS[1], InputHelper.GetKeyCodeAsInputName(KeyCode.Alpha6)),
            new ConfigData(SECTIONS[0], SECTION_HOTKEYS[2], InputHelper.GetKeyCodeAsInputName(KeyCode.Alpha7)),
            new ConfigData(SECTIONS[0], SECTION_HOTKEYS[3], InputHelper.GetKeyCodeAsInputName(KeyCode.Alpha8)),
            new ConfigData(SECTIONS[0], SECTION_HOTKEYS[4], InputHelper.GetKeyCodeAsInputName(KeyCode.Alpha9)),
            new ConfigData(SECTIONS[0], SECTION_HOTKEYS[5], InputHelper.GetKeyCodeAsInputName(KeyCode.I)),
            new ConfigData(SECTIONS[0], SECTION_HOTKEYS[6], InputHelper.GetKeyCodeAsInputName(KeyCode.O)),
            new ConfigData(SECTIONS[0], SECTION_HOTKEYS[7], InputHelper.GetKeyCodeAsInputName(KeyCode.P))
        };        

        internal static void InitSLOTKEYS()
        {
            SLOTKEYS.Clear();

            SLOTKEYS.Add("Slot1", GameInput.GetBindingName(GameInput.Button.Slot1, GameInput.BindingSet.Primary));
            SLOTKEYS.Add("Slot2", GameInput.GetBindingName(GameInput.Button.Slot2, GameInput.BindingSet.Primary));
            SLOTKEYS.Add("Slot3", GameInput.GetBindingName(GameInput.Button.Slot3, GameInput.BindingSet.Primary));
            SLOTKEYS.Add("Slot4", GameInput.GetBindingName(GameInput.Button.Slot4, GameInput.BindingSet.Primary));
            SLOTKEYS.Add("Slot5", GameInput.GetBindingName(GameInput.Button.Slot5, GameInput.BindingSet.Primary));
            SLOTKEYS.Add("Slot6", InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[SECTION_HOTKEYS[1]]));
            SLOTKEYS.Add("Slot7", InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[SECTION_HOTKEYS[2]]));
            SLOTKEYS.Add("Slot8", InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[SECTION_HOTKEYS[3]]));
            SLOTKEYS.Add("Slot9", InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[SECTION_HOTKEYS[4]]));
            SLOTKEYS.Add("Slot10", InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[SECTION_HOTKEYS[5]]));
            SLOTKEYS.Add("Slot11", InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[SECTION_HOTKEYS[6]]));
            SLOTKEYS.Add("Slot12", InputHelper.GetKeyCodeAsInputName(KEYBINDINGS[SECTION_HOTKEYS[7]]));
        }
               
        internal static void InitConfig()
        {
            if (!File.Exists(FILENAME))
            {
                UnityEngine.Debug.Log($"[{PROGRAM_NAME}] Warning! Configuration file is missing. Creating a new one.");

                VERSION = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
                
                Helper.CreateDefaultConfigFile(FILENAME, PROGRAM_NAME, VERSION, DEFAULT_CONFIG);
            }            

            Section_hotkeys = Helper.GetAllKeyValuesFromSection(FILENAME, SECTIONS[0], SECTION_HOTKEYS);

            SetKeyBindings();            

            UnityEngine.Debug.Log($"[{PROGRAM_NAME}] Configuration loaded and initialized.");
        }
        
        internal static void WriteConfig()
        {
            if (Helper.SetAllKeyValuesInSection(FILENAME, SECTIONS[0], Section_hotkeys))
                UnityEngine.Debug.Log($"[{PROGRAM_NAME}] Configuration saved.");            
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
                    UnityEngine.Debug.Log($"[{PROGRAM_NAME}] Warning! ({kvp.Value}) is not a valid KeyCode! Setting default value!");

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
                SyncConfig();
        }
    }
}
