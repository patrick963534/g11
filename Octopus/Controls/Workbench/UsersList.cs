using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Octopus.Core;

namespace Octopus.Controls.Workbench
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

        private void m_users_list_DoubleClick(object sender, EventArgs e)
        {
            UserInfo user = (UserInfo)m_users_list.SelectedItem;
            if (user == null)
                return;

            user.ShowChatter();
        }
    }
}
