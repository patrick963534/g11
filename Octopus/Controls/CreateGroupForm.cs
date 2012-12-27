using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Octopus.Controls
{
    public partial class CreateGroupForm : Form
    {
        public string GroupName
        {
            get { return textBox1.Text; }
        }

        public CreateGroupForm()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                OK_Click(sender, e);
            }
        }
    }
}
