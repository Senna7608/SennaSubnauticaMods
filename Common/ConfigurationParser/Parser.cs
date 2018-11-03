using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ConfigurationParser
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
            try
            {
                return _sections[section].GetKeyValue(key);
            }
            catch
            {
                Console.WriteLine($"Parser Error! Section [{section}] or Key [{key}] is missing!");
                return "Error!";
            }
        }

        public void SetKeyValueInSection(string section, string key, string value)
        {
            SetAndWrite(section, key, value);
        }

        public bool SectionIsExists(string section)
        {
            return _sections.ContainsKey(section);
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
                Console.WriteLine($"Parser Error! Section [{section}] is already included in dictionary!");
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
    }
}

