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
using System.IO;
using System.Reflection;
using System.Net;
using System.Threading;
using System.Runtime.InteropServices;
using SAI_Editor.Classes.CustomControls;

namespace SAI_Editor.Forms
{
    public partial class MainForm : Form
    {
        public int expandAndContractSpeed = 5, lastSmartScriptIdOfScript = 0, previousLinkFrom = -1;
        public const int expandAndContractSpeedListView = 2;
        public EntryOrGuidAndSourceType originalEntryOrGuidAndSourceType = new EntryOrGuidAndSourceType();
        private readonly ListViewColumnSorter lvwColumnSorter = new ListViewColumnSorter();
        private bool contractingToLoginForm, expandingToMainForm, expandingListView, contractingListView;
        private bool runningConstructor = false, updatingFieldsBasedOnSelectedScript = false, adjustedLoginSettings = false;
        private int originalHeight = 0, originalWidth = 0;
        private int MainFormWidth = (int)FormSizes.MainFormWidth, MainFormHeight = (int)FormSizes.MainFormHeight;
        private int listViewSmartScriptsHeightToChangeTo;
        private List<SmartScript> lastDeletedSmartScripts = new List<SmartScript>(), smartScriptsOnClipBoard = new List<SmartScript>();
        private Thread updateSurveyThread = null, checkIfUpdatesAvailableThread = null;
        private FormState formState = FormState.FormStateLogin;
        private string applicationVersion = String.Empty;
        private System.Windows.Forms.Timer timerCheckForInternetConnection = new System.Windows.Forms.Timer();

        public MainForm()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            ResizeRedraw = true;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000; // WS_EX_COMPOSITED       
                return handleParam;
            }
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            runningConstructor = true;

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            applicationVersion = "v" + version.Major + "." + version.Minor + "." + version.Build;
            Text = "SAI-Editor " + applicationVersion + ": Login";

            menuStrip.Visible = false; //! Doing this in main code so we can actually see the menustrip in designform

            Width = (int)FormSizes.LoginFormWidth;
            Height = (int)FormSizes.LoginFormHeight;

            originalHeight = Height;
            originalWidth = Width;

            if (MainFormWidth > SystemInformation.VirtualScreen.Width)
                MainFormWidth = SystemInformation.VirtualScreen.Width;

            if (MainFormHeight > SystemInformation.VirtualScreen.Height)
                MainFormHeight = SystemInformation.VirtualScreen.Height;

            try
            {
                textBoxHost.Text = Settings.Default.Host;
                textBoxUsername.Text = Settings.Default.User;
                textBoxPassword.Text = SAI_Editor_Manager.Instance.GetPasswordSetting();
                textBoxWorldDatabase.Text = Settings.Default.Database;
                textBoxPort.Text = Settings.Default.Port > 0 ? Settings.Default.Port.ToString() : String.Empty;
                expandAndContractSpeed = Settings.Default.AnimationSpeed;
                radioButtonConnectToMySql.Checked = Settings.Default.UseWorldDatabase;
                radioButtonDontUseDatabase.Checked = !Settings.Default.UseWorldDatabase;
                checkBoxListActionlistsOrEntries.Enabled = Settings.Default.UseWorldDatabase;
                menuItemRevertQuery.Enabled = Settings.Default.UseWorldDatabase;
                SetGenerateCommentsEnabled(listViewSmartScripts.Items.Count > 0 && Settings.Default.UseWorldDatabase);
                buttonSearchForEntryOrGuid.Enabled = Settings.Default.UseWorldDatabase || (SourceTypes)Settings.Default.LastSourceType == SourceTypes.SourceTypeAreaTrigger;
                searchForAQuestToolStripMenuItem1.Enabled = Settings.Default.UseWorldDatabase;
                searchForACreatureEntryToolStripMenuItem.Enabled = Settings.Default.UseWorldDatabase;
                searchForACreatureGuidToolStripMenuItem.Enabled = Settings.Default.UseWorldDatabase;
                searchForAGameobjectEntryToolStripMenuItem.Enabled = Settings.Default.UseWorldDatabase;
                searchForAGameobjectGuidToolStripMenuItem.Enabled = Settings.Default.UseWorldDatabase;
                searchForAGameEventToolStripMenuItem.Enabled = Settings.Default.UseWorldDatabase;
                searchForAnItemEntryToolStripMenuItem.Enabled = Settings.Default.UseWorldDatabase;
                searchForACreatureSummonsIdToolStripMenuItem.Enabled = Settings.Default.UseWorldDatabase;
                searchForAnEquipmentTemplateToolStripMenuItem.Enabled = Settings.Default.UseWorldDatabase;
                searchForAWaypointToolStripMenuItem.Enabled = Settings.Default.UseWorldDatabase;
                searchForANpcTextToolStripMenuItem.Enabled = Settings.Default.UseWorldDatabase;
                searchForAGossipMenuOptionToolStripMenuItem.Enabled = Settings.Default.UseWorldDatabase;
                searchForAGossipOptionIdToolStripMenuItem.Enabled = Settings.Default.UseWorldDatabase;
                adjustedLoginSettings = true;
            }
            catch
            {
                MessageBox.Show("Something went wrong when loading the settings.", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            customPanelMain.Visible = false;

            comboBoxSourceType.SelectedIndex = 0;
            comboBoxEventType.SelectedIndex = 0;
            comboBoxActionType.SelectedIndex = 0;
            comboBoxTargetType.SelectedIndex = 0;

            //! We first load the information and then change the parameter fields
            await SAI_Editor_Manager.Instance.LoadSQLiteDatabaseInfo();
            ChangeParameterFieldsBasedOnType();

            if (Settings.Default.AutoConnect)
            {
                checkBoxAutoConnect.Checked = true;

                if (Settings.Default.UseWorldDatabase)
                {
                    SAI_Editor_Manager.Instance.connString = new MySqlConnectionStringBuilder();
                    SAI_Editor_Manager.Instance.connString.Server = textBoxHost.Text;
                    SAI_Editor_Manager.Instance.connString.UserID = textBoxUsername.Text;
                    SAI_Editor_Manager.Instance.connString.Port = XConverter.ToUInt32(textBoxPort.Text);
                    SAI_Editor_Manager.Instance.connString.Database = textBoxWorldDatabase.Text;

                    if (textBoxPassword.Text.Length > 0)
                        SAI_Editor_Manager.Instance.connString.Password = textBoxPassword.Text;

                    SAI_Editor_Manager.Instance.ResetWorldDatabase(true);
                }

                if (!Settings.Default.UseWorldDatabase || SAI_Editor_Manager.Instance.worldDatabase.CanConnectToDatabase(SAI_Editor_Manager.Instance.connString, false))
                {
                    SAI_Editor_Manager.Instance.ResetWorldDatabase(Settings.Default.UseWorldDatabase);
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
            }

            customPanelLogin.Location = new Point(9, 8);

            if (Settings.Default.HidePass)
                textBoxPassword.PasswordChar = '●';

            textBoxComments.GotFocus += textBoxComments_GotFocus;
            textBoxComments.LostFocus += textBoxComments_LostFocus;

            panelPermanentTooltipTypes.BackColor = Color.FromArgb(255, 255, 225);
            panelPermanentTooltipParameters.BackColor = Color.FromArgb(255, 255, 225);
            labelPermanentTooltipTextTypes.BackColor = Color.FromArgb(255, 255, 225);

            pictureBoxLoadScript.Enabled = textBoxEntryOrGuid.Text.Length > 0 && Settings.Default.UseWorldDatabase;
            pictureBoxCreateScript.Enabled = textBoxEntryOrGuid.Text.Length > 0;

            textBoxEventType.MouseWheel += textBoxEventType_MouseWheel;
            textBoxActionType.MouseWheel += textBoxActionType_MouseWheel;
            textBoxTargetType.MouseWheel += textBoxTargetType_MouseWheel;

            buttonNewLine.Enabled = textBoxEntryOrGuid.Text.Length > 0;

            timerCheckForInternetConnection.Interval = 600000; //! 10 minutes
            timerCheckForInternetConnection.Tick += timerCheckForInternetConnection_Tick;
            timerCheckForInternetConnection.Enabled = false;

            if (!Settings.Default.InformedAboutSurvey)
            {
                string termsArgeementString = "By clicking 'Yes' you agree to the application keeping a record of its usage in a remote database. Keep " +
                                              "in mind that this data will not be disclosed to a third party. It is for internal use and bookkeeping only.";

                DialogResult result = MessageBox.Show(termsArgeementString, "Agree to the terms", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result != DialogResult.Yes)
                {
                    //! Not running this in a diff thread because we want this to complete before exiting.
                    using (WebClient client = new WebClient())
                    {
                        try
                        {
                            client.DownloadData("http://www.jasper-rietrae.com/SAI-Editor/survey.php?agreed=false");
                        }
                        catch
                        {

                        }
                    }
                }

                Settings.Default.InformedAboutSurvey = true;
                Settings.Default.AgreedToSurvey = result == DialogResult.Yes;
            }

            updateSurveyThread = new Thread(UpdateSurvey);
            updateSurveyThread.Start();

            checkIfUpdatesAvailableThread = new Thread(CheckIfUpdatesAvailable);
            checkIfUpdatesAvailableThread.Start();

            UpdatePermanentTooltipOfTypes(comboBoxEventType, ScriptTypeId.ScriptTypeEvent);
            UpdatePermanentTooltipOfParameter(labelEventParam1, 1, comboBoxEventType, ScriptTypeId.ScriptTypeEvent);

            runningConstructor = false;
        }

        private void UpdateSurvey()
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    string url = "http://www.jasper-rietrae.com/SAI-Editor/survey.php?";

                    if (!Settings.Default.AgreedToSurvey)
                        url += "agreed=false";
                    else
                        url += "version=" + applicationVersion.Replace('.', '-');

                    client.DownloadData(url);
                }
                catch (ThreadAbortException)
                {

                }
                catch (WebException)
                {
                    //! Try to connect to google.com. If it can't connect, it means no internet connection
                    //! is available. We then start a timer which checks for an internet connection every
                    //! 10 minutes.
                    if (!SAI_Editor_Manager.Instance.HasInternetConnection())
                        timerCheckForInternetConnection.Enabled = true;
                }
                catch (Exception ex)
                {
                    //! Run the messagebox on the mainthread
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show("Something went wrong while attempting to keep track of the use count. Please report the following message to developers:\n\n" + ex.ToString(), "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
            }
        }

        private void CheckIfUpdatesAvailable()
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    using (Stream streamVersion = client.OpenRead("http://dl.dropbox.com/u/84527004/SAI-Editor/version.txt"))
                    {
                        if (streamVersion != null)
                        {
                            using (StreamReader streamReaderVersion = new StreamReader(streamVersion))
                            {
                                string newAppVersionStr = streamReaderVersion.ReadToEnd();
                                int newAppVersion = XConverter.ToInt32(newAppVersionStr.Replace("v", String.Empty).Replace(".", String.Empty));
                                int currAppVersion = XConverter.ToInt32(applicationVersion.Replace("v", String.Empty).Replace(".", String.Empty));

                                if (newAppVersion > 0 && currAppVersion > 0 && newAppVersion > currAppVersion)
                                {
                                    //! Run the messagebox in the mainthread
                                    Invoke(new Action(() =>
                                    {
                                        DialogResult result = MessageBox.Show(this, "A new version of the application is available (" + newAppVersionStr + "). Do you wish to go to the download page?", "New version available!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);

                                        if (result == DialogResult.Yes)
                                            SAI_Editor_Manager.Instance.StartProcess("http://www.trinitycore.org/f/files/file/17-sai-editor/");
                                    }));
                                }
                            }
                        }
                    }
                }
                catch (ThreadAbortException)
                {

                }
                catch (WebException)
                {
                    //! Try to connect to google.com. If it can't connect, it means no internet connection
                    //! is available. We then start a timer which checks for an internet connection every
                    //! 10 minutes.
                    if (!SAI_Editor_Manager.Instance.HasInternetConnection())
                        timerCheckForInternetConnection.Enabled = true;
                }
                catch (Exception ex)
                {
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show("Something went wrong while checking for updates. Please report the following message to developers:\n\n" + ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
            }
        }

        [DllImportAttribute("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImportAttribute("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImportAttribute("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        private void timerCheckForInternetConnection_Tick(object sender, EventArgs e)
        {
            //! Try to connect to google.com. If it can't connect, it means no internet connection
            //! is available.
            if (SAI_Editor_Manager.Instance.HasInternetConnection())
            {
                timerCheckForInternetConnection.Enabled = false;
                checkIfUpdatesAvailableThread.Start();
                updateSurveyThread.Start();
            }
        }

        private void timerExpandOrContract_Tick(object sender, EventArgs e)
        {
            if (expandingToMainForm)
            {
                if (Height < MainFormHeight)
                {
                    Height += expandAndContractSpeed;

                    if (Height >= MainFormHeight)
                        timerExpandOrContract_Tick(sender, e);
                }
                else
                {
                    Height = MainFormHeight;

                    if (Width >= MainFormWidth && timerExpandOrContract.Enabled) //! If both finished
                    {
                        Width = MainFormWidth;
                        timerExpandOrContract.Enabled = false;
                        expandingToMainForm = false;
                        formState = FormState.FormStateMain;
                        FinishedExpandingOrContracting(true);
                    }
                }

                if (Width < MainFormWidth)
                {
                    Width += expandAndContractSpeed;

                    if (Width >= MainFormWidth)
                        timerExpandOrContract_Tick(sender, e);
                }
                else
                {
                    Width = MainFormWidth;

                    if (Height >= MainFormHeight && timerExpandOrContract.Enabled) //! If both finished
                    {
                        Height = MainFormHeight;
                        timerExpandOrContract.Enabled = false;
                        expandingToMainForm = false;
                        formState = FormState.FormStateMain;
                        FinishedExpandingOrContracting(true);
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

                    if (Width <= originalWidth && timerExpandOrContract.Enabled) //! If both finished
                    {
                        Width = originalWidth;
                        timerExpandOrContract.Enabled = false;
                        contractingToLoginForm = false;
                        formState = FormState.FormStateLogin;
                        FinishedExpandingOrContracting(false);
                    }
                }

                if (Width > originalWidth)
                    Width -= expandAndContractSpeed;
                else
                {
                    Width = originalWidth;

                    if (Height <= originalHeight && timerExpandOrContract.Enabled) //! If both finished
                    {
                        Height = originalHeight;
                        timerExpandOrContract.Enabled = false;
                        contractingToLoginForm = false;
                        formState = FormState.FormStateLogin;
                        FinishedExpandingOrContracting(false);
                    }
                }
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (radioButtonConnectToMySql.Checked)
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

                SAI_Editor_Manager.Instance.connString = new MySqlConnectionStringBuilder();
                SAI_Editor_Manager.Instance.connString.Server = textBoxHost.Text;
                SAI_Editor_Manager.Instance.connString.UserID = textBoxUsername.Text;
                SAI_Editor_Manager.Instance.connString.Port = XConverter.ToUInt32(textBoxPort.Text);
                SAI_Editor_Manager.Instance.connString.Database = textBoxWorldDatabase.Text;

                if (textBoxPassword.Text.Length > 0)
                    SAI_Editor_Manager.Instance.connString.Password = textBoxPassword.Text;

                SAI_Editor_Manager.Instance.ResetWorldDatabase(true);
            }

            buttonConnect.Enabled = false;

            Settings.Default.UseWorldDatabase = radioButtonConnectToMySql.Checked;
            Settings.Default.Save();

            if (!radioButtonConnectToMySql.Checked || SAI_Editor_Manager.Instance.worldDatabase.CanConnectToDatabase(SAI_Editor_Manager.Instance.connString))
            {
                StartExpandingToMainForm(Settings.Default.InstantExpand);
                HandleUseWorldDatabaseSettingChanged();
            }

            buttonConnect.Enabled = true;
        }

        private void StartExpandingToMainForm(bool instant = false)
        {
            if (radioButtonConnectToMySql.Checked)
            {
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] buffer = new byte[1024];
                rng.GetBytes(buffer);
                string salt = BitConverter.ToString(buffer);
                rng.Dispose();

                Settings.Default.Entropy = salt;
                Settings.Default.Host = textBoxHost.Text;
                Settings.Default.User = textBoxUsername.Text;
                Settings.Default.Password = textBoxPassword.Text.Length == 0 ? String.Empty : textBoxPassword.Text.ToSecureString().EncryptString(Encoding.Unicode.GetBytes(salt));
                Settings.Default.Database = textBoxWorldDatabase.Text;
                Settings.Default.AutoConnect = checkBoxAutoConnect.Checked;
                Settings.Default.Port = XConverter.ToUInt32(textBoxPort.Text);
                Settings.Default.UseWorldDatabase = true;
                Settings.Default.Save();
            }

            ResetFieldsToDefault();

            if (radioButtonConnectToMySql.Checked)
                Text = "SAI-Editor " + applicationVersion + " - Connection: " + textBoxUsername.Text + ", " + textBoxHost.Text + ", " + textBoxPort.Text;
            else
                Text = "SAI-Editor " + applicationVersion + " - Creator-only mode, no database connection";

            if (instant)
            {
                Width = MainFormWidth;
                Height = MainFormHeight;
                formState = FormState.FormStateMain;
                FinishedExpandingOrContracting(true);
            }
            else
            {
                formState = FormState.FormStateExpandingOrContracting;
                timerExpandOrContract.Enabled = true;
                expandingToMainForm = true;
            }

            customPanelLogin.Visible = false;

            panelPermanentTooltipTypes.Visible = false;
            panelPermanentTooltipParameters.Visible = false;
        }

        private void StartContractingToLoginForm(bool instant = false)
        {
            Text = "SAI-Editor " + applicationVersion + ": Login";

            if (Settings.Default.ShowTooltipsPermanently)
                listViewSmartScripts.Height += (int)FormSizes.ListViewHeightContract;

            if (instant)
            {
                Width = originalWidth;
                Height = originalHeight;
                formState = FormState.FormStateLogin;
                FinishedExpandingOrContracting(false);
            }
            else
            {
                formState = FormState.FormStateExpandingOrContracting;
                timerExpandOrContract.Enabled = true;
                contractingToLoginForm = true;
            }

            customPanelMain.Visible = false;
            menuStrip.Visible = false;
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBoxHost.Text = String.Empty;
            textBoxUsername.Text = String.Empty;
            textBoxPassword.Text = String.Empty;
            textBoxWorldDatabase.Text = String.Empty;
            textBoxPort.Text = String.Empty;
            checkBoxAutoConnect.Checked = false;
            radioButtonConnectToMySql.Checked = true;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    switch (formState)
                    {
                        case FormState.FormStateLogin:
                            buttonConnect.PerformClick();
                            break;
                        case FormState.FormStateMain:
                            if (!textBoxEntryOrGuid.Focused)
                                break;

                            //! Load the script of the entryorguid when no world database can be used. Otherwise create a new one.
                            if (Settings.Default.UseWorldDatabase)
                                TryToLoadScript();
                            else
                                TryToCreateScript();

                            break;
                    }
                    break;
            }
        }

        private void buttonSearchForEntry_Click(object sender, EventArgs e)
        {
            //! Just keep it in main thread; no purpose starting a new thread for this (unless workspaces get implemented, maybe)
            using (SearchForEntryForm entryForm = new SearchForEntryForm(textBoxEntryOrGuid.Text, GetSourceTypeByIndex()))
                entryForm.ShowDialog(this);
        }

        private void menuItemReconnect_Click(object sender, EventArgs e)
        {
            if (formState != FormState.FormStateMain || contractingListView || expandingListView)
                return;

            for (int i = 0; i < Application.OpenForms.Count; ++i)
                if (Application.OpenForms[i] != this)
                    Application.OpenForms[i].Close();

            panelPermanentTooltipTypes.Visible = false;
            panelPermanentTooltipParameters.Visible = false;
            SaveLastUsedFields();
            ResetFieldsToDefault();
            listViewSmartScripts.ClearScripts();
            StartContractingToLoginForm(Settings.Default.InstantExpand);
        }

        private async void comboBoxEventType_SelectedIndexChanged(object sender, EventArgs e)
        {
            await HandleComboBoxTypeIdSelectedIndexChanged(comboBoxEventType, textBoxEventType, ScriptTypeId.ScriptTypeEvent);
        }

        private async void comboBoxActionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            await HandleComboBoxTypeIdSelectedIndexChanged(comboBoxActionType, textBoxActionType, ScriptTypeId.ScriptTypeAction);
        }

        private async void comboBoxTargetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            await HandleComboBoxTypeIdSelectedIndexChanged(comboBoxTargetType, textBoxTargetType, ScriptTypeId.ScriptTypeTarget);
        }

        private async Task HandleComboBoxTypeIdSelectedIndexChanged(ComboBox comboBox, TextBox textBox, ScriptTypeId scriptTypeId)
        {
            textBox.Text = comboBox.SelectedIndex.ToString();
            textBox.SelectionStart = 3; //! Set cursor to end of text

            if (!runningConstructor)
            {
                ChangeParameterFieldsBasedOnType();
                UpdatePermanentTooltipOfTypes(comboBox, scriptTypeId);
            }

            if (listViewSmartScripts.SelectedItems.Count > 0)
            {
                switch (scriptTypeId)
                {
                    case ScriptTypeId.ScriptTypeEvent:
                        listViewSmartScripts.SelectedScript.event_type = comboBox.SelectedIndex;
                        break;
                    case ScriptTypeId.ScriptTypeAction:
                        listViewSmartScripts.SelectedScript.action_type = comboBox.SelectedIndex;
                        break;
                    case ScriptTypeId.ScriptTypeTarget:
                        listViewSmartScripts.SelectedScript.target_type = comboBox.SelectedIndex;
                        break;
                }

                listViewSmartScripts.ReplaceScript(listViewSmartScripts.SelectedScript);
                await GenerateCommentForSmartScript(listViewSmartScripts.SelectedScript);
            }
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

        private void checkBoxLockEventId_CheckedChanged(object sender, EventArgs e)
        {
            textBoxId.Enabled = !checkBoxLockEventId.Checked;
        }

        private void FinishedExpandingOrContracting(bool expanding)
        {
            customPanelLogin.Visible = !expanding;
            customPanelMain.Visible = expanding;
            menuStrip.Visible = expanding;
            Invalidate();

            if (!expanding)
                HandleHeightLoginFormBasedOnuseDatabaseSetting();

            panelPermanentTooltipTypes.Visible = false;
            panelPermanentTooltipParameters.Visible = false;

            textBoxEntryOrGuid.Text = Settings.Default.LastEntryOrGuid;
            comboBoxSourceType.SelectedIndex = Settings.Default.LastSourceType;
            checkBoxShowBasicInfo.Checked = Settings.Default.ShowBasicInfo;
            checkBoxLockEventId.Checked = Settings.Default.LockSmartScriptId;
            checkBoxListActionlistsOrEntries.Checked = Settings.Default.ListActionLists;
            checkBoxAllowChangingEntryAndSourceType.Checked = Settings.Default.AllowChangingEntryAndSourceType;
            checkBoxUsePhaseColors.Checked = Settings.Default.PhaseHighlighting;
            checkBoxUsePermanentTooltips.Checked = Settings.Default.ShowTooltipsPermanently;

            if (expanding)
            {
                if (checkBoxUsePermanentTooltips.Checked)
                    ExpandToShowPermanentTooltips(false);

                if (radioButtonConnectToMySql.Checked)
                    TryToLoadScript(showErrorIfNoneFound: false);
            }
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
                            message += "\n\nA script was found with this entry using sourcetype " + smartScripts[0].source_type + " (" + SAI_Editor_Manager.Instance.GetSourceTypeString((SourceTypes)smartScripts[0].source_type).ToLower() + "). Do you wish to load this instead?";
                            DialogResult dialogResult = MessageBox.Show(message, "No scripts found!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                            if (dialogResult == DialogResult.Yes)
                            {
                                textBoxEntryOrGuid.Text = smartScripts[0].entryorguid.ToString();
                                comboBoxSourceType.SelectedIndex = GetIndexBySourceType((SourceTypes)smartScripts[0].source_type);
                                TryToLoadScript(forced: true);
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
                                                textBoxEntryOrGuid.Text = smartScripts[0].entryorguid.ToString();
                                                comboBoxSourceType.SelectedIndex = GetIndexBySourceType(SourceTypes.SourceTypeCreature);
                                                TryToLoadScript();
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
                                                textBoxEntryOrGuid.Text = smartScripts[0].entryorguid.ToString();
                                                comboBoxSourceType.SelectedIndex = GetIndexBySourceType(SourceTypes.SourceTypeGameobject);
                                                TryToLoadScript();
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
                                    TryToCreateScript();
                            }
                            else
                                MessageBox.Show(message, "No scripts found!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    pictureBoxLoadScript.Enabled = textBoxEntryOrGuid.Text.Length > 0 && Settings.Default.UseWorldDatabase;
                    pictureBoxCreateScript.Enabled = textBoxEntryOrGuid.Text.Length > 0;
                    return new List<SmartScript>();
                }

                for (int i = 0; i < smartScripts.Count; ++i)
                {
                    smartScriptsToReturn.Add(smartScripts[i]);

                    if (!checkBoxListActionlistsOrEntries.Checked || !checkBoxListActionlistsOrEntries.Enabled)
                        continue;

                    if (i == smartScripts.Count - 1 && originalEntryOrGuidAndSourceType.sourceType == SourceTypes.SourceTypeScriptedActionlist)
                    {
                        List<EntryOrGuidAndSourceType> timedActionListOrEntries = await SAI_Editor_Manager.Instance.GetTimedActionlistsOrEntries(smartScripts[i], sourceType);

                        //if (timedActionListOrEntries.sourceTypeOfEntry != SourceTypes.SourceTypeScriptedActionlist)
                        {
                            foreach (EntryOrGuidAndSourceType entryOrGuidAndSourceType in timedActionListOrEntries)
                            {
                                if (entryOrGuidAndSourceType.sourceType == SourceTypes.SourceTypeScriptedActionlist)
                                    continue;

                                List<SmartScript> newSmartScripts = await GetSmartScriptsForEntryAndSourceType(entryOrGuidAndSourceType.entryOrGuid.ToString(), entryOrGuidAndSourceType.sourceType);

                                if (newSmartScripts != null)
                                    foreach (SmartScript item in newSmartScripts.Where(item => !ListContainsSmartScript(smartScriptsToReturn, item)))
                                        smartScriptsToReturn.Add(item);

                                pictureBoxCreateScript.Enabled = textBoxEntryOrGuid.Text.Length > 0;
                            }
                        }
                    }

                    if (sourceType == originalEntryOrGuidAndSourceType.sourceType && originalEntryOrGuidAndSourceType.sourceType != SourceTypes.SourceTypeScriptedActionlist)
                    {
                        List<EntryOrGuidAndSourceType> timedActionListOrEntries = await SAI_Editor_Manager.Instance.GetTimedActionlistsOrEntries(smartScripts[i], sourceType);

                        foreach (EntryOrGuidAndSourceType entryOrGuidAndSourceType in timedActionListOrEntries)
                        {
                            List<SmartScript> newSmartScripts = await GetSmartScriptsForEntryAndSourceType(entryOrGuidAndSourceType.entryOrGuid.ToString(), entryOrGuidAndSourceType.sourceType);

                            foreach (SmartScript item in newSmartScripts.Where(item => !ListContainsSmartScript(smartScriptsToReturn, item))) smartScriptsToReturn.Add(item);

                            pictureBoxCreateScript.Enabled = textBoxEntryOrGuid.Text.Length > 0;
                        }
                    }
                }

                foreach (ColumnHeader header in listViewSmartScripts.Columns)
                    header.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            catch
            {
                if (showError)
                    MessageBox.Show("An exception was thrown while trying to load the smart_scripts for entryorguid " + entryOrGuid.ToString() + ".", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            pictureBoxLoadScript.Enabled = textBoxEntryOrGuid.Text.Length > 0 && Settings.Default.UseWorldDatabase;
            pictureBoxCreateScript.Enabled = textBoxEntryOrGuid.Text.Length > 0;
            return smartScriptsToReturn;
        }

        bool ListContainsSmartScript(List<SmartScript> smartScriptsToReturn, SmartScript item)
        {
            return smartScriptsToReturn.Any(itemToReturn => itemToReturn.entryorguid == item.entryorguid && itemToReturn.id == item.id);
        }

        private void menuItemExit_Click(object sender, System.EventArgs e)
        {
            if (formState == FormState.FormStateMain)
                TryCloseApplication();
        }

        private void TryCloseApplication()
        {
            if (!Settings.Default.PromptToQuit || DialogResult.Yes == MessageBox.Show("Are you sure you want to quit?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                Close();
        }

        private void menuItemSettings_Click(object sender, EventArgs e)
        {
            if (formState != FormState.FormStateMain)
                return;

            using (SettingsForm settingsForm = new SettingsForm())
                settingsForm.ShowDialog(this);
        }

        private void menuItemAbout_Click(object sender, EventArgs e)
        {
            if (formState != FormState.FormStateMain)
                return;

            using (AboutForm aboutForm = new AboutForm())
                aboutForm.ShowDialog(this);
        }

        private void listViewSmartScripts_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            menuItemDeleteSelectedRow.Enabled = listViewSmartScripts.SelectedItems.Count > 0;
            menuItemGenerateSql.Enabled = listViewSmartScripts.SelectedItems.Count > 0;
            buttonGenerateSql.Enabled = listViewSmartScripts.SelectedItems.Count > 0;
            menuitemLoadSelectedEntry.Enabled = listViewSmartScripts.SelectedItems.Count > 0;
            menuItemDuplicateRow.Enabled = listViewSmartScripts.SelectedItems.Count > 0;
            menuItemGenerateComment.Enabled = listViewSmartScripts.SelectedItems.Count > 0;
            menuItemCopySelectedRow.Enabled = listViewSmartScripts.SelectedItems.Count > 0;

            if (!e.IsSelected)
                return;

            FillFieldsBasedOnSelectedScript();

            if (Settings.Default.ChangeStaticInfo)
                checkBoxListActionlistsOrEntries.Text = listViewSmartScripts.SelectedItems[0].SubItems[1].Text == "9" ? "List entries too" : "List actionlists too";
        }

        private void FillFieldsBasedOnSelectedScript()
        {
            try
            {
                updatingFieldsBasedOnSelectedScript = true;
                SmartScript selectedScript = listViewSmartScripts.SelectedScript;

                if (Settings.Default.ChangeStaticInfo)
                {
                    textBoxEntryOrGuid.Text = selectedScript.entryorguid.ToString();
                    comboBoxSourceType.SelectedIndex = GetIndexBySourceType((SourceTypes)selectedScript.source_type);
                }

                textBoxId.Text = selectedScript.id.ToString();
                textBoxLinkTo.Text = selectedScript.link.ToString();
                textBoxLinkFrom.Text = GetLinkFromForSelection();

                int event_type = selectedScript.event_type;
                comboBoxEventType.SelectedIndex = event_type;
                textBoxEventPhasemask.Text = selectedScript.event_phase_mask.ToString();
                textBoxEventChance.Text = selectedScript.event_chance.ToString();
                textBoxEventFlags.Text = selectedScript.event_flags.ToString();

                //! Event parameters
                textBoxEventParam1.Text = selectedScript.event_param1.ToString();
                textBoxEventParam2.Text = selectedScript.event_param2.ToString();
                textBoxEventParam3.Text = selectedScript.event_param3.ToString();
                textBoxEventParam4.Text = selectedScript.event_param4.ToString();
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
                int action_type = selectedScript.action_type;
                comboBoxActionType.SelectedIndex = action_type;
                textBoxActionParam1.Text = selectedScript.action_param1.ToString();
                textBoxActionParam2.Text = selectedScript.action_param2.ToString();
                textBoxActionParam3.Text = selectedScript.action_param3.ToString();
                textBoxActionParam4.Text = selectedScript.action_param4.ToString();
                textBoxActionParam5.Text = selectedScript.action_param5.ToString();
                textBoxActionParam6.Text = selectedScript.action_param6.ToString();
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
                int target_type = selectedScript.target_type;
                comboBoxTargetType.SelectedIndex = target_type;
                textBoxTargetParam1.Text = selectedScript.target_param1.ToString();
                textBoxTargetParam2.Text = selectedScript.target_param2.ToString();
                textBoxTargetParam3.Text = selectedScript.target_param3.ToString();
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

                textBoxTargetX.Text = selectedScript.target_x;
                textBoxTargetY.Text = selectedScript.target_y;
                textBoxTargetZ.Text = selectedScript.target_z;
                textBoxTargetO.Text = selectedScript.target_o;
                textBoxComments.Text = selectedScript.comment;

                AdjustAllParameterFields(event_type, action_type, target_type);
                updatingFieldsBasedOnSelectedScript = false;
            }
            catch
            {
                MessageBox.Show("Something went wrong while attempting to edit the fields based on the new selection.", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetLinkFromForSelection()
        {
            SmartScript selectedScript = listViewSmartScripts.SelectedScript;

            foreach (SmartScript smartScript in listViewSmartScripts.Scripts)
            {
                if (smartScript.entryorguid != selectedScript.entryorguid || smartScript.source_type != selectedScript.source_type)
                    continue;

                if (smartScript.link > 0 && smartScript.link == listViewSmartScripts.SelectedScript.id)
                    return smartScript.id.ToString();
            }

            return "None";
        }

        private void AdjustAllParameterFields(int event_type, int action_type, int target_type)
        {
            SetVisibilityOfAllParamButtons(false);

            switch ((SmartEvent)event_type)
            {
                case SmartEvent.SMART_EVENT_SPELLHIT: //! Spell entry & Spell school
                case SmartEvent.SMART_EVENT_SPELLHIT_TARGET: //! Spell entry & Spell school
                case SmartEvent.SMART_EVENT_GOSSIP_SELECT: //! Gossip menu id & gossip id
                case SmartEvent.SMART_EVENT_DISTANCE_CREATURE: //! Creature guid
                case SmartEvent.SMART_EVENT_DISTANCE_GAMEOBJECT: //! Gameobject guid & entry
                    buttonEventParamOneSearch.Visible = true;
                    buttonEventParamTwoSearch.Visible = true;
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
                case SmartEvent.SMART_EVENT_FRIENDLY_MISSING_BUFF: //! Spell id
                case SmartEvent.SMART_EVENT_HAS_AURA: //! Spell id
                case SmartEvent.SMART_EVENT_TARGET_BUFFED: //! Spell id
                case SmartEvent.SMART_EVENT_SUMMON_DESPAWNED: //! Creature entry
                case SmartEvent.SMART_EVENT_SUMMONED_UNIT: //! Creature entry
                case SmartEvent.SMART_EVENT_ACCEPTED_QUEST: //! Quest id
                case SmartEvent.SMART_EVENT_REWARD_QUEST: //! Quest id
                case SmartEvent.SMART_EVENT_RECEIVE_EMOTE: //! Emote id
                    buttonEventParamOneSearch.Visible = true;
                    break;
                case SmartEvent.SMART_EVENT_TEXT_OVER: //! Creature entry
                    buttonEventParamTwoSearch.Visible = true;
                    break;
                case SmartEvent.SMART_EVENT_VICTIM_CASTING: //! Spell id
                    buttonEventParamThreeSearch.Visible = true;
                    break;
                case SmartEvent.SMART_EVENT_KILL: //! Creature entry
                    buttonEventParamFourSearch.Visible = true;
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
                    buttonActionParamOneSearch.Visible = true;
                    buttonActionParamTwoSearch.Visible = true;
                    break;
                case SmartAction.SMART_ACTION_CROSS_CAST:
                    buttonActionParamOneSearch.Visible = true; //! Spell entry
                    buttonActionParamTwoSearch.Visible = true; //! Cast flags
                    buttonActionParamThreeSearch.Visible = true; //! Target type
                    break;
                case SmartAction.SMART_ACTION_WP_STOP: //! Quest entry
                case SmartAction.SMART_ACTION_INTERRUPT_SPELL: //! Spell entry
                case SmartAction.SMART_ACTION_SEND_GOSSIP_MENU: //! Gossip menu id & npc_text.id
                case SmartAction.SMART_ACTION_CALL_TIMED_ACTIONLIST: //! Timer type
                    buttonActionParamTwoSearch.Visible = true;
                    break;
                case SmartAction.SMART_ACTION_WP_START:
                    buttonActionParamTwoSearch.Visible = true; //! Waypoint entry
                    buttonActionParamFourSearch.Visible = true; //! Quest entry
                    buttonActionParamSixSearch.Visible = true; //! React state
                    break;
                case SmartAction.SMART_ACTION_FOLLOW:
                    buttonActionParamThreeSearch.Visible = true; //! Creature entry
                    buttonActionParamFourSearch.Visible = true; //! Creature entry
                    break;
                case SmartAction.SMART_ACTION_RANDOM_PHASE:  //! Event phase 1-6
                case SmartAction.SMART_ACTION_RANDOM_EMOTE: //! Emote entry 1-6
                    buttonActionParamOneSearch.Visible = true;
                    buttonActionParamTwoSearch.Visible = true;
                    buttonActionParamThreeSearch.Visible = true;
                    buttonActionParamFourSearch.Visible = true;
                    buttonActionParamFiveSearch.Visible = true;
                    buttonActionParamSixSearch.Visible = true;
                    break;
                case SmartAction.SMART_ACTION_EQUIP:
                    buttonActionParamOneSearch.Visible = true; //! Equipment entry
                    buttonActionParamThreeSearch.Visible = true; //! Item entry 1
                    buttonActionParamFourSearch.Visible = true; //! Item entry 2
                    buttonActionParamFiveSearch.Visible = true; //! Item entry 3
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

        private void textBoxEventTypeId_TextChanged(object sender, EventArgs e)
        {
            HandleTextBoxTypeIdTextChanged(textBoxEventType, comboBoxEventType, (int)SmartEvent.SMART_EVENT_MAX - 1);
        }

        private void textBoxActionTypeId_TextChanged(object sender, EventArgs e)
        {
            HandleTextBoxTypeIdTextChanged(textBoxActionType, comboBoxActionType, (int)SmartAction.SMART_ACTION_MAX - 1);
        }

        private void textBoxTargetTypeId_TextChanged(object sender, EventArgs e)
        {
            HandleTextBoxTypeIdTextChanged(textBoxTargetType, comboBoxTargetType, (int)SmartTarget.SMART_TARGET_MAX - 1);
        }

        private void HandleTextBoxTypeIdTextChanged(TextBox textBox, ComboBox comboBox, int max)
        {
            if (String.IsNullOrEmpty(textBox.Text))
            {
                comboBox.SelectedIndex = 0;
                textBox.Text = "0";
                textBox.SelectionStart = 3; //! Set cursor position to end of the line
            }
            else
            {
                int targetType;
                Int32.TryParse(textBox.Text, out targetType);

                if (targetType > max)
                {
                    comboBox.SelectedIndex = max;
                    textBox.Text = (max).ToString();
                    textBox.SelectionStart = 3; //! Set cursor position to end of the line
                }
                else
                    comboBox.SelectedIndex = targetType;
            }
        }

        private void menuOptionDeleteSelectedRow_Click(object sender, EventArgs e)
        {
            if (formState != FormState.FormStateMain || listViewSmartScripts.SelectedScript == null)
                return;

            if (listViewSmartScripts.SelectedItems.Count <= 0)
            {
                MessageBox.Show("No rows were selected to delete!", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DeleteSelectedRow();
        }

        private void menuItemCopySelectedRowListView_Click(object sender, EventArgs e)
        {
            if (formState != FormState.FormStateMain || listViewSmartScripts.SelectedScript == null)
                return;

            smartScriptsOnClipBoard.Add(listViewSmartScripts.SelectedScript.Clone());
        }

        private void menuItemPasteLastCopiedRow_Click(object sender, EventArgs e)
        {
            if (formState != FormState.FormStateMain || listViewSmartScripts.SelectedScript == null)
                return;

            if (smartScriptsOnClipBoard.Count <= 0)
            {
                MessageBox.Show("No smart scripts have been copied in this session!", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SmartScript newSmartScript = smartScriptsOnClipBoard.Last().Clone();
            listViewSmartScripts.AddScript(newSmartScript);
        }

        private void DeleteSelectedRow()
        {
            if (listViewSmartScripts.SelectedItems.Count == 0)
                return;

            int prevSelectedIndex = listViewSmartScripts.SelectedItems[0].Index;

            if (listViewSmartScripts.SelectedItems[0].SubItems[0].Text == originalEntryOrGuidAndSourceType.entryOrGuid.ToString())
                if (listViewSmartScripts.SelectedItems[0].SubItems[2].Text == lastSmartScriptIdOfScript.ToString())
                    lastSmartScriptIdOfScript--;

            lastDeletedSmartScripts.Add(listViewSmartScripts.SelectedScript.Clone());
            listViewSmartScripts.RemoveScript(listViewSmartScripts.SelectedScript);
            SetGenerateCommentsEnabled(listViewSmartScripts.Items.Count > 0 && Settings.Default.UseWorldDatabase);

            if (listViewSmartScripts.Items.Count <= 0)
                ResetFieldsToDefault(Settings.Default.ChangeStaticInfo);
            else
                ReSelectListViewItemWithPrevIndex(prevSelectedIndex);

            //! Need to do this if static info is changed
            pictureBoxCreateScript.Enabled = textBoxEntryOrGuid.Text.Length > 0;
        }

        private void SetGenerateCommentsEnabled(bool enabled)
        {
            buttonGenerateComments.Enabled = enabled;
            menuItemGenerateComment.Enabled = enabled;
        }

        private void ReSelectListViewItemWithPrevIndex(int prevIndex)
        {
            if (listViewSmartScripts.Items.Count > prevIndex)
                listViewSmartScripts.Items[prevIndex].Selected = true;
            else if (listViewSmartScripts.Items.Count > 0)
                listViewSmartScripts.Items[prevIndex - 1].Selected = true;
        }

        private async void checkBoxListActionlists_CheckedChanged(object sender, EventArgs e)
        {
            if (listViewSmartScripts.Items.Count == 0)
                return;

            buttonGenerateSql.Enabled = false;
            menuItemGenerateSql.Enabled = false;
            int prevSelectedIndex = listViewSmartScripts.SelectedItems.Count > 0 ? listViewSmartScripts.SelectedItems[0].Index : 0;

            if (checkBoxListActionlistsOrEntries.Checked)
            {
                List<SmartScript> smartScripts = await GetSmartScriptsForEntryAndSourceType(originalEntryOrGuidAndSourceType.entryOrGuid.ToString(), originalEntryOrGuidAndSourceType.sourceType);
                List<SmartScript> newSmartScripts = new List<SmartScript>();

                //! Only add the new smartscript if it doesn't yet exist
                foreach (SmartScript newSmartScript in smartScripts)
                    if (!listViewSmartScripts.Items.Cast<CustomListViewItem>().Any(p => (p.Script as SmartScript).entryorguid == newSmartScript.entryorguid && (p.Script as SmartScript).id == newSmartScript.id))
                        listViewSmartScripts.AddScript(newSmartScript);

                pictureBoxCreateScript.Enabled = textBoxEntryOrGuid.Text.Length > 0;
            }
            else
                RemoveNonOriginalScriptsFromView();

            HandleShowBasicInfo();

            if (listViewSmartScripts.Items.Count > prevSelectedIndex)
                listViewSmartScripts.Items[prevSelectedIndex].Selected = true;

            buttonGenerateSql.Enabled = listViewSmartScripts.Items.Count > 0;
            menuItemGenerateSql.Enabled = listViewSmartScripts.Items.Count > 0;
        }

        private void RemoveNonOriginalScriptsFromView()
        {
            List<DatabaseClass> smartScriptsToRemove = listViewSmartScripts.Scripts.Where(smartScript => smartScript.source_type != (int)originalEntryOrGuidAndSourceType.sourceType).Cast<DatabaseClass>().ToList();

            foreach (SmartScript smartScript in smartScriptsToRemove)
                listViewSmartScripts.Scripts.Remove(smartScript);
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
            TryToLoadScript();
        }

        private void pictureBoxCreateScript_Click(object sender, EventArgs e)
        {
            TryToCreateScript();
        }

        public async void TryToCreateScript(bool fromNewLine = false)
        {
            if (!pictureBoxCreateScript.Enabled || String.IsNullOrWhiteSpace(textBoxEntryOrGuid.Text) || comboBoxSourceType.SelectedIndex == -1)
                return;

            if (listViewSmartScripts.Items.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show("There is already a script loaded at this moment. Do you want to overwrite this?\n\nWarning: overwriting means local unsaved changes will also be discarded!", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult != DialogResult.Yes)
                    return;

                ResetFieldsToDefault();
            }

            int entryorguid = 0;

            try
            {
                entryorguid = Int32.Parse(textBoxEntryOrGuid.Text);
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

            lastSmartScriptIdOfScript = 0;
            int source_type = (int)GetSourceTypeByIndex();
            string sourceTypeString = SAI_Editor_Manager.Instance.GetSourceTypeString((SourceTypes)source_type).ToLower();

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
                        TryToLoadScript();

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
                            string sqlOutput = "UPDATE `" + GetTemplateTableBySourceType((SourceTypes)source_type) + "` SET `AIName`=" + '"' + '"' + " WHERE `entry`=" + entryorguid + ";\n";

                            using (SqlOutputForm sqlOutputForm = new SqlOutputForm(sqlOutput, false, saveToFile: false))
                                sqlOutputForm.ShowDialog(this);
                        }
                        else
                            TryToLoadScript();
                    }

                    return;
                }

                string scriptName = await SAI_Editor_Manager.Instance.worldDatabase.GetObjectScriptName(entryorguid, source_type);

                if (scriptName != String.Empty)
                {
                    MessageBox.Show("This " + sourceTypeString + " already has its ScriptName set (to '" + scriptName + "')!", "Something went wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    TryToLoadScript();

                return;
            }

        SkipWorldDatabaseChecks:
            buttonNewLine.Enabled = false;
            checkBoxListActionlistsOrEntries.Text = GetSourceTypeByIndex() == SourceTypes.SourceTypeScriptedActionlist ? "List entries too" : "List actionlists too";
            pictureBoxLoadScript.Enabled = false;
            pictureBoxCreateScript.Enabled = false;

            originalEntryOrGuidAndSourceType.entryOrGuid = entryorguid;
            originalEntryOrGuidAndSourceType.sourceType = (SourceTypes)source_type;

            listViewSmartScripts.ClearScripts();

            SmartScript newSmartScript = new SmartScript();
            newSmartScript.entryorguid = entryorguid;
            newSmartScript.source_type = source_type;

            if (checkBoxLockEventId.Checked)
                newSmartScript.id = 0;
            else
                newSmartScript.id = -1;

            newSmartScript.link = XConverter.ToInt32(textBoxLinkTo.Text);
            newSmartScript.event_type = XConverter.ToInt32(textBoxEventType.Text);
            newSmartScript.event_phase_mask = XConverter.ToInt32(textBoxEventPhasemask.Text);
            newSmartScript.event_chance = XConverter.ToInt32(textBoxEventChance.Value);
            newSmartScript.event_flags = XConverter.ToInt32(textBoxEventFlags.Text);
            newSmartScript.event_param1 = XConverter.ToInt32(textBoxEventParam1.Text);
            newSmartScript.event_param2 = XConverter.ToInt32(textBoxEventParam2.Text);
            newSmartScript.event_param3 = XConverter.ToInt32(textBoxEventParam3.Text);
            newSmartScript.event_param4 = XConverter.ToInt32(textBoxEventParam4.Text);
            newSmartScript.action_type = XConverter.ToInt32(textBoxActionType.Text);
            newSmartScript.action_param1 = XConverter.ToInt32(textBoxActionParam1.Text);
            newSmartScript.action_param2 = XConverter.ToInt32(textBoxActionParam2.Text);
            newSmartScript.action_param3 = XConverter.ToInt32(textBoxActionParam3.Text);
            newSmartScript.action_param4 = XConverter.ToInt32(textBoxActionParam4.Text);
            newSmartScript.action_param5 = XConverter.ToInt32(textBoxActionParam5.Text);
            newSmartScript.action_param6 = XConverter.ToInt32(textBoxActionParam6.Text);
            newSmartScript.target_type = XConverter.ToInt32(textBoxTargetType.Text);
            newSmartScript.target_param1 = XConverter.ToInt32(textBoxTargetParam1.Text);
            newSmartScript.target_param2 = XConverter.ToInt32(textBoxTargetParam2.Text);
            newSmartScript.target_param3 = XConverter.ToInt32(textBoxTargetParam3.Text);
            newSmartScript.target_x = textBoxTargetX.Text;
            newSmartScript.target_y = textBoxTargetY.Text;
            newSmartScript.target_z = textBoxTargetZ.Text;
            newSmartScript.target_o = textBoxTargetO.Text;

            if (Settings.Default.GenerateComments && Settings.Default.UseWorldDatabase)
                newSmartScript.comment = await CommentGenerator.Instance.GenerateCommentFor(newSmartScript, originalEntryOrGuidAndSourceType);
            else if (textBoxComments.Text.Contains(" - Event - Action (phase) (dungeon difficulty)"))
                newSmartScript.comment = SAI_Editor_Manager.Instance.GetDefaultCommentForSourceType((SourceTypes)newSmartScript.source_type);
            else
                newSmartScript.comment = textBoxComments.Text;

            listViewSmartScripts.AddScript(newSmartScript, selectNewItem: true);

            HandleShowBasicInfo();

            buttonNewLine.Enabled = textBoxEntryOrGuid.Text.Length > 0;
            SetGenerateCommentsEnabled(listViewSmartScripts.Items.Count > 0 && Settings.Default.UseWorldDatabase);
            pictureBoxLoadScript.Enabled = textBoxEntryOrGuid.Text.Length > 0 && Settings.Default.UseWorldDatabase;
            pictureBoxCreateScript.Enabled = textBoxEntryOrGuid.Text.Length > 0;
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

        public async void TryToLoadScript(int entryorguid = -1, SourceTypes sourceType = SourceTypes.SourceTypeNone, bool showErrorIfNoneFound = true, bool promptCreateIfNoneFound = false, bool forced = false)
        {
            if ((!forced && !pictureBoxLoadScript.Enabled) || !Settings.Default.UseWorldDatabase)
                return;

            listViewSmartScripts.ClearScripts();
            ResetFieldsToDefault();

            if (String.IsNullOrEmpty(textBoxEntryOrGuid.Text))
                return;

            buttonGenerateSql.Enabled = false;
            menuItemGenerateSql.Enabled = false;
            pictureBoxLoadScript.Enabled = false;
            pictureBoxCreateScript.Enabled = false;
            lastSmartScriptIdOfScript = 0;

            if (entryorguid != -1 && sourceType != SourceTypes.SourceTypeNone)
            {
                originalEntryOrGuidAndSourceType.entryOrGuid = entryorguid;
                originalEntryOrGuidAndSourceType.sourceType = sourceType;
                textBoxEntryOrGuid.Text = entryorguid.ToString();
                comboBoxSourceType.SelectedIndex = GetIndexBySourceType(sourceType);
            }
            else
            {
                try
                {
                    originalEntryOrGuidAndSourceType.entryOrGuid = Int32.Parse(textBoxEntryOrGuid.Text);
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

                originalEntryOrGuidAndSourceType.sourceType = GetSourceTypeByIndex();
            }

            List<SmartScript> smartScripts = await GetSmartScriptsForEntryAndSourceType(originalEntryOrGuidAndSourceType.entryOrGuid.ToString(), originalEntryOrGuidAndSourceType.sourceType, showErrorIfNoneFound, promptCreateIfNoneFound);
            listViewSmartScripts.ReplaceScripts(smartScripts.Cast<DatabaseClass>().ToList());
            checkBoxListActionlistsOrEntries.Text = originalEntryOrGuidAndSourceType.sourceType == SourceTypes.SourceTypeScriptedActionlist ? "List entries too" : "List actionlists too";

            buttonNewLine.Enabled = false;
            SetGenerateCommentsEnabled(listViewSmartScripts.Items.Count > 0 && Settings.Default.UseWorldDatabase);
            HandleShowBasicInfo();

            if (listViewSmartScripts.Items.Count > 0)
            {
                SortListView(SortOrder.Ascending, 1);
                listViewSmartScripts.Items[0].Selected = true;
                listViewSmartScripts.Select(); //! Sets the focus on the listview

                if (checkBoxListActionlistsOrEntries.Enabled && checkBoxListActionlistsOrEntries.Checked)
                {
                    foreach (ListViewItem item in listViewSmartScripts.Items)
                        if (item.Text == originalEntryOrGuidAndSourceType.entryOrGuid.ToString())
                            lastSmartScriptIdOfScript = XConverter.ToInt32(item.SubItems[2].Text);
                }
                else
                    lastSmartScriptIdOfScript = XConverter.ToInt32(listViewSmartScripts.Items[listViewSmartScripts.Items.Count - 1].SubItems[2].Text);
            }

            buttonNewLine.Enabled = textBoxEntryOrGuid.Text.Length > 0;
            buttonGenerateSql.Enabled = listViewSmartScripts.Items.Count > 0;
            menuItemGenerateSql.Enabled = listViewSmartScripts.Items.Count > 0;
            pictureBoxCreateScript.Enabled = textBoxEntryOrGuid.Text.Length > 0;
        }

        private void buttonSearchPhasemask_Click(object sender, EventArgs e)
        {
            ShowSelectForm("SmartPhaseMasks", textBoxEventPhasemask);
        }

        private void buttonSelectEventFlag_Click(object sender, EventArgs e)
        {
            ShowSelectForm("SmartEventFlags", textBoxEventFlags);
        }

        private async void buttonSearchWorldDb_Click(object sender, EventArgs e)
        {
            buttonSearchWorldDb.Enabled = false;

            SAI_Editor_Manager.Instance.ResetWorldDatabase(false);
            List<string> databaseNames = await SAI_Editor_Manager.Instance.GetDatabasesInConnection(textBoxHost.Text, textBoxUsername.Text, XConverter.ToUInt32(textBoxPort.Text), textBoxPassword.Text);

            if (databaseNames != null && databaseNames.Count > 0)
                using (SelectDatabaseForm selectDatabaseForm = new SelectDatabaseForm(databaseNames, textBoxWorldDatabase))
                    selectDatabaseForm.ShowDialog(this);

            buttonSearchWorldDb.Enabled = true;
        }

        private void listViewSmartScripts_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            //! Don't use the SortListView method here
            listViewSmartScripts.ListViewItemSorter = lvwColumnSorter;

            if (e.Column != lvwColumnSorter.SortColumn)
            {
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }
            else
                lvwColumnSorter.Order = lvwColumnSorter.Order == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;

            listViewSmartScripts.Sort();
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
                lvwColumnSorter.Order = order != SortOrder.None ? order : SortOrder.Ascending;

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
            if (listViewSmartScripts.Items.Count <= 1)
            {
                MessageBox.Show("There are not enough items in the listview in order to link!", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (listViewSmartScripts.SelectedItems.Count == 0)
            {
                MessageBox.Show("You must first select a line in the script", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SearchForLinkForm searchForLinkForm = new SearchForLinkForm(listViewSmartScripts.Scripts, listViewSmartScripts.SelectedItems[0].Index, textBoxToChange))
                searchForLinkForm.ShowDialog(this);
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
            textBoxEventType.Text = "0";
            textBoxActionType.Text = "0";
            textBoxTargetType.Text = "0";
            textBoxEventChance.Text = "100";
            textBoxId.Text = "-1";
            textBoxLinkFrom.Text = "None";
            textBoxLinkTo.Text = "0";
            textBoxComments.Text = SAI_Editor_Manager.Instance.GetDefaultCommentForSourceType(GetSourceTypeByIndex());
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
            foreach (TabPage page in tabControlParameters.TabPages)
                foreach (Control control in page.Controls)
                    if (control is Button)
                        control.Visible = visible;
        }

        private void SetVisibilityOfAllParamButtonsInTab(string tabText, bool visible)
        {
            foreach (TabPage page in tabControlParameters.TabPages)
                if (page.Text == tabText)
                    foreach (Control control in page.Controls)
                        if (control is Button)
                            control.Visible = visible;
        }

        private void ShowSearchFromDatabaseForm(TextBox textBoxToChange, DatabaseSearchFormType searchType)
        {
            using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(textBoxToChange, searchType))
                searchFromDatabaseForm.ShowDialog(this);

            textBoxToChange.Focus();
        }

        private void ShowSelectForm(string formTemplate, TextBox textBoxToChange)
        {
            using (Form selectForm = (Form)Activator.CreateInstance(SAI_Editor_Manager.SearchFormsContainer[formTemplate], new object[] { textBoxToChange }))
                selectForm.ShowDialog(this);

            textBoxToChange.Focus();
        }

        private void buttonEventParamOneSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxEventParam1;

            switch ((SmartEvent)comboBoxEventType.SelectedIndex)
            {
                case SmartEvent.SMART_EVENT_SPELLHIT: //! Spell id
                case SmartEvent.SMART_EVENT_FRIENDLY_MISSING_BUFF: //! Spell id
                case SmartEvent.SMART_EVENT_HAS_AURA: //! Spell id
                case SmartEvent.SMART_EVENT_TARGET_BUFFED: //! Spell id
                case SmartEvent.SMART_EVENT_SPELLHIT_TARGET: //! Spell id
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSpell);
                    break;
                case SmartEvent.SMART_EVENT_RESPAWN: //! Respawn condition
                    ShowSelectForm("SmartRespawnCondition", textBoxToChange);
                    break;
                case SmartEvent.SMART_EVENT_SUMMON_DESPAWNED: //! Creature entry
                case SmartEvent.SMART_EVENT_SUMMONED_UNIT: //! Creature entry
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry);
                    break;
                case SmartEvent.SMART_EVENT_AREATRIGGER_ONTRIGGER: //! Areatrigger entry
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeAreaTrigger);
                    break;
                case SmartEvent.SMART_EVENT_GO_STATE_CHANGED: //! Go state
                    ShowSelectForm("GoStates", textBoxToChange);
                    break;
                case SmartEvent.SMART_EVENT_GAME_EVENT_START: //! Game event entry
                case SmartEvent.SMART_EVENT_GAME_EVENT_END: //! Game event entry
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGameEvent);
                    break;
                case SmartEvent.SMART_EVENT_MOVEMENTINFORM: //! Motion type
                    ShowSelectForm("MovementGeneratorType", textBoxToChange);
                    break;
                case SmartEvent.SMART_EVENT_ACCEPTED_QUEST: //! Quest id
                case SmartEvent.SMART_EVENT_REWARD_QUEST: //! Quest id
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeQuest);
                    break;
                case SmartEvent.SMART_EVENT_RECEIVE_EMOTE: //! Emote id
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeEmote);
                    break;
                case SmartEvent.SMART_EVENT_GOSSIP_SELECT: //! Gossip menu id
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGossipMenuOptionMenuId);
                    break;
                case SmartEvent.SMART_EVENT_DISTANCE_CREATURE: //! Creature guid
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureGuid);
                    break;
                case SmartEvent.SMART_EVENT_DISTANCE_GAMEOBJECT: //! Gameobject guid
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectGuid);
                    break;
            }
        }

        private void buttonEventParamTwoSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxEventParam2;

            switch ((SmartEvent)comboBoxEventType.SelectedIndex)
            {
                case SmartEvent.SMART_EVENT_SPELLHIT: //! Spell school
                case SmartEvent.SMART_EVENT_SPELLHIT_TARGET: //! Spell school
                    ShowSelectForm("SpellSchools", textBoxToChange);
                    break;
                case SmartEvent.SMART_EVENT_RESPAWN: //! Map
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeMap);
                    break;
                case SmartEvent.SMART_EVENT_TEXT_OVER: //! Creature entry
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry);
                    break;
                case SmartEvent.SMART_EVENT_GOSSIP_SELECT: //! Gossip id
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGossipMenuOptionId);
                    break;
                case SmartEvent.SMART_EVENT_DISTANCE_CREATURE: //! Creature entry
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry);
                    break;
                case SmartEvent.SMART_EVENT_DISTANCE_GAMEOBJECT: //! Gameobject entry
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectEntry);
                    break;
            }
        }

        private void buttonEventParamThreeSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxEventParam3;

            switch ((SmartEvent)comboBoxEventType.SelectedIndex)
            {
                case SmartEvent.SMART_EVENT_RESPAWN: //! Zone
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeAreaOrZone);
                    break;
                case SmartEvent.SMART_EVENT_VICTIM_CASTING: //! Spell id
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSpell);
                    break;
            }
        }

        private void buttonEventParamFourSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxEventParam4;

            switch ((SmartEvent)comboBoxEventType.SelectedIndex)
            {
                case SmartEvent.SMART_EVENT_KILL: //! Creature entry
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry);
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
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry);
                    break;
                case SmartTarget.SMART_TARGET_CREATURE_GUID: //! Creature guid
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureGuid);
                    break;
                case SmartTarget.SMART_TARGET_GAMEOBJECT_RANGE:
                case SmartTarget.SMART_TARGET_GAMEOBJECT_DISTANCE:
                case SmartTarget.SMART_TARGET_CLOSEST_GAMEOBJECT: //! Gameobject entry
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectEntry);
                    break;
                case SmartTarget.SMART_TARGET_GAMEOBJECT_GUID: //! Gameobject guid
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectGuid);
                    break;
            }

            switch ((SmartAction)comboBoxActionType.SelectedIndex)
            {
                case SmartAction.SMART_ACTION_INSTALL_AI_TEMPLATE:
                    //! This button is different based on the number in the first parameter field
                    switch ((SmartAiTemplates)XConverter.ToInt32(textBoxActionParam1.Text))
                    {
                        case SmartAiTemplates.SMARTAI_TEMPLATE_CASTER:
                        case SmartAiTemplates.SMARTAI_TEMPLATE_TURRET:
                            ShowSelectForm("SmartCastFlags", textBoxToChange);
                            break;
                    }
                    break;
            }
        }

        private void buttonTargetParamTwoSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxTargetParam2;

            switch ((SmartTarget)comboBoxTargetType.SelectedIndex)
            {
                case SmartTarget.SMART_TARGET_CREATURE_GUID: //! Creature entry
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry);
                    break;
                case SmartTarget.SMART_TARGET_GAMEOBJECT_GUID: //! Gameobject entry
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectEntry);
                    break;
            }
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
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSpell);
                    break;
                case SmartAction.SMART_ACTION_SET_FACTION:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeFaction);
                    break;
                case SmartAction.SMART_ACTION_EMOTE:
                case SmartAction.SMART_ACTION_RANDOM_EMOTE:
                case SmartAction.SMART_ACTION_SET_EMOTE_STATE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeEmote);
                    break;
                case SmartAction.SMART_ACTION_FAIL_QUEST:
                case SmartAction.SMART_ACTION_ADD_QUEST:
                case SmartAction.SMART_ACTION_CALL_AREAEXPLOREDOREVENTHAPPENS:
                case SmartAction.SMART_ACTION_CALL_GROUPEVENTHAPPENS:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeQuest);
                    break;
                case SmartAction.SMART_ACTION_SET_REACT_STATE:
                    ShowSelectForm("ReactState", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_SOUND:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSound);
                    break;
                case SmartAction.SMART_ACTION_MORPH_TO_ENTRY_OR_MODEL:
                case SmartAction.SMART_ACTION_SUMMON_CREATURE:
                case SmartAction.SMART_ACTION_CALL_CASTEDCREATUREORGO:
                case SmartAction.SMART_ACTION_KILLED_MONSTER:
                case SmartAction.SMART_ACTION_UPDATE_TEMPLATE:
                case SmartAction.SMART_ACTION_MOUNT_TO_ENTRY_OR_MODEL:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry);
                    break;
                case SmartAction.SMART_ACTION_GO_SET_LOOT_STATE:
                    ShowSelectForm("GoStates", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_SET_POWER:
                case SmartAction.SMART_ACTION_ADD_POWER:
                case SmartAction.SMART_ACTION_REMOVE_POWER:
                    ShowSelectForm("PowerTypes", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_SUMMON_GO:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectEntry);
                    break;
                case SmartAction.SMART_ACTION_SET_EVENT_PHASE:
                case SmartAction.SMART_ACTION_RANDOM_PHASE:
                case SmartAction.SMART_ACTION_RANDOM_PHASE_RANGE:
                    ShowSelectForm("SmartPhaseMasks", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_SET_PHASE_MASK:
                    ShowSelectForm("PhaseMasks", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_ADD_ITEM:
                case SmartAction.SMART_ACTION_REMOVE_ITEM:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeItemEntry);
                    break;
                case SmartAction.SMART_ACTION_TELEPORT:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeMap);
                    break;
                case SmartAction.SMART_ACTION_SUMMON_CREATURE_GROUP:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSummonsId);
                    break;
                case SmartAction.SMART_ACTION_SET_SHEATH:
                    ShowSelectForm("SheathState", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_ACTIVATE_TAXI:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeTaxiPath);
                    break;
                case SmartAction.SMART_ACTION_SET_UNIT_FLAG:
                case SmartAction.SMART_ACTION_REMOVE_UNIT_FLAG:
                    //! There should be a different form opened based on parameter 2. If parameter two is set to '0' it means
                    //! we target UNIT_FIELD_FLAGS. If it's above 0 it means we target UNIT_FIELD_FLAGS2 (notice the 2).
                    if (textBoxActionParam2.Text == "0" || String.IsNullOrWhiteSpace(textBoxActionParam2.Text))
                        ShowSelectForm("UnitFlags", textBoxToChange);
                    else
                        ShowSelectForm("UnitFlags2", textBoxToChange);

                    break;
                case SmartAction.SMART_ACTION_SET_GO_FLAG:
                case SmartAction.SMART_ACTION_ADD_GO_FLAG:
                case SmartAction.SMART_ACTION_REMOVE_GO_FLAG:
                    ShowSelectForm("GoFlags", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_SET_DYNAMIC_FLAG:
                case SmartAction.SMART_ACTION_ADD_DYNAMIC_FLAG:
                case SmartAction.SMART_ACTION_REMOVE_DYNAMIC_FLAG:
                    ShowSelectForm("DynamicFlags", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_EQUIP:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeEquipTemplate);
                    break;
                case SmartAction.SMART_ACTION_SET_NPC_FLAG:
                case SmartAction.SMART_ACTION_ADD_NPC_FLAG:
                case SmartAction.SMART_ACTION_REMOVE_NPC_FLAG:
                    ShowSelectForm("NpcFlags", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_INSTALL_AI_TEMPLATE:
                    ShowSelectForm("SmartAiTemplates", textBoxToChange);
                    ParameterInstallAiTemplateChanged();
                    break;
                case SmartAction.SMART_ACTION_SET_UNIT_FIELD_BYTES_1:
                case SmartAction.SMART_ACTION_REMOVE_UNIT_FIELD_BYTES_1:
                    int searchType;

                    if (Int32.TryParse(textBoxActionParam2.Text, out searchType))
                    {
                        switch (searchType)
                        {
                            case 0:
                                ShowSelectForm("UnitStandStateType", textBoxToChange);
                                break;
                            //case 1:
                            //    break;
                            case 2:
                                ShowSelectForm("UnitStandFlags", textBoxToChange);
                                break;
                            case 3:
                                ShowSelectForm("UnitBytes1_Flags", textBoxToChange);
                                break;
                            default:
                                MessageBox.Show("The second parameter (type) must be set to a valid search type (0, 2 or 3).", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                        }
                    }

                    break;
            }
        }

        private void ParameterInstallAiTemplateChanged()
        {
            SetVisibilityOfAllParamButtonsInTab("Action", false);
            SetVisibilityOfAllParamButtonsInTab("Target", false);

            labelActionParam1.Text = String.Empty;
            labelActionParam2.Text = String.Empty;
            labelActionParam3.Text = String.Empty;
            labelActionParam4.Text = String.Empty;
            labelActionParam5.Text = String.Empty;
            labelActionParam6.Text = String.Empty;

            labelActionParam1.Text = "Template entry";
            buttonActionParamOneSearch.Visible = true;
            int newTemplateId = XConverter.ToInt32(textBoxActionParam1.Text);

            switch ((SmartAiTemplates)newTemplateId)
            {
                case SmartAiTemplates.SMARTAI_TEMPLATE_BASIC:
                case SmartAiTemplates.SMARTAI_TEMPLATE_PASSIVE:
                    break;
                case SmartAiTemplates.SMARTAI_TEMPLATE_CASTER:
                case SmartAiTemplates.SMARTAI_TEMPLATE_TURRET:
                    labelActionParam2.Text = "Spell id";
                    buttonActionParamTwoSearch.Visible = true; //! Spell id
                    labelActionParam3.Text = "RepeatMin";
                    labelActionParam4.Text = "RepeatMax";
                    labelActionParam5.Text = "Range";
                    labelActionParam6.Text = "Mana pct";

                    labelTargetParam1.Text = "Castflag";
                    buttonTargetParamOneSearch.Visible = true;
                    break;
                case SmartAiTemplates.SMARTAI_TEMPLATE_CAGED_GO_PART:
                    labelActionParam2.Text = "Creature entry";
                    buttonActionParamTwoSearch.Visible = true; //! Creature entry
                    labelActionParam3.Text = "Credit at end (0/1)";
                    break;
                case SmartAiTemplates.SMARTAI_TEMPLATE_CAGED_NPC_PART:
                    labelActionParam2.Text = "Gameobject entry";
                    buttonActionParamTwoSearch.Visible = true; //! Gameobject entry
                    labelActionParam3.Text = "Despawn time";
                    labelActionParam4.Text = "Walk/run (0/1)";
                    labelActionParam5.Text = "Distance";
                    labelActionParam6.Text = "Group id";
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
                    ShowSelectForm("SmartCastFlags", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_WP_STOP:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeQuest);
                    break;
                case SmartAction.SMART_ACTION_INTERRUPT_SPELL:
                case SmartAction.SMART_ACTION_CALL_CASTEDCREATUREORGO:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSpell);
                    break;
                case SmartAction.SMART_ACTION_SUMMON_CREATURE:
                    ShowSelectForm("TempSummonType", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_RANDOM_PHASE:
                case SmartAction.SMART_ACTION_RANDOM_PHASE_RANGE:
                    ShowSelectForm("SmartPhaseMasks", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_RANDOM_EMOTE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeEmote);
                    break;
                case SmartAction.SMART_ACTION_INSTALL_AI_TEMPLATE:
                    //! This button is different based on the number in the first parameter field
                    switch ((SmartAiTemplates)XConverter.ToInt32(textBoxActionParam1.Text))
                    {
                        case SmartAiTemplates.SMARTAI_TEMPLATE_CASTER:
                        case SmartAiTemplates.SMARTAI_TEMPLATE_TURRET:
                            ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSpell);
                            break;
                        case SmartAiTemplates.SMARTAI_TEMPLATE_CAGED_GO_PART:
                            ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry);
                            break;
                        case SmartAiTemplates.SMARTAI_TEMPLATE_CAGED_NPC_PART:
                            ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectEntry);
                            break;
                    }
                    break;
                case SmartAction.SMART_ACTION_WP_START:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeWaypoint);
                    break;
                case SmartAction.SMART_ACTION_SEND_GOSSIP_MENU:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeNpcText);
                    break;
                case SmartAction.SMART_ACTION_SET_UNIT_FIELD_BYTES_1:
                case SmartAction.SMART_ACTION_REMOVE_UNIT_FIELD_BYTES_1:
                    ShowSelectForm("UnitFieldBytes1Types", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_CALL_TIMED_ACTIONLIST:
                    ShowSelectForm("SmartActionlistTimerUpdateType", textBoxToChange);
                    break;
            }
        }

        private void buttonActionParamThreeSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxActionParam3;

            switch ((SmartAction)comboBoxActionType.SelectedIndex)
            {
                case SmartAction.SMART_ACTION_FOLLOW:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry);
                    break;
                case SmartAction.SMART_ACTION_CROSS_CAST:
                    ShowSelectForm("SmartTarget", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_RANDOM_PHASE:
                    ShowSelectForm("SmartPhaseMasks", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_EQUIP:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeItemEntry);
                    break;
                case SmartAction.SMART_ACTION_RANDOM_EMOTE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeEmote);
                    break;
            }
        }

        private void buttonActionParamFourSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxActionParam4;

            switch ((SmartAction)comboBoxActionType.SelectedIndex)
            {
                case SmartAction.SMART_ACTION_FOLLOW:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry);
                    break;
                case SmartAction.SMART_ACTION_WP_START:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeQuest);
                    break;
                case SmartAction.SMART_ACTION_RANDOM_PHASE:
                    ShowSelectForm("SmartPhaseMasks", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_EQUIP:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeItemEntry);
                    break;
                case SmartAction.SMART_ACTION_RANDOM_EMOTE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeEmote);
                    break;
            }
        }

        private void buttonActionParamFiveSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxActionParam5;

            switch ((SmartAction)comboBoxActionType.SelectedIndex)
            {
                case SmartAction.SMART_ACTION_RANDOM_PHASE:
                    ShowSelectForm("SmartPhaseMasks", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_EQUIP:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeItemEntry);
                    break;
                case SmartAction.SMART_ACTION_RANDOM_EMOTE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeEmote);
                    break;
            }
        }

        private void buttonActionParamSixSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxActionParam6;

            switch ((SmartAction)comboBoxActionType.SelectedIndex)
            {
                case SmartAction.SMART_ACTION_WP_START:
                    ShowSelectForm("ReactState", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_RANDOM_PHASE:
                    ShowSelectForm("SmartPhaseMasks", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_RANDOM_EMOTE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeEmote);
                    break;
            }
        }

        private void smartAIWikiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SAI_Editor_Manager.Instance.StartProcess("http://collab.kpsn.org/display/tc/smart_scripts");
        }

        private void textBoxComments_GotFocus(object sender, EventArgs e)
        {
            if (textBoxComments.Text == SAI_Editor_Manager.Instance.GetDefaultCommentForSourceType(GetSourceTypeByIndex()))
                textBoxComments.Text = String.Empty;
        }

        private void textBoxComments_LostFocus(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxComments.Text))
                textBoxComments.Text = SAI_Editor_Manager.Instance.GetDefaultCommentForSourceType(GetSourceTypeByIndex());
        }

        public void ExpandToShowPermanentTooltips(bool expand)
        {
            if (expandingListView == expand && contractingListView == !expand)
                return;

            expandingListView = expand;
            contractingListView = !expand;
            listViewSmartScriptsHeightToChangeTo = expand ? listViewSmartScripts.Height + (int)FormSizes.ListViewHeightContract : listViewSmartScripts.Height - (int)FormSizes.ListViewHeightContract;
            timerShowPermanentTooltips.Enabled = true;
            checkBoxUsePermanentTooltips.Checked = !expand;
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
                    checkBoxUsePermanentTooltips.Enabled = true;
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
                    checkBoxUsePermanentTooltips.Enabled = true;
                }
            }
        }

        private void comboBoxEventType_MouseEnter(object sender, EventArgs e)
        {
            UpdatePermanentTooltipOfTypes(sender as ComboBox, ScriptTypeId.ScriptTypeEvent);
        }

        private void comboBoxActionType_MouseEnter(object sender, EventArgs e)
        {
            UpdatePermanentTooltipOfTypes(sender as ComboBox, ScriptTypeId.ScriptTypeAction);
        }

        private void comboBoxTargetType_MouseEnter(object sender, EventArgs e)
        {
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

        private int GetSelectedIndexByScriptTypeId(ScriptTypeId scriptTypeId)
        {
            switch (scriptTypeId)
            {
                case ScriptTypeId.ScriptTypeEvent:
                    return comboBoxEventType.SelectedIndex;
                case ScriptTypeId.ScriptTypeAction:
                    return comboBoxActionType.SelectedIndex;
                case ScriptTypeId.ScriptTypeTarget:
                    return comboBoxTargetType.SelectedIndex;
            }

            return 0;
        }

        private string GetSelectedItemByScriptTypeId(ScriptTypeId scriptTypeId)
        {
            switch (scriptTypeId)
            {
                case ScriptTypeId.ScriptTypeEvent:
                    return comboBoxEventType.SelectedItem.ToString();
                case ScriptTypeId.ScriptTypeAction:
                    return comboBoxActionType.SelectedItem.ToString();
                case ScriptTypeId.ScriptTypeTarget:
                    return comboBoxTargetType.SelectedItem.ToString();
            }

            return String.Empty;
        }

        private void UpdatePermanentTooltipOfParameter(Label labelToTarget, int paramId, ComboBox comboBoxToTarget, ScriptTypeId scriptTypeId)
        {
            string toolTipOfType = SAI_Editor_Manager.Instance.GetParameterTooltipById(comboBoxToTarget.SelectedIndex, paramId, scriptTypeId);

            if (!String.IsNullOrWhiteSpace(toolTipOfType))
            {
                labelPermanentTooltipTextParameters.Text = toolTipOfType;
                labelPermanentTooltipParameterTitleTypes.Text = comboBoxToTarget.SelectedItem + " - " + labelToTarget.Text;
            }
        }

        private void labelEventParams_MouseEnter(object sender, EventArgs e)
        {
            UpdatePermanentTooltipOfParameter(sender as Label, (sender as LabelWithTooltip).TooltipParameterId, comboBoxEventType, ScriptTypeId.ScriptTypeEvent);
        }

        private void labelActionParams_MouseEnter(object sender, EventArgs e)
        {
            UpdatePermanentTooltipOfParameter(sender as Label, (sender as LabelWithTooltip).TooltipParameterId, comboBoxActionType, ScriptTypeId.ScriptTypeAction);
        }

        private void labelTargetParams_MouseEnter(object sender, EventArgs e)
        {
            UpdatePermanentTooltipOfParameter(sender as Label, (sender as LabelWithTooltip).TooltipParameterId, comboBoxTargetType, ScriptTypeId.ScriptTypeTarget);
        }

        private async void buttonNewLine_Click(object sender, EventArgs e)
        {
            if (listViewSmartScripts.Items.Count == 0)
            {
                if (!Settings.Default.UseWorldDatabase)
                {
                    TryToCreateScript(true);
                    return;
                }

                string aiName = await SAI_Editor_Manager.Instance.worldDatabase.GetObjectAiName(XConverter.ToInt32(textBoxEntryOrGuid.Text), (int)GetSourceTypeByIndex());

                if (!SAI_Editor_Manager.Instance.IsAiNameSmartAi(aiName))
                {
                    DialogResult dialogResult = MessageBox.Show("Are you sure you want to create a new script for the given entry and sourcetype?", "Something went wrong", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (dialogResult == DialogResult.Yes)
                        TryToCreateScript(true);
                }
                else
                    TryToCreateScript(true);

                return;
            }

            buttonNewLine.Enabled = false;
            SmartScript newSmartScript = new SmartScript();
            newSmartScript.entryorguid = originalEntryOrGuidAndSourceType.entryOrGuid;
            newSmartScript.source_type = (int)originalEntryOrGuidAndSourceType.sourceType;

            if (checkBoxLockEventId.Checked)
                newSmartScript.id = ++lastSmartScriptIdOfScript;
            else
                newSmartScript.id = -1;

            if (Settings.Default.GenerateComments && Settings.Default.UseWorldDatabase)
                newSmartScript.comment = await CommentGenerator.Instance.GenerateCommentFor(newSmartScript, originalEntryOrGuidAndSourceType);
            else
                newSmartScript.comment = SAI_Editor_Manager.Instance.GetDefaultCommentForSourceType((SourceTypes)newSmartScript.source_type);

            newSmartScript.event_chance = 100;

            //! All strings have to be initialized otherwise they become null and give cause an exception in the future.
            newSmartScript.target_x = "0";
            newSmartScript.target_y = "0";
            newSmartScript.target_z = "0";
            newSmartScript.target_o = "0";
            int index = listViewSmartScripts.AddScript(newSmartScript, selectNewItem: true);
            HandleShowBasicInfo();

            buttonNewLine.Enabled = textBoxEntryOrGuid.Text.Length > 0;
        }

        private async void textBoxLinkTo_TextChanged(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
            {
                if (!updatingFieldsBasedOnSelectedScript && listViewSmartScripts.SelectedScript.id.ToString() == textBoxLinkTo.Text)
                {
                    MessageBox.Show("You can not link to or from the same id you're linking to.", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxLinkFrom.Text = GetLinkFromForSelection();
                    textBoxLinkTo.Text = "0";
                    return;
                }

                int linkTo = XConverter.ToInt32(textBoxLinkTo.Text);
                listViewSmartScripts.SelectedScript.link = linkTo;
                listViewSmartScripts.ReplaceScript(listViewSmartScripts.SelectedScript);
                await GenerateCommentForSmartScript(listViewSmartScripts.SelectedScript);

                foreach (SmartScript smartScript in listViewSmartScripts.Scripts)
                {
                    if (smartScript.id == linkTo)
                    {
                        if ((SmartEvent)smartScript.event_type == SmartEvent.SMART_EVENT_LINK)
                            await GenerateCommentForSmartScript(smartScript, false);

                        break;
                    }
                }
            }
        }

        private void textBoxComments_TextChanged(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
            {
                listViewSmartScripts.SelectedScript.comment = textBoxComments.Text;
                listViewSmartScripts.ReplaceScript(listViewSmartScripts.SelectedScript);
                ResizeColumns();
            }
        }

        private async void textBoxEventPhasemask_TextChanged(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
            {
                listViewSmartScripts.SelectedScript.event_phase_mask = XConverter.ToInt32(textBoxEventPhasemask.Text);
                listViewSmartScripts.ReplaceScript(listViewSmartScripts.SelectedScript);
                await GenerateCommentForSmartScript(listViewSmartScripts.SelectedScript);
            }
        }

        private async void textBoxEventChance_ValueChanged(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
            {
                listViewSmartScripts.SelectedScript.event_chance = (int)textBoxEventChance.Value; //! Using .Text propert results in wrong value
                listViewSmartScripts.ReplaceScript(listViewSmartScripts.SelectedScript);
                await GenerateCommentForSmartScript(listViewSmartScripts.SelectedScript);
            }
        }

        private async void textBoxEventFlags_TextChanged(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
            {
                listViewSmartScripts.SelectedScript.event_flags = XConverter.ToInt32(textBoxEventFlags.Text);
                listViewSmartScripts.ReplaceScript(listViewSmartScripts.SelectedScript);
                await GenerateCommentForSmartScript(listViewSmartScripts.SelectedScript);
            }
        }

        private async void textBoxLinkFrom_TextChanged(object sender, EventArgs e)
        {
            int newLinkFrom = 0;// XConverter.ToInt32(textBoxLinkFrom.Text);

            try
            {
                newLinkFrom = Int32.Parse(textBoxLinkFrom.Text);
            }
            catch
            {
                previousLinkFrom = -1;
                return;
            }

            //! Only if the property was changed by hand (by user) and not by selecting a line
            if (!updatingFieldsBasedOnSelectedScript)
            {
                if (newLinkFrom == listViewSmartScripts.SelectedScript.id)
                {
                    MessageBox.Show("You can not link to or from the same id you're linking to.", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxLinkFrom.Text = GetLinkFromForSelection();
                    previousLinkFrom = -1;
                    return;
                }

                if (previousLinkFrom == newLinkFrom)
                    return;

                for (int i = 0; i < listViewSmartScripts.Scripts.Count; ++i)
                {
                    SmartScript smartScript = listViewSmartScripts.Scripts[i];

                    if (smartScript.entryorguid != originalEntryOrGuidAndSourceType.entryOrGuid || smartScript.source_type != (int)originalEntryOrGuidAndSourceType.sourceType)
                        continue;

                    if (smartScript.link == previousLinkFrom)
                    {
                        smartScript.link = 0;
                        await GenerateCommentForSmartScript(smartScript, false);
                    }

                    if (smartScript.id == newLinkFrom && listViewSmartScripts.SelectedScript != null)
                    {
                        smartScript.link = listViewSmartScripts.SelectedScript.id;
                        await GenerateCommentForSmartScript(smartScript, false);
                    }
                }

                listViewSmartScripts.Init(true);
            }

            previousLinkFrom = newLinkFrom;
        }

        private async void textBoxEventParam1_Leave(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
            {
                listViewSmartScripts.SelectedScript.event_param1 = XConverter.ToInt32(textBoxEventParam1.Text);
                listViewSmartScripts.ReplaceScript(listViewSmartScripts.SelectedScript);
                await GenerateCommentForSmartScript(listViewSmartScripts.SelectedScript);
            }
        }

        private async void textBoxEventParam2_Leave(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
            {
                listViewSmartScripts.SelectedScript.event_param2 = XConverter.ToInt32(textBoxEventParam2.Text);
                listViewSmartScripts.ReplaceScript(listViewSmartScripts.SelectedScript);
                await GenerateCommentForSmartScript(listViewSmartScripts.SelectedScript);
            }
        }

        private async void textBoxEventParam3_Leave(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
            {
                listViewSmartScripts.SelectedScript.event_param3 = XConverter.ToInt32(textBoxEventParam3.Text);
                listViewSmartScripts.ReplaceScript(listViewSmartScripts.SelectedScript);
                await GenerateCommentForSmartScript(listViewSmartScripts.SelectedScript);
            }
        }

        private async void textBoxEventParam4_Leave(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
            {
                listViewSmartScripts.SelectedScript.event_param4 = XConverter.ToInt32(textBoxEventParam4.Text);
                listViewSmartScripts.ReplaceScript(listViewSmartScripts.SelectedScript);
                await GenerateCommentForSmartScript(listViewSmartScripts.SelectedScript);
            }
        }

        private async void textBoxActionParam1_Leave(object sender, EventArgs e)
        {
            if ((SmartAction)comboBoxActionType.SelectedIndex == SmartAction.SMART_ACTION_INSTALL_AI_TEMPLATE)
                ParameterInstallAiTemplateChanged();

            if (listViewSmartScripts.SelectedItems.Count > 0)
            {
                listViewSmartScripts.SelectedScript.action_param1 = XConverter.ToInt32(textBoxActionParam1.Text);
                listViewSmartScripts.ReplaceScript(listViewSmartScripts.SelectedScript);
                await GenerateCommentForSmartScript(listViewSmartScripts.SelectedScript);
            }
        }

        private async void textBoxActionParam2_Leave(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
            {
                listViewSmartScripts.SelectedScript.action_param2 = XConverter.ToInt32(textBoxActionParam2.Text);
                listViewSmartScripts.ReplaceScript(listViewSmartScripts.SelectedScript);
                await GenerateCommentForSmartScript(listViewSmartScripts.SelectedScript);
            }
        }

        private async void textBoxActionParam3_Leave(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
            {
                listViewSmartScripts.SelectedScript.action_param3 = XConverter.ToInt32(textBoxActionParam3.Text);
                listViewSmartScripts.ReplaceScript(listViewSmartScripts.SelectedScript);
                await GenerateCommentForSmartScript(listViewSmartScripts.SelectedScript);
            }
        }

        private async void textBoxActionParam4_Leave(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
            {
                listViewSmartScripts.SelectedScript.action_param4 = XConverter.ToInt32(textBoxActionParam4.Text);
                listViewSmartScripts.ReplaceScript(listViewSmartScripts.SelectedScript);
                await GenerateCommentForSmartScript(listViewSmartScripts.SelectedScript);
            }
        }

        private async void textBoxActionParam5_Leave(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
            {
                listViewSmartScripts.SelectedScript.action_param5 = XConverter.ToInt32(textBoxActionParam5.Text);
                listViewSmartScripts.ReplaceScript(listViewSmartScripts.SelectedScript);
                await GenerateCommentForSmartScript(listViewSmartScripts.SelectedScript);
            }
        }

        private async void textBoxActionParam6_Leave(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
            {
                listViewSmartScripts.SelectedScript.action_param6 = XConverter.ToInt32(textBoxActionParam6.Text);
                listViewSmartScripts.ReplaceScript(listViewSmartScripts.SelectedScript);
                await GenerateCommentForSmartScript(listViewSmartScripts.SelectedScript);
            }
        }

        private async void textBoxTargetParam1_Leave(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
            {
                listViewSmartScripts.SelectedScript.target_param1 = XConverter.ToInt32(textBoxTargetParam1.Text);
                listViewSmartScripts.ReplaceScript(listViewSmartScripts.SelectedScript);
                await GenerateCommentForSmartScript(listViewSmartScripts.SelectedScript);
            }
        }

        private async void textBoxTargetParam2_Leave(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
            {
                listViewSmartScripts.SelectedScript.target_param2 = XConverter.ToInt32(textBoxTargetParam2.Text);
                listViewSmartScripts.ReplaceScript(listViewSmartScripts.SelectedScript);
                await GenerateCommentForSmartScript(listViewSmartScripts.SelectedScript);
            }
        }

        private async void textBoxTargetParam3_Leave(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
            {
                listViewSmartScripts.SelectedScript.target_param3 = XConverter.ToInt32(textBoxTargetParam3.Text);
                listViewSmartScripts.ReplaceScript(listViewSmartScripts.SelectedScript);
                await GenerateCommentForSmartScript(listViewSmartScripts.SelectedScript);
            }
        }

        private void textBoxTargetCoordinateParams_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsPunctuation(e.KeyChar) && !Char.IsDigit(e.KeyChar))
                return;

            double result;

            if (!Double.TryParse((sender as TextBox).Text + e.KeyChar, out result))
                e.Handled = true;
        }

        private async void textBoxTargetX_Leave(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
            {
                textBoxTargetX.Text = textBoxTargetX.Text.Replace(".", ",");
                textBoxTargetX.SelectionStart = textBoxTargetX.Text.Length + 1; //! Set cursor to end of text
                listViewSmartScripts.SelectedScript.target_x = textBoxTargetX.Text;
                listViewSmartScripts.ReplaceScript(listViewSmartScripts.SelectedScript);
                await GenerateCommentForSmartScript(listViewSmartScripts.SelectedScript);
            }
        }

        private async void textBoxTargetY_Leave(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
            {
                textBoxTargetY.Text = textBoxTargetY.Text.Replace(".", ",");
                textBoxTargetY.SelectionStart = textBoxTargetY.Text.Length + 1; //! Set cursor to end of text
                listViewSmartScripts.SelectedScript.target_y = textBoxTargetY.Text;
                listViewSmartScripts.ReplaceScript(listViewSmartScripts.SelectedScript);
                await GenerateCommentForSmartScript(listViewSmartScripts.SelectedScript);
            }
        }

        private async void textBoxTargetZ_Leave(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
            {
                textBoxTargetZ.Text = textBoxTargetZ.Text.Replace(".", ",");
                textBoxTargetZ.SelectionStart = textBoxTargetZ.Text.Length + 1; //! Set cursor to end of text
                listViewSmartScripts.SelectedScript.target_z = textBoxTargetZ.Text;
                listViewSmartScripts.ReplaceScript(listViewSmartScripts.SelectedScript);
                await GenerateCommentForSmartScript(listViewSmartScripts.SelectedScript);
            }
        }

        private async void textBoxTargetO_Leave(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
            {
                textBoxTargetO.Text = textBoxTargetO.Text.Replace(".", ",");
                textBoxTargetO.SelectionStart = textBoxTargetO.Text.Length + 1; //! Set cursor to end of text
                listViewSmartScripts.SelectedScript.target_o = textBoxTargetO.Text;
                listViewSmartScripts.ReplaceScript(listViewSmartScripts.SelectedScript);
                await GenerateCommentForSmartScript(listViewSmartScripts.SelectedScript);
            }
        }

        private async void textBoxEntryOrGuid_TextChanged(object sender, EventArgs e)
        {
            pictureBoxLoadScript.Enabled = textBoxEntryOrGuid.Text.Length > 0 && Settings.Default.UseWorldDatabase;
            pictureBoxCreateScript.Enabled = textBoxEntryOrGuid.Text.Length > 0;
            buttonNewLine.Enabled = textBoxEntryOrGuid.Text.Length > 0;

            if (checkBoxAllowChangingEntryAndSourceType.Checked && listViewSmartScripts.SelectedItems.Count > 0)
            {
                listViewSmartScripts.SelectedScript.entryorguid = XConverter.ToInt32(textBoxEntryOrGuid.Text);
                listViewSmartScripts.ReplaceScript(listViewSmartScripts.SelectedScript);
                await GenerateCommentForSmartScript(listViewSmartScripts.SelectedScript);

                //! When all entryorguids are the same, also adjust the originalEntryOrGuid data
                List<EntryOrGuidAndSourceType> uniqueEntriesOrGuidsAndSourceTypes = SAI_Editor_Manager.Instance.GetUniqueEntriesOrGuidsAndSourceTypes(listViewSmartScripts.Scripts);

                if (uniqueEntriesOrGuidsAndSourceTypes != null && uniqueEntriesOrGuidsAndSourceTypes.Count == 1)
                {
                    originalEntryOrGuidAndSourceType.entryOrGuid = uniqueEntriesOrGuidsAndSourceTypes[0].entryOrGuid;
                    originalEntryOrGuidAndSourceType.sourceType = uniqueEntriesOrGuidsAndSourceTypes[0].sourceType;
                }
            }
        }

        private async void comboBoxSourceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SourceTypes newSourceType = GetSourceTypeByIndex();

            if (listViewSmartScripts.Items.Count == 0)
                textBoxComments.Text = SAI_Editor_Manager.Instance.GetDefaultCommentForSourceType(newSourceType);

            if (checkBoxAllowChangingEntryAndSourceType.Checked && listViewSmartScripts.SelectedItems.Count > 0)
            {
                listViewSmartScripts.SelectedScript.source_type = (int)newSourceType;
                listViewSmartScripts.ReplaceScript(listViewSmartScripts.SelectedScript);
                await GenerateCommentForSmartScript(listViewSmartScripts.SelectedScript);
            }

            //! When no database connection can be made, only enable the search button if
            //! we're searching for areatriggers.
            buttonSearchForEntryOrGuid.Enabled = Settings.Default.UseWorldDatabase || newSourceType == SourceTypes.SourceTypeAreaTrigger;
        }

        private async void generateSQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (formState != FormState.FormStateMain)
                return;

            using (SqlOutputForm sqlOutputForm = new SqlOutputForm(await GenerateSmartAiSqlFromListView(), true, await GenerateSmartAiRevertQuery()))
                sqlOutputForm.ShowDialog(this);
        }

        private async void buttonGenerateSql_Click(object sender, EventArgs e)
        {
            if (formState != FormState.FormStateMain)
                return;

            using (SqlOutputForm sqlOutputForm = new SqlOutputForm(await GenerateSmartAiSqlFromListView(), true, await GenerateSmartAiRevertQuery()))
                sqlOutputForm.ShowDialog(this);
        }

        private async Task<string> GenerateSmartAiSqlFromListView()
        {
            List<EntryOrGuidAndSourceType> entriesOrGuidsAndSourceTypes = SAI_Editor_Manager.Instance.GetUniqueEntriesOrGuidsAndSourceTypes(listViewSmartScripts.Scripts);
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

            switch (originalEntryOrGuidAndSourceType.sourceType)
            {
                case SourceTypes.SourceTypeCreature:
                case SourceTypes.SourceTypeGameobject:
                    if (!Settings.Default.UseWorldDatabase)
                    {
                        sourceName = " <Could not generate name>";
                        break;
                    }

                    sourceName = " " + await SAI_Editor_Manager.Instance.worldDatabase.GetObjectNameByIdOrGuidAndSourceType(originalEntryOrGuidAndSourceType.sourceType, originalEntryOrGuidAndSourceType.entryOrGuid);
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

            bool originalEntryIsGuid = originalEntryOrGuidAndSourceType.entryOrGuid < 0;
            string sourceSet = originalEntryIsGuid ? "@GUID" : "@ENTRY";

            generatedSql += "--" + sourceName + " SAI\n";
            generatedSql += "SET " + sourceSet + " := " + originalEntryOrGuidAndSourceType.entryOrGuid + ";\n";

            if (entriesOrGuidsAndSourceTypes.Count == 1)
            {
                switch (originalEntryOrGuidAndSourceType.sourceType)
                {
                    case SourceTypes.SourceTypeCreature:
                        if (!Settings.Default.UseWorldDatabase)
                        {
                            generatedSql += "-- No changes to the AIName were made as there is no world database connection.\n";
                            break;
                        }

                        if (originalEntryIsGuid)
                        {
                            int actualEntry = await SAI_Editor_Manager.Instance.worldDatabase.GetCreatureIdByGuid(-originalEntryOrGuidAndSourceType.entryOrGuid);
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
                            int actualEntry = await SAI_Editor_Manager.Instance.worldDatabase.GetGameobjectIdByGuid(-originalEntryOrGuidAndSourceType.entryOrGuid);
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

                generatedSql += "DELETE FROM `smart_scripts` WHERE `entryorguid`=" + sourceSet + " AND `source_type`=" + (int)originalEntryOrGuidAndSourceType.sourceType + ";\n";
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

                                if (entryOrGuidAndSourceType.entryOrGuid == originalEntryOrGuidAndSourceType.entryOrGuid)
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
                                        string sourceTypeString = SAI_Editor_Manager.Instance.GetSourceTypeString(entryOrGuidAndSourceType.sourceType).ToLower();
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

                            if (entryOrGuidAndSourceType.entryOrGuid == originalEntryOrGuidAndSourceType.entryOrGuid)
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

                        if (listEntryOrGuidAndSourceTypes[0].entryOrGuid == originalEntryOrGuidAndSourceType.entryOrGuid)
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

            List<SmartScript> smartScripts = listViewSmartScripts.Scripts;
            smartScripts = smartScripts.OrderBy(smartScript => smartScript.entryorguid).ToList();

            for (int i = 0; i < smartScripts.Count; ++i)
            {
                SmartScript smartScript = smartScripts[i];
                string actualSourceSet = sourceSet;

                if (originalEntryOrGuidAndSourceType.entryOrGuid != smartScripts[i].entryorguid)
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

                //! SQL accepts a period instead of a comma for float/double values
                string target_x = smartScript.target_x.Replace(",", ".");
                string target_y = smartScript.target_y.Replace(",", ".");
                string target_z = smartScript.target_z.Replace(",", ".");
                string target_o = smartScript.target_o.Replace(",", ".");

                generatedSql += "(" + actualSourceSet + "," + smartScript.source_type + "," + smartScript.id + "," + smartScript.link + "," + smartScript.event_type + "," +
                                              smartScript.event_phase_mask + "," + smartScript.event_chance + "," + smartScript.event_flags + "," + eventParameters[0] + "," +
                                              eventParameters[1] + "," + eventParameters[2] + "," + eventParameters[3] + "," + smartScript.action_type + "," +
                                              actionParameters[0] + "," + actionParameters[1] + "," + actionParameters[2] + "," + actionParameters[3] + "," +
                                              actionParameters[4] + "," + actionParameters[5] + "," + smartScript.target_type + "," + targetParameters[0] + "," +
                                              targetParameters[1] + "," + targetParameters[2] + "," + target_x + "," + target_y + "," + target_z + "," + target_o + "," +
                                              '"' + smartScript.comment + '"' + ")";

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
                    string stringToReplace = originalEntryOrGuidAndSourceType.entryOrGuid + "0" + i.ToString() + characterToReplace;

                    if (!generatedSql.Contains(stringToReplace))
                    {
                        stringToReplace = originalEntryOrGuidAndSourceType.entryOrGuid + "0" + i.ToString() + ")";

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
            List<EntryOrGuidAndSourceType> entriesOrGuidsAndSourceTypes = SAI_Editor_Manager.Instance.GetUniqueEntriesOrGuidsAndSourceTypes(listViewSmartScripts.Scripts);

            foreach (EntryOrGuidAndSourceType entryOrGuidAndSourceType in entriesOrGuidsAndSourceTypes)
            {
                List<SmartScript> smartScripts = await SAI_Editor_Manager.Instance.worldDatabase.GetSmartScripts(entryOrGuidAndSourceType.entryOrGuid, (int)entryOrGuidAndSourceType.sourceType);
                string scriptName = String.Empty, aiName = String.Empty;

                switch (entryOrGuidAndSourceType.sourceType)
                {
                    case SourceTypes.SourceTypeCreature:
                        scriptName = await SAI_Editor_Manager.Instance.worldDatabase.GetCreatureScriptNameById(entryOrGuidAndSourceType.entryOrGuid);
                        aiName = await SAI_Editor_Manager.Instance.worldDatabase.GetCreatureAiNameById(entryOrGuidAndSourceType.entryOrGuid);

                        revertQuery += "UPDATE creature_template SET Ainame='" + aiName + "',Scriptname='" + scriptName + "' WHERE entry=" + entryOrGuidAndSourceType.entryOrGuid + ";";
                        break;
                    case SourceTypes.SourceTypeGameobject:
                        scriptName = await SAI_Editor_Manager.Instance.worldDatabase.GetGameobjectScriptNameById(entryOrGuidAndSourceType.entryOrGuid);
                        aiName = await SAI_Editor_Manager.Instance.worldDatabase.GetGameobjectAiNameById(entryOrGuidAndSourceType.entryOrGuid);

                        revertQuery += "UPDATE gameobject_template SET Ainame='" + aiName + "',Scriptname='" + await SAI_Editor_Manager.Instance.worldDatabase.GetGameobjectScriptNameById(entryOrGuidAndSourceType.entryOrGuid) + "' WHERE entry=" + entryOrGuidAndSourceType.entryOrGuid + ";";
                        break;
                    case SourceTypes.SourceTypeAreaTrigger:
                        scriptName = await SAI_Editor_Manager.Instance.worldDatabase.GetAreaTriggerScriptNameById(entryOrGuidAndSourceType.entryOrGuid);

                        if (scriptName != String.Empty)
                            revertQuery += "UPDATE areatrigger_scripts SET Scriptname='" + scriptName + "' WHERE entry=" + entryOrGuidAndSourceType.entryOrGuid + ";";
                        else
                            revertQuery += "DELETE FROM areatrigger_scripts WHERE entry=" + entryOrGuidAndSourceType.entryOrGuid + ";";

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
                    revertQuery += "DELETE FROM smart_scripts WHERE entryorguid=" + entryOrGuidAndSourceType.entryOrGuid + ";";
            }

            return revertQuery;
        }

        public async void GenerateCommentsForAllItems()
        {
            if (listViewSmartScripts.Scripts.Count == 0)
                return;

            for (int i = 0; i < listViewSmartScripts.Scripts.Count; ++i)
            {
                SmartScript smartScript = listViewSmartScripts.Scripts[i];
                string newComment = await CommentGenerator.Instance.GenerateCommentFor(smartScript, originalEntryOrGuidAndSourceType, true, GetInitialSmartScriptLink(smartScript));
                smartScript.comment = newComment;
                listViewSmartScripts.ReplaceScript(smartScript);
                FillFieldsBasedOnSelectedScript();
            }

            textBoxComments.Text = listViewSmartScripts.SelectedScript.comment;
        }

        private void buttonGenerateComments_Click(object sender, EventArgs e)
        {
            if (!Settings.Default.UseWorldDatabase)
                return;

            GenerateCommentsForAllItems();
            ResizeColumns();
            listViewSmartScripts.Select();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Control control in Controls)
                control.Enabled = false;

            if (adjustedLoginSettings)
                SaveLastUsedFields();

            if (updateSurveyThread != null)
                updateSurveyThread.Abort();
        }

        private void SaveLastUsedFields()
        {
            Settings.Default.LastEntryOrGuid = textBoxEntryOrGuid.Text;
            Settings.Default.LastSourceType = comboBoxSourceType.SelectedIndex;
            Settings.Default.ShowBasicInfo = checkBoxShowBasicInfo.Checked;
            Settings.Default.LockSmartScriptId = checkBoxLockEventId.Checked;
            Settings.Default.ListActionLists = checkBoxListActionlistsOrEntries.Checked;
            Settings.Default.AllowChangingEntryAndSourceType = checkBoxAllowChangingEntryAndSourceType.Checked;
            Settings.Default.PhaseHighlighting = checkBoxUsePhaseColors.Checked;
            Settings.Default.ShowTooltipsPermanently = checkBoxUsePermanentTooltips.Checked;

            if (formState == FormState.FormStateLogin)
            {
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] buffer = new byte[1024];
                rng.GetBytes(buffer);
                string salt = BitConverter.ToString(buffer);
                rng.Dispose();

                Settings.Default.Entropy = salt;
                Settings.Default.Host = textBoxHost.Text;
                Settings.Default.User = textBoxUsername.Text;
                Settings.Default.Password = textBoxPassword.Text.Length == 0 ? String.Empty : textBoxPassword.Text.ToSecureString().EncryptString(Encoding.Unicode.GetBytes(salt));
                Settings.Default.Database = textBoxWorldDatabase.Text;
                Settings.Default.Port = XConverter.ToUInt32(textBoxPort.Text);
                Settings.Default.UseWorldDatabase = radioButtonConnectToMySql.Checked;
                Settings.Default.AutoConnect = checkBoxAutoConnect.Checked;
            }

            Settings.Default.Save();
        }

        private void ResizeColumns()
        {
            foreach (ColumnHeader header in listViewSmartScripts.Columns)
                header.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private async Task GenerateCommentForSmartScript(SmartScript smartScript, bool resize = true)
        {
            if (smartScript == null || !Settings.Default.GenerateComments)
                return;

            string newComment = smartScript.comment;

            if (!updatingFieldsBasedOnSelectedScript)
            {
                newComment = await CommentGenerator.Instance.GenerateCommentFor(smartScript, originalEntryOrGuidAndSourceType, true, GetInitialSmartScriptLink(smartScript));

                if (smartScript.link != 0 && (SmartEvent)smartScript.event_type != SmartEvent.SMART_EVENT_LINK)
                    await GenerateCommentForAllEventsLinkingFromSmartScript(smartScript);
            }

            //! For some reason we have to re-check it here...
            if (listViewSmartScripts.SelectedItems.Count == 0)
                return;

            string oldComment = smartScript.comment;
            smartScript.comment = newComment;
            listViewSmartScripts.ReplaceScript(smartScript);

            if (!updatingFieldsBasedOnSelectedScript)
                FillFieldsBasedOnSelectedScript();

            if (oldComment != newComment)
                ResizeColumns();
        }

        private async Task GenerateCommentForAllEventsLinkingFromSmartScript(SmartScript smartScript)
        {
            if (smartScript == null || !Settings.Default.GenerateComments || smartScript.link == 0)
                return;

            List<SmartScript> smartScriptsLinkedFrom = GetAllSmartScriptThatLinkFrom(smartScript);

            if (smartScriptsLinkedFrom == null || smartScriptsLinkedFrom.Count == 0)
                return;

            for (int i = 0; i < smartScriptsLinkedFrom.Count; ++i)
            {
                SmartScript smartScriptListView = smartScriptsLinkedFrom[i];

                if (smartScriptListView.entryorguid != smartScript.entryorguid)
                    continue;

                smartScriptListView.comment = await CommentGenerator.Instance.GenerateCommentFor(smartScriptListView, originalEntryOrGuidAndSourceType, true, smartScript);
                listViewSmartScripts.ReplaceScript(smartScriptListView);
            }
        }

        private async Task GenerateCommentForAllEventsLinkingFromAndToSmartScript(SmartScript smartScript)
        {
            if (smartScript == null || !Settings.Default.GenerateComments)
                return;

            for (int i = 0; i < listViewSmartScripts.Scripts.Count; ++i)
            {
                SmartScript smartScriptListView = listViewSmartScripts.Scripts[i];

                if (smartScriptListView.entryorguid != smartScript.entryorguid)
                    continue;

                if (smartScript.link == smartScriptListView.id)
                {
                    smartScriptListView.comment = await CommentGenerator.Instance.GenerateCommentFor(smartScriptListView, originalEntryOrGuidAndSourceType, true, GetInitialSmartScriptLink(smartScriptListView));
                    listViewSmartScripts.ReplaceScript(smartScriptListView);
                }
                else if (smartScriptListView.link == smartScript.id)
                {
                    smartScript.comment = await CommentGenerator.Instance.GenerateCommentFor(smartScript, originalEntryOrGuidAndSourceType, true, GetInitialSmartScriptLink(smartScript));
                    listViewSmartScripts.ReplaceScript(smartScript);
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
            foreach (SmartScript smartScriptInListView in listViewSmartScripts.Scripts)
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
                smartScriptLink = null;
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

            foreach (SmartScript smartScriptInListView in listViewSmartScripts.Scripts)
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
            if (formState != FormState.FormStateMain)
                return;

            using (RevertQueryForm revertQueryForm = new RevertQueryForm())
                revertQueryForm.ShowDialog(this);
        }

        private void checkBoxShowBasicInfo_CheckedChanged(object sender, EventArgs e)
        {
            HandleShowBasicInfo();
        }

        private void HandleShowBasicInfo()
        {
            int prevSelectedIndex = listViewSmartScripts.SelectedItems.Count > 0 ? listViewSmartScripts.SelectedItems[0].Index : 0;

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

            if (checkBoxShowBasicInfo.Checked)
                listViewSmartScripts.ExcludeProperties(properties);
            else
                listViewSmartScripts.IncludeProperties(properties);

            if (listViewSmartScripts.Items.Count > prevSelectedIndex)
            {
                listViewSmartScripts.Items[prevSelectedIndex].Selected = true;
                listViewSmartScripts.EnsureVisible(prevSelectedIndex);
            }

            listViewSmartScripts.Select(); //! Sets the focus on the listview
        }

        private async void menuItemGenerateCommentListView_Click(object sender, EventArgs e)
        {
            if (formState != FormState.FormStateMain || listViewSmartScripts.SelectedScript == null || !Settings.Default.UseWorldDatabase)
                return;

            for (int i = 0; i < listViewSmartScripts.Scripts.Count; ++i)
            {
                SmartScript smartScript = listViewSmartScripts.Scripts[i];

                if (smartScript != listViewSmartScripts.SelectedScript)
                    continue;

                string newComment = await CommentGenerator.Instance.GenerateCommentFor(smartScript, originalEntryOrGuidAndSourceType, true, GetInitialSmartScriptLink(smartScript));
                smartScript.comment = newComment;
                listViewSmartScripts.ReplaceScript(smartScript);
                FillFieldsBasedOnSelectedScript();
            }

            textBoxComments.Text = listViewSmartScripts.SelectedScript.comment;
        }

        private void menuItemDuplicateSelectedRow_Click(object sender, EventArgs e)
        {
            if (formState != FormState.FormStateMain || listViewSmartScripts.SelectedScript == null)
                return;

            SmartScript clonedSmartScript = listViewSmartScripts.SelectedScript.Clone();

            if (!Settings.Default.DuplicatePrimaryFields)
                clonedSmartScript.id = ++lastSmartScriptIdOfScript;

            listViewSmartScripts.EnsureVisible(listViewSmartScripts.AddScript(clonedSmartScript, selectNewItem: true));
        }

        private void textBoxEventType_MouseWheel(object sender, MouseEventArgs e)
        {
            int newNumber = XConverter.ToInt32(textBoxEventType.Text);

            if (e.Delta > 0)
                newNumber--;
            else
                newNumber++;

            if (newNumber < 0)
                newNumber = 0;

            if (newNumber > (int)SmartEvent.SMART_EVENT_MAX - 1)
                newNumber = (int)SmartEvent.SMART_EVENT_MAX - 1;

            textBoxEventType.Text = newNumber.ToString();
        }

        private void textBoxActionType_MouseWheel(object sender, MouseEventArgs e)
        {
            int newNumber = XConverter.ToInt32(textBoxActionType.Text);

            if (e.Delta > 0)
                newNumber--;
            else
                newNumber++;

            if (newNumber < 0)
                newNumber = 0;

            if (newNumber > (int)SmartAction.SMART_ACTION_MAX - 1)
                newNumber = (int)SmartAction.SMART_ACTION_MAX - 1;

            textBoxActionType.Text = newNumber.ToString();
        }

        private void textBoxTargetType_MouseWheel(object sender, MouseEventArgs e)
        {
            int newNumber = XConverter.ToInt32(textBoxTargetType.Text);

            if (e.Delta > 0)
                newNumber--;
            else
                newNumber++;

            if (newNumber < 0)
                newNumber = 0;

            if (newNumber > (int)SmartTarget.SMART_TARGET_MAX - 1)
                newNumber = (int)SmartTarget.SMART_TARGET_MAX - 1;

            textBoxTargetType.Text = newNumber.ToString();
        }

        private void menuItemLoadSelectedEntry_Click(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedScript == null)
                return;

            int entryorguid = listViewSmartScripts.SelectedScript.entryorguid;
            SourceTypes source_type = (SourceTypes)listViewSmartScripts.SelectedScript.source_type;
            listViewSmartScripts.ClearScripts();
            listViewSmartScripts.Items.Clear();
            TryToLoadScript(entryorguid, source_type);
        }

        private async void textBoxId_TextChanged(object sender, EventArgs e)
        {
            if (listViewSmartScripts.SelectedItems.Count > 0)
            {
                listViewSmartScripts.SelectedScript.id = XConverter.ToInt32(textBoxId.Text);
                listViewSmartScripts.ReplaceScript(listViewSmartScripts.SelectedScript);
                await GenerateCommentForSmartScript(listViewSmartScripts.SelectedScript);
            }
        }

        private void radioButtonConnectToMySql_CheckedChanged(object sender, EventArgs e)
        {
            HandleRadioButtonUseDatabaseChanged();
        }

        private void radioButtonDontUseDatabase_CheckedChanged(object sender, EventArgs e)
        {
            HandleRadioButtonUseDatabaseChanged();
        }

        private void HandleRadioButtonUseDatabaseChanged()
        {
            textBoxHost.Enabled = radioButtonConnectToMySql.Checked;
            textBoxUsername.Enabled = radioButtonConnectToMySql.Checked;
            textBoxPassword.Enabled = radioButtonConnectToMySql.Checked;
            textBoxWorldDatabase.Enabled = radioButtonConnectToMySql.Checked;
            textBoxPort.Enabled = radioButtonConnectToMySql.Checked;
            buttonSearchWorldDb.Enabled = radioButtonConnectToMySql.Checked;
            labelDontUseDatabaseWarning.Visible = !radioButtonConnectToMySql.Checked;

            HandleHeightLoginFormBasedOnuseDatabaseSetting();
        }

        private void HandleHeightLoginFormBasedOnuseDatabaseSetting()
        {
            if (formState != FormState.FormStateMain)
            {
                if (radioButtonConnectToMySql.Checked)
                {
                    MaximumSize = new Size((int)FormSizes.MainFormWidth, (int)FormSizes.MainFormHeight);
                    Height = (int)FormSizes.LoginFormHeight;
                }
                else
                {
                    MaximumSize = new Size((int)FormSizes.MainFormWidth, (int)FormSizes.MainFormHeight);
                    Height = (int)FormSizes.LoginFormHeightShowWarning;
                }
            }
        }

        public void HandleUseWorldDatabaseSettingChanged()
        {
            radioButtonConnectToMySql.Checked = Settings.Default.UseWorldDatabase;
            radioButtonDontUseDatabase.Checked = !Settings.Default.UseWorldDatabase;
            buttonSearchForEntryOrGuid.Enabled = Settings.Default.UseWorldDatabase || comboBoxSourceType.SelectedIndex == 2;
            pictureBoxLoadScript.Enabled = textBoxEntryOrGuid.Text.Length > 0 && Settings.Default.UseWorldDatabase;
            checkBoxListActionlistsOrEntries.Enabled = Settings.Default.UseWorldDatabase;
            menuItemRevertQuery.Enabled = Settings.Default.UseWorldDatabase;
            SetGenerateCommentsEnabled(listViewSmartScripts.Items.Count > 0 && Settings.Default.UseWorldDatabase);
            searchForAQuestToolStripMenuItem1.Enabled = Settings.Default.UseWorldDatabase;
            searchForACreatureEntryToolStripMenuItem.Enabled = Settings.Default.UseWorldDatabase;
            searchForACreatureGuidToolStripMenuItem.Enabled = Settings.Default.UseWorldDatabase;
            searchForAGameobjectEntryToolStripMenuItem.Enabled = Settings.Default.UseWorldDatabase;
            searchForAGameobjectGuidToolStripMenuItem.Enabled = Settings.Default.UseWorldDatabase;
            searchForAGameEventToolStripMenuItem.Enabled = Settings.Default.UseWorldDatabase;
            searchForAnItemEntryToolStripMenuItem.Enabled = Settings.Default.UseWorldDatabase;
            searchForACreatureSummonsIdToolStripMenuItem.Enabled = Settings.Default.UseWorldDatabase;
            searchForAnEquipmentTemplateToolStripMenuItem.Enabled = Settings.Default.UseWorldDatabase;
            searchForAWaypointToolStripMenuItem.Enabled = Settings.Default.UseWorldDatabase;
            searchForANpcTextToolStripMenuItem.Enabled = Settings.Default.UseWorldDatabase;
            searchForAGossipMenuOptionToolStripMenuItem.Enabled = Settings.Default.UseWorldDatabase;
            searchForAGossipOptionIdToolStripMenuItem.Enabled = Settings.Default.UseWorldDatabase;

            if (Settings.Default.UseWorldDatabase)
                Text = "SAI-Editor " + applicationVersion + " - Connection: " + Settings.Default.User + ", " + Settings.Default.Host + ", " + Settings.Default.Port.ToString();
            else
                Text = "SAI-Editor " + applicationVersion + " - Creator-only mode, no database connection";
        }

        private void menuItemRetrieveLastDeletedRow_Click(object sender, EventArgs e)
        {
            if (lastDeletedSmartScripts.Count == 0)
            {
                MessageBox.Show("There are no items deleted in this session ready to be restored.", "Nothing to retrieve!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            listViewSmartScripts.AddScript(lastDeletedSmartScripts.Last());
            lastDeletedSmartScripts.Remove(lastDeletedSmartScripts.Last());
        }

        private void checkBoxUsePhaseColors_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.PhaseHighlighting = checkBoxUsePhaseColors.Checked;
            Settings.Default.Save();

            listViewSmartScripts.Init(true);
        }

        private void checkBoxUsePermanentTooltips_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxUsePermanentTooltips.Enabled = false;
            Settings.Default.ShowTooltipsPermanently = checkBoxUsePermanentTooltips.Checked;
            Settings.Default.Save();

            ExpandToShowPermanentTooltips(!checkBoxUsePermanentTooltips.Checked);
        }

        private void searchForASpellToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowSearchFromDatabaseForm(null, DatabaseSearchFormType.DatabaseSearchFormTypeSpell);
        }

        private void searchForAFactionToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowSearchFromDatabaseForm(null, DatabaseSearchFormType.DatabaseSearchFormTypeFaction);
        }

        private void searchForAnEmoteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowSearchFromDatabaseForm(null, DatabaseSearchFormType.DatabaseSearchFormTypeEmote);
        }

        private void searchForAMapToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowSearchFromDatabaseForm(null, DatabaseSearchFormType.DatabaseSearchFormTypeQuest);
        }

        private void searchForAQuestToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowSearchFromDatabaseForm(null, DatabaseSearchFormType.DatabaseSearchFormTypeMap);
        }

        private void searchForAZoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSearchFromDatabaseForm(null, DatabaseSearchFormType.DatabaseSearchFormTypeAreaOrZone);
        }

        private void searchForACreatureEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSearchFromDatabaseForm(null, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry);
        }

        private void searchForACreatureGuidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSearchFromDatabaseForm(null, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureGuid);
        }

        private void searchForAGameobjectEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSearchFromDatabaseForm(null, DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectEntry);
        }

        private void searchForAGameobjectGuidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSearchFromDatabaseForm(null, DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectGuid);
        }

        private void searchForASoundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSearchFromDatabaseForm(null, DatabaseSearchFormType.DatabaseSearchFormTypeSound);
        }

        private void searchForAnAreatriggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSearchFromDatabaseForm(null, DatabaseSearchFormType.DatabaseSearchFormTypeAreaTrigger);
        }

        private void searchForAGameEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSearchFromDatabaseForm(null, DatabaseSearchFormType.DatabaseSearchFormTypeGameEvent);
        }

        private void searchForAnItemEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSearchFromDatabaseForm(null, DatabaseSearchFormType.DatabaseSearchFormTypeItemEntry);
        }

        private void searchForACreatureSummonsIdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSearchFromDatabaseForm(null, DatabaseSearchFormType.DatabaseSearchFormTypeSummonsId);
        }

        private void searchForATaxiPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSearchFromDatabaseForm(null, DatabaseSearchFormType.DatabaseSearchFormTypeTaxiPath);
        }

        private void searchForAnEquipmentTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSearchFromDatabaseForm(null, DatabaseSearchFormType.DatabaseSearchFormTypeEquipTemplate);
        }

        private void searchForAWaypointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSearchFromDatabaseForm(null, DatabaseSearchFormType.DatabaseSearchFormTypeWaypoint);
        }

        private void searchForANpcTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSearchFromDatabaseForm(null, DatabaseSearchFormType.DatabaseSearchFormTypeNpcText);
        }

        private void searchForAGossipOptionIdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSearchFromDatabaseForm(null, DatabaseSearchFormType.DatabaseSearchFormTypeGossipMenuOptionMenuId);
        }

        private void searchForAGossipMenuOptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSearchFromDatabaseForm(null, DatabaseSearchFormType.DatabaseSearchFormTypeGossipMenuOptionId);
        }

        private void conditionEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (formState != FormState.FormStateMain)
                return;

            foreach (Form form in Application.OpenForms)
            {
                if (form.Name == "ConditionForm")
                {
                    form.BringToFront();
                    form.Show(); //! Show the form in case it's hidden
                    (form as ConditionForm).formHidden = false;
                    return;
                }
            }

            ConditionForm conditionForm = new ConditionForm();
            conditionForm.formHidden = false;
            conditionForm.Show();
        }

        Dictionary<string, Type> searchEventHandlers = new Dictionary<string, Type>()
        {
            {"Search for gameobject flags", typeof(MultiSelectForm<GoFlags>)},
            {"Search for unit flags", typeof(MultiSelectForm<UnitFlags>)},
            {"Search for unit flags 2", typeof(MultiSelectForm<UnitFlags2>)},
            {"Search for dynamic flags", typeof(MultiSelectForm<DynamicFlags>)},
            {"Search for npc flags", typeof(MultiSelectForm<NpcFlags>)},
            {"Search for unit stand flags", typeof(SingleSelectForm<UnitStandStateType>)},
            {"Search for unit bytes1 flags", typeof(MultiSelectForm<UnitBytes1_Flags>)},
            {"Search for SAI event flags", typeof(MultiSelectForm<SmartEventFlags>)},
            {"Search for SAI phase masks", typeof(MultiSelectForm<SmartPhaseMasks>)},
            {"Search for SAI cast flags", typeof(MultiSelectForm<SmartCastFlags>)},
            {"Search for SAI templates", typeof(SingleSelectForm<SmartAiTemplates>)},
            {"Search for SAI respawn conditions", typeof(SingleSelectForm<SmartRespawnCondition>)},
            {"Search for SAI event types", typeof(SingleSelectForm<SmartEvent>)},
            {"Search for SAI action types", typeof(SingleSelectForm<SmartAction>)},
            {"Search for SAI target types", typeof(SingleSelectForm<SmartTarget>)},
            {"Search for SAI actionlist timer update types", typeof(SingleSelectForm<SmartActionlistTimerUpdateType>)},
            {"Search for gameobject states", typeof(SingleSelectForm<GoStates>)},
            {"Search for react states", typeof(SingleSelectForm<ReactState>)},
            {"Search for sheath states", typeof(SingleSelectForm<SheathState>)},
            {"Search for movement generator types", typeof(SingleSelectForm<MovementGeneratorType>)},
            {"Search for spell schools", typeof(SingleSelectForm<SpellSchools>)},
            {"Search for power types", typeof(SingleSelectForm<PowerTypes>)},
            {"Search for unit stand state types", typeof(SingleSelectForm<UnitStandStateType>)},
            {"Search for temp summon types", typeof(SingleSelectForm<TempSummonType>)},
        };

        private void searchForFlagsMenuItem_Click(object sender, EventArgs e)
        {
            using (Form selectForm = (Form)Activator.CreateInstance(searchEventHandlers[((ToolStripItem)sender).Text], new object[] { null }))
                selectForm.ShowDialog(this);
        }
    }
}
