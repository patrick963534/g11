﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Octopus.Core
{
    public class DataManager
    {
        private static string m_normalFile = "Octopus.exe";
        private static string m_updateFile = "Octopus_update.exe";
        public static string Original_Path;
        public static string Version = "v1.5.6";

        public static bool IsUpdateFile()
        {
            return AppPath.Contains(m_updateFile);
        }

        public static bool IsStartup()
        {
            string startup_folder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            return (startup_folder == Path.GetDirectoryName(AppPath));
        }

        public static string StartupAppPath
        {
            get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), Path.GetFileName(AppPath)); }
        }

        public static string AppPath
        {
            get { return System.Reflection.Assembly.GetExecutingAssembly().Location; }
        }

        public static string NormalFile
        {
            get { return Path.Combine(Path.GetDirectoryName(AppPath), m_normalFile); }
        }

        public static string UpdatingFile
        {
            get { return Path.Combine(Path.GetDirectoryName(AppPath), m_updateFile); }
        }

    }
}
