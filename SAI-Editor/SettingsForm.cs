using System;
using System.Windows.Forms;

namespace SAI_Editor
{
    public partial class SettingsForm : Form
    {
        private Settings settings = null;

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
            MinimizeBox = true;
            FormClosed += SettingsForm_FormClosed;
            settings = ((MainForm)Owner).settings;

            textBoxHost.Text = settings.GetSetting("Host", "localhost");
            textBoxUsername.Text = settings.GetSetting("User", "root");
            textBoxPassword.Text = settings.GetSetting("Password", String.Empty);
            textBoxWorldDatabase.Text = settings.GetSetting("Database", "trinitycore_world");
            textBoxPort.Text = settings.GetSetting("Port", "3306");
            checkBoxAutoLogin.Checked = settings.GetSetting("Autologin", "no") == "yes";
            checkBoxExpandInstantly.Checked = settings.GetSetting("InstantExpand", "no") == "yes";
        }

        private void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Close();
        }

        private void SaveSettings()
        {
            settings.PutSetting("Host", textBoxHost.Text);
            settings.PutSetting("User", textBoxUsername.Text);
            settings.PutSetting("Password", textBoxPassword.Text);
            settings.PutSetting("Database", textBoxWorldDatabase.Text);
            settings.PutSetting("Port", textBoxPort.Text);
            settings.PutSetting("Autologin", (checkBoxAutoLogin.Checked ? "yes" : "no"));
            settings.PutSetting("InstantExpand", (checkBoxExpandInstantly.Checked ? "yes" : "no"));
            checkBoxAutoLogin.Checked = checkBoxAutoLogin.Checked;
        }

        private void buttonExitSettings_Click(object sender, EventArgs e)
        {
            PromptSaveSettingsOnClose();
            Close();
        }

        void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
                PromptSaveSettingsOnClose();
        }

        void PromptSaveSettingsOnClose()
        {
            if (textBoxHost.Text == settings.GetSetting("Host", "localhost") && textBoxUsername.Text == settings.GetSetting("User", "root") &&
                textBoxPassword.Text == settings.GetSetting("Password", String.Empty) && textBoxWorldDatabase.Text == settings.GetSetting("Database", "trinitycore_world") &&
                textBoxPort.Text == settings.GetSetting("Port", "3306") && checkBoxAutoLogin.Checked == (settings.GetSetting("Autologin", "no") == "yes") &&
                checkBoxExpandInstantly.Checked == (settings.GetSetting("InstantExpand", "no") == "yes"))
                return;

            if (MessageBox.Show("Do you wish to save the edited settings?", "Save settings?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                SaveSettings();
        }

        private void buttonClearSettings_Click(object sender, EventArgs e)
        {
            switch (tabControlSettings.SelectedIndex)
            {
                case 0:
                    checkBoxExpandInstantly.Checked = false;
                    break;
                case 1:
                    textBoxHost.Text = "";
                    textBoxUsername.Text = "";
                    textBoxPassword.Text = "";
                    textBoxWorldDatabase.Text = "";
                    textBoxPort.Text = "";
                    checkBoxAutoLogin.Checked = false;
                    break;
            }
        }
    }
}
