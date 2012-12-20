namespace Octopus.Controls
{
    partial class LogViewer
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
            this.m_msg_show_tbx = new System.Windows.Forms.TextBox();
            this.m_extraMessage_tbx = new System.Windows.Forms.TextBox();
            this.m_cmdCounter_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
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
            this.m_msg_show_tbx.Size = new System.Drawing.Size(494, 235);
            this.m_msg_show_tbx.TabIndex = 1;
            // 
            // m_extraMessage_tbx
            // 
            this.m_extraMessage_tbx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_extraMessage_tbx.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_extraMessage_tbx.Location = new System.Drawing.Point(12, 253);
            this.m_extraMessage_tbx.Multiline = true;
            this.m_extraMessage_tbx.Name = "m_extraMessage_tbx";
            this.m_extraMessage_tbx.ReadOnly = true;
            this.m_extraMessage_tbx.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.m_extraMessage_tbx.Size = new System.Drawing.Size(335, 154);
            this.m_extraMessage_tbx.TabIndex = 2;
            // 
            // m_cmdCounter_btn
            // 
            this.m_cmdCounter_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cmdCounter_btn.Location = new System.Drawing.Point(353, 253);
            this.m_cmdCounter_btn.Name = "m_cmdCounter_btn";
            this.m_cmdCounter_btn.Size = new System.Drawing.Size(138, 23);
            this.m_cmdCounter_btn.TabIndex = 3;
            this.m_cmdCounter_btn.Text = "Command Counts";
            this.m_cmdCounter_btn.UseVisualStyleBackColor = true;
            this.m_cmdCounter_btn.Click += new System.EventHandler(this.m_cmdCounter_btn_Click);
            // 
            // LogViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 419);
            this.Controls.Add(this.m_cmdCounter_btn);
            this.Controls.Add(this.m_extraMessage_tbx);
            this.Controls.Add(this.m_msg_show_tbx);
            this.Name = "LogViewer";
            this.Text = "LogViewer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LogViewer_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox m_msg_show_tbx;
        private System.Windows.Forms.TextBox m_extraMessage_tbx;
        private System.Windows.Forms.Button m_cmdCounter_btn;
    }
}