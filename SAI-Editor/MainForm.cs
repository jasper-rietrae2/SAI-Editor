using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient;

//internal enum FormSizes
//{
//    WidthToExpandTo = 1035,
//    HeightToExpandTo = 252,
//};

namespace SAI_Editor
{
    public partial class MainForm : Form
    {
        public Settings settings = new Settings();
        private readonly MySqlConnectionStringBuilder connectionString = new MySqlConnectionStringBuilder();
        private readonly List<Control> controlsLoginForm = new List<Control>();
        private readonly List<Control> controlsMainForm = new List<Control>();
        private bool contractingToLoginForm = false;
        private bool expandingToMainForm = false;
        private int originalHeight = 0, originalWidth = 0;
        private Timer timerExpandOrContract = new Timer { Enabled = false, Interval = 4 };
        private int WidthToExpandTo = 985, HeightToExpandTo = 505;

        public MainForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            menuStrip.Visible = false; //! Doing this in main code so we can actually see the menustrip in designform

            //MaximizeBox = false;
            //MinimizeBox = true;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Width = 278;
            Height = 260;

            originalHeight = Height;
            originalWidth = Width;

            if (WidthToExpandTo > SystemInformation.VirtualScreen.Width)
                WidthToExpandTo = SystemInformation.VirtualScreen.Width;

            if (HeightToExpandTo > SystemInformation.VirtualScreen.Height)
                HeightToExpandTo = SystemInformation.VirtualScreen.Height;

            textBoxHost.Text = settings.GetSetting("Host", "localhost");
            textBoxUsername.Text = settings.GetSetting("User", "root");
            textBoxPassword.Text = settings.GetSetting("Password", string.Empty);
            textBoxWorldDatabase.Text = settings.GetSetting("Database", "trinitycore_world");
            textBoxPort.Text = settings.GetSetting("Port", "3306");

            KeyPreview = true;
            KeyDown += MainForm_KeyDown;

            //! Disallow writing anything in comboboxes (the 3D version looks like shit)
            comboBoxActionType.KeyPress += comboBox_KeyPress;
            comboBoxTargetType.KeyPress += comboBox_KeyPress;
            comboBoxEventType.KeyPress += comboBox_KeyPress;
            comboBoxSourceType.KeyPress += comboBox_KeyPress;

            timerExpandOrContract.Tick += timerExpandOrContract_Tick;

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
            menuItemExit.Click += TryCloseApplication;
            menuItemSettings.Click += menuItemSettings_Click;
            menuItemAbout.Click += menuItemAbout_Click;

            listViewSmartScripts.View = View.Details;
            listViewSmartScripts.FullRowSelect = true;
            listViewSmartScripts.Columns.Add("entryorguid", 67, HorizontalAlignment.Left);  // 0
            listViewSmartScripts.Columns.Add("source_type", 70, HorizontalAlignment.Right); // 0 (first subitem)
            listViewSmartScripts.Columns.Add("id",          20, HorizontalAlignment.Right); // 1
            listViewSmartScripts.Columns.Add("link",        30, HorizontalAlignment.Right); // 2
            listViewSmartScripts.Columns.Add("event_type",  66, HorizontalAlignment.Right); // 3
            listViewSmartScripts.Columns.Add("event_phase", 74, HorizontalAlignment.Right); // 4
            listViewSmartScripts.Columns.Add("event_chance",81, HorizontalAlignment.Right); // 5
            listViewSmartScripts.Columns.Add("event_flags", 69, HorizontalAlignment.Right); // 6
            listViewSmartScripts.Columns.Add("p1",          24, HorizontalAlignment.Right); // 7
            listViewSmartScripts.Columns.Add("p2",          24, HorizontalAlignment.Right); // 8
            listViewSmartScripts.Columns.Add("p3",          24, HorizontalAlignment.Right); // 9
            listViewSmartScripts.Columns.Add("p4",          24, HorizontalAlignment.Right); // 10
            listViewSmartScripts.Columns.Add("action_type", 67, HorizontalAlignment.Right); // 11
            listViewSmartScripts.Columns.Add("p1",          24, HorizontalAlignment.Right); // 12
            listViewSmartScripts.Columns.Add("p2",          24, HorizontalAlignment.Right); // 13
            listViewSmartScripts.Columns.Add("p3",          24, HorizontalAlignment.Right); // 14
            listViewSmartScripts.Columns.Add("p4",          24, HorizontalAlignment.Right); // 15
            listViewSmartScripts.Columns.Add("p5",          24, HorizontalAlignment.Right); // 16
            listViewSmartScripts.Columns.Add("p6",          24, HorizontalAlignment.Right); // 17
            listViewSmartScripts.Columns.Add("target_type", 67, HorizontalAlignment.Right); // 18
            listViewSmartScripts.Columns.Add("p1",          24, HorizontalAlignment.Right); // 19
            listViewSmartScripts.Columns.Add("p2",          24, HorizontalAlignment.Right); // 20
            listViewSmartScripts.Columns.Add("p3",          24, HorizontalAlignment.Right); // 21
            listViewSmartScripts.Columns.Add("x",           20, HorizontalAlignment.Right); // 22
            listViewSmartScripts.Columns.Add("y",           20, HorizontalAlignment.Right); // 23
            listViewSmartScripts.Columns.Add("z",           20, HorizontalAlignment.Right); // 24
            listViewSmartScripts.Columns.Add("o",           20, HorizontalAlignment.Right); // 25
          //listViewSmartScripts.Columns.Add("comment",     56, HorizontalAlignment.Right); // 26
            listViewSmartScripts.Columns.Add("comment",     400, HorizontalAlignment.Left); // 26

            if (settings.GetSetting("AutoConnect", "no") == "yes")
            {
                checkBoxAutoConnect.Checked = true;
                buttonConnect_Click(sender, e);

                if (settings.GetSetting("InstantExpand", "no") == "yes")
                    StartExpandingToMainForm(true);
            }

            listViewSmartScripts.Click += listViewSmartScripts_Click;

            //listViewSmartScripts.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
        }

        private void timerExpandOrContract_Tick(object sender, EventArgs e)
        {
            if (expandingToMainForm)
            {
                if (Height < HeightToExpandTo)
                    Height += 5;
                else
                {
                    Height = HeightToExpandTo;

                    if (Width >= WidthToExpandTo) //! If both finished
                    {
                        Width = WidthToExpandTo;
                        timerExpandOrContract.Enabled = false;
                        expandingToMainForm = false;
                        FinishedExpandingOrContracting(true);
                    }
                }

                if (Width < WidthToExpandTo)
                    Width += 5;
                else
                {
                    Width = WidthToExpandTo;

                    if (Height >= HeightToExpandTo) //! If both finished
                    {
                        Height = HeightToExpandTo;
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
                StartExpandingToMainForm(settings.GetSetting("InstantExpand", "no") == "yes");
        }

        private void StartExpandingToMainForm(bool instant = false)
        {
            if (checkBoxSaveSettings.Checked)
            {
                settings.PutSetting("Host", textBoxHost.Text);
                settings.PutSetting("User", textBoxUsername.Text);
                settings.PutSetting("Password", textBoxPassword.Text);
                settings.PutSetting("Database", textBoxWorldDatabase.Text);
                settings.PutSetting("Port", textBoxPort.Text);
                settings.PutSetting("AutoConnect", (checkBoxAutoConnect.Checked ? "yes" : "no"));
            }

            Text = "SAI-Editor: " + textBoxUsername.Text + "@" + textBoxHost.Text + ":" + textBoxPort.Text;

            if (instant)
            {
                Width = WidthToExpandTo;
                Height = HeightToExpandTo;
            }
            else
            {
                timerExpandOrContract.Enabled = true;
                expandingToMainForm = true;
            }

            foreach (Control control in controlsLoginForm)
                control.Visible = false;

            foreach (Control control in controlsMainForm)
                control.Visible = instant;
        }

        private void StartContractingToLoginForm(bool instant = false)
        {
            Text = "SAI-Editor: Login";

            if (instant)
            {
                Width = originalWidth;
                Height = originalHeight;
            }
            else
            {
                timerExpandOrContract.Enabled = true;
                contractingToLoginForm = true;
            }

            foreach (var control in controlsLoginForm)
                control.Visible = instant;

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
            checkBoxAutoConnect.Checked = false;
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

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    TryCloseApplication();
                    break;
                case Keys.F5:
                    buttonConnect_Click(sender, e);
                    break;
            }
        }

        private void buttonSearchForCreature_Click(object sender, EventArgs e)
        {
            //! Just keep it in main thread; no purpose starting a new thread for this
            new SearchForEntryForm(connectionString, comboBoxSourceType.SelectedIndex == 0).ShowDialog(this);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxActionTypeId.Text = comboBoxActionType.SelectedIndex.ToString();
        }

        private void menuItemReconnect_Click(object sender, EventArgs e)
        {
            StartContractingToLoginForm(settings.GetSetting("InstantExpand", "no") == "yes");
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
                    labelCreatureEntry.Text = "Actionlist entry:";
                    buttonSearchForCreature.Enabled = false;
                    break;
            }
        }

        public void buttonLoadScriptForEntry_Click(object sender, EventArgs e)
        {
            if (IsInvalidString(textBoxEntryOrGuid.Text))
                return;

            listViewSmartScripts.Items.Clear();

            try
            {
                using (var connection = new MySqlConnection(connectionString.ToString()))
                {
                    connection.Open();
                    var returnVal = new MySqlDataAdapter(String.Format("SELECT * FROM smart_scripts WHERE entryorguid={0} AND source_type={1}", textBoxEntryOrGuid.Text, comboBoxSourceType.SelectedIndex), connection);
                    var dataTable = new DataTable();
                    returnVal.Fill(dataTable);

                    if (dataTable.Rows.Count <= 0)
                    {
                        MessageBox.Show(String.Format("The entry '{0}' could not be found in the SmartAI table for the given source type!", textBoxEntryOrGuid.Text), "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        //! Needs object and EventAgrs parameters so we can trigger it as an event when 'Exit' is called from the menu.
        private void TryCloseApplication(object sender = null, EventArgs e = null)
        {
            if (MessageBox.Show("Are you sure you want to quit?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Close();
        }

        private void menuItemSettings_Click(object sender, EventArgs e)
        {
            new SettingsForm().ShowDialog(this);
        }

        private void menuItemAbout_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog(this);
        }

        private void listViewSmartScripts_Click(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count <= 0)
                return;

            FillFieldsBasedOnSelectedScript();
        }

        private void listViewSmartScripts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count <= 0)
                return;

            FillFieldsBasedOnSelectedScript();
        }

        private void FillFieldsBasedOnSelectedScript()
        {
            ListViewItem.ListViewSubItemCollection selectedItem = listViewSmartScripts.SelectedItems[0].SubItems;
            comboBoxEventType.SelectedIndex = Convert.ToInt32(selectedItem[4].Text);
            comboBoxActionType.SelectedIndex = Convert.ToInt32(selectedItem[12].Text);
            comboBoxTargetType.SelectedIndex = Convert.ToInt32(selectedItem[19].Text);
            textBoxComments.Text = selectedItem[27].Text;
            textBoxEventScriptId.Text = selectedItem[2].Text;
            textBoxEventLink.Text = selectedItem[3].Text;
            textBoxEventPhasemask.Text = selectedItem[5].Text;
            textBoxEventChance.Text = selectedItem[6].Text;
            textBoxEventFlags.Text = selectedItem[7].Text;

            //! Event parameters
            textBoxEventParam1.Text = selectedItem[8].Text;
            textBoxEventParam2.Text = selectedItem[9].Text;
            textBoxEventParam3.Text = selectedItem[10].Text;
            textBoxEventParam4.Text = selectedItem[11].Text;

            //! Action parameters
            textBoxActionParam1.Text = selectedItem[13].Text;
            textBoxActionParam2.Text = selectedItem[14].Text;
            textBoxActionParam3.Text = selectedItem[15].Text;
            textBoxActionParam4.Text = selectedItem[16].Text;
            textBoxActionParam5.Text = selectedItem[17].Text;
            textBoxActionParam6.Text = selectedItem[18].Text;

            //! Target parameters
            textBoxTargetParam1.Text = selectedItem[20].Text;
            textBoxTargetParam2.Text = selectedItem[21].Text;
            textBoxTargetParam3.Text = selectedItem[22].Text;
            textBoxTargetX.Text = selectedItem[23].Text;
            textBoxTargetY.Text = selectedItem[24].Text;
            textBoxTargetZ.Text = selectedItem[25].Text;
        }

        private void comboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true; //! Disallow changing content of the combobox, but setting it to 3D looks like shit
        }
    }
}
