using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Octopus.Core;
using Octopus.Commands;
using Octopus.Net;
using Octopus.Controls;
using System.Net;

namespace Octopus
{
    internal delegate void DoAction();

    internal partial class Workbench : Form
    {
        private static Workbench s_singleton;

        private bool m_really_close;

        internal Workbench()
        {
            InitializeComponent();
            s_singleton = this;

            // Call the Show() method to force create Window Handler in the insternal.
            // Otherwise we can't use Invoke() method.
            this.Show();
            
            TouchVerifyCore.Start();
            NetService.Start();

            Workbench.Log(DataManager.Version);
        }

        internal static void HideIt()
        {
            s_singleton.Hide();
        }

        internal static void Log(string msg)
        {
            s_singleton.Invoke(new DoAction(delegate
            {
                if (!string.IsNullOrEmpty(s_singleton.m_msg_show_tbx.Text))
                    s_singleton.m_msg_show_tbx.Text += "\r\n";

                s_singleton.m_msg_show_tbx.Text += msg;
            }));
        }

        internal static void DoAction(DoAction action)
        {
            s_singleton.Invoke(action);
        }

        internal static void AddClient(string name, IPEndPoint remoteIP)
        {
            if (UserInfoManager.FindUser(remoteIP) != null)
                return;

            if (name == Dns.GetHostName())
                return;

            s_singleton.Invoke(new DoAction(delegate
            {
                IPAddress addr = remoteIP.Address;
                int port = remoteIP.Port;

                Workbench.Log(string.Format("Add User: {0}, IP: {1}", name, remoteIP));

                UserInfo user = new UserInfo(new IPEndPoint(addr, port), name);
                UserInfoManager.AddUser(user);
                s_singleton.m_users_list.Items.Add(user);
            }));
        }

        internal static void ExitForm()
        {
            s_singleton.Invoke(new DoAction(delegate
            {
                s_singleton.m_really_close = true;
                s_singleton.m_tray.Visible = false;
                NetService.Stop();
                TouchVerifyCore.Stop();
                Application.Exit();
            }));
        }

        private void m_tray_Click(object sender, EventArgs e)
        {
            MouseEventArgs mouse_e = (MouseEventArgs)e;

            if (mouse_e.Button == MouseButtons.Left)
            {
                s_singleton.Show();
                s_singleton.Activate();
            }
            else if (mouse_e.Button == MouseButtons.Right)
            {
                //if (MessageBox.Show("是否退出八爪鱼?", "Octopus", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ExitForm();
                }
            }
        }

        private void Workbench_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!s_singleton.m_really_close)
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        private void m_users_list_DoubleClick(object sender, EventArgs e)
        {
            UserInfo user = (UserInfo)m_users_list.SelectedItem;

            if (user.Chatter == null)
                user.Chatter = new ChatForm(user);

            user.Chatter.Show();
        }

        private void m_add_client_btn_Click(object sender, EventArgs e)
        {
            OutgoingPackagePool.AddFirst(NetPackageGenerater.BroadcastFindUser());
        }
    }
}
