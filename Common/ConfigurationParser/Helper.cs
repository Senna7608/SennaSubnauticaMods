using System.Collections.Generic;
using System.IO;

namespace Common.ConfigurationParser
{
    public class ConfigData : List<ConfigData>
    {
        public string Section { get; private set; }
        public string Key { get; private set; }
        public string Value { get; private set; }

        public ConfigData(string section, string key, string value)
        {
            Section = section;
            Key = key;
            Value = value;
        }
    }

    public class Helper
    {
        public static void CreateDefaultConfigFile(string filename, string programName, string version, List<ConfigData> configData)
        {            
            File.WriteAllText(filename,$"[{programName}]\r\nVersion: {version}\r\n");

            Parser parser = new Parser(filename);
            
            foreach (ConfigData data in configData)
            {
                if (!parser.IsExists(data.Section))
                    parser.AddNewSection(data.Section);

                parser.SetKeyValueInSection(data.Section, data.Key, data.Value);
            }            
        }

        public static void AddInfoText(string filename, string section, string value)
        {
            Parser parser = new Parser(filename);
            parser.WriteInfoText(section, value);
        }

        public static string GetKeyValue(string filename, string section, string key)
        {
            Parser parser = new Parser(filename);

            if (parser.IsExists(section, key))
            {
                return parser.GetKeyValueFromSection(section, key);
            }
            else
                return string.Empty;
        }        

        public static Dictionary<string, string> GetAllKeyValuesFromSection(string filename, string section, string[] keys)
        {
            Parser parser = new Parser(filename);                

            Dictionary<string, string> result = new Dictionary<string, string>();

            if (!parser.IsExists(section))
            {
                result.Add(section, "Error");
                return result;
            }            

            foreach (string key in keys)
            {
                if (parser.IsExists(section, key))
                {
                    result.Add(key, parser.GetKeyValueFromSection(section, key));                    
                }
                else
                    result.Add(section, key);
            }

            return result;
        }

        public static void SetKeyValue(string filename, string section, string key, string value)
        {
            Parser parser = new Parser(filename);
            parser.SetKeyValueInSection(section, key, value);
        }

        public static bool SetAllKeyValuesInSection(string filename, string section, Dictionary<string, string> keyValuePairs)
        {
            Parser parser = new Parser(filename);            

            foreach (KeyValuePair<string, string> kvp in keyValuePairs)
            {
                parser.SetKeyValueInSection(section, kvp.Key, kvp.Value);
            }

            return true;
        }
    }
}
