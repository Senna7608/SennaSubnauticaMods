using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Common.ConfigurationParser
{
    public class Parser
    {
        private FileReader _reader;
        private Dictionary<string, Section> _sections;

        public Parser(string filePath)
        {
            _reader = new FileReader(filePath);
            _sections = new SectionParser(_reader).Sections;
        }

        public string GetKeyValueFromSection(string section, string key)
        {
            if (!IsExists(section, key))
                return "Error";
            try
            {
                return _sections[section].GetKeyValue(key);
            }
            catch
            {
                Console.WriteLine($"Parser Error! Section [{section}] or Key [{key}] is missing from file: '{_reader.FilePath}'");
                return key;
            }
        }

        public void SetKeyValueInSection(string section, string key, string value)
        {
            SetAndWrite(section, key, value);
        }

        public bool IsExists(string section)
        {
            try
            {
                return _sections.ContainsKey(section);
            }
            catch
            {
                Console.WriteLine($"Parser Error! Section [{section}] is missing from file: '{_reader.FilePath}'");
                return false;
            }            
        }

        public bool IsExists(string section, string key)
        {
            if (!IsExists(section))
                return false;
            try
            {
                return _sections[section].ContainsKey(key);
            }
            catch
            {
                Console.WriteLine($"Parser Error! Section [{section}] or Key [{key}] is missing from file: '{_reader.FilePath}'");
                return false;
            }            
        }        

        public bool AddNewSection(string section)
        {
            try
            {
                _sections.Add(section, new Section(section));
                return true;
            }
            catch
            {
                Console.WriteLine($"Parser warning! The [{section}] section already exists in file: '{_reader.FilePath}'");
                return false;
            }
        }

        private void SetAndWrite(string section, string key, string value)
        {
            var s = _sections[section];

            if (s.ContainsKey(key) && s.GetKeyValue(key) == value)
                return;

            s.SetKeyValue(key, value);
            var sb = new StringBuilder();
            _sections.All(kvp => { sb.AppendFormat("{0}\r\n", kvp.Value.ToString()); return true; });
            File.WriteAllText(_reader.FilePath, sb.ToString());
        }

        public void WriteInfoText(string section, string value)
        {
            SetAndWrite(section, "Information", value);
        }
    }
}

