using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using MySql.Data.MySqlClient;

namespace SAI_Editor
{
    public partial class MainForm : Form
    {
        MySqlConnectionStringBuilder connectionString = new MySqlConnectionStringBuilder();
        private System.Windows.Forms.Timer timerExpandToMainForm = null;
        private readonly Settings _settings = new Settings();
        private int originalHeight = 0, originalWidth = 0;
        private List<Control> controlsLoginForm = new List<Control>();
        private List<Control> controlsMainForm = new List<Control>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
            MinimizeBox = true;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Width = 278;
            Height = 260;

            originalHeight = Height;
            originalWidth = Width;

            textBoxHost.Text = _settings.GetSetting("Host", "localhost");
            textBoxUsername.Text = _settings.GetSetting("User", "root");
            textBoxPassword.Text = _settings.GetSetting("Password", string.Empty);
            textBoxWorldDatabase.Text = _settings.GetSetting("database", "trinitycore_world");
            textBoxPort.Text = _settings.GetSetting("Port", "3306");

            timerExpandToMainForm = new System.Windows.Forms.Timer();
            timerExpandToMainForm.Enabled = false;
            timerExpandToMainForm.Interval = 16;
            timerExpandToMainForm.Tick += timerExpandToMainForm_Tick;

            foreach (Control control in Controls)
            {
                if (control.Visible)
                    controlsLoginForm.Add(control);
                else
                    controlsMainForm.Add(control);
            }
        }

        private void timerExpandToMainForm_Tick(object sender, EventArgs e)
        {
            if (Height >= originalHeight + 600)
            {
                Height = originalHeight + 600;

                if (Width >= originalWidth + 600) //! If both finished
                    timerExpandToMainForm.Enabled = false;
            }
            else
                Height += 10;

            if (Width >= originalWidth + 400)
            {
                Width = originalWidth + 400;

                if (Height >= originalHeight + 400) //! If both finished
                    timerExpandToMainForm.Enabled = false;
            }
            else
                Width += 10;
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (IsInvalidString(textBoxHost.Text))
            {
                MessageBox.Show("The host field has to be filled!", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (IsInvalidString(textBoxUsername.Text))
            {
                MessageBox.Show("The username field has to be filled!", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (textBoxPassword.Text.Length > 0 && IsInvalidString(textBoxPassword.Text))
            {
                MessageBox.Show("The password field can not consist of only whitespaces!", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (IsInvalidString(textBoxWorldDatabase.Text))
            {
                MessageBox.Show("The world database field has to be filled!", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (IsInvalidString(textBoxPort.Text))
            {
                MessageBox.Show("The port field has to be filled!", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            connectionString.Server = textBoxHost.Text;
            connectionString.UserID = textBoxUsername.Text;
            connectionString.Port = Convert.ToUInt16(textBoxPort.Text);
            connectionString.Database = textBoxWorldDatabase.Text;

            if (textBoxPassword.Text.Length > 0)
                connectionString.Password = textBoxPassword.Text;

            if (CanConnectToDatabase())
            {
                if (checkBoxSaveSettings.Checked)
                {
                    _settings.PutSetting("User", textBoxUsername.Text);
                    _settings.PutSetting("Password", textBoxPassword.Text);
                    _settings.PutSetting("DB", textBoxWorldDatabase.Text);
                    _settings.PutSetting("Host", textBoxHost.Text);
                    _settings.PutSetting("Port", textBoxPort.Text);
                }

                Text = "SAI-Editor: " + textBoxUsername.Text + "@" + textBoxHost.Text + ":" + textBoxPort.Text;
                timerExpandToMainForm.Enabled = true;

                foreach (Control control in controlsLoginForm)
                    control.Visible = false;

                foreach (Control control in controlsMainForm)
                    control.Visible = true;

                //Close();
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            // not working
            foreach (Control control in controlsLoginForm)
                if (control is TextBox)
                    ((TextBox)control).Text = "";
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool IsInvalidString(string str)
        {
            return (String.IsNullOrEmpty(str) || String.IsNullOrWhiteSpace(str));
        }

        private bool CanConnectToDatabase()
        {
            bool successFulConnection = true; //! We have to use a variable because the connection would otherwise not be closed if an error happened.
            MySqlConnection connection = null;

            try
            {
                connection = new MySqlConnection(connectionString.ToString());
                connection.Open();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Could not connect.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                successFulConnection = false;
            }
            finally
            {
                //! Close the connection again since this is just a try-connection function. We actually connect
                //! when the mainform is opened.
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return successFulConnection;
        }
    }
}
