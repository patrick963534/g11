namespace Octopus.Controls
{
    partial class UsersList
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.m_users_list = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // m_users_list
            // 
            this.m_users_list.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_users_list.FormattingEnabled = true;
            this.m_users_list.ItemHeight = 12;
            this.m_users_list.Location = new System.Drawing.Point(3, 3);
            this.m_users_list.Name = "m_users_list";
            this.m_users_list.Size = new System.Drawing.Size(178, 400);
            this.m_users_list.TabIndex = 0;
            this.m_users_list.DoubleClick += new System.EventHandler(this.m_users_list_DoubleClick);
            this.m_users_list.MouseDown += new System.Windows.Forms.MouseEventHandler(this.m_users_list_MouseDown);
            // 
            // UsersList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_users_list);
            this.Name = "UsersList";
            this.Size = new System.Drawing.Size(184, 404);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox m_users_list;
    }
}
