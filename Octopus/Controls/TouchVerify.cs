using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Octopus.Base
{
    public partial class TouchVerify : Form
    {
        private static TouchVerify m_singleton;
        private static Color m_keycolor = Color.Gray;
        private static Font m_font = new Font("Arial", 24);
        private static string m_touch_tip_file = "m_touch_tip";
        private static Image m_touch_tip;

        static TouchVerify()
        {
            m_singleton = new TouchVerify();
        }

        public TouchVerify()
        {
            InitializeComponent();

            this.Size = SystemInformation.VirtualScreen.Size;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = m_keycolor;
            this.TransparencyKey = m_keycolor;

            if (File.Exists(m_touch_tip_file))
                m_touch_tip = Image.FromFile(m_touch_tip_file);
        }

        public static bool Tell()
        {
            if (m_singleton.Visible)
                return false;

            Workbench.DoAction(new Action(delegate{ m_singleton.Show(); }));

            return true;
        }

        private void TouchVerify_KeyDown(object sender, KeyEventArgs e)
        {
            m_singleton.Hide();
        }

        private void TouchVerify_Click(object sender, EventArgs e)
        {
            m_singleton.Hide();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            Bitmap img = new Bitmap(this.Size.Width, this.Size.Height);
            Graphics g = Graphics.FromImage(img);
            g.FillRectangle(new SolidBrush(m_keycolor), new Rectangle(0, 0, img.Width, img.Height));

            if (m_touch_tip == null)
            {
                g.FillRectangle(Brushes.Green, new Rectangle(img.Width / 2 - 300, img.Height / 2 - 100, 600, 200));
                g.DrawString("打卡时间到了!!", m_font, Brushes.Black, new Point(img.Width / 2, img.Height / 2), format);
            }
            else
            {
                g.DrawImage(m_touch_tip, (img.Width - m_touch_tip.Width) / 2, (img.Height - m_touch_tip.Height) / 2);
            }

            e.Graphics.DrawImage(img, 0, 0);

            g.Dispose();
            img.Dispose();
        }
    }
}
