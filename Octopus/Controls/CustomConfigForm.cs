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
    public partial class CustomConfigForm : Form
    {
        private static CustomConfigForm s_singleton;

        public CustomConfigForm()
        {
            InitializeComponent();

            m_displayName.Text = CustomConfigure.DisplayName;
        }

        public static void PopShow()
        {
            if (s_singleton == null)
                s_singleton = new CustomConfigForm();

            s_singleton.Show();
            s_singleton.Activate();
            s_singleton.WindowState = FormWindowState.Normal;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            s_singleton = null;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            CustomConfigure.SetDisplayName(m_displayName.Text);
            CustomConfigure.Save();

            Close();
        }
    }
}
