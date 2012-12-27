using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Octopus.Core
{
    public class GroupConfig
    {
        private static string cfg_file = "Octopus_Group.cfg";

        public static void Save()
        {
            try
            {
                string path = Path.Combine(Path.GetTempPath(), cfg_file);

                if (File.Exists(path))
                    File.Delete(path);

                StreamWriter sw = new StreamWriter(path);
                GroupInfo[] groups = GroupInfoManager.GetGroupArray();
                foreach (GroupInfo grp in groups)
                {
                    sw.WriteLine(grp.Key + ";" + grp.Name);
                }
                sw.Close();
            }
            catch
            {
            	
            }
        }

        public static void Load()
        {
            try
            {
                string path = Path.Combine(Path.GetTempPath(), cfg_file);
                if (!File.Exists(path))
                    return;

                StreamReader sr = new StreamReader(path);
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    int pos = line.IndexOf(';');
                    string key = line.Substring(0, pos);
                    string name = line.Substring(pos + 1);
                    GroupInfoManager.AddGroup(new GroupInfo(key, name));
                }
            }
            catch
            {
            	
            }
        }
    }
}
