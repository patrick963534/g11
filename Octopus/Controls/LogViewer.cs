﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Octopus.Core;
using Octopus.Net;

namespace Octopus.Controls
{
    public partial class LogViewer : Form
    {
        private static LogViewer s_singleton;

        private Timer m_timer;
        private static int m_index;

        public LogViewer()
        {
            InitializeComponent();

            m_timer = new Timer();
            m_timer.Interval = 1000;
            m_timer.Tick += new EventHandler(m_timer_Tick);
            m_timer.Start();
        }

        void m_timer_Tick(object sender, EventArgs e)
        {
            m_timer.Start();

            m_packagesInQueue_tbx.Text = OutgoingPackagePool.GetUnprocessedPackageCount().ToString();
            m_processedPackagesInPool_tbx.Text = OutgoingPackagePool.GetProcessedPackageInPoolCount().ToString();
        }

        public bool IsReady
        {
            get { return s_singleton != null; }
        }

        public static void UpdateMsg()
        {
            if (s_singleton != null)
            {
                int max = 100 * 1000;
                if (s_singleton.listBox1.Items.Count > max)
                {
                    s_singleton.listBox1.Items.Clear();
                    m_index -= max / 2;
                }

                List<string> items = Logger.GetMessageByIdx(ref m_index);
                if (items != null)
                {
                    s_singleton.listBox1.Items.AddRange(items.ToArray());
                    m_index += items.Count;
                }
            }            
        }

        public static void MakeValid()
        {
            if (s_singleton == null)
            {
                s_singleton = new LogViewer();
            }

            s_singleton.Show();
            s_singleton.Activate();
        }

        private void m_cmdCounter_btn_Click(object sender, EventArgs e)
        {
            m_extraMessage_tbx.Text += "\r\n";
            m_extraMessage_tbx.Text += "**********Recv msg counter********** \r\n";
            m_extraMessage_tbx.Text += Logger.Get_Recv_CounterCommandMsg();
            m_extraMessage_tbx.Text += Logger.Get_Recv_Part_Packages();

            m_extraMessage_tbx.Text += "**********Send msg counter********** \r\n";
            m_extraMessage_tbx.Text += Logger.Get_Send_CounterCommandMsg();
            m_extraMessage_tbx.Text += Logger.Get_Dead_Pkg_Counter();

            m_extraMessage_tbx.SelectionStart = m_extraMessage_tbx.Text.Length;
            m_extraMessage_tbx.ScrollToCaret();
        }

        private void LogViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            s_singleton = null;
            m_index = 0;
        }
    }
}
