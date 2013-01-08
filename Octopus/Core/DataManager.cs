using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace Octopus.Core
{
    public class DataManager
    {

        private static string m_normalFile = "Octopus.exe";
        private static string m_updateFile = "Octopus_update.exe";
        public static string Version = "1.98.96";
        public static bool InDevelopment = false;

        private static string CustomFaceFolderName = "CustomFace";
        private static string m_cfg_folder;

        static DataManager()
        {
            m_cfg_folder = Path.Combine(Path.GetTempPath(), "Octopus");
            MakeSureCfgFolder();
        }

        private static void MakeSureCfgFolder()
        {
            if (!Directory.Exists(m_cfg_folder))
                Directory.CreateDirectory(m_cfg_folder);

            string customFace = Path.Combine(m_cfg_folder, CustomFaceFolderName);
            if (!Directory.Exists(customFace))
                Directory.CreateDirectory(customFace);
        }

        public static string GetCustomFaceFolderPath()
        {
            return Path.Combine(m_cfg_folder, CustomFaceFolderName);
        }

        public static string GetCfgFolderPath()
        {
            return m_cfg_folder;
        }

        public static bool IsUpdateFile()
        {
            return AppPath.Contains(m_updateFile);
        }

        public static bool IsStartup()
        {
            string startup_folder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            return (startup_folder == Path.GetDirectoryName(AppPath));
        }

        public static string WhoAmI
        {
            get { return string.Format("{0} ({1})", Environment.UserName, Environment.MachineName); }
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
