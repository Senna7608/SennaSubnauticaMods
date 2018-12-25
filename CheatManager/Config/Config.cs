using System;
using System.Collections.Generic;
using UnityEngine;
using ConfigurationParser;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace CheatManager.Config
{
    public static class Config
    {
        internal static string VERSION = string.Empty;        
        private const string PROGRAM_NAME = "CheatManager";
        private static readonly string FILENAME = $"{Environment.CurrentDirectory}\\QMods\\{PROGRAM_NAME}\\config.txt";

        private static readonly string[] SECTIONS = { "Hotkeys", "Settings" };
        internal static Dictionary<string, KeyCode> KEYBINDINGS;
        internal static Dictionary<string, string> Section_hotkeys;
        internal static Dictionary<string, string> Section_settings;

        private static readonly string[] SECTION_HOTKEYS =
        {
            "ToggleWindow",
            "ToggleMouse",
            "ToggleConsole"
        };

        private static readonly string[] SECTION_SETTINGS =
        {
            "OverPowerMultiplier"            
        };

        private static readonly List<ConfigData> DEFAULT_CONFIG = new List<ConfigData>
        {
            new ConfigData(SECTIONS[0], SECTION_HOTKEYS[0], KeyCode.F5.ToString()),
            new ConfigData(SECTIONS[0], SECTION_HOTKEYS[1], KeyCode.F4.ToString()),
            new ConfigData(SECTIONS[0], SECTION_HOTKEYS[2], KeyCode.Delete.ToString()),
            new ConfigData(SECTIONS[1], SECTION_SETTINGS[0], 2.ToString())
    };        

        internal static void InitConfig()
        {
            VERSION = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;            

            if (!File.Exists(FILENAME))
            {
                UnityEngine.Debug.Log($"[{PROGRAM_NAME}] Warning! Configuration file is missing. Creating a new one.");                
                
                Helper.CreateDefaultConfigFile(FILENAME, PROGRAM_NAME, VERSION, DEFAULT_CONFIG);
            }

            Section_hotkeys = Helper.GetAllKeyValuesFromSection(FILENAME, SECTIONS[0], SECTION_HOTKEYS);
            Section_settings = Helper.GetAllKeyValuesFromSection(FILENAME, SECTIONS[1], SECTION_SETTINGS);

            int.TryParse(Section_settings[SECTION_SETTINGS[0]], out int ovpMultiplier);
            
            if (ovpMultiplier > 0 && ovpMultiplier <= 10)
                CheatManager.OverPowerMultiplier = ovpMultiplier;
            else
            {
                CheatManager.OverPowerMultiplier = 2;
                Helper.SetKeyValue(FILENAME, SECTIONS[1], Section_settings[SECTION_SETTINGS[0]], 2.ToString());
            }

            SetKeyBindings();
            
        }
        
        internal static void WriteConfig()
        {
            Helper.SetAllKeyValuesInSection(FILENAME, SECTIONS[0], Section_hotkeys);
            Helper.SetAllKeyValuesInSection(FILENAME, SECTIONS[1], Section_settings);
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
                    UnityEngine.Debug.Log($"[{PROGRAM_NAME}] Warning! ({kvp.Value}) is not a valid KeyCode! Setting default value!");

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
