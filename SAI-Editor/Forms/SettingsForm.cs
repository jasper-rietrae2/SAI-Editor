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
using SAI_Editor.Enumerators;

namespace SAI_Editor.Forms
{
    public partial class SettingsForm : Form
    {
        private bool closedFormManually = false;

        public SettingsForm()
        {
            InitializeComponent();

            textBoxHost.Text = Settings.Default.Host;
            textBoxUsername.Text = Settings.Default.User;
            textBoxPassword.Text = SAI_Editor_Manager.Instance.GetPasswordSetting();
            textBoxWorldDatabase.Text = Settings.Default.Database;
            textBoxPort.Text = Settings.Default.Port > 0 ? Settings.Default.Port.ToString() : String.Empty;

            checkBoxAutoConnect.Checked = Settings.Default.AutoConnect;
            checkBoxInstantExpand.Checked = Settings.Default.InstantExpand;
            checkBoxAutoSaveSettings.Checked = Settings.Default.AutoSaveSettings;
            checkBoxPromptToQuit.Checked = Settings.Default.PromptToQuit;
            checkBoxHidePass.Checked = Settings.Default.HidePass;
            checkBoxPromptExecuteQuery.Checked = Settings.Default.PromptExecuteQuery;
            checkBoxChangeStaticInfo.Checked = Settings.Default.ChangeStaticInfo;
            checkBoxShowTooltipsStaticly.Checked = Settings.Default.ShowTooltipsStaticly;
            checkBoxAutoGenerateComments.Checked = Settings.Default.GenerateComments;
            checkBoxCreateRevertQuery.Checked = Settings.Default.CreateRevertQuery;
            checkBoxPhaseHighlighting.Checked = Settings.Default.PhaseHighlighting;
            textBoxAnimationSpeed.Text = Settings.Default.AnimationSpeed.ToString();
            textBoxPassword.PasswordChar = Convert.ToChar(checkBoxHidePass.Checked ? '●' : '\0');
            radioButtonConnectToMySql.Checked = Settings.Default.UseWorldDatabase;
            radioButtonDontUseDatabase.Checked = !Settings.Default.UseWorldDatabase;
            checkBoxDuplicatePrimaryFields.Checked = Settings.Default.DuplicatePrimaryFields;
            comboBoxWowExpansion.SelectedIndex = (int)Settings.Default.WowExpansionIndex;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {

        }

        private void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            SaveSettings();
            closedFormManually = true;
            Close();
        }

        private void SaveSettings()
        {
            if (checkBoxChangeStaticInfo.Checked != Settings.Default.ChangeStaticInfo)
            {
                EntryOrGuidAndSourceType originalEntryOrGuidAndSourceType = ((MainForm)Owner).GetActiveUserControl().originalEntryOrGuidAndSourceType;
                ((MainForm)Owner).GetActiveUserControl().textBoxEntryOrGuid.Text = originalEntryOrGuidAndSourceType.entryOrGuid.ToString();
                ((MainForm)Owner).GetActiveUserControl().comboBoxSourceType.SelectedIndex = ((MainForm)Owner).GetActiveUserControl().GetIndexBySourceType(originalEntryOrGuidAndSourceType.sourceType);
            }

            bool showTooltipsStaticly = Settings.Default.ShowTooltipsStaticly;
            bool generateComments = Settings.Default.GenerateComments;
            bool phaseHighlighting = Settings.Default.PhaseHighlighting;

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[1024];

            rng.GetBytes(buffer);
            string salt = BitConverter.ToString(buffer);

            rng.Dispose();
            string decryptedPassword = textBoxPassword.Text;

            SAI_Editor_Manager.Instance.connString = new MySqlConnectionStringBuilder();
            SAI_Editor_Manager.Instance.connString.Server = textBoxHost.Text;
            SAI_Editor_Manager.Instance.connString.UserID = textBoxUsername.Text;
            SAI_Editor_Manager.Instance.connString.Port = CustomConverter.ToUInt32(textBoxPort.Text);
            SAI_Editor_Manager.Instance.connString.Database = textBoxWorldDatabase.Text;

            if (textBoxPassword.Text.Length > 0)
                SAI_Editor_Manager.Instance.connString.Password = textBoxPassword.Text;

            Settings.Default.UseWorldDatabase = radioButtonConnectToMySql.Checked;
            Settings.Default.Save();

            bool newConnectionSuccesfull = false;
            
            if (radioButtonConnectToMySql.Checked)
                newConnectionSuccesfull = SAI_Editor_Manager.Instance.worldDatabase.CanConnectToDatabase(SAI_Editor_Manager.Instance.connString, false);

            //! We also save the settings if no connection was made. It's possible the user unchecked
            //! the radioboxes, changed some settings and then set it back to no DB connection.
            if (newConnectionSuccesfull || !radioButtonConnectToMySql.Checked)
            {
                Settings.Default.Entropy = salt;
                Settings.Default.Host = textBoxHost.Text;
                Settings.Default.User = textBoxUsername.Text;
                Settings.Default.Password = textBoxPassword.Text.Length == 0 ? String.Empty : textBoxPassword.Text.ToSecureString().EncryptString(Encoding.Unicode.GetBytes(salt));
                Settings.Default.Database = textBoxWorldDatabase.Text;
                Settings.Default.Port = textBoxPort.Text.Length > 0 ? CustomConverter.ToUInt32(textBoxPort.Text) : 0;
                Settings.Default.AutoConnect = checkBoxAutoConnect.Checked;

                if (radioButtonConnectToMySql.Checked)
                    SAI_Editor_Manager.Instance.ResetDatabases();
            }

            Settings.Default.InstantExpand = checkBoxInstantExpand.Checked;
            Settings.Default.PromptToQuit = checkBoxPromptToQuit.Checked;
            Settings.Default.HidePass = checkBoxHidePass.Checked;
            Settings.Default.AnimationSpeed = CustomConverter.ToInt32(textBoxAnimationSpeed.Text);
            Settings.Default.PromptExecuteQuery = checkBoxPromptExecuteQuery.Checked;
            Settings.Default.ChangeStaticInfo = checkBoxChangeStaticInfo.Checked;
            Settings.Default.ShowTooltipsStaticly = checkBoxShowTooltipsStaticly.Checked;
            Settings.Default.GenerateComments = checkBoxAutoGenerateComments.Checked;
            Settings.Default.CreateRevertQuery = checkBoxCreateRevertQuery.Checked;
            Settings.Default.PhaseHighlighting = checkBoxPhaseHighlighting.Checked;
            Settings.Default.DuplicatePrimaryFields = checkBoxDuplicatePrimaryFields.Checked;
            Settings.Default.WowExpansionIndex = (uint)comboBoxWowExpansion.SelectedIndex;
            SAI_Editor_Manager.Instance.Expansion = (WowExpansion)Settings.Default.WowExpansionIndex;
            Settings.Default.Save();

            if (newConnectionSuccesfull)
            {
                ((MainForm)Owner).checkBoxAutoConnect.Checked = checkBoxAutoConnect.Checked;
                ((MainForm)Owner).textBoxHost.Text = textBoxHost.Text;
                ((MainForm)Owner).textBoxUsername.Text = textBoxUsername.Text;
                ((MainForm)Owner).textBoxPassword.Text = decryptedPassword;
                ((MainForm)Owner).textBoxWorldDatabase.Text = textBoxWorldDatabase.Text;
                ((MainForm)Owner).checkBoxAutoConnect.Checked = checkBoxAutoConnect.Checked;
                ((MainForm)Owner).expandAndContractSpeed = CustomConverter.ToInt32(textBoxAnimationSpeed.Text);
                ((MainForm)Owner).textBoxPassword.PasswordChar = Convert.ToChar(checkBoxHidePass.Checked ? '●' : '\0');
            }
            else if (radioButtonConnectToMySql.Checked) //! Don't report this if there is no connection to be made anyway
                MessageBox.Show("The database settings were not saved because no connection could be established. All other changed settings were saved.", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (checkBoxAutoGenerateComments.Checked != generateComments && checkBoxAutoGenerateComments.Checked)
                ((MainForm)Owner).GetActiveUserControl().GenerateCommentsForAllItems();
            
            if (checkBoxShowTooltipsStaticly.Checked != showTooltipsStaticly)
                ((MainForm)Owner).GetActiveUserControl().ExpandOrContractToShowStaticTooltips(!checkBoxShowTooltipsStaticly.Checked);

            if (checkBoxPhaseHighlighting.Checked != phaseHighlighting)
            {
                ((MainForm)Owner).GetActiveUserControl().ListViewList.Apply(true);
                ((MainForm)Owner).GetActiveUserControl().checkBoxUsePhaseColors.Checked = checkBoxPhaseHighlighting.Checked;
            }

            ((MainForm)Owner).HandleUseWorldDatabaseSettingChanged();
        }

        private void buttonExitSettings_Click(object sender, EventArgs e)
        {
            PromptSaveSettingsOnClose();
            closedFormManually = true;
            Close();
        }

        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //! Only call this prompt method if the form was closed by the user itself and the form was not
            //! already closed before because we called Form::Close() (by pressing 'Exit' or so).
            if (e.CloseReason == CloseReason.UserClosing && !closedFormManually)
                PromptSaveSettingsOnClose();
        }

        private void PromptSaveSettingsOnClose()
        {
            if (checkBoxAutoSaveSettings.Checked)
            {
                Settings.Default.AutoSaveSettings = true;
                SaveSettings();
                return;
            }

            if (textBoxHost.Text == Settings.Default.Host &&
                textBoxUsername.Text == Settings.Default.User &&
                textBoxWorldDatabase.Text == Settings.Default.Database &&
                textBoxPort.Text == Settings.Default.Port.ToString() &&
                checkBoxAutoConnect.Checked == Settings.Default.AutoConnect &&
                checkBoxInstantExpand.Checked == Settings.Default.InstantExpand &&
                checkBoxHidePass.Checked == Settings.Default.HidePass &&
                checkBoxPromptToQuit.Checked == Settings.Default.PromptToQuit &&
                textBoxAnimationSpeed.Text == Settings.Default.AnimationSpeed.ToString() &&
                checkBoxPromptExecuteQuery.Checked == Settings.Default.PromptExecuteQuery &&
                checkBoxChangeStaticInfo.Checked == Settings.Default.ChangeStaticInfo &&
                checkBoxShowTooltipsStaticly.Checked == Settings.Default.ShowTooltipsStaticly &&
                checkBoxAutoGenerateComments.Checked == Settings.Default.GenerateComments &&
                checkBoxCreateRevertQuery.Checked == Settings.Default.CreateRevertQuery &&
                checkBoxPhaseHighlighting.Checked == Settings.Default.PhaseHighlighting &&
                checkBoxDuplicatePrimaryFields.Checked == Settings.Default.DuplicatePrimaryFields &&
                comboBoxWowExpansion.SelectedIndex == (int)Settings.Default.WowExpansionIndex &&

                //! Check password last because it's the most 'expensive' check
                textBoxPassword.Text == SAI_Editor_Manager.Instance.GetPasswordSetting())
                return;

            DialogResult dialogResult = MessageBox.Show("Do you wish to save the edited settings?", "Save settings", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
                SaveSettings();
        }

        private void buttonClearSettings_Click(object sender, EventArgs e)
        {
            switch (tabControlSettings.SelectedIndex)
            {
                case 0: //! 'General' tab
                    checkBoxInstantExpand.Checked = false;
                    checkBoxAutoSaveSettings.Checked = false;
                    checkBoxInstantExpand.Checked = false;
                    checkBoxPromptToQuit.Checked = false;
                    textBoxAnimationSpeed.Text = "10";
                    trackBarAnimationSpeed.Value = 10;
                    checkBoxPromptExecuteQuery.Checked = true;
                    checkBoxChangeStaticInfo.Checked = true;
                    checkBoxShowTooltipsStaticly.Checked = false;
                    checkBoxAutoGenerateComments.Checked = false;
                    checkBoxCreateRevertQuery.Checked = false;
                    checkBoxPhaseHighlighting.Checked = true;
                    checkBoxDuplicatePrimaryFields.Checked = false;
                    comboBoxWowExpansion.SelectedIndex = 0;
                    break;
                case 1: // ! 'Connection' tab
                    textBoxHost.Text = "";
                    textBoxUsername.Text = "";
                    textBoxPassword.Text = "";
                    textBoxWorldDatabase.Text = "";
                    textBoxPort.Text = "";
                    checkBoxAutoConnect.Checked = false;
                    checkBoxHidePass.Checked = true;
                    break;
            }
        }

        private void SettingsForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    PromptSaveSettingsOnClose();
                    closedFormManually = true;
                    Close();
                    break;
            }
        }

        private void trackBarAnimationSpeed_ValueChanged(object sender, EventArgs e)
        {
            textBoxAnimationSpeed.Text = trackBarAnimationSpeed.Value.ToString();
        }

        private void textBoxAnimationSpeed_TextChanged(object sender, EventArgs e)
        {
            int newValue = CustomConverter.ToInt32(textBoxAnimationSpeed.Text);

            if (newValue > 12)
                newValue = 12;

            if (newValue < 1)
                newValue = 1;

            trackBarAnimationSpeed.Value = newValue;

            if (newValue != CustomConverter.ToInt32(textBoxAnimationSpeed.Text))
                textBoxAnimationSpeed.Text = newValue.ToString();
        }

        private void checkBoxExpandInstantly_CheckedChanged(object sender, EventArgs e)
        {
            trackBarAnimationSpeed.Enabled = !checkBoxInstantExpand.Checked;
        }

        private void checkBoxDontHidePass_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPassword.PasswordChar = Convert.ToChar(checkBoxHidePass.Checked ? '●' : '\0');
        }

        private async void buttonSearchForWorldDb_Click(object sender, EventArgs e)
        {
            buttonSearchForWorldDb.Enabled = false;

            WorldDatabase worldDatabase = new WorldDatabase(textBoxHost.Text, CustomConverter.ToUInt32(textBoxPort.Text), textBoxUsername.Text, textBoxPassword.Text, "");
            List<string> databaseNames = await SAI_Editor_Manager.Instance.GetDatabasesInConnection(textBoxHost.Text, textBoxUsername.Text, CustomConverter.ToUInt32(textBoxPort.Text), textBoxPassword.Text, worldDatabase);

            if (databaseNames != null && databaseNames.Count > 0)
                using (var selectDatabaseForm = new SelectDatabaseForm(databaseNames, textBoxWorldDatabase))
                    selectDatabaseForm.ShowDialog(this);

            buttonSearchForWorldDb.Enabled = true;
        }

        private void settingTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                buttonSaveSettings_Click(sender, e);
        }

        private void buttonTestConnection_Click(object sender, EventArgs e)
        {
            buttonTestConnection.Enabled = false;

            MySqlConnectionStringBuilder _connectionString = new MySqlConnectionStringBuilder();
            _connectionString.Server = textBoxHost.Text;
            _connectionString.UserID = textBoxUsername.Text;
            _connectionString.Port = CustomConverter.ToUInt32(textBoxPort.Text);
            _connectionString.Database = textBoxWorldDatabase.Text;

            if (textBoxPassword.Text.Length > 0)
                _connectionString.Password = textBoxPassword.Text;

            WorldDatabase worldDatabase = null;

            if (!Settings.Default.UseWorldDatabase)
                worldDatabase = new WorldDatabase(Settings.Default.Host, Settings.Default.Port, Settings.Default.User, SAI_Editor_Manager.Instance.GetPasswordSetting(), Settings.Default.Database);
            else
                worldDatabase = SAI_Editor_Manager.Instance.worldDatabase;

            //! If no connection was established, it would throw an error in WorldDatabase.CanConnectToDatabase.
            if (worldDatabase.CanConnectToDatabase(_connectionString))
                MessageBox.Show("Connection successful!", "Connection status", MessageBoxButtons.OK, MessageBoxIcon.Information);

            buttonTestConnection.Enabled = true;
        }

        private void radioButtonConnectToMySql_CheckedChanged(object sender, EventArgs e)
        {
            HandleRadioButtonUseDatabaseChanged();
        }

        private void radioButtonDontUseDatabase_CheckedChanged(object sender, EventArgs e)
        {
            HandleRadioButtonUseDatabaseChanged();
        }

        private void HandleRadioButtonUseDatabaseChanged()
        {
            textBoxHost.Enabled = radioButtonConnectToMySql.Checked;
            textBoxUsername.Enabled = radioButtonConnectToMySql.Checked;
            textBoxPassword.Enabled = radioButtonConnectToMySql.Checked;
            textBoxWorldDatabase.Enabled = radioButtonConnectToMySql.Checked;
            textBoxPort.Enabled = radioButtonConnectToMySql.Checked;
            buttonSearchForWorldDb.Enabled = radioButtonConnectToMySql.Checked;
            buttonTestConnection.Enabled = radioButtonConnectToMySql.Checked;
        }

        private void comboBoxWowExpansion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxWowExpansion.SelectedIndex == -1)
            {
                comboBoxWowExpansion.SelectedIndex = 0;
                return;
            }

            switch ((WowExpansion)comboBoxWowExpansion.SelectedIndex)
            {
                case WowExpansion.ExpansionMop:
                case WowExpansion.ExpansionWod:
                    MessageBox.Show("This expansion is currently not supported.", "Not supported", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    comboBoxWowExpansion.SelectedIndex = 0;
                    break;
            }
        }
    }
}
