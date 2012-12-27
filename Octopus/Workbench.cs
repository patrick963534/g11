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
    public delegate void DoAction();

    public partial class Workbench : Form
    {
        private static Workbench s_singleton;

        private bool m_really_close;
        private TrayFlashTimer m_trayFlashTimer;

        public Workbench()
        {
            InitializeComponent();
            Show();
        }

        private void Workbench_Load(object sender, EventArgs e)
        {
            s_singleton = this;
            m_trayFlashTimer = new TrayFlashTimer(m_tray);
            StartPosition = FormStartPosition.Manual;
            Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - 300, 50);

            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            TouchVerifyCore.Start();
            NetService.Start();
            Logger.WriteLine(DataManager.Version);
        }

        void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
        }

        public static void NotifyUpdateLog()
        {
            s_singleton.Invoke(new DoAction(delegate
            {
                LogViewer.UpdateMsg();
            }));
        }

        public static void DoAction(DoAction action)
        {
            s_singleton.Invoke(action);
        }

        public static void AddClient(string name, IPEndPoint remoteIP)
        {
            if (UserInfoManager.FindUser(remoteIP) != null)
                return;

            if (name == DataManager.WhoAmI)
                return;

            s_singleton.Invoke(new DoAction(delegate
            {
                IPAddress addr = remoteIP.Address;
                int port = remoteIP.Port;

                Logger.WriteLine(string.Format("Add User: {0}, IP: {1}", name, remoteIP));

                UserInfo user = new UserInfo(new IPEndPoint(addr, port), name);
                UserInfoManager.AddUser(user);

                s_singleton.m_users.AddUser(user);
            }));
        }

        public static void GroupAddUser(GroupInfo group, UserInfo user)
        {
            if (group == null || user == null)
                return;

            s_singleton.Invoke(new DoAction(delegate
            {
                Logger.WriteLine(string.Format("Group: {0}, Add user: {1}", group.Name, user.Username));

                group.AddUser(user);
            }));
        }

        public static void AddGroup(string groupKey, string name)
        {
            if (string.IsNullOrEmpty(groupKey) || string.IsNullOrEmpty(name))
                return;

            s_singleton.Invoke(new DoAction(delegate
            {
                Logger.WriteLine(string.Format("Add Group: {0}. with key: {1}", name, groupKey));

                GroupInfo gi = new GroupInfo(groupKey, name);
                GroupInfoManager.AddGroup(gi);
                s_singleton.m_groups.AddGroup(gi);
            }));
        }

        public static void QueryGroupUser(GroupInfo group)
        {
            if (group == null)
                return;

            s_singleton.Invoke(new DoAction(delegate
            {
                Logger.WriteLine(string.Format("Query Group Users, group name: {0}. with key: {1}", group.Name, group.Key));

                group.QueryGroupUsers();
            }));
        }

        public static void QuitGroup(GroupInfo group)
        {
            if (group == null)
                return;

            s_singleton.Invoke(new DoAction(delegate
            {
                Logger.WriteLine(string.Format("Quit Group: {0}. with key: {1}", group.Name, group.Key));

                GroupInfoManager.DeleteGroup(group);
                s_singleton.m_groups.DeleteGroup(group);
            }));
        }

        public static void ExitForm()
        {
            s_singleton.Invoke(new DoAction(delegate
            {
                GroupConfig.Save();

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
                if (m_trayFlashTimer.FlashingUser != null)
                {
                    m_trayFlashTimer.FlashingUser.ShowChatter();
                }
                else if (m_trayFlashTimer.FlashingGroup != null)
                {
                    m_trayFlashTimer.FlashingGroup.ShowChatter();
                }
                else
                {
                    s_singleton.Show();
                    s_singleton.Activate();                    
                }                
            }
            else if (mouse_e.Button == MouseButtons.Right)
            {
                s_singleton.Activate();
                if (MessageBox.Show("是否退出章鱼?", "Octopus", MessageBoxButtons.YesNo) == DialogResult.Yes)
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

        private void m_add_client_btn_Click(object sender, EventArgs e)
        {
            OutgoingPackagePool.AddFirst(NetPackageGenerater.BroadcastFindUser());
        }

        private void m_logger_btn_Click(object sender, EventArgs e)
        {
            LogViewer.MakeValid();
            NotifyUpdateLog();
        }

        private class TrayFlashTimer
        {
            private Timer m_timer;
            private NotifyIcon m_tray;
            private bool m_state;
            private UserInfo m_flashingUser;
            private GroupInfo m_flashingGroup;

            public TrayFlashTimer(NotifyIcon tray)
            {
                m_timer = new Timer();
                m_timer.Interval = 500;
                m_timer.Tick += new EventHandler(m_timer_Tick);
                m_timer.Start();

                m_tray = tray;
            }

            public UserInfo FlashingUser
            {
                get { return m_flashingUser; }
            }

            public GroupInfo FlashingGroup
            {
                get { return m_flashingGroup; }
            }

            void m_timer_Tick(object sender, EventArgs e)
            {
                m_timer.Start();

                if (m_flashingUser != null && !m_flashingUser.IsReceiveNewMessage)
                {
                    m_flashingUser = null;
                }

                if (m_flashingGroup != null && !m_flashingGroup.IsReceiveNewMessage)
                {
                    m_flashingGroup = null;
                }

                if (m_flashingUser == null)
                {
                    m_flashingUser = UserInfoManager.FindUserWhichHaveNewMessage();
                }

                if (m_flashingUser == null)
                {
                    m_flashingGroup = GroupInfoManager.FindGroupWhichHaveNewMessage();
                }
               
                if (m_flashingUser == null && m_flashingGroup == null)
                {
                    m_tray.Icon = IconManager.Tray_Normal;
                    return;
                }
                else
                {
                    m_state = !m_state;

                    if (m_state)
                        m_tray.Icon = IconManager.Tray_Normal;
                    else
                        m_tray.Icon = IconManager.Tray_Blank;
                }
            }
        }
    }
}