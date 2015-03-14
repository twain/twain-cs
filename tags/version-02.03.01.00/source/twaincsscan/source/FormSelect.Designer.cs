namespace TWAINCSScan
{
    partial class FormSelect
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
            this.m_listboxSelect = new System.Windows.Forms.ListBox();
            this.m_buttonOpen = new System.Windows.Forms.Button();
            this.m_labelSelect = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // m_listboxSelect
            // 
            this.m_listboxSelect.FormattingEnabled = true;
            this.m_listboxSelect.Location = new System.Drawing.Point(12, 38);
            this.m_listboxSelect.Name = "m_listboxSelect";
            this.m_listboxSelect.Size = new System.Drawing.Size(259, 173);
            this.m_listboxSelect.Sorted = true;
            this.m_listboxSelect.TabIndex = 0;
            this.m_listboxSelect.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(m_listboxSelect_MouseDoubleClick);
            // 
            // m_buttonOpen
            // 
            this.m_buttonOpen.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.m_buttonOpen.Location = new System.Drawing.Point(197, 223);
            this.m_buttonOpen.Name = "m_buttonOpen";
            this.m_buttonOpen.Size = new System.Drawing.Size(75, 23);
            this.m_buttonOpen.TabIndex = 1;
            this.m_buttonOpen.Text = "Open";
            this.m_buttonOpen.UseVisualStyleBackColor = true;
            this.m_buttonOpen.Click += new System.EventHandler(this.m_buttonOpen_Click);
            // 
            // m_labelSelect
            // 
            this.m_labelSelect.AutoSize = true;
            this.m_labelSelect.Location = new System.Drawing.Point(12, 12);
            this.m_labelSelect.Name = "m_labelSelect";
            this.m_labelSelect.Size = new System.Drawing.Size(110, 13);
            this.m_labelSelect.TabIndex = 3;
            this.m_labelSelect.Text = "Select TWAIN Driver:";
            // 
            // FormSelect
            // 
            this.AcceptButton = this.m_buttonOpen;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.m_labelSelect);
            this.Controls.Add(this.m_buttonOpen);
            this.Controls.Add(this.m_listboxSelect);
            this.Name = "FormSelect";
            this.Text = "Open TWAIN Driver";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox m_listboxSelect;
        private System.Windows.Forms.Button m_buttonOpen;
        private System.Windows.Forms.Label m_labelSelect;
    }
}

