using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

namespace Octopus.Core
{
    public class CustomConfigure
    {
        private const string Key_DisplayName = "DisplayName";
        private const string Key_FontSize = "FontSize";
        private const string Key_FontColor = "FontColor";

        private static string ConfigFile = "CustomConfig.cfg";
        private static string Seperator = "$%$";

        private static string m_displayName = Environment.UserName;

        static CustomConfigure()
        {
            string path = Path.Combine(DataManager.GetCfgFolderPath(), ConfigFile);

            if (!File.Exists(path))
                return;

            using (StreamReader sr = new StreamReader(path))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] pair = line.Split(new string[] { Seperator }, StringSplitOptions.RemoveEmptyEntries);

                    if (pair.Length != 2)
                        continue;

                    string key = pair[0].Trim();
                    string val = pair[1].Trim();

                    if (key == Key_DisplayName)
                    {
                        m_displayName = val;
                    }
                    else if (key == Key_FontColor)
                    {
                        MsgInputConfig.FontColor = val;
                    }
                    else if (key == Key_FontSize)
                    {
                        MsgInputConfig.FontSize = val;
                    }
                }
            }
        }

        public static string DisplayName
        {
            get { return m_displayName; }
        }

        public static void SetFontSize(int sz)
        {
            if (sz > 7 && sz < 30)
            {
                MsgInputConfig.FontSize = sz.ToString();
            }
        }

        public static void SetFontColor(Color color)
        {
            MsgInputConfig.FontColor = string.Format("#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
        }

        public static void SetDisplayName(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                m_displayName = name;
            }
        }

        public static void Save()
        {
            string path = Path.Combine(DataManager.GetCfgFolderPath(), ConfigFile);
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                sw.WriteLine(string.Format("{0}{1}{2}", Key_DisplayName, Seperator, DisplayName));
                sw.WriteLine(string.Format("{0}{1}{2}", Key_FontSize, Seperator, MsgInputConfig.FontSize));
                sw.WriteLine(string.Format("{0}{1}{2}", Key_FontColor, Seperator, MsgInputConfig.FontColor));
                sw.Flush();  
            }
        }
    }
}
