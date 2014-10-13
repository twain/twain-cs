namespace TWAINCSScan
{
    partial class FormSetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSetup));
            this.m_labelUseUiSettings = new System.Windows.Forms.Label();
            this.m_buttonShowDriverUi = new System.Windows.Forms.Button();
            this.m_buttonSaveUiSettings = new System.Windows.Forms.Button();
            this.m_labelSelectDestinationFolder = new System.Windows.Forms.Label();
            this.m_textboxFolder = new System.Windows.Forms.TextBox();
            this.m_buttonSelectDestinationFolder = new System.Windows.Forms.Button();
            this.m_buttonUseUiSettings = new System.Windows.Forms.Button();
            this.m_textboxUseUiSettings = new System.Windows.Forms.TextBox();
            this.m_groupboxCreateUiSetting = new System.Windows.Forms.GroupBox();
            this.m_groupboxManageSettings = new System.Windows.Forms.GroupBox();
            this.m_buttonDeleteSetting = new System.Windows.Forms.Button();
            this.m_groupboxImageDestination = new System.Windows.Forms.GroupBox();
            this.m_groupboxManageSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_labelUseUiSettings
            // 
            this.m_labelUseUiSettings.AutoSize = true;
            this.m_labelUseUiSettings.Location = new System.Drawing.Point(30, 261);
            this.m_labelUseUiSettings.Name = "m_labelUseUiSettings";
            this.m_labelUseUiSettings.Size = new System.Drawing.Size(305, 13);
            this.m_labelUseUiSettings.TabIndex = 1;
            this.m_labelUseUiSettings.Text = "Select driver settings for next scan (if blank the driver chooses):";
            // 
            // m_buttonShowDriverUi
            // 
            this.m_buttonShowDriverUi.Location = new System.Drawing.Point(33, 133);
            this.m_buttonShowDriverUi.Name = "m_buttonShowDriverUi";
            this.m_buttonShowDriverUi.Size = new System.Drawing.Size(294, 23);
            this.m_buttonShowDriverUi.TabIndex = 1;
            this.m_buttonShowDriverUi.Text = "Change driver settings (press this first)...";
            this.m_buttonShowDriverUi.UseVisualStyleBackColor = true;
            this.m_buttonShowDriverUi.Click += new System.EventHandler(this.m_buttonSetup_Click);
            // 
            // m_buttonSaveUiSettings
            // 
            this.m_buttonSaveUiSettings.Location = new System.Drawing.Point(33, 171);
            this.m_buttonSaveUiSettings.Name = "m_buttonSaveUiSettings";
            this.m_buttonSaveUiSettings.Size = new System.Drawing.Size(294, 23);
            this.m_buttonSaveUiSettings.TabIndex = 2;
            this.m_buttonSaveUiSettings.Text = "Save driver settings (then press this one)...";
            this.m_buttonSaveUiSettings.UseVisualStyleBackColor = true;
            this.m_buttonSaveUiSettings.Click += new System.EventHandler(this.m_buttonSaveas_Click);
            // 
            // m_labelSelectDestinationFolder
            // 
            this.m_labelSelectDestinationFolder.AutoSize = true;
            this.m_labelSelectDestinationFolder.Location = new System.Drawing.Point(30, 38);
            this.m_labelSelectDestinationFolder.Name = "m_labelSelectDestinationFolder";
            this.m_labelSelectDestinationFolder.Size = new System.Drawing.Size(302, 13);
            this.m_labelSelectDestinationFolder.TabIndex = 4;
            this.m_labelSelectDestinationFolder.Text = "Select image destination folder (set blank if not saving images):";
            // 
            // m_textboxFolder
            // 
            this.m_textboxFolder.Location = new System.Drawing.Point(33, 56);
            this.m_textboxFolder.Name = "m_textboxFolder";
            this.m_textboxFolder.Size = new System.Drawing.Size(262, 20);
            this.m_textboxFolder.TabIndex = 5;
            this.m_textboxFolder.TextChanged += new System.EventHandler(this.m_textboxFolder_TextChanged);
            // 
            // m_buttonSelectDestinationFolder
            // 
            this.m_buttonSelectDestinationFolder.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("m_buttonSelectDestinationFolder.BackgroundImage")));
            this.m_buttonSelectDestinationFolder.Location = new System.Drawing.Point(301, 54);
            this.m_buttonSelectDestinationFolder.Name = "m_buttonSelectDestinationFolder";
            this.m_buttonSelectDestinationFolder.Size = new System.Drawing.Size(26, 23);
            this.m_buttonSelectDestinationFolder.TabIndex = 6;
            this.m_buttonSelectDestinationFolder.UseVisualStyleBackColor = true;
            this.m_buttonSelectDestinationFolder.Click += new System.EventHandler(this.m_buttonBrowse_Click);
            // 
            // m_buttonUseUiSettings
            // 
            this.m_buttonUseUiSettings.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("m_buttonUseUiSettings.BackgroundImage")));
            this.m_buttonUseUiSettings.Location = new System.Drawing.Point(301, 277);
            this.m_buttonUseUiSettings.Name = "m_buttonUseUiSettings";
            this.m_buttonUseUiSettings.Size = new System.Drawing.Size(26, 23);
            this.m_buttonUseUiSettings.TabIndex = 8;
            this.m_buttonUseUiSettings.UseVisualStyleBackColor = true;
            this.m_buttonUseUiSettings.Click += new System.EventHandler(this.m_buttonUseUiSettings_Click);
            // 
            // m_textboxUseUiSettings
            // 
            this.m_textboxUseUiSettings.Location = new System.Drawing.Point(33, 279);
            this.m_textboxUseUiSettings.Name = "m_textboxUseUiSettings";
            this.m_textboxUseUiSettings.Size = new System.Drawing.Size(262, 20);
            this.m_textboxUseUiSettings.TabIndex = 7;
            // 
            // m_groupboxCreateUiSetting
            // 
            this.m_groupboxCreateUiSetting.Location = new System.Drawing.Point(15, 103);
            this.m_groupboxCreateUiSetting.Name = "m_groupboxCreateUiSetting";
            this.m_groupboxCreateUiSetting.Size = new System.Drawing.Size(329, 109);
            this.m_groupboxCreateUiSetting.TabIndex = 9;
            this.m_groupboxCreateUiSetting.TabStop = false;
            this.m_groupboxCreateUiSetting.Text = "Create new driver settings";
            // 
            // m_groupboxManageSettings
            // 
            this.m_groupboxManageSettings.Controls.Add(this.m_buttonDeleteSetting);
            this.m_groupboxManageSettings.Location = new System.Drawing.Point(15, 230);
            this.m_groupboxManageSettings.Name = "m_groupboxManageSettings";
            this.m_groupboxManageSettings.Size = new System.Drawing.Size(329, 109);
            this.m_groupboxManageSettings.TabIndex = 10;
            this.m_groupboxManageSettings.TabStop = false;
            this.m_groupboxManageSettings.Text = "Select driver settings";
            // 
            // m_buttonDeleteSetting
            // 
            this.m_buttonDeleteSetting.Location = new System.Drawing.Point(195, 77);
            this.m_buttonDeleteSetting.Name = "m_buttonDeleteSetting";
            this.m_buttonDeleteSetting.Size = new System.Drawing.Size(117, 23);
            this.m_buttonDeleteSetting.TabIndex = 0;
            this.m_buttonDeleteSetting.Text = "Delete setting...";
            this.m_buttonDeleteSetting.UseVisualStyleBackColor = true;
            this.m_buttonDeleteSetting.Click += new System.EventHandler(this.m_buttonDeleteSetting_Click);
            // 
            // m_groupboxImageDestination
            // 
            this.m_groupboxImageDestination.Location = new System.Drawing.Point(15, 12);
            this.m_groupboxImageDestination.Name = "m_groupboxImageDestination";
            this.m_groupboxImageDestination.Size = new System.Drawing.Size(329, 78);
            this.m_groupboxImageDestination.TabIndex = 11;
            this.m_groupboxImageDestination.TabStop = false;
            this.m_groupboxImageDestination.Text = "Select image destination";
            // 
            // FormSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 354);
            this.Controls.Add(this.m_buttonUseUiSettings);
            this.Controls.Add(this.m_textboxUseUiSettings);
            this.Controls.Add(this.m_buttonSelectDestinationFolder);
            this.Controls.Add(this.m_textboxFolder);
            this.Controls.Add(this.m_labelSelectDestinationFolder);
            this.Controls.Add(this.m_buttonSaveUiSettings);
            this.Controls.Add(this.m_buttonShowDriverUi);
            this.Controls.Add(this.m_labelUseUiSettings);
            this.Controls.Add(this.m_groupboxCreateUiSetting);
            this.Controls.Add(this.m_groupboxManageSettings);
            this.Controls.Add(this.m_groupboxImageDestination);
            this.Name = "FormSetup";
            this.Text = "Setup TWAIN Scan";
            this.m_groupboxManageSettings.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label m_labelUseUiSettings;
        private System.Windows.Forms.Button m_buttonShowDriverUi;
        private System.Windows.Forms.Button m_buttonSaveUiSettings;
        private System.Windows.Forms.Label m_labelSelectDestinationFolder;
        private System.Windows.Forms.TextBox m_textboxFolder;
        private System.Windows.Forms.Button m_buttonSelectDestinationFolder;
        private System.Windows.Forms.Button m_buttonUseUiSettings;
        private System.Windows.Forms.TextBox m_textboxUseUiSettings;
        private System.Windows.Forms.GroupBox m_groupboxCreateUiSetting;
        private System.Windows.Forms.GroupBox m_groupboxManageSettings;
        private System.Windows.Forms.Button m_buttonDeleteSetting;
        private System.Windows.Forms.GroupBox m_groupboxImageDestination;
    }
}