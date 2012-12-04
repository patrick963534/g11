namespace OctopusServer
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
            this.m_browse_btn = new System.Windows.Forms.Button();
            this.m_update_file_path_txb = new System.Windows.Forms.TextBox();
            this.m_start_service_btn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.m_information_listbox = new System.Windows.Forms.ListBox();
            this.m_version_tbx = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_browse_btn
            // 
            this.m_browse_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_browse_btn.Location = new System.Drawing.Point(424, 12);
            this.m_browse_btn.Name = "m_browse_btn";
            this.m_browse_btn.Size = new System.Drawing.Size(75, 23);
            this.m_browse_btn.TabIndex = 0;
            this.m_browse_btn.Text = "browse";
            this.m_browse_btn.UseVisualStyleBackColor = true;
            this.m_browse_btn.Click += new System.EventHandler(this.m_browse_btn_Click);
            // 
            // m_update_file_path_txb
            // 
            this.m_update_file_path_txb.Location = new System.Drawing.Point(81, 12);
            this.m_update_file_path_txb.Name = "m_update_file_path_txb";
            this.m_update_file_path_txb.Size = new System.Drawing.Size(337, 21);
            this.m_update_file_path_txb.TabIndex = 1;
            // 
            // m_start_service_btn
            // 
            this.m_start_service_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_start_service_btn.Location = new System.Drawing.Point(424, 41);
            this.m_start_service_btn.Name = "m_start_service_btn";
            this.m_start_service_btn.Size = new System.Drawing.Size(75, 40);
            this.m_start_service_btn.TabIndex = 2;
            this.m_start_service_btn.Text = "start service";
            this.m_start_service_btn.UseVisualStyleBackColor = true;
            this.m_start_service_btn.Click += new System.EventHandler(this.m_start_service_btn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "File Path:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.m_information_listbox);
            this.groupBox1.Location = new System.Drawing.Point(14, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(404, 309);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Information:";
            // 
            // m_information_listbox
            // 
            this.m_information_listbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_information_listbox.FormattingEnabled = true;
            this.m_information_listbox.ItemHeight = 12;
            this.m_information_listbox.Location = new System.Drawing.Point(6, 20);
            this.m_information_listbox.Name = "m_information_listbox";
            this.m_information_listbox.Size = new System.Drawing.Size(392, 280);
            this.m_information_listbox.TabIndex = 0;
            // 
            // m_version_tbx
            // 
            this.m_version_tbx.Location = new System.Drawing.Point(425, 87);
            this.m_version_tbx.Name = "m_version_tbx";
            this.m_version_tbx.Size = new System.Drawing.Size(74, 21);
            this.m_version_tbx.TabIndex = 5;
            this.m_version_tbx.TextChanged += new System.EventHandler(this.m_version_tbx_TextChanged);
            // 
            // Workbench
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 362);
            this.Controls.Add(this.m_version_tbx);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_start_service_btn);
            this.Controls.Add(this.m_update_file_path_txb);
            this.Controls.Add(this.m_browse_btn);
            this.Name = "Workbench";
            this.Text = "OctopusServer";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button m_browse_btn;
        private System.Windows.Forms.TextBox m_update_file_path_txb;
        private System.Windows.Forms.Button m_start_service_btn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox m_information_listbox;
        private System.Windows.Forms.TextBox m_version_tbx;
    }
}

