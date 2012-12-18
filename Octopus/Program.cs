using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Octopus.Core;
using System.Threading;

namespace Octopus
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                if (args[0] == "-remove_update_file")
                {
                    Thread.Sleep(5000);
                    File.Delete(DataManager.UpdatingFile);
                }
                else if (args[0] == "-development")
                {
                    DataManager.InDevelopment = true;
                }                
            }

            if (!DataManager.InDevelopment && DataManager.IsUpdateFile())
            {
                Thread.Sleep(10000);
                File.Copy(DataManager.UpdatingFile, DataManager.NormalFile, true);
                Process.Start("\"" + DataManager.NormalFile + "\"", "-remove_update_file");
            }
            else if (!DataManager.InDevelopment && !DataManager.IsStartup())
            {
                File.Copy(DataManager.AppPath, DataManager.StartupAppPath, true);
                Process.Start("\"" + DataManager.StartupAppPath + "\"");
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Workbench());
            }
        }
    }

    internal class HideOnStartupApplicationContext : ApplicationContext
    {
        private Form form;

        internal HideOnStartupApplicationContext(Form mainForm)
        {
            this.form = mainForm;
        }
    }
}
