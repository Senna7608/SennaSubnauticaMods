using System.Collections.Generic;
using System.IO;

namespace ConfigurationParser
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
        public static void CreateDefaultConfigFile(string filename, string programName, string version,List<ConfigData> configDatas)
        {            
            File.WriteAllText(filename,$"[{programName}]\r\nVersion: {version}\r\n");

            Parser configParser = new Parser(filename);
            
            foreach (ConfigData data in configDatas)
            {
                if (!configParser.SectionIsExists(data.Section))
                    configParser.AddNewSection(data.Section);

                configParser.SetKeyValueInSection(data.Section, data.Key, data.Value);
            }            
        }
        
        public static object GetKeyValue(string filename, string section, string key)
        {
            Parser parser = new Parser(filename);
            return parser.GetKeyValueFromSection(section, key);
        }

        public static void SetKeyValue(string filename, string section, string key, string value)
        {
            Parser parser = new Parser(filename);
            parser.SetKeyValueInSection(section, key, value);
        }

        public static Dictionary<string, string> GetAllKeyValuesFromSection(string filename, string section, string[] keys)
        {
            Parser parser = new Parser(filename);

            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach (string key in keys)
            {
                result.Add(key, parser.GetKeyValueFromSection(section, key));
            }

            return result;
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
