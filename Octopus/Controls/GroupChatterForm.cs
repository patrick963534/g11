using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Octopus.Core;
using Octopus.Net;
using System.Runtime.InteropServices;

namespace Octopus.Controls
{
    public partial class GroupChatterForm : Form
    {
        [DllImport("user32.dll")]
        public static extern bool FlashWindow(IntPtr hWnd, bool bInvert);

        private GroupInfo m_group;
        private bool m_isActive;

        public GroupChatterForm(GroupInfo gi)
        {
            InitializeComponent();
            Show();

            StartPosition = FormStartPosition.CenterScreen;
            m_group = gi;
            m_msg_input_tbx.Focus();

            m_user_list.Items.AddRange(m_group.GetUserArray());
            QueryGroupUsers();
            ShowMessage();
        }

        public void AddUser(UserInfo user)
        {
            m_user_list.Items.Add(user);
        }

        public void ShowMessage()
        {
            this.Invoke(new DoAction(delegate
            {
                m_msg_show_tbx.Text = m_group.Messages;

                m_msg_show_tbx.SelectionStart = m_msg_show_tbx.Text.Length;
                m_msg_show_tbx.ScrollToCaret();

                if (!m_isActive)
                {
                    FlashWindow(this.Handle, true);
                }
            }));
        }

        private void GroupChatterForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_group.Chatter = null;
        }

        public void QueryGroupUsers()
        {
            UserInfo[] users = UserInfoManager.GetUserArray();
            foreach (UserInfo usr in users)
            {
                OutgoingPackagePool.AddFirst(NetPackageGenerater.FindGroupUser(m_group.Key, usr.RemoteIP));
            }
        }

        private void m_msg_input_tbx_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.Control && e.KeyCode == Keys.Enter)
            {
                string msg = m_msg_input_tbx.Text.Trim();
                if (string.IsNullOrEmpty(msg))
                    return;

                m_msg_input_tbx.Text = string.Empty;
                msg = Helper.FormatMessage(DataManager.WhoAmI, msg);
                m_group.AppendMessage(msg);

                foreach (UserInfo user in m_user_list.Items)
                {
                    OutgoingPackagePool.Add(NetPackageGenerater.AppendGroupTextMessage(m_group.Key, msg, user.RemoteIP));
                }

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.Alt && e.KeyCode == Keys.C)
            {
                Close();
            }
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
