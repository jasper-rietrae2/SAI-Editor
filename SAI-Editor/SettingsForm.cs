using System;
using System.Windows.Forms;

namespace SAI_Editor
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
            MinimizeBox = true;

            textBoxHost.Text = ((MainForm)Owner).settings.GetSetting("Host", "localhost");
            textBoxUsername.Text = ((MainForm)Owner).settings.GetSetting("User", "root");
            textBoxPassword.Text = ((MainForm)Owner).settings.GetSetting("Password", string.Empty);
            textBoxWorldDatabase.Text = ((MainForm)Owner).settings.GetSetting("Database", "trinitycore_world");
            textBoxPort.Text = ((MainForm)Owner).settings.GetSetting("Port", "3306");
            checkBoxAutoLogin.Checked = ((MainForm)Owner).settings.GetSetting("Autologin", "no") == "yes";
            checkBoxStartApplicationFullSize.Checked = ((MainForm)Owner).settings.GetSetting("Startfullsize", "no") == "yes";
        }

        private void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            ((MainForm)Owner).settings.PutSetting("Host", textBoxHost.Text);
            ((MainForm)Owner).settings.PutSetting("User", textBoxUsername.Text);
            ((MainForm)Owner).settings.PutSetting("Password", textBoxPassword.Text);
            ((MainForm)Owner).settings.PutSetting("Database", textBoxWorldDatabase.Text);
            ((MainForm)Owner).settings.PutSetting("Port", textBoxPort.Text);
            ((MainForm)Owner).settings.PutSetting("Autologin", (checkBoxAutoLogin.Checked ? "yes" : "no"));
            ((MainForm)Owner).settings.PutSetting("Startfullsize", (checkBoxStartApplicationFullSize.Checked ? "yes" : "no"));
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
