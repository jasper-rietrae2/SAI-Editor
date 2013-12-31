using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using SAI_Editor.Classes;
using SAI_Editor.Classes.Database;
using SAI_Editor.Forms.SearchForms;
using SAI_Editor.Properties;

namespace SAI_Editor.Forms
{

    public partial class SettingsForm : Form
    {
        private bool closedFormByHand = false;
        MySqlConnectionStringBuilder connectionString = new MySqlConnectionStringBuilder();

        public SettingsForm()
        {
            this.InitializeComponent();

            this.textBoxHost.Text = Settings.Default.Host;
            this.textBoxUsername.Text = Settings.Default.User;
            this.textBoxPassword.Text = SAI_Editor_Manager.Instance.GetPasswordSetting();
            this.textBoxWorldDatabase.Text = Settings.Default.Database;
            this.textBoxPort.Text = Settings.Default.Port > 0 ? Settings.Default.Port.ToString() : String.Empty;

            this.checkBoxAutoConnect.Checked = Settings.Default.AutoConnect;
            this.checkBoxInstantExpand.Checked = Settings.Default.InstantExpand;
            this.checkBoxAutoSaveSettings.Checked = Settings.Default.AutoSaveSettings;
            this.checkBoxPromptToQuit.Checked = Settings.Default.PromptToQuit;
            this.checkBoxHidePass.Checked = Settings.Default.HidePass;
            this.checkBoxPromptExecuteQuery.Checked = Settings.Default.PromptExecuteQuery;
            this.checkBoxChangeStaticInfo.Checked = Settings.Default.ChangeStaticInfo;
            this.checkBoxShowTooltipsPermanently.Checked = Settings.Default.ShowTooltipsPermanently;
            this.checkBoxAutoGenerateComments.Checked = Settings.Default.GenerateComments;
            this.checkBoxCreateRevertQuery.Checked = Settings.Default.CreateRevertQuery;
            this.checkBoxPhaseHighlighting.Checked = Settings.Default.PhaseHighlighting;
            this.textBoxAnimationSpeed.Text = Settings.Default.AnimationSpeed.ToString();
            this.textBoxPassword.PasswordChar = Convert.ToChar(this.checkBoxHidePass.Checked ? '●' : '\0');
            this.radioButtonConnectToMySql.Checked = Settings.Default.UseWorldDatabase;
            this.radioButtonDontUseDatabase.Checked = !Settings.Default.UseWorldDatabase;
        }

        private void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            this.SaveSettings();
            this.closedFormByHand = true;
            this.Close();
        }

        private void SaveSettings()
        {
            if (this.checkBoxChangeStaticInfo.Checked != Settings.Default.ChangeStaticInfo)
            {
                EntryOrGuidAndSourceType originalEntryOrGuidAndSourceType = ((MainForm)this.Owner).originalEntryOrGuidAndSourceType;
                ((MainForm)this.Owner).textBoxEntryOrGuid.Text = originalEntryOrGuidAndSourceType.entryOrGuid.ToString();
                ((MainForm)this.Owner).comboBoxSourceType.SelectedIndex = ((MainForm)this.Owner).GetIndexBySourceType(originalEntryOrGuidAndSourceType.sourceType);
            }

            bool showTooltipsPermanently = Settings.Default.ShowTooltipsPermanently;
            bool generateComments = Settings.Default.GenerateComments;
            bool phaseHighlighting = Settings.Default.PhaseHighlighting;

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[1024];

            rng.GetBytes(buffer);
            string salt = BitConverter.ToString(buffer);

            rng.Dispose();
            string decryptedPassword = this.textBoxPassword.Text;

            this.connectionString = new MySqlConnectionStringBuilder();
            this.connectionString.Server = this.textBoxHost.Text;
            this.connectionString.UserID = this.textBoxUsername.Text;
            this.connectionString.Port = XConverter.ToUInt32(this.textBoxPort.Text);
            this.connectionString.Database = this.textBoxWorldDatabase.Text;

            if (this.textBoxPassword.Text.Length > 0)
                this.connectionString.Password = this.textBoxPassword.Text;

            Settings.Default.UseWorldDatabase = this.radioButtonConnectToMySql.Checked;
            Settings.Default.Save();

            bool newConnectionSuccesfull = false;
            
            if (this.radioButtonConnectToMySql.Checked)
                newConnectionSuccesfull = SAI_Editor_Manager.Instance.worldDatabase.CanConnectToDatabase(this.connectionString, false);

            //! We also save the settings if no connection was made. It's possible the user unchecked
            //! the radioboxes, changed some settings and then set it back to no DB connection.
            if (newConnectionSuccesfull || !this.radioButtonConnectToMySql.Checked)
            {
                Settings.Default.Entropy = salt;
                Settings.Default.Host = this.textBoxHost.Text;
                Settings.Default.User = this.textBoxUsername.Text;
                Settings.Default.Password = this.textBoxPassword.Text.Length == 0 ? String.Empty : this.textBoxPassword.Text.ToSecureString().EncryptString(Encoding.Unicode.GetBytes(salt));
                Settings.Default.Database = this.textBoxWorldDatabase.Text;
                Settings.Default.Port = this.textBoxPort.Text.Length > 0 ? XConverter.ToUInt32(this.textBoxPort.Text) : 0;
                Settings.Default.AutoConnect = this.checkBoxAutoConnect.Checked;

                if (this.radioButtonConnectToMySql.Checked)
                {
                    ((MainForm)this.Owner).connectionString = this.connectionString;
                    SAI_Editor_Manager.Instance.ResetDatabases();
                }
            }

            Settings.Default.InstantExpand = this.checkBoxInstantExpand.Checked;
            Settings.Default.PromptToQuit = this.checkBoxPromptToQuit.Checked;
            Settings.Default.HidePass = this.checkBoxHidePass.Checked;
            Settings.Default.AnimationSpeed = XConverter.ToInt32(this.textBoxAnimationSpeed.Text);
            Settings.Default.PromptExecuteQuery = this.checkBoxPromptExecuteQuery.Checked;
            Settings.Default.ChangeStaticInfo = this.checkBoxChangeStaticInfo.Checked;
            Settings.Default.ShowTooltipsPermanently = this.checkBoxShowTooltipsPermanently.Checked;
            Settings.Default.GenerateComments = this.checkBoxAutoGenerateComments.Checked;
            Settings.Default.CreateRevertQuery = this.checkBoxCreateRevertQuery.Checked;
            Settings.Default.PhaseHighlighting = this.checkBoxPhaseHighlighting.Checked;
            Settings.Default.Save();

            if (newConnectionSuccesfull)
            {
                ((MainForm)this.Owner).checkBoxAutoConnect.Checked = this.checkBoxAutoConnect.Checked;
                ((MainForm)this.Owner).textBoxHost.Text = this.textBoxHost.Text;
                ((MainForm)this.Owner).textBoxUsername.Text = this.textBoxUsername.Text;
                ((MainForm)this.Owner).textBoxPassword.Text = decryptedPassword;
                ((MainForm)this.Owner).textBoxWorldDatabase.Text = this.textBoxWorldDatabase.Text;
                ((MainForm)this.Owner).checkBoxAutoConnect.Checked = this.checkBoxAutoConnect.Checked;
                ((MainForm)this.Owner).expandAndContractSpeed = XConverter.ToInt32(this.textBoxAnimationSpeed.Text);
                ((MainForm)this.Owner).textBoxPassword.PasswordChar = Convert.ToChar(this.checkBoxHidePass.Checked ? '●' : '\0');
            }
            else if (this.radioButtonConnectToMySql.Checked) //! Don't report this if there is no connection to be made anyway
                MessageBox.Show("The database settings were not saved because no connection could be established. All other changed settings were saved.", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (this.checkBoxAutoGenerateComments.Checked != generateComments && this.checkBoxAutoGenerateComments.Checked)
                ((MainForm)this.Owner).GenerateCommentsForAllItems();
            
            if (this.checkBoxShowTooltipsPermanently.Checked != showTooltipsPermanently)
                ((MainForm)this.Owner).ExpandToShowPermanentTooltips(!this.checkBoxShowTooltipsPermanently.Checked);

            if (this.checkBoxPhaseHighlighting.Checked != phaseHighlighting)
                ((MainForm)this.Owner).listViewSmartScripts.Init(true);

            ((MainForm)this.Owner).HandleUseWorldDatabaseSettingChanged();
        }

        private void buttonExitSettings_Click(object sender, EventArgs e)
        {
            this.PromptSaveSettingsOnClose();
            this.closedFormByHand = true;
            this.Close();
        }

        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //! Only call this prompt method if the form was closed by the user itself and the form was not
            //! already closed before because we called Form::Close() (by pressing 'Exit' or so).
            if (e.CloseReason == CloseReason.UserClosing && !this.closedFormByHand)
                this.PromptSaveSettingsOnClose();
        }

        private void PromptSaveSettingsOnClose()
        {
            if (this.checkBoxAutoSaveSettings.Checked)
            {
                Settings.Default.AutoSaveSettings = true;
                this.SaveSettings();
                return;
            }

            if (this.textBoxHost.Text == Settings.Default.Host &&
                this.textBoxUsername.Text == Settings.Default.User &&
                this.textBoxWorldDatabase.Text == Settings.Default.Database &&
                this.textBoxPort.Text == Settings.Default.Port.ToString() &&
                this.checkBoxAutoConnect.Checked == Settings.Default.AutoConnect &&
                this.checkBoxInstantExpand.Checked == Settings.Default.InstantExpand &&
                this.checkBoxHidePass.Checked == Settings.Default.HidePass &&
                this.checkBoxPromptToQuit.Checked == Settings.Default.PromptToQuit &&
                this.textBoxAnimationSpeed.Text == Settings.Default.AnimationSpeed.ToString() &&
                this.checkBoxPromptExecuteQuery.Checked == Settings.Default.PromptExecuteQuery &&
                this.checkBoxChangeStaticInfo.Checked == Settings.Default.ChangeStaticInfo &&
                this.checkBoxShowTooltipsPermanently.Checked == Settings.Default.ShowTooltipsPermanently &&
                this.checkBoxAutoGenerateComments.Checked == Settings.Default.GenerateComments &&
                this.checkBoxCreateRevertQuery.Checked == Settings.Default.CreateRevertQuery &&
                this.checkBoxPhaseHighlighting.Checked == Settings.Default.PhaseHighlighting &&

                //! Check password last because it's the most 'expensive' check
                this.textBoxPassword.Text == SAI_Editor_Manager.Instance.GetPasswordSetting())
                return;

            DialogResult dialogResult = MessageBox.Show("Do you wish to save the edited settings?", "Save settings?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
                this.SaveSettings();
        }

        private void buttonClearSettings_Click(object sender, EventArgs e)
        {
            switch (this.tabControlSettings.SelectedIndex)
            {
                case 0: //! 'General' tab
                    this.checkBoxInstantExpand.Checked = false;
                    this.checkBoxAutoSaveSettings.Checked = false;
                    this.checkBoxInstantExpand.Checked = false;
                    this.checkBoxPromptToQuit.Checked = false;
                    this.textBoxAnimationSpeed.Text = "10";
                    this.trackBarAnimationSpeed.Value = 10;
                    this.checkBoxPromptExecuteQuery.Checked = true;
                    this.checkBoxChangeStaticInfo.Checked = true;
                    this.checkBoxShowTooltipsPermanently.Checked = false;
                    this.checkBoxAutoGenerateComments.Checked = false;
                    this.checkBoxCreateRevertQuery.Checked = false;
                    this.checkBoxPhaseHighlighting.Checked = true;
                    break;
                case 1: // ! 'Connection' tab
                    this.textBoxHost.Text = "";
                    this.textBoxUsername.Text = "";
                    this.textBoxPassword.Text = "";
                    this.textBoxWorldDatabase.Text = "";
                    this.textBoxPort.Text = "";
                    this.checkBoxAutoConnect.Checked = false;
                    this.checkBoxHidePass.Checked = true;
                    break;
            }
        }

        private void SettingsForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.PromptSaveSettingsOnClose();
                    this.closedFormByHand = true;
                    this.Close();
                    break;
            }
        }

        private void trackBarAnimationSpeed_ValueChanged(object sender, EventArgs e)
        {
            this.textBoxAnimationSpeed.Text = this.trackBarAnimationSpeed.Value.ToString();
        }

        private void textBoxAnimationSpeed_TextChanged(object sender, EventArgs e)
        {
            int newValue = XConverter.ToInt32(this.textBoxAnimationSpeed.Text);

            if (newValue > 12)
                newValue = 12;

            if (newValue < 1)
                newValue = 1;

            this.trackBarAnimationSpeed.Value = newValue;

            if (newValue != XConverter.ToInt32(this.textBoxAnimationSpeed.Text))
                this.textBoxAnimationSpeed.Text = newValue.ToString();
        }

        private void checkBoxExpandInstantly_CheckedChanged(object sender, EventArgs e)
        {
            this.trackBarAnimationSpeed.Enabled = !this.checkBoxInstantExpand.Checked;
        }

        private void numericField_KeyPress(object sender, KeyPressEventArgs e)
        {
            //! Only allow typing keys that are numbers
            //if (!Char.IsNumber(e.KeyChar))
            //    e.Handled = e.KeyChar != (char)Keys.Back;
        }

        private void checkBoxDontHidePass_CheckedChanged(object sender, EventArgs e)
        {
            this.textBoxPassword.PasswordChar = Convert.ToChar(this.checkBoxHidePass.Checked ? '●' : '\0');
        }

        private async void buttonSearchForWorldDb_Click(object sender, EventArgs e)
        {
            WorldDatabase worldDatabase = new WorldDatabase(this.textBoxHost.Text, XConverter.ToUInt32(this.textBoxPort.Text), this.textBoxUsername.Text, this.textBoxPassword.Text, "");
            List<string> databaseNames = await SAI_Editor_Manager.Instance.GetDatabasesInConnection(this.textBoxHost.Text, this.textBoxUsername.Text, XConverter.ToUInt32(this.textBoxPort.Text), this.textBoxPassword.Text, worldDatabase);

            if (databaseNames != null && databaseNames.Count > 0)
                using (var selectDatabaseForm = new SelectDatabaseForm(databaseNames, this.textBoxWorldDatabase))
                    selectDatabaseForm.ShowDialog(this);
        }

        private void settingTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.buttonSaveSettings_Click(sender, e);
        }

        private void buttonTestConnection_Click(object sender, EventArgs e)
        {
            MySqlConnectionStringBuilder _connectionString = new MySqlConnectionStringBuilder();
            _connectionString.Server = this.textBoxHost.Text;
            _connectionString.UserID = this.textBoxUsername.Text;
            _connectionString.Port = XConverter.ToUInt32(this.textBoxPort.Text);
            _connectionString.Database = this.textBoxWorldDatabase.Text;

            if (this.textBoxPassword.Text.Length > 0)
                _connectionString.Password = this.textBoxPassword.Text;

            WorldDatabase worldDatabase = null;

            if (!Settings.Default.UseWorldDatabase)
                worldDatabase = new WorldDatabase(Settings.Default.Host, Settings.Default.Port, Settings.Default.User, SAI_Editor_Manager.Instance.GetPasswordSetting(), Settings.Default.Database);
            else
                worldDatabase = SAI_Editor_Manager.Instance.worldDatabase;

            //! If no connection was established, it would throw an error in WorldDatabase.CanConnectToDatabase.
            if (worldDatabase.CanConnectToDatabase(_connectionString))
                MessageBox.Show("Connection successful!", "Connection status", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void radioButtonConnectToMySql_CheckedChanged(object sender, EventArgs e)
        {
            this.HandleRadioButtonUseDatabaseChanged();
        }

        private void radioButtonDontUseDatabase_CheckedChanged(object sender, EventArgs e)
        {
            this.HandleRadioButtonUseDatabaseChanged();
        }

        private void HandleRadioButtonUseDatabaseChanged()
        {
            this.textBoxHost.Enabled = this.radioButtonConnectToMySql.Checked;
            this.textBoxUsername.Enabled = this.radioButtonConnectToMySql.Checked;
            this.textBoxPassword.Enabled = this.radioButtonConnectToMySql.Checked;
            this.textBoxWorldDatabase.Enabled = this.radioButtonConnectToMySql.Checked;
            this.textBoxPort.Enabled = this.radioButtonConnectToMySql.Checked;
            this.buttonSearchForWorldDb.Enabled = this.radioButtonConnectToMySql.Checked;
            this.buttonTestConnection.Enabled = this.radioButtonConnectToMySql.Checked;
        }
    }
}
