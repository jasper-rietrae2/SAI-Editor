using System;
using System.Windows.Forms;

namespace SAI_Editor
{
    public partial class SettingsForm : Form
    {
        private readonly Settings settings = new Settings();

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
            MinimizeBox = true;

            textBoxHost.Text = settings.GetSetting("Host", "localhost");
            textBoxUsername.Text = settings.GetSetting("User", "root");
            textBoxPassword.Text = settings.GetSetting("Password", string.Empty);
            textBoxWorldDatabase.Text = settings.GetSetting("Database", "trinitycore_world");
            textBoxPort.Text = settings.GetSetting("Port", "3306");
            checkBoxAutoLogin.Checked = settings.GetSetting("Autologin", "no") == "yes";
            checkBoxStartApplicationFullSize.Checked = settings.GetSetting("Startfullsize", "no") == "yes";
        }

        private void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            settings.PutSetting("Host", textBoxHost.Text);
            settings.PutSetting("User", textBoxUsername.Text);
            settings.PutSetting("Password", textBoxPassword.Text);
            settings.PutSetting("Database", textBoxWorldDatabase.Text);
            settings.PutSetting("Port", textBoxPort.Text);
            settings.PutSetting("Autologin", (checkBoxAutoLogin.Checked ? "yes" : "no"));
            settings.PutSetting("Startfullsize", (checkBoxStartApplicationFullSize.Checked ? "yes" : "no"));
            ((MainForm)Owner).checkBoxAutoLogin.Checked = checkBoxAutoLogin.Checked;
            Close();
        }

        private void buttonExitSettings_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonClearSettings_Click(object sender, EventArgs e)
        {
            textBoxHost.Text = "";
            textBoxUsername.Text = "";
            textBoxPassword.Text = "";
            textBoxWorldDatabase.Text = "";
            textBoxPort.Text = "";
            checkBoxAutoLogin.Checked = false;
        }
    }
}
