using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using SAI_Editor.Classes;
using SAI_Editor.Properties;
using SAI_Editor.Security;

namespace SAI_Editor
{
    public partial class SettingsForm : Form
    {
        private bool closedFormByHand = false;
        MySqlConnectionStringBuilder connectionString = new MySqlConnectionStringBuilder();

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            textBoxHost.Text = Settings.Default.Host;
            textBoxUsername.Text = Settings.Default.User;
            textBoxPassword.Text = Settings.Default.Password.DecryptString(Encoding.Unicode.GetBytes(Settings.Default.Entropy)).ToInsecureString();
            textBoxWorldDatabase.Text = Settings.Default.Database;
            textBoxPort.Text = Settings.Default.Port > 0 ? Settings.Default.Port.ToString() : String.Empty;

            checkBoxAutoConnect.Checked = Settings.Default.AutoConnect;
            checkBoxInstantExpand.Checked = Settings.Default.InstantExpand;
            checkBoxLoadScriptInstantly.Checked = Settings.Default.LoadScriptInstantly;
            checkBoxAutoSaveSettings.Checked = Settings.Default.AutoSaveSettings;
            checkBoxPromptToQuit.Checked = Settings.Default.PromptToQuit;
            checkBoxHidePass.Checked = Settings.Default.HidePass;
            checkBoxPromptExecuteQuery.Checked = Settings.Default.PromptExecuteQuery;
            checkBoxChangeStaticInfo.Checked = Settings.Default.ChangeStaticInfo;
            checkBoxShowTooltipsPermanently.Checked = Settings.Default.ShowTooltipsPermanently;
            checkBoxAutoGenerateComments.Checked = Settings.Default.GenerateComments;
            checkBoxCreateRevertQuery.Checked = Settings.Default.CreateRevertQuery;

            textBoxAnimationSpeed.Text = Settings.Default.AnimationSpeed.ToString();
            textBoxPassword.PasswordChar = Convert.ToChar(checkBoxHidePass.Checked ? '●' : '\0');
        }

        private void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            SaveSettings();
            closedFormByHand = true;
            Close();
        }

        private void SaveSettings()
        {
            if (checkBoxChangeStaticInfo.Checked != Settings.Default.ChangeStaticInfo)
            {
                EntryOrGuidAndSourceType originalEntryOrGuidAndSourceType = ((MainForm)Owner).originalEntryOrGuidAndSourceType;
                ((MainForm)Owner).textBoxEntryOrGuid.Text = originalEntryOrGuidAndSourceType.entryOrGuid.ToString();
                ((MainForm)Owner).comboBoxSourceType.SelectedIndex = ((MainForm)Owner).GetIndexBySourceType(originalEntryOrGuidAndSourceType.sourceType);
            }

            bool showTooltipsPermanently = Settings.Default.ShowTooltipsPermanently;
            bool generateComments = Settings.Default.GenerateComments;

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[1024];

            rng.GetBytes(buffer);
            string salt = BitConverter.ToString(buffer);

            rng.Dispose();
            string decryptedPassword = textBoxPassword.Text;

            connectionString.Server = textBoxHost.Text;
            connectionString.UserID = textBoxUsername.Text;
            connectionString.Port = XConverter.ToUInt32(textBoxPort.Text);
            connectionString.Database = textBoxWorldDatabase.Text;

            if (textBoxPassword.Text.Length > 0)
                connectionString.Password = textBoxPassword.Text;

            bool newConnectionSuccesfull = SAI_Editor_Manager.Instance.worldDatabase.CanConnectToDatabase(connectionString, false);

            if (newConnectionSuccesfull)
            {
                Settings.Default.Entropy = salt;
                Settings.Default.Host = textBoxHost.Text;
                Settings.Default.User = textBoxUsername.Text;
                Settings.Default.Password = decryptedPassword.ToSecureString().EncryptString(Encoding.Unicode.GetBytes(salt));
                Settings.Default.Database = textBoxWorldDatabase.Text;
                Settings.Default.Port = textBoxPort.Text.Length > 0 ? XConverter.ToUInt32(textBoxPort.Text) : 0;
                Settings.Default.AutoConnect = checkBoxAutoConnect.Checked;
                ((MainForm)Owner).connectionString = connectionString;
                SAI_Editor_Manager.Instance.ResetDatabases();
            }

            Settings.Default.InstantExpand = checkBoxInstantExpand.Checked;
            Settings.Default.LoadScriptInstantly = checkBoxLoadScriptInstantly.Checked;
            Settings.Default.AutoSaveSettings = checkBoxAutoSaveSettings.Checked;
            Settings.Default.PromptToQuit = checkBoxPromptToQuit.Checked;
            Settings.Default.HidePass = checkBoxHidePass.Checked;
            Settings.Default.AnimationSpeed = XConverter.ToInt32(textBoxAnimationSpeed.Text);
            Settings.Default.PromptExecuteQuery = checkBoxPromptExecuteQuery.Checked;
            Settings.Default.ChangeStaticInfo = checkBoxChangeStaticInfo.Checked;
            Settings.Default.ShowTooltipsPermanently = checkBoxShowTooltipsPermanently.Checked;
            Settings.Default.GenerateComments = checkBoxAutoGenerateComments.Checked;
            Settings.Default.CreateRevertQuery = checkBoxCreateRevertQuery.Checked;
            Settings.Default.Save();

            if (newConnectionSuccesfull)
            {
                ((MainForm)Owner).checkBoxAutoConnect.Checked = checkBoxAutoConnect.Checked;
                ((MainForm)Owner).textBoxHost.Text = textBoxHost.Text;
                ((MainForm)Owner).textBoxUsername.Text = textBoxUsername.Text;
                ((MainForm)Owner).textBoxPassword.Text = decryptedPassword;
                ((MainForm)Owner).textBoxWorldDatabase.Text = textBoxWorldDatabase.Text;
                ((MainForm)Owner).checkBoxAutoConnect.Checked = checkBoxAutoConnect.Checked;
                ((MainForm)Owner).expandAndContractSpeed = XConverter.ToInt32(textBoxAnimationSpeed.Text);
                ((MainForm)Owner).textBoxPassword.PasswordChar = Convert.ToChar(checkBoxHidePass.Checked ? '●' : '\0');
            }
            else
                MessageBox.Show("The database settings were not saved because no connection could be established.", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (checkBoxAutoGenerateComments.Checked != generateComments && checkBoxAutoGenerateComments.Checked)
                ((MainForm)Owner).GenerateCommentsForAllItems();
            
            if (checkBoxShowTooltipsPermanently.Checked != showTooltipsPermanently)
                ((MainForm)Owner).ExpandToShowPermanentTooltips(!checkBoxShowTooltipsPermanently.Checked);
        }

        private void buttonExitSettings_Click(object sender, EventArgs e)
        {
            PromptSaveSettingsOnClose();
            closedFormByHand = true;
            Close();
        }

        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //! Only call this prompt method if the form was closed by the user itself and the form was not
            //! already closed before because we called Form::Close() (by pressing 'Exit' or so).
            if (e.CloseReason == CloseReason.UserClosing && !closedFormByHand)
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
                checkBoxLoadScriptInstantly.Checked == Settings.Default.LoadScriptInstantly &&
                checkBoxHidePass.Checked == Settings.Default.HidePass &&
                checkBoxPromptToQuit.Checked == Settings.Default.PromptToQuit &&
                textBoxAnimationSpeed.Text == Settings.Default.AnimationSpeed.ToString() &&
                checkBoxPromptExecuteQuery.Checked == Settings.Default.PromptExecuteQuery &&
                checkBoxChangeStaticInfo.Checked == Settings.Default.ChangeStaticInfo &&
                checkBoxShowTooltipsPermanently.Checked == Settings.Default.ShowTooltipsPermanently &&
                checkBoxAutoGenerateComments.Checked == Settings.Default.GenerateComments &&
                checkBoxCreateRevertQuery.Checked == Settings.Default.CreateRevertQuery &&

                //! Check password last because it's the most 'expensive' check
                textBoxPassword.Text == Settings.Default.Password.DecryptString(Encoding.Unicode.GetBytes(Settings.Default.Entropy)).ToInsecureString())
                return;

            DialogResult dialogResult = MessageBox.Show("Do you wish to save the edited settings?", "Save settings?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

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
                    checkBoxLoadScriptInstantly.Checked = false;
                    checkBoxPromptToQuit.Checked = false;
                    textBoxAnimationSpeed.Text = "10";
                    trackBarAnimationSpeed.Value = 10;
                    checkBoxPromptExecuteQuery.Checked = true;
                    checkBoxChangeStaticInfo.Checked = true;
                    checkBoxShowTooltipsPermanently.Checked = false;
                    checkBoxAutoGenerateComments.Checked = false;
                    checkBoxCreateRevertQuery.Checked = false;
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
                    closedFormByHand = true;
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
            int newValue = XConverter.ToInt32(textBoxAnimationSpeed.Text);

            if (newValue > 12)
                newValue = 12;

            if (newValue < 1)
                newValue = 1;

            trackBarAnimationSpeed.Value = newValue;

            if (newValue != XConverter.ToInt32(textBoxAnimationSpeed.Text))
                textBoxAnimationSpeed.Text = newValue.ToString();
        }

        private void checkBoxExpandInstantly_CheckedChanged(object sender, EventArgs e)
        {
            trackBarAnimationSpeed.Enabled = !checkBoxInstantExpand.Checked;
        }

        private void numericField_KeyPress(object sender, KeyPressEventArgs e)
        {
            //! Only allow typing keys that are numbers
            //if (!Char.IsNumber(e.KeyChar))
            //    e.Handled = e.KeyChar != (char)Keys.Back;
        }

        private void checkBoxDontHidePass_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPassword.PasswordChar = Convert.ToChar(checkBoxHidePass.Checked ? '●' : '\0');
        }

        private async void buttonSearchForWorldDb_Click(object sender, EventArgs e)
        {
            List<string> databaseNames = await SAI_Editor_Manager.Instance.GetDatabasesInConnection(textBoxHost.Text, textBoxUsername.Text, XConverter.ToUInt32(textBoxPort.Text), textBoxPassword.Text);

            if (databaseNames != null && databaseNames.Count > 0)
                using (var selectDatabaseForm = new SelectDatabaseForm(databaseNames, textBoxWorldDatabase))
                    selectDatabaseForm.ShowDialog(this);
        }

        private void settingTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                buttonSaveSettings_Click(sender, e);
        }

        private void buttonTestConnection_Click(object sender, EventArgs e)
        {
            connectionString.Server = textBoxHost.Text;
            connectionString.UserID = textBoxUsername.Text;
            connectionString.Port = XConverter.ToUInt32(textBoxPort.Text);
            connectionString.Database = textBoxWorldDatabase.Text;

            if (textBoxPassword.Text.Length > 0)
                connectionString.Password = textBoxPassword.Text;

            if (SAI_Editor_Manager.Instance.worldDatabase.CanConnectToDatabase(connectionString))
                MessageBox.Show("Connection successful!", "Connection status", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
