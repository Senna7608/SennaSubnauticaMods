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
        private static readonly string FILENAME = Environment.CurrentDirectory + "\\QMods\\CheatManager\\config.txt";
        private const string PROGRAM_NAME = "CheatManager";
        private static readonly string[] SECTIONS = { "Hotkeys" };
        internal static List<KeyCode> KEYBINDINGS;

        internal static Dictionary<string, string> HotKeys { get; set; } = new Dictionary<string, string>();

        private static readonly string[] SECTION_HOTKEYS =
        {
            "ToggleWindow",
            "ToggleMouse",
            "ToggleConsole"
        };

        private static readonly List<ConfigData> DEFAULT_CONFIG = new List<ConfigData>
        {
            new ConfigData(SECTIONS[0], SECTION_HOTKEYS[0], KeyCode.F5.ToString()),
            new ConfigData(SECTIONS[0], SECTION_HOTKEYS[1], KeyCode.F4.ToString()),
            new ConfigData(SECTIONS[0], SECTION_HOTKEYS[2], KeyCode.Delete.ToString())
        };        

        internal static void InitConfig()
        {
            VERSION = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;            

            if (!File.Exists(FILENAME))
            {
                UnityEngine.Debug.Log($"[{PROGRAM_NAME}] Warning! Configuration file is missing. Creating a new one.");                
                
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
