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
            this.m_information_listbox = new System.Windows.Forms.ListBox();
            this.m_tray = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // m_information_listbox
            // 
            this.m_information_listbox.FormattingEnabled = true;
            this.m_information_listbox.ItemHeight = 12;
            this.m_information_listbox.Location = new System.Drawing.Point(12, 12);
            this.m_information_listbox.Name = "m_information_listbox";
            this.m_information_listbox.Size = new System.Drawing.Size(421, 304);
            this.m_information_listbox.TabIndex = 0;
            // 
            // m_tray
            // 
            this.m_tray.Icon = ((System.Drawing.Icon)(resources.GetObject("m_tray.Icon")));
            this.m_tray.Text = "Octopus";
            this.m_tray.Visible = true;
            this.m_tray.Click += new System.EventHandler(this.m_tray_Click);
            // 
            // Workbench
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 332);
            this.Controls.Add(this.m_information_listbox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Workbench";
            this.ShowInTaskbar = false;
            this.Text = "Octopus";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Workbench_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox m_information_listbox;
        private System.Windows.Forms.NotifyIcon m_tray;


    }
}

