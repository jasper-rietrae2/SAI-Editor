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
        public int expandAndContractSpeed = 5;
        public bool runningConstructor = false;
        private bool contractingToLoginForm = false, expandingToMainForm = false, adjustedLoginSettings = false;
        private int originalHeight = 0, originalWidth = 0;
        private int MainFormWidth = (int)FormSizes.MainFormWidth, MainFormHeight = (int)FormSizes.MainFormHeight;
        private List<SmartScript> lastDeletedSmartScripts = new List<SmartScript>(), smartScriptsOnClipBoard = new List<SmartScript>();
        private Thread updateSurveyThread = null, checkIfUpdatesAvailableThread = null;
        private FormState formState = FormState.FormStateLogin;
        private string applicationVersion = String.Empty;
        private System.Windows.Forms.Timer timerCheckForInternetConnection = new System.Windows.Forms.Timer();

        private List<UserControlTabPage> userControls = new List<UserControlTabPage>();

        public MainForm()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            ResizeRedraw = true;
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

            //! HAS to be called before try-catch block
            tabControl.TabPages.Clear(); //! We only have it in the designer to get an idea of how stuff looks
            CreateTabControl(true);

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

                foreach (UserControlTabPage uc in userControls)
                {
                    uc.checkBoxListActionlistsOrEntries.Enabled = Settings.Default.UseWorldDatabase;
                    uc.buttonGenerateComments.Enabled = uc.listViewSmartScripts.Items.Count > 0 && Settings.Default.UseWorldDatabase;
                    uc.buttonSearchForEntryOrGuid.Enabled = Settings.Default.UseWorldDatabase || (SourceTypes)Settings.Default.LastSourceType == SourceTypes.SourceTypeAreaTrigger;
                }

                menuItemRevertQuery.Enabled = Settings.Default.UseWorldDatabase;
                menuItemGenerateComment.Enabled = GetActiveUserControl().listViewSmartScripts.Items.Count > 0 && Settings.Default.UseWorldDatabase;
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

            tabControl.Visible = false;
            customPanelLogin.Visible = true;

            customPanelLogin.Location = new Point(9, 8);

            //! We first load the information and then change the parameter fields
            await SAI_Editor_Manager.Instance.LoadSQLiteDatabaseInfo();

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

            foreach (UserControlTabPage uc in userControls)
            {
                uc.tabControlParameters.AutoScrollOffset = new Point(5, 5);

                //! Static scrollbar to the parameters tabpage windows
                foreach (TabPage page in uc.tabControlParameters.TabPages)
                {
                    page.HorizontalScroll.Enabled = false;
                    page.HorizontalScroll.Visible = false;
                    page.AutoScroll = true;
                    page.AutoScrollMinSize = new Size(page.Width, page.Height);
                }
            }

            if (Settings.Default.HidePass)
                textBoxPassword.PasswordChar = '●';

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

            runningConstructor = false;
        }

        public UserControlTabPage GetActiveUserControl()
        {
            if (userControls.Count == 0)
                return null;

            return userControls[tabControl.SelectedIndex];
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

        protected override void WndProc(ref Message m)
        {
            //! Don't allow moving the window while we are expanding or contracting. This is required because
            //! the window often breaks and has an incorrect size in the end if the application had been moved
            //! while expanding or contracting.
            if (((m.Msg == 274 && m.WParam.ToInt32() == 61456) || (m.Msg == 161 && m.WParam.ToInt32() == 2)) && (expandingToMainForm || contractingToLoginForm))
                return;

            base.WndProc(ref m);
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

            foreach (UserControlTabPage uc in userControls)
            {
                uc.panelStaticTooltipTypes.Visible = false;
                uc.panelStaticTooltipParameters.Visible = false;
            }
        }

        private void ResetFieldsToDefault()
        {
            foreach (UserControlTabPage uc in userControls)
                uc.ResetFieldsToDefault();
        }

        private void StartContractingToLoginForm(bool instant = false)
        {
            Text = "SAI-Editor " + applicationVersion + ": Login";

            if (Settings.Default.ShowTooltipsStaticly)
                foreach (UserControlTabPage uc in userControls)
                    uc.listViewSmartScripts.Height += (int)FormSizes.ListViewHeightContract;

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

            tabControl.Visible = false;
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
                            break;
                    }
                    break;
            }
        }

        private void menuItemReconnect_Click(object sender, EventArgs e)
        {
            if (formState != FormState.FormStateMain || GetActiveUserControl().contractingListView || GetActiveUserControl().expandingListView)
                return;

            for (int i = 0; i < Application.OpenForms.Count; ++i)
                if (Application.OpenForms[i] != this)
                    Application.OpenForms[i].Close();

            foreach (UserControlTabPage uc in userControls)
            {
                uc.panelStaticTooltipTypes.Visible = false;
                uc.panelStaticTooltipParameters.Visible = false;
            }

            SaveLastUsedFields();
            ResetFieldsToDefault();

            foreach (UserControlTabPage uc in userControls)
                uc.listViewSmartScripts.ClearScripts();

            StartContractingToLoginForm(Settings.Default.InstantExpand);
        }

        private void FinishedExpandingOrContracting(bool expanding)
        {
            customPanelLogin.Visible = !expanding;
            tabControl.Visible = expanding;
            menuStrip.Visible = expanding;
            Invalidate();

            if (!expanding)
                HandleHeightLoginFormBasedOnuseDatabaseSetting();

            foreach (UserControlTabPage uc in userControls)
            {
                uc.panelStaticTooltipTypes.Visible = false;
                uc.panelStaticTooltipParameters.Visible = false;
            }

            foreach (UserControlTabPage uc in userControls)
            {
                uc.textBoxEntryOrGuid.Text = Settings.Default.LastEntryOrGuid;
                uc.comboBoxSourceType.SelectedIndex = Settings.Default.LastSourceType;
                uc.checkBoxShowBasicInfo.Checked = Settings.Default.ShowBasicInfo;
                uc.checkBoxLockEventId.Checked = Settings.Default.LockSmartScriptId;
                uc.checkBoxListActionlistsOrEntries.Checked = Settings.Default.ListActionLists;
                uc.checkBoxAllowChangingEntryAndSourceType.Checked = Settings.Default.AllowChangingEntryAndSourceType;
                uc.checkBoxUsePhaseColors.Checked = Settings.Default.PhaseHighlighting;
                uc.checkBoxUseStaticTooltips.Checked = Settings.Default.ShowTooltipsStaticly;
            }

            foreach (UserControlTabPage uc in userControls)
                uc.FinishedExpandingOrContracting(expanding);
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

        private void menuOptionDeleteSelectedRow_Click(object sender, EventArgs e)
        {
            SmartScriptListView listViewSmartScripts = GetActiveUserControl().listViewSmartScripts;

            if (formState != FormState.FormStateMain || listViewSmartScripts.SelectedScript == null)
                return;

            if (listViewSmartScripts.SelectedItems.Count <= 0)
            {
                MessageBox.Show("No rows were selected to delete!", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            GetActiveUserControl().DeleteSelectedRow();
        }

        private void menuItemCopySelectedRowListView_Click(object sender, EventArgs e)
        {
            SmartScriptListView listViewSmartScripts = GetActiveUserControl().listViewSmartScripts;

            if (formState != FormState.FormStateMain || listViewSmartScripts.SelectedScript == null)
                return;

            smartScriptsOnClipBoard.Add(listViewSmartScripts.SelectedScript.Clone());
        }

        private void menuItemPasteLastCopiedRow_Click(object sender, EventArgs e)
        {
            SmartScriptListView listViewSmartScripts = GetActiveUserControl().listViewSmartScripts;

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

        private void testToolStripMenuItemDeleteRow_Click(object sender, EventArgs e)
        {
            GetActiveUserControl().DeleteSelectedRow();
        }

        private void smartAIWikiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SAI_Editor_Manager.Instance.StartProcess("http://collab.kpsn.org/display/tc/smart_scripts");
        }

        private async void generateSQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (formState != FormState.FormStateMain)
                return;

            using (SqlOutputForm sqlOutputForm = new SqlOutputForm(await GetActiveUserControl().GenerateSmartAiSqlFromListView(), true, await GetActiveUserControl().GenerateSmartAiRevertQuery()))
                sqlOutputForm.ShowDialog(this);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (UserControlTabPage uc in userControls)
                foreach (Control control in uc.Controls)
                    control.Enabled = false;

            foreach (Control control in Controls)
                control.Enabled = false;

            if (adjustedLoginSettings)
                SaveLastUsedFields();

            if (updateSurveyThread != null)
                updateSurveyThread.Abort();
        }

        private void SaveLastUsedFields()
        {
            //Settings.Default.LastEntryOrGuid = textBoxEntryOrGuid.Text;
            //Settings.Default.LastSourceType = comboBoxSourceType.SelectedIndex;
            //Settings.Default.ShowBasicInfo = checkBoxShowBasicInfo.Checked;
            //Settings.Default.LockSmartScriptId = checkBoxLockEventId.Checked;
            //Settings.Default.ListActionLists = checkBoxListActionlistsOrEntries.Checked;
            //Settings.Default.AllowChangingEntryAndSourceType = checkBoxAllowChangingEntryAndSourceType.Checked;
            //Settings.Default.PhaseHighlighting = checkBoxUsePhaseColors.Checked;
            //Settings.Default.ShowTooltipsStaticly = checkBoxUseStaticTooltips.Checked;

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

        private void menuItemRevertQuery_Click(object sender, EventArgs e)
        {
            if (formState != FormState.FormStateMain)
                return;

            using (RevertQueryForm revertQueryForm = new RevertQueryForm())
                revertQueryForm.ShowDialog(this);
        }

        private async void menuItemGenerateCommentListView_Click(object sender, EventArgs e)
        {
            if (formState != FormState.FormStateMain || !Settings.Default.UseWorldDatabase)
                return;

            await GetActiveUserControl().GenerateCommentListView();
        }

        private void menuItemDuplicateSelectedRow_Click(object sender, EventArgs e)
        {
            if (formState != FormState.FormStateMain)
                return;

            GetActiveUserControl().DuplicateSelectedRow();
        }

        private void menuItemLoadSelectedEntry_Click(object sender, EventArgs e)
        {
            GetActiveUserControl().LoadSelectedEntry();
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

            foreach (UserControlTabPage uc in userControls)
            {
                uc.buttonSearchForEntryOrGuid.Enabled = Settings.Default.UseWorldDatabase || uc.comboBoxSourceType.SelectedIndex == 2;
                uc.pictureBoxLoadScript.Enabled = uc.textBoxEntryOrGuid.Text.Length > 0 && Settings.Default.UseWorldDatabase;
                uc.checkBoxListActionlistsOrEntries.Enabled = Settings.Default.UseWorldDatabase;
                uc.buttonGenerateComments.Enabled = uc.listViewSmartScripts.Items.Count > 0 && Settings.Default.UseWorldDatabase;
            }

            menuItemRevertQuery.Enabled = Settings.Default.UseWorldDatabase;
            menuItemGenerateComment.Enabled = GetActiveUserControl().listViewSmartScripts.Items.Count > 0 && Settings.Default.UseWorldDatabase;
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

            Text = "SAI-Editor " + applicationVersion + " - ";

            if (Settings.Default.UseWorldDatabase)
                Text += "Connection: " + Settings.Default.User + ", " + Settings.Default.Host + ", " + Settings.Default.Port.ToString();
            else
                Text += "Creator-only mode, no database connection";
        }

        private void menuItemRetrieveLastDeletedRow_Click(object sender, EventArgs e)
        {
            if (lastDeletedSmartScripts.Count == 0)
            {
                MessageBox.Show("There are no items deleted in this session ready to be restored.", "Nothing to retrieve!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            GetActiveUserControl().listViewSmartScripts.AddScript(lastDeletedSmartScripts.Last());
            lastDeletedSmartScripts.Remove(lastDeletedSmartScripts.Last());
        }

        private void ShowSearchFromDatabaseForm(TextBox textBoxToChange, DatabaseSearchFormType searchType)
        {
            GetActiveUserControl().ShowSearchFromDatabaseForm(textBoxToChange, searchType);
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

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == null || tabControl.SelectedTab.Text != "+")
                return;

            CreateTabControl();
        }

        private void CreateTabControl(bool first = false)
        {
            if (!first)
                tabControl.TabPages.RemoveAt(tabControl.TabPages.Count - 1);

            UserControlTabPage userControlTabPage = new UserControlTabPage();
            userControlTabPage.Parent = this;
            userControlTabPage.LoadUserControl();
            TabPage newPage = new TabPage();
            newPage.Text = "Tab " + (tabControl.TabPages.Count + 1).ToString();
            newPage.Controls.Add(userControlTabPage);
            tabControl.TabPages.Add(newPage);
            tabControl.TabPages.Add(new TabPage("+"));
            userControls.Add(userControlTabPage);

            if (!first)
                tabControl.SelectedIndex = tabControl.TabPages.Count - 2;
        }
    }
}
