using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Octopus.Core;
using System.IO;
using Octopus.Net;

namespace Octopus.Controls
{
    public partial class CustomFaceForm : Form
    {
        private int StartX = 5;
        private int StartY = 5;
        private int LineItemCount = 11;
        private int LineCount = 5;

        private int m_pageIndex;
        private int m_preview_idx = -1;
        private bool m_avoidclose;

        private CustomFaceItem m_item;

        public EventHandler<EventArgs> SelectItem;

        public CustomFaceForm()
        {
            InitializeComponent();
            m_preview_pbx.Visible = false;
        }

        public CustomFaceItem CustomFaceItem
        {
            get { return m_item; }
        }

        public void ShowIt(Control control)
        {
            int x = control.Width + control.Location.X - Width - 50;
            int y = control.Height + control.Location.Y - Height - 100;
            Location = new Point(x, y);

            Show();
        }

        private void m_nextPage_btn_Click(object sender, EventArgs e)
        {
            int count = CustomFaceManager.GetItemCount();

            if ((m_pageIndex + 1) * LineItemCount * LineCount < count)
                m_pageIndex++;

            this.Invalidate();
        }

        private void m_previousPage_btn_Click(object sender, EventArgs e)
        {
            if (m_pageIndex > 0)
            {
                m_pageIndex--;
                this.Invalidate();
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            CustomFaceManager.Dispose();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            int bx = ((e.X - StartX)) / CustomFaceManager.IconSize;
            int by = ((e.Y - StartY)) / CustomFaceManager.IconSize;

            int index = by * LineItemCount + bx;
            if (bx < LineItemCount && by < LineCount && index < LineCount * LineItemCount)
            {
                index += m_pageIndex * LineItemCount * LineCount;

                if (m_pageIndex == index)
                    return;

                CustomFaceItem item = CustomFaceManager.GetItem(index);

                if (item != null)
                {
                    if (m_preview_pbx.Image != null)
                        m_preview_pbx.Image.Dispose();

                    string path = Path.Combine(DataManager.GetCustomFaceFolderPath(), item.Filename);
                    Bitmap img = new Bitmap(path);
                    m_preview_pbx.Image = img;

                    m_preview_idx = index;
                    m_preview_pbx.Visible = true;

                    if (img.Width < m_preview_pbx.Width && img.Height < m_preview_pbx.Height)
                        m_preview_pbx.SizeMode = PictureBoxSizeMode.CenterImage;
                    else
                        m_preview_pbx.SizeMode = PictureBoxSizeMode.Zoom;

                    if (bx > LineItemCount / 2)
                        m_preview_pbx.Location = new Point(0, 0);
                    else
                        m_preview_pbx.Location = new Point(this.Size.Width - m_preview_pbx.Size.Width, 0);
                }
            }
            else
            {
                m_preview_idx = -1;
                m_preview_pbx.Visible = false;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int count = CustomFaceManager.GetItemCount();
            int iconSz = CustomFaceManager.IconSize;

            for (int j = 0; j < LineCount; j++)
            {
                for (int i = 0; i < LineItemCount; i++)
                {
                    int idx = j * LineItemCount + i;
                    idx += m_pageIndex * LineItemCount * LineCount;

                    if (idx >= count)
                        return;

                    Image img = CustomFaceManager.GetItem(idx).Icon;
                    e.Graphics.DrawImage(img, StartX + i * iconSz, StartY + j * iconSz);
                    e.Graphics.DrawRectangle(Pens.Black, new Rectangle(StartX + i * iconSz, StartY + j * iconSz, iconSz, iconSz));
                }
            }
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);

            if (!m_avoidclose)
                Close();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);

            if (!m_avoidclose)
                Close();
        }

        private void CustomFaceForm_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            int bx = ((me.X - StartX)) / CustomFaceManager.IconSize;
            int by = ((me.Y - StartY)) / CustomFaceManager.IconSize;

            int index = by * LineItemCount + bx;
            if (bx < LineItemCount && by < LineCount && index < LineCount * LineItemCount)
            {
                index += m_pageIndex * LineItemCount * LineCount;
                m_item = CustomFaceManager.GetItem(index);

                if (SelectItem != null)
                    SelectItem(this, e);

                if (me.Button == MouseButtons.Left)
                    Close();
            }
        }

        private void m_cancel_btn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void m_add_btn_Click(object sender, EventArgs e)
        {
            m_avoidclose = true;
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = true;
            dlg.Filter = "Images(*.gif,*.png,*.jpg)|*.gif;*.png;*.jpg|所有文件(*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in dlg.FileNames)
                {
                    CustomFaceManager.AddCustomFace(file);
                }
                
                this.Invalidate();
            }
            m_avoidclose = false;
        }
    }
}
