using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using SAI_Editor.Classes;
using SAI_Editor.Classes.Database.Classes;
using SAI_Editor.Enumerators;
using SAI_Editor.Forms.SearchForms;
using SAI_Editor.Properties;

namespace SAI_Editor.Forms
{

    public enum FormState
    {
        FormStateLogin,
        FormStateExpandingOrContracting,
        FormStateMain,
    }

    internal enum FormSizes
    {
        LoginFormWidth = 403,
        LoginFormHeight = 236,

        MainFormWidth = 957,
        MainFormHeight = 505,

        ListViewHeightContract = 65,

        LoginFormHeightShowWarning = 309,
    }

    internal enum MaxValues
    {
        MaxEventType = 74,
        MaxActionType = 110,
        MaxTargetType = 26,
    }

    public enum SourceTypes
    {
        SourceTypeNone = -1,
        SourceTypeCreature = 0,
        SourceTypeGameobject = 1,
        SourceTypeAreaTrigger = 2,
        SourceTypeScriptedActionlist = 9,
    }

    public struct EntryOrGuidAndSourceType
    {
        public EntryOrGuidAndSourceType(int _entryOrGuid, SourceTypes _sourceType) { this.entryOrGuid = _entryOrGuid; this.sourceType = _sourceType; }

        public int entryOrGuid;
        public SourceTypes sourceType;
    }

    public partial class MainForm : Form
    {
        public MySqlConnectionStringBuilder connectionString = new MySqlConnectionStringBuilder();
        private readonly List<Control> controlsLoginForm = new List<Control>();
        private readonly List<Control> controlsMainForm = new List<Control>();
        private bool contractingToLoginForm, expandingToMainForm, expandingListView, contractingListView;
        private int originalHeight = 0, originalWidth = 0;
        private int MainFormWidth = (int)FormSizes.MainFormWidth, MainFormHeight = (int)FormSizes.MainFormHeight;
        private int listViewSmartScriptsInitialHeight, listViewSmartScriptsHeightToChangeTo;
        public int expandAndContractSpeed = 5, expandAndContractSpeedListView = 2;
        public EntryOrGuidAndSourceType originalEntryOrGuidAndSourceType = new EntryOrGuidAndSourceType();
        public int lastSmartScriptIdOfScript = 0;
        private readonly ListViewColumnSorter lvwColumnSorter = new ListViewColumnSorter();
        private bool runningConstructor = false, updatingFieldsBasedOnSelectedScript = false;
        private int previousLinkFrom = -1;
        private List<SmartScript> lastDeletedSmartScripts = new List<SmartScript>(), smartScriptsOnClipBoard = new List<SmartScript>();

        private FormState _formState = FormState.FormStateLogin;
        public FormState formState
        {
            get
            {
                return this._formState;
            }
            set
            {
                this._formState = value;

                switch (this._formState)
                {
                    case FormState.FormStateExpandingOrContracting:
                        this.FormBorderStyle = FormBorderStyle.FixedDialog; //! Don't allow resizing by user
                        this.MainFormHeight = Settings.Default.MainFormHeight;
                        this.MinimumSize = new Size((int)FormSizes.LoginFormWidth, (int)FormSizes.LoginFormHeight);
                        this.MaximumSize = new Size(this.MainFormWidth, this.MainFormHeight);
                        this.panelPermanentTooltipTypes.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                        this.panelPermanentTooltipParameters.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                        break;
                    case FormState.FormStateLogin:
                        this.FormBorderStyle = FormBorderStyle.FixedDialog;
                        this.MinimumSize = new Size((int)FormSizes.LoginFormWidth, (int)FormSizes.LoginFormHeight);
                        this.MaximumSize = new Size((int)FormSizes.LoginFormWidth, (int)FormSizes.LoginFormHeight);
                        this.panelPermanentTooltipTypes.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                        this.panelPermanentTooltipParameters.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                        break;
                    case FormState.FormStateMain:
                        this.FormBorderStyle = FormBorderStyle.Sizable;
                        this.MinimumSize = new Size(this.MainFormWidth, (int)FormSizes.MainFormHeight);
                        this.MaximumSize = new Size(this.MainFormWidth, (int)FormSizes.MainFormHeight + 100);
                        this.panelPermanentTooltipTypes.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                        this.panelPermanentTooltipParameters.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                        break;
                }
            }
        }

        public MainForm()
        {
            this.InitializeComponent();
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            this.runningConstructor = true;
            this.menuStrip.Visible = false; //! Doing this in main code so we can actually see the menustrip in designform

            this.Width = (int)FormSizes.LoginFormWidth;
            this.Height = (int)FormSizes.LoginFormHeight;

            this.originalHeight = this.Height;
            this.originalWidth = this.Width;

            if (this.MainFormWidth > SystemInformation.VirtualScreen.Width)
                this.MainFormWidth = SystemInformation.VirtualScreen.Width;

            if (this.MainFormHeight > SystemInformation.VirtualScreen.Height)
                this.MainFormHeight = SystemInformation.VirtualScreen.Height;

            try
            {
                this.textBoxHost.Text = Settings.Default.Host;
                this.textBoxUsername.Text = Settings.Default.User;
                this.textBoxPassword.Text = SAI_Editor_Manager.Instance.GetPasswordSetting();
                this.textBoxWorldDatabase.Text = Settings.Default.Database;
                this.textBoxPort.Text = Settings.Default.Port > 0 ? Settings.Default.Port.ToString() : String.Empty;
                this.expandAndContractSpeed = Settings.Default.AnimationSpeed;
                this.radioButtonConnectToMySql.Checked = Settings.Default.UseWorldDatabase;
                this.radioButtonDontUseDatabase.Checked = !Settings.Default.UseWorldDatabase;
                this.checkBoxListActionlistsOrEntries.Enabled = Settings.Default.UseWorldDatabase;
                this.menuItemRevertQuery.Enabled = Settings.Default.UseWorldDatabase;
                this.SetGenerateCommentsEnabled(Settings.Default.UseWorldDatabase);
                this.buttonSearchForEntryOrGuid.Enabled = Settings.Default.UseWorldDatabase || (SourceTypes)Settings.Default.LastSourceType == SourceTypes.SourceTypeAreaTrigger;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            foreach (Control control in this.Controls)
            {
                //! These two are set manually because otherwise they will always show for a split second before disappearing again.
                if (control.Name == "panelPermanentTooltipTypes" || control.Name == "panelPermanentTooltipParameters")
                    continue;

                if (control.Visible)
                    this.controlsLoginForm.Add(control);
                else
                    this.controlsMainForm.Add(control);
            }

            this.comboBoxSourceType.SelectedIndex = 0;
            this.comboBoxEventType.SelectedIndex = 0;
            this.comboBoxActionType.SelectedIndex = 0;
            this.comboBoxTargetType.SelectedIndex = 0;

            //! We first load the information and then change the parameter fields
            await SAI_Editor_Manager.Instance.LoadSQLiteDatabaseInfo();
            this.ChangeParameterFieldsBasedOnType();

            if (Settings.Default.AutoConnect)
            {
                this.checkBoxAutoConnect.Checked = true;

                if (Settings.Default.UseWorldDatabase)
                {
                    this.connectionString = new MySqlConnectionStringBuilder();
                    this.connectionString.Server = this.textBoxHost.Text;
                    this.connectionString.UserID = this.textBoxUsername.Text;
                    this.connectionString.Port = XConverter.ToUInt32(this.textBoxPort.Text);
                    this.connectionString.Database = this.textBoxWorldDatabase.Text;

                    if (this.textBoxPassword.Text.Length > 0)
                        this.connectionString.Password = this.textBoxPassword.Text;
                }

                if (!Settings.Default.UseWorldDatabase || SAI_Editor_Manager.Instance.worldDatabase.CanConnectToDatabase(this.connectionString, false))
                {
                    SAI_Editor_Manager.Instance.ResetWorldDatabase(this.connectionString);
                    this.buttonConnect.PerformClick();

                    if (Settings.Default.InstantExpand)
                        this.StartExpandingToMainForm(true);
                }
            }

            this.tabControlParameters.AutoScrollOffset = new Point(5, 5);

            //! Permanent scrollbar to the parameters tabpage windows
            foreach (TabPage page in this.tabControlParameters.TabPages)
            {
                page.HorizontalScroll.Enabled = false;
                page.HorizontalScroll.Visible = false;

                page.AutoScroll = true;
                page.AutoScrollMinSize = new Size(page.Width, page.Height);
            }

            this.panelLoginBox.Location = new Point(9, 8);

            if (Settings.Default.HidePass)
                this.textBoxPassword.PasswordChar = '●';

            this.textBoxComments.GotFocus += this.textBoxComments_GotFocus;
            this.textBoxComments.LostFocus += this.textBoxComments_LostFocus;

            this.panelPermanentTooltipTypes.BackColor = Color.FromArgb(255, 255, 225);
            this.panelPermanentTooltipParameters.BackColor = Color.FromArgb(255, 255, 225);
            this.labelPermanentTooltipTextTypes.BackColor = Color.FromArgb(255, 255, 225);

            this.pictureBoxLoadScript.Enabled = this.textBoxEntryOrGuid.Text.Length > 0 && Settings.Default.UseWorldDatabase;
            this.pictureBoxCreateScript.Enabled = this.textBoxEntryOrGuid.Text.Length > 0;

            this.textBoxEventType.MouseWheel += this.textBoxEventType_MouseWheel;
            this.textBoxActionType.MouseWheel += this.textBoxActionType_MouseWheel;
            this.textBoxTargetType.MouseWheel += this.textBoxTargetType_MouseWheel;

            this.buttonNewLine.Enabled = this.textBoxEntryOrGuid.Text.Length > 0;

            this.runningConstructor = false;
        }

        private void timerExpandOrContract_Tick(object sender, EventArgs e)
        {
            if (this.expandingToMainForm)
            {
                if (this.Height < this.MainFormHeight)
                    this.Height += this.expandAndContractSpeed;
                else
                {
                    this.Height = this.MainFormHeight;

                    if (this.Width >= this.MainFormWidth && this.timerExpandOrContract.Enabled) //! If both finished
                    {
                        this.Width = this.MainFormWidth;
                        this.timerExpandOrContract.Enabled = false;
                        this.expandingToMainForm = false;
                        this.formState = FormState.FormStateMain;
                        this.FinishedExpandingOrContracting(true);
                    }
                }

                if (this.Width < this.MainFormWidth)
                    this.Width += this.expandAndContractSpeed;
                else
                {
                    this.Width = this.MainFormWidth;

                    if (this.Height >= this.MainFormHeight && this.timerExpandOrContract.Enabled) //! If both finished
                    {
                        this.Height = this.MainFormHeight;
                        this.timerExpandOrContract.Enabled = false;
                        this.expandingToMainForm = false;
                        this.formState = FormState.FormStateMain;
                        this.FinishedExpandingOrContracting(true);
                    }
                }
            }
            else if (this.contractingToLoginForm)
            {
                if (this.Height > this.originalHeight)
                    this.Height -= this.expandAndContractSpeed;
                else
                {
                    this.Height = this.originalHeight;

                    if (this.Width <= this.originalWidth && this.timerExpandOrContract.Enabled) //! If both finished
                    {
                        this.Width = this.originalWidth;
                        this.timerExpandOrContract.Enabled = false;
                        this.contractingToLoginForm = false;
                        this.formState = FormState.FormStateLogin;
                        this.FinishedExpandingOrContracting(false);
                    }
                }

                if (this.Width > this.originalWidth)
                    this.Width -= this.expandAndContractSpeed;
                else
                {
                    this.Width = this.originalWidth;

                    if (this.Height <= this.originalHeight && this.timerExpandOrContract.Enabled) //! If both finished
                    {
                        this.Height = this.originalHeight;
                        this.timerExpandOrContract.Enabled = false;
                        this.contractingToLoginForm = false;
                        this.formState = FormState.FormStateLogin;
                        this.FinishedExpandingOrContracting(false);
                    }
                }
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            bool connectToMySql = this.radioButtonConnectToMySql.Checked;

            if (connectToMySql)
            {
                if (String.IsNullOrEmpty(this.textBoxHost.Text))
                {
                    MessageBox.Show("The host field has to be filled!", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (String.IsNullOrEmpty(this.textBoxUsername.Text))
                {
                    MessageBox.Show("The username field has to be filled!", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (this.textBoxPassword.Text.Length > 0 && String.IsNullOrEmpty(this.textBoxPassword.Text))
                {
                    MessageBox.Show("The password field can not consist of only whitespaces!", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (String.IsNullOrEmpty(this.textBoxWorldDatabase.Text))
                {
                    MessageBox.Show("The world database field has to be filled!", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (String.IsNullOrEmpty(this.textBoxPort.Text))
                {
                    MessageBox.Show("The port field has to be filled!", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                this.connectionString = new MySqlConnectionStringBuilder();
                this.connectionString.Server = this.textBoxHost.Text;
                this.connectionString.UserID = this.textBoxUsername.Text;
                this.connectionString.Port = XConverter.ToUInt32(this.textBoxPort.Text);
                this.connectionString.Database = this.textBoxWorldDatabase.Text;

                if (this.textBoxPassword.Text.Length > 0)
                    this.connectionString.Password = this.textBoxPassword.Text;
            }

            Settings.Default.UseWorldDatabase = connectToMySql;
            Settings.Default.Save();

            if (!connectToMySql || SAI_Editor_Manager.Instance.worldDatabase.CanConnectToDatabase(this.connectionString))
            {
                this.StartExpandingToMainForm(Settings.Default.InstantExpand);

                if (!connectToMySql)
                    SAI_Editor_Manager.Instance.ResetDatabases();

                this.HandleUseWorldDatabaseSettingChanged();
            }
        }

        private void StartExpandingToMainForm(bool instant = false)
        {
            if (this.radioButtonConnectToMySql.Checked)
            {
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] buffer = new byte[1024];
                rng.GetBytes(buffer);
                string salt = BitConverter.ToString(buffer);
                rng.Dispose();

                Settings.Default.Entropy = salt;
                Settings.Default.Host = this.textBoxHost.Text;
                Settings.Default.User = this.textBoxUsername.Text;
                Settings.Default.Password = this.textBoxPassword.Text.Length == 0 ? String.Empty : this.textBoxPassword.Text.ToSecureString().EncryptString(Encoding.Unicode.GetBytes(salt));
                Settings.Default.Database = this.textBoxWorldDatabase.Text;
                Settings.Default.AutoConnect = this.checkBoxAutoConnect.Checked;
                Settings.Default.Port = XConverter.ToUInt32(this.textBoxPort.Text);
                Settings.Default.UseWorldDatabase = true;
                Settings.Default.Save();
            }

            this.ResetFieldsToDefault();

            if (this.radioButtonConnectToMySql.Checked)
                this.Text = "SAI-Editor - Connection: " + this.textBoxUsername.Text + ", " + this.textBoxHost.Text + ", " + this.textBoxPort.Text;
            else
                this.Text = "SAI-Editor - Creator-only mode, no database connection";

            if (instant)
            {
                this.Width = this.MainFormWidth;
                this.Height = Settings.Default.MainFormHeight;
                this.formState = FormState.FormStateMain;
                this.FinishedExpandingOrContracting(true);
            }
            else
            {
                this.formState = FormState.FormStateExpandingOrContracting;
                this.timerExpandOrContract.Enabled = true;
                this.expandingToMainForm = true;
            }

            foreach (Control control in this.controlsLoginForm)
                control.Visible = false;

            foreach (Control control in this.controlsMainForm)
                control.Visible = instant;

            this.panelPermanentTooltipTypes.Visible = false;
            this.panelPermanentTooltipParameters.Visible = false;
        }

        private void StartContractingToLoginForm(bool instant = false)
        {
            this.Text = "SAI-Editor: Login";

            if (Settings.Default.ShowTooltipsPermanently)
                this.listViewSmartScripts.Height += (int)FormSizes.ListViewHeightContract;

            if (instant)
            {
                this.Width = this.originalWidth;
                this.Height = this.originalHeight;
                this.formState = FormState.FormStateLogin;
                this.FinishedExpandingOrContracting(false);
            }
            else
            {
                this.formState = FormState.FormStateExpandingOrContracting;
                this.timerExpandOrContract.Enabled = true;
                this.contractingToLoginForm = true;
            }

            foreach (var control in this.controlsLoginForm)
                control.Visible = instant;

            foreach (var control in this.controlsMainForm)
                control.Visible = false;
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            this.textBoxHost.Text = "";
            this.textBoxUsername.Text = "";
            this.textBoxPassword.Text = "";
            this.textBoxWorldDatabase.Text = "";
            this.textBoxPort.Text = "";
            this.checkBoxAutoConnect.Checked = false;
            this.radioButtonConnectToMySql.Checked = true;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    switch (this.formState)
                    {
                        case FormState.FormStateLogin:
                            this.buttonConnect.PerformClick();
                            break;
                        case FormState.FormStateMain:
                            if (this.textBoxEntryOrGuid.Focused)
                            {
                                if (Settings.Default.UseWorldDatabase)
                                    this.pictureBoxLoadScript_Click(this.pictureBoxLoadScript, null);
                                else
                                    this.pictureBoxCreateScript_Click(this.pictureBoxCreateScript, null);
                            }

                            break;
                    }
                    break;
            }
        }

        private void buttonSearchForEntry_Click(object sender, EventArgs e)
        {
            //! Just keep it in main thread; no purpose starting a new thread for this (unless workspaces get implemented, maybe)
            using (var entryForm = new SearchForEntryForm(this.connectionString, this.textBoxEntryOrGuid.Text, this.GetSourceTypeByIndex()))
                entryForm.ShowDialog(this);
        }

        private void menuItemReconnect_Click(object sender, EventArgs e)
        {
            if (this.formState != FormState.FormStateMain)
                return;

            this.panelPermanentTooltipTypes.Visible = false;
            this.panelPermanentTooltipParameters.Visible = false;
            this.SaveLastUsedFields();
            this.ResetFieldsToDefault();
            this.listViewSmartScripts.ReplaceSmartScripts(new List<SmartScript>());
            this.StartContractingToLoginForm(Settings.Default.InstantExpand);
        }

        private async void comboBoxEventType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.textBoxEventType.Text = this.comboBoxEventType.SelectedIndex.ToString();
            this.textBoxEventType.SelectionStart = 3; //! Set cursor to end of text

            if (!this.runningConstructor)
            {
                this.ChangeParameterFieldsBasedOnType();
                this.UpdatePermanentTooltipOfTypes(this.comboBoxEventType, ScriptTypeId.ScriptTypeEvent);
            }

            if (this.listViewSmartScripts.SelectedItems.Count > 0)
            {
                this.listViewSmartScripts.SelectedSmartScript.event_type = this.comboBoxEventType.SelectedIndex;
                this.listViewSmartScripts.ReplaceSmartScript(this.listViewSmartScripts.SelectedSmartScript);
                //listViewSmartScripts.SelectedItems[0].SubItems[4].Text = comboBoxEventType.SelectedIndex.ToString();
                await this.GenerateCommentForSmartScript(this.listViewSmartScripts.SelectedSmartScript);
            }
        }

        private async void comboBoxActionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.textBoxActionType.Text = this.comboBoxActionType.SelectedIndex.ToString();
            this.textBoxActionType.SelectionStart = 3; //! Set cursor to end of text

            if (!this.runningConstructor)
            {
                this.ChangeParameterFieldsBasedOnType();
                this.UpdatePermanentTooltipOfTypes(this.comboBoxActionType, ScriptTypeId.ScriptTypeAction);
            }

            if (this.listViewSmartScripts.SelectedItems.Count > 0)
            {
                this.listViewSmartScripts.SelectedSmartScript.action_type = this.comboBoxActionType.SelectedIndex;
                this.listViewSmartScripts.ReplaceSmartScript(this.listViewSmartScripts.SelectedSmartScript);
                //listViewSmartScripts.SelectedItems[0].SubItems[12].Text = comboBoxActionType.SelectedIndex.ToString();
                await this.GenerateCommentForSmartScript(this.listViewSmartScripts.SelectedSmartScript);
            }
        }

        private async void comboBoxTargetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.textBoxTargetType.Text = this.comboBoxTargetType.SelectedIndex.ToString();
            this.textBoxTargetType.SelectionStart = 3; //! Set cursor to end of text

            if (!this.runningConstructor)
            {
                this.ChangeParameterFieldsBasedOnType();
                this.UpdatePermanentTooltipOfTypes(this.comboBoxTargetType, ScriptTypeId.ScriptTypeTarget);
            }

            if (this.listViewSmartScripts.SelectedItems.Count > 0)
            {
                this.listViewSmartScripts.SelectedSmartScript.target_type = this.comboBoxTargetType.SelectedIndex;
                this.listViewSmartScripts.ReplaceSmartScript(this.listViewSmartScripts.SelectedSmartScript);
                //listViewSmartScripts.SelectedItems[0].SubItems[19].Text = comboBoxTargetType.SelectedIndex.ToString();
                await this.GenerateCommentForSmartScript(this.listViewSmartScripts.SelectedSmartScript);
            }
        }

        private void ChangeParameterFieldsBasedOnType()
        {
            //! Event parameters
            int event_type = this.comboBoxEventType.SelectedIndex;
            this.labelEventParam1.Text = SAI_Editor_Manager.Instance.GetParameterStringById(event_type, 1, ScriptTypeId.ScriptTypeEvent);
            this.labelEventParam2.Text = SAI_Editor_Manager.Instance.GetParameterStringById(event_type, 2, ScriptTypeId.ScriptTypeEvent);
            this.labelEventParam3.Text = SAI_Editor_Manager.Instance.GetParameterStringById(event_type, 3, ScriptTypeId.ScriptTypeEvent);
            this.labelEventParam4.Text = SAI_Editor_Manager.Instance.GetParameterStringById(event_type, 4, ScriptTypeId.ScriptTypeEvent);

            if (!Settings.Default.ShowTooltipsPermanently)
            {
                this.AddTooltip(this.comboBoxEventType, this.comboBoxEventType.SelectedItem.ToString(), SAI_Editor_Manager.Instance.GetScriptTypeTooltipById(event_type, ScriptTypeId.ScriptTypeEvent));
                this.AddTooltip(this.labelEventParam1, this.labelEventParam1.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(event_type, 1, ScriptTypeId.ScriptTypeEvent));
                this.AddTooltip(this.labelEventParam2, this.labelEventParam2.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(event_type, 2, ScriptTypeId.ScriptTypeEvent));
                this.AddTooltip(this.labelEventParam3, this.labelEventParam3.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(event_type, 3, ScriptTypeId.ScriptTypeEvent));
                this.AddTooltip(this.labelEventParam4, this.labelEventParam4.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(event_type, 4, ScriptTypeId.ScriptTypeEvent));
            }

            //! Action parameters
            int action_type = this.comboBoxActionType.SelectedIndex;
            this.labelActionParam1.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 1, ScriptTypeId.ScriptTypeAction);
            this.labelActionParam2.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 2, ScriptTypeId.ScriptTypeAction);
            this.labelActionParam3.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 3, ScriptTypeId.ScriptTypeAction);
            this.labelActionParam4.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 4, ScriptTypeId.ScriptTypeAction);
            this.labelActionParam5.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 5, ScriptTypeId.ScriptTypeAction);
            this.labelActionParam6.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 6, ScriptTypeId.ScriptTypeAction);

            if (!Settings.Default.ShowTooltipsPermanently)
            {
                this.AddTooltip(this.comboBoxActionType, this.comboBoxActionType.SelectedItem.ToString(), SAI_Editor_Manager.Instance.GetScriptTypeTooltipById(action_type, ScriptTypeId.ScriptTypeAction));
                this.AddTooltip(this.labelActionParam1, this.labelActionParam1.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 1, ScriptTypeId.ScriptTypeAction));
                this.AddTooltip(this.labelActionParam2, this.labelActionParam2.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 2, ScriptTypeId.ScriptTypeAction));
                this.AddTooltip(this.labelActionParam3, this.labelActionParam3.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 3, ScriptTypeId.ScriptTypeAction));
                this.AddTooltip(this.labelActionParam4, this.labelActionParam4.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 4, ScriptTypeId.ScriptTypeAction));
                this.AddTooltip(this.labelActionParam5, this.labelActionParam5.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 5, ScriptTypeId.ScriptTypeAction));
                this.AddTooltip(this.labelActionParam6, this.labelActionParam6.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 6, ScriptTypeId.ScriptTypeAction));
            }

            //! Target parameters
            int target_type = this.comboBoxTargetType.SelectedIndex;
            this.labelTargetParam1.Text = SAI_Editor_Manager.Instance.GetParameterStringById(target_type, 1, ScriptTypeId.ScriptTypeTarget);
            this.labelTargetParam2.Text = SAI_Editor_Manager.Instance.GetParameterStringById(target_type, 2, ScriptTypeId.ScriptTypeTarget);
            this.labelTargetParam3.Text = SAI_Editor_Manager.Instance.GetParameterStringById(target_type, 3, ScriptTypeId.ScriptTypeTarget);

            if (!Settings.Default.ShowTooltipsPermanently)
            {
                this.AddTooltip(this.comboBoxTargetType, this.comboBoxTargetType.SelectedItem.ToString(), SAI_Editor_Manager.Instance.GetScriptTypeTooltipById(target_type, ScriptTypeId.ScriptTypeTarget));
                this.AddTooltip(this.labelTargetParam1, this.labelTargetParam1.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(target_type, 1, ScriptTypeId.ScriptTypeTarget));
                this.AddTooltip(this.labelTargetParam2, this.labelTargetParam2.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(target_type, 2, ScriptTypeId.ScriptTypeTarget));
                this.AddTooltip(this.labelTargetParam3, this.labelTargetParam3.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(target_type, 3, ScriptTypeId.ScriptTypeTarget));
            }

            this.AdjustAllParameterFields(event_type, action_type, target_type);
        }

        private void checkBoxLockEventId_CheckedChanged(object sender, EventArgs e)
        {
            this.textBoxId.Enabled = !this.checkBoxLockEventId.Checked;
        }

        private void FinishedExpandingOrContracting(bool expanding)
        {
            foreach (var control in this.controlsLoginForm)
                control.Visible = !expanding;

            foreach (var control in this.controlsMainForm)
                control.Visible = expanding;

            if (!expanding)
                this.HandleHeightLoginFormBasedOnuseDatabaseSetting();

            this.panelPermanentTooltipTypes.Visible = false;
            this.panelPermanentTooltipParameters.Visible = false;

            if (expanding && Settings.Default.ShowTooltipsPermanently)
                this.ExpandToShowPermanentTooltips(false);

            this.textBoxEntryOrGuid.Text = Settings.Default.LastEntryOrGuid;
            this.comboBoxSourceType.SelectedIndex = Settings.Default.LastSourceType;
            this.checkBoxShowBasicInfo.Checked = Settings.Default.ShowBasicInfo;
            this.checkBoxLockEventId.Checked = Settings.Default.LockSmartScriptId;
            this.checkBoxListActionlistsOrEntries.Checked = Settings.Default.ListActionLists;
            this.checkBoxAllowChangingEntryAndSourceType.Checked = Settings.Default.AllowChangingEntryAndSourceType;
            this.checkBoxUsePhaseColors.Checked = Settings.Default.PhaseHighlighting;
            this.checkBoxUsePermanentTooltips.Checked = Settings.Default.ShowTooltipsPermanently;

            if (expanding && this.radioButtonConnectToMySql.Checked)
                this.TryToLoadScript(showErrorIfNoneFound: false);
        }

        private async Task<List<SmartScript>> GetSmartScriptsForEntryAndSourceType(string entryOrGuid, SourceTypes sourceType, bool showError = true, bool promptCreateIfNoneFound = false)
        {
            List<SmartScript> smartScriptsToReturn = new List<SmartScript>();

            try
            {
                List<SmartScript> smartScripts = await SAI_Editor_Manager.Instance.worldDatabase.GetSmartScripts(XConverter.ToInt32(entryOrGuid), (int)sourceType);

                if (smartScripts == null)
                {
                    if (showError)
                    {
                        bool showNormalErrorMessage = false;
                        string message = String.Format("The entryorguid '{0}' could not be found in the smart_scripts table for the given source_type!", entryOrGuid);
                        smartScripts = await SAI_Editor_Manager.Instance.worldDatabase.GetSmartScriptsWithoutSourceType(XConverter.ToInt32(entryOrGuid), (int)sourceType);

                        if (smartScripts != null)
                        {
                            message += "\n\nA script was found with this entry using sourcetype " + smartScripts[0].source_type + " (" + this.GetSourceTypeString((SourceTypes)smartScripts[0].source_type) + "). Do you wish to load this instead?";
                            DialogResult dialogResult = MessageBox.Show(message, "No scripts found!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                            if (dialogResult == DialogResult.Yes)
                            {
                                this.textBoxEntryOrGuid.Text = smartScripts[0].entryorguid.ToString();
                                this.comboBoxSourceType.SelectedIndex = this.GetIndexBySourceType((SourceTypes)smartScripts[0].source_type);
                                this.TryToLoadScript();
                            }
                        }
                        else
                        {
                            switch (sourceType)
                            {
                                case SourceTypes.SourceTypeCreature:
                                    //! Get `id` from `creature` and check it for SAI
                                    if (XConverter.ToInt32(entryOrGuid) < 0) //! Guid
                                    {
                                        int entry = await SAI_Editor_Manager.Instance.worldDatabase.GetCreatureIdByGuid(-XConverter.ToInt32(entryOrGuid));
                                        smartScripts = await SAI_Editor_Manager.Instance.worldDatabase.GetSmartScripts(entry, (int)SourceTypes.SourceTypeCreature);

                                        if (smartScripts != null)
                                        {
                                            message += "\n\nA script was not found for this guid but we did find one using the entry of the guid (" + smartScripts[0].entryorguid + "). Do you wish to load this instead?";
                                            DialogResult dialogResult = MessageBox.Show(message, "No scripts found!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                                            if (dialogResult == DialogResult.Yes)
                                            {
                                                this.textBoxEntryOrGuid.Text = smartScripts[0].entryorguid.ToString();
                                                this.comboBoxSourceType.SelectedIndex = this.GetIndexBySourceType(SourceTypes.SourceTypeCreature);
                                                this.TryToLoadScript();
                                            }
                                        }
                                        else
                                            showNormalErrorMessage = true;
                                    }
                                    //! Get all `guid` instances from `creature` for the given `id` and allow user to select a script
                                    else //! Non-guid (entry)
                                    {
                                        int actualEntry = XConverter.ToInt32(entryOrGuid);
                                        List<Creature> creatures = await SAI_Editor_Manager.Instance.worldDatabase.GetCreaturesById(actualEntry);

                                        if (creatures != null)
                                        {
                                            List<List<SmartScript>> creaturesWithSmartAi = new List<List<SmartScript>>();

                                            foreach (Creature creature in creatures)
                                            {
                                                smartScripts = await SAI_Editor_Manager.Instance.worldDatabase.GetSmartScripts(-creature.guid, (int)SourceTypes.SourceTypeCreature);

                                                if (smartScripts != null)
                                                    creaturesWithSmartAi.Add(smartScripts);
                                            }

                                            if (creaturesWithSmartAi.Count > 0)
                                            {
                                                message += "\n\nA script was not found for this entry but we did find script(s) for guid(s) spawned under this entry. Do you wish to select one of these instead? (you can pick one out of all guid-scripts for this entry)";
                                                DialogResult dialogResult = MessageBox.Show(message, "No scripts found!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                                                if (dialogResult == DialogResult.Yes)
                                                    using (SelectSmartScriptForm selectSmartScriptForm = new SelectSmartScriptForm(creaturesWithSmartAi))
                                                        selectSmartScriptForm.ShowDialog(this);
                                            }
                                            else
                                                showNormalErrorMessage = true;
                                        }
                                        else
                                            showNormalErrorMessage = true;
                                    }
                                    break;
                                case SourceTypes.SourceTypeGameobject:
                                    //! Get `id` from `gameobject` and check it for SAI
                                    if (XConverter.ToInt32(entryOrGuid) < 0) //! Guid
                                    {
                                        int entry = await SAI_Editor_Manager.Instance.worldDatabase.GetGameobjectIdByGuid(-XConverter.ToInt32(entryOrGuid));
                                        smartScripts = await SAI_Editor_Manager.Instance.worldDatabase.GetSmartScripts(entry, (int)SourceTypes.SourceTypeGameobject);

                                        if (smartScripts != null)
                                        {
                                            message += "\n\nA script was not found for this guid but we did find one using the entry of the guid (" + smartScripts[0].entryorguid + "). Do you wish to load this instead?";
                                            DialogResult dialogResult = MessageBox.Show(message, "No scripts found!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                                            if (dialogResult == DialogResult.Yes)
                                            {
                                                this.textBoxEntryOrGuid.Text = smartScripts[0].entryorguid.ToString();
                                                this.comboBoxSourceType.SelectedIndex = this.GetIndexBySourceType(SourceTypes.SourceTypeGameobject);
                                                this.TryToLoadScript();
                                            }
                                        }
                                        else
                                            showNormalErrorMessage = true;
                                    }
                                    //! Get all `guid` instances from `gameobject` for the given `id` and allow user to select a script
                                    else //! Non-guid (entry)
                                    {
                                        int actualEntry = XConverter.ToInt32(entryOrGuid);
                                        List<Gameobject> gameobjects = await SAI_Editor_Manager.Instance.worldDatabase.GetGameobjectsById(actualEntry);

                                        if (gameobjects != null)
                                        {
                                            List<List<SmartScript>> gameobjectsWithSmartAi = new List<List<SmartScript>>();

                                            foreach (Gameobject gameobject in gameobjects)
                                            {
                                                smartScripts = await SAI_Editor_Manager.Instance.worldDatabase.GetSmartScripts(-gameobject.guid, (int)SourceTypes.SourceTypeGameobject);

                                                if (smartScripts != null)
                                                    gameobjectsWithSmartAi.Add(smartScripts);
                                            }

                                            if (gameobjectsWithSmartAi.Count > 0)
                                            {
                                                message += "\n\nA script was not found for this entry but we did find script(s) for guid(s) spawned under this entry. Do you wish to select one of these instead? (you can pick one out of all guid-scripts for this entry)";
                                                DialogResult dialogResult = MessageBox.Show(message, "No scripts found!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                                                if (dialogResult == DialogResult.Yes)
                                                    using (SelectSmartScriptForm selectSmartScriptForm = new SelectSmartScriptForm(gameobjectsWithSmartAi))
                                                        selectSmartScriptForm.ShowDialog(this);
                                            }
                                            else
                                                showNormalErrorMessage = true;
                                        }
                                        else
                                            showNormalErrorMessage = true;
                                    }
                                    break;
                                default:
                                    showNormalErrorMessage = true;
                                    break;
                            }
                        }

                        if (showNormalErrorMessage)
                        {
                            if (promptCreateIfNoneFound)
                            {
                                DialogResult dialogResult = MessageBox.Show(message + "\n\nDo you want to create a new script using this entryorguid?", "No scripts found!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                                if (dialogResult == DialogResult.Yes)
                                    this.TryToCreateScript();
                            }
                            else
                                MessageBox.Show(message, "No scripts found!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    this.pictureBoxLoadScript.Enabled = this.textBoxEntryOrGuid.Text.Length > 0 && Settings.Default.UseWorldDatabase;
                    this.pictureBoxCreateScript.Enabled = this.textBoxEntryOrGuid.Text.Length > 0;
                    return new List<SmartScript>();
                }

                for (int i = 0; i < smartScripts.Count; ++i)
                {
                    smartScriptsToReturn.Add(smartScripts[i]);

                    if (!this.checkBoxListActionlistsOrEntries.Checked || !this.checkBoxListActionlistsOrEntries.Enabled)
                        continue;

                    if (i == smartScripts.Count - 1 && this.originalEntryOrGuidAndSourceType.sourceType == SourceTypes.SourceTypeScriptedActionlist)
                    {
                        List<EntryOrGuidAndSourceType> timedActionListOrEntries = await SAI_Editor_Manager.Instance.GetTimedActionlistsOrEntries(smartScripts[i], sourceType);

                        //if (timedActionListOrEntries.sourceTypeOfEntry != SourceTypes.SourceTypeScriptedActionlist)
                        {
                            foreach (EntryOrGuidAndSourceType entryOrGuidAndSourceType in timedActionListOrEntries)
                            {
                                if (entryOrGuidAndSourceType.sourceType == SourceTypes.SourceTypeScriptedActionlist)
                                    continue;

                                List<SmartScript> newSmartScripts = await this.GetSmartScriptsForEntryAndSourceType(entryOrGuidAndSourceType.entryOrGuid.ToString(), entryOrGuidAndSourceType.sourceType);

                                if (newSmartScripts != null)
                                    foreach (SmartScript item in newSmartScripts)
                                        if (!this.ListContainsSmartScript(smartScriptsToReturn, item))
                                            smartScriptsToReturn.Add(item);

                                this.pictureBoxCreateScript.Enabled = this.textBoxEntryOrGuid.Text.Length > 0;
                            }
                        }
                    }

                    if (sourceType == this.originalEntryOrGuidAndSourceType.sourceType && this.originalEntryOrGuidAndSourceType.sourceType != SourceTypes.SourceTypeScriptedActionlist)
                    {
                        List<EntryOrGuidAndSourceType> timedActionListOrEntries = await SAI_Editor_Manager.Instance.GetTimedActionlistsOrEntries(smartScripts[i], sourceType);

                        foreach (EntryOrGuidAndSourceType entryOrGuidAndSourceType in timedActionListOrEntries)
                        {
                            List<SmartScript> newSmartScripts = await this.GetSmartScriptsForEntryAndSourceType(entryOrGuidAndSourceType.entryOrGuid.ToString(), entryOrGuidAndSourceType.sourceType);

                            foreach (SmartScript item in newSmartScripts)
                                if (!this.ListContainsSmartScript(smartScriptsToReturn, item))
                                    smartScriptsToReturn.Add(item);

                            this.pictureBoxCreateScript.Enabled = this.textBoxEntryOrGuid.Text.Length > 0;
                        }
                    }
                }

                foreach (ColumnHeader header in this.listViewSmartScripts.Columns)
                    header.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            catch (Exception ex)
            {
                if (showError)
                    MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.pictureBoxLoadScript.Enabled = this.textBoxEntryOrGuid.Text.Length > 0 && Settings.Default.UseWorldDatabase;
            this.pictureBoxCreateScript.Enabled = this.textBoxEntryOrGuid.Text.Length > 0;
            return smartScriptsToReturn;
        }

        bool ListContainsSmartScript(List<SmartScript> smartScriptsToReturn, SmartScript item)
        {
            foreach (SmartScript itemToReturn in smartScriptsToReturn)
                if (itemToReturn.entryorguid == item.entryorguid && itemToReturn.id == item.id)
                    return true;

            return false;
        }

        private void menuItemExit_Click(object sender, System.EventArgs e)
        {
            if (this.formState == FormState.FormStateMain)
                this.TryCloseApplication();
        }

        private void TryCloseApplication()
        {
            if (!Settings.Default.PromptToQuit || DialogResult.Yes == MessageBox.Show("Are you sure you want to quit?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                this.Close();
        }

        private void menuItemSettings_Click(object sender, EventArgs e)
        {
            if (this.formState != FormState.FormStateMain)
                return;

            using (SettingsForm settingsForm = new SettingsForm())
                settingsForm.ShowDialog(this);
        }

        private void menuItemAbout_Click(object sender, EventArgs e)
        {
            if (this.formState != FormState.FormStateMain)
                return;

            using (AboutForm aboutForm = new AboutForm())
                aboutForm.ShowDialog(this);
        }

        private void listViewSmartScripts_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            this.menuItemDeleteSelectedRow.Enabled = this.listViewSmartScripts.SelectedItems.Count > 0;
            this.menuItemGenerateSql.Enabled = this.listViewSmartScripts.SelectedItems.Count > 0;
            this.buttonGenerateSql.Enabled = this.listViewSmartScripts.SelectedItems.Count > 0;
            this.menuitemLoadSelectedEntry.Enabled = this.listViewSmartScripts.SelectedItems.Count > 0;
            this.menuItemDuplicateRow.Enabled = this.listViewSmartScripts.SelectedItems.Count > 0;
            this.menuItemGenerateComment.Enabled = this.listViewSmartScripts.SelectedItems.Count > 0;
            this.menuItemCopySelectedRow.Enabled = this.listViewSmartScripts.SelectedItems.Count > 0;

            if (!e.IsSelected)
                return;

            this.FillFieldsBasedOnSelectedScript();

            if (Settings.Default.ChangeStaticInfo)
                this.checkBoxListActionlistsOrEntries.Text = this.listViewSmartScripts.SelectedItems[0].SubItems[1].Text == "9" ? "List entries too" : "List actionlists too";
        }

        private void FillFieldsBasedOnSelectedScript()
        {
            try
            {
                this.updatingFieldsBasedOnSelectedScript = true;
                SmartScript selectedScript = this.listViewSmartScripts.SelectedSmartScript;

                if (Settings.Default.ChangeStaticInfo)
                {
                    this.textBoxEntryOrGuid.Text = selectedScript.entryorguid.ToString();
                    this.comboBoxSourceType.SelectedIndex = this.GetIndexBySourceType((SourceTypes)selectedScript.source_type);
                }

                this.textBoxId.Text = selectedScript.id.ToString();
                this.textBoxLinkTo.Text = selectedScript.link.ToString();
                this.textBoxLinkFrom.Text = this.GetLinkFromForSelection();

                int event_type = selectedScript.event_type;
                this.comboBoxEventType.SelectedIndex = event_type;
                this.textBoxEventPhasemask.Text = selectedScript.event_phase_mask.ToString();
                this.textBoxEventChance.Text = selectedScript.event_chance.ToString();
                this.textBoxEventFlags.Text = selectedScript.event_flags.ToString();

                //! Event parameters
                this.textBoxEventParam1.Text = selectedScript.event_param1.ToString();
                this.textBoxEventParam2.Text = selectedScript.event_param2.ToString();
                this.textBoxEventParam3.Text = selectedScript.event_param3.ToString();
                this.textBoxEventParam4.Text = selectedScript.event_param4.ToString();
                this.labelEventParam1.Text = SAI_Editor_Manager.Instance.GetParameterStringById(event_type, 1, ScriptTypeId.ScriptTypeEvent);
                this.labelEventParam2.Text = SAI_Editor_Manager.Instance.GetParameterStringById(event_type, 2, ScriptTypeId.ScriptTypeEvent);
                this.labelEventParam3.Text = SAI_Editor_Manager.Instance.GetParameterStringById(event_type, 3, ScriptTypeId.ScriptTypeEvent);
                this.labelEventParam4.Text = SAI_Editor_Manager.Instance.GetParameterStringById(event_type, 4, ScriptTypeId.ScriptTypeEvent);

                if (!Settings.Default.ShowTooltipsPermanently)
                {
                    this.AddTooltip(this.comboBoxEventType, this.comboBoxEventType.SelectedItem.ToString(), SAI_Editor_Manager.Instance.GetScriptTypeTooltipById(event_type, ScriptTypeId.ScriptTypeEvent));
                    this.AddTooltip(this.labelEventParam1, this.labelEventParam1.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(event_type, 1, ScriptTypeId.ScriptTypeEvent));
                    this.AddTooltip(this.labelEventParam2, this.labelEventParam2.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(event_type, 2, ScriptTypeId.ScriptTypeEvent));
                    this.AddTooltip(this.labelEventParam3, this.labelEventParam3.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(event_type, 3, ScriptTypeId.ScriptTypeEvent));
                    this.AddTooltip(this.labelEventParam4, this.labelEventParam4.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(event_type, 4, ScriptTypeId.ScriptTypeEvent));
                }

                //! Action parameters
                int action_type = selectedScript.action_type;
                this.comboBoxActionType.SelectedIndex = action_type;
                this.textBoxActionParam1.Text = selectedScript.action_param1.ToString();
                this.textBoxActionParam2.Text = selectedScript.action_param2.ToString();
                this.textBoxActionParam3.Text = selectedScript.action_param3.ToString();
                this.textBoxActionParam4.Text = selectedScript.action_param4.ToString();
                this.textBoxActionParam5.Text = selectedScript.action_param5.ToString();
                this.textBoxActionParam6.Text = selectedScript.action_param6.ToString();
                this.labelActionParam1.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 1, ScriptTypeId.ScriptTypeAction);
                this.labelActionParam2.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 2, ScriptTypeId.ScriptTypeAction);
                this.labelActionParam3.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 3, ScriptTypeId.ScriptTypeAction);
                this.labelActionParam4.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 4, ScriptTypeId.ScriptTypeAction);
                this.labelActionParam5.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 5, ScriptTypeId.ScriptTypeAction);
                this.labelActionParam6.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 6, ScriptTypeId.ScriptTypeAction);

                if (!Settings.Default.ShowTooltipsPermanently)
                {
                    this.AddTooltip(this.comboBoxActionType, this.comboBoxActionType.SelectedItem.ToString(), SAI_Editor_Manager.Instance.GetScriptTypeTooltipById(action_type, ScriptTypeId.ScriptTypeAction));
                    this.AddTooltip(this.labelActionParam1, this.labelActionParam1.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 1, ScriptTypeId.ScriptTypeAction));
                    this.AddTooltip(this.labelActionParam2, this.labelActionParam2.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 2, ScriptTypeId.ScriptTypeAction));
                    this.AddTooltip(this.labelActionParam3, this.labelActionParam3.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 3, ScriptTypeId.ScriptTypeAction));
                    this.AddTooltip(this.labelActionParam4, this.labelActionParam4.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 4, ScriptTypeId.ScriptTypeAction));
                    this.AddTooltip(this.labelActionParam5, this.labelActionParam5.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 5, ScriptTypeId.ScriptTypeAction));
                    this.AddTooltip(this.labelActionParam6, this.labelActionParam6.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 6, ScriptTypeId.ScriptTypeAction));
                }

                //! Target parameters
                int target_type = selectedScript.target_type;
                this.comboBoxTargetType.SelectedIndex = target_type;
                this.textBoxTargetParam1.Text = selectedScript.target_param1.ToString();
                this.textBoxTargetParam2.Text = selectedScript.target_param2.ToString();
                this.textBoxTargetParam3.Text = selectedScript.target_param3.ToString();
                this.labelTargetParam1.Text = SAI_Editor_Manager.Instance.GetParameterStringById(target_type, 1, ScriptTypeId.ScriptTypeTarget);
                this.labelTargetParam2.Text = SAI_Editor_Manager.Instance.GetParameterStringById(target_type, 2, ScriptTypeId.ScriptTypeTarget);
                this.labelTargetParam3.Text = SAI_Editor_Manager.Instance.GetParameterStringById(target_type, 3, ScriptTypeId.ScriptTypeTarget);

                if (!Settings.Default.ShowTooltipsPermanently)
                {
                    this.AddTooltip(this.comboBoxTargetType, this.comboBoxTargetType.SelectedItem.ToString(), SAI_Editor_Manager.Instance.GetScriptTypeTooltipById(target_type, ScriptTypeId.ScriptTypeTarget));
                    this.AddTooltip(this.labelTargetParam1, this.labelTargetParam1.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(target_type, 1, ScriptTypeId.ScriptTypeTarget));
                    this.AddTooltip(this.labelTargetParam2, this.labelTargetParam2.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(target_type, 2, ScriptTypeId.ScriptTypeTarget));
                    this.AddTooltip(this.labelTargetParam3, this.labelTargetParam3.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(target_type, 3, ScriptTypeId.ScriptTypeTarget));
                }

                this.textBoxTargetX.Text = selectedScript.target_x.ToString();
                this.textBoxTargetY.Text = selectedScript.target_y.ToString();
                this.textBoxTargetZ.Text = selectedScript.target_z.ToString();
                this.textBoxTargetO.Text = selectedScript.target_o.ToString();
                this.textBoxComments.Text = selectedScript.comment;

                this.AdjustAllParameterFields(event_type, action_type, target_type);
                this.updatingFieldsBasedOnSelectedScript = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetLinkFromForSelection()
        {
            SmartScript selectedScript = this.listViewSmartScripts.SelectedSmartScript;

            foreach (SmartScript smartScript in this.listViewSmartScripts.SmartScripts)
            {
                if (smartScript.entryorguid != selectedScript.entryorguid || smartScript.source_type != selectedScript.source_type)
                    continue;

                if (smartScript.link > 0 && smartScript.link == this.listViewSmartScripts.SelectedSmartScript.id)
                    return smartScript.id.ToString();
            }

            return "None";
        }

        private void AdjustAllParameterFields(int event_type, int action_type, int target_type)
        {
            this.SetVisibilityOfAllParamButtons(false);

            switch ((SmartEvent)event_type)
            {
                case SmartEvent.SMART_EVENT_SPELLHIT: //! Spell entry & Spell school
                case SmartEvent.SMART_EVENT_SPELLHIT_TARGET: //! Spell entry & Spell school
                case SmartEvent.SMART_EVENT_GOSSIP_SELECT: //! Gossip menu id & gossip id
                    this.buttonEventParamOneSearch.Visible = true;
                    this.buttonEventParamTwoSearch.Visible = true;
                    break;
                case SmartEvent.SMART_EVENT_RESPAWN:
                    this.buttonEventParamOneSearch.Visible = true; //! Respawn condition (SMART_SCRIPT_RESPAWN_CONDITION_MAP / SMART_SCRIPT_RESPAWN_CONDITION_AREA)
                    this.buttonEventParamTwoSearch.Visible = true; //! Map entry
                    this.buttonEventParamThreeSearch.Visible = true; //! Zone entry
                    break;
                case SmartEvent.SMART_EVENT_AREATRIGGER_ONTRIGGER: //! Areatrigger entry
                case SmartEvent.SMART_EVENT_GO_STATE_CHANGED: //! Go state
                case SmartEvent.SMART_EVENT_GAME_EVENT_START: //! Game event entry
                case SmartEvent.SMART_EVENT_GAME_EVENT_END: //! Game event entry
                case SmartEvent.SMART_EVENT_MOVEMENTINFORM: //! Movement type
                case SmartEvent.SMART_EVENT_FRIENDLY_MISSING_BUFF: //! Spell id
                case SmartEvent.SMART_EVENT_HAS_AURA: //! Spell id
                case SmartEvent.SMART_EVENT_TARGET_BUFFED: //! Spell id
                case SmartEvent.SMART_EVENT_SUMMON_DESPAWNED: //! Creature entry
                case SmartEvent.SMART_EVENT_SUMMONED_UNIT: //! Creature entry
                case SmartEvent.SMART_EVENT_ACCEPTED_QUEST: //! Quest id
                case SmartEvent.SMART_EVENT_REWARD_QUEST: //! Quest id
                case SmartEvent.SMART_EVENT_RECEIVE_EMOTE: //! Emote id
                    this.buttonEventParamOneSearch.Visible = true;
                    break;
                case SmartEvent.SMART_EVENT_TEXT_OVER: //! Creature entry
                    this.buttonEventParamTwoSearch.Visible = true;
                    break;
                case SmartEvent.SMART_EVENT_VICTIM_CASTING: //! Spell id
                    this.buttonEventParamThreeSearch.Visible = true;
                    break;
                case SmartEvent.SMART_EVENT_KILL: //! Creature entry
                    this.buttonEventParamFourSearch.Visible = true;
                    break;
            }

            switch ((SmartAction)action_type)
            {
                case SmartAction.SMART_ACTION_CAST: //! Spell entry & Cast flags
                case SmartAction.SMART_ACTION_INVOKER_CAST: //! Spell entry & Cast flags
                case SmartAction.SMART_ACTION_CALL_CASTEDCREATUREORGO: //! Creature entry & Spell entry
                case SmartAction.SMART_ACTION_SUMMON_CREATURE: //! Creature entry & Summon type
                case SmartAction.SMART_ACTION_SET_UNIT_FIELD_BYTES_1: //! Bytes1flags & Type
                case SmartAction.SMART_ACTION_REMOVE_UNIT_FIELD_BYTES_1: //! Bytes1flags & Type
                case SmartAction.SMART_ACTION_RANDOM_PHASE_RANGE: //! Event phase 1 & 2
                    this.buttonActionParamOneSearch.Visible = true;
                    this.buttonActionParamTwoSearch.Visible = true;
                    break;
                case SmartAction.SMART_ACTION_CROSS_CAST:
                    this.buttonActionParamOneSearch.Visible = true; //! Spell entry
                    this.buttonActionParamTwoSearch.Visible = true; //! Cast flags
                    this.buttonActionParamThreeSearch.Visible = true; //! Target type
                    break;
                case SmartAction.SMART_ACTION_WP_STOP: //! Quest entry
                case SmartAction.SMART_ACTION_INTERRUPT_SPELL: //! Spell entry
                case SmartAction.SMART_ACTION_SEND_GOSSIP_MENU: //! Gossip menu id & npc_text.id
                case SmartAction.SMART_ACTION_CALL_TIMED_ACTIONLIST: //! Timer type
                    this.buttonActionParamTwoSearch.Visible = true;
                    break;
                case SmartAction.SMART_ACTION_WP_START:
                    this.buttonActionParamTwoSearch.Visible = true; //! Waypoint entry
                    this.buttonActionParamFourSearch.Visible = true; //! Quest entry
                    this.buttonActionParamSixSearch.Visible = true; //! React state
                    break;
                case SmartAction.SMART_ACTION_FOLLOW:
                    this.buttonActionParamThreeSearch.Visible = true; //! Creature entry
                    this.buttonActionParamFourSearch.Visible = true; //! Creature entry
                    break;
                case SmartAction.SMART_ACTION_RANDOM_PHASE:  //! Event phase 1-6
                case SmartAction.SMART_ACTION_RANDOM_EMOTE: //! Emote entry 1-6
                    this.buttonActionParamOneSearch.Visible = true;
                    this.buttonActionParamTwoSearch.Visible = true;
                    this.buttonActionParamThreeSearch.Visible = true;
                    this.buttonActionParamFourSearch.Visible = true;
                    this.buttonActionParamFiveSearch.Visible = true;
                    this.buttonActionParamSixSearch.Visible = true;
                    break;
                case SmartAction.SMART_ACTION_EQUIP:
                    this.buttonActionParamOneSearch.Visible = true; //! Equipment entry
                    this.buttonActionParamThreeSearch.Visible = true; //! Item entry 1
                    this.buttonActionParamFourSearch.Visible = true; //! Item entry 2
                    this.buttonActionParamFiveSearch.Visible = true; //! Item entry 3
                    break;
                case SmartAction.SMART_ACTION_SET_FACTION: //! Faction entry
                case SmartAction.SMART_ACTION_EMOTE: //! Emote entry
                case SmartAction.SMART_ACTION_SET_EMOTE_STATE: //! Emote entry
                case SmartAction.SMART_ACTION_FAIL_QUEST: //! Quest entry
                case SmartAction.SMART_ACTION_ADD_QUEST: //! Quest entry
                case SmartAction.SMART_ACTION_CALL_AREAEXPLOREDOREVENTHAPPENS: //! Quest entry
                case SmartAction.SMART_ACTION_CALL_GROUPEVENTHAPPENS: //! Quest entry
                case SmartAction.SMART_ACTION_SET_REACT_STATE: //! Reactstate
                case SmartAction.SMART_ACTION_SOUND: //! Sound entry
                case SmartAction.SMART_ACTION_MORPH_TO_ENTRY_OR_MODEL: //! Creature entry
                case SmartAction.SMART_ACTION_KILLED_MONSTER: //! Creature entry
                case SmartAction.SMART_ACTION_UPDATE_TEMPLATE: //! Creature entry
                case SmartAction.SMART_ACTION_MOUNT_TO_ENTRY_OR_MODEL: //! Creature entry
                case SmartAction.SMART_ACTION_GO_SET_LOOT_STATE: //! Gameobject state
                case SmartAction.SMART_ACTION_SET_POWER: //! Power type
                case SmartAction.SMART_ACTION_ADD_POWER: //! Power type
                case SmartAction.SMART_ACTION_REMOVE_POWER: //! Power type
                case SmartAction.SMART_ACTION_SUMMON_GO: //! Gameobject entry
                case SmartAction.SMART_ACTION_SET_EVENT_PHASE: //! Event phase
                case SmartAction.SMART_ACTION_SET_PHASE_MASK: //! Ingame phase
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
                case SmartAction.SMART_ACTION_SET_NPC_FLAG: //! Npc flags
                case SmartAction.SMART_ACTION_ADD_NPC_FLAG: //! Npc flags
                case SmartAction.SMART_ACTION_REMOVE_NPC_FLAG: //! Npc flags
                case SmartAction.SMART_ACTION_INSTALL_AI_TEMPLATE: //! AI template
                    this.buttonActionParamOneSearch.Visible = true;
                    break;
            }

            switch ((SmartTarget)target_type)
            {
                case SmartTarget.SMART_TARGET_CREATURE_GUID:
                    this.buttonTargetParamOneSearch.Visible = true; //! Creature guid
                    this.buttonTargetParamTwoSearch.Visible = true; //! Creature entry
                    break;
                case SmartTarget.SMART_TARGET_GAMEOBJECT_GUID:
                    this.buttonTargetParamOneSearch.Visible = true; //! Gameobject guid
                    this.buttonTargetParamTwoSearch.Visible = true; //! Gameobject entry
                    break;
                case SmartTarget.SMART_TARGET_CREATURE_RANGE: //! Creature entry
                case SmartTarget.SMART_TARGET_CREATURE_DISTANCE: //! Creature entry
                case SmartTarget.SMART_TARGET_CLOSEST_CREATURE: //! Creature entry
                case SmartTarget.SMART_TARGET_GAMEOBJECT_RANGE: //! Gameobject entry
                case SmartTarget.SMART_TARGET_GAMEOBJECT_DISTANCE: //! Gameobject entry
                case SmartTarget.SMART_TARGET_CLOSEST_GAMEOBJECT: //! Gameobject entry
                    this.buttonTargetParamOneSearch.Visible = true;
                    break;
            }
        }

        private void AddTooltip(Control control, string title, string text, ToolTipIcon icon = ToolTipIcon.Info, bool isBallon = true, bool active = true, int autoPopDelay = 2100000000, bool showAlways = true)
        {
            if (String.IsNullOrWhiteSpace(title) || String.IsNullOrWhiteSpace(text))
            {
                DetailedToolTip toolTipExistent = ToolTipHelper.GetExistingToolTip(control);

                if (toolTipExistent != null)
                    toolTipExistent.Active = false;

                return;
            }

            DetailedToolTip toolTip = ToolTipHelper.GetControlToolTip(control);
            toolTip.ToolTipIcon = icon;
            toolTip.ToolTipTitle = title;
            toolTip.IsBalloon = isBallon;
            toolTip.Active = active;
            toolTip.AutoPopDelay = autoPopDelay;
            toolTip.ShowAlways = showAlways;
            toolTip.SetToolTipText(control, text);
        }

        private void AddTooltip(string controlName, string title, string text, ToolTipIcon icon = ToolTipIcon.Info, bool isBallon = true, bool active = true, int autoPopDelay = 2100000000, bool showAlways = true)
        {
            Control[] controls = this.Controls.Find(controlName, true);

            if (controls.Length > 0)
                foreach (Control control in controls)
                    this.AddTooltip(control, title, text, icon, isBallon, active, autoPopDelay, showAlways);
        }

        private void textBoxEventTypeId_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.textBoxEventType.Text))
            {
                this.comboBoxEventType.SelectedIndex = 0;
                this.textBoxEventType.Text = "0";
                this.textBoxEventType.SelectionStart = 3; //! Set cursor position to end of the line
            }
            else
            {
                int eventType;
                Int32.TryParse(this.textBoxEventType.Text, out eventType);

                if (eventType > (int)MaxValues.MaxEventType)
                {
                    this.comboBoxEventType.SelectedIndex = (int)MaxValues.MaxEventType;
                    this.textBoxEventType.Text = ((int)MaxValues.MaxEventType).ToString();
                    this.textBoxEventType.SelectionStart = 3; //! Set cursor position to end of the line
                }
                else
                    this.comboBoxEventType.SelectedIndex = eventType;
            }
        }

        private void textBoxActionTypeId_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.textBoxActionType.Text))
            {
                this.comboBoxActionType.SelectedIndex = 0;
                this.textBoxActionType.Text = "0";
                this.textBoxActionType.SelectionStart = 3; //! Set cursor position to end of the line
            }
            else
            {
                int actionType;
                Int32.TryParse(this.textBoxActionType.Text, out actionType);

                if (actionType > (int)MaxValues.MaxActionType)
                {
                    this.comboBoxActionType.SelectedIndex = (int)MaxValues.MaxActionType;
                    this.textBoxActionType.Text = ((int)MaxValues.MaxActionType).ToString();
                    this.textBoxActionType.SelectionStart = 3; //! Set cursor position to end of the line
                }
                else
                    this.comboBoxActionType.SelectedIndex = actionType;
            }
        }

        private void textBoxTargetTypeId_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.textBoxTargetType.Text))
            {
                this.comboBoxTargetType.SelectedIndex = 0;
                this.textBoxTargetType.Text = "0";
                this.textBoxTargetType.SelectionStart = 3; //! Set cursor position to end of the line
            }
            else
            {
                int targetType;
                Int32.TryParse(this.textBoxTargetType.Text, out targetType);

                if (targetType > (int)MaxValues.MaxTargetType)
                {
                    this.comboBoxTargetType.SelectedIndex = (int)MaxValues.MaxTargetType;
                    this.textBoxTargetType.Text = ((int)MaxValues.MaxTargetType).ToString();
                    this.textBoxTargetType.SelectionStart = 3; //! Set cursor position to end of the line
                }
                else
                    this.comboBoxTargetType.SelectedIndex = targetType;
            }
        }

        private void menuOptionDeleteSelectedRow_Click(object sender, EventArgs e)
        {
            if (this.formState != FormState.FormStateMain || this.listViewSmartScripts.SelectedSmartScript == null)
                return;

            if (this.listViewSmartScripts.SelectedItems.Count <= 0)
            {
                MessageBox.Show("No rows were selected to delete!", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.DeleteSelectedRow();
        }

        private void menuItemCopySelectedRowListView_Click(object sender, EventArgs e)
        {
            if (this.formState != FormState.FormStateMain || this.listViewSmartScripts.SelectedSmartScript == null)
                return;

            this.smartScriptsOnClipBoard.Add(this.listViewSmartScripts.SelectedSmartScript.Clone());
        }

        private void menuItemPasteLastCopiedRow_Click(object sender, EventArgs e)
        {
            if (this.formState != FormState.FormStateMain || this.listViewSmartScripts.SelectedSmartScript == null)
                return;

            if (this.smartScriptsOnClipBoard.Count <= 0)
            {
                MessageBox.Show("No smart scripts have been copied in this session!", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SmartScript newSmartScript = this.smartScriptsOnClipBoard.Last().Clone();
            this.listViewSmartScripts.AddSmartScript(newSmartScript);
        }

        private void DeleteSelectedRow()
        {
            if (this.listViewSmartScripts.SelectedItems.Count == 0)
                return;

            int prevSelectedIndex = this.listViewSmartScripts.SelectedItems[0].Index;

            if (this.listViewSmartScripts.SelectedItems[0].SubItems[0].Text == this.originalEntryOrGuidAndSourceType.entryOrGuid.ToString())
                if (this.listViewSmartScripts.SelectedItems[0].SubItems[2].Text == this.lastSmartScriptIdOfScript.ToString())
                    this.lastSmartScriptIdOfScript--;

            this.lastDeletedSmartScripts.Add(this.listViewSmartScripts.SelectedSmartScript.Clone());
            this.listViewSmartScripts.RemoveSmartScript(this.listViewSmartScripts.SelectedSmartScript);
            this.SetGenerateCommentsEnabled(this.listViewSmartScripts.Items.Count > 0 && Settings.Default.UseWorldDatabase);

            if (this.listViewSmartScripts.Items.Count <= 0)
                this.ResetFieldsToDefault(Settings.Default.ChangeStaticInfo);
            else
                this.ReSelectListViewItemWithPrevIndex(prevSelectedIndex);

            //! Need to do this if static info is changed
            this.pictureBoxCreateScript.Enabled = this.textBoxEntryOrGuid.Text.Length > 0;
        }

        private void SetGenerateCommentsEnabled(bool enabled)
        {
            this.buttonGenerateComments.Enabled = enabled;
            this.menuItemGenerateComment.Enabled = enabled;
        }

        private void ReSelectListViewItemWithPrevIndex(int prevIndex)
        {
            if (this.listViewSmartScripts.Items.Count > prevIndex)
                this.listViewSmartScripts.Items[prevIndex].Selected = true;
            else
                this.listViewSmartScripts.Items[prevIndex - 1].Selected = true;
        }

        private async void checkBoxListActionlists_CheckedChanged(object sender, EventArgs e)
        {
            if (this.listViewSmartScripts.Items.Count == 0)
                return;

            this.buttonGenerateSql.Enabled = false;
            this.menuItemGenerateSql.Enabled = false;
            int prevSelectedIndex = this.listViewSmartScripts.SelectedItems.Count > 0 ? this.listViewSmartScripts.SelectedItems[0].Index : 0;

            if (this.checkBoxListActionlistsOrEntries.Checked)
            {
                List<SmartScript> smartScripts = await this.GetSmartScriptsForEntryAndSourceType(this.originalEntryOrGuidAndSourceType.entryOrGuid.ToString(), this.originalEntryOrGuidAndSourceType.sourceType);
                List<SmartScript> newSmartScripts = new List<SmartScript>();

                //! Only add the new smartscript if it doesn't yet exist
                foreach (SmartScript newSmartScript in smartScripts)
                    if (!this.listViewSmartScripts.Items.Cast<SmartScriptListViewItem>().Any(p => p.Script.entryorguid == newSmartScript.entryorguid && p.Script.id == newSmartScript.id))
                        this.listViewSmartScripts.AddSmartScript(newSmartScript);

                this.pictureBoxCreateScript.Enabled = this.textBoxEntryOrGuid.Text.Length > 0;
            }
            else
                this.RemoveNonOriginalScriptsFromView();

            this.HandleShowBasicInfo();

            if (this.listViewSmartScripts.Items.Count > prevSelectedIndex)
                this.listViewSmartScripts.Items[prevSelectedIndex].Selected = true;

            this.buttonGenerateSql.Enabled = this.listViewSmartScripts.Items.Count > 0;
            this.menuItemGenerateSql.Enabled = this.listViewSmartScripts.Items.Count > 0;
        }

        private void RemoveNonOriginalScriptsFromView()
        {
            List<SmartScript> smartScriptsToRemove = new List<SmartScript>();

            foreach (SmartScript smartScript in this.listViewSmartScripts.SmartScripts)
                if (smartScript.source_type != (int)this.originalEntryOrGuidAndSourceType.sourceType)
                    smartScriptsToRemove.Add(smartScript);

            foreach (SmartScript smartScript in smartScriptsToRemove)
                this.listViewSmartScripts.SmartScripts.Remove(smartScript);
        }

        public SourceTypes GetSourceTypeByIndex()
        {
            switch (this.comboBoxSourceType.SelectedIndex)
            {
                case 0: //! Creature
                case 1: //! Gameobject
                case 2: //! Areatrigger
                    return (SourceTypes)this.comboBoxSourceType.SelectedIndex;
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
            if (!this.pictureBoxLoadScript.Enabled || !Settings.Default.UseWorldDatabase)
                return;

            this.TryToLoadScript();
        }

        private void pictureBoxCreateScript_Click(object sender, EventArgs e)
        {
            if (!this.pictureBoxCreateScript.Enabled)
                return;

            if (String.IsNullOrWhiteSpace(this.textBoxEntryOrGuid.Text) || this.comboBoxSourceType.SelectedIndex == -1)
                return;

            this.TryToCreateScript();
        }

        public async void TryToCreateScript(bool fromNewLine = false)
        {
            if (this.listViewSmartScripts.Items.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show("There is already a script loaded at this moment. Do you want to overwrite this?\n\nWarning: overwriting means local unsaved changes will also be discarded!", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult != DialogResult.Yes)
                    return;

                this.ResetFieldsToDefault();
            }

            int entryorguid = 0;

            try
            {
                entryorguid = Int32.Parse(this.textBoxEntryOrGuid.Text);
            }
            catch (OverflowException)
            {
                MessageBox.Show("The entryorguid is either too big or too small.", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (FormatException)
            {
                MessageBox.Show("The entryorguid field does not contain a valid number.", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.lastSmartScriptIdOfScript = 0;
            int source_type = (int)this.GetSourceTypeByIndex();
            string sourceTypeString = this.GetSourceTypeString((SourceTypes)source_type);

            if (!Settings.Default.UseWorldDatabase)
                goto SkipWorldDatabaseChecks;

            string aiName = await SAI_Editor_Manager.Instance.worldDatabase.GetObjectAiName(entryorguid, source_type);
            List<SmartScript> smartScripts = await SAI_Editor_Manager.Instance.worldDatabase.GetSmartScripts(entryorguid, source_type);

            //! Allow adding new lines even if the AIName is already set
            if ((SourceTypes)source_type == SourceTypes.SourceTypeAreaTrigger)
            {
                if (aiName != String.Empty)
                {
                    string errorMessage = "This areatrigger already has its ";

                    if (aiName != "SmartTrigger")
                        errorMessage += "ScriptName set to '" + aiName + "'";
                    else
                        errorMessage += "AIName set (for SmartAI)! Do you want to load it instead?";

                    DialogResult dialogResult = MessageBox.Show(errorMessage, "Something went wrong", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (dialogResult == DialogResult.Yes)
                        this.TryToLoadScript();

                    return;
                }
            }
            else
            {
                if (aiName != String.Empty)
                {
                    string strAlreadyHasAiName = String.Empty;
                    bool aiNameIsSmart = SAI_Editor_Manager.Instance.IsAiNameSmartAi(aiName);

                    if (aiNameIsSmart)
                    {
                        if (smartScripts == null || smartScripts.Count == 0)
                            goto SkipWorldDatabaseChecks;

                        if (fromNewLine)
                            goto SkipAiNameAndScriptNameChecks;

                        strAlreadyHasAiName += "This " + sourceTypeString + " already has its AIName set to '" + aiName + "'";
                        strAlreadyHasAiName += "! Do you want to load it instead?";
                    }
                    else
                    {
                        strAlreadyHasAiName += "This " + sourceTypeString + " already has its AIName set to '" + aiName + "'";
                        strAlreadyHasAiName += " and can therefore not have any SmartAI. Do you want to get rid of this AIName right now?";
                    }

                    DialogResult dialogResult = MessageBox.Show(strAlreadyHasAiName, "Something went wrong", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (dialogResult == DialogResult.Yes)
                    {
                        if (!aiNameIsSmart)
                        {
                            //! We don't have to target areatrigger_scripts here, as we've already done this a few lines up
                            string sqlOutput = "UPDATE `" + this.GetTemplateTableBySourceType((SourceTypes)source_type) + "` SET `AIName`=" + '"' + '"' + " WHERE `entry`=" + entryorguid + ";\n";

                            using (SqlOutputForm sqlOutputForm = new SqlOutputForm(sqlOutput))
                                sqlOutputForm.ShowDialog(this);
                        }
                        else
                            this.TryToLoadScript();
                    }

                    return;
                }

                string scriptName = await SAI_Editor_Manager.Instance.worldDatabase.GetObjectScriptName(entryorguid, source_type);

                if (scriptName != String.Empty)
                {
                    MessageBox.Show("This " + sourceTypeString + " already has a ScriptName set (to '" + scriptName + "')!", "Something went wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

        SkipAiNameAndScriptNameChecks:

            if (smartScripts != null && smartScripts.Count > 0)
            {
                string errorMessage = "This " + sourceTypeString + " already has smart scripts";// (without its AIName set to SmartAI)! Do you want to load it instead?";

                if ((SourceTypes)source_type != SourceTypes.SourceTypeScriptedActionlist)
                    errorMessage += " (without its AIName set to SmartAI)!";
                else
                    errorMessage += "!";

                errorMessage += " Do you want to load it instead?";
                DialogResult dialogResult = MessageBox.Show(errorMessage, "Something went wrong", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.Yes)
                    this.TryToLoadScript();

                return;
            }

        SkipWorldDatabaseChecks:
            this.buttonNewLine.Enabled = false;
            this.checkBoxListActionlistsOrEntries.Text = this.GetSourceTypeByIndex() == SourceTypes.SourceTypeScriptedActionlist ? "List entries too" : "List actionlists too";
            this.pictureBoxLoadScript.Enabled = false;
            this.pictureBoxCreateScript.Enabled = false;

            this.originalEntryOrGuidAndSourceType.entryOrGuid = entryorguid;
            this.originalEntryOrGuidAndSourceType.sourceType = (SourceTypes)source_type;

            this.listViewSmartScripts.ReplaceSmartScripts(new List<SmartScript>());

            SmartScript newSmartScript = new SmartScript();
            newSmartScript.entryorguid = entryorguid;
            newSmartScript.source_type = source_type;

            if (this.checkBoxLockEventId.Checked)
                newSmartScript.id = 0;
            else
                newSmartScript.id = -1;

            newSmartScript.link = XConverter.ToInt32(this.textBoxLinkTo.Text);
            newSmartScript.event_type = XConverter.ToInt32(this.textBoxEventType.Text);
            newSmartScript.event_phase_mask = XConverter.ToInt32(this.textBoxEventPhasemask.Text);
            newSmartScript.event_chance = XConverter.ToInt32(this.textBoxEventChance.Value);
            newSmartScript.event_flags = XConverter.ToInt32(this.textBoxEventFlags.Text);
            newSmartScript.event_param1 = XConverter.ToInt32(this.textBoxEventParam1.Text);
            newSmartScript.event_param2 = XConverter.ToInt32(this.textBoxEventParam2.Text);
            newSmartScript.event_param3 = XConverter.ToInt32(this.textBoxEventParam3.Text);
            newSmartScript.event_param4 = XConverter.ToInt32(this.textBoxEventParam4.Text);
            newSmartScript.action_type = XConverter.ToInt32(this.textBoxActionType.Text);
            newSmartScript.action_param1 = XConverter.ToInt32(this.textBoxActionParam1.Text);
            newSmartScript.action_param2 = XConverter.ToInt32(this.textBoxActionParam2.Text);
            newSmartScript.action_param3 = XConverter.ToInt32(this.textBoxActionParam3.Text);
            newSmartScript.action_param4 = XConverter.ToInt32(this.textBoxActionParam4.Text);
            newSmartScript.action_param5 = XConverter.ToInt32(this.textBoxActionParam5.Text);
            newSmartScript.action_param6 = XConverter.ToInt32(this.textBoxActionParam6.Text);
            newSmartScript.target_type = XConverter.ToInt32(this.textBoxTargetType.Text);
            newSmartScript.target_param1 = XConverter.ToInt32(this.textBoxTargetParam1.Text);
            newSmartScript.target_param2 = XConverter.ToInt32(this.textBoxTargetParam2.Text);
            newSmartScript.target_param3 = XConverter.ToInt32(this.textBoxTargetParam3.Text);
            newSmartScript.target_x = XConverter.ToDouble(this.textBoxTargetX.Text);
            newSmartScript.target_y = XConverter.ToDouble(this.textBoxTargetY.Text);
            newSmartScript.target_z = XConverter.ToDouble(this.textBoxTargetZ.Text);
            newSmartScript.target_o = XConverter.ToDouble(this.textBoxTargetO.Text);

            if (Settings.Default.GenerateComments && Settings.Default.UseWorldDatabase)
                newSmartScript.comment = await CommentGenerator.Instance.GenerateCommentFor(newSmartScript, this.originalEntryOrGuidAndSourceType);
            else if (this.textBoxComments.Text.Contains(" - Event - Action (phase) (dungeon difficulty)"))
                newSmartScript.comment = SAI_Editor_Manager.Instance.GetDefaultCommentForSourceType((SourceTypes)newSmartScript.source_type);
            else
                newSmartScript.comment = this.textBoxComments.Text;

            this.listViewSmartScripts.AddSmartScript(newSmartScript);

            this.HandleShowBasicInfo();

            this.listViewSmartScripts.Items[0].Selected = true;
            this.listViewSmartScripts.Select();

            this.buttonNewLine.Enabled = this.textBoxEntryOrGuid.Text.Length > 0;
            this.SetGenerateCommentsEnabled(Settings.Default.UseWorldDatabase);
            this.pictureBoxLoadScript.Enabled = this.textBoxEntryOrGuid.Text.Length > 0 && Settings.Default.UseWorldDatabase;
            this.pictureBoxCreateScript.Enabled = this.textBoxEntryOrGuid.Text.Length > 0;
        }

        private string GetTemplateTableBySourceType(SourceTypes sourceType)
        {
            switch (sourceType)
            {
                case SourceTypes.SourceTypeCreature:
                    return "creature_template";
                case SourceTypes.SourceTypeGameobject:
                    return "gameobject_template";
            }

            return "<unknown template table>";
        }

        public async void TryToLoadScript(int entryorguid = -1, SourceTypes sourceType = SourceTypes.SourceTypeNone, bool showErrorIfNoneFound = true, bool promptCreateIfNoneFound = false)
        {
            this.listViewSmartScripts.ReplaceSmartScripts(new List<SmartScript>());
            this.ResetFieldsToDefault();

            if (String.IsNullOrEmpty(this.textBoxEntryOrGuid.Text))
                return;

            this.buttonGenerateSql.Enabled = false;
            this.menuItemGenerateSql.Enabled = false;
            this.pictureBoxLoadScript.Enabled = false;
            this.pictureBoxCreateScript.Enabled = false;
            this.lastSmartScriptIdOfScript = 0;

            if (entryorguid != -1 && sourceType != SourceTypes.SourceTypeNone)
            {
                this.originalEntryOrGuidAndSourceType.entryOrGuid = entryorguid;
                this.originalEntryOrGuidAndSourceType.sourceType = sourceType;
                this.textBoxEntryOrGuid.Text = entryorguid.ToString();
                this.comboBoxSourceType.SelectedIndex = this.GetIndexBySourceType(sourceType);
            }
            else
            {
                try
                {
                    this.originalEntryOrGuidAndSourceType.entryOrGuid = Int32.Parse(this.textBoxEntryOrGuid.Text);
                }
                catch (OverflowException)
                {
                    MessageBox.Show("The entryorguid is either too big or too small.", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (FormatException)
                {
                    MessageBox.Show("The entryorguid field does not contain a valid number.", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                this.originalEntryOrGuidAndSourceType.sourceType = this.GetSourceTypeByIndex();
            }

            List<SmartScript> smartScripts = await this.GetSmartScriptsForEntryAndSourceType(this.originalEntryOrGuidAndSourceType.entryOrGuid.ToString(), this.originalEntryOrGuidAndSourceType.sourceType, showErrorIfNoneFound, promptCreateIfNoneFound);
            this.listViewSmartScripts.ReplaceSmartScripts(smartScripts);
            this.checkBoxListActionlistsOrEntries.Text = this.originalEntryOrGuidAndSourceType.sourceType == SourceTypes.SourceTypeScriptedActionlist ? "List entries too" : "List actionlists too";

            this.buttonNewLine.Enabled = false;
            this.SetGenerateCommentsEnabled(this.listViewSmartScripts.Items.Count > 0 && Settings.Default.UseWorldDatabase);
            this.HandleShowBasicInfo();

            if (this.listViewSmartScripts.Items.Count > 0)
            {
                this.SortListView(SortOrder.Ascending, 1);
                this.listViewSmartScripts.Items[0].Selected = true;
                this.listViewSmartScripts.Select(); //! Sets the focus on the listview

                if (this.checkBoxListActionlistsOrEntries.Enabled && this.checkBoxListActionlistsOrEntries.Checked)
                {
                    foreach (ListViewItem item in this.listViewSmartScripts.Items)
                        if (item.Text == this.originalEntryOrGuidAndSourceType.entryOrGuid.ToString())
                            this.lastSmartScriptIdOfScript = XConverter.ToInt32(item.SubItems[2].Text);
                }
                else
                    this.lastSmartScriptIdOfScript = XConverter.ToInt32(this.listViewSmartScripts.Items[this.listViewSmartScripts.Items.Count - 1].SubItems[2].Text);
            }

            this.buttonNewLine.Enabled = this.textBoxEntryOrGuid.Text.Length > 0;
            this.buttonGenerateSql.Enabled = this.listViewSmartScripts.Items.Count > 0;
            this.menuItemGenerateSql.Enabled = this.listViewSmartScripts.Items.Count > 0;
            this.pictureBoxCreateScript.Enabled = this.textBoxEntryOrGuid.Text.Length > 0;
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
            using (MultiSelectForm<SmartPhaseMasks> multiSelectForm = new MultiSelectForm<SmartPhaseMasks>(this.textBoxEventPhasemask))
                multiSelectForm.ShowDialog(this);
        }

        private void buttonSelectEventFlag_Click(object sender, EventArgs e)
        {
            using (MultiSelectForm<SmartEventFlags> multiSelectForm = new MultiSelectForm<SmartEventFlags>(this.textBoxEventFlags))
                multiSelectForm.ShowDialog(this);
        }

        private async void buttonSearchWorldDb_Click(object sender, EventArgs e)
        {
            List<string> databaseNames = await SAI_Editor_Manager.Instance.GetDatabasesInConnection(this.textBoxHost.Text, this.textBoxUsername.Text, XConverter.ToUInt32(this.textBoxPort.Text), this.textBoxPassword.Text);

            if (databaseNames != null && databaseNames.Count > 0)
                using (var selectDatabaseForm = new SelectDatabaseForm(databaseNames, this.textBoxWorldDatabase))
                    selectDatabaseForm.ShowDialog(this);
        }

        private void listViewSmartScripts_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            //! Don't use the SortListView method here
            this.listViewSmartScripts.ListViewItemSorter = this.lvwColumnSorter;

            if (e.Column != this.lvwColumnSorter.SortColumn)
            {
                this.lvwColumnSorter.SortColumn = e.Column;
                this.lvwColumnSorter.Order = SortOrder.Ascending;
            }
            else
                this.lvwColumnSorter.Order = this.lvwColumnSorter.Order == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;

            this.listViewSmartScripts.Sort();
        }

        private void SortListView(SortOrder order, int column)
        {
            this.listViewSmartScripts.ListViewItemSorter = this.lvwColumnSorter;

            if (column != this.lvwColumnSorter.SortColumn)
            {
                this.lvwColumnSorter.SortColumn = column;
                this.lvwColumnSorter.Order = order != SortOrder.None ? order : SortOrder.Ascending;
            }
            else
                this.lvwColumnSorter.Order = order != SortOrder.None ? order : SortOrder.Ascending;

            this.listViewSmartScripts.Sort();
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
            this.TryToOpenLinkForm(this.textBoxLinkTo);
        }

        private void buttonLinkFrom_Click(object sender, EventArgs e)
        {
            this.TryToOpenLinkForm(this.textBoxLinkFrom);
        }

        private void TryToOpenLinkForm(TextBox textBoxToChange)
        {
            if (this.listViewSmartScripts.Items.Count <= 1)
            {
                MessageBox.Show("There are not enough items in the listview in order to link!", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (this.listViewSmartScripts.SelectedItems.Count == 0)
            {
                MessageBox.Show("You must first select a line in the script", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SearchForLinkForm searchForLinkForm = new SearchForLinkForm(this.listViewSmartScripts.SmartScripts, this.listViewSmartScripts.SelectedItems[0].Index, textBoxToChange))
                searchForLinkForm.ShowDialog(this);
        }

        protected override void WndProc(ref Message m)
        {
            //! Don't allow moving the window while we are expanding or contracting. This is required because
            //! the window often breaks and has an incorrect size in the end if the application had been moved
            //! while expanding or contracting.
            if (((m.Msg == 274 && m.WParam.ToInt32() == 61456) || (m.Msg == 161 && m.WParam.ToInt32() == 2)) && (this.expandingToMainForm || this.contractingToLoginForm))
                return;

            base.WndProc(ref m);
        }

        private void listViewSmartScripts_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                if (this.listViewSmartScripts.FocusedItem.Bounds.Contains(e.Location))
                    this.contextMenuStripListView.Show(Cursor.Position);
        }

        private void testToolStripMenuItemDeleteRow_Click(object sender, EventArgs e)
        {
            this.DeleteSelectedRow();
        }

        private void ResetFieldsToDefault(bool withStatic = false)
        {
            if (withStatic)
            {
                this.textBoxEntryOrGuid.Text = String.Empty;
                this.comboBoxSourceType.SelectedIndex = 0;
            }

            this.comboBoxEventType.SelectedIndex = 0;
            this.comboBoxActionType.SelectedIndex = 0;
            this.comboBoxTargetType.SelectedIndex = 0;
            this.textBoxEventType.Text = "0";
            this.textBoxActionType.Text = "0";
            this.textBoxTargetType.Text = "0";
            this.textBoxEventChance.Text = "100";
            this.textBoxId.Text = "-1";
            this.textBoxLinkFrom.Text = "None";
            this.textBoxLinkTo.Text = "0";
            this.textBoxComments.Text = SAI_Editor_Manager.Instance.GetDefaultCommentForSourceType(this.GetSourceTypeByIndex());
            this.textBoxEventPhasemask.Text = "0";
            this.textBoxEventFlags.Text = "0";

            foreach (TabPage page in this.tabControlParameters.TabPages)
                foreach (Control control in page.Controls)
                    if (control is TextBox)
                        control.Text = "0";

            this.SetVisibilityOfAllParamButtons(false);
        }

        private void SetVisibilityOfAllParamButtons(bool visible)
        {
            this.buttonEventParamOneSearch.Visible = visible;
            this.buttonEventParamTwoSearch.Visible = visible;
            this.buttonEventParamThreeSearch.Visible = visible;
            this.buttonEventParamFourSearch.Visible = visible;
            this.buttonActionParamOneSearch.Visible = visible;
            this.buttonActionParamTwoSearch.Visible = visible;
            this.buttonActionParamThreeSearch.Visible = visible;
            this.buttonActionParamFourSearch.Visible = visible;
            this.buttonActionParamFiveSearch.Visible = visible;
            this.buttonActionParamSixSearch.Visible = visible;
            this.buttonTargetParamOneSearch.Visible = visible;
            this.buttonTargetParamTwoSearch.Visible = visible;
            this.buttonTargetParamThreeSearch.Visible = visible;
            this.buttonTargetParamFourSearch.Visible = visible;
            this.buttonTargetParamFiveSearch.Visible = visible;
            this.buttonTargetParamSixSearch.Visible = visible;
            this.buttonTargetParamSevenSearch.Visible = visible;
        }

        private void SetVisibilityOfAllEventParamButtons(bool visible)
        {
            this.buttonEventParamOneSearch.Visible = visible;
            this.buttonEventParamTwoSearch.Visible = visible;
            this.buttonEventParamThreeSearch.Visible = visible;
            this.buttonEventParamFourSearch.Visible = visible;
        }

        private void SetVisibilityOfAllActionParamButtons(bool visible)
        {
            this.buttonActionParamOneSearch.Visible = visible;
            this.buttonActionParamTwoSearch.Visible = visible;
            this.buttonActionParamThreeSearch.Visible = visible;
            this.buttonActionParamFourSearch.Visible = visible;
            this.buttonActionParamFiveSearch.Visible = visible;
            this.buttonActionParamSixSearch.Visible = visible;
        }

        private void SetVisibilityOfAllTargetParamButtons(bool visible)
        {
            this.buttonTargetParamOneSearch.Visible = visible;
            this.buttonTargetParamTwoSearch.Visible = visible;
            this.buttonTargetParamThreeSearch.Visible = visible;
            this.buttonTargetParamFourSearch.Visible = visible;
            this.buttonTargetParamFiveSearch.Visible = visible;
            this.buttonTargetParamSixSearch.Visible = visible;
            this.buttonTargetParamSevenSearch.Visible = visible;
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
            TextBox textBoxToChange = this.textBoxEventParam1;

            switch ((SmartEvent)this.comboBoxEventType.SelectedIndex)
            {
                case SmartEvent.SMART_EVENT_SPELLHIT: //! Spell id
                case SmartEvent.SMART_EVENT_FRIENDLY_MISSING_BUFF: //! Spell id
                case SmartEvent.SMART_EVENT_HAS_AURA: //! Spell id
                case SmartEvent.SMART_EVENT_TARGET_BUFFED: //! Spell id
                case SmartEvent.SMART_EVENT_SPELLHIT_TARGET: //! Spell id
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSpell))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartEvent.SMART_EVENT_RESPAWN: //! Respawn condition
                    using (SingleSelectForm<SmartScriptRespawnCondition> singleSelectForm = new SingleSelectForm<SmartScriptRespawnCondition>(textBoxToChange))
                        singleSelectForm.ShowDialog(this);
                    break;
                case SmartEvent.SMART_EVENT_SUMMON_DESPAWNED: //! Creature entry
                case SmartEvent.SMART_EVENT_SUMMONED_UNIT: //! Creature entry
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartEvent.SMART_EVENT_AREATRIGGER_ONTRIGGER: //! Areatrigger entry
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeAreaTrigger))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartEvent.SMART_EVENT_GO_STATE_CHANGED: //! Go state
                    using (SingleSelectForm<GoStates> singleSelectForm = new SingleSelectForm<GoStates>(textBoxToChange))
                        singleSelectForm.ShowDialog(this);
                    break;
                case SmartEvent.SMART_EVENT_GAME_EVENT_START: //! Game event entry
                case SmartEvent.SMART_EVENT_GAME_EVENT_END: //! Game event entry
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGameEvent))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartEvent.SMART_EVENT_MOVEMENTINFORM: //! Motion type
                    using (SingleSelectForm<MovementGeneratorType> singleSelectForm = new SingleSelectForm<MovementGeneratorType>(textBoxToChange))
                        singleSelectForm.ShowDialog(this);
                    break;
                case SmartEvent.SMART_EVENT_ACCEPTED_QUEST: //! Quest id
                case SmartEvent.SMART_EVENT_REWARD_QUEST: //! Quest id
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeQuest))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartEvent.SMART_EVENT_RECEIVE_EMOTE: //! Emote id
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeEmote))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartEvent.SMART_EVENT_GOSSIP_SELECT: //! Gossip menu id
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGossipMenuOption))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
            }
        }

        private void buttonEventParamTwoSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = this.textBoxEventParam2;

            switch ((SmartEvent)this.comboBoxEventType.SelectedIndex)
            {
                case SmartEvent.SMART_EVENT_SPELLHIT: //! Spell school
                case SmartEvent.SMART_EVENT_SPELLHIT_TARGET: //! Spell school
                    using (SingleSelectForm<SpellSchools> singleSelectForm = new SingleSelectForm<SpellSchools>(textBoxToChange))
                        singleSelectForm.ShowDialog(this);
                    break;
                case SmartEvent.SMART_EVENT_RESPAWN: //! Map
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeMap))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartEvent.SMART_EVENT_TEXT_OVER: //! Creature entry
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartEvent.SMART_EVENT_GOSSIP_SELECT: //! Gossip id
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGossipOptionId))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
            }
        }

        private void buttonEventParamThreeSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = this.textBoxEventParam3;

            switch ((SmartEvent)this.comboBoxEventType.SelectedIndex)
            {
                case SmartEvent.SMART_EVENT_RESPAWN: //! Zone
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeZone))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartEvent.SMART_EVENT_VICTIM_CASTING: //! Spell id
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSpell))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
            }
        }

        private void buttonEventParamFourSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = this.textBoxEventParam4;

            switch ((SmartEvent)this.comboBoxEventType.SelectedIndex)
            {
                case SmartEvent.SMART_EVENT_KILL: //! Creature entry
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
            }
        }

        private void buttonTargetParamOneSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = this.textBoxTargetParam1;

            switch ((SmartTarget)this.comboBoxTargetType.SelectedIndex)
            {
                case SmartTarget.SMART_TARGET_CREATURE_RANGE: //! Creature entry
                case SmartTarget.SMART_TARGET_CREATURE_DISTANCE:
                case SmartTarget.SMART_TARGET_CLOSEST_CREATURE:
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartTarget.SMART_TARGET_CREATURE_GUID: //! Creature guid
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureGuid))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartTarget.SMART_TARGET_GAMEOBJECT_RANGE:
                case SmartTarget.SMART_TARGET_GAMEOBJECT_DISTANCE:
                case SmartTarget.SMART_TARGET_CLOSEST_GAMEOBJECT: //! Gameobject entry
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectEntry))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartTarget.SMART_TARGET_GAMEOBJECT_GUID: //! Gameobject guid
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectGuid))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
            }

            switch ((SmartAction)this.comboBoxActionType.SelectedIndex)
            {
                case SmartAction.SMART_ACTION_INSTALL_AI_TEMPLATE:
                    //! This button is different based on the number in the first parameter field
                    switch ((SmartAiTemplates)XConverter.ToInt32(this.textBoxActionParam1.Text))
                    {
                        case SmartAiTemplates.SMARTAI_TEMPLATE_CASTER:
                        case SmartAiTemplates.SMARTAI_TEMPLATE_TURRET:
                            using (MultiSelectForm<SmartCastFlags> multiSelectForm = new MultiSelectForm<SmartCastFlags>(textBoxToChange))
                                multiSelectForm.ShowDialog(this);
                            break;
                    }
                    break;
            }
        }

        private void buttonTargetParamTwoSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = this.textBoxTargetParam2;

            switch ((SmartTarget)this.comboBoxTargetType.SelectedIndex)
            {
                case SmartTarget.SMART_TARGET_CREATURE_GUID: //! Creature entry
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartTarget.SMART_TARGET_GAMEOBJECT_GUID: //! Gameobject entry
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectEntry))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
            }
        }

        private void buttonTargetParamThreeSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = this.textBoxTargetParam3;
        }

        private void buttonTargetParamFourSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = this.textBoxTargetX;
        }

        private void buttonTargetParamFiveSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = this.textBoxTargetY;
        }

        private void buttonTargetParamSixSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = this.textBoxTargetZ;
        }

        private void buttonTargetParamSevenSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = this.textBoxTargetO;
        }

        private void buttonActionParamOneSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = this.textBoxActionParam1;

            switch ((SmartAction)this.comboBoxActionType.SelectedIndex)
            {
                case SmartAction.SMART_ACTION_CAST:
                case SmartAction.SMART_ACTION_INVOKER_CAST:
                case SmartAction.SMART_ACTION_CROSS_CAST:
                case SmartAction.SMART_ACTION_REMOVEAURASFROMSPELL:
                case SmartAction.SMART_ACTION_ADD_AURA:
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSpell))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_SET_FACTION:
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeFaction))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_EMOTE:
                case SmartAction.SMART_ACTION_RANDOM_EMOTE:
                case SmartAction.SMART_ACTION_SET_EMOTE_STATE:
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeEmote))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_FAIL_QUEST:
                case SmartAction.SMART_ACTION_ADD_QUEST:
                case SmartAction.SMART_ACTION_CALL_AREAEXPLOREDOREVENTHAPPENS:
                case SmartAction.SMART_ACTION_CALL_GROUPEVENTHAPPENS:
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeQuest))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_SET_REACT_STATE:
                    using (SingleSelectForm<ReactStates> singleSelectForm = new SingleSelectForm<ReactStates>(textBoxToChange))
                        singleSelectForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_SOUND:
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSound))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_MORPH_TO_ENTRY_OR_MODEL:
                case SmartAction.SMART_ACTION_SUMMON_CREATURE:
                case SmartAction.SMART_ACTION_CALL_CASTEDCREATUREORGO:
                case SmartAction.SMART_ACTION_KILLED_MONSTER:
                case SmartAction.SMART_ACTION_UPDATE_TEMPLATE:
                case SmartAction.SMART_ACTION_MOUNT_TO_ENTRY_OR_MODEL:
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_GO_SET_LOOT_STATE:
                    using (SingleSelectForm<GoStates> singleSelectForm = new SingleSelectForm<GoStates>(textBoxToChange))
                        singleSelectForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_SET_POWER:
                case SmartAction.SMART_ACTION_ADD_POWER:
                case SmartAction.SMART_ACTION_REMOVE_POWER:
                    using (SingleSelectForm<PowerTypes> singleSelectForm = new SingleSelectForm<PowerTypes>(textBoxToChange))
                        singleSelectForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_SUMMON_GO:
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectEntry))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_SET_EVENT_PHASE:
                case SmartAction.SMART_ACTION_RANDOM_PHASE:
                case SmartAction.SMART_ACTION_RANDOM_PHASE_RANGE:
                    using (MultiSelectForm<SmartPhaseMasks> multiSelectForm = new MultiSelectForm<SmartPhaseMasks>(textBoxToChange))
                        multiSelectForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_SET_PHASE_MASK:
                    using (MultiSelectForm<PhaseMasks> multiSelectForm = new MultiSelectForm<PhaseMasks>(textBoxToChange))
                        multiSelectForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_ADD_ITEM:
                case SmartAction.SMART_ACTION_REMOVE_ITEM:
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeItemEntry))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_TELEPORT:
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeMap))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_SUMMON_CREATURE_GROUP:
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSummonsId))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_SET_SHEATH:
                    using (SingleSelectForm<SheathState> singleSelectForm = new SingleSelectForm<SheathState>(textBoxToChange))
                        singleSelectForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_ACTIVATE_TAXI:
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeTaxiPath))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_SET_UNIT_FLAG:
                case SmartAction.SMART_ACTION_REMOVE_UNIT_FLAG:
                    //! There should be a different form opened based on parameter 2. If parameter two is set to '0' it means
                    //! we target UNIT_FIELD_FLAGS. If it's above 0 it means we target UNIT_FIELD_FLAGS2 (notice the 2).
                    if (this.textBoxActionParam2.Text == "0" || String.IsNullOrWhiteSpace(this.textBoxActionParam2.Text))
                    {
                        using (MultiSelectForm<UnitFlags> multiSelectForm = new MultiSelectForm<UnitFlags>(textBoxToChange))
                            multiSelectForm.ShowDialog(this);
                    }
                    else
                    {
                        using (MultiSelectForm<UnitFlags2> multiSelectForm = new MultiSelectForm<UnitFlags2>(textBoxToChange))
                            multiSelectForm.ShowDialog(this);
                    }

                    break;
                case SmartAction.SMART_ACTION_SET_GO_FLAG:
                case SmartAction.SMART_ACTION_ADD_GO_FLAG:
                case SmartAction.SMART_ACTION_REMOVE_GO_FLAG:
                    using (MultiSelectForm<GoFlags> multiSelectForm = new MultiSelectForm<GoFlags>(textBoxToChange))
                        multiSelectForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_SET_DYNAMIC_FLAG:
                case SmartAction.SMART_ACTION_ADD_DYNAMIC_FLAG:
                case SmartAction.SMART_ACTION_REMOVE_DYNAMIC_FLAG:
                    using (MultiSelectForm<DynamicFlags> multiSelectForm = new MultiSelectForm<DynamicFlags>(textBoxToChange))
                        multiSelectForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_EQUIP:
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeEquipTemplate))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_SET_NPC_FLAG:
                case SmartAction.SMART_ACTION_ADD_NPC_FLAG:
                case SmartAction.SMART_ACTION_REMOVE_NPC_FLAG:
                    using (MultiSelectForm<NpcFlags> multiSelectForm = new MultiSelectForm<NpcFlags>(textBoxToChange))
                        multiSelectForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_INSTALL_AI_TEMPLATE:
                    using (SingleSelectForm<SmartAiTemplates> singleSelectForm = new SingleSelectForm<SmartAiTemplates>(textBoxToChange))
                        singleSelectForm.ShowDialog(this);

                    this.ParameterInstallAiTemplateChanged();
                    break;
                case SmartAction.SMART_ACTION_SET_UNIT_FIELD_BYTES_1:
                case SmartAction.SMART_ACTION_REMOVE_UNIT_FIELD_BYTES_1:
                    int searchType;

                    if (Int32.TryParse(this.textBoxActionParam2.Text, out searchType))
                    {
                        switch (searchType)
                        {
                            case 0:
                                using (SingleSelectForm<UnitStandStateType> singleSelectForm = new SingleSelectForm<UnitStandStateType>(textBoxToChange))
                                    singleSelectForm.ShowDialog(this);
                                break;
                            //case 1:
                            //    break;
                            case 2:
                                using (MultiSelectForm<UnitStandFlags> multiSelectForm = new MultiSelectForm<UnitStandFlags>(textBoxToChange))
                                    multiSelectForm.ShowDialog(this);
                                break;
                            case 3:
                                using (MultiSelectForm<UnitBytes1_Flags> multiSelectForm = new MultiSelectForm<UnitBytes1_Flags>(textBoxToChange))
                                    multiSelectForm.ShowDialog(this);
                                break;
                            default:
                                MessageBox.Show("The second parameter (type) must be set to a valid search type (0, 2 or 3).", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                        }
                    }

                    break;
            }
        }

        private void SetTextOfAllEventParameterLabels(string str)
        {
            this.labelEventParam1.Text = str;
            this.labelEventParam2.Text = str;
            this.labelEventParam3.Text = str;
            this.labelEventParam4.Text = str;
        }

        private void SetTextOfAllActionParameterLabels(string str)
        {
            this.labelActionParam1.Text = str;
            this.labelActionParam2.Text = str;
            this.labelActionParam3.Text = str;
            this.labelActionParam4.Text = str;
            this.labelActionParam5.Text = str;
            this.labelActionParam6.Text = str;
        }

        private void SetTextOfAllTargetParameterLabels(string str)
        {
            this.labelTargetParam1.Text = str;
            this.labelTargetParam2.Text = str;
            this.labelTargetParam3.Text = str;
        }

        private void ParameterInstallAiTemplateChanged()
        {
            this.SetVisibilityOfAllActionParamButtons(false);
            this.SetVisibilityOfAllTargetParamButtons(false);
            this.SetTextOfAllActionParameterLabels("");

            this.labelActionParam1.Text = "Template entry";
            this.buttonActionParamOneSearch.Visible = true;
            int newTemplateId = XConverter.ToInt32(this.textBoxActionParam1.Text);

            switch ((SmartAiTemplates)newTemplateId)
            {
                case SmartAiTemplates.SMARTAI_TEMPLATE_BASIC:
                case SmartAiTemplates.SMARTAI_TEMPLATE_PASSIVE:
                    break;
                case SmartAiTemplates.SMARTAI_TEMPLATE_CASTER:
                case SmartAiTemplates.SMARTAI_TEMPLATE_TURRET:
                    this.labelActionParam2.Text = "Spell id";
                    this.buttonActionParamTwoSearch.Visible = true; //! Spell id
                    this.labelActionParam3.Text = "RepeatMin";
                    this.labelActionParam4.Text = "RepeatMax";
                    this.labelActionParam5.Text = "Range";
                    this.labelActionParam6.Text = "Mana pct";

                    this.labelTargetParam1.Text = "Castflag";
                    this.buttonTargetParamOneSearch.Visible = true;
                    break;
                case SmartAiTemplates.SMARTAI_TEMPLATE_CAGED_GO_PART:
                    this.labelActionParam2.Text = "Creature entry";
                    this.buttonActionParamTwoSearch.Visible = true; //! Creature entry
                    this.labelActionParam3.Text = "Credit at end (0/1)";
                    break;
                case SmartAiTemplates.SMARTAI_TEMPLATE_CAGED_NPC_PART:
                    this.labelActionParam2.Text = "Gameobject entry";
                    this.buttonActionParamTwoSearch.Visible = true; //! Gameobject entry
                    this.labelActionParam3.Text = "Despawn time";
                    this.labelActionParam4.Text = "Walk/run (0/1)";
                    this.labelActionParam5.Text = "Distance";
                    this.labelActionParam6.Text = "Group id";
                    break;
            }
        }

        private void buttonActionParamTwoSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = this.textBoxActionParam2;

            switch ((SmartAction)this.comboBoxActionType.SelectedIndex)
            {
                case SmartAction.SMART_ACTION_CAST:
                case SmartAction.SMART_ACTION_INVOKER_CAST:
                case SmartAction.SMART_ACTION_CROSS_CAST:
                    using (MultiSelectForm<SmartCastFlags> multiSelectForm = new MultiSelectForm<SmartCastFlags>(textBoxToChange))
                        multiSelectForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_WP_STOP:
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeQuest))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_INTERRUPT_SPELL:
                case SmartAction.SMART_ACTION_CALL_CASTEDCREATUREORGO:
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSpell))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_SUMMON_CREATURE:
                    using (SingleSelectForm<TempSummonType> singleSelectForm = new SingleSelectForm<TempSummonType>(textBoxToChange))
                        singleSelectForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_RANDOM_PHASE:
                case SmartAction.SMART_ACTION_RANDOM_PHASE_RANGE:
                    using (MultiSelectForm<SmartPhaseMasks> multiSelectForm = new MultiSelectForm<SmartPhaseMasks>(textBoxToChange))
                        multiSelectForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_RANDOM_EMOTE:
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeEmote))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_INSTALL_AI_TEMPLATE:
                    //! This button is different based on the number in the first parameter field
                    switch ((SmartAiTemplates)XConverter.ToInt32(this.textBoxActionParam1.Text))
                    {
                        case SmartAiTemplates.SMARTAI_TEMPLATE_CASTER:
                        case SmartAiTemplates.SMARTAI_TEMPLATE_TURRET:
                            using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSpell))
                                searchFromDatabaseForm.ShowDialog(this);
                            break;
                        case SmartAiTemplates.SMARTAI_TEMPLATE_CAGED_GO_PART:
                            using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry))
                                searchFromDatabaseForm.ShowDialog(this);
                            break;
                        case SmartAiTemplates.SMARTAI_TEMPLATE_CAGED_NPC_PART:
                            using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectEntry))
                                searchFromDatabaseForm.ShowDialog(this);
                            break;
                    }
                    break;
                case SmartAction.SMART_ACTION_WP_START:
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeWaypoint))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_SEND_GOSSIP_MENU:
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeNpcText))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_SET_UNIT_FIELD_BYTES_1:
                case SmartAction.SMART_ACTION_REMOVE_UNIT_FIELD_BYTES_1:
                    using (SingleSelectForm<UnitFieldBytes1Types> singleSelectForm = new SingleSelectForm<UnitFieldBytes1Types>(textBoxToChange))
                        singleSelectForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_CALL_TIMED_ACTIONLIST:
                    using (SingleSelectForm<ActionlistTimerUpdateType> singleSelectForm = new SingleSelectForm<ActionlistTimerUpdateType>(textBoxToChange))
                        singleSelectForm.ShowDialog(this);
                    break;
            }
        }

        private void buttonActionParamThreeSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = this.textBoxActionParam3;

            switch ((SmartAction)this.comboBoxActionType.SelectedIndex)
            {
                case SmartAction.SMART_ACTION_FOLLOW:
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_CROSS_CAST:
                    using (SingleSelectForm<SmartTarget> singleSelectForm = new SingleSelectForm<SmartTarget>(textBoxToChange))
                        singleSelectForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_RANDOM_PHASE:
                    using (MultiSelectForm<SmartPhaseMasks> multiSelectForm = new MultiSelectForm<SmartPhaseMasks>(textBoxToChange))
                        multiSelectForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_EQUIP:
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeItemEntry))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_RANDOM_EMOTE:
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeEmote))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
            }
        }

        private void buttonActionParamFourSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = this.textBoxActionParam4;

            switch ((SmartAction)this.comboBoxActionType.SelectedIndex)
            {
                case SmartAction.SMART_ACTION_FOLLOW:
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_WP_START:
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeQuest))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_RANDOM_PHASE:
                    using (MultiSelectForm<SmartPhaseMasks> multiSelectForm = new MultiSelectForm<SmartPhaseMasks>(textBoxToChange))
                        multiSelectForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_EQUIP:
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeItemEntry))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_RANDOM_EMOTE:
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeEmote))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
            }
        }

        private void buttonActionParamFiveSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = this.textBoxActionParam5;

            switch ((SmartAction)this.comboBoxActionType.SelectedIndex)
            {
                case SmartAction.SMART_ACTION_RANDOM_PHASE:
                    using (MultiSelectForm<SmartPhaseMasks> multiSelectForm = new MultiSelectForm<SmartPhaseMasks>(textBoxToChange))
                        multiSelectForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_EQUIP:
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeItemEntry))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_RANDOM_EMOTE:
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeEmote))
                        searchFromDatabaseForm.ShowDialog(this);
                    break;
            }
        }

        private void buttonActionParamSixSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = this.textBoxActionParam6;

            switch ((SmartAction)this.comboBoxActionType.SelectedIndex)
            {
                case SmartAction.SMART_ACTION_WP_START:
                    using (SingleSelectForm<ReactStates> singleSelectForm = new SingleSelectForm<ReactStates>(textBoxToChange))
                        singleSelectForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_RANDOM_PHASE:
                    using (MultiSelectForm<SmartPhaseMasks> multiSelectForm = new MultiSelectForm<SmartPhaseMasks>(textBoxToChange))
                        multiSelectForm.ShowDialog(this);
                    break;
                case SmartAction.SMART_ACTION_RANDOM_EMOTE:
                    using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(this.connectionString, textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeEmote))
                        searchFromDatabaseForm.ShowDialog(this);
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
            this.TryToOpenPage("http://collab.kpsn.org/display/tc/smart_scripts");
        }

        private void textBoxComments_GotFocus(object sender, EventArgs e)
        {
            if (this.textBoxComments.Text == SAI_Editor_Manager.Instance.GetDefaultCommentForSourceType(this.GetSourceTypeByIndex()))
                this.textBoxComments.Text = "";
        }

        private void textBoxComments_LostFocus(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.textBoxComments.Text))
                this.textBoxComments.Text = SAI_Editor_Manager.Instance.GetDefaultCommentForSourceType(this.GetSourceTypeByIndex());
        }

        public void ExpandToShowPermanentTooltips(bool expand)
        {
            this.listViewSmartScriptsInitialHeight = this.listViewSmartScripts.Height;
            this.expandingListView = expand;
            this.contractingListView = !expand;
            this.listViewSmartScriptsHeightToChangeTo = expand ? this.listViewSmartScripts.Height + (int)FormSizes.ListViewHeightContract : this.listViewSmartScripts.Height - (int)FormSizes.ListViewHeightContract;
            this.timerShowPermanentTooltips.Enabled = true;
            ToolTipHelper.DisableOrEnableAllToolTips(false);

            if (expand)
            {
                this.panelPermanentTooltipTypes.Visible = false;
                this.panelPermanentTooltipParameters.Visible = false;
                this.listViewSmartScriptsHeightToChangeTo = this.listViewSmartScripts.Height + (int)FormSizes.ListViewHeightContract;
                this.ChangeParameterFieldsBasedOnType();
            }
            else
            {
                this.listViewSmartScriptsHeightToChangeTo = this.listViewSmartScripts.Height - (int)FormSizes.ListViewHeightContract;
                //ChangeParameterFieldsBasedOnType();
            }
        }

        private void timerShowPermanentTooltips_Tick(object sender, EventArgs e)
        {
            if (this.expandingListView)
            {
                if (this.listViewSmartScripts.Height < this.listViewSmartScriptsHeightToChangeTo)
                    this.listViewSmartScripts.Height += this.expandAndContractSpeedListView;
                else
                {
                    this.listViewSmartScripts.Height = this.listViewSmartScriptsHeightToChangeTo;
                    this.timerShowPermanentTooltips.Enabled = false;
                    this.expandingListView = false;
                    ToolTipHelper.DisableOrEnableAllToolTips(true);
                    this.checkBoxUsePermanentTooltips.Enabled = true;
                }
            }
            else if (this.contractingListView)
            {
                if (this.listViewSmartScripts.Height > this.listViewSmartScriptsHeightToChangeTo)
                    this.listViewSmartScripts.Height -= this.expandAndContractSpeedListView;
                else
                {
                    this.listViewSmartScripts.Height = this.listViewSmartScriptsHeightToChangeTo;
                    this.timerShowPermanentTooltips.Enabled = false;
                    this.contractingListView = false;
                    this.panelPermanentTooltipTypes.Visible = true;
                    this.panelPermanentTooltipParameters.Visible = true;
                    this.checkBoxUsePermanentTooltips.Enabled = true;
                }
            }
        }

        private void comboBoxEventType_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                this.UpdatePermanentTooltipOfTypes(sender as ComboBox, ScriptTypeId.ScriptTypeEvent);
        }

        private void comboBoxActionType_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                this.UpdatePermanentTooltipOfTypes(sender as ComboBox, ScriptTypeId.ScriptTypeAction);
        }

        private void comboBoxTargetType_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                this.UpdatePermanentTooltipOfTypes(sender as ComboBox, ScriptTypeId.ScriptTypeTarget);
        }

        private void UpdatePermanentTooltipOfTypes(ComboBox comboBoxToTarget, ScriptTypeId scriptTypeId)
        {
            string toolTipOfType = SAI_Editor_Manager.Instance.GetScriptTypeTooltipById(comboBoxToTarget.SelectedIndex, scriptTypeId);
            string toolTipTitleOfType = comboBoxToTarget.SelectedItem.ToString();

            if (!String.IsNullOrWhiteSpace(toolTipOfType) && !String.IsNullOrWhiteSpace(toolTipTitleOfType))
            {
                this.labelPermanentTooltipTextTypes.Text = toolTipOfType;
                this.labelPermanentTooltipTitleTypes.Text = toolTipTitleOfType;
            }
        }

        private void UpdatePermanentTooltipOfParameter(Label labelToTarget, int paramId, ComboBox comboBoxToTarget, ScriptTypeId scriptTypeId)
        {
            string toolTipOfType = SAI_Editor_Manager.Instance.GetParameterTooltipById(comboBoxToTarget.SelectedIndex, paramId, scriptTypeId);

            if (!String.IsNullOrWhiteSpace(toolTipOfType))
            {
                this.labelPermanentTooltipTextParameters.Text = toolTipOfType;
                this.labelPermanentTooltipParameterTitleTypes.Text = comboBoxToTarget.SelectedItem + " - " + labelToTarget.Text;
            }
        }

        private int GetSelectedIndexByScriptTypeId(ScriptTypeId scriptTypeId)
        {
            switch (scriptTypeId)
            {
                case ScriptTypeId.ScriptTypeEvent:
                    return this.comboBoxEventType.SelectedIndex;
                case ScriptTypeId.ScriptTypeAction:
                    return this.comboBoxActionType.SelectedIndex;
                case ScriptTypeId.ScriptTypeTarget:
                    return this.comboBoxTargetType.SelectedIndex;
            }

            return 0;
        }

        private string GetSelectedItemByScriptTypeId(ScriptTypeId scriptTypeId)
        {
            switch (scriptTypeId)
            {
                case ScriptTypeId.ScriptTypeEvent:
                    return this.comboBoxEventType.SelectedItem.ToString();
                case ScriptTypeId.ScriptTypeAction:
                    return this.comboBoxActionType.SelectedItem.ToString();
                case ScriptTypeId.ScriptTypeTarget:
                    return this.comboBoxTargetType.SelectedItem.ToString();
            }

            return String.Empty;
        }

        private void labelEventParam1_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                this.UpdatePermanentTooltipOfParameter(sender as Label, 1, this.comboBoxEventType, ScriptTypeId.ScriptTypeEvent);
        }

        private void labelEventParam2_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                this.UpdatePermanentTooltipOfParameter(sender as Label, 2, this.comboBoxEventType, ScriptTypeId.ScriptTypeEvent);
        }

        private void labelEventParam3_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                this.UpdatePermanentTooltipOfParameter(sender as Label, 3, this.comboBoxEventType, ScriptTypeId.ScriptTypeEvent);
        }

        private void labelEventParam4_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                this.UpdatePermanentTooltipOfParameter(sender as Label, 4, this.comboBoxEventType, ScriptTypeId.ScriptTypeEvent);
        }

        private void labelActionParam1_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                this.UpdatePermanentTooltipOfParameter(sender as Label, 1, this.comboBoxActionType, ScriptTypeId.ScriptTypeAction);
        }

        private void labelActionParam2_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                this.UpdatePermanentTooltipOfParameter(sender as Label, 2, this.comboBoxActionType, ScriptTypeId.ScriptTypeAction);
        }

        private void labelActionParam3_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                this.UpdatePermanentTooltipOfParameter(sender as Label, 3, this.comboBoxActionType, ScriptTypeId.ScriptTypeAction);
        }

        private void labelActionParam4_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                this.UpdatePermanentTooltipOfParameter(sender as Label, 4, this.comboBoxActionType, ScriptTypeId.ScriptTypeAction);
        }

        private void labelActionParam5_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                this.UpdatePermanentTooltipOfParameter(sender as Label, 5, this.comboBoxActionType, ScriptTypeId.ScriptTypeAction);
        }

        private void labelActionParam6_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                this.UpdatePermanentTooltipOfParameter(sender as Label, 6, this.comboBoxActionType, ScriptTypeId.ScriptTypeAction);
        }

        private void labelTargetParam1_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                this.UpdatePermanentTooltipOfParameter(sender as Label, 1, this.comboBoxTargetType, ScriptTypeId.ScriptTypeTarget);
        }

        private void labelTargetParam2_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                this.UpdatePermanentTooltipOfParameter(sender as Label, 2, this.comboBoxTargetType, ScriptTypeId.ScriptTypeTarget);
        }

        private void labelTargetParam3_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                this.UpdatePermanentTooltipOfParameter(sender as Label, 3, this.comboBoxTargetType, ScriptTypeId.ScriptTypeTarget);
        }

        private void labelTargetX_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                this.UpdatePermanentTooltipOfParameter(sender as Label, 4, this.comboBoxTargetType, ScriptTypeId.ScriptTypeTarget);
        }

        private void labelTargetY_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                this.UpdatePermanentTooltipOfParameter(sender as Label, 5, this.comboBoxTargetType, ScriptTypeId.ScriptTypeTarget);
        }

        private void labelTargetZ_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                this.UpdatePermanentTooltipOfParameter(sender as Label, 6, this.comboBoxTargetType, ScriptTypeId.ScriptTypeTarget);
        }

        private void labelTargetO_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowTooltipsPermanently)
                this.UpdatePermanentTooltipOfParameter(sender as Label, 7, this.comboBoxTargetType, ScriptTypeId.ScriptTypeTarget);
        }

        private async void buttonNewLine_Click(object sender, EventArgs e)
        {
            if (this.listViewSmartScripts.Items.Count == 0)
            {
                if (!Settings.Default.UseWorldDatabase)
                {
                    this.TryToCreateScript(true);
                    return;
                }

                string aiName = await SAI_Editor_Manager.Instance.worldDatabase.GetObjectAiName(XConverter.ToInt32(this.textBoxEntryOrGuid.Text), (int)this.GetSourceTypeByIndex());

                if (!SAI_Editor_Manager.Instance.IsAiNameSmartAi(aiName))
                {
                    DialogResult dialogResult = MessageBox.Show("Are you sure you want to create a new script for the given entry and sourcetype?", "Something went wrong", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (dialogResult == DialogResult.Yes)
                        this.TryToCreateScript(true);
                }
                else
                    this.TryToCreateScript(true);

                return;
            }

            this.buttonNewLine.Enabled = false;
            SmartScript newSmartScript = new SmartScript();
            newSmartScript.entryorguid = this.originalEntryOrGuidAndSourceType.entryOrGuid;
            newSmartScript.source_type = (int)this.originalEntryOrGuidAndSourceType.sourceType;

            if (this.checkBoxLockEventId.Checked)
                newSmartScript.id = ++this.lastSmartScriptIdOfScript;
            else
                newSmartScript.id = -1;

            if (Settings.Default.GenerateComments && Settings.Default.UseWorldDatabase)
                newSmartScript.comment = await CommentGenerator.Instance.GenerateCommentFor(newSmartScript, this.originalEntryOrGuidAndSourceType);
            else
                newSmartScript.comment = SAI_Editor_Manager.Instance.GetDefaultCommentForSourceType((SourceTypes)newSmartScript.source_type);

            newSmartScript.event_chance = 100;
            int index = this.listViewSmartScripts.AddSmartScript(newSmartScript);
            this.HandleShowBasicInfo();

            this.listViewSmartScripts.Items[index].Selected = true;
            this.listViewSmartScripts.Select();
            this.listViewSmartScripts.EnsureVisible(index);

            this.buttonNewLine.Enabled = this.textBoxEntryOrGuid.Text.Length > 0;
        }

        private async void textBoxLinkTo_TextChanged(object sender, EventArgs e)
        {
            if (this.listViewSmartScripts.SelectedItems.Count > 0)
            {
                if (!this.updatingFieldsBasedOnSelectedScript && this.listViewSmartScripts.SelectedSmartScript.id.ToString() == this.textBoxLinkTo.Text)
                {
                    MessageBox.Show("You can not link to or from the same id you're linking to.", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.textBoxLinkFrom.Text = this.GetLinkFromForSelection();
                    this.textBoxLinkTo.Text = "0";
                    return;
                }

                int linkTo = XConverter.ToInt32(this.textBoxLinkTo.Text);
                this.listViewSmartScripts.SelectedSmartScript.link = linkTo;
                this.listViewSmartScripts.ReplaceSmartScript(this.listViewSmartScripts.SelectedSmartScript);
                await this.GenerateCommentForSmartScript(this.listViewSmartScripts.SelectedSmartScript);

                foreach (SmartScript smartScript in this.listViewSmartScripts.SmartScripts)
                {
                    if (smartScript.id == linkTo)
                    {
                        if ((SmartEvent)smartScript.event_type == SmartEvent.SMART_EVENT_LINK)
                            await this.GenerateCommentForSmartScript(smartScript, false);

                        break;
                    }
                }
            }
        }

        private void textBoxComments_TextChanged(object sender, EventArgs e)
        {
            if (this.listViewSmartScripts.SelectedItems.Count > 0)
            {
                this.listViewSmartScripts.SelectedSmartScript.comment = this.textBoxComments.Text;
                this.listViewSmartScripts.ReplaceSmartScript(this.listViewSmartScripts.SelectedSmartScript);
                this.ResizeColumns();
            }
        }

        private async void textBoxEventPhasemask_TextChanged(object sender, EventArgs e)
        {
            if (this.listViewSmartScripts.SelectedItems.Count > 0)
            {
                this.listViewSmartScripts.SelectedSmartScript.event_phase_mask = XConverter.ToInt32(this.textBoxEventPhasemask.Text);
                this.listViewSmartScripts.ReplaceSmartScript(this.listViewSmartScripts.SelectedSmartScript);
                await this.GenerateCommentForSmartScript(this.listViewSmartScripts.SelectedSmartScript);
            }
        }

        private async void textBoxEventChance_ValueChanged(object sender, EventArgs e)
        {
            if (this.listViewSmartScripts.SelectedItems.Count > 0)
            {
                this.listViewSmartScripts.SelectedSmartScript.event_chance = (int)this.textBoxEventChance.Value; //! Using .Text propert results in wrong value
                this.listViewSmartScripts.ReplaceSmartScript(this.listViewSmartScripts.SelectedSmartScript);
                await this.GenerateCommentForSmartScript(this.listViewSmartScripts.SelectedSmartScript);
            }
        }

        private async void textBoxEventFlags_TextChanged(object sender, EventArgs e)
        {
            if (this.listViewSmartScripts.SelectedItems.Count > 0)
            {
                this.listViewSmartScripts.SelectedSmartScript.event_flags = XConverter.ToInt32(this.textBoxEventFlags.Text);
                this.listViewSmartScripts.ReplaceSmartScript(this.listViewSmartScripts.SelectedSmartScript);
                await this.GenerateCommentForSmartScript(this.listViewSmartScripts.SelectedSmartScript);
            }
        }

        private async void textBoxLinkFrom_TextChanged(object sender, EventArgs e)
        {
            int newLinkFrom = 0;// XConverter.ToInt32(textBoxLinkFrom.Text);

            try
            {
                newLinkFrom = Int32.Parse(this.textBoxLinkFrom.Text);
            }
            catch (Exception)
            {
                this.previousLinkFrom = -1;
                return;
            }

            //! Only if the property was changed by hand (by user) and not by selecting a line
            if (!this.updatingFieldsBasedOnSelectedScript)
            {
                if (newLinkFrom == this.listViewSmartScripts.SelectedSmartScript.id)
                {
                    MessageBox.Show("You can not link to or from the same id you're linking to.", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.textBoxLinkFrom.Text = this.GetLinkFromForSelection();
                    this.previousLinkFrom = -1;
                    return;
                }

                if (this.previousLinkFrom == newLinkFrom)
                    return;

                for (int i = 0; i < this.listViewSmartScripts.SmartScripts.Count; ++i)
                {
                    SmartScript smartScript = this.listViewSmartScripts.SmartScripts[i];

                    if (smartScript.entryorguid != this.originalEntryOrGuidAndSourceType.entryOrGuid || smartScript.source_type != (int)this.originalEntryOrGuidAndSourceType.sourceType)
                        continue;

                    if (smartScript.link == this.previousLinkFrom)
                    {
                        smartScript.link = 0;
                        await this.GenerateCommentForSmartScript(smartScript, false);
                    }

                    if (smartScript.id == newLinkFrom && this.listViewSmartScripts.SelectedSmartScript != null)
                    {
                        smartScript.link = this.listViewSmartScripts.SelectedSmartScript.id;
                        await this.GenerateCommentForSmartScript(smartScript, false);
                    }
                }

                this.listViewSmartScripts.Init(true);
            }

            this.previousLinkFrom = newLinkFrom;
        }

        private async void textBoxEventParam1_TextChanged(object sender, EventArgs e)
        {
            if (this.listViewSmartScripts.SelectedItems.Count > 0)
            {
                this.listViewSmartScripts.SelectedSmartScript.event_param1 = XConverter.ToInt32(this.textBoxEventParam1.Text);
                this.listViewSmartScripts.ReplaceSmartScript(this.listViewSmartScripts.SelectedSmartScript);
                await this.GenerateCommentForSmartScript(this.listViewSmartScripts.SelectedSmartScript);
            }
        }

        private async void textBoxEventParam2_TextChanged(object sender, EventArgs e)
        {
            if (this.listViewSmartScripts.SelectedItems.Count > 0)
            {
                this.listViewSmartScripts.SelectedSmartScript.event_param2 = XConverter.ToInt32(this.textBoxEventParam2.Text);
                this.listViewSmartScripts.ReplaceSmartScript(this.listViewSmartScripts.SelectedSmartScript);
                await this.GenerateCommentForSmartScript(this.listViewSmartScripts.SelectedSmartScript);
            }
        }

        private async void textBoxEventParam3_TextChanged(object sender, EventArgs e)
        {
            if (this.listViewSmartScripts.SelectedItems.Count > 0)
            {
                this.listViewSmartScripts.SelectedSmartScript.event_param3 = XConverter.ToInt32(this.textBoxEventParam3.Text);
                this.listViewSmartScripts.ReplaceSmartScript(this.listViewSmartScripts.SelectedSmartScript);
                await this.GenerateCommentForSmartScript(this.listViewSmartScripts.SelectedSmartScript);
            }
        }

        private async void textBoxEventParam4_TextChanged(object sender, EventArgs e)
        {
            if (this.listViewSmartScripts.SelectedItems.Count > 0)
            {
                this.listViewSmartScripts.SelectedSmartScript.event_param4 = XConverter.ToInt32(this.textBoxEventParam4.Text);
                this.listViewSmartScripts.ReplaceSmartScript(this.listViewSmartScripts.SelectedSmartScript);
                await this.GenerateCommentForSmartScript(this.listViewSmartScripts.SelectedSmartScript);
            }
        }

        private async void textBoxActionParam1_TextChanged(object sender, EventArgs e)
        {
            if ((SmartAction)this.comboBoxActionType.SelectedIndex == SmartAction.SMART_ACTION_INSTALL_AI_TEMPLATE)
                this.ParameterInstallAiTemplateChanged();

            if (this.listViewSmartScripts.SelectedItems.Count > 0)
            {
                this.listViewSmartScripts.SelectedSmartScript.action_param1 = XConverter.ToInt32(this.textBoxActionParam1.Text);
                this.listViewSmartScripts.ReplaceSmartScript(this.listViewSmartScripts.SelectedSmartScript);
                await this.GenerateCommentForSmartScript(this.listViewSmartScripts.SelectedSmartScript);
            }
        }

        private async void textBoxActionParam2_TextChanged(object sender, EventArgs e)
        {
            if (this.listViewSmartScripts.SelectedItems.Count > 0)
            {
                this.listViewSmartScripts.SelectedSmartScript.action_param2 = XConverter.ToInt32(this.textBoxActionParam2.Text);
                this.listViewSmartScripts.ReplaceSmartScript(this.listViewSmartScripts.SelectedSmartScript);
                await this.GenerateCommentForSmartScript(this.listViewSmartScripts.SelectedSmartScript);
            }
        }

        private async void textBoxActionParam3_TextChanged(object sender, EventArgs e)
        {
            if (this.listViewSmartScripts.SelectedItems.Count > 0)
            {
                this.listViewSmartScripts.SelectedSmartScript.action_param3 = XConverter.ToInt32(this.textBoxActionParam3.Text);
                this.listViewSmartScripts.ReplaceSmartScript(this.listViewSmartScripts.SelectedSmartScript);
                await this.GenerateCommentForSmartScript(this.listViewSmartScripts.SelectedSmartScript);
            }
        }

        private async void textBoxActionParam4_TextChanged(object sender, EventArgs e)
        {
            if (this.listViewSmartScripts.SelectedItems.Count > 0)
            {
                this.listViewSmartScripts.SelectedSmartScript.action_param4 = XConverter.ToInt32(this.textBoxActionParam4.Text);
                this.listViewSmartScripts.ReplaceSmartScript(this.listViewSmartScripts.SelectedSmartScript);
                await this.GenerateCommentForSmartScript(this.listViewSmartScripts.SelectedSmartScript);
            }
        }

        private async void textBoxActionParam5_TextChanged(object sender, EventArgs e)
        {
            if (this.listViewSmartScripts.SelectedItems.Count > 0)
            {
                this.listViewSmartScripts.SelectedSmartScript.action_param5 = XConverter.ToInt32(this.textBoxActionParam5.Text);
                this.listViewSmartScripts.ReplaceSmartScript(this.listViewSmartScripts.SelectedSmartScript);
                await this.GenerateCommentForSmartScript(this.listViewSmartScripts.SelectedSmartScript);
            }
        }

        private async void textBoxActionParam6_TextChanged(object sender, EventArgs e)
        {
            if (this.listViewSmartScripts.SelectedItems.Count > 0)
            {
                this.listViewSmartScripts.SelectedSmartScript.action_param6 = XConverter.ToInt32(this.textBoxActionParam6.Text);
                this.listViewSmartScripts.ReplaceSmartScript(this.listViewSmartScripts.SelectedSmartScript);
                await this.GenerateCommentForSmartScript(this.listViewSmartScripts.SelectedSmartScript);
            }
        }

        private async void textBoxTargetParam1_TextChanged(object sender, EventArgs e)
        {
            if (this.listViewSmartScripts.SelectedItems.Count > 0)
            {
                this.listViewSmartScripts.SelectedSmartScript.target_param1 = XConverter.ToInt32(this.textBoxTargetParam1.Text);
                this.listViewSmartScripts.ReplaceSmartScript(this.listViewSmartScripts.SelectedSmartScript);
                await this.GenerateCommentForSmartScript(this.listViewSmartScripts.SelectedSmartScript);
            }
        }

        private async void textBoxTargetParam2_TextChanged(object sender, EventArgs e)
        {
            if (this.listViewSmartScripts.SelectedItems.Count > 0)
            {
                this.listViewSmartScripts.SelectedSmartScript.target_param2 = XConverter.ToInt32(this.textBoxTargetParam2.Text);
                this.listViewSmartScripts.ReplaceSmartScript(this.listViewSmartScripts.SelectedSmartScript);
                await this.GenerateCommentForSmartScript(this.listViewSmartScripts.SelectedSmartScript);
            }
        }

        private async void textBoxTargetParam3_TextChanged(object sender, EventArgs e)
        {
            if (this.listViewSmartScripts.SelectedItems.Count > 0)
            {
                this.listViewSmartScripts.SelectedSmartScript.target_param3 = XConverter.ToInt32(this.textBoxTargetParam3.Text);
                this.listViewSmartScripts.ReplaceSmartScript(this.listViewSmartScripts.SelectedSmartScript);
                await this.GenerateCommentForSmartScript(this.listViewSmartScripts.SelectedSmartScript);
            }
        }

        private async void textBoxTargetX_TextChanged(object sender, EventArgs e)
        {
            if (this.listViewSmartScripts.SelectedItems.Count > 0)
            {
                this.textBoxTargetX.Text = this.textBoxTargetX.Text.Replace(",", ".");
                this.textBoxTargetX.SelectionStart = this.textBoxTargetX.Text.Length + 1; //! Set cursor to end of text
                this.listViewSmartScripts.SelectedSmartScript.target_x = XConverter.ToDouble(this.textBoxTargetX.Text);
                this.listViewSmartScripts.ReplaceSmartScript(this.listViewSmartScripts.SelectedSmartScript);
                await this.GenerateCommentForSmartScript(this.listViewSmartScripts.SelectedSmartScript);
            }
        }

        private async void textBoxTargetY_TextChanged(object sender, EventArgs e)
        {
            if (this.listViewSmartScripts.SelectedItems.Count > 0)
            {
                this.textBoxTargetY.Text = this.textBoxTargetY.Text.Replace(",", ".");
                this.textBoxTargetY.SelectionStart = this.textBoxTargetY.Text.Length + 1; //! Set cursor to end of text
                this.listViewSmartScripts.SelectedSmartScript.target_y = XConverter.ToDouble(this.textBoxTargetY.Text);
                this.listViewSmartScripts.ReplaceSmartScript(this.listViewSmartScripts.SelectedSmartScript);
                await this.GenerateCommentForSmartScript(this.listViewSmartScripts.SelectedSmartScript);
            }
        }

        private async void textBoxTargetZ_TextChanged(object sender, EventArgs e)
        {
            if (this.listViewSmartScripts.SelectedItems.Count > 0)
            {
                this.textBoxTargetZ.Text = this.textBoxTargetZ.Text.Replace(",", ".");
                this.textBoxTargetZ.SelectionStart = this.textBoxTargetZ.Text.Length + 1; //! Set cursor to end of text
                this.listViewSmartScripts.SelectedSmartScript.target_z = XConverter.ToDouble(this.textBoxTargetZ.Text);
                this.listViewSmartScripts.ReplaceSmartScript(this.listViewSmartScripts.SelectedSmartScript);
                await this.GenerateCommentForSmartScript(this.listViewSmartScripts.SelectedSmartScript);
            }
        }

        private async void textBoxTargetO_TextChanged(object sender, EventArgs e)
        {
            if (this.listViewSmartScripts.SelectedItems.Count > 0)
            {
                this.textBoxTargetO.Text = this.textBoxTargetO.Text.Replace(",", ".");
                this.textBoxTargetO.SelectionStart = this.textBoxTargetO.Text.Length + 1; //! Set cursor to end of text
                this.listViewSmartScripts.SelectedSmartScript.target_o = XConverter.ToDouble(this.textBoxTargetO.Text);
                this.listViewSmartScripts.ReplaceSmartScript(this.listViewSmartScripts.SelectedSmartScript);
                await this.GenerateCommentForSmartScript(this.listViewSmartScripts.SelectedSmartScript);
            }
        }

        private async void textBoxEntryOrGuid_TextChanged(object sender, EventArgs e)
        {
            this.pictureBoxLoadScript.Enabled = this.textBoxEntryOrGuid.Text.Length > 0 && Settings.Default.UseWorldDatabase;
            this.pictureBoxCreateScript.Enabled = this.textBoxEntryOrGuid.Text.Length > 0;
            this.buttonNewLine.Enabled = this.textBoxEntryOrGuid.Text.Length > 0;

            if (this.checkBoxAllowChangingEntryAndSourceType.Checked && this.listViewSmartScripts.SelectedItems.Count > 0)
            {
                this.listViewSmartScripts.SelectedSmartScript.entryorguid = XConverter.ToInt32(this.textBoxEntryOrGuid.Text);
                this.listViewSmartScripts.ReplaceSmartScript(this.listViewSmartScripts.SelectedSmartScript);
                await this.GenerateCommentForSmartScript(this.listViewSmartScripts.SelectedSmartScript);

                //! When all entryorguids are the same, also adjust the originalEntryOrGuid data
                List<EntryOrGuidAndSourceType> uniqueEntriesOrGuidsAndSourceTypes = SAI_Editor_Manager.Instance.GetUniqueEntriesOrGuidsAndSourceTypes(this.listViewSmartScripts.SmartScripts);

                if (uniqueEntriesOrGuidsAndSourceTypes != null && uniqueEntriesOrGuidsAndSourceTypes.Count == 1)
                {
                    this.originalEntryOrGuidAndSourceType.entryOrGuid = uniqueEntriesOrGuidsAndSourceTypes[0].entryOrGuid;
                    this.originalEntryOrGuidAndSourceType.sourceType = uniqueEntriesOrGuidsAndSourceTypes[0].sourceType;
                }
            }
        }

        private async void comboBoxSourceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SourceTypes newSourceType = this.GetSourceTypeByIndex();
            this.textBoxComments.Text = SAI_Editor_Manager.Instance.GetDefaultCommentForSourceType(newSourceType);

            if (this.checkBoxAllowChangingEntryAndSourceType.Checked && this.listViewSmartScripts.SelectedItems.Count > 0)
            {
                this.listViewSmartScripts.SelectedSmartScript.source_type = (int)newSourceType;
                this.listViewSmartScripts.ReplaceSmartScript(this.listViewSmartScripts.SelectedSmartScript);
                await this.GenerateCommentForSmartScript(this.listViewSmartScripts.SelectedSmartScript);
            }

            //! When no database connection can be made, only enable the search button if
            //! we're searching for areatriggers.
            this.buttonSearchForEntryOrGuid.Enabled = Settings.Default.UseWorldDatabase || newSourceType == SourceTypes.SourceTypeAreaTrigger;
        }

        private async void generateSQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.formState != FormState.FormStateMain)
                return;

            using (SqlOutputForm sqlOutputForm = new SqlOutputForm(await this.GenerateSmartAiSqlFromListView(), await this.GenerateSmartAiRevertQuery()))
                sqlOutputForm.ShowDialog(this);
        }

        private async void buttonGenerateSql_Click(object sender, EventArgs e)
        {
            if (this.formState != FormState.FormStateMain)
                return;

            using (SqlOutputForm sqlOutputForm = new SqlOutputForm(await this.GenerateSmartAiSqlFromListView(), await this.GenerateSmartAiRevertQuery()))
                sqlOutputForm.ShowDialog(this);
        }

        private async Task<string> GenerateSmartAiSqlFromListView()
        {
            List<EntryOrGuidAndSourceType> entriesOrGuidsAndSourceTypes = SAI_Editor_Manager.Instance.GetUniqueEntriesOrGuidsAndSourceTypes(this.listViewSmartScripts.SmartScripts);
            string generatedSql = String.Empty, sourceName = String.Empty;

            Dictionary<SourceTypes, List<EntryOrGuidAndSourceType>> entriesOrGuidsAndSourceTypesPerSourceType = new Dictionary<SourceTypes, List<EntryOrGuidAndSourceType>>();

            if (entriesOrGuidsAndSourceTypes.Count > 1)
            {
                foreach (EntryOrGuidAndSourceType entryOrGuidAndSourceType in entriesOrGuidsAndSourceTypes)
                {
                    if (!entriesOrGuidsAndSourceTypesPerSourceType.ContainsKey(entryOrGuidAndSourceType.sourceType))
                    {
                        List<EntryOrGuidAndSourceType> _newEntryOrGuidAndSourceType = new List<EntryOrGuidAndSourceType>();
                        _newEntryOrGuidAndSourceType.Add(entryOrGuidAndSourceType);
                        entriesOrGuidsAndSourceTypesPerSourceType[entryOrGuidAndSourceType.sourceType] = _newEntryOrGuidAndSourceType;
                    }
                    else
                        entriesOrGuidsAndSourceTypesPerSourceType[entryOrGuidAndSourceType.sourceType].Add(entryOrGuidAndSourceType);
                }
            }

            switch (this.originalEntryOrGuidAndSourceType.sourceType)
            {
                case SourceTypes.SourceTypeCreature:
                case SourceTypes.SourceTypeGameobject:
                    if (!Settings.Default.UseWorldDatabase)
                    {
                        sourceName = " <Could not generate name>";
                        break;
                    }

                    sourceName = " " + await SAI_Editor_Manager.Instance.worldDatabase.GetObjectNameByIdOrGuidAndSourceType(this.originalEntryOrGuidAndSourceType.sourceType, this.originalEntryOrGuidAndSourceType.entryOrGuid);
                    break;
                case SourceTypes.SourceTypeAreaTrigger:
                    sourceName = " Areatrigger";
                    break;
                case SourceTypes.SourceTypeScriptedActionlist:
                    if (entriesOrGuidsAndSourceTypes.Count > 1)
                    {
                        if (!Settings.Default.UseWorldDatabase)
                        {
                            sourceName = " <Could not generate name>";
                            break;
                        }

                        foreach (List<EntryOrGuidAndSourceType> listEntryOrGuidAndSourceTypes in entriesOrGuidsAndSourceTypesPerSourceType.Values)
                        {
                            foreach (EntryOrGuidAndSourceType entryOrGuidAndSourceType in listEntryOrGuidAndSourceTypes)
                            {
                                if (entryOrGuidAndSourceType.sourceType != SourceTypes.SourceTypeGameobject && entryOrGuidAndSourceType.sourceType != SourceTypes.SourceTypeCreature)
                                    continue;

                                sourceName = " " + await SAI_Editor_Manager.Instance.worldDatabase.GetObjectNameByIdOrGuidAndSourceType(entryOrGuidAndSourceType.sourceType, entryOrGuidAndSourceType.entryOrGuid);
                                break;
                            }
                        }
                    }
                    else
                        sourceName = " Actionlist";

                    break;
                default:
                    sourceName = " <Could not generate name>";
                    break;
            }

            bool originalEntryIsGuid = this.originalEntryOrGuidAndSourceType.entryOrGuid < 0;
            string sourceSet = originalEntryIsGuid ? "@GUID" : "@ENTRY";

            generatedSql += "--" + sourceName + " SAI\n";
            generatedSql += "SET " + sourceSet + " := " + this.originalEntryOrGuidAndSourceType.entryOrGuid + ";\n";

            if (entriesOrGuidsAndSourceTypes.Count == 1)
            {
                switch ((SourceTypes)this.originalEntryOrGuidAndSourceType.sourceType)
                {
                    case SourceTypes.SourceTypeCreature:
                        if (!Settings.Default.UseWorldDatabase)
                        {
                            generatedSql += "-- No changes to the AIName were made as there is no world database connection.\n";
                            break;
                        }

                        if (originalEntryIsGuid)
                        {
                            int actualEntry = await SAI_Editor_Manager.Instance.worldDatabase.GetCreatureIdByGuid(-this.originalEntryOrGuidAndSourceType.entryOrGuid);
                            generatedSql += "UPDATE `creature_template` SET `AIName`=" + '"' + "SmartAI" + '"' + " WHERE `entry`=" + actualEntry + ";\n";
                        }
                        else
                            generatedSql += "UPDATE `creature_template` SET `AIName`=" + '"' + "SmartAI" + '"' + " WHERE `entry`=" + sourceSet + ";\n";

                        break;
                    case SourceTypes.SourceTypeGameobject:
                        if (!Settings.Default.UseWorldDatabase)
                        {
                            generatedSql += "-- No changes to the AIName were made as there is no world database connection.\n";
                            break;
                        }

                        if (originalEntryIsGuid)
                        {
                            int actualEntry = await SAI_Editor_Manager.Instance.worldDatabase.GetGameobjectIdByGuid(-this.originalEntryOrGuidAndSourceType.entryOrGuid);
                            generatedSql += "UPDATE `gameobject_template` SET `AIName`=" + '"' + "SmartGameObjectAI" + '"' + " WHERE `entry`=" + actualEntry + ";\n";
                        }
                        else
                            generatedSql += "UPDATE `gameobject_template` SET `AIName`=" + '"' + "SmartGameObjectAI" + '"' + " WHERE `entry`=" + sourceSet + ";\n";

                        break;
                    case SourceTypes.SourceTypeAreaTrigger:
                        generatedSql += "DELETE FROM `areatrigger_scripts` WHERE `entry`=" + sourceSet + ";\n";
                        generatedSql += "INSERT INTO `areatrigger_scripts` (`entry`,`ScriptName`) VALUES (" + sourceSet + "," + '"' + "SmartTrigger" + '"' + ");\n";
                        break;
                    case SourceTypes.SourceTypeScriptedActionlist:
                        // todo
                        break;
                }

                generatedSql += "DELETE FROM `smart_scripts` WHERE `entryorguid`=" + sourceSet + " AND `source_type`=" + (int)this.originalEntryOrGuidAndSourceType.sourceType + ";\n";
            }
            else if (entriesOrGuidsAndSourceTypes.Count > 1)
            {
                foreach (List<EntryOrGuidAndSourceType> listEntryOrGuidAndSourceTypes in entriesOrGuidsAndSourceTypesPerSourceType.Values)
                {
                    bool generatedInitialUpdateQuery = false;

                    for (int i = 0; i < listEntryOrGuidAndSourceTypes.Count; ++i)
                    {
                        EntryOrGuidAndSourceType entryOrGuidAndSourceType = listEntryOrGuidAndSourceTypes[i];

                        switch (entryOrGuidAndSourceType.sourceType)
                        {
                            case SourceTypes.SourceTypeCreature:
                            case SourceTypes.SourceTypeGameobject:
                                string entryOrGuidToUse = entryOrGuidAndSourceType.entryOrGuid.ToString();
                                bool sourceTypeIsCreature = entryOrGuidAndSourceType.sourceType == SourceTypes.SourceTypeCreature;
                                string tableToTarget = sourceTypeIsCreature ? "creature_template" : "gameobject_template";
                                string newAiName = sourceTypeIsCreature ? "SmartAI" : "SmartGameObjectAI";

                                if (entryOrGuidAndSourceType.entryOrGuid == this.originalEntryOrGuidAndSourceType.entryOrGuid)
                                    entryOrGuidToUse = sourceSet;

                                if (entryOrGuidAndSourceType.entryOrGuid < 0)
                                {
                                    if (!Settings.Default.UseWorldDatabase)
                                    {
                                        generatedSql += "-- No changes to the AIName were made as there is no world database connection.";
                                        break;
                                    }

                                    entryOrGuidToUse = (await SAI_Editor_Manager.Instance.worldDatabase.GetObjectIdByGuidAndSourceType(-entryOrGuidAndSourceType.entryOrGuid, (int)entryOrGuidAndSourceType.sourceType)).ToString();

                                    if (entryOrGuidToUse == "0")
                                    {
                                        string sourceTypeString = this.GetSourceTypeString(entryOrGuidAndSourceType.sourceType);
                                        string message = "While generating a script for your SmartAI, the " + sourceTypeString + " guid ";
                                        message += -entryOrGuidAndSourceType.entryOrGuid + " was not spawned in your current database which means the AIName was not properly set.";
                                        message += "\n\nThis is only a warning, which means the AIName of entry 0 will be set in `" + sourceTypeString + "_template` and this has no effect.";
                                        MessageBox.Show(message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }

                                if (listEntryOrGuidAndSourceTypes.Count > 1)
                                {
                                    if (!generatedInitialUpdateQuery)
                                    {
                                        generatedSql += "UPDATE `" + tableToTarget + "` SET `AIName`=" + '"' + newAiName + '"' + " WHERE `entry` IN (" + entryOrGuidToUse;
                                        generatedInitialUpdateQuery = true;
                                    }
                                    else
                                        generatedSql += "," + entryOrGuidToUse;

                                    if (i == listEntryOrGuidAndSourceTypes.Count - 1)
                                        generatedSql += ");\n";
                                }
                                else
                                    generatedSql += "UPDATE `" + tableToTarget + "` SET `AIName`=" + '"' + newAiName + '"' + " WHERE `entry`=" + entryOrGuidToUse + ";\n";

                                break;
                            case SourceTypes.SourceTypeAreaTrigger:
                                generatedSql += "DELETE FROM `areatrigger_scripts` WHERE `entry`=" + sourceSet + ";\n";
                                generatedSql += "INSERT INTO `areatrigger_scripts` VALUES (" + sourceSet + "," + '"' + "SmartTrigger" + '"' + ");\n";
                                break;
                            case SourceTypes.SourceTypeScriptedActionlist:
                                // todo
                                break;
                        }
                    }
                }

                foreach (List<EntryOrGuidAndSourceType> listEntryOrGuidAndSourceTypes in entriesOrGuidsAndSourceTypesPerSourceType.Values)
                {
                    //! If there are more entries
                    if (listEntryOrGuidAndSourceTypes.Count > 1)
                    {
                        generatedSql += "DELETE FROM `smart_scripts` WHERE `entryorguid` IN (";
                        SourceTypes sourceTypeOfSource = SourceTypes.SourceTypeNone;

                        for (int i = 0; i < listEntryOrGuidAndSourceTypes.Count; ++i)
                        {
                            EntryOrGuidAndSourceType entryOrGuidAndSourceType = listEntryOrGuidAndSourceTypes[i];
                            sourceTypeOfSource = entryOrGuidAndSourceType.sourceType;

                            if (entryOrGuidAndSourceType.entryOrGuid == this.originalEntryOrGuidAndSourceType.entryOrGuid)
                                generatedSql += sourceSet;
                            else
                                generatedSql += entryOrGuidAndSourceType.entryOrGuid;

                            if (i != listEntryOrGuidAndSourceTypes.Count - 1)
                                generatedSql += ",";
                            else
                                generatedSql += ")";
                        }

                        generatedSql += " AND `source_type`=" + (int)sourceTypeOfSource + ";\n";
                    }
                    else if (listEntryOrGuidAndSourceTypes.Count == 1)
                    {
                        generatedSql += "DELETE FROM `smart_scripts` WHERE `entryorguid`=";

                        if (listEntryOrGuidAndSourceTypes[0].entryOrGuid == this.originalEntryOrGuidAndSourceType.entryOrGuid)
                            generatedSql += sourceSet;
                        else
                            generatedSql += listEntryOrGuidAndSourceTypes[0].entryOrGuid;

                        generatedSql += " AND `source_type`=" + (int)listEntryOrGuidAndSourceTypes[0].sourceType + ";\n";
                    }
                    else
                        generatedSql += "-- No 'DELETE FROM `smart_scripts` WHERE ...' query could be generated as the size of listEntryOrGuidAndSourceTypes is not correct (" + listEntryOrGuidAndSourceTypes.Count + ").";
                }
            }

            generatedSql += "INSERT INTO `smart_scripts` (`entryorguid`,`source_type`,`id`,`link`,`event_type`,`event_phase_mask`,`event_chance`,`event_flags`,`event_param1`,`event_param2`,`event_param3`,`event_param4`,`action_type`,`action_param1`,`action_param2`,`action_param3`,`action_param4`,`action_param5`,`action_param6`,`target_type`,`target_param1`,`target_param2`,`target_param3`,`target_x`,`target_y`,`target_z`,`target_o`,`comment`) VALUES\n";

            List<SmartScript> smartScripts = this.listViewSmartScripts.SmartScripts;
            smartScripts = smartScripts.OrderBy(smartScript => smartScript.entryorguid).ToList();

            for (int i = 0; i < smartScripts.Count; ++i)
            {
                SmartScript smartScript = smartScripts[i];
                string actualSourceSet = sourceSet;

                if (this.originalEntryOrGuidAndSourceType.entryOrGuid != smartScripts[i].entryorguid)
                    actualSourceSet = smartScripts[i].entryorguid.ToString();

                int[] eventParameters = new int[4];
                eventParameters[0] = smartScript.event_param1;
                eventParameters[1] = smartScript.event_param2;
                eventParameters[2] = smartScript.event_param3;
                eventParameters[3] = smartScript.event_param4;

                int[] actionParameters = new int[6];
                actionParameters[0] = smartScript.action_param1;
                actionParameters[1] = smartScript.action_param2;
                actionParameters[2] = smartScript.action_param3;
                actionParameters[3] = smartScript.action_param4;
                actionParameters[4] = smartScript.action_param5;
                actionParameters[5] = smartScript.action_param6;

                int[] targetParameters = new int[3];
                targetParameters[0] = smartScript.target_param1;
                targetParameters[1] = smartScript.target_param2;
                targetParameters[2] = smartScript.target_param3;

                for (int x = 0; x < 6; ++x)
                {
                    if (x < 4)
                        if (eventParameters[x].ToString() == sourceSet)
                            eventParameters[x] = XConverter.ToInt32(sourceSet);

                    if (actionParameters[x].ToString() == sourceSet)
                        actionParameters[x] = XConverter.ToInt32(sourceSet);

                    if (x < 3)
                        if (targetParameters[x].ToString() == sourceSet)
                            targetParameters[x] = XConverter.ToInt32(sourceSet);
                }

                generatedSql += "(" + actualSourceSet + "," + smartScript.source_type + "," + smartScript.id + "," + smartScript.link + "," + smartScript.event_type + "," +
                                              smartScript.event_phase_mask + "," + smartScript.event_chance + "," + smartScript.event_flags + "," + eventParameters[0] + "," +
                                              eventParameters[1] + "," + eventParameters[2] + "," + eventParameters[3] + "," + smartScript.action_type + "," +
                                              actionParameters[0] + "," + actionParameters[1] + "," + actionParameters[2] + "," + actionParameters[3] + "," +
                                              actionParameters[4] + "," + actionParameters[5] + "," + smartScript.target_type + "," + targetParameters[0] + "," +
                                              targetParameters[1] + "," + targetParameters[2] + "," + smartScript.target_x + "," + smartScript.target_y + "," +
                                              smartScript.target_z + "," + smartScript.target_o + "," + '"' + smartScript.comment + '"' + ")";

                if (i == smartScripts.Count - 1)
                    generatedSql += ";";
                else
                    generatedSql += ",";

                generatedSql += "\n"; //! White line at end of script to make it easier to select
            }

            //! Replaces '<entry>0[id]' ([id] being like 00, 01, 02, 03, etc.) by '@ENTRY*100+03' for example.
            //! Example: replaces 2891401 by @ENTRY*100+01 if original entryorguid is 28914.
            for (int i = 0; i < 50; ++i) // Regex.Matches(generatedSql, originalEntryOrGuidAndSourceType.entryOrGuid + "0" + i.ToString()).Count
            {
                string[] charactersToReplace = new string[3] { ",", ")", " " };

                for (int j = 0; j < 3; ++j)
                {
                    string characterToReplace = charactersToReplace[j];
                    string stringToReplace = this.originalEntryOrGuidAndSourceType.entryOrGuid + "0" + i.ToString() + characterToReplace;

                    if (!generatedSql.Contains(stringToReplace))
                    {
                        stringToReplace = this.originalEntryOrGuidAndSourceType.entryOrGuid + "0" + i.ToString() + ")";

                        if (!generatedSql.Contains(stringToReplace))
                            continue;
                    }

                    string _i = i.ToString();

                    if (i < 10)
                        _i = "0" + _i.Substring(0);

                    generatedSql = generatedSql.Replace(stringToReplace, sourceSet + "*100+" + _i + characterToReplace);
                }
            }

            return generatedSql;
        }

        private async Task<string> GenerateSmartAiRevertQuery()
        {
            if (!Settings.Default.UseWorldDatabase)
                return String.Empty;

            string revertQuery = String.Empty;
            List<EntryOrGuidAndSourceType> entriesOrGuidsAndSourceTypes = SAI_Editor_Manager.Instance.GetUniqueEntriesOrGuidsAndSourceTypes(this.listViewSmartScripts.SmartScripts);

            foreach (EntryOrGuidAndSourceType entryOrGuidAndSourceType in entriesOrGuidsAndSourceTypes)
            {
                List<SmartScript> smartScripts = await SAI_Editor_Manager.Instance.worldDatabase.GetSmartScripts(entryOrGuidAndSourceType.entryOrGuid, (int)entryOrGuidAndSourceType.sourceType);
                string scriptName = String.Empty, aiName = String.Empty;

                switch (entryOrGuidAndSourceType.sourceType)
                {
                    case SourceTypes.SourceTypeCreature:
                        scriptName = await SAI_Editor_Manager.Instance.worldDatabase.GetCreatureScriptNameById(entryOrGuidAndSourceType.entryOrGuid);
                        aiName = await SAI_Editor_Manager.Instance.worldDatabase.GetCreatureAiNameById(entryOrGuidAndSourceType.entryOrGuid);

                        revertQuery += "UPDATE creature_template SET Ainame='" + aiName + "',Scriptname='" + scriptName + "' WHERE entry='" + entryOrGuidAndSourceType.entryOrGuid + "';";
                        break;
                    case SourceTypes.SourceTypeGameobject:
                        scriptName = await SAI_Editor_Manager.Instance.worldDatabase.GetGameobjectScriptNameById(entryOrGuidAndSourceType.entryOrGuid);
                        aiName = await SAI_Editor_Manager.Instance.worldDatabase.GetGameobjectAiNameById(entryOrGuidAndSourceType.entryOrGuid);

                        revertQuery += "UPDATE gameobject_template SET Ainame='" + aiName + "',Scriptname='" + await SAI_Editor_Manager.Instance.worldDatabase.GetGameobjectScriptNameById(entryOrGuidAndSourceType.entryOrGuid) + "' WHERE entry='" + entryOrGuidAndSourceType.entryOrGuid + "';";
                        break;
                    case SourceTypes.SourceTypeAreaTrigger:
                        scriptName = await SAI_Editor_Manager.Instance.worldDatabase.GetAreaTriggerScriptNameById(entryOrGuidAndSourceType.entryOrGuid);

                        if (scriptName != String.Empty)
                            revertQuery += "UPDATE areatrigger_scripts SET Scriptname='" + scriptName + "' WHERE entry='" + entryOrGuidAndSourceType.entryOrGuid + "';";
                        else
                            revertQuery += "DELETE FROM areatrigger_scripts WHERE entry='" + entryOrGuidAndSourceType.entryOrGuid + "';";

                        break;
                }

                if (smartScripts != null && smartScripts.Count > 0)
                {
                    revertQuery += "DELETE FROM smart_scripts WHERE entryorguid=" + smartScripts[0].entryorguid.ToString() + ";";
                    revertQuery += "REPLACE INTO smart_scripts VALUES ";

                    for (int i = 0; i < smartScripts.Count; ++i)
                    {
                        SmartScript smartScript = smartScripts[i];
                        revertQuery += "(";
                        revertQuery += String.Format("{0},{1},{2},{3},", smartScript.entryorguid, smartScript.source_type, smartScript.id, smartScript.link);
                        revertQuery += String.Format("{0},{1},{2},{3},", smartScript.event_type, smartScript.event_phase_mask, smartScript.event_chance, smartScript.event_flags);
                        revertQuery += String.Format("{0},{1},{2},{3},", smartScript.event_param1, smartScript.event_param2, smartScript.event_param3, smartScript.event_param4);
                        revertQuery += String.Format("{0},{1},{2},{3},", smartScript.action_type, smartScript.action_param1, smartScript.action_param2, smartScript.action_param3);
                        revertQuery += String.Format("{0},{1},{2},{3},", smartScript.action_param4, smartScript.action_param5, smartScript.action_param6, smartScript.target_type);
                        revertQuery += String.Format("{0},{1},{2},{3},", smartScript.target_param1, smartScript.target_param2, smartScript.target_param3, smartScript.target_x);
                        revertQuery += String.Format("{0},{1},{2}," + '"' + "{3}" + '"', smartScript.target_y, smartScript.target_z, smartScript.target_o, smartScript.comment);
                        revertQuery += ")";

                        if (i == smartScripts.Count - 1)
                            revertQuery += ";";
                        else
                            revertQuery += ",";
                    }
                }
                else
                    revertQuery += "DELETE FROM smart_scripts WHERE entryorguid='" + entryOrGuidAndSourceType.entryOrGuid + "';";
            }

            return revertQuery;
        }

        public async void GenerateCommentsForAllItems()
        {
            if (this.listViewSmartScripts.SmartScripts.Count == 0)
                return;

            for (int i = 0; i < this.listViewSmartScripts.SmartScripts.Count; ++i)
            {
                SmartScript smartScript = this.listViewSmartScripts.SmartScripts[i];
                SmartScript smartScriptLink = this.GetInitialSmartScriptLink(smartScript);
                string newComment = await CommentGenerator.Instance.GenerateCommentFor(smartScript, this.originalEntryOrGuidAndSourceType, true, smartScriptLink);
                smartScript.comment = newComment;
                this.listViewSmartScripts.ReplaceSmartScript(smartScript);
                this.FillFieldsBasedOnSelectedScript();
            }

            this.textBoxComments.Text = this.listViewSmartScripts.SelectedSmartScript.comment;
        }

        private void buttonGenerateComments_Click(object sender, EventArgs e)
        {
            if (!Settings.Default.UseWorldDatabase)
                return;

            this.GenerateCommentsForAllItems();
            this.ResizeColumns();
            this.listViewSmartScripts.Select();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.SaveLastUsedFields();
        }

        private void SaveLastUsedFields()
        {
            Settings.Default.LastEntryOrGuid = this.textBoxEntryOrGuid.Text;
            Settings.Default.LastSourceType = this.comboBoxSourceType.SelectedIndex;
            Settings.Default.ShowBasicInfo = this.checkBoxShowBasicInfo.Checked;
            Settings.Default.LockSmartScriptId = this.checkBoxLockEventId.Checked;
            Settings.Default.ListActionLists = this.checkBoxListActionlistsOrEntries.Checked;
            Settings.Default.AllowChangingEntryAndSourceType = this.checkBoxAllowChangingEntryAndSourceType.Checked;
            Settings.Default.PhaseHighlighting = this.checkBoxUsePhaseColors.Checked;
            Settings.Default.ShowTooltipsPermanently = this.checkBoxUsePermanentTooltips.Checked;

            if (this.formState == FormState.FormStateMain)
                Settings.Default.MainFormHeight = this.Height;

            if (this.formState == FormState.FormStateLogin)
            {
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] buffer = new byte[1024];
                rng.GetBytes(buffer);
                string salt = BitConverter.ToString(buffer);
                rng.Dispose();

                Settings.Default.Entropy = salt;
                Settings.Default.Host = this.textBoxHost.Text;
                Settings.Default.User = this.textBoxUsername.Text;
                Settings.Default.Password = this.textBoxPassword.Text.Length == 0 ? String.Empty : this.textBoxPassword.Text.ToSecureString().EncryptString(Encoding.Unicode.GetBytes(salt));
                Settings.Default.Database = this.textBoxWorldDatabase.Text;
                Settings.Default.Port = XConverter.ToUInt32(this.textBoxPort.Text);
                Settings.Default.UseWorldDatabase = this.radioButtonConnectToMySql.Checked;
                Settings.Default.AutoConnect = this.checkBoxAutoConnect.Checked;
            }

            Settings.Default.Save();
        }

        private void ResizeColumns()
        {
            foreach (ColumnHeader header in this.listViewSmartScripts.Columns)
                header.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private async Task GenerateCommentForSmartScript(SmartScript smartScript, bool resize = true)
        {
            if (smartScript == null || !Settings.Default.GenerateComments)
                return;

            SmartScript smartScriptLink = this.GetInitialSmartScriptLink(smartScript);
            string newComment = smartScript.comment;

            if (!this.updatingFieldsBasedOnSelectedScript)
            {
                newComment = await CommentGenerator.Instance.GenerateCommentFor(smartScript, this.originalEntryOrGuidAndSourceType, true, smartScriptLink);

                if (smartScript.link != 0 && (SmartEvent)smartScript.event_type != SmartEvent.SMART_EVENT_LINK)
                    await this.GenerateCommentForAllEventsLinkingFromSmartScript(smartScript);
            }

            //! For some reason we have to re-check it here...
            if (this.listViewSmartScripts.SelectedItems.Count == 0)
                return;

            string oldComment = smartScript.comment;
            smartScript.comment = newComment;
            this.listViewSmartScripts.ReplaceSmartScript(smartScript);
            this.FillFieldsBasedOnSelectedScript();

            if (oldComment != newComment)
                this.ResizeColumns();
        }

        private async Task GenerateCommentForAllEventsLinkingFromSmartScript(SmartScript smartScript)
        {
            if (smartScript == null || !Settings.Default.GenerateComments || smartScript.link == 0)
                return;

            List<SmartScript> smartScriptsLinkedFrom = this.GetAllSmartScriptThatLinkFrom(smartScript);

            if (smartScriptsLinkedFrom == null || smartScriptsLinkedFrom.Count == 0)
                return;

            for (int i = 0; i < smartScriptsLinkedFrom.Count; ++i)
            {
                SmartScript smartScriptListView = smartScriptsLinkedFrom[i];

                if (smartScriptListView.entryorguid != smartScript.entryorguid)
                    continue;

                smartScriptListView.comment = await CommentGenerator.Instance.GenerateCommentFor(smartScriptListView, this.originalEntryOrGuidAndSourceType, true, smartScript);
                this.listViewSmartScripts.ReplaceSmartScript(smartScriptListView);
            }
        }

        private async Task GenerateCommentForAllEventsLinkingFromAndToSmartScript(SmartScript smartScript)
        {
            if (smartScript == null || !Settings.Default.GenerateComments)
                return;

            for (int i = 0; i < this.listViewSmartScripts.SmartScripts.Count; ++i)
            {
                SmartScript smartScriptListView = this.listViewSmartScripts.SmartScripts[i];

                if (smartScriptListView.entryorguid != smartScript.entryorguid)
                    continue;

                if (smartScript.link == smartScriptListView.id)
                {
                    smartScriptListView.comment = await CommentGenerator.Instance.GenerateCommentFor(smartScriptListView, this.originalEntryOrGuidAndSourceType, true, this.GetInitialSmartScriptLink(smartScriptListView));
                    this.listViewSmartScripts.ReplaceSmartScript(smartScriptListView);
                }
                else if (smartScriptListView.link == smartScript.id)
                {
                    smartScript.comment = await CommentGenerator.Instance.GenerateCommentFor(smartScript, this.originalEntryOrGuidAndSourceType, true, this.GetInitialSmartScriptLink(smartScript));
                    this.listViewSmartScripts.ReplaceSmartScript(smartScript);
                }
            }
        }

        private SmartScript GetInitialSmartScriptLink(SmartScript smartScript)
        {
            if (smartScript == null || (SmartEvent)smartScript.event_type != SmartEvent.SMART_EVENT_LINK)
                return null;

            SmartScript smartScriptLink = null;
            int idToCheck = smartScript.id;

        GetLinkForCurrentSmartScriptLink:
            foreach (SmartScript smartScriptInListView in this.listViewSmartScripts.SmartScripts)
            {
                if (smartScriptInListView.link == idToCheck)
                {
                    smartScriptLink = smartScriptInListView;
                    break;
                }
            }

            if (smartScriptLink != null && (SmartEvent)smartScriptLink.event_type == SmartEvent.SMART_EVENT_LINK)
            {
                idToCheck = smartScriptLink.id;
                goto GetLinkForCurrentSmartScriptLink;
            }

            return smartScriptLink;
        }

        //! MUST take initial smartscript of linkings
        private List<SmartScript> GetAllSmartScriptThatLinkFrom(SmartScript smartScriptInitial)
        {
            if (smartScriptInitial == null || smartScriptInitial.link == 0)
                return null;

            List<SmartScript> smartScriptsLinking = new List<SmartScript>();
            smartScriptsLinking.Add(smartScriptInitial);
            SmartScript lastInitialSmartScript = smartScriptInitial;

            foreach (SmartScript smartScriptInListView in this.listViewSmartScripts.SmartScripts)
            {
                if ((SmartEvent)smartScriptInListView.event_type != SmartEvent.SMART_EVENT_LINK)
                    continue;

                if (smartScriptInListView.id == lastInitialSmartScript.link)
                {
                    smartScriptsLinking.Add(smartScriptInListView);
                    lastInitialSmartScript = smartScriptInListView;
                }
            }

            return smartScriptsLinking;
        }

        private void menuItemRevertQuery_Click(object sender, EventArgs e)
        {
            if (this.formState != FormState.FormStateMain)
                return;

            using (RevertQueryForm revertQueryForm = new RevertQueryForm())
                revertQueryForm.ShowDialog(this);
        }

        private void checkBoxShowBasicInfo_CheckedChanged(object sender, EventArgs e)
        {
            this.HandleShowBasicInfo();
        }

        private void HandleShowBasicInfo()
        {
            int prevSelectedIndex = this.listViewSmartScripts.SelectedItems.Count > 0 ? this.listViewSmartScripts.SelectedItems[0].Index : 0;

            List<string> properties = new List<string>();

            properties.Add("event_phase_mask");
            properties.Add("event_chance");
            properties.Add("event_flags");
            properties.Add("event_param1");
            properties.Add("event_param2");
            properties.Add("event_param3");
            properties.Add("event_param4");
            properties.Add("action_param1");
            properties.Add("action_param2");
            properties.Add("action_param3");
            properties.Add("action_param4");
            properties.Add("action_param5");
            properties.Add("action_param6");
            properties.Add("target_param1");
            properties.Add("target_param2");
            properties.Add("target_param3");
            properties.Add("target_x");
            properties.Add("target_y");
            properties.Add("target_z");
            properties.Add("target_o");

            if (this.checkBoxShowBasicInfo.Checked)
                this.listViewSmartScripts.ExcludeProperties(properties);
            else
                this.listViewSmartScripts.IncludeProperties(properties);

            if (this.listViewSmartScripts.Items.Count > prevSelectedIndex)
            {
                this.listViewSmartScripts.Items[prevSelectedIndex].Selected = true;
                this.listViewSmartScripts.EnsureVisible(prevSelectedIndex);
            }

            this.listViewSmartScripts.Select(); //! Sets the focus on the listview
        }

        private async void menuItemGenerateCommentListView_Click(object sender, EventArgs e)
        {
            if (this.formState != FormState.FormStateMain || this.listViewSmartScripts.SelectedSmartScript == null || !Settings.Default.UseWorldDatabase)
                return;

            for (int i = 0; i < this.listViewSmartScripts.SmartScripts.Count; ++i)
            {
                SmartScript smartScript = this.listViewSmartScripts.SmartScripts[i];

                if (smartScript != this.listViewSmartScripts.SelectedSmartScript)
                    continue;

                SmartScript smartScriptLink = this.GetInitialSmartScriptLink(smartScript);
                string newComment = await CommentGenerator.Instance.GenerateCommentFor(smartScript, this.originalEntryOrGuidAndSourceType, true, smartScriptLink);
                smartScript.comment = newComment;
                this.listViewSmartScripts.ReplaceSmartScript(smartScript);
                this.FillFieldsBasedOnSelectedScript();
            }

            this.textBoxComments.Text = this.listViewSmartScripts.SelectedSmartScript.comment;
        }

        private void menuItemDuplicateSelectedRow_Click(object sender, EventArgs e)
        {
            if (this.formState != FormState.FormStateMain || this.listViewSmartScripts.SelectedSmartScript == null)
                return;

            this.listViewSmartScripts.EnsureVisible(this.listViewSmartScripts.AddSmartScript(this.listViewSmartScripts.SelectedSmartScript.Clone(), false, true));
        }

        private void textBoxEventType_MouseWheel(object sender, MouseEventArgs e)
        {
            int newNumber = XConverter.ToInt32(this.textBoxEventType.Text);

            if (e.Delta > 0)
                newNumber--;
            else
                newNumber++;

            if (newNumber < 0)
                newNumber = 0;

            if (newNumber > (int)MaxValues.MaxEventType)
                newNumber = (int)MaxValues.MaxEventType;

            this.textBoxEventType.Text = newNumber.ToString();
        }

        private void textBoxActionType_MouseWheel(object sender, MouseEventArgs e)
        {
            int newNumber = XConverter.ToInt32(this.textBoxActionType.Text);

            if (e.Delta > 0)
                newNumber--;
            else
                newNumber++;

            if (newNumber < 0)
                newNumber = 0;

            if (newNumber > (int)MaxValues.MaxActionType)
                newNumber = (int)MaxValues.MaxActionType;

            this.textBoxActionType.Text = newNumber.ToString();
        }

        private void textBoxTargetType_MouseWheel(object sender, MouseEventArgs e)
        {
            int newNumber = XConverter.ToInt32(this.textBoxTargetType.Text);

            if (e.Delta > 0)
                newNumber--;
            else
                newNumber++;

            if (newNumber < 0)
                newNumber = 0;

            if (newNumber > (int)MaxValues.MaxTargetType)
                newNumber = (int)MaxValues.MaxTargetType;

            this.textBoxTargetType.Text = newNumber.ToString();
        }

        private void menuItemLoadSelectedEntry_Click(object sender, EventArgs e)
        {
            if (this.listViewSmartScripts.SelectedSmartScript == null)
                return;

            int entryorguid = this.listViewSmartScripts.SelectedSmartScript.entryorguid;
            SourceTypes source_type = (SourceTypes)this.listViewSmartScripts.SelectedSmartScript.source_type;
            this.listViewSmartScripts.ReplaceSmartScripts(new List<SmartScript>());
            this.listViewSmartScripts.Items.Clear();
            this.TryToLoadScript(entryorguid, source_type);
        }

        private async void textBoxId_TextChanged(object sender, EventArgs e)
        {
            if (this.listViewSmartScripts.SelectedItems.Count > 0)
            {
                this.listViewSmartScripts.SelectedSmartScript.id = XConverter.ToInt32(this.textBoxId.Text);
                this.listViewSmartScripts.ReplaceSmartScript(this.listViewSmartScripts.SelectedSmartScript);
                await this.GenerateCommentForSmartScript(this.listViewSmartScripts.SelectedSmartScript);
            }
        }

        private void radioButtonConnectToMySql_CheckedChanged(object sender, EventArgs e)
        {
            this.HandleRadioButtonUseDatabaseChanged();
        }

        private void radioButtonDontUseDatabase_CheckedChanged(object sender, EventArgs e)
        {
            this.HandleRadioButtonUseDatabaseChanged();
        }

        private void HandleRadioButtonUseDatabaseChanged()
        {
            this.textBoxHost.Enabled = this.radioButtonConnectToMySql.Checked;
            this.textBoxUsername.Enabled = this.radioButtonConnectToMySql.Checked;
            this.textBoxPassword.Enabled = this.radioButtonConnectToMySql.Checked;
            this.textBoxWorldDatabase.Enabled = this.radioButtonConnectToMySql.Checked;
            this.textBoxPort.Enabled = this.radioButtonConnectToMySql.Checked;
            this.buttonSearchWorldDb.Enabled = this.radioButtonConnectToMySql.Checked;
            this.labelDontUseDatabaseWarning.Visible = !this.radioButtonConnectToMySql.Checked;

            this.HandleHeightLoginFormBasedOnuseDatabaseSetting();
        }

        private void HandleHeightLoginFormBasedOnuseDatabaseSetting()
        {
            if (this.formState != FormState.FormStateMain)
            {
                if (this.radioButtonConnectToMySql.Checked)
                {
                    this.MaximumSize = new Size((int)FormSizes.LoginFormWidth, (int)FormSizes.LoginFormHeight);
                    this.Height = (int)FormSizes.LoginFormHeight;
                }
                else
                {
                    this.MaximumSize = new Size((int)FormSizes.LoginFormWidth, (int)FormSizes.LoginFormHeightShowWarning);
                    this.Height = (int)FormSizes.LoginFormHeightShowWarning;
                }
            }
        }

        public void HandleUseWorldDatabaseSettingChanged()
        {
            this.radioButtonConnectToMySql.Checked = Settings.Default.UseWorldDatabase;
            this.radioButtonDontUseDatabase.Checked = !Settings.Default.UseWorldDatabase;
            this.buttonSearchForEntryOrGuid.Enabled = Settings.Default.UseWorldDatabase || this.comboBoxSourceType.SelectedIndex == 2;
            this.pictureBoxLoadScript.Enabled = this.textBoxEntryOrGuid.Text.Length > 0 && Settings.Default.UseWorldDatabase;
            this.checkBoxListActionlistsOrEntries.Enabled = Settings.Default.UseWorldDatabase;
            this.menuItemRevertQuery.Enabled = Settings.Default.UseWorldDatabase;
            this.SetGenerateCommentsEnabled(Settings.Default.UseWorldDatabase);

            if (Settings.Default.UseWorldDatabase)
                this.Text = "SAI-Editor - Connection: " + Settings.Default.User + ", " + Settings.Default.Host + ", " + Settings.Default.Port.ToString();
            else
                this.Text = "SAI-Editor - Creator-only mode, no database connection";
        }

        private void menuItemRetrieveLastDeletedRow_Click(object sender, EventArgs e)
        {
            if (this.lastDeletedSmartScripts.Count == 0)
            {
                MessageBox.Show("There are no items deleted in this session ready to be restored.", "Nothing to retrieve!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            this.listViewSmartScripts.AddSmartScript(this.lastDeletedSmartScripts.Last());
            this.lastDeletedSmartScripts.Remove(this.lastDeletedSmartScripts.Last());
        }

        private void checkBoxUsePhaseColors_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.PhaseHighlighting = this.checkBoxUsePhaseColors.Checked;
            Settings.Default.Save();

            this.listViewSmartScripts.Init(true);
        }

        private void checkBoxUsePermanentTooltips_CheckedChanged(object sender, EventArgs e)
        {
            this.checkBoxUsePermanentTooltips.Enabled = false;
            Settings.Default.ShowTooltipsPermanently = this.checkBoxUsePermanentTooltips.Checked;
            Settings.Default.Save();

            this.ExpandToShowPermanentTooltips(!this.checkBoxUsePermanentTooltips.Checked);
        }
    }
}
