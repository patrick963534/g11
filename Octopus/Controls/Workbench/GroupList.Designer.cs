namespace Octopus.Controls
{
    partial class GroupList
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
            this.m_list = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // m_list
            // 
            this.m_list.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_list.FormattingEnabled = true;
            this.m_list.ItemHeight = 12;
            this.m_list.Location = new System.Drawing.Point(3, 4);
            this.m_list.Name = "m_list";
            this.m_list.Size = new System.Drawing.Size(184, 412);
            this.m_list.TabIndex = 0;
            this.m_list.DoubleClick += new System.EventHandler(this.m_list_DoubleClick);
            this.m_list.MouseDown += new System.Windows.Forms.MouseEventHandler(this.m_list_MouseDown);
            // 
            // GroupList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_list);
            this.Name = "GroupList";
            this.Size = new System.Drawing.Size(190, 419);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox m_list;
    }
}
