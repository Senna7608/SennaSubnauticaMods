using System;
using System.Collections.Generic;
using UnityEngine;
using ConfigurationParser;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace SlotExtender.Config
{
    public static class Config
    {
        internal static string VERSION = string.Empty;
        public static List<KeyCode> KEYBINDINGS;

        private const string PROGRAM_NAME = "SlotExtender";
        private static readonly string[] SECTIONS = { "Hotkeys" };

        private static readonly string FILENAME = Environment.CurrentDirectory + "\\QMods\\SlotExtender\\config.txt";

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
            new ConfigData(SECTIONS[0], SECTION_HOTKEYS[0], KeyCode.R.ToString()),            
            new ConfigData(SECTIONS[0], SECTION_HOTKEYS[1], KeyCode.Alpha6.ToString()),
            new ConfigData(SECTIONS[0], SECTION_HOTKEYS[2], KeyCode.Alpha7.ToString()),
            new ConfigData(SECTIONS[0], SECTION_HOTKEYS[3], KeyCode.Alpha8.ToString()),
            new ConfigData(SECTIONS[0], SECTION_HOTKEYS[4], KeyCode.Alpha9.ToString()),
            new ConfigData(SECTIONS[0], SECTION_HOTKEYS[5], KeyCode.I.ToString()),
            new ConfigData(SECTIONS[0], SECTION_HOTKEYS[6], KeyCode.O.ToString()),
            new ConfigData(SECTIONS[0], SECTION_HOTKEYS[7], KeyCode.P.ToString())
        };

        internal static Dictionary<string, string> HotKeys { get; set; } = new Dictionary<string, string>();

        internal static void InitConfig()
        {
            if (!File.Exists(FILENAME))
            {
                UnityEngine.Debug.Log($"[{PROGRAM_NAME}] Warning! Configuration file is missing. Creating a new one.");

                VERSION = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
                
                Helper.CreateDefaultConfigFile(FILENAME, PROGRAM_NAME, VERSION, DEFAULT_CONFIG);
            }            

            HotKeys = Helper.GetAllKeyValuesFromSection(FILENAME, SECTIONS[0], SECTION_HOTKEYS);

            SetKeyBindings();
        }
        
        internal static void WriteConfig()
        {
            Helper.SetAllKeyValuesInSection(FILENAME, SECTIONS[0], HotKeys);
        }

        internal static void SyncConfig()
        {
            int i = 0;

            foreach (string key in SECTION_HOTKEYS)
            {
                HotKeys[key] = KEYBINDINGS[i].ToString();
                i++;
            }

            WriteConfig();
        }

        internal static void SetKeyBindings()
        {
            KEYBINDINGS = new List<KeyCode>();

            bool sync = false;

            foreach (KeyValuePair<string, string> kvp in HotKeys)
            {
                try
                {
                    KeyCode keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), kvp.Value);
                    KEYBINDINGS.Add(keyCode);
                }
                catch (ArgumentException)
                {
                    UnityEngine.Debug.Log($"[{PROGRAM_NAME}] Warning! ({kvp.Value}) is not a valid KeyCode! Setting default value!");

                    for (int i = 0; i < DEFAULT_CONFIG.Count; i++)
                    {
                        if (DEFAULT_CONFIG[i].Key.Equals(kvp.Key))
                        {
                            KEYBINDINGS.Add((KeyCode)Enum.Parse(typeof(KeyCode), DEFAULT_CONFIG[i].Value, true));
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
