using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Octopus.Core
{
    internal class DataManager
    {
        private static string m_normalFile = "Octopus.exe";
        private static string m_updateFile = "Octopus_update.exe";
        internal static string Version = "1.5.40";
        internal static bool InDevelopment = false;

        internal static bool IsUpdateFile()
        {
            return AppPath.Contains(m_updateFile);
        }

        internal static bool IsStartup()
        {
            string startup_folder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            return (startup_folder == Path.GetDirectoryName(AppPath));
        }

        internal static string StartupAppPath
        {
            get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), Path.GetFileName(AppPath)); }
        }

        internal static string AppPath
        {
            get { return System.Reflection.Assembly.GetExecutingAssembly().Location; }
        }

        internal static string NormalFile
        {
            get { return Path.Combine(Path.GetDirectoryName(AppPath), m_normalFile); }
        }

        internal static string UpdatingFile
        {
            get { return Path.Combine(Path.GetDirectoryName(AppPath), m_updateFile); }
        }

    }
}
