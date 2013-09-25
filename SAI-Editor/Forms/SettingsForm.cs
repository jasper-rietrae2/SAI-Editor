using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using SAI_Editor.Classes;
using SAI_Editor.Properties;
using SAI_Editor.Security;

namespace SAI_Editor
{
    public partial class SettingsForm : Form
    {
        private bool closedFormByHand = false;

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
                ((MainForm)Owner).textBoxEntryOrGuid.Text = ((MainForm)Owner).originalEntryOrGuid;
                ((MainForm)Owner).comboBoxSourceType.SelectedIndex = ((MainForm)Owner).GetIndexBySourceType(((MainForm)Owner).originalSourceType);
            }

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[1024];

            rng.GetBytes(buffer);
            string salt = BitConverter.ToString(buffer);

            rng.Dispose();
            string decryptedPassword = textBoxPassword.Text;

            Settings.Default.Entropy = salt;
            Settings.Default.Host = textBoxHost.Text;
            Settings.Default.User = textBoxUsername.Text;
            Settings.Default.Password = decryptedPassword.ToSecureString().EncryptString(Encoding.Unicode.GetBytes(salt));
            Settings.Default.Database = textBoxWorldDatabase.Text;
            Settings.Default.Port = textBoxPort.Text.Length > 0 ? XConverter.TryParseStringToUInt32(textBoxPort.Text) : 0;
            Settings.Default.AutoConnect = checkBoxAutoConnect.Checked;
            Settings.Default.InstantExpand = checkBoxInstantExpand.Checked;
            Settings.Default.LoadScriptInstantly = checkBoxLoadScriptInstantly.Checked;
            Settings.Default.AutoSaveSettings = checkBoxAutoSaveSettings.Checked;
            Settings.Default.PromptToQuit = checkBoxPromptToQuit.Checked;
            Settings.Default.HidePass = checkBoxHidePass.Checked;
            Settings.Default.AnimationSpeed = XConverter.TryParseStringToInt32(textBoxAnimationSpeed.Text);
            Settings.Default.PromptExecuteQuery = checkBoxPromptExecuteQuery.Checked;
            Settings.Default.ChangeStaticInfo = checkBoxChangeStaticInfo.Checked;
            Settings.Default.Save();

            ((MainForm)Owner).checkBoxAutoConnect.Checked = checkBoxAutoConnect.Checked;
            ((MainForm)Owner).textBoxHost.Text = textBoxHost.Text;
            ((MainForm)Owner).textBoxUsername.Text = textBoxUsername.Text;
            ((MainForm)Owner).textBoxPassword.Text = decryptedPassword;
            ((MainForm)Owner).textBoxWorldDatabase.Text = textBoxWorldDatabase.Text;
            ((MainForm)Owner).checkBoxAutoConnect.Checked = checkBoxAutoConnect.Checked;
            ((MainForm)Owner).animationSpeed = XConverter.TryParseStringToInt32(textBoxAnimationSpeed.Text);
            ((MainForm)Owner).textBoxPassword.PasswordChar = Convert.ToChar(checkBoxHidePass.Checked ? '●' : '\0');
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

                //! Check password last because it's the most 'expensive' check
                textBoxPassword.Text == Settings.Default.Password.DecryptString(Encoding.Unicode.GetBytes(Settings.Default.Entropy)).ToInsecureString())
                return;

            if (MessageBox.Show("Do you wish to save the edited settings?", "Save settings?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
            int newValue = XConverter.TryParseStringToInt32(textBoxAnimationSpeed.Text);

            if (newValue > 12)
                newValue = 12;

            if (newValue < 1)
                newValue = 1;

            trackBarAnimationSpeed.Value = newValue;

            if (newValue != XConverter.TryParseStringToInt32(textBoxAnimationSpeed.Text))
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
            List<string> databaseNames = await SAI_Editor_Manager.Instance.GetDatabasesInConnection(textBoxHost.Text, textBoxUsername.Text, XConverter.TryParseStringToUInt32(textBoxPort.Text), textBoxPassword.Text);

            if (databaseNames != null && databaseNames.Count > 0)
                using (var selectDatabaseForm = new SelectDatabaseForm(databaseNames, textBoxWorldDatabase))
                    selectDatabaseForm.ShowDialog(this);
        }

        private void settingTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                buttonSaveSettings_Click(sender, e);
        }
    }
}
