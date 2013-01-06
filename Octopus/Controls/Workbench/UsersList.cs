using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Octopus.Core;
using Octopus.Net;

namespace Octopus.Controls
{
    public partial class UsersList : UserControl
    {
        public UsersList()
        {
            InitializeComponent();
        }

        public void AddUser(UserInfo user)
        {
            m_users_list.Items.Add(user);
        }

        private void m_users_list_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                UserInfo user = (UserInfo)m_users_list.SelectedItem;
                if (user != null)
                    this.ContextMenuStrip = new UserContextMenu(user);
            }
        }

        private void m_users_list_DoubleClick(object sender, EventArgs e)
        {
            UserInfo user = (UserInfo)m_users_list.SelectedItem;
            if (user == null)
                return;

            user.ShowChatter();
        }

        private class UserContextMenu : ContextMenuStrip
        {
            private UserInfo m_user;

            public UserContextMenu(UserInfo user)
            {
                this.Items.Add("JoinGroup");
                this.ItemClicked += new ToolStripItemClickedEventHandler(GroupListBoxContextMenu_ItemClicked);

                m_user = user;
            }

            void GroupListBoxContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
            {
                if (e.ClickedItem.Text == "JoinGroup")
                {
                    GroupSelectionForm dlg = new GroupSelectionForm();
                    dlg.StartPosition = FormStartPosition.CenterParent;
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        GroupInfo group = dlg.SelectedGroup;

                        if (!group.ContainsUser(m_user))
                            group.AddUser(m_user);

                        OutgoingPackagePool.AddFirst(NetPackageGenerater.CreateNewGroup(group.Key, group.Name, m_user.RemoteIP));

                        UserInfo[] users = group.GetUserArray();
                        foreach (UserInfo usr in users)
                        {
                            OutgoingPackagePool.Add(NetPackageGenerater.CheckGroupUserCount(group.Key, users.Length, usr.RemoteIP));
                        }
                    }
                }
            }
        }
    }
}
