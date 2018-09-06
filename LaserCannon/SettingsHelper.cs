using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace LaserCannon
{
    internal static class SettingsHelper
    {
        private const string configFile = "./QMods/LaserCannon/LaserCannonConfig.txt";
        private const string configKey_main = "----LaserCannonConfig----";
        private const string config_splitter = " = ";
        private const string lang_marker = "<=>";
        private const string configKey_language = "Language";

        internal static string title = string.Empty;
        internal static string toggle = string.Empty;
        internal static string color = string.Empty;
        internal static string description = string.Empty;
        internal static string friendlyName = string.Empty;

        internal const string toggleID = "onlyHostile";
        internal const string choiceID = "laserBeamColor";
        internal static string mainLang = string.Empty;

        private const string title_English = "Laser Cannon Module Options";
        private const string toggle_English = "Damage hostile target only";
        private const string color_English = "Laser Beam Color";
        private const string description_English = "Recovered laser beam technology from an ancient Precursor weapon fragment.";
        private const string friendlyName_English = "Laser Cannon";

        private static readonly string[] Languages =
        {   "Bulgarian",
            "Chinese (Simplified)",
            "Croatian",
            "Czech",
            "Danish",
            "Dutch",
            "English",
            "Finnish",            
            "French",
            "German",
            "Greek",
            "Hebrew",
            "Hungarian",
            "Irish",
            "Italian",
            "Japanese",
            "Korean",
            "Latvian",
            "Lithuanian",
            "Norwegian",
            "Polish",
            "Portuguese (Brazil)",
            "Portuguese",
            "Romanian",
            "Russian",
            "Serbian",
            "Slovak",
            "Spanish",
            "Swedish",
            "Thai",
            "Turkish",
            "Ukrainian",
            "Vietnamese"
        };

        private static readonly string[] LanguageItems = { "   title_", "   toggle_", "   color_", "   description_", "   friendlyName_" };
        internal static readonly string[] beamColors = Modules.Colors.ColorNames;

        private static Dictionary<string, string> LangText = new Dictionary<string, string>();        

        internal static int LaserBeamColor { get; set; }
        internal static bool OnlyHostile { get; set; }

        internal static void Init()
        {
            try
            {
                LoadFromFile();                
            }
            catch (Exception ex)
            {
                Debug.Log($"[LaserCannon] Error loading {configKey_main}: " + ex.ToString());                
            }

            SetLang(mainLang);
        }

        internal static void OnLanguageChanged()
        {
            mainLang = Language.main.GetCurrentLanguage();            
            
            int i = Array.IndexOf(Languages, mainLang);

            if (i >= 0 && i < Languages.Length)
            {
                bool succes = SetLang(Languages[i]);

                if (!succes)
                {
                    mainLang = Languages[6];
                    SetLang(mainLang);
                }
            }
            else
            {
                mainLang = Languages[6];
                SetLang(mainLang);
            }

            WriteConfigFile(true);
        }

        internal static void WriteConfigFile(bool isRewrite)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(configKey_main);
            sb.AppendLine(toggleID + config_splitter + OnlyHostile.ToString());
            sb.AppendLine(choiceID + config_splitter + beamColors[LaserBeamColor]);
            sb.AppendLine(configKey_language + config_splitter + mainLang);

            foreach (KeyValuePair<string, string> item in LangText)
            {
                if (item.Value == lang_marker)
                {
                    sb.AppendLine(item.Key + item.Value);
                }
                else
                {
                    if (isRewrite)
                        sb.AppendLine("   " + item.Key + config_splitter + item.Value);
                    else
                        sb.AppendLine(item.Key + config_splitter + item.Value);
                }
            }

            File.WriteAllText(configFile, sb.ToString(), Encoding.UTF8);
        }


        internal static void LoadFromFile()
        {
            if (!File.Exists(configFile))
            {
                Debug.Log($"[LaserCannon] Laser Cannon module config file not found. Writing default file.");
                mainLang = Languages[6];
                CreateLanguageDictionary();
                WriteConfigFile(false);
                LangText.Clear();
            }

            string[] configText = File.ReadAllLines(configFile, Encoding.UTF8);

            if (!ConfigProcess(configText))
            {
                Debug.Log($"[LaserCannon] Configfile: {configFile} damaged! Please correct it!");
            }
        }

        private static bool ConfigProcess(string[] configText)
        {
            if (configText[0] != configKey_main)
            {
                return false;
            }

            bool isExists = false;

            Dictionary<string, string> tempDic = new Dictionary<string, string>();

            for (int i = 1; i < configText.Length; i++)
            {
                string[] splittedText = configText[i].Split(new[] { '=' }, StringSplitOptions.None);

                tempDic.Add(splittedText[0].Trim(), splittedText[1].Trim());
            }

            foreach (KeyValuePair<string, string> item in tempDic)
            {
                if (item.Key.Equals(toggleID))
                {
                    bool.TryParse(item.Value, out bool result);
                    OnlyHostile = result;
                }

                if (item.Key.Equals(choiceID))
                {
                    LaserBeamColor = Array.IndexOf(beamColors, item.Value);
                }

                if (item.Key.Equals(configKey_language))
                {
                    int lang = Array.IndexOf(Languages, item.Value);

                    if (lang >= 0 && lang < Languages.Length)
                        mainLang = Languages[lang];
                    else
                        mainLang = Languages[6];
                }

                for (int x = 0; x < LanguageItems.Length; x++)
                {
                    for (int j = 0; j < Languages.Length; j++)
                    {
                        if (item.Key.Equals($"[{Languages[j]}]<"))
                        {
                            if (!LangText.ContainsKey($"[{Languages[j]}]"))
                                LangText.Add($"[{Languages[j]}]", lang_marker);
                        }

                        if (item.Key.Equals($"{LanguageItems[x].Trim()}{Languages[j]}"))
                        {
                            LangText.Add($"{LanguageItems[x].Trim()}{Languages[j]}", item.Value);

                            if (Languages[j].Equals(mainLang) && item.Value != "")
                                isExists = true;
                        }
                    }
                }
            }

            if (!isExists)
            {
                Debug.Log($"[LaserCannon] Warning! ({mainLang}) translate is empty or damaged in ({configFile})!");
                mainLang = Languages[6];
                return false;
            }
            return true;
        }

        internal static bool CreateLanguageDictionary()
        {
            foreach (string item in Languages)
            {
                if (item != Languages[6])
                {
                    LangText.Add($"[{item}]", lang_marker);
                    LangText.Add($"{LanguageItems[0]}{item}", "");
                    LangText.Add($"{LanguageItems[1]}{item}", "");
                    LangText.Add($"{LanguageItems[2]}{item}", "");
                    LangText.Add($"{LanguageItems[3]}{item}", "");
                    LangText.Add($"{LanguageItems[4]}{item}", "");
                }
                else
                {
                    LangText.Add($"[{item}]", lang_marker);
                    LangText.Add($"{LanguageItems[0]}{item}", title_English);
                    LangText.Add($"{LanguageItems[1]}{item}", toggle_English);
                    LangText.Add($"{LanguageItems[2]}{item}", color_English);
                    LangText.Add($"{LanguageItems[3]}{item}", description_English);
                    LangText.Add($"{LanguageItems[4]}{item}", friendlyName_English);
                }
            }
            return true;
        }


        internal static bool SetLang(string language)
        {
            foreach (KeyValuePair<string, string> item in LangText)
            {
                for (int i = 0; i < LanguageItems.Length; i++)
                {
                    if (item.Key.Equals(LanguageItems[0].Trim() + language))
                    {
                        title = item.Value;
                        if (item.Value == "")
                            return false;
                    }
                    if (item.Key.Equals(LanguageItems[1].Trim() + language))
                        toggle = item.Value;
                    if (item.Key.Equals(LanguageItems[2].Trim() + language))
                        color = item.Value;
                    if (item.Key.Equals(LanguageItems[3].Trim() + language))
                        description = item.Value;
                    if (item.Key.Equals(LanguageItems[4].Trim() + language))
                        friendlyName = item.Value;
                }
            }
            return true;
        }




    }
}
