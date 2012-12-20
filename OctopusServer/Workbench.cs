using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;
using OctopusServer.Core;

namespace OctopusServer
{
    public delegate void Action();

    public partial class Workbench : Form
    {
        private static Workbench s_singleton;

        public Workbench()
        {
            InitializeComponent();
            s_singleton = this;

            Config.Load();
            m_update_file_path_txb.Text = DataManager.UpdateFilePath;
            m_version_tbx.Text = DataManager.Version;
        }

        public static void Log(string msg)
        {
            s_singleton.Invoke(new Action(delegate {
                if (s_singleton.m_information_listbox.Items.Count > 200)
                    s_singleton.m_information_listbox.Items.Clear();
                s_singleton.m_information_listbox.Items.Add(DateTime.Now.ToShortTimeString() + " : " + msg);
            }));
        }

        private void m_browse_btn_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                m_update_file_path_txb.Text = dlg.FileName;
                Workbench.Log("Update File: " + DataManager.UpdateFilePath);
            }
        }

        private void m_start_service_btn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(DataManager.UpdateFilePath))
            {
                MessageBox.Show("Please select the update file path before start service.");
                return;
            }

            if (string.IsNullOrEmpty(m_version_tbx.Text))
            {
                MessageBox.Show("Please specify the data version.");
                return;
            }

            NetServer.Start();
        }

        private void m_version_tbx_TextChanged(object sender, EventArgs e)
        {
            DataManager.Version = m_version_tbx.Text.Trim();
        }

        private void m_clear_information_Click(object sender, EventArgs e)
        {
            m_information_listbox.Items.Clear();
        }

        private void m_update_file_path_txb_TextChanged(object sender, EventArgs e)
        {
            DataManager.UpdateFilePath = m_update_file_path_txb.Text;
        }

        private void Workbench_FormClosing(object sender, FormClosingEventArgs e)
        {
            Config.Save();
        }

        private void m_for_user_txb_TextChanged(object sender, EventArgs e)
        {
            DataManager.ForUser = m_for_user_txb.Text;
        }        
    }
}
