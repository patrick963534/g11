﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Octopus.Net;
using System.Net;
using Octopus.Core;
using System.Runtime.InteropServices;

namespace Octopus.Controls
{
    public partial class ChatForm : Form
    {
        [DllImport("user32.dll")]
        public static extern bool FlashWindow(IntPtr hWnd, bool bInvert);   

        private UserInfo m_userinfo;
        private bool m_isActive;

        public ChatForm(UserInfo userinfo)
        {
            InitializeComponent();

            Show();

            StartPosition = FormStartPosition.CenterScreen;
            m_userinfo = userinfo;
            m_msg_input_tbx.Focus();

            this.Text = string.Format("聊天对象: {0}", userinfo.Username);

            ShowMessage();
        }

        public void ShowMessage()
        {
            this.Invoke(new DoAction(delegate
            {
                m_msg_show_tbx.Text = m_userinfo.Messages;

                m_msg_show_tbx.SelectionStart = m_msg_show_tbx.Text.Length;
                m_msg_show_tbx.ScrollToCaret();

                if (!m_isActive)
                {
                    FlashWindow(this.Handle, true); 
                }
            }));
        }

        private void m_msg_input_tbx_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.Control && e.KeyCode == Keys.Enter)
            {
                string msg = m_msg_input_tbx.Text.Trim();
                if (string.IsNullOrEmpty(msg))
                    return;

                msg = Helper.FormatMessage(DataManager.WhoAmI, msg);
                m_userinfo.AppendMessage(msg);

                OutgoingPackagePool.Add(NetPackageGenerater.AppendTextMessage(msg, m_userinfo.RemoteIP));
                m_msg_input_tbx.Text = string.Empty;
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.Alt && e.KeyCode == Keys.C)
            {
                Close();
            }
        }

        private void ChatForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_userinfo.Chatter = null;
        }

        protected override void OnDeactivate(EventArgs e)
        {
            m_isActive = false;
            base.OnDeactivate(e);
        }

        protected override void OnActivated(EventArgs e)
        {
            m_isActive = true;
            base.OnActivated(e);
        }
    }
}
