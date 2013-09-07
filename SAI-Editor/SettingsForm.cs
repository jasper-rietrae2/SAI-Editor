using System;
using System.Windows.Forms;
using System.Configuration;

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
            textBoxHost.Text = Properties.Settings.Default.Host;
            textBoxUsername.Text = Properties.Settings.Default.User;
            textBoxPassword.Text = Properties.Settings.Default.Password;
            textBoxWorldDatabase.Text = Properties.Settings.Default.Database;
            textBoxPort.Text = Properties.Settings.Default.Port.ToString();

            checkBoxAutoConnect.Checked = Properties.Settings.Default.AutoConnect;
            checkBoxInstantExpand.Checked = Properties.Settings.Default.InstantExpand;
            checkBoxLoadScriptInstantly.Checked = Properties.Settings.Default.LoadScriptInstantly;
            checkBoxAutoSaveSettings.Checked = Properties.Settings.Default.AutoSaveSettings;
            checkBoxPromptToQuit.Checked = Properties.Settings.Default.PromptToQuit;
            checkBoxDontHidePass.Checked = Properties.Settings.Default.DontHidePass;

            textBoxAnimationSpeed.Text = Properties.Settings.Default.AnimationSpeed.ToString();

            textBoxPassword.PasswordChar = Convert.ToChar(!checkBoxDontHidePass.Checked ? '*' : '\0');
        }

        private void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            SaveSettings();
            closedFormByHand = true;
            Close();
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.Host = textBoxHost.Text;
            Properties.Settings.Default.User = textBoxUsername.Text;
            Properties.Settings.Default.Password = textBoxPassword.Text;
            Properties.Settings.Default.Database = textBoxWorldDatabase.Text;
            Properties.Settings.Default.Port = Convert.ToInt32(textBoxPort.Text);
            Properties.Settings.Default.AutoConnect = checkBoxAutoConnect.Checked;
            Properties.Settings.Default.InstantExpand = checkBoxInstantExpand.Checked;
            Properties.Settings.Default.LoadScriptInstantly = checkBoxLoadScriptInstantly.Checked;
            Properties.Settings.Default.AutoSaveSettings = checkBoxAutoSaveSettings.Checked;
            Properties.Settings.Default.PromptToQuit = checkBoxPromptToQuit.Checked;
            Properties.Settings.Default.DontHidePass = checkBoxDontHidePass.Checked;
            Properties.Settings.Default.AnimationSpeed = Convert.ToInt32(textBoxAnimationSpeed.Text);
            Properties.Settings.Default.Save();

            ((MainForm)Owner).checkBoxAutoConnect.Checked = checkBoxAutoConnect.Checked;
            ((MainForm)Owner).textBoxHost.Text = textBoxHost.Text;
            ((MainForm)Owner).textBoxUsername.Text = textBoxUsername.Text;
            ((MainForm)Owner).textBoxPassword.Text = textBoxPassword.Text;
            ((MainForm)Owner).textBoxWorldDatabase.Text = textBoxWorldDatabase.Text;
            ((MainForm)Owner).checkBoxAutoConnect.Checked = checkBoxAutoConnect.Checked;
            ((MainForm)Owner).animationSpeed = Convert.ToInt32(textBoxAnimationSpeed.Text);
            ((MainForm)Owner).textBoxPassword.PasswordChar = Convert.ToChar(!checkBoxDontHidePass.Checked ? '*' : '\0');
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
                SaveSettings();
                return;
            }

            if (textBoxHost.Text == Properties.Settings.Default.Host && textBoxUsername.Text == Properties.Settings.Default.User &&
                textBoxPassword.Text == Properties.Settings.Default.Password && textBoxWorldDatabase.Text == Properties.Settings.Default.Database &&
                textBoxPort.Text == Properties.Settings.Default.Port.ToString() && checkBoxAutoConnect.Checked == Properties.Settings.Default.AutoConnect &&
                checkBoxInstantExpand.Checked == Properties.Settings.Default.InstantExpand && checkBoxLoadScriptInstantly.Checked == Properties.Settings.Default.LoadScriptInstantly &&
                checkBoxAutoSaveSettings.Checked == Properties.Settings.Default.AutoSaveSettings && checkBoxPromptToQuit.Checked == Properties.Settings.Default.PromptToQuit &&
                textBoxAnimationSpeed.Text == Properties.Settings.Default.AnimationSpeed.ToString() && checkBoxDontHidePass.Checked == Properties.Settings.Default.DontHidePass)
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
                    checkBoxDontHidePass.Checked = false;
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

        private bool IsEmptyString(string str)
        {
            return String.IsNullOrEmpty(str) || String.IsNullOrWhiteSpace(str);
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
            textBoxPassword.PasswordChar = Convert.ToChar(!checkBoxDontHidePass.Checked ? '*' : '\0');
        }
    }
}
