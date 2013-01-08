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
    public partial class RawMessageForm : Form
    {
        public RawMessageForm(MessageStore messages)
        {
            InitializeComponent();

            int count = messages.MsgCount;
            for (int i = 0; i < count; i++)
            {
                listBox1.Items.Add(messages.GetFormmattedMsg(i));
            }
        }
    }
}
