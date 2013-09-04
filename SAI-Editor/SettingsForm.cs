using System;
using System.Windows.Forms;

namespace SAI_Editor
{
    public partial class SettingsForm : Form
    {
        private Settings settings = null;
        private bool closedFormByHand = false;

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
            MinimizeBox = true;
            FormBorderStyle = FormBorderStyle.Fixed3D;

            FormClosed += SettingsForm_FormClosed; //! To save settings

            KeyPreview = true;
            KeyDown += SettingsForm_KeyDown;

            textBoxAnimationSpeed.KeyPress += textBoxAnimationSpeed_KeyPress;
            trackBarAnimationSpeed.ValueChanged += trackBarAnimationSpeed_ValueChanged;

            settings = ((MainForm)Owner).settings;
            textBoxHost.Text = settings.GetSetting("Host", "localhost");
            textBoxUsername.Text = settings.GetSetting("User", "root");
            textBoxPassword.Text = settings.GetSetting("Password", String.Empty);
            textBoxWorldDatabase.Text = settings.GetSetting("Database", "trinitycore_world");
            textBoxPort.Text = settings.GetSetting("Port", "3306");
            checkBoxAutoConnect.Checked = settings.GetSetting("AutoConnect", "no") == "yes";
            checkBoxExpandInstantly.Checked = settings.GetSetting("InstantExpand", "no") == "yes";
            checkBoxLoadScriptOfEntry.Checked = settings.GetSetting("LoadScriptInstantly", "yes") == "yes";
            checkBoxAutoSaveSettings.Checked = settings.GetSetting("AutoSaveSettings", "no") == "yes";
            checkBoxPromptToQuit.Checked = settings.GetSetting("PromptToQuit", "yes") == "yes";
            textBoxAnimationSpeed.Text = settings.GetSetting("AnimationSpeed", "6");
        }

        private void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            SaveSettings();
            closedFormByHand = true;
            Close();
        }

        private void SaveSettings()
        {
            settings.PutSetting("Host", textBoxHost.Text);
            settings.PutSetting("User", textBoxUsername.Text);
            settings.PutSetting("Password", textBoxPassword.Text);
            settings.PutSetting("Database", textBoxWorldDatabase.Text);
            settings.PutSetting("Port", textBoxPort.Text);
            settings.PutSetting("AutoConnect", (checkBoxAutoConnect.Checked ? "yes" : "no"));
            settings.PutSetting("InstantExpand", (checkBoxExpandInstantly.Checked ? "yes" : "no"));
            settings.PutSetting("LoadScriptInstantly", (checkBoxLoadScriptOfEntry.Checked ? "yes" : "no"));
            settings.PutSetting("AutoSaveSettings", (checkBoxAutoSaveSettings.Checked ? "yes" : "no"));
            settings.PutSetting("PromptToQuit", (checkBoxPromptToQuit.Checked ? "yes" : "no"));
            settings.PutSetting("AnimationSpeed", textBoxAnimationSpeed.Text);
            checkBoxAutoConnect.Checked = checkBoxAutoConnect.Checked;

            //! Update main form's fields so re-connecting will work with new settings
            ((MainForm)Owner).textBoxHost.Text = textBoxHost.Text;
            ((MainForm)Owner).textBoxUsername.Text = textBoxUsername.Text;
            ((MainForm)Owner).textBoxPassword.Text = textBoxPassword.Text;
            ((MainForm)Owner).textBoxWorldDatabase.Text = textBoxWorldDatabase.Text;
            ((MainForm)Owner).checkBoxAutoConnect.Checked = checkBoxAutoConnect.Checked;

            ((MainForm)Owner).animationSpeed = Convert.ToInt32(textBoxAnimationSpeed.Text);
        }

        private void buttonExitSettings_Click(object sender, EventArgs e)
        {
            PromptSaveSettingsOnClose();
            closedFormByHand = true;
            Close();
        }

        void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !closedFormByHand)
                PromptSaveSettingsOnClose();
        }

        void PromptSaveSettingsOnClose()
        {
            if (settings.GetSetting("AutoSaveSettings", "no") == "yes")
            {
                SaveSettings();
                return;
            }

            if (textBoxHost.Text == settings.GetSetting("Host", "localhost") && textBoxUsername.Text == settings.GetSetting("User", "root") &&
                textBoxPassword.Text == settings.GetSetting("Password", String.Empty) && textBoxWorldDatabase.Text == settings.GetSetting("Database", "trinitycore_world") &&
                textBoxPort.Text == settings.GetSetting("Port", "3306") && checkBoxAutoConnect.Checked == (settings.GetSetting("AutoConnect", "no") == "yes") &&
                checkBoxExpandInstantly.Checked == (settings.GetSetting("InstantExpand", "no") == "yes") && checkBoxLoadScriptOfEntry.Checked == (settings.GetSetting("LoadScriptInstantly", "yes") == "yes") &&
                checkBoxAutoSaveSettings.Checked == (settings.GetSetting("AutoSaveSettings", "no") == "yes") && checkBoxPromptToQuit.Checked == (settings.GetSetting("PromptToQuit", "yes") == "yes") &&
                textBoxAnimationSpeed.Text == settings.GetSetting("AnimationSpeed", "6"))
                return;

            if (MessageBox.Show("Do you wish to save the edited settings?", "Save settings?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                SaveSettings();
        }

        private void buttonClearSettings_Click(object sender, EventArgs e)
        {
            switch (tabControlSettings.SelectedIndex)
            {
                case 0: //! 'Other' tab
                    checkBoxExpandInstantly.Checked = false;
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

        private void textBoxAnimationSpeed_KeyPress(object sender, KeyPressEventArgs e)
        {
            //! Only numbers
            if (!Char.IsNumber(e.KeyChar))
                e.Handled = e.KeyChar != (char)Keys.Back;
        }

        private void trackBarAnimationSpeed_ValueChanged(object sender, EventArgs e)
        {
            textBoxAnimationSpeed.Text = (trackBarAnimationSpeed.Value).ToString();
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
    }
}
