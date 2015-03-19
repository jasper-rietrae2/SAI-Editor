namespace SAI_Editor.Forms
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
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.checkBoxDuplicatePrimaryFields = new System.Windows.Forms.CheckBox();
            this.checkBoxPhaseHighlighting = new System.Windows.Forms.CheckBox();
            this.checkBoxCreateRevertQuery = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoGenerateComments = new System.Windows.Forms.CheckBox();
            this.checkBoxShowTooltipsStaticly = new System.Windows.Forms.CheckBox();
            this.checkBoxChangeStaticInfo = new System.Windows.Forms.CheckBox();
            this.checkBoxPromptExecuteQuery = new System.Windows.Forms.CheckBox();
            this.textBoxAnimationSpeed = new System.Windows.Forms.TextBox();
            this.labelAnimationSpeed = new System.Windows.Forms.Label();
            this.trackBarAnimationSpeed = new System.Windows.Forms.TrackBar();
            this.checkBoxPromptToQuit = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoSaveSettings = new System.Windows.Forms.CheckBox();
            this.checkBoxInstantExpand = new System.Windows.Forms.CheckBox();
            this.tabControlSettings = new System.Windows.Forms.TabControl();
            this.tabPageConnection = new System.Windows.Forms.TabPage();
            this.radioButtonDontUseDatabase = new System.Windows.Forms.RadioButton();
            this.radioButtonConnectToMySql = new System.Windows.Forms.RadioButton();
            this.buttonTestConnection = new System.Windows.Forms.Button();
            this.buttonSearchForWorldDb = new System.Windows.Forms.Button();
            this.checkBoxHidePass = new System.Windows.Forms.CheckBox();
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
            this.buttonDefaultSettings = new System.Windows.Forms.Button();
            this.comboBoxWowExpansion = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPageGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAnimationSpeed)).BeginInit();
            this.tabControlSettings.SuspendLayout();
            this.tabPageConnection.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonExitSettings
            // 
            this.buttonExitSettings.Location = new System.Drawing.Point(452, 213);
            this.buttonExitSettings.Name = "buttonExitSettings";
            this.buttonExitSettings.Size = new System.Drawing.Size(75, 23);
            this.buttonExitSettings.TabIndex = 9;
            this.buttonExitSettings.Text = "Exit";
            this.buttonExitSettings.UseVisualStyleBackColor = true;
            this.buttonExitSettings.Click += new System.EventHandler(this.buttonExitSettings_Click);
            // 
            // buttonSaveSettings
            // 
            this.buttonSaveSettings.Location = new System.Drawing.Point(272, 213);
            this.buttonSaveSettings.Name = "buttonSaveSettings";
            this.buttonSaveSettings.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveSettings.TabIndex = 7;
            this.buttonSaveSettings.Text = "Save";
            this.buttonSaveSettings.UseVisualStyleBackColor = true;
            this.buttonSaveSettings.Click += new System.EventHandler(this.buttonSaveSettings_Click);
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.label1);
            this.tabPageGeneral.Controls.Add(this.comboBoxWowExpansion);
            this.tabPageGeneral.Controls.Add(this.checkBoxDuplicatePrimaryFields);
            this.tabPageGeneral.Controls.Add(this.checkBoxPhaseHighlighting);
            this.tabPageGeneral.Controls.Add(this.checkBoxCreateRevertQuery);
            this.tabPageGeneral.Controls.Add(this.checkBoxAutoGenerateComments);
            this.tabPageGeneral.Controls.Add(this.checkBoxShowTooltipsStaticly);
            this.tabPageGeneral.Controls.Add(this.checkBoxChangeStaticInfo);
            this.tabPageGeneral.Controls.Add(this.checkBoxPromptExecuteQuery);
            this.tabPageGeneral.Controls.Add(this.textBoxAnimationSpeed);
            this.tabPageGeneral.Controls.Add(this.labelAnimationSpeed);
            this.tabPageGeneral.Controls.Add(this.trackBarAnimationSpeed);
            this.tabPageGeneral.Controls.Add(this.checkBoxPromptToQuit);
            this.tabPageGeneral.Controls.Add(this.checkBoxAutoSaveSettings);
            this.tabPageGeneral.Controls.Add(this.checkBoxInstantExpand);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(511, 173);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // checkBoxDuplicatePrimaryFields
            // 
            this.checkBoxDuplicatePrimaryFields.AutoSize = true;
            this.checkBoxDuplicatePrimaryFields.Location = new System.Drawing.Point(253, 52);
            this.checkBoxDuplicatePrimaryFields.Name = "checkBoxDuplicatePrimaryFields";
            this.checkBoxDuplicatePrimaryFields.Size = new System.Drawing.Size(253, 17);
            this.checkBoxDuplicatePrimaryFields.TabIndex = 10;
            this.checkBoxDuplicatePrimaryFields.Text = "Duplicate primary fields like `id` when duplicating";
            this.checkBoxDuplicatePrimaryFields.UseVisualStyleBackColor = true;
            // 
            // checkBoxPhaseHighlighting
            // 
            this.checkBoxPhaseHighlighting.AutoSize = true;
            this.checkBoxPhaseHighlighting.Location = new System.Drawing.Point(6, 144);
            this.checkBoxPhaseHighlighting.Name = "checkBoxPhaseHighlighting";
            this.checkBoxPhaseHighlighting.Size = new System.Drawing.Size(203, 17);
            this.checkBoxPhaseHighlighting.TabIndex = 10;
            this.checkBoxPhaseHighlighting.Text = "Show a different color for each phase";
            this.checkBoxPhaseHighlighting.UseVisualStyleBackColor = true;
            // 
            // checkBoxCreateRevertQuery
            // 
            this.checkBoxCreateRevertQuery.AutoSize = true;
            this.checkBoxCreateRevertQuery.Location = new System.Drawing.Point(253, 29);
            this.checkBoxCreateRevertQuery.Name = "checkBoxCreateRevertQuery";
            this.checkBoxCreateRevertQuery.Size = new System.Drawing.Size(181, 17);
            this.checkBoxCreateRevertQuery.TabIndex = 10;
            this.checkBoxCreateRevertQuery.Text = "Create a revert query on execute";
            this.checkBoxCreateRevertQuery.UseVisualStyleBackColor = true;
            // 
            // checkBoxAutoGenerateComments
            // 
            this.checkBoxAutoGenerateComments.AutoSize = true;
            this.checkBoxAutoGenerateComments.Location = new System.Drawing.Point(253, 6);
            this.checkBoxAutoGenerateComments.Name = "checkBoxAutoGenerateComments";
            this.checkBoxAutoGenerateComments.Size = new System.Drawing.Size(184, 17);
            this.checkBoxAutoGenerateComments.TabIndex = 13;
            this.checkBoxAutoGenerateComments.Text = "Automatically generate comments";
            this.checkBoxAutoGenerateComments.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowTooltipsStaticly
            // 
            this.checkBoxShowTooltipsStaticly.AutoSize = true;
            this.checkBoxShowTooltipsStaticly.Location = new System.Drawing.Point(6, 121);
            this.checkBoxShowTooltipsStaticly.Name = "checkBoxShowTooltipsStaticly";
            this.checkBoxShowTooltipsStaticly.Size = new System.Drawing.Size(149, 17);
            this.checkBoxShowTooltipsStaticly.TabIndex = 12;
            this.checkBoxShowTooltipsStaticly.Text = "Show tooltips permanently";
            this.checkBoxShowTooltipsStaticly.UseVisualStyleBackColor = true;
            // 
            // checkBoxChangeStaticInfo
            // 
            this.checkBoxChangeStaticInfo.AutoSize = true;
            this.checkBoxChangeStaticInfo.Checked = true;
            this.checkBoxChangeStaticInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxChangeStaticInfo.Location = new System.Drawing.Point(6, 98);
            this.checkBoxChangeStaticInfo.Name = "checkBoxChangeStaticInfo";
            this.checkBoxChangeStaticInfo.Size = new System.Drawing.Size(219, 17);
            this.checkBoxChangeStaticInfo.TabIndex = 11;
            this.checkBoxChangeStaticInfo.Text = "Change static script information on select";
            this.checkBoxChangeStaticInfo.UseVisualStyleBackColor = true;
            // 
            // checkBoxPromptExecuteQuery
            // 
            this.checkBoxPromptExecuteQuery.AutoSize = true;
            this.checkBoxPromptExecuteQuery.Checked = true;
            this.checkBoxPromptExecuteQuery.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxPromptExecuteQuery.Location = new System.Drawing.Point(6, 75);
            this.checkBoxPromptExecuteQuery.Name = "checkBoxPromptExecuteQuery";
            this.checkBoxPromptExecuteQuery.Size = new System.Drawing.Size(231, 17);
            this.checkBoxPromptExecuteQuery.TabIndex = 10;
            this.checkBoxPromptExecuteQuery.Text = "Ask me if I\'m sure I want to execute a query";
            this.checkBoxPromptExecuteQuery.UseVisualStyleBackColor = true;
            // 
            // textBoxAnimationSpeed
            // 
            this.textBoxAnimationSpeed.Enabled = false;
            this.textBoxAnimationSpeed.Location = new System.Drawing.Point(347, 124);
            this.textBoxAnimationSpeed.Name = "textBoxAnimationSpeed";
            this.textBoxAnimationSpeed.Size = new System.Drawing.Size(37, 20);
            this.textBoxAnimationSpeed.TabIndex = 8;
            this.textBoxAnimationSpeed.Text = "10";
            this.textBoxAnimationSpeed.TextChanged += new System.EventHandler(this.textBoxAnimationSpeed_TextChanged);
            // 
            // labelAnimationSpeed
            // 
            this.labelAnimationSpeed.AutoSize = true;
            this.labelAnimationSpeed.Location = new System.Drawing.Point(250, 127);
            this.labelAnimationSpeed.Name = "labelAnimationSpeed";
            this.labelAnimationSpeed.Size = new System.Drawing.Size(91, 13);
            this.labelAnimationSpeed.TabIndex = 7;
            this.labelAnimationSpeed.Text = "Animation speed: ";
            // 
            // trackBarAnimationSpeed
            // 
            this.trackBarAnimationSpeed.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.trackBarAnimationSpeed.Location = new System.Drawing.Point(243, 144);
            this.trackBarAnimationSpeed.Maximum = 12;
            this.trackBarAnimationSpeed.Minimum = 1;
            this.trackBarAnimationSpeed.Name = "trackBarAnimationSpeed";
            this.trackBarAnimationSpeed.Size = new System.Drawing.Size(272, 45);
            this.trackBarAnimationSpeed.TabIndex = 6;
            this.trackBarAnimationSpeed.Value = 10;
            this.trackBarAnimationSpeed.ValueChanged += new System.EventHandler(this.trackBarAnimationSpeed_ValueChanged);
            // 
            // checkBoxPromptToQuit
            // 
            this.checkBoxPromptToQuit.AutoSize = true;
            this.checkBoxPromptToQuit.Checked = true;
            this.checkBoxPromptToQuit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxPromptToQuit.Location = new System.Drawing.Point(6, 52);
            this.checkBoxPromptToQuit.Name = "checkBoxPromptToQuit";
            this.checkBoxPromptToQuit.Size = new System.Drawing.Size(171, 17);
            this.checkBoxPromptToQuit.TabIndex = 3;
            this.checkBoxPromptToQuit.Text = "Ask me if I\'m sure I want to exit";
            this.checkBoxPromptToQuit.UseVisualStyleBackColor = true;
            // 
            // checkBoxAutoSaveSettings
            // 
            this.checkBoxAutoSaveSettings.AutoSize = true;
            this.checkBoxAutoSaveSettings.Location = new System.Drawing.Point(6, 6);
            this.checkBoxAutoSaveSettings.Name = "checkBoxAutoSaveSettings";
            this.checkBoxAutoSaveSettings.Size = new System.Drawing.Size(113, 17);
            this.checkBoxAutoSaveSettings.TabIndex = 0;
            this.checkBoxAutoSaveSettings.Text = "Auto save settings";
            this.checkBoxAutoSaveSettings.UseVisualStyleBackColor = true;
            // 
            // checkBoxInstantExpand
            // 
            this.checkBoxInstantExpand.AutoSize = true;
            this.checkBoxInstantExpand.Location = new System.Drawing.Point(6, 29);
            this.checkBoxInstantExpand.Name = "checkBoxInstantExpand";
            this.checkBoxInstantExpand.Size = new System.Drawing.Size(166, 17);
            this.checkBoxInstantExpand.TabIndex = 1;
            this.checkBoxInstantExpand.Text = "Expand and contract instantly";
            this.checkBoxInstantExpand.UseVisualStyleBackColor = true;
            this.checkBoxInstantExpand.CheckedChanged += new System.EventHandler(this.checkBoxExpandInstantly_CheckedChanged);
            // 
            // tabControlSettings
            // 
            this.tabControlSettings.Controls.Add(this.tabPageGeneral);
            this.tabControlSettings.Controls.Add(this.tabPageConnection);
            this.tabControlSettings.Location = new System.Drawing.Point(12, 8);
            this.tabControlSettings.Name = "tabControlSettings";
            this.tabControlSettings.SelectedIndex = 0;
            this.tabControlSettings.Size = new System.Drawing.Size(519, 199);
            this.tabControlSettings.TabIndex = 1;
            // 
            // tabPageConnection
            // 
            this.tabPageConnection.Controls.Add(this.radioButtonDontUseDatabase);
            this.tabPageConnection.Controls.Add(this.radioButtonConnectToMySql);
            this.tabPageConnection.Controls.Add(this.buttonTestConnection);
            this.tabPageConnection.Controls.Add(this.buttonSearchForWorldDb);
            this.tabPageConnection.Controls.Add(this.checkBoxHidePass);
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
            this.tabPageConnection.Size = new System.Drawing.Size(511, 173);
            this.tabPageConnection.TabIndex = 1;
            this.tabPageConnection.Text = "Connection";
            this.tabPageConnection.UseVisualStyleBackColor = true;
            // 
            // radioButtonDontUseDatabase
            // 
            this.radioButtonDontUseDatabase.AutoSize = true;
            this.radioButtonDontUseDatabase.Location = new System.Drawing.Point(373, 37);
            this.radioButtonDontUseDatabase.Name = "radioButtonDontUseDatabase";
            this.radioButtonDontUseDatabase.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.radioButtonDontUseDatabase.Size = new System.Drawing.Size(126, 17);
            this.radioButtonDontUseDatabase.TabIndex = 33;
            this.radioButtonDontUseDatabase.TabStop = true;
            this.radioButtonDontUseDatabase.Text = "Don\'t use a database";
            this.radioButtonDontUseDatabase.UseVisualStyleBackColor = true;
            this.radioButtonDontUseDatabase.CheckedChanged += new System.EventHandler(this.radioButtonDontUseDatabase_CheckedChanged);
            // 
            // radioButtonConnectToMySql
            // 
            this.radioButtonConnectToMySql.AutoSize = true;
            this.radioButtonConnectToMySql.Location = new System.Drawing.Point(384, 14);
            this.radioButtonConnectToMySql.Name = "radioButtonConnectToMySql";
            this.radioButtonConnectToMySql.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.radioButtonConnectToMySql.Size = new System.Drawing.Size(115, 17);
            this.radioButtonConnectToMySql.TabIndex = 32;
            this.radioButtonConnectToMySql.TabStop = true;
            this.radioButtonConnectToMySql.Text = "Connect to MySQL";
            this.radioButtonConnectToMySql.UseVisualStyleBackColor = true;
            this.radioButtonConnectToMySql.CheckedChanged += new System.EventHandler(this.radioButtonConnectToMySql_CheckedChanged);
            // 
            // buttonTestConnection
            // 
            this.buttonTestConnection.Location = new System.Drawing.Point(387, 136);
            this.buttonTestConnection.Name = "buttonTestConnection";
            this.buttonTestConnection.Size = new System.Drawing.Size(112, 23);
            this.buttonTestConnection.TabIndex = 31;
            this.buttonTestConnection.Text = "Test connection";
            this.buttonTestConnection.UseVisualStyleBackColor = true;
            this.buttonTestConnection.Click += new System.EventHandler(this.buttonTestConnection_Click);
            // 
            // buttonSearchForWorldDb
            // 
            this.buttonSearchForWorldDb.Location = new System.Drawing.Point(212, 107);
            this.buttonSearchForWorldDb.Name = "buttonSearchForWorldDb";
            this.buttonSearchForWorldDb.Size = new System.Drawing.Size(24, 22);
            this.buttonSearchForWorldDb.TabIndex = 30;
            this.buttonSearchForWorldDb.Text = "...";
            this.buttonSearchForWorldDb.UseVisualStyleBackColor = true;
            this.buttonSearchForWorldDb.Click += new System.EventHandler(this.buttonSearchForWorldDb_Click);
            // 
            // checkBoxHidePass
            // 
            this.checkBoxHidePass.AutoSize = true;
            this.checkBoxHidePass.Location = new System.Drawing.Point(387, 111);
            this.checkBoxHidePass.Name = "checkBoxHidePass";
            this.checkBoxHidePass.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxHidePass.Size = new System.Drawing.Size(112, 17);
            this.checkBoxHidePass.TabIndex = 29;
            this.checkBoxHidePass.Text = "Hide my password";
            this.checkBoxHidePass.UseVisualStyleBackColor = true;
            this.checkBoxHidePass.CheckedChanged += new System.EventHandler(this.checkBoxDontHidePass_CheckedChanged);
            // 
            // checkBoxAutoConnect
            // 
            this.checkBoxAutoConnect.AutoSize = true;
            this.checkBoxAutoConnect.Location = new System.Drawing.Point(409, 88);
            this.checkBoxAutoConnect.Name = "checkBoxAutoConnect";
            this.checkBoxAutoConnect.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxAutoConnect.Size = new System.Drawing.Size(90, 17);
            this.checkBoxAutoConnect.TabIndex = 5;
            this.checkBoxAutoConnect.Text = "Auto connect";
            this.checkBoxAutoConnect.UseVisualStyleBackColor = true;
            // 
            // textBoxHost
            // 
            this.textBoxHost.Location = new System.Drawing.Point(106, 15);
            this.textBoxHost.Name = "textBoxHost";
            this.textBoxHost.Size = new System.Drawing.Size(130, 20);
            this.textBoxHost.TabIndex = 0;
            this.textBoxHost.KeyDown += new System.Windows.Forms.KeyEventHandler(this.settingTextBox_KeyDown);
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Location = new System.Drawing.Point(106, 46);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(130, 20);
            this.textBoxUsername.TabIndex = 1;
            this.textBoxUsername.KeyDown += new System.Windows.Forms.KeyEventHandler(this.settingTextBox_KeyDown);
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(106, 77);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(130, 20);
            this.textBoxPassword.TabIndex = 2;
            this.textBoxPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.settingTextBox_KeyDown);
            // 
            // textBoxWorldDatabase
            // 
            this.textBoxWorldDatabase.Location = new System.Drawing.Point(106, 108);
            this.textBoxWorldDatabase.Name = "textBoxWorldDatabase";
            this.textBoxWorldDatabase.Size = new System.Drawing.Size(106, 20);
            this.textBoxWorldDatabase.TabIndex = 3;
            this.textBoxWorldDatabase.KeyDown += new System.Windows.Forms.KeyEventHandler(this.settingTextBox_KeyDown);
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(106, 139);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(130, 20);
            this.textBoxPort.TabIndex = 4;
            this.textBoxPort.KeyDown += new System.Windows.Forms.KeyEventHandler(this.settingTextBox_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(42, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Username:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(44, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Password:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 111);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "World Database:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(68, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 26;
            this.label6.Text = "Host:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(71, 142);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 28;
            this.label7.Text = "Port:";
            // 
            // buttonDefaultSettings
            // 
            this.buttonDefaultSettings.Location = new System.Drawing.Point(362, 213);
            this.buttonDefaultSettings.Name = "buttonDefaultSettings";
            this.buttonDefaultSettings.Size = new System.Drawing.Size(75, 23);
            this.buttonDefaultSettings.TabIndex = 8;
            this.buttonDefaultSettings.Text = "Default";
            this.buttonDefaultSettings.UseVisualStyleBackColor = true;
            this.buttonDefaultSettings.Click += new System.EventHandler(this.buttonClearSettings_Click);
            // 
            // comboBoxWowExpansion
            // 
            this.comboBoxWowExpansion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWowExpansion.FormattingEnabled = true;
            this.comboBoxWowExpansion.Items.AddRange(new object[] {
            "Wrath of the Lich King",
            "Cataclysm",
            "Mists of the Pandaria",
            "Warlords of Draenor"});
            this.comboBoxWowExpansion.Location = new System.Drawing.Point(346, 73);
            this.comboBoxWowExpansion.Name = "comboBoxWowExpansion";
            this.comboBoxWowExpansion.Size = new System.Drawing.Size(159, 21);
            this.comboBoxWowExpansion.TabIndex = 10;
            this.comboBoxWowExpansion.SelectedIndexChanged += new System.EventHandler(this.comboBoxWowExpansion_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(253, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "WoW expansion:";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 247);
            this.Controls.Add(this.buttonDefaultSettings);
            this.Controls.Add(this.buttonSaveSettings);
            this.Controls.Add(this.buttonExitSettings);
            this.Controls.Add(this.tabControlSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SAI-Editor Settings";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SettingsForm_FormClosed);
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SettingsForm_KeyDown);
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAnimationSpeed)).EndInit();
            this.tabControlSettings.ResumeLayout(false);
            this.tabPageConnection.ResumeLayout(false);
            this.tabPageConnection.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonExitSettings;
        private System.Windows.Forms.Button buttonSaveSettings;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.TabControl tabControlSettings;
        private System.Windows.Forms.Button buttonDefaultSettings;
        private System.Windows.Forms.TabPage tabPageConnection;
        private System.Windows.Forms.CheckBox checkBoxAutoConnect;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox checkBoxInstantExpand;
        private System.Windows.Forms.CheckBox checkBoxAutoSaveSettings;
        private System.Windows.Forms.CheckBox checkBoxPromptToQuit;
        private System.Windows.Forms.Label labelAnimationSpeed;
        private System.Windows.Forms.TrackBar trackBarAnimationSpeed;
        private System.Windows.Forms.TextBox textBoxAnimationSpeed;
        private System.Windows.Forms.CheckBox checkBoxHidePass;
        private System.Windows.Forms.CheckBox checkBoxPromptExecuteQuery;
        private System.Windows.Forms.CheckBox checkBoxChangeStaticInfo;
        private System.Windows.Forms.Button buttonSearchForWorldDb;
        public System.Windows.Forms.TextBox textBoxHost;
        public System.Windows.Forms.TextBox textBoxUsername;
        public System.Windows.Forms.TextBox textBoxPassword;
        public System.Windows.Forms.TextBox textBoxWorldDatabase;
        public System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Button buttonTestConnection;
        private System.Windows.Forms.CheckBox checkBoxShowTooltipsStaticly;
        private System.Windows.Forms.CheckBox checkBoxAutoGenerateComments;
        private System.Windows.Forms.CheckBox checkBoxCreateRevertQuery;
        private System.Windows.Forms.RadioButton radioButtonDontUseDatabase;
        private System.Windows.Forms.RadioButton radioButtonConnectToMySql;
        public System.Windows.Forms.CheckBox checkBoxPhaseHighlighting;
        private System.Windows.Forms.CheckBox checkBoxDuplicatePrimaryFields;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxWowExpansion;

    }
}