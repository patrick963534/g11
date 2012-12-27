using Octopus.Core;
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
            this.m_add_client_btn = new System.Windows.Forms.Button();
            this.m_logger_btn = new System.Windows.Forms.Button();
            this.m_tabs = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.m_users = new Octopus.Controls.UsersList();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.m_groups = new Octopus.Controls.GroupList();
            this.m_tabs.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_tray
            // 
            this.m_tray.Icon = ((System.Drawing.Icon)(resources.GetObject("m_tray.Icon")));
            this.m_tray.Text = "Octopus";
            this.m_tray.Visible = true;
            this.m_tray.Click += new System.EventHandler(this.m_tray_Click);
            // 
            // m_add_client_btn
            // 
            this.m_add_client_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_add_client_btn.Location = new System.Drawing.Point(104, 353);
            this.m_add_client_btn.Name = "m_add_client_btn";
            this.m_add_client_btn.Size = new System.Drawing.Size(75, 23);
            this.m_add_client_btn.TabIndex = 3;
            this.m_add_client_btn.Text = "Refresh";
            this.m_add_client_btn.UseVisualStyleBackColor = true;
            this.m_add_client_btn.Click += new System.EventHandler(this.m_add_client_btn_Click);
            // 
            // m_logger_btn
            // 
            this.m_logger_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_logger_btn.Location = new System.Drawing.Point(7, 353);
            this.m_logger_btn.Name = "m_logger_btn";
            this.m_logger_btn.Size = new System.Drawing.Size(75, 23);
            this.m_logger_btn.TabIndex = 4;
            this.m_logger_btn.Text = "Log";
            this.m_logger_btn.UseVisualStyleBackColor = true;
            this.m_logger_btn.Click += new System.EventHandler(this.m_logger_btn_Click);
            // 
            // m_tabs
            // 
            this.m_tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_tabs.Controls.Add(this.tabPage1);
            this.m_tabs.Controls.Add(this.tabPage2);
            this.m_tabs.Location = new System.Drawing.Point(7, 0);
            this.m_tabs.Name = "m_tabs";
            this.m_tabs.SelectedIndex = 0;
            this.m_tabs.Size = new System.Drawing.Size(179, 347);
            this.m_tabs.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.m_users);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(171, 321);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "好友";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // m_users
            // 
            this.m_users.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_users.Location = new System.Drawing.Point(6, 6);
            this.m_users.Name = "m_users";
            this.m_users.Size = new System.Drawing.Size(159, 309);
            this.m_users.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.m_groups);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(171, 321);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "房间";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // m_groups
            // 
            this.m_groups.Location = new System.Drawing.Point(3, 6);
            this.m_groups.Name = "m_groups";
            this.m_groups.Size = new System.Drawing.Size(165, 309);
            this.m_groups.TabIndex = 0;
            // 
            // Workbench
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(191, 388);
            this.Controls.Add(this.m_tabs);
            this.Controls.Add(this.m_logger_btn);
            this.Controls.Add(this.m_add_client_btn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Workbench";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Octopus";
            this.Load += new System.EventHandler(this.Workbench_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Workbench_FormClosing);
            this.m_tabs.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon m_tray;
        private System.Windows.Forms.Button m_add_client_btn;
        private System.Windows.Forms.Button m_logger_btn;
        private System.Windows.Forms.TabControl m_tabs;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Octopus.Controls.GroupList m_groups;
        private Octopus.Controls.UsersList m_users;


    }
}

