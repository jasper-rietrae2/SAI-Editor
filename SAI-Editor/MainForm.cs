using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using MySql.Data.MySqlClient;

internal enum FormState
{
    FormStateLogin,
    FormStateExpandingOrContracting,
    FormStateMain,
};

internal enum FormSizes
{
    Width = 278,
    Height = 260,

    WidthToExpandTo = 985,
    HeightToExpandTo = 505,
};

internal enum MaxValues
{
    MaxEventType = 74,
    MaxActionType = 110,
    MaxTargetType = 26,
};

internal enum SourceTypes
{
    SourceTypeCreature = 0,
    SourceTypeGameobject = 1,
    SourceTypeScriptedActionlist = 9,
};

namespace SAI_Editor
{
    public partial class MainForm : Form
    {
        public Settings settings = new Settings();
        private readonly MySqlConnectionStringBuilder connectionString = new MySqlConnectionStringBuilder();
        private readonly List<Control> controlsLoginForm = new List<Control>();
        private readonly List<Control> controlsMainForm = new List<Control>();
        private bool contractingToLoginForm = false, expandingToMainForm = false;
        private int originalHeight = 0, originalWidth = 0;
        private readonly Timer timerExpandOrContract = new Timer { Enabled = false, Interval = 4 };
        private int WidthToExpandTo = (int)FormSizes.WidthToExpandTo, HeightToExpandTo = (int)FormSizes.HeightToExpandTo;
        public int animationSpeed = 5;
        private FormState formState = FormState.FormStateLogin;

        public MainForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            menuStrip.Visible = false; //! Doing this in main code so we can actually see the menustrip in designform

            MaximizeBox = false;
            MinimizeBox = true;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Width = (int)FormSizes.Width;
            Height = (int)FormSizes.Height;

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
            animationSpeed = Convert.ToInt32(settings.GetSetting("AnimationSpeed", "6"));

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
            menuItemDeleteSelectedRow.Click += menuOptionDeleteSelectedRow_Click;

            //! We hardcode the actual shortcuts because there are certain conditons under which the menu should not be
            //! opened at all.
            //menuItemExit.ShortcutKeys = (Keys.Shift | Keys.F5);
            menuItemExit.ShortcutKeyDisplayString = "(Shift + F5)";
            //menuItemReconnect.ShortcutKeys = (Keys.Shift | Keys.F4);
            menuItemReconnect.ShortcutKeyDisplayString = "(Shift + F4)";
            //menuItemSettings.ShortcutKeys = Keys.F1;
            menuItemSettings.ShortcutKeyDisplayString = "(F1)";
            //menuItemAbout.ShortcutKeys = (Keys.Alt | Keys.F1);
            menuItemAbout.ShortcutKeyDisplayString = "(Alt + F1)";
            //menuItemDeleteSelectedRow.ShortcutKeys = (Keys.Control | Keys.D);
            menuItemDeleteSelectedRow.ShortcutKeyDisplayString = "(Ctrl + D)";

            listViewSmartScripts.View = View.Details;
            listViewSmartScripts.FullRowSelect = true;
            listViewSmartScripts.Columns.Add("entryorguid", 67, HorizontalAlignment.Left);  // 0
            listViewSmartScripts.Columns.Add("source_type", 70, HorizontalAlignment.Right); // 1
            listViewSmartScripts.Columns.Add("id",          20, HorizontalAlignment.Right); // 2
            listViewSmartScripts.Columns.Add("link",        30, HorizontalAlignment.Right); // 3
            listViewSmartScripts.Columns.Add("event_type",  66, HorizontalAlignment.Right); // 4
            listViewSmartScripts.Columns.Add("event_phase", 74, HorizontalAlignment.Right); // 5
            listViewSmartScripts.Columns.Add("event_chance",81, HorizontalAlignment.Right); // 6
            listViewSmartScripts.Columns.Add("event_flags", 69, HorizontalAlignment.Right); // 7
            listViewSmartScripts.Columns.Add("p1",          24, HorizontalAlignment.Right); // 8
            listViewSmartScripts.Columns.Add("p2",          24, HorizontalAlignment.Right); // 9
            listViewSmartScripts.Columns.Add("p3",          24, HorizontalAlignment.Right); // 10
            listViewSmartScripts.Columns.Add("p4",          24, HorizontalAlignment.Right); // 11
            listViewSmartScripts.Columns.Add("action_type", 67, HorizontalAlignment.Right); // 12
            listViewSmartScripts.Columns.Add("p1",          24, HorizontalAlignment.Right); // 13
            listViewSmartScripts.Columns.Add("p2",          24, HorizontalAlignment.Right); // 14
            listViewSmartScripts.Columns.Add("p3",          24, HorizontalAlignment.Right); // 15
            listViewSmartScripts.Columns.Add("p4",          24, HorizontalAlignment.Right); // 16
            listViewSmartScripts.Columns.Add("p5",          24, HorizontalAlignment.Right); // 17
            listViewSmartScripts.Columns.Add("p6",          24, HorizontalAlignment.Right); // 18
            listViewSmartScripts.Columns.Add("target_type", 67, HorizontalAlignment.Right); // 19
            listViewSmartScripts.Columns.Add("p1",          24, HorizontalAlignment.Right); // 20
            listViewSmartScripts.Columns.Add("p2",          24, HorizontalAlignment.Right); // 21
            listViewSmartScripts.Columns.Add("p3",          24, HorizontalAlignment.Right); // 22
            listViewSmartScripts.Columns.Add("x",           20, HorizontalAlignment.Right); // 23
            listViewSmartScripts.Columns.Add("y",           20, HorizontalAlignment.Right); // 24
            listViewSmartScripts.Columns.Add("z",           20, HorizontalAlignment.Right); // 25
            listViewSmartScripts.Columns.Add("o",           20, HorizontalAlignment.Right); // 26
            listViewSmartScripts.Columns.Add("comment",     400, HorizontalAlignment.Left); // 27 (width 56 to fit)

            if (settings.GetSetting("AutoConnect", "no") == "yes")
            {
                checkBoxAutoConnect.Checked = true;
                buttonConnect_Click(sender, e);

                if (settings.GetSetting("InstantExpand", "no") == "yes")
                    StartExpandingToMainForm(true);
            }

            listViewSmartScripts.Click += listViewSmartScripts_Click;

            //listViewSmartScripts.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

            tabControlParameters.AutoScrollOffset = new Point(5, 5);

            //! Permanent scrollbar to the parameters tabpage windows
            foreach (TabPage page in tabControlParameters.TabPages)
            {
                page.HorizontalScroll.Enabled = false;
                page.HorizontalScroll.Visible = false;

                page.AutoScroll = true;
                page.AutoScrollMinSize = new Size(page.Width, page.Height);
            }

            foreach (Control control in tabControlParameters.Controls)
                if (control is TabPage)
                    foreach (Control controlTabPage in control.Controls)
                        if (controlTabPage is TextBox)
                            controlTabPage.KeyPress += numericField_KeyPress;

            textBoxEventTypeId.KeyPress += numericField_KeyPress;
            textBoxActionTypeId.KeyPress += numericField_KeyPress;
            textBoxTargetTypeId.KeyPress += numericField_KeyPress;
            textBoxEntryOrGuid.KeyPress += numericField_KeyPress;
            textBoxEventScriptId.KeyPress += numericField_KeyPress;
            textBoxLinkId.KeyPress += numericField_KeyPress;
            textBoxEventPhasemask.KeyPress += numericField_KeyPress;
            textBoxEventFlags.KeyPress += numericField_KeyPress;
            textBoxEventLink.KeyPress += numericField_KeyPress;

            //! Temp..
            panelLoginBox.Location = new Point(9, 8);

            if (settings.GetSetting("DontHidePass", "no") == "no")
                textBoxPassword.PasswordChar = '*';
        }

        private void timerExpandOrContract_Tick(object sender, EventArgs e)
        {
            if (expandingToMainForm)
            {
                if (Height < HeightToExpandTo)
                    Height += animationSpeed;
                else
                {
                    Height = HeightToExpandTo;

                    if (Width >= WidthToExpandTo) //! If both finished
                    {
                        Width = WidthToExpandTo;
                        timerExpandOrContract.Enabled = false;
                        expandingToMainForm = false;
                        FinishedExpandingOrContracting(true);
                        formState = FormState.FormStateMain;
                    }
                }

                if (Width < WidthToExpandTo)
                    Width += animationSpeed;
                else
                {
                    Width = WidthToExpandTo;

                    if (Height >= HeightToExpandTo) //! If both finished
                    {
                        Height = HeightToExpandTo;
                        timerExpandOrContract.Enabled = false;
                        expandingToMainForm = false;
                        FinishedExpandingOrContracting(true);
                        formState = FormState.FormStateMain;
                    }
                }
            }
            else if (contractingToLoginForm)
            {
                if (Height > originalHeight)
                    Height -= animationSpeed;
                else
                {
                    Height = originalHeight;

                    if (Width <= originalWidth) //! If both finished
                    {
                        Width = originalWidth;
                        timerExpandOrContract.Enabled = false;
                        contractingToLoginForm = false;
                        FinishedExpandingOrContracting(false);
                        formState = FormState.FormStateLogin;
                    }
                }

                if (Width > originalWidth)
                    Width -= animationSpeed;
                else
                {
                    Width = originalWidth;

                    if (Height <= originalHeight) //! If both finished
                    {
                        Height = originalHeight;
                        timerExpandOrContract.Enabled = false;
                        contractingToLoginForm = false;
                        FinishedExpandingOrContracting(false);
                        formState = FormState.FormStateLogin;
                    }
                }
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (IsEmptyString(textBoxHost.Text))
            {
                MessageBox.Show("The host field has to be filled!", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (IsEmptyString(textBoxUsername.Text))
            {
                MessageBox.Show("The username field has to be filled!", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (textBoxPassword.Text.Length > 0 && IsEmptyString(textBoxPassword.Text))
            {
                MessageBox.Show("The password field can not consist of only whitespaces!", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (IsEmptyString(textBoxWorldDatabase.Text))
            {
                MessageBox.Show("The world database field has to be filled!", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (IsEmptyString(textBoxPort.Text))
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
                formState = FormState.FormStateMain;
            }
            else
            {
                formState = FormState.FormStateExpandingOrContracting;
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
                formState = FormState.FormStateLogin;
            }
            else
            {
                formState = FormState.FormStateExpandingOrContracting;
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

        private bool IsEmptyString(string str)
        {
            return String.IsNullOrEmpty(str) || String.IsNullOrWhiteSpace(str);
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
                MessageBox.Show(ex.Message, "Could not connect", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                case Keys.Enter:
                    if (formState == FormState.FormStateLogin)
                        buttonConnect_Click(sender, e);
                    break;
                case Keys.F5:
                    if (panelLoginBox.Location.X == 1000 && panelLoginBox.Location.Y == 50)
                        panelLoginBox.Location = new Point(9, 8);
                    else
                        panelLoginBox.Location = new Point(1000, 50);
                    break;
            }

            //! Hardcode shortcuts to menu because we can't use conditions otherwise
            if (formState == FormState.FormStateMain)
            {
                if (e.KeyData == (Keys.Shift | Keys.F5) || e.KeyData == (Keys.ShiftKey | Keys.F5))
                    TryCloseApplication();
                else if (e.KeyData == (Keys.Shift | Keys.F4) || e.KeyData == (Keys.ShiftKey | Keys.F4))
                    menuItemReconnect_Click(sender, e);
                else if (e.KeyData == Keys.F1)
                    menuItemSettings_Click(sender, e);
                else if (e.KeyData == (Keys.Alt | Keys.F1))
                    menuItemAbout_Click(sender, e);
                else if (e.KeyData == (Keys.Control | Keys.D) || e.KeyData == (Keys.ControlKey | Keys.D))
                    menuOptionDeleteSelectedRow_Click(sender, e);
            }
        }

        private void buttonSearchForCreature_Click(object sender, EventArgs e)
        {
            //! Just keep it in main thread; no purpose starting a new thread for this (unless workspaces get implemented, maybe)
            new SearchForEntryForm(connectionString, textBoxEntryOrGuid.Text, comboBoxSourceType.SelectedIndex == 0).ShowDialog(this);
        }

        private void menuItemReconnect_Click(object sender, EventArgs e)
        {
            StartContractingToLoginForm(settings.GetSetting("InstantExpand", "no") == "yes");
        }

        private void comboBoxEventType_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxEventTypeId.Text = comboBoxEventType.SelectedIndex.ToString();

            if (textBoxEventTypeId.Text == "0" || textBoxEventTypeId.Text == ((int)MaxValues.MaxEventType).ToString())
                textBoxEventTypeId.SelectionStart = 3; //! Set cursot to end of text
        }

        private void comboBoxActionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxActionTypeId.Text = comboBoxActionType.SelectedIndex.ToString();

            if (textBoxActionTypeId.Text == "0" || textBoxActionTypeId.Text == ((int)MaxValues.MaxActionType).ToString())
                textBoxActionTypeId.SelectionStart = 3; //! Set cursot to end of text
        }

        private void comboBoxTargetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxTargetTypeId.Text = comboBoxTargetType.SelectedIndex.ToString();

            if (textBoxTargetTypeId.Text == "0" || textBoxTargetTypeId.Text == ((int)MaxValues.MaxTargetType).ToString())
                textBoxTargetTypeId.SelectionStart = 3; //! Set cursot to end of text
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

        private void SelectAndFillListViewWithQuery(string queryToExecute, string entryOrGuid, SourceTypes sourceType)
        {
            var timedActionlistEntries = new List<int>();

            try
            {
                using (var connection = new MySqlConnection(connectionString.ToString()))
                {
                    connection.Open();
                    var returnVal = new MySqlDataAdapter(String.Format("SELECT * FROM smart_scripts WHERE entryorguid={0} AND source_type={1}", entryOrGuid, (int)sourceType), connection);
                    var dataTable = new DataTable();
                    returnVal.Fill(dataTable);

                    if (dataTable.Rows.Count <= 0)
                    {
                        MessageBox.Show(String.Format("The entryorguid '{0}' could not be found in the SmartAI table for the given source type ({1})!", entryOrGuid, (int)sourceType), "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                        if (checkBoxListActionlists.Checked && sourceType != SourceTypes.SourceTypeScriptedActionlist)
                        {
                            int actionParam1 = Convert.ToInt32(row.ItemArray[13].ToString());
                            int actionParam2 = Convert.ToInt32(row.ItemArray[14].ToString());

                            if (Convert.ToInt32(row.ItemArray[12].ToString()) == 80)      // SMART_ACTION_CALL_TIMED_ACTIONLIST
                                timedActionlistEntries.Add(actionParam1);
                            else if (Convert.ToInt32(row.ItemArray[12].ToString()) == 87) // SMART_ACTION_CALL_RANDOM_TIMED_ACTIONLIST
                            {
                                for (int i = 13; i < 19; ++i)
                                {
                                    if (Convert.ToInt32(row.ItemArray[i].ToString()) == 0)
                                        break; //! Once the first 0 is reached we can stop looking for other scripts, no gaps allowed

                                    timedActionlistEntries.Add(Convert.ToInt32(row.ItemArray[i].ToString()));
                                }
                            }
                            else if (Convert.ToInt32(row.ItemArray[12].ToString()) == 88) // SMART_ACTION_CALL_RANDOM_RANGE_TIMED_ACTIONLIST
                            {
                                for (int i = actionParam1; i <= actionParam2; ++i)
                                    timedActionlistEntries.Add(i);
                            }
                        }

                        listViewSmartScripts.Items.Add(listViewItem);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (checkBoxListActionlists.Checked && sourceType != SourceTypes.SourceTypeScriptedActionlist)
                foreach (var scriptEntry in timedActionlistEntries)
                    SelectAndFillListViewWithQuery(String.Format("SELECT * FROM smart_scripts WHERE entryorguid={0} AND source_type=9", scriptEntry), scriptEntry.ToString(), SourceTypes.SourceTypeScriptedActionlist);
        }

        //! Needs object and EventAgrs parameters so we can trigger it as an event when 'Exit' is called from the menu.
        private void TryCloseApplication(object sender = null, EventArgs e = null)
        {
            if (settings.GetSetting("PromptToQuit", "yes") == "no" || MessageBox.Show("Are you sure you want to quit?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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

            menuItemDeleteSelectedRow.Text = "Delete selected row";

            //! Set 'row' to 'rows' if there are several rows selected
            if (listViewSmartScripts.SelectedItems.Count > 1)
                menuItemDeleteSelectedRow.Text += "s";

            FillFieldsBasedOnSelectedScript();
        }

        private void FillFieldsBasedOnSelectedScript()
        {
            ListViewItem.ListViewSubItemCollection selectedItem = listViewSmartScripts.SelectedItems[0].SubItems;

            textBoxEntryOrGuid.Text = selectedItem[0].Text;

            switch (Convert.ToInt32(selectedItem[1].Text))
            {
                case 0: //! Creature
                case 1: //! Gameobject
                case 2: //! Areatrigger
                    comboBoxSourceType.SelectedIndex = Convert.ToInt32(selectedItem[1].Text);
                    break;
                case 9:
                    comboBoxSourceType.SelectedIndex = 3;
                    break;
                default:
                    MessageBox.Show(String.Format("Unknown/unsupported source type found ({0})", selectedItem[0].Text), "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

            textBoxEventScriptId.Text = selectedItem[2].Text;
            textBoxEventLink.Text = selectedItem[3].Text;

            comboBoxEventType.SelectedIndex = Convert.ToInt32(selectedItem[4].Text);
            textBoxEventPhasemask.Text = selectedItem[5].Text;
            textBoxEventChance.Text = selectedItem[6].Text;
            textBoxEventFlags.Text = selectedItem[7].Text;

            //! Event parameters
            textBoxEventParam1.Text = selectedItem[8].Text;
            textBoxEventParam2.Text = selectedItem[9].Text;
            textBoxEventParam3.Text = selectedItem[10].Text;
            textBoxEventParam4.Text = selectedItem[11].Text;

            //! Action parameters
            comboBoxActionType.SelectedIndex = Convert.ToInt32(selectedItem[12].Text);
            textBoxActionParam1.Text = selectedItem[13].Text;
            textBoxActionParam2.Text = selectedItem[14].Text;
            textBoxActionParam3.Text = selectedItem[15].Text;
            textBoxActionParam4.Text = selectedItem[16].Text;
            textBoxActionParam5.Text = selectedItem[17].Text;
            textBoxActionParam6.Text = selectedItem[18].Text;

            //! Target parameters
            comboBoxTargetType.SelectedIndex = Convert.ToInt32(selectedItem[19].Text);
            textBoxTargetParam1.Text = selectedItem[20].Text;
            textBoxTargetParam2.Text = selectedItem[21].Text;
            textBoxTargetParam3.Text = selectedItem[22].Text;
            textBoxTargetX.Text = selectedItem[23].Text;
            textBoxTargetY.Text = selectedItem[24].Text;
            textBoxTargetZ.Text = selectedItem[25].Text;
            textBoxTargetO.Text = selectedItem[26].Text;

            textBoxComments.Text = selectedItem[27].Text;
        }

        private void comboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true; //! Disallow changing content of the combobox (because setting it to 3D looks like shit)
        }

        private void textBoxEventTypeId_TextChanged(object sender, EventArgs e)
        {
            if (IsEmptyString(textBoxEventTypeId.Text))
            {
                comboBoxEventType.SelectedIndex = 0;
                textBoxEventTypeId.Text = "0";
                textBoxEventTypeId.SelectionStart = 3; //! Set cursor position to end of the line
            }
            else if (Convert.ToInt32(textBoxEventTypeId.Text) > (int)MaxValues.MaxEventType)
            {
                comboBoxEventType.SelectedIndex = (int)MaxValues.MaxEventType;
                textBoxEventTypeId.Text = ((int)MaxValues.MaxEventType).ToString();
                textBoxEventTypeId.SelectionStart = 3; //! Set cursor position to end of the line
            }
            else
                comboBoxEventType.SelectedIndex = Convert.ToInt32(textBoxEventTypeId.Text);
        }

        private void textBoxActionTypeId_TextChanged(object sender, EventArgs e)
        {
            if (IsEmptyString(textBoxActionTypeId.Text))
            {
                comboBoxActionType.SelectedIndex = 0;
                textBoxActionTypeId.Text = "0";
                textBoxActionTypeId.SelectionStart = 3; //! Set cursor position to end of the line
            }
            else if (Convert.ToInt32(textBoxActionTypeId.Text) > (int)MaxValues.MaxActionType)
            {
                comboBoxActionType.SelectedIndex = (int)MaxValues.MaxActionType;
                textBoxActionTypeId.Text = ((int)MaxValues.MaxActionType).ToString();
                textBoxActionTypeId.SelectionStart = 3; //! Set cursor position to end of the line
            }
            else
                comboBoxActionType.SelectedIndex = Convert.ToInt32(textBoxActionTypeId.Text);
        }

        private void textBoxTargetTypeId_TextChanged(object sender, EventArgs e)
        {
            if (IsEmptyString(textBoxTargetTypeId.Text))
            {
                comboBoxTargetType.SelectedIndex = 0;
                textBoxTargetTypeId.Text = "0";
                textBoxTargetTypeId.SelectionStart = 3; //! Set cursor position to end of the line
            }
            else if (Convert.ToInt32(textBoxTargetTypeId.Text) > (int)MaxValues.MaxTargetType)
            {
                comboBoxTargetType.SelectedIndex = (int)MaxValues.MaxTargetType;
                textBoxTargetTypeId.Text = ((int)MaxValues.MaxTargetType).ToString();
                textBoxTargetTypeId.SelectionStart = 3; //! Set cursor position to end of the line
            }
            else
                comboBoxTargetType.SelectedIndex = Convert.ToInt32(textBoxTargetTypeId.Text);
        }

        private void menuOptionDeleteSelectedRow_Click(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count <= 0)
            {
                MessageBox.Show("No rows were selected to delete!", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (ListViewItem listViewItem in listViewSmartScripts.SelectedItems)
                listViewSmartScripts.Items.Remove(listViewItem);
        }

        private void checkBoxListActionlists_CheckedChanged(object sender, EventArgs e)
        {
            if (listViewSmartScripts.Items.Count > 0 && GetSourceTypeByIndex() != (int)SourceTypes.SourceTypeScriptedActionlist)
            {
                listViewSmartScripts.Items.Clear();
                
                //! to-do: We need to store the entry and source type we're searching for so we can use it here, else we're getting wrong results. To test
                //! you can search for an entry with scripted actionlist(s), like 
                SelectAndFillListViewWithQuery(String.Format("SELECT * FROM smart_scripts WHERE entryorguid={0} AND source_type={1}", textBoxEntryOrGuid.Text, GetSourceTypeByIndex()), textBoxEntryOrGuid.Text, (SourceTypes)GetSourceTypeByIndex());
            }
        }

        private int GetSourceTypeByIndex()
        {
            switch (comboBoxSourceType.SelectedIndex)
            {
                case 0: //! Creature
                case 1: //! Gameobject
                case 2: //! Areatrigger
                    return comboBoxSourceType.SelectedIndex;
                case 3: //! Actionlist
                    return 9;
                default:
                    return -1;
            }
        }

        public void pictureBox1_Click(object sender, EventArgs e)
        {
            if (IsEmptyString(textBoxEntryOrGuid.Text))
                return;

            listViewSmartScripts.Items.Clear();
            SelectAndFillListViewWithQuery(String.Format("SELECT * FROM smart_scripts WHERE entryorguid={0} AND source_type={1}", textBoxEntryOrGuid.Text, GetSourceTypeByIndex()), textBoxEntryOrGuid.Text, (SourceTypes)GetSourceTypeByIndex());
        }

        private void numericField_KeyPress(object sender, KeyPressEventArgs e)
        {
            //! Only allow typing keys that are numbers
            //! Inset is '-'
            if (!Char.IsNumber(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Insert)
                e.Handled = true;
        }

        private void buttonSearchPhasemask_Click(object sender, EventArgs e)
        {
            new MultiSelectForm(true).ShowDialog(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new MultiSelectForm(false).ShowDialog(this);
        }
    }
}
