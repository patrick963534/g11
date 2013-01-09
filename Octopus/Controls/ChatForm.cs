using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Octopus.Net;
using System.Net;
using Octopus.Core;
using System.Runtime.InteropServices;
using System.IO;

namespace Octopus.Controls
{
    public partial class ChatForm : Form
    {
        [DllImport("user32.dll")]
        public static extern bool FlashWindow(IntPtr hWnd, bool bInvert);   

        private UserInfo m_userinfo;
        private bool m_isActive;

        public ChatForm(UserInfo userinfo)
        {
            InitializeComponent();

            Show();

            StartPosition = FormStartPosition.CenterScreen;
            m_userinfo = userinfo;
            m_msg_input_tbx.Focus();

            this.Text = string.Format("聊天对象: {0}", userinfo.Username);

            UpdateMessage();
        }

        public void PopShow()
        {
            Show();
            Activate();
            WindowState = FormWindowState.Normal;
        }

        public void UpdateMessage()
        {
            this.Invoke(new DoAction(delegate
            {
                msgRichViewer1.UpdateMessages(m_userinfo.MessageStore);

                if (!m_isActive)
                {
                    FlashWindow(this.Handle, true); 
                }
            }));
        }

        private void m_msg_input_tbx_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.Shift && e.KeyCode == Keys.Enter)
            {
                string msg = m_msg_input_tbx.Text.Trim();
                if (string.IsNullOrEmpty(msg))
                    return;

                msg = MsgInputConfig.FormatMessage(msg);
                m_msg_input_tbx.Text = string.Empty;

                m_userinfo.AppendMessage(msg, DataManager.WhoAmI);
                OutgoingPackagePool.Add(NetPackageGenerater.AppendTextMessage(msg, m_userinfo.RemoteIP));

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.Alt && e.KeyCode == Keys.C)
            {
                Close();
            }
        }

        private void ChatForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_userinfo.ExitChatter();
        }

        protected override void OnDeactivate(EventArgs e)
        {
            m_isActive = false;
            base.OnDeactivate(e);
        }

        protected override void OnActivated(EventArgs e)
        {
            m_isActive = true;
            base.OnActivated(e);
        }

        private void m_sendImage_btn_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Images(*.gif,*.png,*.jpg)|*.gif;*.png;*.jpg";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                m_userinfo.AppendMessage(MsgInputConfig.FormatImageMessage(dlg.FileName), DataManager.WhoAmI);
                OutgoingPackagePool.AddFirst(NetPackageGenerater.AppendImageMessage(dlg.FileName, m_userinfo.RemoteIP));
            }            
        }

        private void m_customFace_btn_Click(object sender, EventArgs e)
        {
            CustomFaceForm form = new CustomFaceForm();
            form.StartPosition = FormStartPosition.Manual;
            form.SelectItem += new EventHandler<EventArgs>(select_customface);
            form.ShowIt(this);
        }

        private void select_customface(object sender, EventArgs e)
        {
            CustomFaceForm form = (CustomFaceForm)sender;

            if (form.CustomFaceItem != null)
            {
                string path = Path.Combine(DataManager.GetCustomFaceFolderPath(), form.CustomFaceItem.Filename);
                m_userinfo.AppendMessage(MsgInputConfig.FormatImageMessage(path), DataManager.WhoAmI);
                OutgoingPackagePool.AddFirst(NetPackageGenerater.AppendImageMessage(path, m_userinfo.RemoteIP));
            }
        }

        private void m_screenShot_btn_Click(object sender, EventArgs e)
        {
            ScreenShotForm form = new ScreenShotForm();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.SendImage += new EventHandler<EventArgs>(send_screenshot);
            form.Show();
        }

        private void send_screenshot(object sender, EventArgs e)
        {
            ScreenShotForm form = (ScreenShotForm)sender;

            if (form.ImagePath != null)
            {
                m_userinfo.AppendMessage(MsgInputConfig.FormatImageMessage(form.ImagePath), DataManager.WhoAmI);
                OutgoingPackagePool.AddFirst(NetPackageGenerater.AppendImageMessage(form.ImagePath, m_userinfo.RemoteIP));
            }
        }

        private void m_rawMessage_btn_Click(object sender, EventArgs e)
        {
            RawMessageForm form = new RawMessageForm(m_userinfo.MessageStore);
            form.Show();
        }
    }
}
