using Common.ConfigurationParser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

#pragma warning disable CS1591

namespace Common.Helpers
{
    public abstract class ConfigManager
    {
        public string PROGRAM_VERSION { get; protected set; } = string.Empty;
        public string CONFIG_VERSION { get; protected set; } = string.Empty;

        public static readonly string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        protected static readonly string FILENAME = $"{modFolder}/config.txt";

        public abstract string ProgramName { get; }            
        
        public abstract List<ConfigDataNew> DEFAULT_CONFIG { get; }       

        public virtual void LoadConfig()
        {
            PROGRAM_VERSION = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

            if (!CheckConfig())
            {
               CreateDefault();
            }            
        }

        private bool CheckConfig()
        {
            if (!File.Exists(FILENAME))
            {
                SNLogger.Error(ProgramName, "Configuration file does not exists!");
                return false;
            }

            CONFIG_VERSION = ParserHelper.GetKeyValue(FILENAME, ProgramName, "Version");

            if (!CONFIG_VERSION.Equals(PROGRAM_VERSION))
            {
                SNLogger.Error(ProgramName, "Configuration file version error!");
                return false;
            }            

            foreach (ConfigDataNew configData in DEFAULT_CONFIG)
            {
                if (!ParserHelper.CheckSectionKeys(FILENAME, configData.Section, configData.Keys))
                {
                    SNLogger.Error(ProgramName, $"Configuration file [{configData.Section}] section error!");
                    return false;
                }
            }            

            return true;
        }

                  

        public virtual void CreateDefault()
        {
            /*
            SNLogger.Warn(ProgramName, "Configuration file is missing or wrong version. Trying to create a new one.");

            try
            {
                ParserHelper.CreateDefaultConfigFile(FILENAME, ProgramName, PROGRAM_VERSION, DEFAULT_CONFIG);                

                SNLogger.Log(ProgramName, "The new configuration file was successfully created.");
            }
            catch
            {
                SNLogger.Error(ProgramName, "An error occured while creating the new configuration file!");
            }
            */
        }







    }
}
