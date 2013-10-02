using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using MySql.Data.MySqlClient;
using SAI_Editor.Properties;
using SAI_Editor.Database.Classes;
using SAI_Editor.SearchForms;
using SAI_Editor.Security;
using SAI_Editor.Classes;
using System.Threading.Tasks;
using SAI_Editor.Forms;

namespace SAI_Editor
{
    internal enum FormState
    {
        FormStateLogin,
        FormStateExpandingOrContracting,
        FormStateMain,
    }

    internal enum FormSizes
    {
        Width = 278,
        Height = 260,

        WidthToExpandTo = 957,
        HeightToExpandTo = 505,

        ListViewHeightContract = 57,
    }

    internal enum MaxValues
    {
        MaxEventType = 74,
        MaxActionType = 110,
        MaxTargetType = 26,
    }

    public enum SourceTypes
    {
        SourceTypeCreature = 0,
        SourceTypeGameobject = 1,
        SourceTypeAreaTrigger = 2,
        SourceTypeScriptedActionlist = 9,
    }

    public struct EntryOrGuidAndSourceType
    {
        public int entryOrGuid;
        public SourceTypes sourceType;
    };

    public partial class MainForm : Form
    {
        public MySqlConnectionStringBuilder connectionString = new MySqlConnectionStringBuilder();
        private readonly List<Control> controlsLoginForm = new List<Control>();
        private readonly List<Control> controlsMainForm = new List<Control>();
        private bool contractingToLoginForm , expandingToMainForm, expandingListView, contractingListView;
        private int originalHeight = 0, originalWidth = 0;
        private readonly Timer timerExpandOrContract = new Timer { Enabled = false, Interval = 4 };
        private readonly Timer timerShowPermanentTooltips = new Timer { Enabled = false, Interval = 4 };
        private int WidthToExpandTo = (int)FormSizes.WidthToExpandTo, HeightToExpandTo = (int)FormSizes.HeightToExpandTo;
        private int listViewSmartScriptsInitialHeight, listViewSmartScriptsHeightToChangeTo;
        public int expandAndContractSpeed = 5, expandAndContractSpeedListView = 2;
        private FormState formState = FormState.FormStateLogin;
        public EntryOrGuidAndSourceType originalEntryOrGuidAndSourceType = new EntryOrGuidAndSourceType();
        public int lastSmartScriptIdOfScript = 0;
        private readonly ListViewColumnSorter lvwColumnSorter = new ListViewColumnSorter();
        private bool runningConstructor = false;

        public MainForm()
        {
            InitializeComponent();
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            runningConstructor = true;
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
                textBoxPassword.Text = Settings.Default.Password.Length > 150 ? Settings.Default.Password.DecryptString(Encoding.Unicode.GetBytes(Settings.Default.Entropy)).ToInsecureString() : Settings.Default.Password;
                textBoxWorldDatabase.Text = Settings.Default.Database;
                textBoxPort.Text = Settings.Default.Port > 0 ? Settings.Default.Port.ToString() : String.Empty;
                expandAndContractSpeed = Settings.Default.AnimationSpeed;

                //! Handled when the initial animation finished
                //if (Settings.Default.ShowTooltipsPermanently)
                //    ExpandToShowPermanentTooltips(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            timerExpandOrContract.Tick += timerExpandOrContract_Tick;
            timerShowPermanentTooltips.Tick += timerShowPermanentTooltips_Tick;

            panelPermanentTooltipTypes.Visible = false;
            panelPermanentTooltipParameters.Visible = false;

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

            //! We first load the information and then change the parameter fields
            await SAI_Editor_Manager.Instance.LoadSQLiteDatabaseInfo();
            ChangeParameterFieldsBasedOnType();

            //! We hardcode the actual shortcuts because there are certain conditons under which the menu should not be
            //! opened at all.
            //menuItemExit.ShortcutKeys = (Keys.Alt | Keys.F4);
            menuItemExit.ShortcutKeyDisplayString = "(Alt + F4)";
            //menuItemReconnect.ShortcutKeys = (Keys.Shift | Keys.F4);
            menuItemReconnect.ShortcutKeyDisplayString = "(Shift + F4)";
            //menuItemSettings.ShortcutKeys = Keys.F1;
            menuItemSettings.ShortcutKeyDisplayString = "(F1)";
            //menuItemAbout.ShortcutKeys = (Keys.Alt | Keys.F1);
            menuItemAbout.ShortcutKeyDisplayString = "(Alt + F1)";
            //menuItemDeleteSelectedRow.ShortcutKeys = (Keys.Control | Keys.D);
            menuItemDeleteSelectedRow.ShortcutKeyDisplayString = "(Ctrl + D)";
            menuItemDeleteSelectedRowListView.ShortcutKeyDisplayString = "(Ctrl + D)";
            //menuItemGenerateSql.ShortcutKeys = (Keys.Control | Keys.M);
            menuItemGenerateSql.ShortcutKeyDisplayString = "(Ctrl + M)";

            listViewSmartScripts.Columns.Add("entryorguid", 67, HorizontalAlignment.Left);  // 0
            listViewSmartScripts.Columns.Add("source_type", 70, HorizontalAlignment.Right); // 1
            listViewSmartScripts.Columns.Add("id", 20, HorizontalAlignment.Right); // 2
            listViewSmartScripts.Columns.Add("link", 30, HorizontalAlignment.Right); // 3
            listViewSmartScripts.Columns.Add("event_type", 66, HorizontalAlignment.Right); // 4
            listViewSmartScripts.Columns.Add("event_phase", 74, HorizontalAlignment.Right); // 5
            listViewSmartScripts.Columns.Add("event_chance", 81, HorizontalAlignment.Right); // 6
            listViewSmartScripts.Columns.Add("event_flags", 69, HorizontalAlignment.Right); // 7
            listViewSmartScripts.Columns.Add("p1", 24, HorizontalAlignment.Right); // 8
            listViewSmartScripts.Columns.Add("p2", 24, HorizontalAlignment.Right); // 9
            listViewSmartScripts.Columns.Add("p3", 24, HorizontalAlignment.Right); // 10
            listViewSmartScripts.Columns.Add("p4", 24, HorizontalAlignment.Right); // 11
            listViewSmartScripts.Columns.Add("action_type", 67, HorizontalAlignment.Right); // 12
            listViewSmartScripts.Columns.Add("p1", 24, HorizontalAlignment.Right); // 13
            listViewSmartScripts.Columns.Add("p2", 24, HorizontalAlignment.Right); // 14
            listViewSmartScripts.Columns.Add("p3", 24, HorizontalAlignment.Right); // 15
            listViewSmartScripts.Columns.Add("p4", 24, HorizontalAlignment.Right); // 16
            listViewSmartScripts.Columns.Add("p5", 24, HorizontalAlignment.Right); // 17
            listViewSmartScripts.Columns.Add("p6", 24, HorizontalAlignment.Right); // 18
            listViewSmartScripts.Columns.Add("target_type", 67, HorizontalAlignment.Right); // 19
            listViewSmartScripts.Columns.Add("p1", 24, HorizontalAlignment.Right); // 20
            listViewSmartScripts.Columns.Add("p2", 24, HorizontalAlignment.Right); // 21
            listViewSmartScripts.Columns.Add("p3", 24, HorizontalAlignment.Right); // 22
            listViewSmartScripts.Columns.Add("x", 20, HorizontalAlignment.Right); // 23
            listViewSmartScripts.Columns.Add("y", 20, HorizontalAlignment.Right); // 24
            listViewSmartScripts.Columns.Add("z", 20, HorizontalAlignment.Right); // 25
            listViewSmartScripts.Columns.Add("o", 20, HorizontalAlignment.Right); // 26
            listViewSmartScripts.Columns.Add("comment", 400, HorizontalAlignment.Left); // 27 (width 56 to fit)

            if (Settings.Default.AutoConnect)
            {
                checkBoxAutoConnect.Checked = true;
                connectionString.Server = textBoxHost.Text;
                connectionString.UserID = textBoxUsername.Text;
                connectionString.Port = XConverter.TryParseStringToUInt32(textBoxPort.Text);
                connectionString.Database = textBoxWorldDatabase.Text;

                if (textBoxPassword.Text.Length > 0)
                    connectionString.Password = textBoxPassword.Text;

                if (SAI_Editor_Manager.Instance.worldDatabase.CanConnectToDatabase(connectionString, false))
                {
                    buttonConnect.PerformClick();

                    if (Settings.Default.InstantExpand)
                        StartExpandingToMainForm(true);
                }
            }

            tabControlParameters.AutoScrollOffset = new Point(5, 5);

            //! Permanent scrollbar to the parameters tabpage windows
            foreach (TabPage page in tabControlParameters.TabPages)
            {
                page.HorizontalScroll.Enabled = false;
                page.HorizontalScroll.Visible = false;

                page.AutoScroll = true;
                page.AutoScrollMinSize = new Size(page.Width, page.Height);

                //foreach (Control control in page.Controls)
                //{
                //    if (control is Label)
                //    {
                //        switch (page.TabIndex)
                //        {
                //            case 0: //! Events
                //                control.MouseEnter += labelsEventParameters_MouseEnter;
                //                break;
                //            case 1: //! Actions
                //                control.MouseEnter += labelsActionsParameters_MouseEnter;
                //                break;
                //            case 2: //! Targets
                //                control.MouseEnter += labelsTargetsParameters_MouseEnter;
                //                break;
                //        }
                //    }
                //}
            }

            //! Temp..
            panelLoginBox.Location = new Point(9, 8);

            if (Settings.Default.HidePass)
                textBoxPassword.PasswordChar = '●';

            textBoxComments.GotFocus += textBoxComments_GotFocus;
            textBoxComments.LostFocus += textBoxComments_LostFocus;

            panelPermanentTooltipTypes.BackColor = Color.FromArgb(255, 255, 225);
            panelPermanentTooltipParameters.BackColor = Color.FromArgb(255, 255, 225);
            labelPermanentTooltipTextTypes.BackColor = Color.FromArgb(255, 255, 225);

            //! Set them to invisible by default; they become visible when the timer finished
            panelPermanentTooltipTypes.Visible = false;
            panelPermanentTooltipParameters.Visible = false;

            pictureBoxLoadScript.Enabled = textBoxEntryOrGuid.Text.Length > 0;

            runningConstructor = false;
        }

        private void timerExpandOrContract_Tick(object sender, EventArgs e)
        {
            if (expandingToMainForm)
            {
                if (Height < HeightToExpandTo)
                    Height += expandAndContractSpeed;
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
                    Width += expandAndContractSpeed;
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
                    Height -= expandAndContractSpeed;
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
                    Width -= expandAndContractSpeed;
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
            connectionString.Port = XConverter.TryParseStringToUInt32(textBoxPort.Text);
            connectionString.Database = textBoxWorldDatabase.Text;

            if (textBoxPassword.Text.Length > 0)
                connectionString.Password = textBoxPassword.Text;

            if (SAI_Editor_Manager.Instance.worldDatabase.CanConnectToDatabase(connectionString))
            {
                StartExpandingToMainForm(Settings.Default.InstantExpand);
                SAI_Editor_Manager.Instance.ResetDatabases();
            }
        }

        private void StartExpandingToMainForm(bool instant = false)
        {
            if (checkBoxSaveSettings.Checked)
            {
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] buffer = new byte[1024];
                rng.GetBytes(buffer);
                string salt = BitConverter.ToString(buffer);
                rng.Dispose();

                Settings.Default.Entropy = salt;
                Settings.Default.Host = textBoxHost.Text;
                Settings.Default.User = textBoxUsername.Text;
                Settings.Default.Password = textBoxPassword.Text.ToSecureString().EncryptString(Encoding.Unicode.GetBytes(salt));
                Settings.Default.Database = textBoxWorldDatabase.Text;
                Settings.Default.AutoConnect = checkBoxAutoConnect.Checked;
                Settings.Default.Port = XConverter.TryParseStringToUInt32(textBoxPort.Text);
                Settings.Default.Save();
            }

            ResetFieldsToDefault();
            Text = "SAI-Editor - Connection: " + textBoxUsername.Text + ", " + textBoxHost.Text + ", " + textBoxPort.Text;

            if (instant)
            {
                Width = WidthToExpandTo;
                Height = HeightToExpandTo;
                formState = FormState.FormStateMain;

                if (Settings.Default.ShowTooltipsPermanently)
                    ExpandToShowPermanentTooltips(false);
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

            panelPermanentTooltipTypes.Visible = false;
            panelPermanentTooltipParameters.Visible = false;
        }

        private void StartContractingToLoginForm(bool instant = false)
        {
            Text = "SAI-Editor: Login";

            if (Settings.Default.ShowTooltipsPermanently)
                listViewSmartScripts.Height += (int)FormSizes.ListViewHeightContract;

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

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    TryCloseApplication();
                    break;
                case Keys.Enter:
                    switch (formState)
                    {
                        case FormState.FormStateLogin:
                            buttonConnect.PerformClick();
                            break;
                        case FormState.FormStateMain:
                            if (textBoxEntryOrGuid.Focused)
                                pictureBoxLoadScript_Click(pictureBoxLoadScript, null);

                            break;
                    }
                    break;
            }

            //! Hardcode shortcuts to menu because we can't use conditions otherwise
            if (formState == FormState.FormStateMain)
            {
                if (e.KeyData == (Keys.Alt | Keys.F4))
                    TryCloseApplication();
                else if (e.KeyData == (Keys.Shift | Keys.F4) || e.KeyData == (Keys.ShiftKey | Keys.F4))
                    menuItemReconnect.PerformClick();
                else if (e.KeyData == Keys.F1)
                    menuItemSettings.PerformClick();
                else if (e.KeyData == (Keys.Alt | Keys.F1))
                    menuItemAbout.PerformClick();
                else if (e.KeyData == (Keys.Control | Keys.D) || e.KeyData == (Keys.ControlKey | Keys.D))
                {
                    if (menuItemDeleteSelectedRow.Enabled)
                        menuItemDeleteSelectedRow.PerformClick();
                }
                else if (e.KeyData == (Keys.Control | Keys.M) || e.KeyData == (Keys.ControlKey | Keys.M))
                {
                    if (menuItemGenerateSql.Enabled)
                        menuItemGenerateSql.PerformClick();
                }
            }
        }

        private void buttonSearchForEntry_Click(object sender, EventArgs e)
        {
            //! Just keep it in main thread; no purpose starting a new thread for this (unless workspaces get implemented, maybe)
            using (var entryForm = new SearchForEntryForm(connectionString, textBoxEntryOrGuid.Text, GetSourceTypeByIndex()))
                entryForm.ShowDialog(this);
        }

        private void menuItemReconnect_Click(object sender, EventArgs e)
        {
            panelPermanentTooltipTypes.Visible = false;
            panelPermanentTooltipParameters.Visible = false;
            ResetFieldsToDefault(true);
            listViewSmartScripts.Items.Clear();
            StartContractingToLoginForm(Settings.Default.InstantExpand);
        }

        private void comboBoxEventType_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxEventTypeId.Text = comboBoxEventType.SelectedIndex.ToString();
            textBoxEventTypeId.SelectionStart = 3; //! Set cursot to end of text

            if (!runningConstructor)
            {
                ChangeParameterFieldsBasedOnType();
                UpdatePermanentTooltipOfTypes(comboBoxEventType, ScriptTypeId.ScriptTypeEvent);
            }

            if (listViewSmartScripts.SelectedItems.Count > 0)
                listViewSmartScripts.SelectedItems[0].SubItems[4].Text = comboBoxEventType.SelectedIndex.ToString();
        }

        private void comboBoxActionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxActionTypeId.Text = comboBoxActionType.SelectedIndex.ToString();
            textBoxActionTypeId.SelectionStart = 3; //! Set cursot to end of text

            if (!runningConstructor)
            {
                ChangeParameterFieldsBasedOnType();
                UpdatePermanentTooltipOfTypes(comboBoxActionType, ScriptTypeId.ScriptTypeAction);
            }

            if (listViewSmartScripts.SelectedItems.Count > 0)
                listViewSmartScripts.SelectedItems[0].SubItems[12].Text = comboBoxActionType.SelectedIndex.ToString();
        }

        private void comboBoxTargetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxTargetTypeId.Text = comboBoxTargetType.SelectedIndex.ToString();
            textBoxTargetTypeId.SelectionStart = 3; //! Set cursot to end of text

            if (!runningConstructor)
            {
                ChangeParameterFieldsBasedOnType();
                UpdatePermanentTooltipOfTypes(comboBoxTargetType, ScriptTypeId.ScriptTypeTarget);
            }

            if (listViewSmartScripts.SelectedItems.Count > 0)
                listViewSmartScripts.SelectedItems[0].SubItems[19].Text = comboBoxTargetType.SelectedIndex.ToString();
        }

        private void ChangeParameterFieldsBasedOnType()
        {
            //! Event parameters
            int event_type = comboBoxEventType.SelectedIndex;
            labelEventParam1.Text = SAI_Editor_Manager.Instance.GetParameterStringById(event_type, 1, ScriptTypeId.ScriptTypeEvent);
            labelEventParam2.Text = SAI_Editor_Manager.Instance.GetParameterStringById(event_type, 2, ScriptTypeId.ScriptTypeEvent);
            labelEventParam3.Text = SAI_Editor_Manager.Instance.GetParameterStringById(event_type, 3, ScriptTypeId.ScriptTypeEvent);
            labelEventParam4.Text = SAI_Editor_Manager.Instance.GetParameterStringById(event_type, 4, ScriptTypeId.ScriptTypeEvent);

            if (!Settings.Default.ShowTooltipsPermanently)
            {
                AddTooltip(comboBoxEventType, comboBoxEventType.SelectedItem.ToString(), SAI_Editor_Manager.Instance.GetScriptTypeTooltipById(event_type, ScriptTypeId.ScriptTypeEvent));
                AddTooltip(labelEventParam1, labelEventParam1.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(event_type, 1, ScriptTypeId.ScriptTypeEvent));
                AddTooltip(labelEventParam2, labelEventParam2.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(event_type, 2, ScriptTypeId.ScriptTypeEvent));
                AddTooltip(labelEventParam3, labelEventParam3.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(event_type, 3, ScriptTypeId.ScriptTypeEvent));
                AddTooltip(labelEventParam4, labelEventParam4.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(event_type, 4, ScriptTypeId.ScriptTypeEvent));
            }

            //! Action parameters
            int action_type = comboBoxActionType.SelectedIndex;
            labelActionParam1.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 1, ScriptTypeId.ScriptTypeAction);
            labelActionParam2.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 2, ScriptTypeId.ScriptTypeAction);
            labelActionParam3.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 3, ScriptTypeId.ScriptTypeAction);
            labelActionParam4.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 4, ScriptTypeId.ScriptTypeAction);
            labelActionParam5.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 5, ScriptTypeId.ScriptTypeAction);
            labelActionParam6.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 6, ScriptTypeId.ScriptTypeAction);

            if (!Settings.Default.ShowTooltipsPermanently)
            {
                AddTooltip(comboBoxActionType, comboBoxActionType.SelectedItem.ToString(), SAI_Editor_Manager.Instance.GetScriptTypeTooltipById(action_type, ScriptTypeId.ScriptTypeAction));
                AddTooltip(labelActionParam1, labelActionParam1.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 1, ScriptTypeId.ScriptTypeAction));
                AddTooltip(labelActionParam2, labelActionParam2.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 2, ScriptTypeId.ScriptTypeAction));
                AddTooltip(labelActionParam3, labelActionParam3.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 3, ScriptTypeId.ScriptTypeAction));
                AddTooltip(labelActionParam4, labelActionParam4.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 4, ScriptTypeId.ScriptTypeAction));
                AddTooltip(labelActionParam5, labelActionParam5.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 5, ScriptTypeId.ScriptTypeAction));
                AddTooltip(labelActionParam6, labelActionParam6.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 6, ScriptTypeId.ScriptTypeAction));
            }

            //! Target parameters
            int target_type = comboBoxTargetType.SelectedIndex;
            labelTargetParam1.Text = SAI_Editor_Manager.Instance.GetParameterStringById(target_type, 1, ScriptTypeId.ScriptTypeTarget);
            labelTargetParam2.Text = SAI_Editor_Manager.Instance.GetParameterStringById(target_type, 2, ScriptTypeId.ScriptTypeTarget);
            labelTargetParam3.Text = SAI_Editor_Manager.Instance.GetParameterStringById(target_type, 3, ScriptTypeId.ScriptTypeTarget);

            if (!Settings.Default.ShowTooltipsPermanently)
            {
                AddTooltip(comboBoxTargetType, comboBoxTargetType.SelectedItem.ToString(), SAI_Editor_Manager.Instance.GetScriptTypeTooltipById(target_type, ScriptTypeId.ScriptTypeTarget));
                AddTooltip(labelTargetParam1, labelTargetParam1.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(target_type, 1, ScriptTypeId.ScriptTypeTarget));
                AddTooltip(labelTargetParam2, labelTargetParam2.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(target_type, 2, ScriptTypeId.ScriptTypeTarget));
                AddTooltip(labelTargetParam3, labelTargetParam3.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(target_type, 3, ScriptTypeId.ScriptTypeTarget));
            }

            AdjustAllParameterFields(event_type, action_type, target_type);
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

            panelPermanentTooltipTypes.Visible = false;
            panelPermanentTooltipParameters.Visible = false;

            if (expanding && Settings.Default.ShowTooltipsPermanently)
                ExpandToShowPermanentTooltips(false);
        }

        private async Task<bool> SelectAndFillListViewByEntryAndSource(string entryOrGuid, SourceTypes sourceType)
        {
            try
            {
                List<SmartScript> smartScripts = await SAI_Editor_Manager.Instance.worldDatabase.GetSmartScripts(XConverter.TryParseStringToInt32(entryOrGuid), (int)sourceType);

                if (smartScripts == null)
                {
                    MessageBox.Show(String.Format("The entryorguid '{0}' could not be found in the SmartAI (smart_scripts) table for the given source type ({1})!", entryOrGuid, GetSourceTypeString(sourceType)), "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    pictureBoxLoadScript.Enabled = true;
                    return false;
                }

                foreach (SmartScript smartScript in smartScripts)
                {
                    ListViewItem listViewItem = new ListViewItem();
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

                    listViewSmartScripts.Items.Add(listViewItem);

                    if (checkBoxListActionlistsOrEntries.Checked && sourceType == originalEntryOrGuidAndSourceType.sourceType)
                    {
                        TimedActionListOrEntries timedActionListOrEntries = await SAI_Editor_Manager.Instance.GetTimedActionlistsOrEntries(smartScript, sourceType);

                        foreach (string scriptEntry in timedActionListOrEntries.entries)
                            await SelectAndFillListViewByEntryAndSource(scriptEntry, timedActionListOrEntries.sourceTypeOfEntry);
                    }
                }

                //! This causes them to be resized to the item in the column with the highest width value.
                for (int i = 0; i <= 11; ++i)
                    listViewSmartScripts.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);

                for (int i = 13; i <= 18; ++i)
                    listViewSmartScripts.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);

                for (int i = 20; i <= 26; ++i)
                    listViewSmartScripts.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);

                listViewSmartScripts.Columns[27].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize); //! Comment column
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pictureBoxLoadScript.Enabled = true;
                return false;
            }

            pictureBoxLoadScript.Enabled = true;
            return true;
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

        private void listViewSmartScripts_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            menuItemDeleteSelectedRow.Enabled = listViewSmartScripts.SelectedItems.Count > 0;
            menuItemGenerateSql.Enabled = listViewSmartScripts.SelectedItems.Count > 0;
            buttonGenerateSql.Enabled = listViewSmartScripts.SelectedItems.Count > 0;

            if (!e.IsSelected)
                return;

            FillFieldsBasedOnSelectedScript();
        }

        private void FillFieldsBasedOnSelectedScript()
        {
            try
            {
                ListViewItem.ListViewSubItemCollection selectedItem = listViewSmartScripts.SelectedItems[0].SubItems;

                if (Settings.Default.ChangeStaticInfo)
                {
                    textBoxEntryOrGuid.Text = selectedItem[0].Text;

                    switch (XConverter.TryParseStringToInt32(selectedItem[1].Text))
                    {
                        case 0: //! Creature
                        case 1: //! Gameobject
                        case 2: //! Areatrigger
                            comboBoxSourceType.SelectedIndex = XConverter.TryParseStringToInt32(selectedItem[1].Text);
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

                int event_type = XConverter.TryParseStringToInt32(selectedItem[4].Text);
                comboBoxEventType.SelectedIndex = event_type;
                textBoxEventPhasemask.Text = selectedItem[5].Text;
                textBoxEventChance.Text = selectedItem[6].Text;
                textBoxEventFlags.Text = selectedItem[7].Text;

                //! Event parameters
                textBoxEventParam1.Text = selectedItem[8].Text;
                textBoxEventParam2.Text = selectedItem[9].Text;
                textBoxEventParam3.Text = selectedItem[10].Text;
                textBoxEventParam4.Text = selectedItem[11].Text;
                labelEventParam1.Text = SAI_Editor_Manager.Instance.GetParameterStringById(event_type, 1, ScriptTypeId.ScriptTypeEvent);
                labelEventParam2.Text = SAI_Editor_Manager.Instance.GetParameterStringById(event_type, 2, ScriptTypeId.ScriptTypeEvent);
                labelEventParam3.Text = SAI_Editor_Manager.Instance.GetParameterStringById(event_type, 3, ScriptTypeId.ScriptTypeEvent);
                labelEventParam4.Text = SAI_Editor_Manager.Instance.GetParameterStringById(event_type, 4, ScriptTypeId.ScriptTypeEvent);

                if (!Settings.Default.ShowTooltipsPermanently)
                {
                    AddTooltip(comboBoxEventType, comboBoxEventType.SelectedItem.ToString(), SAI_Editor_Manager.Instance.GetScriptTypeTooltipById(event_type, ScriptTypeId.ScriptTypeEvent));
                    AddTooltip(labelEventParam1, labelEventParam1.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(event_type, 1, ScriptTypeId.ScriptTypeEvent));
                    AddTooltip(labelEventParam2, labelEventParam2.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(event_type, 2, ScriptTypeId.ScriptTypeEvent));
                    AddTooltip(labelEventParam3, labelEventParam3.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(event_type, 3, ScriptTypeId.ScriptTypeEvent));
                    AddTooltip(labelEventParam4, labelEventParam4.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(event_type, 4, ScriptTypeId.ScriptTypeEvent));
                }

                //! Action parameters
                int action_type = XConverter.TryParseStringToInt32(selectedItem[12].Text);
                comboBoxActionType.SelectedIndex = action_type;
                textBoxActionParam1.Text = selectedItem[13].Text;
                textBoxActionParam2.Text = selectedItem[14].Text;
                textBoxActionParam3.Text = selectedItem[15].Text;
                textBoxActionParam4.Text = selectedItem[16].Text;
                textBoxActionParam5.Text = selectedItem[17].Text;
                textBoxActionParam6.Text = selectedItem[18].Text;
                labelActionParam1.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 1, ScriptTypeId.ScriptTypeAction);
                labelActionParam2.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 2, ScriptTypeId.ScriptTypeAction);
                labelActionParam3.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 3, ScriptTypeId.ScriptTypeAction);
                labelActionParam4.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 4, ScriptTypeId.ScriptTypeAction);
                labelActionParam5.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 5, ScriptTypeId.ScriptTypeAction);
                labelActionParam6.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 6, ScriptTypeId.ScriptTypeAction);

                if (!Settings.Default.ShowTooltipsPermanently)
                {
                    AddTooltip(comboBoxActionType, comboBoxActionType.SelectedItem.ToString(), SAI_Editor_Manager.Instance.GetScriptTypeTooltipById(action_type, ScriptTypeId.ScriptTypeAction));
                    AddTooltip(labelActionParam1, labelActionParam1.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 1, ScriptTypeId.ScriptTypeAction));
                    AddTooltip(labelActionParam2, labelActionParam2.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 2, ScriptTypeId.ScriptTypeAction));
                    AddTooltip(labelActionParam3, labelActionParam3.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 3, ScriptTypeId.ScriptTypeAction));
                    AddTooltip(labelActionParam4, labelActionParam4.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 4, ScriptTypeId.ScriptTypeAction));
                    AddTooltip(labelActionParam5, labelActionParam5.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 5, ScriptTypeId.ScriptTypeAction));
                    AddTooltip(labelActionParam6, labelActionParam6.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 6, ScriptTypeId.ScriptTypeAction));
                }

                //! Target parameters
                int target_type = XConverter.TryParseStringToInt32(selectedItem[19].Text);
                comboBoxTargetType.SelectedIndex = target_type;
                textBoxTargetParam1.Text = selectedItem[20].Text;
                textBoxTargetParam2.Text = selectedItem[21].Text;
                textBoxTargetParam3.Text = selectedItem[22].Text;
                labelTargetParam1.Text = SAI_Editor_Manager.Instance.GetParameterStringById(target_type, 1, ScriptTypeId.ScriptTypeTarget);
                labelTargetParam2.Text = SAI_Editor_Manager.Instance.GetParameterStringById(target_type, 2, ScriptTypeId.ScriptTypeTarget);
                labelTargetParam3.Text = SAI_Editor_Manager.Instance.GetParameterStringById(target_type, 3, ScriptTypeId.ScriptTypeTarget);

                if (!Settings.Default.ShowTooltipsPermanently)
                {
                    AddTooltip(comboBoxTargetType, comboBoxTargetType.SelectedItem.ToString(), SAI_Editor_Manager.Instance.GetScriptTypeTooltipById(target_type, ScriptTypeId.ScriptTypeTarget));
                    AddTooltip(labelTargetParam1, labelTargetParam1.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(target_type, 1, ScriptTypeId.ScriptTypeTarget));
                    AddTooltip(labelTargetParam2, labelTargetParam2.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(target_type, 2, ScriptTypeId.ScriptTypeTarget));
                    AddTooltip(labelTargetParam3, labelTargetParam3.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(target_type, 3, ScriptTypeId.ScriptTypeTarget));
                }

                textBoxTargetX.Text = selectedItem[23].Text;
                textBoxTargetY.Text = selectedItem[24].Text;
                textBoxTargetZ.Text = selectedItem[25].Text;
                textBoxTargetO.Text = selectedItem[26].Text;
                textBoxComments.Text = selectedItem[27].Text;

                AdjustAllParameterFields(event_type, action_type, target_type);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AdjustAllParameterFields(int event_type, int action_type, int target_type)
        {
            SetVisibilityOfAllParamButtons(false);

            switch ((SmartEvent)event_type)
            {
                case SmartEvent.SMART_EVENT_SPELLHIT:
                    buttonEventParamOneSearch.Visible = true; //! Spell entry
                    buttonEventParamTwoSearch.Visible = true; //! Spell school
                    break;
                case SmartEvent.SMART_EVENT_RESPAWN:
                    buttonEventParamOneSearch.Visible = true; //! Respawn condition (SMART_SCRIPT_RESPAWN_CONDITION_MAP / SMART_SCRIPT_RESPAWN_CONDITION_AREA)
                    buttonEventParamTwoSearch.Visible = true; //! Map entry
                    buttonEventParamThreeSearch.Visible = true; //! Zone entry
                    break;
                case SmartEvent.SMART_EVENT_AREATRIGGER_ONTRIGGER: //! Areatrigger entry
                case SmartEvent.SMART_EVENT_GO_STATE_CHANGED: //! Go state
                case SmartEvent.SMART_EVENT_GAME_EVENT_START: //! Game event entry
                case SmartEvent.SMART_EVENT_GAME_EVENT_END: //! Game event entry
                case SmartEvent.SMART_EVENT_MOVEMENTINFORM: //! Movement type
                case SmartEvent.SMART_EVENT_FRIENDLY_MISSING_BUFF:
                case SmartEvent.SMART_EVENT_HAS_AURA:
                case SmartEvent.SMART_EVENT_TARGET_BUFFED:
                case SmartEvent.SMART_EVENT_SPELLHIT_TARGET:
                case SmartEvent.SMART_EVENT_SUMMON_DESPAWNED: //! Creature entry
                    buttonEventParamOneSearch.Visible = true; //! Spell entry
                    break;
            }

            switch ((SmartAction)action_type)
            {
                case SmartAction.SMART_ACTION_CAST:
                case SmartAction.SMART_ACTION_INVOKER_CAST:
                    buttonActionParamOneSearch.Visible = true; //! Spell entry
                    buttonActionParamTwoSearch.Visible = true; //! Cast flags
                    break;
                case SmartAction.SMART_ACTION_CROSS_CAST:
                    buttonActionParamOneSearch.Visible = true; //! Spell entry
                    buttonActionParamTwoSearch.Visible = true; //! Cast flags
                    buttonActionParamThreeSearch.Visible = true; //! Target type
                    break;
                case SmartAction.SMART_ACTION_SUMMON_CREATURE:
                    buttonActionParamOneSearch.Visible = true; //! Creature entry
                    buttonActionParamTwoSearch.Visible = true; //! Summon type
                    break;
                case SmartAction.SMART_ACTION_WP_STOP: //! Quest entry
                case SmartAction.SMART_ACTION_INTERRUPT_SPELL: //! Spell entry
                    buttonActionParamTwoSearch.Visible = true;
                    break;
                case SmartAction.SMART_ACTION_WP_START:
                    buttonActionParamFourSearch.Visible = true; //! Quest entry
                    buttonActionParamSixSearch.Visible = true; //! React state
                    break;
                case SmartAction.SMART_ACTION_FOLLOW:
                    buttonActionParamThreeSearch.Visible = true; //! Creature entry
                    break;
                case SmartAction.SMART_ACTION_RANDOM_PHASE_RANGE:
                    buttonActionParamOneSearch.Visible = true; //! Event phase 1
                    buttonActionParamTwoSearch.Visible = true; //! Event phase 2
                    break;
                case SmartAction.SMART_ACTION_RANDOM_PHASE:
                    buttonActionParamOneSearch.Visible = true; //! Event phase 1
                    buttonActionParamTwoSearch.Visible = true; //! Event phase 2
                    buttonActionParamThreeSearch.Visible = true; //! Event phase 3
                    buttonActionParamFourSearch.Visible = true; //! Event phase 4
                    buttonActionParamFiveSearch.Visible = true; //! Event phase 5
                    buttonActionParamSixSearch.Visible = true; //! Event phase 6
                    break;
                case SmartAction.SMART_ACTION_EQUIP:
                    buttonActionParamOneSearch.Visible = true; //! Equipment entry
                    buttonActionParamThreeSearch.Visible = true; //! Item entry 1
                    buttonActionParamFourSearch.Visible = true; //! Item entry 2
                    buttonActionParamFiveSearch.Visible = true; //! Item entry 3
                    break;
                case SmartAction.SMART_ACTION_SET_FACTION: //! Faction entry
                case SmartAction.SMART_ACTION_EMOTE: //! Emote entry
                case SmartAction.SMART_ACTION_RANDOM_EMOTE: //! Emote entry
                case SmartAction.SMART_ACTION_SET_EMOTE_STATE: //! Emote entry
                case SmartAction.SMART_ACTION_FAIL_QUEST: //! Quest entry
                case SmartAction.SMART_ACTION_ADD_QUEST: //! Quest entry
                case SmartAction.SMART_ACTION_CALL_AREAEXPLOREDOREVENTHAPPENS: //! Quest entry
                case SmartAction.SMART_ACTION_CALL_GROUPEVENTHAPPENS: //! Quest entry
                case SmartAction.SMART_ACTION_SET_REACT_STATE: //! Reactstate
                case SmartAction.SMART_ACTION_SOUND: //! Sound entry
                case SmartAction.SMART_ACTION_MORPH_TO_ENTRY_OR_MODEL: //! Creature entry
                case SmartAction.SMART_ACTION_CALL_CASTEDCREATUREORGO: //! Creature entry
                case SmartAction.SMART_ACTION_KILLED_MONSTER: //! Creature entry
                case SmartAction.SMART_ACTION_UPDATE_TEMPLATE: //! Creature entry
                case SmartAction.SMART_ACTION_MOUNT_TO_ENTRY_OR_MODEL: //! Creature entry
                case SmartAction.SMART_ACTION_GO_SET_LOOT_STATE: //! Gameobject state
                case SmartAction.SMART_ACTION_SET_POWER: //! Power type
                case SmartAction.SMART_ACTION_ADD_POWER: //! Power type
                case SmartAction.SMART_ACTION_REMOVE_POWER: //! Power type
                case SmartAction.SMART_ACTION_SUMMON_GO: //! Gameobject entry
                case SmartAction.SMART_ACTION_SET_EVENT_PHASE: //! Event/ingame phase
                case SmartAction.SMART_ACTION_SET_PHASE_MASK: //! Event/ingame phase
                case SmartAction.SMART_ACTION_ADD_ITEM: //! Item entry
                case SmartAction.SMART_ACTION_REMOVE_ITEM: //! Item entry
                case SmartAction.SMART_ACTION_TELEPORT: //! Map id
                case SmartAction.SMART_ACTION_SUMMON_CREATURE_GROUP: //! Summons group id
                case SmartAction.SMART_ACTION_REMOVEAURASFROMSPELL: //! Spell id
                case SmartAction.SMART_ACTION_SET_SHEATH: //! Sheath state
                case SmartAction.SMART_ACTION_ACTIVATE_TAXI: //! Taxi path id
                case SmartAction.SMART_ACTION_SET_UNIT_FLAG: //! Unit flags
                case SmartAction.SMART_ACTION_REMOVE_UNIT_FLAG: //! Unit flags
                case SmartAction.SMART_ACTION_SET_GO_FLAG: //! Gameobject flags
                case SmartAction.SMART_ACTION_ADD_GO_FLAG: //! Gameobject flags
                case SmartAction.SMART_ACTION_REMOVE_GO_FLAG: //! Gameobject flags
                case SmartAction.SMART_ACTION_SET_DYNAMIC_FLAG: //! Dynamic flags
                case SmartAction.SMART_ACTION_ADD_DYNAMIC_FLAG: //! Dynamic flags
                case SmartAction.SMART_ACTION_REMOVE_DYNAMIC_FLAG: //! Dynamic flags
                case SmartAction.SMART_ACTION_ADD_AURA: //! Spell id
                    buttonActionParamOneSearch.Visible = true;
                    break;
            }

            switch ((SmartTarget)target_type)
            {
                case SmartTarget.SMART_TARGET_CREATURE_GUID:
                    buttonTargetParamOneSearch.Visible = true; //! Creature guid
                    buttonTargetParamTwoSearch.Visible = true; //! Creature entry
                    break;
                case SmartTarget.SMART_TARGET_GAMEOBJECT_GUID:
                    buttonTargetParamOneSearch.Visible = true; //! Gameobject guid
                    buttonTargetParamTwoSearch.Visible = true; //! Gameobject entry
                    break;
                case SmartTarget.SMART_TARGET_CREATURE_RANGE: //! Creature entry
                case SmartTarget.SMART_TARGET_CREATURE_DISTANCE: //! Creature entry
                case SmartTarget.SMART_TARGET_CLOSEST_CREATURE: //! Creature entry
                case SmartTarget.SMART_TARGET_GAMEOBJECT_RANGE: //! Gameobject entry
                case SmartTarget.SMART_TARGET_GAMEOBJECT_DISTANCE: //! Gameobject entry
                case SmartTarget.SMART_TARGET_CLOSEST_GAMEOBJECT: //! Gameobject entry
                    buttonTargetParamOneSearch.Visible = true;
                    break;
            }

            textBoxEventParam1.Enabled = labelEventParam1.Text.Length > 0;
            textBoxEventParam2.Enabled = labelEventParam2.Text.Length > 0;
            textBoxEventParam3.Enabled = labelEventParam3.Text.Length > 0;
            textBoxEventParam4.Enabled = labelEventParam4.Text.Length > 0;

            textBoxActionParam1.Enabled = labelActionParam1.Text.Length > 0;
            textBoxActionParam2.Enabled = labelActionParam2.Text.Length > 0;
            textBoxActionParam3.Enabled = labelActionParam3.Text.Length > 0;
            textBoxActionParam4.Enabled = labelActionParam4.Text.Length > 0;
            textBoxActionParam5.Enabled = labelActionParam5.Text.Length > 0;
            textBoxActionParam6.Enabled = labelActionParam6.Text.Length > 0;

            textBoxTargetParam1.Enabled = labelTargetParam1.Text.Length > 0;
            textBoxTargetParam2.Enabled = labelTargetParam2.Text.Length > 0;
            textBoxTargetParam3.Enabled = labelTargetParam3.Text.Length > 0;

            textBoxTargetX.Enabled = (SmartTarget)target_type == SmartTarget.SMART_TARGET_POSITION;
            textBoxTargetY.Enabled = (SmartTarget)target_type == SmartTarget.SMART_TARGET_POSITION;
            textBoxTargetZ.Enabled = (SmartTarget)target_type == SmartTarget.SMART_TARGET_POSITION;
            textBoxTargetO.Enabled = (SmartTarget)target_type == SmartTarget.SMART_TARGET_POSITION;
        }

        private void AddTooltip(Control control, string title, string text, ToolTipIcon icon = ToolTipIcon.Info, bool isBallon = true, bool active = true, int autoPopDelay = 2100000000, bool showAlways = true)
        {
            if (String.IsNullOrWhiteSpace(title) || String.IsNullOrWhiteSpace(text))
            {
                XToolTip toolTipExistent = ToolTipHelper.GetExistingToolTip(control);

                if (toolTipExistent != null)
                    toolTipExistent.Active = false;

                return;
            }

            XToolTip toolTip = ToolTipHelper.GetControlToolTip(control);
            toolTip.ToolTipIcon = icon;
            toolTip.ToolTipTitle = title;
            toolTip.IsBalloon = isBallon;
            toolTip.Active = active;
            toolTip.AutoPopDelay = autoPopDelay;
            toolTip.ShowAlways = showAlways;
            toolTip.SetToolTipText(control, text);
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
            else
            {
                int eventType;
                Int32.TryParse(textBoxEventTypeId.Text, out eventType);

                if (eventType > (int)MaxValues.MaxEventType)
                {
                    comboBoxEventType.SelectedIndex = (int)MaxValues.MaxEventType;
                    textBoxEventTypeId.Text = ((int)MaxValues.MaxEventType).ToString();
                    textBoxEventTypeId.SelectionStart = 3; //! Set cursor position to end of the line
                }
                else
                    comboBoxEventType.SelectedIndex = eventType;
            }
        }

        private void textBoxActionTypeId_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxActionTypeId.Text))
            {
                comboBoxActionType.SelectedIndex = 0;
                textBoxActionTypeId.Text = "0";
                textBoxActionTypeId.SelectionStart = 3; //! Set cursor position to end of the line
            }
            else
            {
                int actionType;
                Int32.TryParse(textBoxActionTypeId.Text, out actionType);

                if (actionType > (int)MaxValues.MaxActionType)
                {
                    comboBoxActionType.SelectedIndex = (int)MaxValues.MaxActionType;
                    textBoxActionTypeId.Text = ((int)MaxValues.MaxActionType).ToString();
                    textBoxActionTypeId.SelectionStart = 3; //! Set cursor position to end of the line
                }
                else
                    comboBoxActionType.SelectedIndex = actionType;
            }
        }

        private void textBoxTargetTypeId_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxTargetTypeId.Text))
            {
                comboBoxTargetType.SelectedIndex = 0;
                textBoxTargetTypeId.Text = "0";
                textBoxTargetTypeId.SelectionStart = 3; //! Set cursor position to end of the line
            }
            else
            {
                int targetType;
                Int32.TryParse(textBoxTargetTypeId.Text, out targetType);

                if (targetType > (int)MaxValues.MaxTargetType)
                {
                    comboBoxTargetType.SelectedIndex = (int)MaxValues.MaxTargetType;
                    textBoxTargetTypeId.Text = ((int)MaxValues.MaxTargetType).ToString();
                    textBoxTargetTypeId.SelectionStart = 3; //! Set cursor position to end of the line
                }
                else
                    comboBoxTargetType.SelectedIndex = targetType;
            }
        }

        private void menuOptionDeleteSelectedRow_Click(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count <= 0)
            {
                MessageBox.Show("No rows were selected to delete!", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DeleteSelectedRow();
        }

        private void DeleteSelectedRow()
        {
            if (listViewSmartScripts.SelectedItems.Count == 0)
                return;

            listViewSmartScripts.Items.Remove(listViewSmartScripts.SelectedItems[0]);
            buttonNewLine.Enabled = listViewSmartScripts.Items.Count > 0;
            lastSmartScriptIdOfScript--;

            if (listViewSmartScripts.Items.Count <= 0)
                ResetFieldsToDefault(true);
            else
                listViewSmartScripts.Items[0].Selected = true;
        }

        private async void checkBoxListActionlists_CheckedChanged(object sender, EventArgs e)
        {
            if (listViewSmartScripts.Items.Count == 0)
                return;

            buttonGenerateSql.Enabled = false;
            menuItemGenerateSql.Enabled = false;

            if (checkBoxListActionlistsOrEntries.Checked)
            {
                listViewSmartScripts.Items.Clear();
                await SelectAndFillListViewByEntryAndSource(originalEntryOrGuidAndSourceType.entryOrGuid.ToString(), originalEntryOrGuidAndSourceType.sourceType);
            }
            else
                RemoveNonOriginalScriptsFromView();

            buttonGenerateSql.Enabled = true;
            menuItemGenerateSql.Enabled = true;
        }

        private void RemoveNonOriginalScriptsFromView()
        {
            foreach (ListViewItem item in listViewSmartScripts.Items)
                if ((SourceTypes)Int32.Parse(item.SubItems[1].Text) != originalEntryOrGuidAndSourceType.sourceType)
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

        public async void pictureBoxLoadScript_Click(object sender, EventArgs e)
        {
            if (!pictureBoxLoadScript.Enabled)
                return;

            // @Debug new AreatriggersForm().Show();

            listViewSmartScripts.Items.Clear(); //! Clear this even if the search criteria was left empty

            if (String.IsNullOrEmpty(textBoxEntryOrGuid.Text))
                return;

            pictureBoxLoadScript.Enabled = false;

            SourceTypes newSourceType = GetSourceTypeByIndex();
            originalEntryOrGuidAndSourceType.entryOrGuid = XConverter.TryParseStringToInt32(textBoxEntryOrGuid.Text);
            originalEntryOrGuidAndSourceType.sourceType = newSourceType;
            await SelectAndFillListViewByEntryAndSource(textBoxEntryOrGuid.Text, newSourceType);
            checkBoxListActionlistsOrEntries.Text = newSourceType == SourceTypes.SourceTypeScriptedActionlist ? "List entries too" : "List actionlists too";
            buttonNewLine.Enabled = listViewSmartScripts.Items.Count > 0;

            if (listViewSmartScripts.Items.Count > 0)
            {
                SortListView(SortOrder.Ascending, 1);
                listViewSmartScripts.Items[0].Selected = true;
                listViewSmartScripts.Select(); //! Sets the focus on the listview
                lastSmartScriptIdOfScript = XConverter.TryParseStringToInt32(listViewSmartScripts.Items[listViewSmartScripts.Items.Count - 1].SubItems[2].Text);
            }
        }

        private void numericField_KeyPress(object sender, KeyPressEventArgs e)
        {
            //! Only allow typing keys that are numbers
            //! Insert is '-'
            //if (!Char.IsNumber(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.ControlKey && e.KeyChar != (char)Keys.Insert)
            //    e.Handled = true;
        }

        private void buttonSearchPhasemask_Click(object sender, EventArgs e)
        {
            new MultiSelectForm(MultiSelectFormType.MultiSelectFormTypePhaseMask, textBoxEventPhasemask).ShowDialog(this);
        }

        private void buttonSelectEventFlag_Click(object sender, EventArgs e)
        {
            new MultiSelectForm(MultiSelectFormType.MultiSelectFormTypeEventFlag, textBoxEventFlags).ShowDialog(this);
        }

        private async void buttonSearchWorldDb_Click(object sender, EventArgs e)
        {
            List<string> databaseNames = await SAI_Editor_Manager.Instance.GetDatabasesInConnection(textBoxHost.Text, textBoxUsername.Text, XConverter.TryParseStringToUInt32(textBoxPort.Text), textBoxPassword.Text);

            if (databaseNames != null && databaseNames.Count > 0)
                using (var selectDatabaseForm = new SelectDatabaseForm(databaseNames, textBoxWorldDatabase))
                    selectDatabaseForm.ShowDialog(this);
        }

        private void listViewSmartScripts_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            SortListView(SortOrder.None, e.Column);
        }

        private void SortListView(SortOrder order, int column)
        {
            listViewSmartScripts.ListViewItemSorter = lvwColumnSorter;

            if (column != lvwColumnSorter.SortColumn)
            {
                lvwColumnSorter.SortColumn = column;
                lvwColumnSorter.Order = order != SortOrder.None ? order : SortOrder.Ascending;
            }
            else
                lvwColumnSorter.Order = lvwColumnSorter.Order == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;

            listViewSmartScripts.Sort();
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
            TryToOpenLinkForm(textBoxLinkTo);
        }

        private void buttonLinkFrom_Click(object sender, EventArgs e)
        {
            TryToOpenLinkForm(textBoxLinkFrom);
        }

        private void TryToOpenLinkForm(TextBox textBoxToChange)
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

            new SearchForLinkForm(GetItemsBasedOnSelection(listViewSmartScripts), listViewSmartScripts.SelectedItems[0].Index, textBoxToChange).ShowDialog(this);
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
            DeleteSelectedRow();
        }

        private void ResetFieldsToDefault(bool withStatic = false)
        {
            if (withStatic)
            {
                textBoxEntryOrGuid.Text = String.Empty;
                comboBoxSourceType.SelectedIndex = 0;
            }

            comboBoxEventType.SelectedIndex = 0;
            comboBoxActionType.SelectedIndex = 0;
            comboBoxTargetType.SelectedIndex = 0;
            textBoxEventTypeId.Text = "0";
            textBoxActionTypeId.Text = "0";
            textBoxTargetTypeId.Text = "0";
            textBoxEventChance.Text = "100";
            textBoxEventScriptId.Text = "-1";
            textBoxLinkFrom.Text = "0";
            textBoxLinkTo.Text = "0";
            textBoxComments.Text = "Npc - Event - Action (phase) (dungeon difficulty)";
            textBoxEventPhasemask.Text = "0";
            textBoxEventFlags.Text = "0";

            foreach (TabPage page in tabControlParameters.TabPages)
                foreach (Control control in page.Controls)
                    if (control is TextBox)
                        control.Text = "0";

            SetVisibilityOfAllParamButtons(false);
        }

        private void SetVisibilityOfAllParamButtons(bool visible)
        {
            buttonEventParamOneSearch.Visible = visible;
            buttonEventParamTwoSearch.Visible = visible;
            buttonEventParamThreeSearch.Visible = visible;
            buttonEventParamFourSearch.Visible = visible;
            buttonActionParamOneSearch.Visible = visible;
            buttonActionParamTwoSearch.Visible = visible;
            buttonActionParamThreeSearch.Visible = visible;
            buttonActionParamFourSearch.Visible = visible;
            buttonActionParamFiveSearch.Visible = visible;
            buttonActionParamSixSearch.Visible = visible;
            buttonTargetParamOneSearch.Visible = visible;
            buttonTargetParamTwoSearch.Visible = visible;
            buttonTargetParamThreeSearch.Visible = visible;
            buttonTargetParamFourSearch.Visible = visible;
            buttonTargetParamFiveSearch.Visible = visible;
            buttonTargetParamSixSearch.Visible = visible;
            buttonTargetParamSevenSearch.Visible = visible;
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

        private void buttonEventParamOneSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxEventParam1;

            switch ((SmartEvent)comboBoxEventType.SelectedIndex)
            {
                case SmartEvent.SMART_EVENT_SPELLHIT: //! Spell id
                case SmartEvent.SMART_EVENT_FRIENDLY_MISSING_BUFF:
                case SmartEvent.SMART_EVENT_HAS_AURA:
                case SmartEvent.SMART_EVENT_TARGET_BUFFED:
                case SmartEvent.SMART_EVENT_SPELLHIT_TARGET:
                    new SearchFromDatabaseForm(connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSpell).ShowDialog(this);
                    break;
                case SmartEvent.SMART_EVENT_RESPAWN: //! Respawn condition
                    new SingleSelectForm(textBoxToChange, SingleSelectFormType.SingleSelectFormTypeRespawnType).ShowDialog(this);
                    break;
                case SmartEvent.SMART_EVENT_SUMMON_DESPAWNED: //! Creature entry
                case SmartEvent.SMART_EVENT_SUMMONED_UNIT:
                    new SearchFromDatabaseForm(connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry).ShowDialog(this);
                    break;
                case SmartEvent.SMART_EVENT_AREATRIGGER_ONTRIGGER: //! Areatrigger entry
                    new SearchFromDatabaseForm(connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeAreaTrigger).ShowDialog(this);
                    break;
                case SmartEvent.SMART_EVENT_GO_STATE_CHANGED: //! Go state
                    new SingleSelectForm(textBoxToChange, SingleSelectFormType.SingleSelectFormTypeGoState).ShowDialog(this);
                    break;
                case SmartEvent.SMART_EVENT_GAME_EVENT_START: //! Game event entry
                case SmartEvent.SMART_EVENT_GAME_EVENT_END:
                    new SearchFromDatabaseForm(connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGameEvent).ShowDialog(this);
                    break;
                case SmartEvent.SMART_EVENT_MOVEMENTINFORM:
                    new SingleSelectForm(textBoxToChange, SingleSelectFormType.SingleSelectFormTypeMotionType).ShowDialog(this);
                    break;
            }
        }

        private void buttonEventParamTwoSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxEventParam2;

            switch ((SmartEvent)comboBoxEventType.SelectedIndex)
            {
                case SmartEvent.SMART_EVENT_SPELLHIT: //! Spell school
                case SmartEvent.SMART_EVENT_SPELLHIT_TARGET:
                    new SingleSelectForm(textBoxToChange, SingleSelectFormType.SingleSelectFormTypeSpellSchool).ShowDialog(this);
                    break;
                case SmartEvent.SMART_EVENT_RESPAWN: //! Map
                    new SearchFromDatabaseForm(connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeMap).ShowDialog(this);
                    break;
            }
        }

        private void buttonEventParamThreeSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxEventParam3;

            switch ((SmartEvent)comboBoxEventType.SelectedIndex)
            {
                case SmartEvent.SMART_EVENT_RESPAWN: //! Zone
                    new SearchFromDatabaseForm(connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeZone).ShowDialog(this);
                    break;
            }
        }

        private void buttonEventParamFourSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxEventParam4;

            switch ((SmartEvent)comboBoxEventType.SelectedIndex)
            {
                case SmartEvent.SMART_EVENT_KILL: //! Creature entry
                    new SearchFromDatabaseForm(connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry).ShowDialog(this);
                    break;
            }
        }

        private void buttonTargetParamOneSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxTargetParam1;

            switch ((SmartTarget)comboBoxTargetType.SelectedIndex)
            {
                case SmartTarget.SMART_TARGET_CREATURE_RANGE: //! Creature entry
                case SmartTarget.SMART_TARGET_CREATURE_DISTANCE:
                case SmartTarget.SMART_TARGET_CLOSEST_CREATURE:
                    new SearchFromDatabaseForm(connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry).ShowDialog(this);
                    break;
                case SmartTarget.SMART_TARGET_CREATURE_GUID: //! Creature guid
                    new SearchFromDatabaseForm(connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureGuid).ShowDialog(this);
                    break;
                case SmartTarget.SMART_TARGET_GAMEOBJECT_RANGE:
                case SmartTarget.SMART_TARGET_GAMEOBJECT_DISTANCE:
                case SmartTarget.SMART_TARGET_CLOSEST_GAMEOBJECT: //! Gameobject entry
                    new SearchFromDatabaseForm(connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectEntry).ShowDialog(this);
                    break;
                case SmartTarget.SMART_TARGET_GAMEOBJECT_GUID: //! Gameobject guid
                    new SearchFromDatabaseForm(connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectGuid).ShowDialog(this);
                    break;
            }
        }

        private void buttonTargetParamTwoSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxTargetParam2;

            switch ((SmartTarget)comboBoxTargetType.SelectedIndex)
            {
                case SmartTarget.SMART_TARGET_CREATURE_GUID: //! Creature entry
                    new SearchFromDatabaseForm(connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry).ShowDialog(this);
                    break;
                case SmartTarget.SMART_TARGET_GAMEOBJECT_GUID: //! Gameobject entry
                    new SearchFromDatabaseForm(connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectEntry).ShowDialog(this);
                    break;
            }
        }

        private void buttonTargetParamThreeSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxTargetParam3;
        }

        private void buttonTargetParamFourSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxTargetX;
        }

        private void buttonTargetParamFiveSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxTargetY;
        }

        private void buttonTargetParamSixSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxTargetZ;
        }

        private void buttonTargetParamSevenSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxTargetO;
        }

        private void buttonActionParamOneSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxActionParam1;

            switch ((SmartAction)comboBoxActionType.SelectedIndex)
            {
                case SmartAction.SMART_ACTION_CAST:
                case SmartAction.SMART_ACTION_INVOKER_CAST:
                case SmartAction.SMART_ACTION_CROSS_CAST:
                case SmartAction.SMART_ACTION_REMOVEAURASFROMSPELL:
                case SmartAction.SMART_ACTION_ADD_AURA:
                    new SearchFromDatabaseForm(connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSpell).ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_SET_FACTION:
                    new SearchFromDatabaseForm(connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeFaction).ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_EMOTE:
                case SmartAction.SMART_ACTION_RANDOM_EMOTE:
                case SmartAction.SMART_ACTION_SET_EMOTE_STATE:
                    new SearchFromDatabaseForm(connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeEmote).ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_FAIL_QUEST:
                case SmartAction.SMART_ACTION_ADD_QUEST:
                case SmartAction.SMART_ACTION_CALL_AREAEXPLOREDOREVENTHAPPENS:
                case SmartAction.SMART_ACTION_CALL_GROUPEVENTHAPPENS:
                    new SearchFromDatabaseForm(connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeQuest).ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_SET_REACT_STATE:
                    new SingleSelectForm(textBoxToChange, SingleSelectFormType.SingleSelectFormTypeReactState).ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_SOUND:
                    new SearchFromDatabaseForm(connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSound).ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_MORPH_TO_ENTRY_OR_MODEL:
                case SmartAction.SMART_ACTION_SUMMON_CREATURE:
                case SmartAction.SMART_ACTION_CALL_CASTEDCREATUREORGO:
                case SmartAction.SMART_ACTION_KILLED_MONSTER:
                case SmartAction.SMART_ACTION_UPDATE_TEMPLATE:
                case SmartAction.SMART_ACTION_MOUNT_TO_ENTRY_OR_MODEL:
                    new SearchFromDatabaseForm(connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry).ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_GO_SET_LOOT_STATE:
                    new SingleSelectForm(textBoxToChange, SingleSelectFormType.SingleSelectFormTypeGoState).ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_SET_POWER: //! Power type
                case SmartAction.SMART_ACTION_ADD_POWER:
                case SmartAction.SMART_ACTION_REMOVE_POWER:
                    new SingleSelectForm(textBoxToChange, SingleSelectFormType.SingleSelectFormTypePowerType).ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_SUMMON_GO:
                    new SearchFromDatabaseForm(connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectEntry).ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_SET_EVENT_PHASE:
                case SmartAction.SMART_ACTION_RANDOM_PHASE:
                case SmartAction.SMART_ACTION_RANDOM_PHASE_RANGE:
                case SmartAction.SMART_ACTION_SET_PHASE_MASK:
                    new MultiSelectForm(MultiSelectFormType.MultiSelectFormTypePhaseMask, textBoxToChange).ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_ADD_ITEM:
                case SmartAction.SMART_ACTION_REMOVE_ITEM:
                    new SearchFromDatabaseForm(connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeItemEntry).ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_TELEPORT: //! Map
                    new SearchFromDatabaseForm(connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeMap).ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_SUMMON_CREATURE_GROUP:
                    new SearchFromDatabaseForm(connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSummonsId).ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_SET_SHEATH:
                    new SingleSelectForm(textBoxToChange, SingleSelectFormType.SingleSelectFormTypeSheathState).ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_ACTIVATE_TAXI:
                    new SearchFromDatabaseForm(connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeTaxiPath).ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_SET_UNIT_FLAG:
                case SmartAction.SMART_ACTION_REMOVE_UNIT_FLAG:
                    //! There should be a different form opened based on parameter 2. If parameter two is set to '0' it means
                    //! we target UNIT_FIELD_FLAGS. If it's above 0 it means we target UNIT_FIELD_FLAGS2 (notice the 2).
                    if (textBoxActionParam2.Text == "0" || String.IsNullOrWhiteSpace(textBoxActionParam2.Text))
                        new MultiSelectForm(MultiSelectFormType.MultiSelectFormTypeUnitFlag, textBoxToChange).ShowDialog(this);
                    else
                        new MultiSelectForm(MultiSelectFormType.MultiSelectFormTypeUnitFlag2, textBoxToChange).ShowDialog(this);

                    break;
                case SmartAction.SMART_ACTION_SET_GO_FLAG:
                case SmartAction.SMART_ACTION_ADD_GO_FLAG:
                case SmartAction.SMART_ACTION_REMOVE_GO_FLAG:
                    new MultiSelectForm(MultiSelectFormType.MultiSelectFormTypeGoFlag, textBoxToChange).ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_SET_DYNAMIC_FLAG:
                case SmartAction.SMART_ACTION_ADD_DYNAMIC_FLAG:
                case SmartAction.SMART_ACTION_REMOVE_DYNAMIC_FLAG:
                    new MultiSelectForm(MultiSelectFormType.MultiSelectFormTypeDynamicFlag, textBoxToChange).ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_EQUIP:
                    new SearchFromDatabaseForm(connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeEquipTemplate).ShowDialog(this);
                    break;
            }
        }

        private void buttonActionParamTwoSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxActionParam2;

            switch ((SmartAction)comboBoxActionType.SelectedIndex)
            {
                case SmartAction.SMART_ACTION_CAST:
                case SmartAction.SMART_ACTION_INVOKER_CAST:
                case SmartAction.SMART_ACTION_CROSS_CAST:
                    new MultiSelectForm(MultiSelectFormType.MultiSelectFormTypeCastFlag, textBoxToChange).ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_WP_STOP:
                    new SearchFromDatabaseForm(connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeQuest).ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_INTERRUPT_SPELL:
                    new SearchFromDatabaseForm(connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSpell).ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_SUMMON_CREATURE:
                    new SingleSelectForm(textBoxToChange, SingleSelectFormType.SingleSelectFormTypeSummonType).ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_RANDOM_PHASE:
                case SmartAction.SMART_ACTION_RANDOM_PHASE_RANGE:
                    new MultiSelectForm(MultiSelectFormType.MultiSelectFormTypePhaseMask, textBoxToChange).ShowDialog(this);
                    break;
            }
        }

        private void buttonActionParamThreeSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxActionParam3;

            switch ((SmartAction)comboBoxActionType.SelectedIndex)
            {
                case SmartAction.SMART_ACTION_FOLLOW:
                    new SearchFromDatabaseForm(connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry).ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_CROSS_CAST:
                    new SingleSelectForm(textBoxToChange, SingleSelectFormType.SingleSelectFormTypeTargetType).ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_RANDOM_PHASE:
                    new MultiSelectForm(MultiSelectFormType.MultiSelectFormTypePhaseMask, textBoxToChange).ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_EQUIP:
                    new SearchFromDatabaseForm(connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeItemEntry).ShowDialog(this);
                    break;
            }
        }

        private void buttonActionParamFourSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxActionParam4;

            switch ((SmartAction)comboBoxActionType.SelectedIndex)
            {
                case SmartAction.SMART_ACTION_WP_START:
                    new SearchFromDatabaseForm(connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeQuest).ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_RANDOM_PHASE:
                    new MultiSelectForm(MultiSelectFormType.MultiSelectFormTypePhaseMask, textBoxToChange).ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_EQUIP:
                    new SearchFromDatabaseForm(connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeItemEntry).ShowDialog(this);
                    break;
            }
        }

        private void buttonActionParamFiveSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxActionParam5;

            switch ((SmartAction)comboBoxActionType.SelectedIndex)
            {
                case SmartAction.SMART_ACTION_RANDOM_PHASE:
                    new MultiSelectForm(MultiSelectFormType.MultiSelectFormTypePhaseMask, textBoxToChange).ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_EQUIP:
                    new SearchFromDatabaseForm(connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeItemEntry).ShowDialog(this);
                    break;
            }
        }

        private void buttonActionParamSixSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxActionParam6;

            switch ((SmartAction)comboBoxActionType.SelectedIndex)
            {
                case SmartAction.SMART_ACTION_WP_START:
                    new SingleSelectForm(textBoxToChange, SingleSelectFormType.SingleSelectFormTypeReactState).ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_RANDOM_PHASE:
                    new MultiSelectForm(MultiSelectFormType.MultiSelectFormTypePhaseMask, textBoxToChange).ShowDialog(this);
                    break;
            }
        }

        private void TryToOpenPage(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch (Exception)
            {
                MessageBox.Show("The webpage could not be opened!", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void smartAIWikiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TryToOpenPage("http://collab.kpsn.org/display/tc/smart_scripts");
        }

        private void textBoxComments_GotFocus(object sender, EventArgs e)
        {
            if (textBoxComments.Text == "Npc - Event - Action (phase) (dungeon difficulty)")
                textBoxComments.Text = "";
        }

        private void textBoxComments_LostFocus(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxComments.Text))
                textBoxComments.Text = "Npc - Event - Action (phase) (dungeon difficulty)";
        }

        public void ExpandToShowPermanentTooltips(bool expand)
        {
            listViewSmartScriptsInitialHeight = listViewSmartScripts.Height;
            expandingListView = expand;
            contractingListView = !expand;
            listViewSmartScriptsHeightToChangeTo = expand ? listViewSmartScripts.Height + (int)FormSizes.ListViewHeightContract : listViewSmartScripts.Height - (int)FormSizes.ListViewHeightContract;
            timerShowPermanentTooltips.Enabled = true;
            ToolTipHelper.DisableOrEnableAllToolTips(false);

            if (expand)
            {
                panelPermanentTooltipTypes.Visible = false;
                panelPermanentTooltipParameters.Visible = false;
                listViewSmartScriptsHeightToChangeTo = listViewSmartScripts.Height + (int)FormSizes.ListViewHeightContract;
                ChangeParameterFieldsBasedOnType();
            }
            else
            {
                listViewSmartScriptsHeightToChangeTo = listViewSmartScripts.Height - (int)FormSizes.ListViewHeightContract;
                //ChangeParameterFieldsBasedOnType();
            }
        }

        private void timerShowPermanentTooltips_Tick(object sender, EventArgs e)
        {
            if (expandingListView)
            {
                if (listViewSmartScripts.Height < listViewSmartScriptsHeightToChangeTo)
                    listViewSmartScripts.Height += expandAndContractSpeedListView;
                else
                {
                    listViewSmartScripts.Height = listViewSmartScriptsHeightToChangeTo;
                    timerShowPermanentTooltips.Enabled = false;
                    expandingListView = false;
                    ToolTipHelper.DisableOrEnableAllToolTips(true);
                }
            }
            else if (contractingListView)
            {
                if (listViewSmartScripts.Height > listViewSmartScriptsHeightToChangeTo)
                    listViewSmartScripts.Height -= expandAndContractSpeedListView;
                else
                {
                    listViewSmartScripts.Height = listViewSmartScriptsHeightToChangeTo;
                    timerShowPermanentTooltips.Enabled = false;
                    contractingListView = false;
                    panelPermanentTooltipTypes.Visible = true;
                    panelPermanentTooltipParameters.Visible = true;
                }
            }
        }

        private void comboBoxEventType_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                UpdatePermanentTooltipOfTypes(sender as ComboBox, ScriptTypeId.ScriptTypeEvent);
        }

        private void comboBoxActionType_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                UpdatePermanentTooltipOfTypes(sender as ComboBox, ScriptTypeId.ScriptTypeAction);
        }

        private void comboBoxTargetType_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                UpdatePermanentTooltipOfTypes(sender as ComboBox, ScriptTypeId.ScriptTypeTarget);
        }

        private void UpdatePermanentTooltipOfTypes(ComboBox comboBoxToTarget, ScriptTypeId scriptTypeId)
        {
            string toolTipOfType = SAI_Editor_Manager.Instance.GetScriptTypeTooltipById(comboBoxToTarget.SelectedIndex, scriptTypeId);
            string toolTipTitleOfType = comboBoxToTarget.SelectedItem.ToString();

            if (!String.IsNullOrWhiteSpace(toolTipOfType) && !String.IsNullOrWhiteSpace(toolTipTitleOfType))
            {
                labelPermanentTooltipTextTypes.Text = toolTipOfType;
                labelPermanentTooltipTitleTypes.Text = toolTipTitleOfType;
            }
        }

        private void UpdatePermanentTooltipOfParameter(Label labelToTarget, int paramId, ComboBox comboBoxToTarget, ScriptTypeId scriptTypeId)
        {
            string toolTipOfType = SAI_Editor_Manager.Instance.GetParameterTooltipById(comboBoxToTarget.SelectedIndex, paramId, scriptTypeId);

            if (!String.IsNullOrWhiteSpace(toolTipOfType))
                labelPermanentTooltipTextParameters.Text = toolTipOfType;
        }

        private void labelEventParam1_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                UpdatePermanentTooltipOfParameter(sender as Label, 1, comboBoxEventType, ScriptTypeId.ScriptTypeEvent);
        }

        private void labelEventParam2_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                UpdatePermanentTooltipOfParameter(sender as Label, 2, comboBoxEventType, ScriptTypeId.ScriptTypeEvent);
        }

        private void labelEventParam3_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                UpdatePermanentTooltipOfParameter(sender as Label, 3, comboBoxEventType, ScriptTypeId.ScriptTypeEvent);
        }

        private void labelEventParam4_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                UpdatePermanentTooltipOfParameter(sender as Label, 4, comboBoxEventType, ScriptTypeId.ScriptTypeEvent);
        }

        private void labelActionParam1_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                UpdatePermanentTooltipOfParameter(sender as Label, 1, comboBoxActionType, ScriptTypeId.ScriptTypeAction);
        }

        private void labelActionParam2_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                UpdatePermanentTooltipOfParameter(sender as Label, 2, comboBoxActionType, ScriptTypeId.ScriptTypeAction);
        }

        private void labelActionParam3_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                UpdatePermanentTooltipOfParameter(sender as Label, 3, comboBoxActionType, ScriptTypeId.ScriptTypeAction);
        }

        private void labelActionParam4_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                UpdatePermanentTooltipOfParameter(sender as Label, 4, comboBoxActionType, ScriptTypeId.ScriptTypeAction);
        }

        private void labelActionParam5_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                UpdatePermanentTooltipOfParameter(sender as Label, 5, comboBoxActionType, ScriptTypeId.ScriptTypeAction);
        }

        private void labelActionParam6_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                UpdatePermanentTooltipOfParameter(sender as Label, 6, comboBoxActionType, ScriptTypeId.ScriptTypeAction);
        }

        private void labelTargetParam1_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                UpdatePermanentTooltipOfParameter(sender as Label, 1, comboBoxTargetType, ScriptTypeId.ScriptTypeTarget);
        }

        private void labelTargetParam2_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                UpdatePermanentTooltipOfParameter(sender as Label, 2, comboBoxTargetType, ScriptTypeId.ScriptTypeTarget);
        }

        private void labelTargetParam3_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                UpdatePermanentTooltipOfParameter(sender as Label, 3, comboBoxTargetType, ScriptTypeId.ScriptTypeTarget);
        }

        private void labelTargetX_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                UpdatePermanentTooltipOfParameter(sender as Label, 4, comboBoxTargetType, ScriptTypeId.ScriptTypeTarget);
        }

        private void labelTargetY_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                UpdatePermanentTooltipOfParameter(sender as Label, 5, comboBoxTargetType, ScriptTypeId.ScriptTypeTarget);
        }

        private void labelTargetZ_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                UpdatePermanentTooltipOfParameter(sender as Label, 6, comboBoxTargetType, ScriptTypeId.ScriptTypeTarget);
        }

        private void labelTargetO_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                UpdatePermanentTooltipOfParameter(sender as Label, 7, comboBoxTargetType, ScriptTypeId.ScriptTypeTarget);
        }

        private void buttonNewLine_Click(object sender, EventArgs e)
        {
            if (listViewSmartScripts.Items.Count == 0)
            {
                MessageBox.Show("This button should not be available if there are no lines in the listview, please report this as a bug!", "Something went wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //! Writing it all out because it's easier to read and edit it this way
            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Text = originalEntryOrGuidAndSourceType.entryOrGuid.ToString(); //! Entryorguid
            listViewItem.SubItems.Add(((int)originalEntryOrGuidAndSourceType.sourceType).ToString()); //! Source type

            if (checkBoxLockEventId.Checked)
                listViewItem.SubItems.Add((++lastSmartScriptIdOfScript).ToString()); // id
            else
                listViewItem.SubItems.Add("0"); // id

            listViewItem.SubItems.Add("0"); // link
            listViewItem.SubItems.Add("0"); // event type
            listViewItem.SubItems.Add("0"); // phasemask
            listViewItem.SubItems.Add("100"); // event chance
            listViewItem.SubItems.Add("0"); // event flags
            listViewItem.SubItems.Add("0"); // event param 1
            listViewItem.SubItems.Add("0"); // event param 2
            listViewItem.SubItems.Add("0"); // event param 3
            listViewItem.SubItems.Add("0"); // event param 4
            listViewItem.SubItems.Add("0"); // action type
            listViewItem.SubItems.Add("0"); // action param 1
            listViewItem.SubItems.Add("0"); // action param 2
            listViewItem.SubItems.Add("0"); // action param 3
            listViewItem.SubItems.Add("0"); // action param 4
            listViewItem.SubItems.Add("0"); // action param 5
            listViewItem.SubItems.Add("0"); // action param 6
            listViewItem.SubItems.Add("0"); // target type
            listViewItem.SubItems.Add("0"); // target param 1
            listViewItem.SubItems.Add("0"); // target param 2
            listViewItem.SubItems.Add("0"); // target param 3
            listViewItem.SubItems.Add("0"); // target X
            listViewItem.SubItems.Add("0"); // target Y
            listViewItem.SubItems.Add("0"); // target Z
            listViewItem.SubItems.Add("0"); // target O

            //! Todo: implement auto-generated comments
            if (checkBoxAutoGenerateComments.Checked)
                listViewItem.SubItems.Add(GenerateCommentForScript(BuildSmartScript(listViewSmartScripts.Items[0]))); // comment
            else
                listViewItem.SubItems.Add("Npc - Event - Action (phase) (dungeon difficulty)"); // comment

            listViewSmartScripts.Items.Add(listViewItem);
            listViewItem.Selected = true;
            listViewSmartScripts.Select();
        }

        private void buttonEditCurrent_Click(object sender, EventArgs e)
        {

        }

        private SmartScript BuildSmartScript(ListViewItem item)
        {
            SmartScript smartScript = new SmartScript();
            smartScript.entryorguid = XConverter.TryParseStringToInt32(item.Text);
            smartScript.source_type = XConverter.TryParseStringToInt32(item.SubItems[1].Text);
            smartScript.id = XConverter.TryParseStringToInt32(item.SubItems[2].Text);
            smartScript.link = XConverter.TryParseStringToInt32(item.SubItems[3].Text);
            smartScript.event_type = XConverter.TryParseStringToInt32(item.SubItems[4].Text);
            smartScript.event_phase_mask = XConverter.TryParseStringToInt32(item.SubItems[5].Text);
            smartScript.event_chance = XConverter.TryParseStringToInt32(item.SubItems[6].Text);
            smartScript.event_flags = XConverter.TryParseStringToInt32(item.SubItems[7].Text);
            smartScript.event_param1 = XConverter.TryParseStringToInt32(item.SubItems[8].Text);
            smartScript.event_param2 = XConverter.TryParseStringToInt32(item.SubItems[9].Text);
            smartScript.event_param3 = XConverter.TryParseStringToInt32(item.SubItems[10].Text);
            smartScript.event_param4 = XConverter.TryParseStringToInt32(item.SubItems[11].Text);
            smartScript.action_type = XConverter.TryParseStringToInt32(item.SubItems[12].Text);
            smartScript.action_param1 = XConverter.TryParseStringToInt32(item.SubItems[13].Text);
            smartScript.action_param2 = XConverter.TryParseStringToInt32(item.SubItems[14].Text);
            smartScript.action_param3 = XConverter.TryParseStringToInt32(item.SubItems[15].Text);
            smartScript.action_param4 = XConverter.TryParseStringToInt32(item.SubItems[16].Text);
            smartScript.action_param5 = XConverter.TryParseStringToInt32(item.SubItems[17].Text);
            smartScript.action_param6 = XConverter.TryParseStringToInt32(item.SubItems[18].Text);
            smartScript.target_type = XConverter.TryParseStringToInt32(item.SubItems[19].Text);
            smartScript.target_param1 = XConverter.TryParseStringToInt32(item.SubItems[20].Text);
            smartScript.target_param2 = XConverter.TryParseStringToInt32(item.SubItems[21].Text);
            smartScript.target_param3 = XConverter.TryParseStringToInt32(item.SubItems[22].Text);
            smartScript.target_x = XConverter.TryParseStringToInt32(item.SubItems[23].Text);
            smartScript.target_y = XConverter.TryParseStringToInt32(item.SubItems[24].Text);
            smartScript.target_z = XConverter.TryParseStringToInt32(item.SubItems[25].Text);
            smartScript.target_o = XConverter.TryParseStringToInt32(item.SubItems[26].Text);
            smartScript.comment = item.SubItems[27].Text;
            return smartScript;
        }

        private string GenerateCommentForScript(SmartScript smartScript)
        {
            return String.Empty;
        }

        private void textBoxLinkTo_TextChanged(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
                listViewSmartScripts.SelectedItems[0].SubItems[3].Text = textBoxLinkTo.Text;
        }

        private void textBoxComments_TextChanged(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
                listViewSmartScripts.SelectedItems[0].SubItems[27].Text = textBoxComments.Text;
        }

        private void textBoxEventPhasemask_TextChanged(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
                listViewSmartScripts.SelectedItems[0].SubItems[5].Text = textBoxEventPhasemask.Text;
        }

        private void textBoxEventChance_ValueChanged(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
                listViewSmartScripts.SelectedItems[0].SubItems[6].Text = textBoxEventChance.Value.ToString(); //! Using .Text propert results in wrong value
        }

        private void textBoxEventFlags_TextChanged(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
                listViewSmartScripts.SelectedItems[0].SubItems[7].Text = textBoxEventFlags.Text;
        }

        private void textBoxLinkFrom_TextChanged(object sender, EventArgs e)
        {
            // unused (?)
        }

        private void textBoxEventParam1_TextChanged(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
                listViewSmartScripts.SelectedItems[0].SubItems[8].Text = textBoxEventParam1.Text;
        }

        private void textBoxEventParam2_TextChanged(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
                listViewSmartScripts.SelectedItems[0].SubItems[9].Text = textBoxEventParam2.Text;
        }

        private void textBoxEventParam3_TextChanged(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
                listViewSmartScripts.SelectedItems[0].SubItems[10].Text = textBoxEventParam3.Text;
        }

        private void textBoxEventParam4_TextChanged(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
                listViewSmartScripts.SelectedItems[0].SubItems[11].Text = textBoxEventParam4.Text;
        }

        private void textBoxActionParam1_TextChanged(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
                listViewSmartScripts.SelectedItems[0].SubItems[13].Text = textBoxActionParam1.Text;
        }

        private void textBoxActionParam2_TextChanged(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
                listViewSmartScripts.SelectedItems[0].SubItems[14].Text = textBoxActionParam2.Text;
        }

        private void textBoxActionParam3_TextChanged(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
                listViewSmartScripts.SelectedItems[0].SubItems[15].Text = textBoxActionParam3.Text;
        }

        private void textBoxActionParam4_TextChanged(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
                listViewSmartScripts.SelectedItems[0].SubItems[16].Text = textBoxActionParam4.Text;
        }

        private void textBoxActionParam5_TextChanged(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
                listViewSmartScripts.SelectedItems[0].SubItems[17].Text = textBoxActionParam5.Text;
        }

        private void textBoxActionParam6_TextChanged(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
                listViewSmartScripts.SelectedItems[0].SubItems[18].Text = textBoxActionParam6.Text;
        }

        private void textBoxTargetParam1_TextChanged(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
                listViewSmartScripts.SelectedItems[0].SubItems[20].Text = textBoxTargetParam1.Text;
        }

        private void textBoxTargetParam2_TextChanged(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
                listViewSmartScripts.SelectedItems[0].SubItems[21].Text = textBoxTargetParam2.Text;
        }

        private void textBoxTargetParam3_TextChanged(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
                listViewSmartScripts.SelectedItems[0].SubItems[22].Text = textBoxTargetParam3.Text;
        }

        private void textBoxTargetX_TextChanged(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
                listViewSmartScripts.SelectedItems[0].SubItems[23].Text = textBoxTargetX.Text;
        }

        private void textBoxTargetY_TextChanged(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
                listViewSmartScripts.SelectedItems[0].SubItems[24].Text = textBoxTargetY.Text;
        }

        private void textBoxTargetZ_TextChanged(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
                listViewSmartScripts.SelectedItems[0].SubItems[25].Text = textBoxTargetZ.Text;
        }

        private void textBoxTargetO_TextChanged(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
                listViewSmartScripts.SelectedItems[0].SubItems[26].Text = textBoxTargetO.Text;
        }

        private void textBoxEntryOrGuid_TextChanged(object sender, EventArgs e)
        {
            pictureBoxLoadScript.Enabled = textBoxEntryOrGuid.Text.Length > 0;
        }

        private void generateSQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSqlOutputForm();
        }

        private void buttonGenerateSql_Click(object sender, EventArgs e)
        {
            OpenSqlOutputForm();
        }

        private void OpenSqlOutputForm()
        {
            List<SmartScript> smartScriptsToExport = new List<SmartScript>();

            foreach (ListViewItem item in listViewSmartScripts.Items)
                smartScriptsToExport.Add(BuildSmartScript(item));

            if (smartScriptsToExport.Count > 0)
                new SqlOutputForm(smartScriptsToExport).ShowDialog(this);
        }
    }
}
