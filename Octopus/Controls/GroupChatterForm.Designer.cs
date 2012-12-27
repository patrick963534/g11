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
            this.m_msg_show_tbx = new System.Windows.Forms.TextBox();
            this.m_user_list = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // m_msg_input_tbx
            // 
            this.m_msg_input_tbx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_msg_input_tbx.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_msg_input_tbx.ImeMode = System.Windows.Forms.ImeMode.On;
            this.m_msg_input_tbx.Location = new System.Drawing.Point(12, 264);
            this.m_msg_input_tbx.Multiline = true;
            this.m_msg_input_tbx.Name = "m_msg_input_tbx";
            this.m_msg_input_tbx.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.m_msg_input_tbx.Size = new System.Drawing.Size(360, 97);
            this.m_msg_input_tbx.TabIndex = 5;
            this.m_msg_input_tbx.KeyDown += new System.Windows.Forms.KeyEventHandler(this.m_msg_input_tbx_KeyDown);
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
            this.m_msg_show_tbx.Size = new System.Drawing.Size(360, 246);
            this.m_msg_show_tbx.TabIndex = 4;
            // 
            // m_user_list
            // 
            this.m_user_list.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_user_list.FormattingEnabled = true;
            this.m_user_list.ItemHeight = 12;
            this.m_user_list.Location = new System.Drawing.Point(378, 10);
            this.m_user_list.Name = "m_user_list";
            this.m_user_list.Size = new System.Drawing.Size(146, 352);
            this.m_user_list.TabIndex = 6;
            // 
            // GroupChatterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 374);
            this.Controls.Add(this.m_user_list);
            this.Controls.Add(this.m_msg_input_tbx);
            this.Controls.Add(this.m_msg_show_tbx);
            this.Name = "GroupChatterForm";
            this.Text = "GroupChatterForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GroupChatterForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox m_msg_input_tbx;
        private System.Windows.Forms.TextBox m_msg_show_tbx;
        private System.Windows.Forms.ListBox m_user_list;
    }
}