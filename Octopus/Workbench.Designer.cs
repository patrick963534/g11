namespace Octopus
{
    partial class Workbench
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Workbench));
            this.m_tray = new System.Windows.Forms.NotifyIcon(this.components);
            this.m_msg_show_tbx = new System.Windows.Forms.TextBox();
            this.m_users_list = new System.Windows.Forms.ListBox();
            this.m_add_client_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_tray
            // 
            this.m_tray.Icon = ((System.Drawing.Icon)(resources.GetObject("m_tray.Icon")));
            this.m_tray.Text = "Octopus";
            this.m_tray.Visible = true;
            this.m_tray.Click += new System.EventHandler(this.m_tray_Click);
            // 
            // m_msg_show_tbx
            // 
            this.m_msg_show_tbx.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_msg_show_tbx.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_msg_show_tbx.Location = new System.Drawing.Point(12, 12);
            this.m_msg_show_tbx.Multiline = true;
            this.m_msg_show_tbx.Name = "m_msg_show_tbx";
            this.m_msg_show_tbx.ReadOnly = true;
            this.m_msg_show_tbx.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.m_msg_show_tbx.Size = new System.Drawing.Size(390, 308);
            this.m_msg_show_tbx.TabIndex = 0;
            // 
            // m_users_list
            // 
            this.m_users_list.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_users_list.FormattingEnabled = true;
            this.m_users_list.ItemHeight = 12;
            this.m_users_list.Location = new System.Drawing.Point(408, 12);
            this.m_users_list.Name = "m_users_list";
            this.m_users_list.Size = new System.Drawing.Size(172, 280);
            this.m_users_list.TabIndex = 1;
            this.m_users_list.DoubleClick += new System.EventHandler(this.m_users_list_DoubleClick);
            // 
            // m_add_client_btn
            // 
            this.m_add_client_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_add_client_btn.Location = new System.Drawing.Point(505, 297);
            this.m_add_client_btn.Name = "m_add_client_btn";
            this.m_add_client_btn.Size = new System.Drawing.Size(75, 23);
            this.m_add_client_btn.TabIndex = 3;
            this.m_add_client_btn.Text = "Refresh";
            this.m_add_client_btn.UseVisualStyleBackColor = true;
            this.m_add_client_btn.Click += new System.EventHandler(this.m_add_client_btn_Click);
            // 
            // Workbench
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 332);
            this.Controls.Add(this.m_add_client_btn);
            this.Controls.Add(this.m_users_list);
            this.Controls.Add(this.m_msg_show_tbx);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Workbench";
            this.ShowInTaskbar = false;
            this.Text = "Octopus";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Workbench_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon m_tray;
        private System.Windows.Forms.TextBox m_msg_show_tbx;
        private System.Windows.Forms.ListBox m_users_list;
        private System.Windows.Forms.Button m_add_client_btn;


    }
}

