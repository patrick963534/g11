using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Octopus.Core;

namespace Octopus
{
    public delegate void Action();

    public partial class Workbench : Form
    {
        private static Workbench s_singleton;

        public Workbench()
        {
            InitializeComponent();

            s_singleton = this;

            // Call the Show() method to force create Window Handler in the insternal.
            // Otherwise we can't use Invoke() method.
            this.Show();
            //this.Hide();           
            
            TouchVerifyCore.Start();
            NetClient.Start();
        }

        public static void HideIt()
        {
            s_singleton.Hide();
        }

        public static void Log(string msg)
        {
            s_singleton.Invoke(new Action(delegate
            {
                s_singleton.m_information_listbox.Items.Add(msg);
            }));
        }

        public static void DoAction(Action action)
        {
            s_singleton.Invoke(action);
        }

        public static void ExitForm()
        {
            s_singleton.Invoke(new Action(delegate
            {
                NetClient.Stop();
                TouchVerifyCore.Stop();
                Application.Exit();
            }));
        }

        private void Workbench_FormClosed(object sender, FormClosedEventArgs e)
        {
            ExitForm();
        }

        private void m_tray_Click(object sender, EventArgs e)
        {
            if (s_singleton.Visible)
                s_singleton.Hide();
            else
            {
                s_singleton.Show();
                s_singleton.BringToFront();
            }
        }
    }
}
