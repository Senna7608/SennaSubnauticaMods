using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using Common;
using Common.ConfigurationParser;

namespace CyclopsLaserCannonModule
{
    internal static class CannonConfig
    {
        public static string PROGRAM_VERSION = string.Empty;
        public static string CONFIG_VERSION = string.Empty;

        public static readonly string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string FILENAME = $"{modFolder}/config.txt";

        internal static Dictionary<string, string> program_settings;
        internal static Dictionary<string, string> language_settings;
        
        internal static string mainLang = string.Empty;                

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
            "Damage",
            "Language",
            "SFX_Volume"
        };

        public static readonly string[] SECTION_LANGUAGE =
        {
            "Item_Name",
            "Item_Description",            
            "Option_Title",
            "Option_OnlyHostile",            
            "Option_Damage",            
            "LowPower_Title",
            "LowPower_Message",
            "Item_Unlock_Message",
            "Option_SFXvolume"
        };


        private static readonly List<ConfigData> DEFAULT_CONFIG = new List<ConfigData>
        {
            new ConfigData(SECTIONS[0], SECTION_PROGRAM[0], false.ToString()),            
            new ConfigData(SECTIONS[0], SECTION_PROGRAM[1], 50.ToString()),
            new ConfigData(SECTIONS[0], SECTION_PROGRAM[2], Languages[6]),
            new ConfigData(SECTIONS[0], SECTION_PROGRAM[3], 10.ToString()),

            new ConfigData(SECTIONS[1], SECTION_LANGUAGE[0], "Cyclops Laser Cannon"),
            new ConfigData(SECTIONS[1], SECTION_LANGUAGE[1], "This Cannon using Precursor technology. Combined with Cyclops control system."),
            new ConfigData(SECTIONS[1], SECTION_LANGUAGE[2], "Cyclops Laser Cannon Module Options"),
            new ConfigData(SECTIONS[1], SECTION_LANGUAGE[3], "Damage hostile target only"),            
            new ConfigData(SECTIONS[1], SECTION_LANGUAGE[4], "Laser beam damage"),            
            new ConfigData(SECTIONS[1], SECTION_LANGUAGE[5], "Warning! Low Power!"),
            new ConfigData(SECTIONS[1], SECTION_LANGUAGE[6], "Cyclops Laser Cannon Disabled!"),
            new ConfigData(SECTIONS[1], SECTION_LANGUAGE[7], "Added blueprint for Cyclops Laser Cannon to database"),
            new ConfigData(SECTIONS[1], SECTION_LANGUAGE[8], "Sound effect volume"),

            new ConfigData(SECTIONS[2], SECTION_LANGUAGE[0], "Zyklop Laserkanone"),
            new ConfigData(SECTIONS[2], SECTION_LANGUAGE[1], "Diese Kanone verwendet die Precursor-Technologie. Kombiniert mit dem Zyklop-Steuersystem."),
            new ConfigData(SECTIONS[2], SECTION_LANGUAGE[2], "Laserkanone Modul Optionen"),
            new ConfigData(SECTIONS[2], SECTION_LANGUAGE[3], "Nur feindliches Ziel beschädigen"),            
            new ConfigData(SECTIONS[2], SECTION_LANGUAGE[4], "Laserstrahl schaden"),            
            new ConfigData(SECTIONS[2], SECTION_LANGUAGE[5], "Warnung! Geringer Strom!"),
            new ConfigData(SECTIONS[2], SECTION_LANGUAGE[6], "Zyklop Laserkanone Deaktiviert!"),
            new ConfigData(SECTIONS[2], SECTION_LANGUAGE[7], "Bauplan für Zyklop Laserkanone zur Datenbank hinzugefügt"),
            new ConfigData(SECTIONS[2], SECTION_LANGUAGE[8], "Lautstärke das Soundeffekte"),

            new ConfigData(SECTIONS[3], SECTION_LANGUAGE[0], "Küklopsz Lézerágyú"),
            new ConfigData(SECTIONS[3], SECTION_LANGUAGE[1], "Prekurzor technológiát felhasználva készített ágyú. Kombinálva a Küklopsz vezérlő rendszerével."),
            new ConfigData(SECTIONS[3], SECTION_LANGUAGE[2], "Küklopsz Lézerágyú beállítások"),
            new ConfigData(SECTIONS[3], SECTION_LANGUAGE[3], "Csak ellenséget sebez"),            
            new ConfigData(SECTIONS[3], SECTION_LANGUAGE[4], "Lézersugár erőssége"),            
            new ConfigData(SECTIONS[3], SECTION_LANGUAGE[5], "Vigyázat! Kevés az Energia!"),
            new ConfigData(SECTIONS[3], SECTION_LANGUAGE[6], "Küklopsz Lézerágyú letiltva!"),
            new ConfigData(SECTIONS[3], SECTION_LANGUAGE[7], "Küklopsz lézerágyú tervrajz hozzáadva az adatbázishoz."),
            new ConfigData(SECTIONS[3], SECTION_LANGUAGE[8], "Hanghatások hangereje"),
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

        internal static void Config_CreateDefault()
        {
            SNLogger.Debug("LaserCannon", "Method call: LaserCannonConfig.Config_CreateDefault()");

            SNLogger.Warn("LaserCannon", "Configuration file is missing or wrong version. Trying to create a new one.");

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

                SNLogger.Log("CyclopsLaserCannon", "The new configuration file was successfully created.");
            }
            catch
            {
                SNLogger.Error("CyclopsLaserCannon", "An error occured while creating the new configuration file!");
            }
        }

        private static bool Config_Check()
        {
            SNLogger.Debug("CyclopsLaserCannon", "Method call: LaserCannonConfig.Config_Check()");

            if (!File.Exists(FILENAME))
            {
                SNLogger.Error("CyclopsLaserCannon", "Configuration file open error!");
                return false;
            }

            CONFIG_VERSION = ParserHelper.GetKeyValue(FILENAME, "LaserCannon", "Version");

            if (!CONFIG_VERSION.Equals(PROGRAM_VERSION))
            {
                SNLogger.Error("CyclopsLaserCannon", "Configuration file version error!");
                return false;
            }

            if (!ParserHelper.CheckSectionKeys(FILENAME, "Hotkeys", SECTION_PROGRAM))
            {
                SNLogger.Error("CyclopsLaserCannon", "Configuration file [Program] section error!");
                return false;
            }

            if (!ParserHelper.CheckSectionKeys(FILENAME, "Settings", SECTION_LANGUAGE))
            {
                SNLogger.Error("CyclopsLaserCannon", "Configuration file [Language] section error!");
                return false;
            }

            return true;
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
                CONFIG_VERSION = ParserHelper.GetKeyValue(FILENAME, "CyclopsLaserCannon", "Version");


                if (CONFIG_VERSION.Equals(PROGRAM_VERSION))
                {
                    SNLogger.Log("CyclopsLaserCannon", "Configuration version match with program version.");
                }
                else
                {
                    CreateDefaultConfigFile();
                }
            }

            ReadConfig();                                             
        }

        internal static void CreateDefaultConfigFile()
        {
            SNLogger.Warn("CyclopsLaserCannon", "Configuration file is missing or wrong version. Trying to create a new one.");

            try
            {
                ParserHelper.CreateDefaultConfigFile(FILENAME, "CyclopsLaserCannon", PROGRAM_VERSION, DEFAULT_CONFIG);

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

                SNLogger.Log("CyclopsLaserCannon", "The new configuration file was successfully created.");
            }
            catch
            {
                SNLogger.Error("CyclopsLaserCannon", "An error occured while creating the new configuration file!");
            }
        }

        internal static void OnLanguageChanged()
        {            
            program_settings["Language"] = Language.main.GetCurrentLanguage();
            WriteConfig();
            ReadConfig();                      
        }

        internal static void ReadConfig()
        {
            try
            {
                program_settings = ParserHelper.GetAllKeyValuesFromSection(FILENAME, "Program", SECTION_PROGRAM);
                language_settings = ParserHelper.GetAllKeyValuesFromSection(FILENAME, program_settings["Language"], SECTION_LANGUAGE);

                SNLogger.Log("CyclopsLaserCannon", "Configuration loaded.");
            }
            catch
            {
                SNLogger.Error("CyclopsLaserCannon", "An error occurred while loading the configuration file!");
            }
        }

        internal static void WriteConfig()
        {
            foreach (KeyValuePair<string, string> item in program_settings)
            {
                ParserHelper.SetKeyValue(FILENAME, "Program", item.Key, item.Value);
            }

            SNLogger.Log("CyclopsLaserCannon", "Configuration saved.");
        }                    
    }
}
