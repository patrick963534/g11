namespace Octopus.Controls.ChatControls
{
    partial class MsgRichViewer
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
            this.m_msgViewer = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // m_msgViewer
            // 
            this.m_msgViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_msgViewer.Location = new System.Drawing.Point(0, 0);
            this.m_msgViewer.MinimumSize = new System.Drawing.Size(20, 20);
            this.m_msgViewer.Name = "m_msgViewer";
            this.m_msgViewer.Size = new System.Drawing.Size(355, 258);
            this.m_msgViewer.TabIndex = 0;
            // 
            // MsgRichViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_msgViewer);
            this.Name = "MsgRichViewer";
            this.Size = new System.Drawing.Size(355, 261);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser m_msgViewer;
    }
}
