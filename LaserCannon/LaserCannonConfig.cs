using System.Collections.Generic;
using System.IO;
using Common.ConfigurationParser;
using Common;
using System.Diagnostics;
using System.Reflection;
using Common.Helpers;

namespace LaserCannon
{
    internal static class LaserCannonConfig
    {        
        internal static string PROGRAM_VERSION = string.Empty;
        internal static string CONFIG_VERSION = string.Empty;
        
        private static readonly string FILENAME = $"{Main.modFolder}/config.txt";

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
            "Hungarian",
            "French"
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
            "Option_Color_LightBlue",
            "LowPower_Title",
            "LowPower_Message",
        };

        private static readonly List<ConfigData> DEFAULT_CONFIG = new List<ConfigData>
        {
            new ConfigData(SECTIONS[0], SECTION_PROGRAM[0], false.ToString()),
            new ConfigData(SECTIONS[0], SECTION_PROGRAM[1], "LightBlue"),
            new ConfigData(SECTIONS[0], SECTION_PROGRAM[2], 50.ToString()),
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
            new ConfigData(SECTIONS[1], SECTION_LANGUAGE[16], "LightBlue"),
            new ConfigData(SECTIONS[1], SECTION_LANGUAGE[17], "Warning! Low Power!"),
            new ConfigData(SECTIONS[1], SECTION_LANGUAGE[18], "Laser Cannon Disabled!"),

            new ConfigData(SECTIONS[2], SECTION_LANGUAGE[0], "Laserkanone"),
            new ConfigData(SECTIONS[2], SECTION_LANGUAGE[1], "Wiedergewonnene Laserstrahltechnologie von einem alten Precursor Waffenfragment."),
            new ConfigData(SECTIONS[2], SECTION_LANGUAGE[2], "Laserkanone Modul Optionen"),
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
            new ConfigData(SECTIONS[2], SECTION_LANGUAGE[16], "Hellblau"),
            new ConfigData(SECTIONS[2], SECTION_LANGUAGE[17], "Warnung! Geringer Strom!"),
            new ConfigData(SECTIONS[2], SECTION_LANGUAGE[18], "Laserkanone Deaktiviert!"),

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
            new ConfigData(SECTIONS[3], SECTION_LANGUAGE[16], "Világoskék"),
            new ConfigData(SECTIONS[3], SECTION_LANGUAGE[17], "Vigyázat! Kevés az Energia!"),
            new ConfigData(SECTIONS[3], SECTION_LANGUAGE[18], "Lézerágyú letiltva!"),

            new ConfigData(SECTIONS[4], SECTION_LANGUAGE[0], "Canon Laser"),
            new ConfigData(SECTIONS[4], SECTION_LANGUAGE[1], "Utilise une ancienne technologie d'arme des Précurseurs."),
            new ConfigData(SECTIONS[4], SECTION_LANGUAGE[2], "Options du Module de Canon Laser"),
            new ConfigData(SECTIONS[4], SECTION_LANGUAGE[3], "Fonctionne seulement sur les animaux aggressifs"),
            new ConfigData(SECTIONS[4], SECTION_LANGUAGE[4], "Couleur du laser"),
            new ConfigData(SECTIONS[4], SECTION_LANGUAGE[5], "Dommage fait par le laser"),
            new ConfigData(SECTIONS[4], SECTION_LANGUAGE[6], "Rouge"),
            new ConfigData(SECTIONS[4], SECTION_LANGUAGE[7], "Vert"),
            new ConfigData(SECTIONS[4], SECTION_LANGUAGE[8], "Bleu"),
            new ConfigData(SECTIONS[4], SECTION_LANGUAGE[9], "Jaune"),
            new ConfigData(SECTIONS[4], SECTION_LANGUAGE[10], "Blanc"),
            new ConfigData(SECTIONS[4], SECTION_LANGUAGE[11], "Magenta"),
            new ConfigData(SECTIONS[4], SECTION_LANGUAGE[12], "Cyan"),
            new ConfigData(SECTIONS[4], SECTION_LANGUAGE[13], "Orange"),
            new ConfigData(SECTIONS[4], SECTION_LANGUAGE[14], "Vert Lime"),
            new ConfigData(SECTIONS[4], SECTION_LANGUAGE[15], "Violet"),
            new ConfigData(SECTIONS[4], SECTION_LANGUAGE[16], "Bleu Clair"),
            new ConfigData(SECTIONS[4], SECTION_LANGUAGE[17], "Attention! Niveau d'énergie critique!"),
            new ConfigData(SECTIONS[4], SECTION_LANGUAGE[18], "Canon Laser désactivé!"),
        };

        internal static void Config_Load()
        {
            PROGRAM_VERSION = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

            if (!Config_Check())
            {
                Config_CreateDefault();
            }                                  

            ReadConfig();            
        }        

        internal static void OnLanguageChanged()
        {            
            program_settings["Language"] = Language.main.GetCurrentLanguage();
            WriteConfig();
            ReadConfig();            
        }
               

        internal static void WriteConfig()
        {
            foreach (KeyValuePair<string, string> item in program_settings)
            {
                ParserHelper.SetKeyValue(FILENAME, "Program", item.Key, item.Value);
            }

            SNLogger.Log("Configuration saved.");
        }        

        internal static void ReadConfig()
        {
            try
            {
                program_settings = ParserHelper.GetAllKeyValuesFromSection(FILENAME, "Program", SECTION_PROGRAM);

                for (int i = 0; i < ColorHelper.ColorNames.Length; i++)
                {
                    if (ColorHelper.ColorNames[i].Equals(program_settings["BeamColor"]))
                        beamColor = i;
                }

                language_settings = ParserHelper.GetAllKeyValuesFromSection(FILENAME, program_settings["Language"], SECTION_LANGUAGE);

                colorNames.Clear();

                foreach (KeyValuePair<string, string> item in language_settings)
                {
                    if (item.Key.StartsWith("Option_Color_"))
                        colorNames.Add(item.Value.ToString());
                }

                SNLogger.Log("Configuration loaded.");
            }
            catch
            {
                SNLogger.Error("An error occurred while loading the configuration file!");
            }
        }




        internal static void Config_CreateDefault()
        {
            SNLogger.Debug("Method call: LaserCannonConfig.Config_CreateDefault()");

            SNLogger.Warn("Configuration file is missing or wrong version. Trying to create a new one.");
            
            try
            {
                ParserHelper.CreateDefaultConfigFile(FILENAME, "LaserCannon", PROGRAM_VERSION, DEFAULT_CONFIG);

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

                SNLogger.Log("The new configuration file was successfully created.");
            }
            catch
            {
                SNLogger.Error("An error occured while creating the new configuration file!");
            }            
        }

        private static bool Config_Check()
        {
            SNLogger.Debug("Method call: LaserCannonConfig.Config_Check()");

            if (!File.Exists(FILENAME))
            {
                SNLogger.Error("Configuration file open error!");
                return false;
            }

            CONFIG_VERSION = ParserHelper.GetKeyValue(FILENAME, "LaserCannon", "Version");

            if (!CONFIG_VERSION.Equals(PROGRAM_VERSION))
            {
                SNLogger.Error("Configuration file version error!");
                return false;
            }

            if (!ParserHelper.CheckSectionKeys(FILENAME, "Program", SECTION_PROGRAM))
            {
                SNLogger.Error("Configuration file [Program] section error!");
                return false;
            }

            foreach (string lang in Languages)
            {
                if (!ParserHelper.CheckSectionKeys(FILENAME, lang, SECTION_LANGUAGE))
                {
                    SNLogger.Error("Configuration file [Language] section error!");
                    return false;
                }
            }
            

            return true;
        }
    }
}
