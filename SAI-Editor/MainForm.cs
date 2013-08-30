using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient;

internal enum FormSizes
{
    WidthToExpandTo  = 830,
    HeightToExpandTo = 395,
};

namespace SAI_Editor
{
    public partial class MainForm : Form
    {
        private readonly Settings settings = new Settings();
        private readonly MySqlConnectionStringBuilder connectionString = new MySqlConnectionStringBuilder();
        private readonly List<Control> controlsLoginForm = new List<Control>();
        private readonly List<Control> controlsMainForm = new List<Control>();
        private bool contractingToLoginForm = false;
        private bool expandingToMainForm = false;
        private int originalHeight = 0, originalWidth = 0;
        private Timer timerExpandOrContract = null;

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

            textBoxHost.Text = settings.GetSetting("Host", "localhost");
            textBoxUsername.Text = settings.GetSetting("User", "root");
            textBoxPassword.Text = settings.GetSetting("Password", string.Empty);
            textBoxWorldDatabase.Text = settings.GetSetting("Database", "trinitycore_world");
            textBoxPort.Text = settings.GetSetting("Port", "3306");

            timerExpandOrContract = new Timer { Enabled = false, Interval = 4 };
            timerExpandOrContract.Tick += timerExpandOrContract_Tick;

            KeyPreview = true;
            KeyDown += Form1_KeyDown;

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
            comboBoxTargetType.SelectedIndex = 0;

            menuItemReconnect.Click += menuItemReconnect_Click;

            listViewSmartScripts.View = View.Details;
            listViewSmartScripts.Columns.Add("entryorguid", 67, HorizontalAlignment.Left);
            listViewSmartScripts.Columns.Add("source_type", 70, HorizontalAlignment.Right);
            listViewSmartScripts.Columns.Add("id",          20, HorizontalAlignment.Right);
            listViewSmartScripts.Columns.Add("link",        30, HorizontalAlignment.Right);
            listViewSmartScripts.Columns.Add("event_type",  66, HorizontalAlignment.Right);
            listViewSmartScripts.Columns.Add("event_phase", 74, HorizontalAlignment.Right);
            listViewSmartScripts.Columns.Add("event_chance",81, HorizontalAlignment.Right);
            listViewSmartScripts.Columns.Add("event_flags", 69, HorizontalAlignment.Right);
            listViewSmartScripts.Columns.Add("p1",          24, HorizontalAlignment.Right);
            listViewSmartScripts.Columns.Add("p2",          24, HorizontalAlignment.Right);
            listViewSmartScripts.Columns.Add("p3",          24, HorizontalAlignment.Right);
            listViewSmartScripts.Columns.Add("p4",          24, HorizontalAlignment.Right);
            listViewSmartScripts.Columns.Add("action_type", 67, HorizontalAlignment.Right);
            listViewSmartScripts.Columns.Add("p1",          24, HorizontalAlignment.Right);
            listViewSmartScripts.Columns.Add("p2",          24, HorizontalAlignment.Right);
            listViewSmartScripts.Columns.Add("p3",          24, HorizontalAlignment.Right);
            listViewSmartScripts.Columns.Add("p4",          24, HorizontalAlignment.Right);
            listViewSmartScripts.Columns.Add("p5",          24, HorizontalAlignment.Right);
            listViewSmartScripts.Columns.Add("p6",          24, HorizontalAlignment.Right);
            listViewSmartScripts.Columns.Add("target_type", 67, HorizontalAlignment.Right);
            listViewSmartScripts.Columns.Add("p1",          24, HorizontalAlignment.Right);
            listViewSmartScripts.Columns.Add("p2",          24, HorizontalAlignment.Right);
            listViewSmartScripts.Columns.Add("p3",          24, HorizontalAlignment.Right);
            listViewSmartScripts.Columns.Add("x",           20, HorizontalAlignment.Right);
            listViewSmartScripts.Columns.Add("y",           20, HorizontalAlignment.Right);
            listViewSmartScripts.Columns.Add("z",           20, HorizontalAlignment.Right);
            listViewSmartScripts.Columns.Add("o",           20, HorizontalAlignment.Right);
          //listViewSmartScripts.Columns.Add("comment",     56, HorizontalAlignment.Right);
            listViewSmartScripts.Columns.Add("comment",     100, HorizontalAlignment.Left);
        }

        private void timerExpandOrContract_Tick(object sender, EventArgs e)
        {
            if (expandingToMainForm)
            {
                if (Height < originalHeight + (int)FormSizes.HeightToExpandTo)
                    Height += 5;
                else
                {
                    Height = originalHeight + (int)FormSizes.HeightToExpandTo;

                    if (Width >= originalWidth + (int)FormSizes.WidthToExpandTo) //! If both finished
                    {
                        Width = originalWidth + (int)FormSizes.WidthToExpandTo;
                        timerExpandOrContract.Enabled = false;
                        expandingToMainForm = false;
                        FinishedExpandingOrContracting(true);
                    }
                }

                if (Width < originalWidth + (int)FormSizes.WidthToExpandTo)
                    Width += 5;
                else
                {
                    Width = originalWidth + (int)FormSizes.WidthToExpandTo;

                    if (Height >= originalHeight + (int)FormSizes.HeightToExpandTo) //! If both finished
                    {
                        Height = originalHeight + (int)FormSizes.HeightToExpandTo;
                        timerExpandOrContract.Enabled = false;
                        expandingToMainForm = false;
                        FinishedExpandingOrContracting(true);
                    }
                }
            }
            else if (contractingToLoginForm)
            {
                if (Height > originalHeight)
                    Height -= 5;
                else
                {
                    Height = originalHeight;

                    if (Width <= originalWidth) //! If both finished
                    {
                        Width = originalWidth;
                        timerExpandOrContract.Enabled = false;
                        contractingToLoginForm = false;
                        FinishedExpandingOrContracting(false);
                    }
                }

                if (Width > originalWidth)
                    Width -= 5;
                else
                {
                    Width = originalWidth;

                    if (Height <= originalHeight) //! If both finished
                    {
                        Height = originalHeight;
                        timerExpandOrContract.Enabled = false;
                        contractingToLoginForm = false;
                        FinishedExpandingOrContracting(false);
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
                settings.PutSetting("User", textBoxUsername.Text);
                settings.PutSetting("Password", textBoxPassword.Text);
                settings.PutSetting("Database", textBoxWorldDatabase.Text);
                settings.PutSetting("Host", textBoxHost.Text);
                settings.PutSetting("Port", textBoxPort.Text);
            }

            Text = "SAI-Editor: " + textBoxUsername.Text + "@" + textBoxHost.Text + ":" + textBoxPort.Text;
            timerExpandOrContract.Enabled = true;
            expandingToMainForm = true;

            foreach (Control control in controlsLoginForm)
                control.Visible = false;

            foreach (Control control in controlsMainForm)
                control.Visible = false;
        }

        private void StartContractingToLoginForm()
        {
            Text = "SAI-Editor: Login";
            timerExpandOrContract.Enabled = true;
            contractingToLoginForm = true;

            foreach (var control in controlsLoginForm)
                control.Visible = false;

            foreach (var control in controlsMainForm)
                control.Visible = false;
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            //! For some reason it won't work with a loop..
            //foreach (Control control in controlsLoginForm)
            //    if (control is TextBox)
            //        ((TextBox)control).Text = "";

            textBoxHost.Text = "";
            textBoxUsername.Text = "";
            textBoxPassword.Text = "";
            textBoxWorldDatabase.Text = "";
            textBoxPort.Text = "";
            checkBoxSaveSettings.Checked = false;
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
            var successFulConnection = true; //! We have to use a variable because the connection would otherwise not be closed if an error happened.
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
                case Keys.Escape:
                    if (MessageBox.Show("Are you sure you want to quit?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        Close();
                    break;
                case Keys.F5:
                    buttonConnect_Click(sender, e);
                    break;
            }
        }

        private void buttonSearchForCreature_Click(object sender, EventArgs e)
        {
            new SearchForEntryForm(connectionString, comboBoxSourceType.SelectedIndex == 0).ShowDialog(this);
        }

        private void comboBoxEventType_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            textBoxEventTypeId.Text = comboBoxEventType.SelectedIndex.ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxActionTypeId.Text = comboBoxActionType.SelectedIndex.ToString();
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

        private void comboBoxTargetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxTargetTypeId.Text = comboBoxTargetType.SelectedIndex.ToString();
        }

        private void checkBoxAutoGenerateComments_CheckedChanged_1(object sender, EventArgs e)
        {
            textBoxComments.Enabled = !checkBoxAutoGenerateComments.Checked;
        }

        private void checkBoxLockEventId_CheckedChanged(object sender, EventArgs e)
        {
            textBoxEventScriptId.Enabled = !checkBoxLockEventId.Checked;
        }

        private void FinishedExpandingOrContracting(bool expanding)
        {
            foreach (var control in controlsLoginForm)
                control.Visible = !expanding;

            foreach (var control in controlsMainForm)
                control.Visible = expanding;
        }

        private void comboBoxSourceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxSourceType.SelectedIndex)
            {
                case 0: //! Creature
                    labelCreatureEntry.Text = "Creature entry:";
                    buttonSearchForCreature.Enabled = true;
                    break;
                case 1: //! Gameobject
                    labelCreatureEntry.Text = "Gameobject entry:";
                    buttonSearchForCreature.Enabled = true;
                    break;
                case 2: //! Areatrigger
                    labelCreatureEntry.Text = "Areatrigger entry:";
                    buttonSearchForCreature.Enabled = false;
                    break;
                case 3: //! Scriptedactionlist
                    labelCreatureEntry.Text = "XX entry:";
                    buttonSearchForCreature.Enabled = false;
                    break;
            }
        }

        private void buttonLoadScriptForEntry_Click(object sender, EventArgs e)
        {
            if (IsInvalidString(textBoxEntryOrGuid.Text))
                return;

            listViewSmartScripts.Items.Clear();

            try
            {
                using (var connection = new MySqlConnection(connectionString.ToString()))
                {
                    connection.Open();
                    var returnVal = new MySqlDataAdapter(String.Format("SELECT * FROM smart_scripts WHERE entryorguid={0}", textBoxEntryOrGuid.Text), connection);
                    var dataTable = new DataTable();
                    returnVal.Fill(dataTable);

                    if (dataTable.Rows.Count <= 0)
                    {
                        MessageBox.Show(String.Format("The entry '{0}' could not be found in the SmartAI table!", textBoxEntryOrGuid.Text), "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    foreach (DataRow row in dataTable.Rows)
                    {
                        var listViewItem = new ListViewItem();

                        for (int i = 0; i < row.ItemArray.Length; i++)
                        {
                            if (i == 0)
                                listViewItem.Text = row.ItemArray[i].ToString();
                            else
                                listViewItem.SubItems.Add(row.ItemArray[i].ToString());
                        }

                        listViewSmartScripts.Items.Add(listViewItem);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
