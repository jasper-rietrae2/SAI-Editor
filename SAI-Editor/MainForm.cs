using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using MySql.Data.MySqlClient;
using SAI_Editor.Properties;
using SAI_Editor.Database.Classes;

namespace SAI_Editor
{
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

    public enum SourceTypes
    {
        SourceTypeCreature = 0,
        SourceTypeGameobject = 1,
        SourceTypeAreaTrigger = 2,
        SourceTypeScriptedActionlist = 9,
    };

    public partial class MainForm : Form
    {
        private readonly MySqlConnectionStringBuilder connectionString = new MySqlConnectionStringBuilder();
        private readonly List<Control> controlsLoginForm = new List<Control>();
        private readonly List<Control> controlsMainForm = new List<Control>();
        private bool contractingToLoginForm = false, expandingToMainForm = false;
        private int originalHeight = 0, originalWidth = 0;
        private readonly Timer timerExpandOrContract = new Timer { Enabled = false, Interval = 4 };
        private int WidthToExpandTo = (int)FormSizes.WidthToExpandTo, HeightToExpandTo = (int)FormSizes.HeightToExpandTo;
        public int animationSpeed = 5;
        private FormState formState = FormState.FormStateLogin;
        public SourceTypes originalSourceType = SourceTypes.SourceTypeCreature;
        public String originalEntryOrGuid = String.Empty;
        private readonly ListViewColumnSorter lvwColumnSorter = new ListViewColumnSorter();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            menuStrip.Visible = false; //! Doing this in main code so we can actually see the menustrip in designform

            Width = (int)FormSizes.Width;
            Height = (int)FormSizes.Height;

            originalHeight = Height;
            originalWidth = Width;

            if (WidthToExpandTo > SystemInformation.VirtualScreen.Width)
                WidthToExpandTo = SystemInformation.VirtualScreen.Width;

            if (HeightToExpandTo > SystemInformation.VirtualScreen.Height)
                HeightToExpandTo = SystemInformation.VirtualScreen.Height;

            try
            {
                textBoxHost.Text = Settings.Default.Host;
                textBoxUsername.Text = Settings.Default.User;
                textBoxPassword.Text = Settings.Default.Password;
                textBoxWorldDatabase.Text = Settings.Default.Database;
                textBoxPort.Text = Settings.Default.Port.ToString();
                animationSpeed = Convert.ToInt32(Settings.Default.AnimationSpeed);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

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

            listViewSmartScripts.ColumnClick += listViewSmartScripts_ColumnClick;

            if (Settings.Default.AutoConnect)
            {
                checkBoxAutoConnect.Checked = true;
                buttonConnect_Click(sender, e);

                if (Settings.Default.InstantExpand)
                    StartExpandingToMainForm(true);
            }

            tabControlParameters.AutoScrollOffset = new Point(5, 5);

            //! Permanent scrollbar to the parameters tabpage windows
            foreach (TabPage page in tabControlParameters.TabPages)
            {
                page.HorizontalScroll.Enabled = false;
                page.HorizontalScroll.Visible = false;

                page.AutoScroll = true;
                page.AutoScrollMinSize = new Size(page.Width, page.Height);
            }

            //! Temp..
            panelLoginBox.Location = new Point(9, 8);

            if (Settings.Default.HidePass)
                textBoxPassword.PasswordChar = '●';
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
            if (String.IsNullOrEmpty(textBoxHost.Text))
            {
                MessageBox.Show("The host field has to be filled!", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (String.IsNullOrEmpty(textBoxUsername.Text))
            {
                MessageBox.Show("The username field has to be filled!", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (textBoxPassword.Text.Length > 0 && String.IsNullOrEmpty(textBoxPassword.Text))
            {
                MessageBox.Show("The password field can not consist of only whitespaces!", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (String.IsNullOrEmpty(textBoxWorldDatabase.Text))
            {
                MessageBox.Show("The world database field has to be filled!", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (String.IsNullOrEmpty(textBoxPort.Text))
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
                StartExpandingToMainForm(Settings.Default.InstantExpand);
        }

        private void StartExpandingToMainForm(bool instant = false)
        {
            if (checkBoxSaveSettings.Checked)
            {
                Settings.Default.Host = textBoxHost.Text;
                Settings.Default.User = textBoxUsername.Text;
                Settings.Default.Password = textBoxPassword.Text;
                Settings.Default.Database = textBoxWorldDatabase.Text;
                Settings.Default.AutoConnect = checkBoxAutoConnect.Checked;
                Settings.Default.Port = Convert.ToInt32(textBoxPort.Text);
                Settings.Default.Save();
            }

            ResetNonStaticFieldsToDefault();
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

        private bool CanConnectToDatabase()
        {
            try
            {
                //! Close the connection again since this is just a try-connection function. We actually connect
                //! when the mainform is opened (this happens automatically because we use 'using').
                using (MySqlConnection connection = new MySqlConnection(connectionString.ToString()))
                    connection.Open();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Could not connect", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
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
                    else if (formState == FormState.FormStateMain && textBoxEntryOrGuid.Focused)
                        pictureBoxLoadScript_Click(sender, e);
                    break;
                case Keys.F5: //! Temp to make it easier to design
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
            new SearchForEntryForm(connectionString, textBoxEntryOrGuid.Text, GetSourceTypeByIndex()).ShowDialog(this);
        }

        private void menuItemReconnect_Click(object sender, EventArgs e)
        {
            listViewSmartScripts.Items.Clear();
            StartContractingToLoginForm(Settings.Default.InstantExpand);
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

        private async void SelectAndFillListViewByEntryAndSource(string entryOrGuid, SourceTypes sourceType)
        {
            var timedActionlistsOrEntries = new List<string>();
            SourceTypes sourceTypeOfEntry = SourceTypes.SourceTypeScriptedActionlist;

            try
            {
                List<SmartScript> smartScripts = await SAI_Editor_Manager.Instance.worldDatabase.GetSmartScripts(Convert.ToInt32(entryOrGuid), (int)sourceType);

                if (smartScripts == null)
                {
                    MessageBox.Show(String.Format("The entryorguid '{0}' could not be found in the SmartAI table for the given source type ({1})!", entryOrGuid, GetSourceTypeString(sourceType)), "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                foreach (SmartScript smartScript in smartScripts)
                {
                    var listViewItem = new ListViewItem();
                    listViewItem.Text = smartScript.entryorguid.ToString();
                    listViewItem.SubItems.Add(smartScript.source_type.ToString());
                    listViewItem.SubItems.Add(smartScript.id.ToString());
                    listViewItem.SubItems.Add(smartScript.link.ToString());
                    listViewItem.SubItems.Add(smartScript.event_type.ToString());
                    listViewItem.SubItems.Add(smartScript.event_phase_mask.ToString());
                    listViewItem.SubItems.Add(smartScript.event_chance.ToString());
                    listViewItem.SubItems.Add(smartScript.event_flags.ToString());
                    listViewItem.SubItems.Add(smartScript.event_param1.ToString());
                    listViewItem.SubItems.Add(smartScript.event_param2.ToString());
                    listViewItem.SubItems.Add(smartScript.event_param3.ToString());
                    listViewItem.SubItems.Add(smartScript.event_param4.ToString());
                    listViewItem.SubItems.Add(smartScript.action_type.ToString());
                    listViewItem.SubItems.Add(smartScript.action_param1.ToString());
                    listViewItem.SubItems.Add(smartScript.action_param2.ToString());
                    listViewItem.SubItems.Add(smartScript.action_param3.ToString());
                    listViewItem.SubItems.Add(smartScript.action_param4.ToString());
                    listViewItem.SubItems.Add(smartScript.action_param5.ToString());
                    listViewItem.SubItems.Add(smartScript.action_param6.ToString());
                    listViewItem.SubItems.Add(smartScript.target_type.ToString());
                    listViewItem.SubItems.Add(smartScript.target_param1.ToString());
                    listViewItem.SubItems.Add(smartScript.target_param2.ToString());
                    listViewItem.SubItems.Add(smartScript.target_param3.ToString());
                    listViewItem.SubItems.Add(smartScript.target_x.ToString());
                    listViewItem.SubItems.Add(smartScript.target_y.ToString());
                    listViewItem.SubItems.Add(smartScript.target_z.ToString());
                    listViewItem.SubItems.Add(smartScript.target_o.ToString());
                    listViewItem.SubItems.Add(smartScript.comment);

                    if (checkBoxListActionlistsOrEntries.Checked && sourceType == originalSourceType)
                    {
                        TimedActionListOrEntries timedActionListOrEntries = await SAI_Editor_Manager.Instance.GetTimedActionlistsOrEntries(smartScript, sourceType);

                        timedActionlistsOrEntries = timedActionListOrEntries.entries;
                        sourceTypeOfEntry = timedActionListOrEntries.sourceTypeOfEntry;
                    }

                    listViewSmartScripts.Items.Add(listViewItem);
                }

                for (int i = 8; i <= 11; ++i)
                    listViewSmartScripts.Columns[i].Width = -1;

                for (int i = 13; i <= 18; ++i)
                    listViewSmartScripts.Columns[i].Width = -1;

                for (int i = 20; i <= 26; ++i)
                    listViewSmartScripts.Columns[i].Width = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (checkBoxListActionlistsOrEntries.Checked)
                foreach (string scriptEntry in timedActionlistsOrEntries)
                    SelectAndFillListViewByEntryAndSource(scriptEntry, sourceTypeOfEntry);
        }

        //! Needs object and EventAgrs parameters so we can trigger it as an event when 'Exit' is called from the menu.
        private void TryCloseApplication(object sender = null, EventArgs e = null)
        {
            if (!Settings.Default.PromptToQuit || MessageBox.Show("Are you sure you want to quit?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
            {
                ResetNonStaticFieldsToDefault();
                return;
            }

            menuItemDeleteSelectedRow.Text = "Delete selected row";

            //! Set 'row' to 'rows' if there are several rows selected
            if (listViewSmartScripts.SelectedItems.Count > 1)
                menuItemDeleteSelectedRow.Text += "s";

            FillFieldsBasedOnSelectedScript();
        }

        private async void FillFieldsBasedOnSelectedScript()
        {
            ListViewItem.ListViewSubItemCollection selectedItem = listViewSmartScripts.SelectedItems[0].SubItems;

            if (Settings.Default.ChangeStaticInfo)
            {
                textBoxEntryOrGuid.Text = selectedItem[0].Text;

                switch (Convert.ToInt32(selectedItem[1].Text))
                {
                    case 0: //! Creature
                    case 1: //! Gameobject
                    case 2: //! Areatrigger
                        comboBoxSourceType.SelectedIndex = Convert.ToInt32(selectedItem[1].Text);
                        break;
                    case 9: //! Actionlist
                        comboBoxSourceType.SelectedIndex = 3;
                        break;
                    default:
                        MessageBox.Show(String.Format("Unknown/unsupported source type found ({0})", selectedItem[0].Text), "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                }
            }

            textBoxEventScriptId.Text = selectedItem[2].Text;
            textBoxLinkTo.Text = selectedItem[3].Text;

            int event_type = Convert.ToInt32(selectedItem[4].Text);

            comboBoxEventType.SelectedIndex = event_type;
            textBoxEventPhasemask.Text = selectedItem[5].Text;
            textBoxEventChance.Text = selectedItem[6].Text;
            textBoxEventFlags.Text = selectedItem[7].Text;

            //! Event parameters
          //comboBoxEventType.SelectedIndex = event_type;
            textBoxEventParam1.Text = selectedItem[8].Text;
            textBoxEventParam2.Text = selectedItem[9].Text;
            textBoxEventParam3.Text = selectedItem[10].Text;
            textBoxEventParam4.Text = selectedItem[11].Text;
            labelEventParam1.Text = await SAI_Editor_Manager.Instance.sqliteDatabase.GetParameterStringsById(event_type, 1, ScriptTypeId.ScriptTypeEvent);
            labelEventParam2.Text = await SAI_Editor_Manager.Instance.sqliteDatabase.GetParameterStringsById(event_type, 2, ScriptTypeId.ScriptTypeEvent);
            labelEventParam3.Text = await SAI_Editor_Manager.Instance.sqliteDatabase.GetParameterStringsById(event_type, 3, ScriptTypeId.ScriptTypeEvent);
            labelEventParam4.Text = await SAI_Editor_Manager.Instance.sqliteDatabase.GetParameterStringsById(event_type, 4, ScriptTypeId.ScriptTypeEvent);

            //! Action parameters
            int action_type = Convert.ToInt32(selectedItem[12].Text);
            comboBoxActionType.SelectedIndex = action_type;
            textBoxActionParam1.Text = selectedItem[13].Text;
            textBoxActionParam2.Text = selectedItem[14].Text;
            textBoxActionParam3.Text = selectedItem[15].Text;
            textBoxActionParam4.Text = selectedItem[16].Text;
            textBoxActionParam5.Text = selectedItem[17].Text;
            textBoxActionParam6.Text = selectedItem[18].Text;
            labelActionParam1.Text = await SAI_Editor_Manager.Instance.sqliteDatabase.GetParameterStringsById(action_type, 1, ScriptTypeId.ScriptTypeAction);
            labelActionParam2.Text = await SAI_Editor_Manager.Instance.sqliteDatabase.GetParameterStringsById(action_type, 2, ScriptTypeId.ScriptTypeAction);
            labelActionParam3.Text = await SAI_Editor_Manager.Instance.sqliteDatabase.GetParameterStringsById(action_type, 3, ScriptTypeId.ScriptTypeAction);
            labelActionParam4.Text = await SAI_Editor_Manager.Instance.sqliteDatabase.GetParameterStringsById(action_type, 4, ScriptTypeId.ScriptTypeAction);
            labelActionParam5.Text = await SAI_Editor_Manager.Instance.sqliteDatabase.GetParameterStringsById(action_type, 5, ScriptTypeId.ScriptTypeAction);
            labelActionParam6.Text = await SAI_Editor_Manager.Instance.sqliteDatabase.GetParameterStringsById(action_type, 6, ScriptTypeId.ScriptTypeAction);

            //! Target parameters
            int target_type = Convert.ToInt32(selectedItem[19].Text);
            comboBoxTargetType.SelectedIndex = target_type;
            textBoxTargetParam1.Text = selectedItem[20].Text;
            textBoxTargetParam2.Text = selectedItem[21].Text;
            textBoxTargetParam3.Text = selectedItem[22].Text;
            labelTargetParam1.Text = await SAI_Editor_Manager.Instance.sqliteDatabase.GetParameterStringsById(target_type, 1, ScriptTypeId.ScriptTypeTarget);
            labelTargetParam2.Text = await SAI_Editor_Manager.Instance.sqliteDatabase.GetParameterStringsById(target_type, 2, ScriptTypeId.ScriptTypeTarget);
            labelTargetParam3.Text = await SAI_Editor_Manager.Instance.sqliteDatabase.GetParameterStringsById(target_type, 3, ScriptTypeId.ScriptTypeTarget);
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
            if (String.IsNullOrEmpty(textBoxEventTypeId.Text))
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
            if (String.IsNullOrEmpty(textBoxActionTypeId.Text))
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
            if (String.IsNullOrEmpty(textBoxTargetTypeId.Text))
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
            if (listViewSmartScripts.Items.Count == 0)
                return;

            if (checkBoxListActionlistsOrEntries.Checked)
            {
                listViewSmartScripts.Items.Clear();
                SelectAndFillListViewByEntryAndSource(originalEntryOrGuid, originalSourceType);
            }
            else
                RemoveNonOriginalScriptsFromView();

            ListView.SelectedIndexCollection selectedIndices = listViewSmartScripts.SelectedIndices;

            for (int i = 0; i < selectedIndices.Count; ++i)
                if (listViewSmartScripts.Items[selectedIndices[i]] != null)
                    listViewSmartScripts.Items[selectedIndices[i]].Selected  = true;
        }

        private void RemoveNonOriginalScriptsFromView()
        {
            foreach (ListViewItem item in listViewSmartScripts.Items)
                if ((SourceTypes)Int32.Parse(item.SubItems[1].Text) != originalSourceType)
                    listViewSmartScripts.Items.Remove(item);
        }

        public SourceTypes GetSourceTypeByIndex()
        {
            switch (comboBoxSourceType.SelectedIndex)
            {
                case 0: //! Creature
                case 1: //! Gameobject
                case 2: //! Areatrigger
                    return (SourceTypes)comboBoxSourceType.SelectedIndex;
                case 3: //! Actionlist
                    return SourceTypes.SourceTypeScriptedActionlist;
                default:
                    return SourceTypes.SourceTypeCreature; //! Default...
            }
        }

        public int GetIndexBySourceType(SourceTypes sourceType)
        {
            switch (sourceType)
            {
                case SourceTypes.SourceTypeCreature:
                case SourceTypes.SourceTypeGameobject:
                case SourceTypes.SourceTypeAreaTrigger:
                    return (int)sourceType;
                case SourceTypes.SourceTypeScriptedActionlist:
                    return 3;
                default:
                    return -1;
            }
        }

        public void pictureBoxLoadScript_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxEntryOrGuid.Text))
                return;

            SourceTypes newSourceType = GetSourceTypeByIndex();

            originalSourceType = newSourceType;
            originalEntryOrGuid = textBoxEntryOrGuid.Text;
            listViewSmartScripts.Items.Clear();
            SelectAndFillListViewByEntryAndSource(textBoxEntryOrGuid.Text, newSourceType);

            checkBoxListActionlistsOrEntries.Text = newSourceType == SourceTypes.SourceTypeScriptedActionlist ? "List entries too" : "List actionlists too";
        }

        private void numericField_KeyPress(object sender, KeyPressEventArgs e)
        {
            //! Only allow typing keys that are numbers
            //! Inset is '-'
            //if (!Char.IsNumber(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.ControlKey && e.KeyChar != (char)Keys.Insert)
            //    e.Handled = true;
        }

        private void buttonSearchPhasemask_Click(object sender, EventArgs e)
        {
            new MultiSelectForm(true).ShowDialog(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new MultiSelectForm(false).ShowDialog(this);
        }

        private void buttonSearchWorldDb_Click(object sender, EventArgs e)
        {
            if (textBoxHost.Text.Length <= 0 || textBoxUsername.Text.Length <= 0 || textBoxPort.Text.Length <= 0)
            {
                MessageBox.Show("You must fill all fields except for the world database field in order to search for your world database (we need to establish a connection to list your databases)!", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var databaseNames = new List<string>();

            MySqlConnectionStringBuilder _connectionString = new MySqlConnectionStringBuilder();
            _connectionString.Server = textBoxHost.Text;
            _connectionString.UserID = textBoxUsername.Text;
            _connectionString.Port = Convert.ToUInt16(textBoxPort.Text);

            if (textBoxPassword.Text.Length > 0)
                _connectionString.Password = textBoxPassword.Text;

            try
            {
                using (var connection = new MySqlConnection(_connectionString.ToString()))
                {
                    connection.Open();
                    var returnVal = new MySqlDataAdapter("SHOW DATABASES", connection);
                    var dataTable = new DataTable();
                    returnVal.Fill(dataTable);

                    if (dataTable.Rows.Count <= 0)
                    {
                        MessageBox.Show("Your connection contains no databases!", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    foreach (DataRow row in dataTable.Rows)
                        for (int i = 0; i < row.ItemArray.Length; i++)
                            databaseNames.Add(row.ItemArray[i].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (databaseNames.Count > 0)
                    new SelectDatabaseForm(databaseNames).Show(this);
            }
        }

        private void listViewSmartScripts_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            var myListView = (ListView)sender;
            myListView.ListViewItemSorter = lvwColumnSorter;

            //! Determine if clicked column is already the column that is being sorted
            if (e.Column != lvwColumnSorter.SortColumn)
            {
                //! Set the column number that is to be sorted; default to ascending
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }
            else
                //! Reverse the current sort direction for this column
                lvwColumnSorter.Order = lvwColumnSorter.Order == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;

            //! Perform the sort with these new sort options
            myListView.Sort();
        }

        private ListView.ListViewItemCollection GetItemsBasedOnSelection(ListView listView)
        {
            ListView listViewScriptsCopy = new ListView();

            foreach (ListViewItem item in listView.Items)
                if (item.SubItems[1].Text == listView.SelectedItems[0].SubItems[1].Text)
                    listViewScriptsCopy.Items.Add((ListViewItem)item.Clone());

            return listViewScriptsCopy.Items;
        }

        private void buttonLinkTo_Click(object sender, EventArgs e)
        {
            TryToOpenLinkForm(true);
        }

        private void buttonLinkFrom_Click(object sender, EventArgs e)
        {
            TryToOpenLinkForm(false);
        }

        private void TryToOpenLinkForm(bool linkTo)
        {
            if (listViewSmartScripts.SelectedItems.Count <= 0)
            {
                MessageBox.Show("You must first select a line in the script", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (listViewSmartScripts.SelectedItems.Count > 1)
            {
                MessageBox.Show("You may only have one selected event when opening the Link form!", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            new SearchForLinkForm(linkTo, GetItemsBasedOnSelection(listViewSmartScripts), listViewSmartScripts.SelectedItems[0].Index).Show(this);
        }

        protected override void WndProc(ref Message m)
        {
            //! Don't allow moving the window while we are expanding or contracting. This is required because
            //! the window often breaks and has an incorrect size in the end if the application had been moved
            //! while expanding or contracting.
            if (((m.Msg == 274 && m.WParam.ToInt32() == 61456) || (m.Msg == 161 && m.WParam.ToInt32() == 2)) && (expandingToMainForm || contractingToLoginForm))
                return;

            base.WndProc(ref m);
        }

        private void listViewSmartScripts_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                if (listViewSmartScripts.FocusedItem.Bounds.Contains(e.Location))
                    contextMenuStripListView.Show(Cursor.Position);
        }

        private void testToolStripMenuItemDeleteRow_Click(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
                foreach (ListViewItem item in listViewSmartScripts.SelectedItems)
                    listViewSmartScripts.Items.Remove(item);
        }

        private void ResetNonStaticFieldsToDefault()
        {
            comboBoxEventType.SelectedIndex = 0;
            comboBoxActionType.SelectedIndex = 0;
            comboBoxTargetType.SelectedIndex = 0;
            textBoxEventTypeId.Text = "0";
            textBoxActionTypeId.Text = "0";
            textBoxTargetTypeId.Text = "0";
            textBoxEventChance.Text = "100";
            textBoxEventScriptId.Text = "0";
            textBoxLinkFrom.Text = "0";
            textBoxLinkTo.Text = "0";
            textBoxComments.Text = "Npc - Event - Action (phase) (dungeon difficulty)";
            textBoxEventPhasemask.Text = "0";
            textBoxEventFlags.Text = "0";

            foreach (TabPage page in tabControlParameters.TabPages)
                foreach (Control control in page.Controls)
                    if (control is TextBox)
                        control.Text = "0";
        }

        private string GetSourceTypeString(SourceTypes sourceType)
        {
            switch (sourceType)
            {
                case SourceTypes.SourceTypeCreature:
                    return "creature";
                case SourceTypes.SourceTypeGameobject:
                    return "gameobject";
                case SourceTypes.SourceTypeAreaTrigger:
                    return "areatrigger";
                case SourceTypes.SourceTypeScriptedActionlist:
                    return "actionlist";
                default:
                    return "unknown";
            }
        }
    }
}
