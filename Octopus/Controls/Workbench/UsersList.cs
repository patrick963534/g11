﻿using System;
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

        public void AddSelectedUserToGroup(GroupInfo group)
        {
            UserInfo user = (UserInfo)m_users_list.SelectedItem;

            if (user != null)
            {
                if (!group.ContainsUser(user))
                {
                    group.AddUser(user);
                    OutgoingPackagePool.AddFirst(NetPackageGenerater.CreateNewGroup(group.Key, group.Name, user.RemoteIP));
                }
            }
        }

        private void m_users_list_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.ContextMenuStrip = new UserContextMenu(this);
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
            private UsersList m_owner;

            public UserContextMenu(UsersList owner)
            {
                this.Items.Add("JoinGroup");
                this.ItemClicked += new ToolStripItemClickedEventHandler(GroupListBoxContextMenu_ItemClicked);

                m_owner = owner;
            }

            void GroupListBoxContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
            {
                switch (e.ClickedItem.Text)
                {
                    case "JoinGroup":
                        GroupSelectionForm dlg = new GroupSelectionForm();
                        dlg.StartPosition = FormStartPosition.CenterParent;
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            m_owner.AddSelectedUserToGroup(dlg.SelectedGroup);
                        }
                        break;
                }
            }
        }
    }
}