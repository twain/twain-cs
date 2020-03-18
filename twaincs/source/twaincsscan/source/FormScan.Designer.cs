namespace TWAINCSScan
{
    partial class FormScan
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.m_pictureboxImage1 = new System.Windows.Forms.PictureBox();
            this.m_buttonScan = new System.Windows.Forms.Button();
            this.m_buttonSetup = new System.Windows.Forms.Button();
            this.m_pictureboxImage2 = new System.Windows.Forms.PictureBox();
            this.m_buttonClose = new System.Windows.Forms.Button();
            this.m_buttonOpen = new System.Windows.Forms.Button();
            this.m_buttonStop = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.m_pictureboxImage1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_pictureboxImage2)).BeginInit();
            this.SuspendLayout();
            // 
            // m_pictureboxImage1
            // 
            this.m_pictureboxImage1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_pictureboxImage1.Location = new System.Drawing.Point(13, 13);
            this.m_pictureboxImage1.Name = "m_pictureboxImage1";
            this.m_pictureboxImage1.Size = new System.Drawing.Size(335, 424);
            this.m_pictureboxImage1.TabIndex = 0;
            this.m_pictureboxImage1.TabStop = false;
            // 
            // m_buttonScan
            // 
            this.m_buttonScan.Location = new System.Drawing.Point(533, 453);
            this.m_buttonScan.Name = "m_buttonScan";
            this.m_buttonScan.Size = new System.Drawing.Size(75, 23);
            this.m_buttonScan.TabIndex = 1;
            this.m_buttonScan.Text = "Scan";
            this.m_buttonScan.UseVisualStyleBackColor = true;
            this.m_buttonScan.Click += new System.EventHandler(this.m_buttonScan_Click);
            // 
            // m_buttonSetup
            // 
            this.m_buttonSetup.Location = new System.Drawing.Point(452, 453);
            this.m_buttonSetup.Name = "m_buttonSetup";
            this.m_buttonSetup.Size = new System.Drawing.Size(75, 23);
            this.m_buttonSetup.TabIndex = 3;
            this.m_buttonSetup.Text = "Setup...";
            this.m_buttonSetup.UseVisualStyleBackColor = true;
            this.m_buttonSetup.Click += new System.EventHandler(this.m_buttonSetup_Click);
            // 
            // m_pictureboxImage2
            // 
            this.m_pictureboxImage2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_pictureboxImage2.Location = new System.Drawing.Point(354, 13);
            this.m_pictureboxImage2.Name = "m_pictureboxImage2";
            this.m_pictureboxImage2.Size = new System.Drawing.Size(335, 424);
            this.m_pictureboxImage2.TabIndex = 4;
            this.m_pictureboxImage2.TabStop = false;
            // 
            // m_buttonClose
            // 
            this.m_buttonClose.Location = new System.Drawing.Point(94, 453);
            this.m_buttonClose.Name = "m_buttonClose";
            this.m_buttonClose.Size = new System.Drawing.Size(75, 23);
            this.m_buttonClose.TabIndex = 5;
            this.m_buttonClose.Text = "Close";
            this.m_buttonClose.UseVisualStyleBackColor = true;
            this.m_buttonClose.Click += new System.EventHandler(this.m_buttonClose_Click);
            // 
            // m_buttonOpen
            // 
            this.m_buttonOpen.Location = new System.Drawing.Point(13, 453);
            this.m_buttonOpen.Name = "m_buttonOpen";
            this.m_buttonOpen.Size = new System.Drawing.Size(75, 23);
            this.m_buttonOpen.TabIndex = 6;
            this.m_buttonOpen.Text = "Open...";
            this.m_buttonOpen.UseVisualStyleBackColor = true;
            this.m_buttonOpen.Click += new System.EventHandler(this.m_buttonOpen_Click);
            // 
            // m_buttonStop
            // 
            this.m_buttonStop.Location = new System.Drawing.Point(614, 453);
            this.m_buttonStop.Name = "m_buttonStop";
            this.m_buttonStop.Size = new System.Drawing.Size(75, 23);
            this.m_buttonStop.TabIndex = 7;
            this.m_buttonStop.Text = "Stop";
            this.m_buttonStop.UseVisualStyleBackColor = true;
            this.m_buttonStop.Click += new System.EventHandler(this.m_buttonStop_Click);
            // 
            // FormScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 488);
            this.Controls.Add(this.m_buttonStop);
            this.Controls.Add(this.m_buttonOpen);
            this.Controls.Add(this.m_buttonClose);
            this.Controls.Add(this.m_pictureboxImage2);
            this.Controls.Add(this.m_buttonSetup);
            this.Controls.Add(this.m_buttonScan);
            this.Controls.Add(this.m_pictureboxImage1);
            this.Name = "FormScan";
            this.Text = "TWAIN C# Scan";
            ((System.ComponentModel.ISupportInitialize)(this.m_pictureboxImage1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_pictureboxImage2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox m_pictureboxImage1;
        private System.Windows.Forms.Button m_buttonScan;
        private System.Windows.Forms.Button m_buttonSetup;
        private System.Windows.Forms.PictureBox m_pictureboxImage2;
        private System.Windows.Forms.Button m_buttonClose;
        private System.Windows.Forms.Button m_buttonOpen;
        private System.Windows.Forms.Button m_buttonStop;
    }
}