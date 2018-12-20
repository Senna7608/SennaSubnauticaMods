using System;
using System.Collections.Generic;
using System.IO;
using ConfigurationParser;
using Common;
using System.Diagnostics;
using System.Reflection;

namespace LaserCannon
{
    internal static class Config
    {
        internal static string VERSION = string.Empty;
        private const string PROGRAM_NAME = "LaserCannon";        

        internal static readonly string FILENAME = Environment.CurrentDirectory + "\\QMods\\LaserCannon\\config.txt";

        internal static Dictionary<string, string> program_settings;
        internal static Dictionary<string, string> language_settings;
        internal static List<string> colorNames = new List<string>();

        internal static string mainLang = string.Empty;
        internal static int beamColor = 1;        

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

        private static readonly string[] SECTIONS =
        {   "Program",
            "English",
            "German",
            "Hungarian"
        };

        public static readonly string[] SECTION_PROGRAM =
        {
            "OnlyHostile",
            "BeamColor",
            "Damage",
            "Language"
        };

        public static readonly string[] SECTION_LANGUAGE =
        {
            "Item_Name",
            "Item_Description",
            "Option_Title",
            "Option_OnlyHostile",
            "Option_BeamColor",
            "Option_Damage",
            "Option_Color_Red",
            "Option_Color_Green",
            "Option_Color_Blue",
            "Option_Color_Yellow",
            "Option_Color_White",
            "Option_Color_Magenta",
            "Option_Color_Cyan",
            "Option_Color_Orange",
            "Option_Color_Lime",
            "Option_Color_Amethyst",
            "Option_Color_Default"
        };

        private static readonly List<ConfigData> DEFAULT_CONFIG = new List<ConfigData>
        {
            new ConfigData(SECTIONS[0], SECTION_PROGRAM[0], false.ToString()),
            new ConfigData(SECTIONS[0], SECTION_PROGRAM[1], Modules.Colors.ColorNames[0]),
            new ConfigData(SECTIONS[0], SECTION_PROGRAM[2], 1.ToString()),
            new ConfigData(SECTIONS[0], SECTION_PROGRAM[3], Languages[6]),

            new ConfigData(SECTIONS[1], SECTION_LANGUAGE[0], "Laser Cannon"),
            new ConfigData(SECTIONS[1], SECTION_LANGUAGE[1], "Recovered laser beam technology from an ancient Precursor weapon fragment."),
            new ConfigData(SECTIONS[1], SECTION_LANGUAGE[2], "Laser Cannon Module Options"),
            new ConfigData(SECTIONS[1], SECTION_LANGUAGE[3], "Damage hostile target only"),
            new ConfigData(SECTIONS[1], SECTION_LANGUAGE[4], "Laser beam color"),
            new ConfigData(SECTIONS[1], SECTION_LANGUAGE[5], "Laser beam damage"),
            new ConfigData(SECTIONS[1], SECTION_LANGUAGE[6], "Red"),
            new ConfigData(SECTIONS[1], SECTION_LANGUAGE[7], "Green"),
            new ConfigData(SECTIONS[1], SECTION_LANGUAGE[8], "Blue"),
            new ConfigData(SECTIONS[1], SECTION_LANGUAGE[9], "Yellow"),
            new ConfigData(SECTIONS[1], SECTION_LANGUAGE[10], "White"),
            new ConfigData(SECTIONS[1], SECTION_LANGUAGE[11], "Magenta"),
            new ConfigData(SECTIONS[1], SECTION_LANGUAGE[12], "Cyan"),
            new ConfigData(SECTIONS[1], SECTION_LANGUAGE[13], "Orange"),
            new ConfigData(SECTIONS[1], SECTION_LANGUAGE[14], "Lime green"),
            new ConfigData(SECTIONS[1], SECTION_LANGUAGE[15], "Purple"),
            new ConfigData(SECTIONS[1], SECTION_LANGUAGE[16], "Default"),

            new ConfigData(SECTIONS[2], SECTION_LANGUAGE[0], "Laserkanone"),
            new ConfigData(SECTIONS[2], SECTION_LANGUAGE[1], "Wiedergewonnene Laserstrahltechnologie von einem alten Precursor Waffenfragment."),
            new ConfigData(SECTIONS[2], SECTION_LANGUAGE[2], "Laser Cannon Modul Optionen"),
            new ConfigData(SECTIONS[2], SECTION_LANGUAGE[3], "Nur feindliches Ziel beschädigen"),
            new ConfigData(SECTIONS[2], SECTION_LANGUAGE[4], "Laserstrahl Farbe"),
            new ConfigData(SECTIONS[2], SECTION_LANGUAGE[5], "Laserstrahl schaden"),
            new ConfigData(SECTIONS[2], SECTION_LANGUAGE[6], "Rot"),
            new ConfigData(SECTIONS[2], SECTION_LANGUAGE[7], "Grün"),
            new ConfigData(SECTIONS[2], SECTION_LANGUAGE[8], "Blau"),
            new ConfigData(SECTIONS[2], SECTION_LANGUAGE[9], "Gelb"),
            new ConfigData(SECTIONS[2], SECTION_LANGUAGE[10], "Weiß"),
            new ConfigData(SECTIONS[2], SECTION_LANGUAGE[11], "Magenta"),
            new ConfigData(SECTIONS[2], SECTION_LANGUAGE[12], "Cyan"),
            new ConfigData(SECTIONS[2], SECTION_LANGUAGE[13], "Orange"),
            new ConfigData(SECTIONS[2], SECTION_LANGUAGE[14], "Lindgrün"),
            new ConfigData(SECTIONS[2], SECTION_LANGUAGE[15], "Lila"),
            new ConfigData(SECTIONS[2], SECTION_LANGUAGE[16], "Standard"),

            new ConfigData(SECTIONS[3], SECTION_LANGUAGE[0], "Lézerágyú"),
            new ConfigData(SECTIONS[3], SECTION_LANGUAGE[1], "Egy ősi idegen eredetű fegyvertöredékből visszafejtett lézersugár technológia."),
            new ConfigData(SECTIONS[3], SECTION_LANGUAGE[2], "Lézerágyú beállítások"),
            new ConfigData(SECTIONS[3], SECTION_LANGUAGE[3], "Csak ellenséget sebez"),
            new ConfigData(SECTIONS[3], SECTION_LANGUAGE[4], "Lézersugár színe"),
            new ConfigData(SECTIONS[3], SECTION_LANGUAGE[5], "Lézersugár erőssége"),
            new ConfigData(SECTIONS[3], SECTION_LANGUAGE[6], "Piros"),
            new ConfigData(SECTIONS[3], SECTION_LANGUAGE[7], "Zöld"),
            new ConfigData(SECTIONS[3], SECTION_LANGUAGE[8], "Kék"),
            new ConfigData(SECTIONS[3], SECTION_LANGUAGE[9], "Sárga"),
            new ConfigData(SECTIONS[3], SECTION_LANGUAGE[10], "Fehér"),
            new ConfigData(SECTIONS[3], SECTION_LANGUAGE[11], "Bíbor"),
            new ConfigData(SECTIONS[3], SECTION_LANGUAGE[12], "Ciánkék"),
            new ConfigData(SECTIONS[3], SECTION_LANGUAGE[13], "Narancssárga"),
            new ConfigData(SECTIONS[3], SECTION_LANGUAGE[14], "Limezöld"),
            new ConfigData(SECTIONS[3], SECTION_LANGUAGE[15], "Lila"),
            new ConfigData(SECTIONS[3], SECTION_LANGUAGE[16], "Alapbeállítás")
        };

        internal static void InitConfig()
        {
            VERSION = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;            

            if (!File.Exists(FILENAME))
            {
                UnityEngine.Debug.Log($"[{PROGRAM_NAME}] Warning! Configuration file is missing. Creating a new one.");

                Helper.CreateDefaultConfigFile(FILENAME, PROGRAM_NAME, VERSION, DEFAULT_CONFIG);

                var configParser = new Parser(FILENAME);

                foreach (string language in Languages)
                {
                    if (!configParser.AddNewSection(language))
                        continue;

                    foreach (string item in SECTION_LANGUAGE)
                    {
                        configParser.SetKeyValueInSection(language, item, "");
                    }
                }
            }

            ReadConfig();
            ReadLanguageText();
            InitColorNames();
        }

        internal static void OnLanguageChanged()
        {            
            program_settings["Language"] = Language.main.GetCurrentLanguage();
            WriteConfig();
            ReadConfig();
            ReadLanguageText();
            InitColorNames();
        }

        internal static void ReadConfig()
        {
            program_settings = Helper.GetAllKeyValuesFromSection(FILENAME, "Program", SECTION_PROGRAM);
           
            for (int i = 0; i < Modules.Colors.ColorNames.Length; i++)
            {
                if (Modules.Colors.ColorNames[i].Equals(program_settings["BeamColor"]))
                    beamColor = i;
            }
        }

        internal static void WriteConfig()
        {
            foreach (KeyValuePair<string, string> item in program_settings)
            {
                Helper.SetKeyValue(FILENAME, "Program", item.Key, item.Value);
            }
        }        

        internal static void ReadLanguageText()
        {            
            language_settings = Helper.GetAllKeyValuesFromSection(FILENAME, program_settings["Language"], SECTION_LANGUAGE);
        }

        internal static void InitColorNames()
        {
            colorNames.Clear();

            foreach (KeyValuePair<string, string> item in language_settings)
            {                
                if (item.Key.StartsWith("Option_Color_"))
                    colorNames.Add(item.Value.ToString());
            }
        }          
    }
}
