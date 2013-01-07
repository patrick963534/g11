namespace Octopus.Controls
{
    partial class GroupChatterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.m_msg_input_tbx = new System.Windows.Forms.TextBox();
            this.m_user_list = new System.Windows.Forms.ListBox();
            this.m_sendImage_btn = new System.Windows.Forms.Button();
            this.m_customFace_btn = new System.Windows.Forms.Button();
            this.msgRichViewer1 = new Octopus.Controls.ChatControls.MsgRichViewer();
            this.m_refresh_bt = new System.Windows.Forms.Button();
            this.m_screenShot_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_msg_input_tbx
            // 
            this.m_msg_input_tbx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_msg_input_tbx.ImeMode = System.Windows.Forms.ImeMode.On;
            this.m_msg_input_tbx.Location = new System.Drawing.Point(12, 280);
            this.m_msg_input_tbx.Multiline = true;
            this.m_msg_input_tbx.Name = "m_msg_input_tbx";
            this.m_msg_input_tbx.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.m_msg_input_tbx.Size = new System.Drawing.Size(360, 97);
            this.m_msg_input_tbx.TabIndex = 5;
            this.m_msg_input_tbx.KeyDown += new System.Windows.Forms.KeyEventHandler(this.m_msg_input_tbx_KeyDown);
            // 
            // m_user_list
            // 
            this.m_user_list.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_user_list.FormattingEnabled = true;
            this.m_user_list.ItemHeight = 12;
            this.m_user_list.Location = new System.Drawing.Point(378, 26);
            this.m_user_list.Name = "m_user_list";
            this.m_user_list.Size = new System.Drawing.Size(146, 316);
            this.m_user_list.TabIndex = 6;
            this.m_user_list.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.m_user_list_MouseDoubleClick);
            // 
            // m_sendImage_btn
            // 
            this.m_sendImage_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_sendImage_btn.Location = new System.Drawing.Point(294, 253);
            this.m_sendImage_btn.Name = "m_sendImage_btn";
            this.m_sendImage_btn.Size = new System.Drawing.Size(63, 23);
            this.m_sendImage_btn.TabIndex = 8;
            this.m_sendImage_btn.Text = "发送图片";
            this.m_sendImage_btn.UseVisualStyleBackColor = true;
            this.m_sendImage_btn.Click += new System.EventHandler(this.m_sendImage_btn_Click);
            // 
            // m_customFace_btn
            // 
            this.m_customFace_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_customFace_btn.Location = new System.Drawing.Point(213, 253);
            this.m_customFace_btn.Name = "m_customFace_btn";
            this.m_customFace_btn.Size = new System.Drawing.Size(75, 23);
            this.m_customFace_btn.TabIndex = 9;
            this.m_customFace_btn.Text = "自定义表情";
            this.m_customFace_btn.UseVisualStyleBackColor = true;
            this.m_customFace_btn.Click += new System.EventHandler(this.m_customFace_btn_Click);
            // 
            // msgRichViewer1
            // 
            this.msgRichViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.msgRichViewer1.Location = new System.Drawing.Point(12, 10);
            this.msgRichViewer1.Name = "msgRichViewer1";
            this.msgRichViewer1.Size = new System.Drawing.Size(360, 240);
            this.msgRichViewer1.TabIndex = 7;
            // 
            // m_refresh_bt
            // 
            this.m_refresh_bt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_refresh_bt.Location = new System.Drawing.Point(449, 354);
            this.m_refresh_bt.Name = "m_refresh_bt";
            this.m_refresh_bt.Size = new System.Drawing.Size(75, 23);
            this.m_refresh_bt.TabIndex = 10;
            this.m_refresh_bt.Text = "Refresh";
            this.m_refresh_bt.UseVisualStyleBackColor = true;
            this.m_refresh_bt.Click += new System.EventHandler(this.m_refresh_bt_Click);
            // 
            // m_screenShot_btn
            // 
            this.m_screenShot_btn.Location = new System.Drawing.Point(132, 253);
            this.m_screenShot_btn.Name = "m_screenShot_btn";
            this.m_screenShot_btn.Size = new System.Drawing.Size(75, 23);
            this.m_screenShot_btn.TabIndex = 12;
            this.m_screenShot_btn.Text = "截屏";
            this.m_screenShot_btn.UseVisualStyleBackColor = true;
            this.m_screenShot_btn.Click += new System.EventHandler(this.m_screenShot_btn_Click);
            // 
            // GroupChatterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 390);
            this.Controls.Add(this.m_screenShot_btn);
            this.Controls.Add(this.m_refresh_bt);
            this.Controls.Add(this.m_customFace_btn);
            this.Controls.Add(this.m_sendImage_btn);
            this.Controls.Add(this.msgRichViewer1);
            this.Controls.Add(this.m_user_list);
            this.Controls.Add(this.m_msg_input_tbx);
            this.Name = "GroupChatterForm";
            this.Text = "GroupChatterForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GroupChatterForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox m_msg_input_tbx;
        private System.Windows.Forms.ListBox m_user_list;
        private Octopus.Controls.ChatControls.MsgRichViewer msgRichViewer1;
        private System.Windows.Forms.Button m_sendImage_btn;
        private System.Windows.Forms.Button m_customFace_btn;
        private System.Windows.Forms.Button m_refresh_bt;
        private System.Windows.Forms.Button m_screenShot_btn;
    }
}