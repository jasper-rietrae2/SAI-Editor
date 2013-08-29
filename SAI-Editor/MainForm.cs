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
        private System.Windows.Forms.Timer timerExpandOrContract = null;
        private readonly Settings _settings = new Settings();
        private int originalHeight = 0, originalWidth = 0;
        private List<Control> controlsLoginForm = new List<Control>();
        private List<Control> controlsMainForm = new List<Control>();
        private bool expandingToMainForm = false, contractingToLoginForm = false;

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

            timerExpandOrContract = new System.Windows.Forms.Timer();
            timerExpandOrContract.Enabled = false;
            timerExpandOrContract.Interval = 32;
            timerExpandOrContract.Tick += timerExpandOrContract_Tick;

            KeyPreview = true;
            KeyDown += new KeyEventHandler(Form1_KeyDown);

            foreach (Control control in Controls)
            {
                if (control.Visible)
                    controlsLoginForm.Add(control);
                else
                    controlsMainForm.Add(control);
            }

            comboBoxSourceType.SelectedIndex = 0;
            comboBoxEventType.SelectedIndex = 0;
            comboBoxActionType.SelectedIndex = 0;

            menuItemReconnect.Click += menuItemReconnect_Click;
        }

        private void timerExpandOrContract_Tick(object sender, EventArgs e)
        {
            if (expandingToMainForm)
            {
                if (Height < originalHeight + 610)
                    Height += 20;
                else
                {
                    Height = originalHeight + 610;

                    if (Width >= originalWidth + 590) //! If both finished
                    {
                        Width = originalWidth + 590;
                        timerExpandOrContract.Enabled = false;
                        expandingToMainForm = false;
                    }
                }

                if (Width < originalWidth + 590)
                    Width += 20;
                else
                {
                    Width = originalWidth + 590;

                    if (Height >= originalHeight + 610) //! If both finished
                    {
                        Height = originalHeight + 610;
                        timerExpandOrContract.Enabled = false;
                        expandingToMainForm = false;
                    }
                }
            }
            else if (contractingToLoginForm)
            {
                if (Height >= originalHeight)
                    Height -= 20;
                else
                {
                    Height = originalHeight;

                    if (Width <= originalWidth) //! If both finished
                    {
                        Width = originalWidth;
                        timerExpandOrContract.Enabled = false;
                        contractingToLoginForm = false;
                    }
                }

                if (Width >= originalWidth)
                    Width -= 20;
                else
                {
                    Width = originalWidth;

                    if (Height <= originalHeight) //! If both finished
                    {
                        Height = originalHeight;
                        timerExpandOrContract.Enabled = false;
                        contractingToLoginForm = false;
                    }
                }
            }
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
                StartExpandingToMainForm();
        }

        private void StartExpandingToMainForm()
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
            timerExpandOrContract.Enabled = true;
            expandingToMainForm = true;

            foreach (Control control in controlsLoginForm)
                control.Visible = false;

            foreach (Control control in controlsMainForm)
                control.Visible = true;
        }

        private void StartContractingToLoginForm()
        {
            Text = "SAI-Editor: Login";
            timerExpandOrContract.Enabled = true;
            contractingToLoginForm = true;

            foreach (Control control in controlsLoginForm)
                control.Visible = true;

            foreach (Control control in controlsMainForm)
                control.Visible = false;
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

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F5:
                    buttonConnect_Click(sender, e);
                    break;
            }
        }

        private void buttonSearchForCreature_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxEventType_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            textBoxEventTypeId.Text = comboBoxEventType.SelectedIndex.ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxActionTypeId.Text = comboBoxActionType.SelectedIndex.ToString();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxAutoGenerateComments_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void menuItemReconnect_Click(object sender, EventArgs e)
        {
            StartContractingToLoginForm();
        }

        private void comboBoxEventType_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxEventTypeId.Text = comboBoxEventType.SelectedIndex.ToString();
        }

        private void comboBoxActionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxActionTypeId.Text = comboBoxActionType.SelectedIndex.ToString();
        }

        private void checkBoxAutoGenerateComments_CheckedChanged_1(object sender, EventArgs e)
        {
            textBoxComments.Enabled = !checkBoxAutoGenerateComments.Checked;
        }

        private void checkBoxLockEventId_CheckedChanged(object sender, EventArgs e)
        {
            textBoxScriptId.Enabled = !checkBoxLockEventId.Checked;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void groupBoxScriptInfo_Enter(object sender, EventArgs e)
        {

        }
    }
}
