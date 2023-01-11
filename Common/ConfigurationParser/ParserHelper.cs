using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#pragma warning disable CS1591
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

    
    public class ConfigDataNew
    {
        public string Section { get; private set; }
        public Dictionary<string, string> KeyValues { get; private set; }

        public string[] Keys { get; private set; }
        public string[] Values { get; private set; }

        public ConfigDataNew(string section, Dictionary<string, string> keyValues)
        {
            Section = section;
            KeyValues = keyValues;
            
            Keys = new string[keyValues.Count];
            Values = new string[keyValues.Count];

            int i = 0;

            foreach (KeyValuePair<string, string> kvp in keyValues)
            {
                Keys[i] = kvp.Key;
                Values[i] = kvp.Value;
                i++;
            }
        }
    }
    

    public class ParserHelper
    {
        public static void CreateDefaultConfigFile(string filename, string programName, string version, List<ConfigData> configData)
        {            
            File.WriteAllText(filename,$"[{programName}]\r\nVersion: {version}\r\n");

            Parser parser = new Parser(filename);           

            foreach (ConfigData data in configData)
            {
                if (!parser.IsExistsSection(data.Section))
                    parser.AddNewSection(data.Section);

                parser.SetKeyValueInSection(data.Section, data.Key, data.Value);
            }            
        }

        public static void AddInfoText(string filename, string key, string value)
        {
            Parser parser = new Parser(filename);

            if (!parser.IsExistsSection("Information"))
                parser.AddNewSection("Information");

            parser.SetKeyValueInSection("Information", key, value);
        }

        public static string GetKeyValue(string filename, string section, string key)
        {
            Parser parser = new Parser(filename);

            if (parser.IsExistsKey(section, key))
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

            if (!parser.IsExistsSection(section))
            {
                result.Add(section, "Error");
                return result;
            }            

            foreach (string key in keys)
            {
                if (parser.IsExistsKey(section, key))
                {
                    result.Add(key, parser.GetKeyValueFromSection(section, key));                    
                }
                else
                    result.Add(section, key);
            }

            return result;
        }

        public static Dictionary<string, string> GetAllKVPFromSection(string filename, string section)
        {
            Parser parser = new Parser(filename);

            Dictionary<string, string> result = new Dictionary<string, string>();

            if (!parser.IsExistsSection(section))
            {
                return result;
            }

            Section _section = parser.GetSection(section);
            
            foreach (KeyValuePair<string, string> kvp in _section)
            {
                result.Add(kvp.Key, kvp.Value);
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

        public static bool CheckSectionKeys(string filename, string section, string[] keys)
        {
            Parser parser = new Parser(filename);

            if (!parser.IsExistsSection(section))
            {
                return false;
            }

            foreach (string key in keys)
            {
                if (parser.IsExistsKey(section, key))
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public static bool CheckSectionKey(string filename, string section, string key)
        {
            Parser parser = new Parser(filename);

            return parser.IsExistsKey(section, key) ? true : false;
        }

        public static bool CheckSectionValuesExists(string filename, string section)
        {
            Parser parser = new Parser(filename);

            if (!parser.IsExistsSection(section))
            {
                return false;
            }

            Section _section = parser.GetSection(section);

            foreach (KeyValuePair<string, string> kvp in _section)
            {
                if (!parser.IsExistsKeyValue(section, kvp.Key))
                {
                    return false;
                }
            }

            return true;
        }

        public static void ClearSection(string filename, string section)
        {
            Parser parser = new Parser(filename);

            parser.ClearAndWrite(section);
        }
    }
}
