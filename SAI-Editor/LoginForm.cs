using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SAI_Editor
{
    public partial class LoginForm : Form
    {
        MySqlConnectionStringBuilder connectionString = new MySqlConnectionStringBuilder();

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
            MinimizeBox = true;

            this.FormBorderStyle = FormBorderStyle.FixedDialog;
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
                // open mainform
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            //foreach (Control control in Controls)
            //    if (control is TextBox)
            //        ((TextBox)control).Text = "";

            //foreach (TextBox textBox in textbox)
            //    control.Text = "";
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
