using System;
using System.Windows.Forms;
using System.Configuration;
using SAI_Editor.Properties;

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
            textBoxPassword.Text = Settings.Default.Password;
            textBoxWorldDatabase.Text = Settings.Default.Database;
            textBoxPort.Text = Settings.Default.Port.ToString();

            checkBoxAutoConnect.Checked = Settings.Default.AutoConnect;
            checkBoxInstantExpand.Checked = Settings.Default.InstantExpand;
            checkBoxLoadScriptInstantly.Checked = Settings.Default.LoadScriptInstantly;
            checkBoxAutoSaveSettings.Checked = Settings.Default.AutoSaveSettings;
            checkBoxPromptToQuit.Checked = Settings.Default.PromptToQuit;
            checkBoxHidePass.Checked = Settings.Default.HidePass;

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
            Settings.Default.Host = textBoxHost.Text;
            Settings.Default.User = textBoxUsername.Text;
            Settings.Default.Password = textBoxPassword.Text;
            Settings.Default.Database = textBoxWorldDatabase.Text;
            Settings.Default.Port = Convert.ToInt32(textBoxPort.Text);
            Settings.Default.AutoConnect = checkBoxAutoConnect.Checked;
            Settings.Default.InstantExpand = checkBoxInstantExpand.Checked;
            Settings.Default.LoadScriptInstantly = checkBoxLoadScriptInstantly.Checked;
            Settings.Default.AutoSaveSettings = checkBoxAutoSaveSettings.Checked;
            Settings.Default.PromptToQuit = checkBoxPromptToQuit.Checked;
            Settings.Default.HidePass = checkBoxHidePass.Checked;
            Settings.Default.AnimationSpeed = Convert.ToInt32(textBoxAnimationSpeed.Text);
            Settings.Default.Save();

            ((MainForm)Owner).checkBoxAutoConnect.Checked = checkBoxAutoConnect.Checked;
            ((MainForm)Owner).textBoxHost.Text = textBoxHost.Text;
            ((MainForm)Owner).textBoxUsername.Text = textBoxUsername.Text;
            ((MainForm)Owner).textBoxPassword.Text = textBoxPassword.Text;
            ((MainForm)Owner).textBoxWorldDatabase.Text = textBoxWorldDatabase.Text;
            ((MainForm)Owner).checkBoxAutoConnect.Checked = checkBoxAutoConnect.Checked;
            ((MainForm)Owner).animationSpeed = Convert.ToInt32(textBoxAnimationSpeed.Text);
            ((MainForm)Owner).textBoxPassword.PasswordChar = Convert.ToChar(checkBoxHidePass.Checked ? '●' : '\0');
        }

        private void buttonExitSettings_Click(object sender, EventArgs e)
        {
            PromptSaveSettingsOnClose();
            closedFormByHand = true;
            Close();
        }

        void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //! Only call this prompt method if the form was closed by the user itself and the form was not
            //! already closed before because we called Form::Close() (by pressing 'Exit' or so).
            if (e.CloseReason == CloseReason.UserClosing && !closedFormByHand)
                PromptSaveSettingsOnClose();
        }

        void PromptSaveSettingsOnClose()
        {
            if (checkBoxAutoSaveSettings.Checked)
            {
                Settings.Default.AutoSaveSettings = true;
                SaveSettings();
                return;
            }

            if (textBoxHost.Text == Settings.Default.Host && textBoxUsername.Text == Settings.Default.User &&
                textBoxPassword.Text == Settings.Default.Password && textBoxWorldDatabase.Text == Settings.Default.Database &&
                textBoxPort.Text == Settings.Default.Port.ToString() && checkBoxAutoConnect.Checked == Settings.Default.AutoConnect &&
                checkBoxInstantExpand.Checked == Settings.Default.InstantExpand && checkBoxLoadScriptInstantly.Checked == Settings.Default.LoadScriptInstantly &&
                checkBoxHidePass.Checked == Settings.Default.HidePass && checkBoxPromptToQuit.Checked == Settings.Default.PromptToQuit &&
                textBoxAnimationSpeed.Text == Settings.Default.AnimationSpeed.ToString())
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
                    checkBoxHidePass.Checked = true;
                    textBoxAnimationSpeed.Text = "10";
                    trackBarAnimationSpeed.Value = 10;
                    break;
                case 1: // ! 'Connection' tab
                    textBoxHost.Text = "";
                    textBoxUsername.Text = "";
                    textBoxPassword.Text = "";
                    textBoxWorldDatabase.Text = "";
                    textBoxPort.Text = "";
                    checkBoxAutoConnect.Checked = false;
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
            int newValue = Convert.ToInt32(textBoxAnimationSpeed.Text);

            if (newValue > 12)
                newValue = 12;

            if (newValue < 1)
                newValue = 1;

            trackBarAnimationSpeed.Value = newValue;

            if (newValue != Convert.ToInt32(textBoxAnimationSpeed.Text))
                textBoxAnimationSpeed.Text = newValue.ToString();
        }

        private void checkBoxExpandInstantly_CheckedChanged(object sender, EventArgs e)
        {
            trackBarAnimationSpeed.Enabled = !checkBoxInstantExpand.Checked;
        }

        private void numericField_KeyPress(object sender, KeyPressEventArgs e)
        {
            //! Only allow typing keys that are numbers
            if (!Char.IsNumber(e.KeyChar))
                e.Handled = e.KeyChar != (char)Keys.Back;
        }

        private void checkBoxDontHidePass_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPassword.PasswordChar = Convert.ToChar(checkBoxHidePass.Checked ? '●' : '\0');
        }
    }
}
