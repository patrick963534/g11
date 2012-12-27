using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Octopus.Core;

namespace Octopus.Controls
{
    public partial class GroupSelectionForm : Form
    {
        public GroupSelectionForm()
        {
            InitializeComponent();

            comboBox1.Items.AddRange(GroupInfoManager.GetGroupArray());
        }

        public GroupInfo SelectedGroup
        {
            get { return (GroupInfo)comboBox1.SelectedItem; }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
