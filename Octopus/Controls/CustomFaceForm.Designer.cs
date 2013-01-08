namespace Octopus.Controls
{
    partial class CustomFaceForm
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
            this.m_nextPage_btn = new System.Windows.Forms.Button();
            this.m_cancel_btn = new System.Windows.Forms.Button();
            this.m_add_btn = new System.Windows.Forms.Button();
            this.m_previousPage_btn = new System.Windows.Forms.Button();
            this.m_preview_pbx = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.m_preview_pbx)).BeginInit();
            this.SuspendLayout();
            // 
            // m_nextPage_btn
            // 
            this.m_nextPage_btn.Location = new System.Drawing.Point(288, 182);
            this.m_nextPage_btn.Name = "m_nextPage_btn";
            this.m_nextPage_btn.Size = new System.Drawing.Size(75, 23);
            this.m_nextPage_btn.TabIndex = 0;
            this.m_nextPage_btn.Text = "下一页";
            this.m_nextPage_btn.UseVisualStyleBackColor = true;
            this.m_nextPage_btn.Click += new System.EventHandler(this.m_nextPage_btn_Click);
            // 
            // m_cancel_btn
            // 
            this.m_cancel_btn.Location = new System.Drawing.Point(12, 182);
            this.m_cancel_btn.Name = "m_cancel_btn";
            this.m_cancel_btn.Size = new System.Drawing.Size(75, 23);
            this.m_cancel_btn.TabIndex = 1;
            this.m_cancel_btn.Text = "取消";
            this.m_cancel_btn.UseVisualStyleBackColor = true;
            this.m_cancel_btn.Click += new System.EventHandler(this.m_cancel_btn_Click);
            // 
            // m_add_btn
            // 
            this.m_add_btn.Location = new System.Drawing.Point(93, 182);
            this.m_add_btn.Name = "m_add_btn";
            this.m_add_btn.Size = new System.Drawing.Size(70, 23);
            this.m_add_btn.TabIndex = 2;
            this.m_add_btn.Text = "添加";
            this.m_add_btn.UseVisualStyleBackColor = true;
            this.m_add_btn.Click += new System.EventHandler(this.m_add_btn_Click);
            // 
            // m_previousPage_btn
            // 
            this.m_previousPage_btn.Location = new System.Drawing.Point(207, 182);
            this.m_previousPage_btn.Name = "m_previousPage_btn";
            this.m_previousPage_btn.Size = new System.Drawing.Size(75, 23);
            this.m_previousPage_btn.TabIndex = 0;
            this.m_previousPage_btn.Text = "上一页";
            this.m_previousPage_btn.UseVisualStyleBackColor = true;
            this.m_previousPage_btn.Click += new System.EventHandler(this.m_previousPage_btn_Click);
            // 
            // m_preview_pbx
            // 
            this.m_preview_pbx.Location = new System.Drawing.Point(263, 12);
            this.m_preview_pbx.Name = "m_preview_pbx";
            this.m_preview_pbx.Size = new System.Drawing.Size(100, 100);
            this.m_preview_pbx.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.m_preview_pbx.TabIndex = 3;
            this.m_preview_pbx.TabStop = false;
            // 
            // CustomFaceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 211);
            this.Controls.Add(this.m_preview_pbx);
            this.Controls.Add(this.m_add_btn);
            this.Controls.Add(this.m_cancel_btn);
            this.Controls.Add(this.m_previousPage_btn);
            this.Controls.Add(this.m_nextPage_btn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CustomFaceForm";
            this.ShowInTaskbar = false;
            this.Text = "CustomFaceForm";
            this.Click += new System.EventHandler(this.CustomFaceForm_Click);
            ((System.ComponentModel.ISupportInitialize)(this.m_preview_pbx)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button m_nextPage_btn;
        private System.Windows.Forms.Button m_cancel_btn;
        private System.Windows.Forms.Button m_add_btn;
        private System.Windows.Forms.Button m_previousPage_btn;
        private System.Windows.Forms.PictureBox m_preview_pbx;
    }
}