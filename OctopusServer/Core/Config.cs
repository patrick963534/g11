using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace OctopusServer.Core
{
    internal class Config
    {
        private static string ConfigFile = "OctopusServer.conf";

        internal static void Load()
        {
            string file = Path.Combine(Path.GetTempPath(), ConfigFile);
            if (!File.Exists(file))
                return;

            using (StreamReader reader = new StreamReader(file))
            {
                string line = reader.ReadLine();
                if (line == null)
                    return;

                string[] sub = line.Split(';');

                if (sub.Length != 2)
                    return;

                DataManager.UpdateFilePath = sub[0];
                DataManager.Version = sub[1];
            }
        }

        internal static void Save()
        {
            string file = Path.Combine(Path.GetTempPath(), ConfigFile);
            if (File.Exists(file))
                File.Delete(file);

            using (StreamWriter writer = new StreamWriter(file))
            {
                writer.WriteLine(DataManager.UpdateFilePath + ";" + DataManager.Version);
            }
        }
    }
}
