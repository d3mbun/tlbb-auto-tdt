using System;
using System.IO;
using System.Collections;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace _i
{
    public class SectionPair
    {
        public static SectionPair Create(string section, string key)
        {
            return new SectionPair(section, key);
        }

        public SectionPair()
        {
        }

        public SectionPair(string section, string key)
        {
            Key = section;
            Value = key;
        }

        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;

        public override int GetHashCode()
        {
            return ("[" + Key + "]" + Value).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj != null && (obj as SectionPair).Key == this.Key && (obj as SectionPair).Value == this.Value;
        }
    }

    public class IniParser
    {
        private Dictionary<SectionPair, string> keyPairs = new Dictionary<SectionPair, string>();

       

        /// <summary>
        /// Opens the INI file at the given path and enumerates the values in the IniParser.
        /// </summary>
        /// <param name="ini">Full path to INI file.</param>
        public IniParser(String ini)
        {

            TextReader iniFile = null;
            String strLine = null;
            String currentRoot = null;
            String[] keyPair = null;



            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(ini);
                iniFile = new StreamReader(new MemoryStream(buffer));

                strLine = iniFile.ReadLine();

                while (strLine != null)
                {
                    if (!strLine.Equals(string.Empty))
                    {
                        if (strLine.StartsWith("[") && strLine.EndsWith("]"))
                        {
                            currentRoot = strLine.Substring(1, strLine.Length - 2);
                        }
                        else
                        {
                            keyPair = strLine.Split(new char[] { '=' }, 2);

                            SectionPair sectionPair = new SectionPair();

                            if (currentRoot == null)
                                currentRoot = "ROOT";


                            sectionPair.Key = currentRoot;
                            sectionPair.Value = keyPair[0];

                            keyPairs.Add(sectionPair, keyPair[1].Replace("\\n", "\r\n"));
                        }
                    }
                    strLine = iniFile.ReadLine();
                }

            }
            catch { }


        }

        public static IniParser Load(string path)
        {
            if (File.Exists(path))
            {
                return new IniParser(TDT.ReadFile(path));
            }
            if (File.Exists(path + ".old"))
            {
                return new IniParser(TDT.ReadFile(path + ".old"));
            }
            return new IniParser(string.Empty);
        }



        public void Save(string path)
        {
            ToString().SafeSave(path);
        }

        /// <summary>
        /// Returns the value for the given section, key pair.
        /// </summary>
        /// <param name="sectionName">Section name.</param>
        /// <param name="settingName">Key name.</param>
        public String Read(String sectionName, String settingName)
        {
            lock (Game.syncObj)
            {
                if (sectionName != "profile" && sectionName != "Pet" && sectionName != "User" && sectionName != "Config" && Main.ProfileName != "#")
                    sectionName += Main.ProfileName;

                var section = new SectionPair(sectionName, settingName);

                if (keyPairs.ContainsKey(section))
                {
                    var value = keyPairs[section];

                    if (value == null)
                        return string.Empty;

                    return value;
                }

                return string.Empty;
            }
        }

        public IEnumerable<SectionPair> this[string section]
        {
            get
            {
                foreach(var k in keyPairs)
                {
                    if (k.Key.Key == section)
                        yield return new SectionPair(k.Key.Value, k.Value);
                }
            }
        }

        public void RemoveSection(string section)
        {
            keyPairs = keyPairs.Where(k => k.Key.Key != section).ToDictionary(k => k.Key, k => k.Value);
        }

        /// <summary>
        /// Enumerates all lines for given section.
        /// </summary>
        /// <param name="sectionName">Section to enum.</param>
        public String[] EnumSection(String sectionName)
        {
            ArrayList tmpArray = new ArrayList();

            foreach (SectionPair pair in keyPairs.Keys)
            {
                if (pair.Key == sectionName)
                {
                    tmpArray.Add(pair.Value);                    
                }
            }

            return (String[])tmpArray.ToArray(typeof(String));
        }


        public void DeleteSectionContain(string input)
        {
            foreach (SectionPair pair in keyPairs.Keys.ToList())
            {
                if (pair.Key.Contains(input))
                    DeleteSetting(pair.Key, pair.Value);
            }
        }

        /// <summary>
        /// Adds or replaces a setting to the table to be saved.
        /// </summary>
        /// <param name="sectionName">Section to add under.</param>
        /// <param name="settingName">Key name to add.</param>
        /// <param name="settingValue">Value of key.</param>
        public void Write(String sectionName, String settingName, String settingValue)
        {
            lock (Game.syncObj)
            {
                if (sectionName != "profile" && sectionName != "Pet" && sectionName != "User" && sectionName != "Config" && Main.ProfileName != "#")
                    sectionName += Main.ProfileName;

                keyPairs[new SectionPair(sectionName, settingName)] = settingValue;

            }
        }


        /// <summary>
        /// Remove a setting.
        /// </summary>
        /// <param name="sectionName">Section to add under.</param>
        /// <param name="settingName">Key name to add.</param>
        public void DeleteSetting(String sectionName, String settingName)
        {
            //keyPairs = keyPairs.Where(k => !(k.Key.Key == sectionName && k.Key.Section == settingName));

            SectionPair sectionPair = new SectionPair();
            sectionPair.Key = sectionName;
            sectionPair.Value = settingName;


            foreach (KeyValuePair<SectionPair, string> kvp in keyPairs.ToList())
            {
                if (kvp.Key.Value == sectionPair.Value && kvp.Key.Key == sectionPair.Key)
                    keyPairs.Remove(kvp.Key);
            }
        }
       // public string strToSave { get; set; } = "";
        public override string ToString()
        {
            IEnumerable<string> sections = keyPairs.Keys.Select(k => k.Key).Distinct();

            String tmpValue = "";
            StringBuilder sb = new StringBuilder();


            foreach (String section in sections)
            {
                sb.Append("[" + section + "]\r\n");
                foreach (SectionPair sectionPair in keyPairs.Keys)
                {
                    try
                    {
                        if (sectionPair.Key == section)
                        {
                            tmpValue = keyPairs[sectionPair];


                            if (!string.IsNullOrEmpty(tmpValue))
                            {
                                sb.Append(sectionPair.Value + "=" + tmpValue.Replace("\r", "").Replace("\n", "\\n") + "\r\n");
                            }
                        }
                    }
                    catch { }
                }
                sb.Append("\r\n");
            }
            return sb.ToString();
        }
    }
}