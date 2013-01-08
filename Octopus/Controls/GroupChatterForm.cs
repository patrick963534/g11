using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Octopus.Core;
using Octopus.Net;
using System.Runtime.InteropServices;
using System.IO;

namespace Octopus.Controls
{
    public partial class GroupChatterForm : Form
    {
        [DllImport("user32.dll")]
        public static extern bool FlashWindow(IntPtr hWnd, bool bInvert);

        private GroupInfo m_group;
        private bool m_isActive;

        public GroupChatterForm(GroupInfo gi)
        {
            InitializeComponent();
            Show();

            StartPosition = FormStartPosition.CenterScreen;
            m_group = gi;
            m_msg_input_tbx.Focus();

            m_user_list.Items.AddRange(m_group.GetUserArray());
            QueryGroupUsers();
            ShowMessage();

            this.Text = string.Format("{0} - 房间({1})", m_group.Name, m_group.Key);
        }

        public void DeleteUser(UserInfo user)
        {
            if (m_user_list.Items.Contains(user))
                m_user_list.Items.Remove(user);
        }

        public void AddUser(UserInfo user)
        {
            if (!m_user_list.Items.Contains(user))
                m_user_list.Items.Add(user);
        }

        public void ShowMessage()
        {
            this.Invoke(new DoAction(delegate
            {
                msgRichViewer1.UpdateMessages(m_group.MessageStore);

                if (!m_isActive)
                {
                    FlashWindow(this.Handle, true);
                }
            }));
        }

        private void GroupChatterForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_group.Chatter = null;
        }

        public void QueryGroupUsers()
        {
            UserInfo[] users = UserInfoManager.GetUserArray();
            foreach (UserInfo usr in users)
            {
                OutgoingPackagePool.AddFirst(NetPackageGenerater.FindGroupUser(m_group.Key, usr.RemoteIP));
            }
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

                foreach (UserInfo user in m_user_list.Items)
                {
                    OutgoingPackagePool.Add(NetPackageGenerater.AppendGroupTextMessage(m_group.Key, msg, user.RemoteIP));
                }

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.Alt && e.KeyCode == Keys.C)
            {
                Close();
            }
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

        private void m_user_list_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            UserInfo user = (UserInfo)m_user_list.SelectedItem;
            if (user == null)
                return;

            user.ShowChatter();
        }

        private void m_sendImage_btn_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Images(*.gif,*.png,*.jpg)|*.gif;*.png;*.jpg";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                byte[] imageData = File.ReadAllBytes(dlg.FileName);

                foreach (UserInfo user in m_user_list.Items)
                {
                    OutgoingPackagePool.Add(NetPackageGenerater.AppendGroupImageMessage(m_group.Key, dlg.FileName, imageData, user.RemoteIP));
                }
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
                byte[] imageData = File.ReadAllBytes(path);

                foreach (UserInfo user in m_user_list.Items)
                {
                    OutgoingPackagePool.Add(NetPackageGenerater.AppendGroupImageMessage(m_group.Key, path, imageData, user.RemoteIP));
                }
            }
        }

        private void m_refresh_bt_Click(object sender, EventArgs e)
        {
            QueryGroupUsers();
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
                string path = form.ImagePath;
                byte[] imageData = File.ReadAllBytes(path);

                foreach (UserInfo user in m_user_list.Items)
                {
                    OutgoingPackagePool.Add(NetPackageGenerater.AppendGroupImageMessage(m_group.Key, path, imageData, user.RemoteIP));
                }
            }
        }

        private void m_rawMessage_btn_Click(object sender, EventArgs e)
        {
            RawMessageForm form = new RawMessageForm(m_group.MessageStore);
            form.Show();
        }
    }
}
