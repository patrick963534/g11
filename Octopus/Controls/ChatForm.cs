using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Octopus.Net;
using System.Net;
using Octopus.Core;

namespace Octopus.Controls
{
    internal partial class ChatForm : Form
    {
        private UserInfo m_userinfo;

        public ChatForm(UserInfo userinfo)
        {
            InitializeComponent();

            Show();

            m_userinfo = userinfo;
            m_msg_input_tbx.Focus();

            ShowMessage();
        }

        public void ShowMessage()
        {
            this.Invoke(new DoAction(delegate
            {
                m_msg_show_tbx.Text = m_userinfo.Messages;
            }));
        }

        private void m_msg_input_tbx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Enter)
            {
                string msg = m_msg_input_tbx.Text.Trim();
                if (string.IsNullOrEmpty(msg))
                    return;

                m_userinfo.AppendMessage(msg);
                OutgoingPackagePool.Add(NetPackageGenerater.AppendTextMessage(msg, m_userinfo.RemoteIP));
                m_msg_input_tbx.Text = string.Empty;
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void ChatForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_userinfo.Chatter = null;
        }
    }
}
