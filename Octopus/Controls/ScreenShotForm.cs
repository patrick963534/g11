using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Octopus.Core;
using System.IO;
using System.Drawing.Imaging;
using Octopus.Core.CustomFace;

namespace Octopus.Controls
{
    public partial class ScreenShotForm : Form
    {
        private Bitmap m_image;
        private Point m_start;
        private Point m_end;
        private bool m_dragging;
        private bool m_finish;
        private string m_path;

        public EventHandler<EventArgs> SendImage;

        public ScreenShotForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            Size = Screen.PrimaryScreen.WorkingArea.Size;

            Timer timer = new Timer();
            timer.Interval = 80;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

            UpateImage();
        }

        public string ImagePath
        {
            get { return m_path; }
        }

        private void UpateImage()
        {
            if (m_image != null)
                m_image.Dispose();

            m_image = new Bitmap(Size.Width, Size.Height);
            Graphics g = Graphics.FromImage(m_image);
            g.CopyFromScreen(0, 0, 0, 0, Size);
            g.Dispose();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (!m_finish && e.Button == MouseButtons.Left)
            {
                m_dragging = true;
                m_start = new Point(e.X, e.Y);
                m_end = new Point(e.X, e.Y);
            }
            else if (e.Button == MouseButtons.Right)
            {
                Close();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (m_dragging)
            {
                m_end = e.Location;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            m_dragging = false;
            m_finish = true;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            MouseEventArgs me = (MouseEventArgs)e;

            if (me.Button == MouseButtons.Left)
            {
                int minx = Math.Min(m_start.X, m_end.X);
                int miny = Math.Min(m_start.Y, m_end.Y);
                int maxx = Math.Max(m_start.X, m_end.X);
                int maxy = Math.Max(m_start.Y, m_end.Y);

                if (me.X >= minx && me.X <= maxx &&
                    me.Y >= miny && me.Y <= maxy &&
                    minx != maxx && miny != maxy)
                {
                    Bitmap img = new Bitmap(maxx - minx, maxy - miny);
                    Graphics g = Graphics.FromImage(img);
                    g.DrawImage(m_image, new Rectangle(0, 0, img.Width, img.Height), 
                        new Rectangle(minx, miny, img.Width, img.Height), GraphicsUnit.Pixel);

                    m_path = Path.Combine(DataManager.GetCustomFaceFolderPath(), Guid.NewGuid().ToString());
                    m_path = m_path.Replace("-", "");
                    m_path = Path.ChangeExtension(m_path, ".png");
                    img.Save(m_path, ImageFormat.Png);

                    CustomFaceManager.AddCustomFace(m_path);

                    if (SendImage != null)
                        SendImage(this, null);

                    Close();
                }
            }

            m_finish = false;
            m_start = new Point();
            m_end = new Point();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.DrawImage(m_image, 0, 0);

            int minx = Math.Min(m_start.X, m_end.X);
            int miny = Math.Min(m_start.Y, m_end.Y);
            int maxx = Math.Max(m_start.X, m_end.X);
            int maxy = Math.Max(m_start.Y, m_end.Y);

            if (minx != maxx && miny != maxy)
            {
                e.Graphics.DrawRectangle(Pens.Black, minx, miny, maxx - minx, maxy - miny);
            }            
        }
    }
}
