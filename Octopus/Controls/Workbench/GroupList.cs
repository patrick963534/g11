using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Octopus.Controls.Workbench
{
    public partial class GroupList : UserControl
    {
        public GroupList()
        {
            InitializeComponent();
        }

        private void m_list_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.ContextMenuStrip = new GroupListBoxContextMenu();
            }
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
                        break;
                }
            }
        }

        private class ItemContextMenu : ContextMenuStrip
        {
            public ItemContextMenu()
            {
                this.Items.Add("QuitGroup");
                this.ItemClicked += new ToolStripItemClickedEventHandler(ItemContextMenu_ItemClicked);
            }

            void ItemContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
            {

                switch (e.ClickedItem.Text)
                {
                    case "QuitGroup":
                        break;
                }
            }
        }
    }
}
