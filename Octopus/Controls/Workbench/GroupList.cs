using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Octopus.Core;

namespace Octopus.Controls
{
    public partial class GroupList : UserControl
    {
        public GroupList()
        {
            InitializeComponent();

            GroupConfig.Load();
            m_list.Items.AddRange(GroupInfoManager.GetGroupArray());
        }

        private void m_list_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (m_list.SelectedItem == null)
                    this.ContextMenuStrip = new GroupListBoxContextMenu();
                else
                    this.ContextMenuStrip = new ItemContextMenu((GroupInfo)m_list.SelectedItem);
            }
        }

        public void DeleteGroup(GroupInfo gi)
        {
            m_list.Items.Remove(gi);
        }

        public void AddGroup(GroupInfo gi)
        {
            m_list.Items.Add(gi);
        }

        private void m_list_DoubleClick(object sender, EventArgs e)
        {
            GroupInfo gi = (GroupInfo)m_list.SelectedItem;
            if (gi == null)
                return;

            gi.ShowChatter();
        }

        private class GroupListBoxContextMenu : ContextMenuStrip
        {
            public GroupListBoxContextMenu()
            {
                this.Items.Add("CreateGroup");
                this.ItemClicked += new ToolStripItemClickedEventHandler(GroupListBoxContextMenu_ItemClicked);
            }

            void GroupListBoxContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
            {
                switch (e.ClickedItem.Text)
                {
                    case "CreateGroup":
                        CreateGroupForm dlg = new CreateGroupForm();
                        dlg.StartPosition = FormStartPosition.CenterParent;
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            GroupInfo info = GroupInfo.Create(dlg.GroupName);
                            Workbench.AddGroup(info.Key, info.Name);
                        }
                        break;
                }
            }
        }

        private class ItemContextMenu : ContextMenuStrip
        {
            private GroupInfo m_group;

            public ItemContextMenu(GroupInfo group)
            {
                m_group = group;

                this.Items.Add("QuitGroup");
                this.ItemClicked += new ToolStripItemClickedEventHandler(ItemContextMenu_ItemClicked);
            }

            void ItemContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
            {
                switch (e.ClickedItem.Text)
                {
                    case "QuitGroup":
                        string msg = string.Format("你确定要退出房间({0})么?", m_group.Name);
                        if (MessageBox.Show(msg, "Octopus", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            Workbench.QuitGroup(m_group);
                        
                        break;
                }
            }
        }
    }
}
