namespace SAI_Editor
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.buttonExitSettings = new System.Windows.Forms.Button();
            this.buttonSaveSettings = new System.Windows.Forms.Button();
            this.tabPage = new System.Windows.Forms.TabPage();
            this.checkBoxLoadScriptOfEntry = new System.Windows.Forms.CheckBox();
            this.checkBoxExpandInstantly = new System.Windows.Forms.CheckBox();
            this.tabControlSettings = new System.Windows.Forms.TabControl();
            this.tabPageConnection = new System.Windows.Forms.TabPage();
            this.checkBoxAutoConnect = new System.Windows.Forms.CheckBox();
            this.textBoxHost = new System.Windows.Forms.TextBox();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.textBoxWorldDatabase = new System.Windows.Forms.TextBox();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.buttonClearSettings = new System.Windows.Forms.Button();
            this.checkBoxAutoSaveSettings = new System.Windows.Forms.CheckBox();
            this.checkBoxPromptToQuit = new System.Windows.Forms.CheckBox();
            this.tabPage.SuspendLayout();
            this.tabControlSettings.SuspendLayout();
            this.tabPageConnection.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonExitSettings
            // 
            this.buttonExitSettings.Location = new System.Drawing.Point(188, 199);
            this.buttonExitSettings.Name = "buttonExitSettings";
            this.buttonExitSettings.Size = new System.Drawing.Size(75, 23);
            this.buttonExitSettings.TabIndex = 2;
            this.buttonExitSettings.Text = "Exit";
            this.buttonExitSettings.UseVisualStyleBackColor = true;
            this.buttonExitSettings.Click += new System.EventHandler(this.buttonExitSettings_Click);
            // 
            // buttonSaveSettings
            // 
            this.buttonSaveSettings.Location = new System.Drawing.Point(12, 199);
            this.buttonSaveSettings.Name = "buttonSaveSettings";
            this.buttonSaveSettings.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveSettings.TabIndex = 3;
            this.buttonSaveSettings.Text = "Save";
            this.buttonSaveSettings.UseVisualStyleBackColor = true;
            this.buttonSaveSettings.Click += new System.EventHandler(this.buttonSaveSettings_Click);
            // 
            // tabPage
            // 
            this.tabPage.Controls.Add(this.checkBoxPromptToQuit);
            this.tabPage.Controls.Add(this.checkBoxAutoSaveSettings);
            this.tabPage.Controls.Add(this.checkBoxLoadScriptOfEntry);
            this.tabPage.Controls.Add(this.checkBoxExpandInstantly);
            this.tabPage.Location = new System.Drawing.Point(4, 22);
            this.tabPage.Name = "tabPage";
            this.tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage.Size = new System.Drawing.Size(243, 159);
            this.tabPage.TabIndex = 0;
            this.tabPage.Text = "Other";
            this.tabPage.UseVisualStyleBackColor = true;
            // 
            // checkBoxLoadScriptOfEntry
            // 
            this.checkBoxLoadScriptOfEntry.AutoSize = true;
            this.checkBoxLoadScriptOfEntry.Checked = true;
            this.checkBoxLoadScriptOfEntry.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLoadScriptOfEntry.Location = new System.Drawing.Point(6, 29);
            this.checkBoxLoadScriptOfEntry.Name = "checkBoxLoadScriptOfEntry";
            this.checkBoxLoadScriptOfEntry.Size = new System.Drawing.Size(159, 17);
            this.checkBoxLoadScriptOfEntry.TabIndex = 5;
            this.checkBoxLoadScriptOfEntry.Text = "Load script of selected entry";
            this.checkBoxLoadScriptOfEntry.UseVisualStyleBackColor = true;
            // 
            // checkBoxExpandInstantly
            // 
            this.checkBoxExpandInstantly.AutoSize = true;
            this.checkBoxExpandInstantly.Location = new System.Drawing.Point(6, 6);
            this.checkBoxExpandInstantly.Name = "checkBoxExpandInstantly";
            this.checkBoxExpandInstantly.Size = new System.Drawing.Size(166, 17);
            this.checkBoxExpandInstantly.TabIndex = 0;
            this.checkBoxExpandInstantly.Text = "Expand and contract instantly";
            this.checkBoxExpandInstantly.UseVisualStyleBackColor = true;
            // 
            // tabControlSettings
            // 
            this.tabControlSettings.Controls.Add(this.tabPage);
            this.tabControlSettings.Controls.Add(this.tabPageConnection);
            this.tabControlSettings.Location = new System.Drawing.Point(12, 8);
            this.tabControlSettings.Name = "tabControlSettings";
            this.tabControlSettings.SelectedIndex = 0;
            this.tabControlSettings.Size = new System.Drawing.Size(251, 185);
            this.tabControlSettings.TabIndex = 1;
            // 
            // tabPageConnection
            // 
            this.tabPageConnection.Controls.Add(this.checkBoxAutoConnect);
            this.tabPageConnection.Controls.Add(this.textBoxHost);
            this.tabPageConnection.Controls.Add(this.textBoxUsername);
            this.tabPageConnection.Controls.Add(this.textBoxPassword);
            this.tabPageConnection.Controls.Add(this.textBoxWorldDatabase);
            this.tabPageConnection.Controls.Add(this.textBoxPort);
            this.tabPageConnection.Controls.Add(this.label3);
            this.tabPageConnection.Controls.Add(this.label4);
            this.tabPageConnection.Controls.Add(this.label5);
            this.tabPageConnection.Controls.Add(this.label6);
            this.tabPageConnection.Controls.Add(this.label7);
            this.tabPageConnection.Location = new System.Drawing.Point(4, 22);
            this.tabPageConnection.Name = "tabPageConnection";
            this.tabPageConnection.Size = new System.Drawing.Size(243, 159);
            this.tabPageConnection.TabIndex = 1;
            this.tabPageConnection.Text = "Connection";
            this.tabPageConnection.UseVisualStyleBackColor = true;
            // 
            // checkBoxAutoConnect
            // 
            this.checkBoxAutoConnect.AutoSize = true;
            this.checkBoxAutoConnect.Location = new System.Drawing.Point(149, 135);
            this.checkBoxAutoConnect.Name = "checkBoxAutoConnect";
            this.checkBoxAutoConnect.Size = new System.Drawing.Size(90, 17);
            this.checkBoxAutoConnect.TabIndex = 29;
            this.checkBoxAutoConnect.Text = "Auto connect";
            this.checkBoxAutoConnect.UseVisualStyleBackColor = true;
            // 
            // textBoxHost
            // 
            this.textBoxHost.Location = new System.Drawing.Point(106, 5);
            this.textBoxHost.Name = "textBoxHost";
            this.textBoxHost.Size = new System.Drawing.Size(130, 20);
            this.textBoxHost.TabIndex = 19;
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Location = new System.Drawing.Point(106, 31);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(130, 20);
            this.textBoxUsername.TabIndex = 21;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(106, 57);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(130, 20);
            this.textBoxPassword.TabIndex = 23;
            // 
            // textBoxWorldDatabase
            // 
            this.textBoxWorldDatabase.Location = new System.Drawing.Point(106, 83);
            this.textBoxWorldDatabase.Name = "textBoxWorldDatabase";
            this.textBoxWorldDatabase.Size = new System.Drawing.Size(130, 20);
            this.textBoxWorldDatabase.TabIndex = 25;
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(106, 109);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(130, 20);
            this.textBoxPort.TabIndex = 27;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(42, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Username:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(44, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Password:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 86);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "World Database:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(68, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 26;
            this.label6.Text = "Host:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(71, 112);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 28;
            this.label7.Text = "Port:";
            // 
            // buttonClearSettings
            // 
            this.buttonClearSettings.Location = new System.Drawing.Point(100, 199);
            this.buttonClearSettings.Name = "buttonClearSettings";
            this.buttonClearSettings.Size = new System.Drawing.Size(75, 23);
            this.buttonClearSettings.TabIndex = 4;
            this.buttonClearSettings.Text = "Clear";
            this.buttonClearSettings.UseVisualStyleBackColor = true;
            this.buttonClearSettings.Click += new System.EventHandler(this.buttonClearSettings_Click);
            // 
            // checkBoxAutoSaveSettings
            // 
            this.checkBoxAutoSaveSettings.AutoSize = true;
            this.checkBoxAutoSaveSettings.Location = new System.Drawing.Point(6, 136);
            this.checkBoxAutoSaveSettings.Name = "checkBoxAutoSaveSettings";
            this.checkBoxAutoSaveSettings.Size = new System.Drawing.Size(113, 17);
            this.checkBoxAutoSaveSettings.TabIndex = 5;
            this.checkBoxAutoSaveSettings.Text = "Auto save settings";
            this.checkBoxAutoSaveSettings.UseVisualStyleBackColor = true;
            // 
            // checkBoxPromptToQuit
            // 
            this.checkBoxPromptToQuit.AutoSize = true;
            this.checkBoxPromptToQuit.Checked = true;
            this.checkBoxPromptToQuit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxPromptToQuit.Location = new System.Drawing.Point(6, 52);
            this.checkBoxPromptToQuit.Name = "checkBoxPromptToQuit";
            this.checkBoxPromptToQuit.Size = new System.Drawing.Size(171, 17);
            this.checkBoxPromptToQuit.TabIndex = 5;
            this.checkBoxPromptToQuit.Text = "Ask me if I\'m sure I want to exit";
            this.checkBoxPromptToQuit.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 230);
            this.Controls.Add(this.buttonClearSettings);
            this.Controls.Add(this.buttonSaveSettings);
            this.Controls.Add(this.buttonExitSettings);
            this.Controls.Add(this.tabControlSettings);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.tabPage.ResumeLayout(false);
            this.tabPage.PerformLayout();
            this.tabControlSettings.ResumeLayout(false);
            this.tabPageConnection.ResumeLayout(false);
            this.tabPageConnection.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonExitSettings;
        private System.Windows.Forms.Button buttonSaveSettings;
        private System.Windows.Forms.TabPage tabPage;
        private System.Windows.Forms.TabControl tabControlSettings;
        private System.Windows.Forms.Button buttonClearSettings;
        private System.Windows.Forms.TabPage tabPageConnection;
        private System.Windows.Forms.CheckBox checkBoxAutoConnect;
        private System.Windows.Forms.TextBox textBoxHost;
        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.TextBox textBoxWorldDatabase;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox checkBoxExpandInstantly;
        private System.Windows.Forms.CheckBox checkBoxLoadScriptOfEntry;
        private System.Windows.Forms.CheckBox checkBoxAutoSaveSettings;
        private System.Windows.Forms.CheckBox checkBoxPromptToQuit;

    }
}