using SAI_Editor.Classes;

namespace SAI_Editor
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.buttonConnect = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuHeaderFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemGenerateSql = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemRevertQuery = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemReconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuitemLoadSelectedEntry = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemGenerateComment = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDuplicateRow = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDeleteSelectedRow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemRetrieveLastDeletedRow = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCopySelectedRow = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemPasteLastCopiedRow = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.otherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smartAIWikiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripListView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemLoadSelectedEntryListView = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemGenerateCommentListView = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDuplicateSelectedRowListView = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDeleteSelectedRowListView = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCopySelectedRowListView = new System.Windows.Forms.ToolStripMenuItem();
            this.panelLoginBox = new System.Windows.Forms.Panel();
            this.groupBoxLogin = new System.Windows.Forms.GroupBox();
            this.radioButtonDontUseDatabase = new System.Windows.Forms.RadioButton();
            this.radioButtonConnectToMySql = new System.Windows.Forms.RadioButton();
            this.buttonSearchWorldDb = new System.Windows.Forms.Button();
            this.checkBoxAutoConnect = new System.Windows.Forms.CheckBox();
            this.textBoxHost = new System.Windows.Forms.TextBox();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.labelUser = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxWorldDatabase = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.labelHost = new System.Windows.Forms.Label();
            this.labelPort = new System.Windows.Forms.Label();
            this.labelDontUseDatabaseWarning = new System.Windows.Forms.Label();
            this.groupBoxStaticScriptInfo = new System.Windows.Forms.GroupBox();
            this.buttonSearchForEntryOrGuid = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxSourceType = new System.Windows.Forms.ComboBox();
            this.labelEntryOrGuid = new System.Windows.Forms.Label();
            this.textBoxEntryOrGuid = new System.Windows.Forms.TextBox();
            this.groupBoxPreferences = new System.Windows.Forms.GroupBox();
            this.checkBoxAllowChangingEntryAndSourceType = new System.Windows.Forms.CheckBox();
            this.checkBoxListActionlistsOrEntries = new System.Windows.Forms.CheckBox();
            this.checkBoxShowBasicInfo = new System.Windows.Forms.CheckBox();
            this.checkBoxScriptByGuid = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBoxLockEventId = new System.Windows.Forms.CheckBox();
            this.groupBoxScriptInfo = new System.Windows.Forms.GroupBox();
            this.buttonLinkTo = new System.Windows.Forms.Button();
            this.buttonLinkFrom = new System.Windows.Forms.Button();
            this.comboBoxTargetType = new System.Windows.Forms.ComboBox();
            this.buttonSelectEventFlag = new System.Windows.Forms.Button();
            this.buttonSearchPhasemask = new System.Windows.Forms.Button();
            this.comboBoxActionType = new System.Windows.Forms.ComboBox();
            this.buttonSearchEventFlags = new System.Windows.Forms.Button();
            this.textBoxTargetType = new System.Windows.Forms.TextBox();
            this.textBoxEventChance = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.textBoxEventFlags = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxComments = new System.Windows.Forms.TextBox();
            this.textBoxActionType = new System.Windows.Forms.TextBox();
            this.textBoxEventPhasemask = new System.Windows.Forms.TextBox();
            this.textBoxEventType = new System.Windows.Forms.TextBox();
            this.textBoxLinkTo = new System.Windows.Forms.TextBox();
            this.textBoxLinkFrom = new System.Windows.Forms.TextBox();
            this.comboBoxEventType = new System.Windows.Forms.ComboBox();
            this.textBoxId = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.labelEventParam1 = new System.Windows.Forms.Label();
            this.textBoxEventParam1 = new System.Windows.Forms.TextBox();
            this.tabControlParameters = new System.Windows.Forms.TabControl();
            this.tabPageEvent = new System.Windows.Forms.TabPage();
            this.buttonEventParamFourSearch = new System.Windows.Forms.Button();
            this.buttonEventParamThreeSearch = new System.Windows.Forms.Button();
            this.buttonEventParamTwoSearch = new System.Windows.Forms.Button();
            this.buttonEventParamOneSearch = new System.Windows.Forms.Button();
            this.labelEventParam4 = new System.Windows.Forms.Label();
            this.labelEventParam3 = new System.Windows.Forms.Label();
            this.labelEventParam2 = new System.Windows.Forms.Label();
            this.textBoxEventParam4 = new System.Windows.Forms.TextBox();
            this.textBoxEventParam3 = new System.Windows.Forms.TextBox();
            this.textBoxEventParam2 = new System.Windows.Forms.TextBox();
            this.tabPageAction = new System.Windows.Forms.TabPage();
            this.buttonActionParamSixSearch = new System.Windows.Forms.Button();
            this.buttonActionParamFiveSearch = new System.Windows.Forms.Button();
            this.buttonActionParamFourSearch = new System.Windows.Forms.Button();
            this.buttonActionParamThreeSearch = new System.Windows.Forms.Button();
            this.buttonActionParamTwoSearch = new System.Windows.Forms.Button();
            this.buttonActionParamOneSearch = new System.Windows.Forms.Button();
            this.labelActionParam6 = new System.Windows.Forms.Label();
            this.labelActionParam5 = new System.Windows.Forms.Label();
            this.labelActionParam4 = new System.Windows.Forms.Label();
            this.labelActionParam3 = new System.Windows.Forms.Label();
            this.labelActionParam2 = new System.Windows.Forms.Label();
            this.textBoxActionParam6 = new System.Windows.Forms.TextBox();
            this.textBoxActionParam5 = new System.Windows.Forms.TextBox();
            this.textBoxActionParam4 = new System.Windows.Forms.TextBox();
            this.textBoxActionParam3 = new System.Windows.Forms.TextBox();
            this.textBoxActionParam2 = new System.Windows.Forms.TextBox();
            this.labelActionParam1 = new System.Windows.Forms.Label();
            this.textBoxActionParam1 = new System.Windows.Forms.TextBox();
            this.tabPageTarget = new System.Windows.Forms.TabPage();
            this.buttonTargetParamSevenSearch = new System.Windows.Forms.Button();
            this.buttonTargetParamSixSearch = new System.Windows.Forms.Button();
            this.buttonTargetParamFiveSearch = new System.Windows.Forms.Button();
            this.buttonTargetParamFourSearch = new System.Windows.Forms.Button();
            this.buttonTargetParamThreeSearch = new System.Windows.Forms.Button();
            this.buttonTargetParamTwoSearch = new System.Windows.Forms.Button();
            this.buttonTargetParamOneSearch = new System.Windows.Forms.Button();
            this.labelTargetO = new System.Windows.Forms.Label();
            this.labelTargetZ = new System.Windows.Forms.Label();
            this.labelTargetY = new System.Windows.Forms.Label();
            this.labelTargetX = new System.Windows.Forms.Label();
            this.textBoxTargetO = new System.Windows.Forms.TextBox();
            this.textBoxTargetZ = new System.Windows.Forms.TextBox();
            this.labelTargetParam3 = new System.Windows.Forms.Label();
            this.textBoxTargetY = new System.Windows.Forms.TextBox();
            this.labelTargetParam2 = new System.Windows.Forms.Label();
            this.textBoxTargetX = new System.Windows.Forms.TextBox();
            this.textBoxTargetParam3 = new System.Windows.Forms.TextBox();
            this.textBoxTargetParam2 = new System.Windows.Forms.TextBox();
            this.labelTargetParam1 = new System.Windows.Forms.Label();
            this.textBoxTargetParam1 = new System.Windows.Forms.TextBox();
            this.groupBoxParameters = new System.Windows.Forms.GroupBox();
            this.LoadTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.panelPermanentTooltipParameters = new System.Windows.Forms.Panel();
            this.labelPermanentTooltipTextParameters = new System.Windows.Forms.Label();
            this.labelPermanentTooltipParameterTitleTypes = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonNewLine = new System.Windows.Forms.Button();
            this.buttonGenerateSql = new System.Windows.Forms.Button();
            this.buttonGenerateComments = new System.Windows.Forms.Button();
            this.timerExpandOrContract = new System.Windows.Forms.Timer(this.components);
            this.timerShowPermanentTooltips = new System.Windows.Forms.Timer(this.components);
            this.pictureBoxPermanentTooltip = new System.Windows.Forms.PictureBox();
            this.labelPermanentTooltipTextTypes = new System.Windows.Forms.Label();
            this.labelPermanentTooltipTitleTypes = new System.Windows.Forms.Label();
            this.panelPermanentTooltipTypes = new System.Windows.Forms.Panel();
            this.listViewSmartScripts = new SAI_Editor.Classes.SmartScriptListView();
            this.pictureBoxCreateScript = new SAI_Editor.Classes.PictureBoxDisableable();
            this.pictureBoxLoadScript = new SAI_Editor.Classes.PictureBoxDisableable();
            this.menuStrip.SuspendLayout();
            this.contextMenuStripListView.SuspendLayout();
            this.panelLoginBox.SuspendLayout();
            this.groupBoxLogin.SuspendLayout();
            this.groupBoxStaticScriptInfo.SuspendLayout();
            this.groupBoxPreferences.SuspendLayout();
            this.groupBoxScriptInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxEventChance)).BeginInit();
            this.tabControlParameters.SuspendLayout();
            this.tabPageEvent.SuspendLayout();
            this.tabPageAction.SuspendLayout();
            this.tabPageTarget.SuspendLayout();
            this.groupBoxParameters.SuspendLayout();
            this.panelPermanentTooltipParameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPermanentTooltip)).BeginInit();
            this.panelPermanentTooltipTypes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCreateScript)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoadScript)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(3, 158);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 9;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(147, 158);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 10;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(292, 158);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 11;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuHeaderFiles,
            this.optionsToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.otherToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1318, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // menuHeaderFiles
            // 
            this.menuHeaderFiles.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemSettings,
            this.menuItemGenerateSql,
            this.menuItemRevertQuery,
            this.toolStripSeparator2,
            this.menuItemReconnect,
            this.menuItemExit});
            this.menuHeaderFiles.Name = "menuHeaderFiles";
            this.menuHeaderFiles.Size = new System.Drawing.Size(37, 20);
            this.menuHeaderFiles.Text = "File";
            // 
            // menuItemSettings
            // 
            this.menuItemSettings.Name = "menuItemSettings";
            this.menuItemSettings.ShortcutKeyDisplayString = "(F1)";
            this.menuItemSettings.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.menuItemSettings.Size = new System.Drawing.Size(244, 22);
            this.menuItemSettings.Text = "Settings";
            this.menuItemSettings.Click += new System.EventHandler(this.menuItemSettings_Click);
            // 
            // menuItemGenerateSql
            // 
            this.menuItemGenerateSql.Enabled = false;
            this.menuItemGenerateSql.Name = "menuItemGenerateSql";
            this.menuItemGenerateSql.ShortcutKeyDisplayString = "(Ctrl + M)";
            this.menuItemGenerateSql.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.menuItemGenerateSql.Size = new System.Drawing.Size(244, 22);
            this.menuItemGenerateSql.Text = "Generate SQL";
            this.menuItemGenerateSql.Click += new System.EventHandler(this.generateSQLToolStripMenuItem_Click);
            // 
            // menuItemRevertQuery
            // 
            this.menuItemRevertQuery.Name = "menuItemRevertQuery";
            this.menuItemRevertQuery.ShortcutKeyDisplayString = "(Ctrl + R)";
            this.menuItemRevertQuery.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.menuItemRevertQuery.Size = new System.Drawing.Size(244, 22);
            this.menuItemRevertQuery.Text = "Execute a revert query";
            this.menuItemRevertQuery.Click += new System.EventHandler(this.menuItemRevertQuery_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(241, 6);
            // 
            // menuItemReconnect
            // 
            this.menuItemReconnect.Name = "menuItemReconnect";
            this.menuItemReconnect.ShortcutKeyDisplayString = "(Shift + F4)";
            this.menuItemReconnect.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F4)));
            this.menuItemReconnect.Size = new System.Drawing.Size(244, 22);
            this.menuItemReconnect.Text = "Re-connect";
            this.menuItemReconnect.Click += new System.EventHandler(this.menuItemReconnect_Click);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.ShortcutKeyDisplayString = "(Alt + F4)";
            this.menuItemExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.menuItemExit.Size = new System.Drawing.Size(244, 22);
            this.menuItemExit.Text = "Exit";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuitemLoadSelectedEntry,
            this.menuItemGenerateComment,
            this.menuItemDuplicateRow,
            this.menuItemDeleteSelectedRow,
            this.toolStripSeparator1,
            this.menuItemRetrieveLastDeletedRow,
            this.menuItemCopySelectedRow,
            this.menuItemPasteLastCopiedRow});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // menuitemLoadSelectedEntry
            // 
            this.menuitemLoadSelectedEntry.Enabled = false;
            this.menuitemLoadSelectedEntry.Name = "menuitemLoadSelectedEntry";
            this.menuitemLoadSelectedEntry.ShortcutKeyDisplayString = "(Ctrl + L)";
            this.menuitemLoadSelectedEntry.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.menuitemLoadSelectedEntry.Size = new System.Drawing.Size(319, 22);
            this.menuitemLoadSelectedEntry.Text = "Load selected entry";
            this.menuitemLoadSelectedEntry.Click += new System.EventHandler(this.menuItemLoadSelectedEntry_Click);
            // 
            // menuItemGenerateComment
            // 
            this.menuItemGenerateComment.Enabled = false;
            this.menuItemGenerateComment.Name = "menuItemGenerateComment";
            this.menuItemGenerateComment.ShortcutKeyDisplayString = "(Ctrl + G)";
            this.menuItemGenerateComment.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.menuItemGenerateComment.Size = new System.Drawing.Size(319, 22);
            this.menuItemGenerateComment.Text = "Generate comment for selected row";
            this.menuItemGenerateComment.Click += new System.EventHandler(this.menuItemGenerateCommentListView_Click);
            // 
            // menuItemDuplicateRow
            // 
            this.menuItemDuplicateRow.Enabled = false;
            this.menuItemDuplicateRow.Name = "menuItemDuplicateRow";
            this.menuItemDuplicateRow.ShortcutKeyDisplayString = "(Ctrl + Q)";
            this.menuItemDuplicateRow.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.menuItemDuplicateRow.Size = new System.Drawing.Size(319, 22);
            this.menuItemDuplicateRow.Text = "Duplicate selected row";
            this.menuItemDuplicateRow.Click += new System.EventHandler(this.menuItemDuplicateSelectedRow_Click);
            // 
            // menuItemDeleteSelectedRow
            // 
            this.menuItemDeleteSelectedRow.Enabled = false;
            this.menuItemDeleteSelectedRow.Name = "menuItemDeleteSelectedRow";
            this.menuItemDeleteSelectedRow.ShortcutKeyDisplayString = "(Ctrl + D)";
            this.menuItemDeleteSelectedRow.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.menuItemDeleteSelectedRow.Size = new System.Drawing.Size(319, 22);
            this.menuItemDeleteSelectedRow.Text = "Delete selected row";
            this.menuItemDeleteSelectedRow.Click += new System.EventHandler(this.menuOptionDeleteSelectedRow_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(316, 6);
            // 
            // menuItemRetrieveLastDeletedRow
            // 
            this.menuItemRetrieveLastDeletedRow.Name = "menuItemRetrieveLastDeletedRow";
            this.menuItemRetrieveLastDeletedRow.ShortcutKeyDisplayString = "(Ctrl + Shift + Z)";
            this.menuItemRetrieveLastDeletedRow.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Z)));
            this.menuItemRetrieveLastDeletedRow.Size = new System.Drawing.Size(319, 22);
            this.menuItemRetrieveLastDeletedRow.Text = "Retrieve last deleted row";
            this.menuItemRetrieveLastDeletedRow.Click += new System.EventHandler(this.menuItemRetrieveLastDeletedRow_Click);
            // 
            // menuItemCopySelectedRow
            // 
            this.menuItemCopySelectedRow.Enabled = false;
            this.menuItemCopySelectedRow.Name = "menuItemCopySelectedRow";
            this.menuItemCopySelectedRow.ShortcutKeyDisplayString = "(Ctrl + Shift + C)";
            this.menuItemCopySelectedRow.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
            this.menuItemCopySelectedRow.Size = new System.Drawing.Size(319, 22);
            this.menuItemCopySelectedRow.Text = "Copy selected row";
            this.menuItemCopySelectedRow.Click += new System.EventHandler(this.menuItemCopySelectedRowListView_Click);
            // 
            // menuItemPasteLastCopiedRow
            // 
            this.menuItemPasteLastCopiedRow.Name = "menuItemPasteLastCopiedRow";
            this.menuItemPasteLastCopiedRow.ShortcutKeyDisplayString = "(Ctrl + Shift + V)";
            this.menuItemPasteLastCopiedRow.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.V)));
            this.menuItemPasteLastCopiedRow.Size = new System.Drawing.Size(319, 22);
            this.menuItemPasteLastCopiedRow.Text = "Paste last copied row";
            this.menuItemPasteLastCopiedRow.Click += new System.EventHandler(this.menuItemPasteLastCopiedRow_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAbout});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // menuItemAbout
            // 
            this.menuItemAbout.Name = "menuItemAbout";
            this.menuItemAbout.ShortcutKeyDisplayString = "(Alt + F1)";
            this.menuItemAbout.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F1)));
            this.menuItemAbout.Size = new System.Drawing.Size(163, 22);
            this.menuItemAbout.Text = "About";
            this.menuItemAbout.Click += new System.EventHandler(this.menuItemAbout_Click);
            // 
            // otherToolStripMenuItem
            // 
            this.otherToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smartAIWikiToolStripMenuItem});
            this.otherToolStripMenuItem.Name = "otherToolStripMenuItem";
            this.otherToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.otherToolStripMenuItem.Text = "Other";
            // 
            // smartAIWikiToolStripMenuItem
            // 
            this.smartAIWikiToolStripMenuItem.Name = "smartAIWikiToolStripMenuItem";
            this.smartAIWikiToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.smartAIWikiToolStripMenuItem.Text = "SmartAI Wiki...";
            this.smartAIWikiToolStripMenuItem.Click += new System.EventHandler(this.smartAIWikiToolStripMenuItem_Click);
            // 
            // contextMenuStripListView
            // 
            this.contextMenuStripListView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemLoadSelectedEntryListView,
            this.menuItemGenerateCommentListView,
            this.menuItemDuplicateSelectedRowListView,
            this.menuItemDeleteSelectedRowListView,
            this.menuItemCopySelectedRowListView});
            this.contextMenuStripListView.Name = "contextMenuStripListView";
            this.contextMenuStripListView.Size = new System.Drawing.Size(266, 114);
            // 
            // menuItemLoadSelectedEntryListView
            // 
            this.menuItemLoadSelectedEntryListView.Name = "menuItemLoadSelectedEntryListView";
            this.menuItemLoadSelectedEntryListView.ShortcutKeyDisplayString = "(Ctrl + L)";
            this.menuItemLoadSelectedEntryListView.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.menuItemLoadSelectedEntryListView.Size = new System.Drawing.Size(265, 22);
            this.menuItemLoadSelectedEntryListView.Text = "Load selected entry";
            this.menuItemLoadSelectedEntryListView.Click += new System.EventHandler(this.menuItemLoadSelectedEntry_Click);
            // 
            // menuItemGenerateCommentListView
            // 
            this.menuItemGenerateCommentListView.Name = "menuItemGenerateCommentListView";
            this.menuItemGenerateCommentListView.ShortcutKeyDisplayString = "(Ctrl + G)";
            this.menuItemGenerateCommentListView.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.menuItemGenerateCommentListView.Size = new System.Drawing.Size(265, 22);
            this.menuItemGenerateCommentListView.Text = "Generate comment";
            this.menuItemGenerateCommentListView.Click += new System.EventHandler(this.menuItemGenerateCommentListView_Click);
            // 
            // menuItemDuplicateSelectedRowListView
            // 
            this.menuItemDuplicateSelectedRowListView.Name = "menuItemDuplicateSelectedRowListView";
            this.menuItemDuplicateSelectedRowListView.ShortcutKeyDisplayString = "(Ctrl  + Q)";
            this.menuItemDuplicateSelectedRowListView.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.menuItemDuplicateSelectedRowListView.Size = new System.Drawing.Size(265, 22);
            this.menuItemDuplicateSelectedRowListView.Text = "Duplicate selected row";
            this.menuItemDuplicateSelectedRowListView.Click += new System.EventHandler(this.menuItemDuplicateSelectedRow_Click);
            // 
            // menuItemDeleteSelectedRowListView
            // 
            this.menuItemDeleteSelectedRowListView.Name = "menuItemDeleteSelectedRowListView";
            this.menuItemDeleteSelectedRowListView.ShortcutKeyDisplayString = "(Ctrl + D)";
            this.menuItemDeleteSelectedRowListView.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.menuItemDeleteSelectedRowListView.Size = new System.Drawing.Size(265, 22);
            this.menuItemDeleteSelectedRowListView.Text = "Delete selected row";
            this.menuItemDeleteSelectedRowListView.Click += new System.EventHandler(this.testToolStripMenuItemDeleteRow_Click);
            // 
            // menuItemCopySelectedRowListView
            // 
            this.menuItemCopySelectedRowListView.Name = "menuItemCopySelectedRowListView";
            this.menuItemCopySelectedRowListView.ShortcutKeyDisplayString = "(Ctrl + Shift + C)";
            this.menuItemCopySelectedRowListView.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
            this.menuItemCopySelectedRowListView.Size = new System.Drawing.Size(265, 22);
            this.menuItemCopySelectedRowListView.Text = "Copy selected row";
            this.menuItemCopySelectedRowListView.Click += new System.EventHandler(this.menuItemCopySelectedRowListView_Click);
            // 
            // panelLoginBox
            // 
            this.panelLoginBox.Controls.Add(this.groupBoxLogin);
            this.panelLoginBox.Controls.Add(this.labelDontUseDatabaseWarning);
            this.panelLoginBox.Controls.Add(this.buttonConnect);
            this.panelLoginBox.Controls.Add(this.buttonClear);
            this.panelLoginBox.Controls.Add(this.buttonCancel);
            this.panelLoginBox.Location = new System.Drawing.Point(933, 31);
            this.panelLoginBox.Name = "panelLoginBox";
            this.panelLoginBox.Size = new System.Drawing.Size(378, 224);
            this.panelLoginBox.TabIndex = 13;
            // 
            // groupBoxLogin
            // 
            this.groupBoxLogin.Controls.Add(this.radioButtonDontUseDatabase);
            this.groupBoxLogin.Controls.Add(this.radioButtonConnectToMySql);
            this.groupBoxLogin.Controls.Add(this.buttonSearchWorldDb);
            this.groupBoxLogin.Controls.Add(this.checkBoxAutoConnect);
            this.groupBoxLogin.Controls.Add(this.textBoxHost);
            this.groupBoxLogin.Controls.Add(this.textBoxUsername);
            this.groupBoxLogin.Controls.Add(this.labelUser);
            this.groupBoxLogin.Controls.Add(this.textBoxPassword);
            this.groupBoxLogin.Controls.Add(this.label1);
            this.groupBoxLogin.Controls.Add(this.textBoxWorldDatabase);
            this.groupBoxLogin.Controls.Add(this.label2);
            this.groupBoxLogin.Controls.Add(this.textBoxPort);
            this.groupBoxLogin.Controls.Add(this.labelHost);
            this.groupBoxLogin.Controls.Add(this.labelPort);
            this.groupBoxLogin.Location = new System.Drawing.Point(3, 3);
            this.groupBoxLogin.Name = "groupBoxLogin";
            this.groupBoxLogin.Size = new System.Drawing.Size(364, 149);
            this.groupBoxLogin.TabIndex = 10;
            this.groupBoxLogin.TabStop = false;
            this.groupBoxLogin.Text = "Connect information";
            // 
            // radioButtonDontUseDatabase
            // 
            this.radioButtonDontUseDatabase.AutoSize = true;
            this.radioButtonDontUseDatabase.Location = new System.Drawing.Point(13, 47);
            this.radioButtonDontUseDatabase.Name = "radioButtonDontUseDatabase";
            this.radioButtonDontUseDatabase.Size = new System.Drawing.Size(126, 17);
            this.radioButtonDontUseDatabase.TabIndex = 1;
            this.radioButtonDontUseDatabase.Text = "Don\'t use a database";
            this.radioButtonDontUseDatabase.UseVisualStyleBackColor = true;
            this.radioButtonDontUseDatabase.CheckedChanged += new System.EventHandler(this.radioButtonDontUseDatabase_CheckedChanged);
            // 
            // radioButtonConnectToMySql
            // 
            this.radioButtonConnectToMySql.AutoSize = true;
            this.radioButtonConnectToMySql.Checked = true;
            this.radioButtonConnectToMySql.Location = new System.Drawing.Point(13, 21);
            this.radioButtonConnectToMySql.Name = "radioButtonConnectToMySql";
            this.radioButtonConnectToMySql.Size = new System.Drawing.Size(115, 17);
            this.radioButtonConnectToMySql.TabIndex = 0;
            this.radioButtonConnectToMySql.TabStop = true;
            this.radioButtonConnectToMySql.Text = "Connect to MySQL";
            this.radioButtonConnectToMySql.UseVisualStyleBackColor = true;
            this.radioButtonConnectToMySql.CheckedChanged += new System.EventHandler(this.radioButtonConnectToMySql_CheckedChanged);
            // 
            // buttonSearchWorldDb
            // 
            this.buttonSearchWorldDb.Location = new System.Drawing.Point(334, 95);
            this.buttonSearchWorldDb.Name = "buttonSearchWorldDb";
            this.buttonSearchWorldDb.Size = new System.Drawing.Size(24, 22);
            this.buttonSearchWorldDb.TabIndex = 7;
            this.buttonSearchWorldDb.Text = "...";
            this.buttonSearchWorldDb.UseVisualStyleBackColor = true;
            this.buttonSearchWorldDb.Click += new System.EventHandler(this.buttonSearchWorldDb_Click);
            // 
            // checkBoxAutoConnect
            // 
            this.checkBoxAutoConnect.AutoSize = true;
            this.checkBoxAutoConnect.Location = new System.Drawing.Point(13, 124);
            this.checkBoxAutoConnect.Name = "checkBoxAutoConnect";
            this.checkBoxAutoConnect.Size = new System.Drawing.Size(90, 17);
            this.checkBoxAutoConnect.TabIndex = 2;
            this.checkBoxAutoConnect.Text = "Auto connect";
            this.checkBoxAutoConnect.UseVisualStyleBackColor = true;
            // 
            // textBoxHost
            // 
            this.textBoxHost.Location = new System.Drawing.Point(228, 18);
            this.textBoxHost.Name = "textBoxHost";
            this.textBoxHost.Size = new System.Drawing.Size(130, 20);
            this.textBoxHost.TabIndex = 3;
            this.textBoxHost.Text = "position groupbox 9;8";
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Location = new System.Drawing.Point(228, 44);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(130, 20);
            this.textBoxUsername.TabIndex = 4;
            this.textBoxUsername.Text = "height grpbox + 20";
            // 
            // labelUser
            // 
            this.labelUser.AutoSize = true;
            this.labelUser.Location = new System.Drawing.Point(164, 47);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(58, 13);
            this.labelUser.TabIndex = 0;
            this.labelUser.Text = "Username:";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(228, 70);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(130, 20);
            this.textBoxPassword.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(166, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Password:";
            // 
            // textBoxWorldDatabase
            // 
            this.textBoxWorldDatabase.Location = new System.Drawing.Point(228, 96);
            this.textBoxWorldDatabase.Name = "textBoxWorldDatabase";
            this.textBoxWorldDatabase.Size = new System.Drawing.Size(106, 20);
            this.textBoxWorldDatabase.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(135, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "World Database:";
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(228, 122);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(130, 20);
            this.textBoxPort.TabIndex = 8;
            this.textBoxPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericField_KeyPress);
            // 
            // labelHost
            // 
            this.labelHost.AutoSize = true;
            this.labelHost.Location = new System.Drawing.Point(190, 21);
            this.labelHost.Name = "labelHost";
            this.labelHost.Size = new System.Drawing.Size(32, 13);
            this.labelHost.TabIndex = 3;
            this.labelHost.Text = "Host:";
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Location = new System.Drawing.Point(193, 125);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(29, 13);
            this.labelPort.TabIndex = 4;
            this.labelPort.Text = "Port:";
            // 
            // labelDontUseDatabaseWarning
            // 
            this.labelDontUseDatabaseWarning.AutoSize = true;
            this.labelDontUseDatabaseWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDontUseDatabaseWarning.ForeColor = System.Drawing.Color.Red;
            this.labelDontUseDatabaseWarning.Location = new System.Drawing.Point(-1, 184);
            this.labelDontUseDatabaseWarning.Name = "labelDontUseDatabaseWarning";
            this.labelDontUseDatabaseWarning.Size = new System.Drawing.Size(380, 39);
            this.labelDontUseDatabaseWarning.TabIndex = 11;
            this.labelDontUseDatabaseWarning.Text = resources.GetString("labelDontUseDatabaseWarning.Text");
            this.labelDontUseDatabaseWarning.Visible = false;
            // 
            // groupBoxStaticScriptInfo
            // 
            this.groupBoxStaticScriptInfo.Controls.Add(this.pictureBoxCreateScript);
            this.groupBoxStaticScriptInfo.Controls.Add(this.pictureBoxLoadScript);
            this.groupBoxStaticScriptInfo.Controls.Add(this.buttonSearchForEntryOrGuid);
            this.groupBoxStaticScriptInfo.Controls.Add(this.label4);
            this.groupBoxStaticScriptInfo.Controls.Add(this.comboBoxSourceType);
            this.groupBoxStaticScriptInfo.Controls.Add(this.labelEntryOrGuid);
            this.groupBoxStaticScriptInfo.Controls.Add(this.textBoxEntryOrGuid);
            this.groupBoxStaticScriptInfo.Location = new System.Drawing.Point(12, 31);
            this.groupBoxStaticScriptInfo.Name = "groupBoxStaticScriptInfo";
            this.groupBoxStaticScriptInfo.Size = new System.Drawing.Size(290, 75);
            this.groupBoxStaticScriptInfo.TabIndex = 0;
            this.groupBoxStaticScriptInfo.TabStop = false;
            this.groupBoxStaticScriptInfo.Text = "Static script information";
            this.groupBoxStaticScriptInfo.Visible = false;
            // 
            // buttonSearchForEntryOrGuid
            // 
            this.buttonSearchForEntryOrGuid.Location = new System.Drawing.Point(211, 18);
            this.buttonSearchForEntryOrGuid.Name = "buttonSearchForEntryOrGuid";
            this.buttonSearchForEntryOrGuid.Size = new System.Drawing.Size(24, 22);
            this.buttonSearchForEntryOrGuid.TabIndex = 13;
            this.buttonSearchForEntryOrGuid.Text = "...";
            this.buttonSearchForEntryOrGuid.UseVisualStyleBackColor = true;
            this.buttonSearchForEntryOrGuid.Click += new System.EventHandler(this.buttonSearchForEntry_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Source type:";
            // 
            // comboBoxSourceType
            // 
            this.comboBoxSourceType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxSourceType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxSourceType.FormattingEnabled = true;
            this.comboBoxSourceType.Items.AddRange(new object[] {
            "SCRIPT_TYPE_CREATURE",
            "SCRIPT_TYPE_GAMEOBJECT",
            "SCRIPT_TYPE_AREATRIGGER",
            "SCRIPT_TYPE_TIMED_ACTIONLIST"});
            this.comboBoxSourceType.Location = new System.Drawing.Point(101, 45);
            this.comboBoxSourceType.Name = "comboBoxSourceType";
            this.comboBoxSourceType.Size = new System.Drawing.Size(183, 21);
            this.comboBoxSourceType.TabIndex = 14;
            this.comboBoxSourceType.SelectedIndexChanged += new System.EventHandler(this.comboBoxSourceType_SelectedIndexChanged);
            // 
            // labelEntryOrGuid
            // 
            this.labelEntryOrGuid.AutoSize = true;
            this.labelEntryOrGuid.Location = new System.Drawing.Point(15, 22);
            this.labelEntryOrGuid.Name = "labelEntryOrGuid";
            this.labelEntryOrGuid.Size = new System.Drawing.Size(69, 13);
            this.labelEntryOrGuid.TabIndex = 14;
            this.labelEntryOrGuid.Text = "Entry or guid:";
            // 
            // textBoxEntryOrGuid
            // 
            this.textBoxEntryOrGuid.Location = new System.Drawing.Point(101, 19);
            this.textBoxEntryOrGuid.Name = "textBoxEntryOrGuid";
            this.textBoxEntryOrGuid.Size = new System.Drawing.Size(110, 20);
            this.textBoxEntryOrGuid.TabIndex = 12;
            this.textBoxEntryOrGuid.TextChanged += new System.EventHandler(this.textBoxEntryOrGuid_TextChanged);
            this.textBoxEntryOrGuid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericField_KeyPress);
            // 
            // groupBoxPreferences
            // 
            this.groupBoxPreferences.Controls.Add(this.checkBoxAllowChangingEntryAndSourceType);
            this.groupBoxPreferences.Controls.Add(this.checkBoxListActionlistsOrEntries);
            this.groupBoxPreferences.Controls.Add(this.checkBoxShowBasicInfo);
            this.groupBoxPreferences.Controls.Add(this.checkBoxScriptByGuid);
            this.groupBoxPreferences.Controls.Add(this.checkBox3);
            this.groupBoxPreferences.Controls.Add(this.checkBoxLockEventId);
            this.groupBoxPreferences.Location = new System.Drawing.Point(12, 115);
            this.groupBoxPreferences.Name = "groupBoxPreferences";
            this.groupBoxPreferences.Size = new System.Drawing.Size(290, 123);
            this.groupBoxPreferences.TabIndex = 6;
            this.groupBoxPreferences.TabStop = false;
            this.groupBoxPreferences.Text = "Preferences";
            this.groupBoxPreferences.Visible = false;
            // 
            // checkBoxAllowChangingEntryAndSourceType
            // 
            this.checkBoxAllowChangingEntryAndSourceType.AutoSize = true;
            this.checkBoxAllowChangingEntryAndSourceType.Location = new System.Drawing.Point(13, 71);
            this.checkBoxAllowChangingEntryAndSourceType.Name = "checkBoxAllowChangingEntryAndSourceType";
            this.checkBoxAllowChangingEntryAndSourceType.Size = new System.Drawing.Size(229, 17);
            this.checkBoxAllowChangingEntryAndSourceType.TabIndex = 19;
            this.checkBoxAllowChangingEntryAndSourceType.Text = "Allow changing entryorguid and sourcetype";
            this.checkBoxAllowChangingEntryAndSourceType.UseVisualStyleBackColor = true;
            // 
            // checkBoxListActionlistsOrEntries
            // 
            this.checkBoxListActionlistsOrEntries.AutoSize = true;
            this.checkBoxListActionlistsOrEntries.Location = new System.Drawing.Point(13, 47);
            this.checkBoxListActionlistsOrEntries.Name = "checkBoxListActionlistsOrEntries";
            this.checkBoxListActionlistsOrEntries.Size = new System.Drawing.Size(109, 17);
            this.checkBoxListActionlistsOrEntries.TabIndex = 17;
            this.checkBoxListActionlistsOrEntries.Text = "List actionlists too";
            this.checkBoxListActionlistsOrEntries.UseVisualStyleBackColor = true;
            this.checkBoxListActionlistsOrEntries.CheckedChanged += new System.EventHandler(this.checkBoxListActionlists_CheckedChanged);
            // 
            // checkBoxShowBasicInfo
            // 
            this.checkBoxShowBasicInfo.AutoSize = true;
            this.checkBoxShowBasicInfo.Location = new System.Drawing.Point(140, 24);
            this.checkBoxShowBasicInfo.Name = "checkBoxShowBasicInfo";
            this.checkBoxShowBasicInfo.Size = new System.Drawing.Size(135, 17);
            this.checkBoxShowBasicInfo.TabIndex = 16;
            this.checkBoxShowBasicInfo.Text = "Show basic information";
            this.checkBoxShowBasicInfo.UseVisualStyleBackColor = true;
            this.checkBoxShowBasicInfo.CheckedChanged += new System.EventHandler(this.checkBoxShowBasicInfo_CheckedChanged);
            // 
            // checkBoxScriptByGuid
            // 
            this.checkBoxScriptByGuid.AutoSize = true;
            this.checkBoxScriptByGuid.Location = new System.Drawing.Point(140, 47);
            this.checkBoxScriptByGuid.Name = "checkBoxScriptByGuid";
            this.checkBoxScriptByGuid.Size = new System.Drawing.Size(97, 17);
            this.checkBoxScriptByGuid.TabIndex = 18;
            this.checkBoxScriptByGuid.Text = "Script by GUID";
            this.checkBoxScriptByGuid.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(13, 94);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(241, 17);
            this.checkBox3.TabIndex = 20;
            this.checkBox3.Text = "Show events and actions for source type only";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBoxLockEventId
            // 
            this.checkBoxLockEventId.AutoSize = true;
            this.checkBoxLockEventId.Checked = true;
            this.checkBoxLockEventId.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLockEventId.Location = new System.Drawing.Point(13, 24);
            this.checkBoxLockEventId.Name = "checkBoxLockEventId";
            this.checkBoxLockEventId.Size = new System.Drawing.Size(125, 17);
            this.checkBoxLockEventId.TabIndex = 15;
            this.checkBoxLockEventId.Text = "Lock smart_scripts.id";
            this.checkBoxLockEventId.UseVisualStyleBackColor = true;
            this.checkBoxLockEventId.CheckedChanged += new System.EventHandler(this.checkBoxLockEventId_CheckedChanged);
            // 
            // groupBoxScriptInfo
            // 
            this.groupBoxScriptInfo.Controls.Add(this.buttonLinkTo);
            this.groupBoxScriptInfo.Controls.Add(this.buttonLinkFrom);
            this.groupBoxScriptInfo.Controls.Add(this.comboBoxTargetType);
            this.groupBoxScriptInfo.Controls.Add(this.buttonSelectEventFlag);
            this.groupBoxScriptInfo.Controls.Add(this.buttonSearchPhasemask);
            this.groupBoxScriptInfo.Controls.Add(this.comboBoxActionType);
            this.groupBoxScriptInfo.Controls.Add(this.buttonSearchEventFlags);
            this.groupBoxScriptInfo.Controls.Add(this.textBoxTargetType);
            this.groupBoxScriptInfo.Controls.Add(this.textBoxEventChance);
            this.groupBoxScriptInfo.Controls.Add(this.label14);
            this.groupBoxScriptInfo.Controls.Add(this.textBoxEventFlags);
            this.groupBoxScriptInfo.Controls.Add(this.label13);
            this.groupBoxScriptInfo.Controls.Add(this.label3);
            this.groupBoxScriptInfo.Controls.Add(this.textBoxComments);
            this.groupBoxScriptInfo.Controls.Add(this.textBoxActionType);
            this.groupBoxScriptInfo.Controls.Add(this.textBoxEventPhasemask);
            this.groupBoxScriptInfo.Controls.Add(this.textBoxEventType);
            this.groupBoxScriptInfo.Controls.Add(this.textBoxLinkTo);
            this.groupBoxScriptInfo.Controls.Add(this.textBoxLinkFrom);
            this.groupBoxScriptInfo.Controls.Add(this.comboBoxEventType);
            this.groupBoxScriptInfo.Controls.Add(this.textBoxId);
            this.groupBoxScriptInfo.Controls.Add(this.label8);
            this.groupBoxScriptInfo.Controls.Add(this.label15);
            this.groupBoxScriptInfo.Controls.Add(this.label5);
            this.groupBoxScriptInfo.Controls.Add(this.label9);
            this.groupBoxScriptInfo.Controls.Add(this.label6);
            this.groupBoxScriptInfo.Controls.Add(this.label11);
            this.groupBoxScriptInfo.Controls.Add(this.label10);
            this.groupBoxScriptInfo.Location = new System.Drawing.Point(308, 31);
            this.groupBoxScriptInfo.Name = "groupBoxScriptInfo";
            this.groupBoxScriptInfo.Size = new System.Drawing.Size(335, 207);
            this.groupBoxScriptInfo.TabIndex = 6;
            this.groupBoxScriptInfo.TabStop = false;
            this.groupBoxScriptInfo.Text = "Dynamic script information";
            this.groupBoxScriptInfo.Visible = false;
            // 
            // buttonLinkTo
            // 
            this.buttonLinkTo.Location = new System.Drawing.Point(100, 150);
            this.buttonLinkTo.Name = "buttonLinkTo";
            this.buttonLinkTo.Size = new System.Drawing.Size(24, 22);
            this.buttonLinkTo.TabIndex = 34;
            this.buttonLinkTo.Text = "...";
            this.buttonLinkTo.UseVisualStyleBackColor = true;
            this.buttonLinkTo.Click += new System.EventHandler(this.buttonLinkTo_Click);
            // 
            // buttonLinkFrom
            // 
            this.buttonLinkFrom.Location = new System.Drawing.Point(302, 150);
            this.buttonLinkFrom.Name = "buttonLinkFrom";
            this.buttonLinkFrom.Size = new System.Drawing.Size(24, 22);
            this.buttonLinkFrom.TabIndex = 36;
            this.buttonLinkFrom.Text = "...";
            this.buttonLinkFrom.UseVisualStyleBackColor = true;
            this.buttonLinkFrom.Click += new System.EventHandler(this.buttonLinkFrom_Click);
            // 
            // comboBoxTargetType
            // 
            this.comboBoxTargetType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxTargetType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxTargetType.FormattingEnabled = true;
            this.comboBoxTargetType.Items.AddRange(new object[] {
            "TARGET_NONE",
            "TARGET_SELF",
            "TARGET_VICTIM",
            "TARGET_HOSTILE_SECOND_AGGRO",
            "TARGET_HOSTILE_LAST_AGGRO",
            "TARGET_HOSTILE_RANDOM",
            "TARGET_HOSTILE_RANDOM_NOT_TOP",
            "TARGET_ACTION_INVOKER",
            "TARGET_POSITION",
            "TARGET_CREATURE_RANGE",
            "TARGET_CREATURE_GUID",
            "TARGET_CREATURE_DISTANCE",
            "TARGET_STORED",
            "TARGET_GAMEOBJECT_RANGE",
            "TARGET_GAMEOBJECT_GUID",
            "TARGET_GAMEOBJECT_DISTANCE",
            "TARGET_INVOKER_PARTY",
            "TARGET_PLAYER_RANGE",
            "TARGET_PLAYER_DISTANCE",
            "TARGET_CLOSEST_CREATURE",
            "TARGET_CLOSEST_GAMEOBJECT",
            "TARGET_CLOSEST_PLAYER",
            "TARGET_ACTION_INVOKER_VEHICLE",
            "TARGET_OWNER_OR_SUMMONER",
            "TARGET_THREAT_LIST",
            "TARGET_CLOSEST_ENEMY",
            "TARGET_CLOSEST_FRIENDLY"});
            this.comboBoxTargetType.Location = new System.Drawing.Point(59, 72);
            this.comboBoxTargetType.Name = "comboBoxTargetType";
            this.comboBoxTargetType.Size = new System.Drawing.Size(235, 21);
            this.comboBoxTargetType.TabIndex = 25;
            this.comboBoxTargetType.SelectedIndexChanged += new System.EventHandler(this.comboBoxTargetType_SelectedIndexChanged);
            this.comboBoxTargetType.MouseEnter += new System.EventHandler(this.comboBoxTargetType_MouseEnter);
            // 
            // buttonSelectEventFlag
            // 
            this.buttonSelectEventFlag.Location = new System.Drawing.Point(302, 125);
            this.buttonSelectEventFlag.Name = "buttonSelectEventFlag";
            this.buttonSelectEventFlag.Size = new System.Drawing.Size(24, 22);
            this.buttonSelectEventFlag.TabIndex = 32;
            this.buttonSelectEventFlag.Text = "...";
            this.buttonSelectEventFlag.UseVisualStyleBackColor = true;
            this.buttonSelectEventFlag.Click += new System.EventHandler(this.buttonSelectEventFlag_Click);
            // 
            // buttonSearchPhasemask
            // 
            this.buttonSearchPhasemask.Location = new System.Drawing.Point(302, 98);
            this.buttonSearchPhasemask.Name = "buttonSearchPhasemask";
            this.buttonSearchPhasemask.Size = new System.Drawing.Size(24, 22);
            this.buttonSearchPhasemask.TabIndex = 29;
            this.buttonSearchPhasemask.Text = "...";
            this.buttonSearchPhasemask.UseVisualStyleBackColor = true;
            this.buttonSearchPhasemask.Click += new System.EventHandler(this.buttonSearchPhasemask_Click);
            // 
            // comboBoxActionType
            // 
            this.comboBoxActionType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxActionType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxActionType.FormattingEnabled = true;
            this.comboBoxActionType.Items.AddRange(new object[] {
            "ACTION_NONE",
            "ACTION_TALK",
            "ACTION_SET_FACTION",
            "ACTION_MORPH_TO_ENTRY_OR_MODEL",
            "ACTION_SOUND",
            "ACTION_PLAY_EMOTE",
            "ACTION_FAIL_QUEST",
            "ACTION_ADD_QUEST",
            "ACTION_SET_REACT_STATE",
            "ACTION_ACTIVATE_GOBJECT",
            "ACTION_RANDOM_EMOTE",
            "ACTION_CAST",
            "ACTION_SUMMON_CREATURE",
            "ACTION_THREAT_SINGLE_PCT",
            "ACTION_THREAT_ALL_PCT",
            "ACTION_CALL_AREAEXPLOREDOREVENTHAPPENS",
            "ACTION_UNUSED_16",
            "ACTION_SET_EMOTE_STATE",
            "ACTION_SET_UNIT_FLAG",
            "ACTION_REMOVE_UNIT_FLAG",
            "ACTION_AUTO_ATTACK",
            "ACTION_ALLOW_COMBAT_MOVEMENT",
            "ACTION_SET_EVENT_PHASE",
            "ACTION_INC_EVENT_PHASE",
            "ACTION_EVADE",
            "ACTION_FLEE_FOR_ASSIST",
            "ACTION_CALL_GROUPEVENTHAPPENS",
            "ACTION_CALL_CASTEDCREATUREORGO",
            "ACTION_REMOVEAURASFROMSPELL",
            "ACTION_FOLLOW",
            "ACTION_RANDOM_PHASE",
            "ACTION_RANDOM_PHASE_RANGE",
            "ACTION_RESET_GOBJECT",
            "ACTION_CALL_KILLEDMONSTER",
            "ACTION_SET_INST_DATA",
            "ACTION_SET_INST_DATA64",
            "ACTION_UPDATE_TEMPLATE",
            "ACTION_DIE",
            "ACTION_SET_IN_COMBAT_WITH_ZONE",
            "ACTION_CALL_FOR_HELP",
            "ACTION_SET_SHEATH",
            "ACTION_FORCE_DESPAWN",
            "ACTION_SET_INVINCIBILITY_HP_LEVEL",
            "ACTION_MOUNT_TO_ENTRY_OR_MODEL",
            "ACTION_SET_INGAME_PHASE_MASK",
            "ACTION_SET_DATA",
            "ACTION_MOVE_FORWARD",
            "ACTION_SET_VISIBILITY",
            "ACTION_SET_ACTIVE",
            "ACTION_ATTACK_START",
            "ACTION_SUMMON_GO",
            "ACTION_KILL_UNIT",
            "ACTION_ACTIVATE_TAXI",
            "ACTION_WP_START",
            "ACTION_WP_PAUSE",
            "ACTION_WP_STOP",
            "ACTION_ADD_ITEM",
            "ACTION_REMOVE_ITEM",
            "ACTION_INSTALL_AI_TEMPLATE",
            "ACTION_SET_RUN",
            "ACTION_SET_FLY",
            "ACTION_SET_SWIM",
            "ACTION_TELEPORT",
            "ACTION_STORE_VARIABLE_DECIMAL",
            "ACTION_STORE_TARGET_LIST",
            "ACTION_WP_RESUME",
            "ACTION_SET_ORIENTATION",
            "ACTION_CREATE_TIMED_EVENT",
            "ACTION_PLAYMOVIE",
            "ACTION_MOVE_TO_POS",
            "ACTION_RESPAWN_TARGET",
            "ACTION_EQUIP",
            "ACTION_CLOSE_GOSSIP",
            "ACTION_TRIGGER_TIMED_EVENT",
            "ACTION_REMOVE_TIMED_EVENT",
            "ACTION_ADD_AURA",
            "ACTION_OVERRIDE_SCRIPT_BASE_OBJECT",
            "ACTION_RESET_SCRIPT_BASE_OBJECT",
            "ACTION_CALL_SCRIPT_RESET",
            "ACTION_SET_RANGED_MOVEMENT",
            "ACTION_CALL_TIMED_ACTIONLIST",
            "ACTION_SET_NPC_FLAG",
            "ACTION_ADD_NPC_FLAG",
            "ACTION_REMOVE_NPC_FLAG",
            "ACTION_SIMPLE_TALK",
            "ACTION_INVOKER_CAST",
            "ACTION_CROSS_CAST",
            "ACTION_CALL_RANDOM_TIMED_ACTIONLIST",
            "ACTION_CALL_RANDOM_RANGE_TIMED_ACTIONLIST",
            "ACTION_RANDOM_MOVE",
            "ACTION_SET_UNIT_FIELD_BYTES_1",
            "ACTION_REMOVE_UNIT_FIELD_BYTES_1",
            "ACTION_INTERRUPT_SPELL",
            "ACTION_SEND_GO_CUSTOM_ANIM",
            "ACTION_SET_DYNAMIC_FLAG",
            "ACTION_ADD_DYNAMIC_FLAG",
            "ACTION_REMOVE_DYNAMIC_FLAG",
            "ACTION_JUMP_TO_POS",
            "ACTION_SEND_GOSSIP_MENU",
            "ACTION_GO_SET_LOOT_STATE",
            "ACTION_SEND_TARGET_TO_TARGET",
            "ACTION_SET_HOME_POS",
            "ACTION_SET_HEALTH_REGEN",
            "ACTION_SET_ROOT",
            "ACTION_SET_GO_FLAG",
            "ACTION_ADD_GO_FLAG",
            "ACTION_REMOVE_GO_FLAG",
            "ACTION_SUMMON_CREATURE_GROUP",
            "ACTION_SET_POWER",
            "ACTION_ADD_POWER",
            "ACTION_REMOVE_POWER"});
            this.comboBoxActionType.Location = new System.Drawing.Point(59, 45);
            this.comboBoxActionType.Name = "comboBoxActionType";
            this.comboBoxActionType.Size = new System.Drawing.Size(235, 21);
            this.comboBoxActionType.TabIndex = 23;
            this.comboBoxActionType.SelectedIndexChanged += new System.EventHandler(this.comboBoxActionType_SelectedIndexChanged);
            this.comboBoxActionType.MouseEnter += new System.EventHandler(this.comboBoxActionType_MouseEnter);
            // 
            // buttonSearchEventFlags
            // 
            this.buttonSearchEventFlags.Location = new System.Drawing.Point(361, 149);
            this.buttonSearchEventFlags.Name = "buttonSearchEventFlags";
            this.buttonSearchEventFlags.Size = new System.Drawing.Size(24, 22);
            this.buttonSearchEventFlags.TabIndex = 28;
            this.buttonSearchEventFlags.Text = "...";
            this.buttonSearchEventFlags.UseVisualStyleBackColor = true;
            // 
            // textBoxTargetType
            // 
            this.textBoxTargetType.Location = new System.Drawing.Point(299, 73);
            this.textBoxTargetType.Name = "textBoxTargetType";
            this.textBoxTargetType.Size = new System.Drawing.Size(26, 20);
            this.textBoxTargetType.TabIndex = 26;
            this.textBoxTargetType.Text = "0";
            this.textBoxTargetType.TextChanged += new System.EventHandler(this.textBoxTargetTypeId_TextChanged);
            this.textBoxTargetType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericField_KeyPress);
            // 
            // textBoxEventChance
            // 
            this.textBoxEventChance.Location = new System.Drawing.Point(59, 126);
            this.textBoxEventChance.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.textBoxEventChance.Name = "textBoxEventChance";
            this.textBoxEventChance.Size = new System.Drawing.Size(64, 20);
            this.textBoxEventChance.TabIndex = 30;
            this.textBoxEventChance.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.textBoxEventChance.ValueChanged += new System.EventHandler(this.textBoxEventChance_ValueChanged);
            this.textBoxEventChance.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericField_KeyPress);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(15, 75);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(41, 13);
            this.label14.TabIndex = 22;
            this.label14.Text = "Target:";
            // 
            // textBoxEventFlags
            // 
            this.textBoxEventFlags.Location = new System.Drawing.Point(263, 126);
            this.textBoxEventFlags.Name = "textBoxEventFlags";
            this.textBoxEventFlags.Size = new System.Drawing.Size(39, 20);
            this.textBoxEventFlags.TabIndex = 31;
            this.textBoxEventFlags.Text = "0";
            this.textBoxEventFlags.TextChanged += new System.EventHandler(this.textBoxEventFlags_TextChanged);
            this.textBoxEventFlags.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericField_KeyPress);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(2, 180);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(54, 13);
            this.label13.TabIndex = 21;
            this.label13.Text = "Comment:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(225, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 27;
            this.label3.Text = "Flags:";
            // 
            // textBoxComments
            // 
            this.textBoxComments.Location = new System.Drawing.Point(59, 177);
            this.textBoxComments.Name = "textBoxComments";
            this.textBoxComments.Size = new System.Drawing.Size(267, 20);
            this.textBoxComments.TabIndex = 37;
            this.textBoxComments.Text = "Npc - Event - Action (phase) (dungeon difficulty)";
            this.textBoxComments.TextChanged += new System.EventHandler(this.textBoxComments_TextChanged);
            // 
            // textBoxActionType
            // 
            this.textBoxActionType.Location = new System.Drawing.Point(299, 46);
            this.textBoxActionType.Name = "textBoxActionType";
            this.textBoxActionType.Size = new System.Drawing.Size(26, 20);
            this.textBoxActionType.TabIndex = 24;
            this.textBoxActionType.Text = "0";
            this.textBoxActionType.TextChanged += new System.EventHandler(this.textBoxActionTypeId_TextChanged);
            this.textBoxActionType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericField_KeyPress);
            // 
            // textBoxEventPhasemask
            // 
            this.textBoxEventPhasemask.Location = new System.Drawing.Point(263, 99);
            this.textBoxEventPhasemask.Name = "textBoxEventPhasemask";
            this.textBoxEventPhasemask.Size = new System.Drawing.Size(39, 20);
            this.textBoxEventPhasemask.TabIndex = 28;
            this.textBoxEventPhasemask.Text = "0";
            this.textBoxEventPhasemask.TextChanged += new System.EventHandler(this.textBoxEventPhasemask_TextChanged);
            this.textBoxEventPhasemask.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericField_KeyPress);
            // 
            // textBoxEventType
            // 
            this.textBoxEventType.Location = new System.Drawing.Point(299, 18);
            this.textBoxEventType.Name = "textBoxEventType";
            this.textBoxEventType.Size = new System.Drawing.Size(26, 20);
            this.textBoxEventType.TabIndex = 22;
            this.textBoxEventType.Text = "0";
            this.textBoxEventType.TextChanged += new System.EventHandler(this.textBoxEventTypeId_TextChanged);
            this.textBoxEventType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericField_KeyPress);
            // 
            // textBoxLinkTo
            // 
            this.textBoxLinkTo.Location = new System.Drawing.Point(59, 151);
            this.textBoxLinkTo.Name = "textBoxLinkTo";
            this.textBoxLinkTo.Size = new System.Drawing.Size(41, 20);
            this.textBoxLinkTo.TabIndex = 33;
            this.textBoxLinkTo.Text = "0";
            this.textBoxLinkTo.TextChanged += new System.EventHandler(this.textBoxLinkTo_TextChanged);
            this.textBoxLinkTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericField_KeyPress);
            // 
            // textBoxLinkFrom
            // 
            this.textBoxLinkFrom.Location = new System.Drawing.Point(263, 151);
            this.textBoxLinkFrom.Name = "textBoxLinkFrom";
            this.textBoxLinkFrom.Size = new System.Drawing.Size(39, 20);
            this.textBoxLinkFrom.TabIndex = 35;
            this.textBoxLinkFrom.Text = "None";
            this.textBoxLinkFrom.TextChanged += new System.EventHandler(this.textBoxLinkFrom_TextChanged);
            this.textBoxLinkFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericField_KeyPress);
            // 
            // comboBoxEventType
            // 
            this.comboBoxEventType.AutoCompleteCustomSource.AddRange(new string[] {
            "SMART_EVENT_UPDATE_IC",
            "SMART_EVENT_UPDATE_OOC",
            "SMART_EVENT_HEALT_PCT",
            "SMART_EVENT_MANA_PCT",
            "SMART_EVENT_AGGRO",
            "SMART_EVENT_KILL",
            "SMART_EVENT_DEATH",
            "SMART_EVENT_EVADE",
            "SMART_EVENT_SPELLHIT",
            "SMART_EVENT_RANGE",
            "SMART_EVENT_OOC_LOS",
            "SMART_EVENT_RESPAWN",
            "SMART_EVENT_TARGET_HEALTH_PCT",
            "SMART_EVENT_VICTIM_CASTING",
            "SMART_EVENT_FRIENDLY_HEALTH",
            "SMART_EVENT_FRIENDLY_IS_CC",
            "SMART_EVENT_FRIENDLY_MISSING_BUFF",
            "SMART_EVENT_SUMMONED_UNIT",
            "SMART_EVENT_TARGET_MANA_PCT",
            "SMART_EVENT_ACCEPTED_QUEST",
            "SMART_EVENT_REWARD_QUEST",
            "SMART_EVENT_REACHED_HOME",
            "SMART_EVENT_RECEIVE_EMOTE",
            "SMART_EVENT_HAS_AURA",
            "SMART_EVENT_TARGET_BUFFED",
            "SMART_EVENT_RESET",
            "SMART_EVENT_IC_LOS",
            "SMART_EVENT_PASSENGER_BOARDED",
            "SMART_EVENT_PASSENGER_REMOVED",
            "SMART_EVENT_CHARMED",
            "SMART_EVENT_CHARMED_TARGET",
            "SMART_EVENT_SPELLHIT_TARGET",
            "SMART_EVENT_DAMAGED",
            "SMART_EVENT_DAMAGED_TARGET",
            "SMART_EVENT_MOVEMENTINFORM",
            "SMART_EVENT_SUMMON_DESPAWNED",
            "SMART_EVENT_CORPSE_REMOVED",
            "SMART_EVENT_AI_INIT",
            "SMART_EVENT_DATA_SET",
            "SMART_EVENT_WAYPOINT_START",
            "SMART_EVENT_WAYPOINT_REACHED",
            "SMART_EVENT_TRANSPORT_ADDPLAYER",
            "SMART_EVENT_TRANSPORT_ADDCREATURE",
            "SMART_EVENT_TRANSPORT_REMOVE_PLAYER",
            "SMART_EVENT_TRANSPORT_RELOCATE",
            "SMART_EVENT_INSTANCE_PLAYER_ENTER",
            "SMART_EVENT_AREATRIGGER_ONTRIGGER",
            "SMART_EVENT_QUEST_ACCEPTED",
            "SMART_EVENT_QUEST_OBJ_COPLETETION",
            "SMART_EVENT_QUEST_COMPLETION",
            "SMART_EVENT_QUEST_REWARDED",
            "SMART_EVENT_QUEST_FAIL",
            "SMART_EVENT_TEXT_OVER",
            "SMART_EVENT_RECEIVE_HEAL",
            "SMART_EVENT_JUST_SUMMONED",
            "SMART_EVENT_WAYPOINT_PAUSED",
            "SMART_EVENT_WAYPOINT_RESUMED",
            "SMART_EVENT_WAYPOINT_STOPPED",
            "SMART_EVENT_WAYPOINT_ENDED",
            "SMART_EVENT_TIMED_EVENT_TRIGGERED",
            "SMART_EVENT_UPDATE",
            "SMART_EVENT_LINK",
            "SMART_EVENT_GOSSIP_SELECT",
            "SMART_EVENT_JUST_CREATED",
            "SMART_EVENT_GOSSIP_HELLO",
            "SMART_EVENT_FOLLOW_COMPLETED",
            "SMART_EVENT_DUMMY_EFFECT ",
            "SMART_EVENT_IS_BEHIND_TARGET",
            "SMART_EVENT_GAME_EVENT_START",
            "SMART_EVENT_GAME_EVENT_END",
            "SMART_EVENT_GO_STATE_CHANGED"});
            this.comboBoxEventType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxEventType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxEventType.FormattingEnabled = true;
            this.comboBoxEventType.Items.AddRange(new object[] {
            "EVENT_UPDATE_IC",
            "EVENT_UPDATE_OOC",
            "EVENT_HEALT_PCT",
            "EVENT_MANA_PCT",
            "EVENT_AGGRO",
            "EVENT_KILL",
            "EVENT_DEATH",
            "EVENT_EVADE",
            "EVENT_SPELLHIT",
            "EVENT_RANGE",
            "EVENT_OOC_LOS",
            "EVENT_RESPAWN",
            "EVENT_TARGET_HEALTH_PCT",
            "EVENT_VICTIM_CASTING",
            "EVENT_FRIENDLY_HEALTH",
            "EVENT_FRIENDLY_IS_CC",
            "EVENT_FRIENDLY_MISSING_BUFF",
            "EVENT_SUMMONED_UNIT",
            "EVENT_TARGET_MANA_PCT",
            "EVENT_ACCEPTED_QUEST",
            "EVENT_REWARD_QUEST",
            "EVENT_REACHED_HOME",
            "EVENT_RECEIVE_EMOTE",
            "EVENT_HAS_AURA",
            "EVENT_TARGET_BUFFED",
            "EVENT_RESET",
            "EVENT_IC_LOS",
            "EVENT_PASSENGER_BOARDED",
            "EVENT_PASSENGER_REMOVED",
            "EVENT_CHARMED",
            "EVENT_CHARMED_TARGET",
            "EVENT_SPELLHIT_TARGET",
            "EVENT_DAMAGED",
            "EVENT_DAMAGED_TARGET",
            "EVENT_MOVEMENTINFORM",
            "EVENT_SUMMON_DESPAWNED",
            "EVENT_CORPSE_REMOVED",
            "EVENT_AI_INIT",
            "EVENT_DATA_SET",
            "EVENT_WAYPOINT_START",
            "EVENT_WAYPOINT_REACHED",
            "EVENT_TRANSPORT_ADDPLAYER",
            "EVENT_TRANSPORT_ADDCREATURE",
            "EVENT_TRANSPORT_REMOVE_PLAYER",
            "EVENT_TRANSPORT_RELOCATE",
            "EVENT_INSTANCE_PLAYER_ENTER",
            "EVENT_AREATRIGGER_ONTRIGGER",
            "EVENT_QUEST_ACCEPTED",
            "EVENT_QUEST_OBJ_COPLETETION",
            "EVENT_QUEST_COMPLETION",
            "EVENT_QUEST_REWARDED",
            "EVENT_QUEST_FAIL",
            "EVENT_TEXT_OVER",
            "EVENT_RECEIVE_HEAL",
            "EVENT_JUST_SUMMONED",
            "EVENT_WAYPOINT_PAUSED",
            "EVENT_WAYPOINT_RESUMED",
            "EVENT_WAYPOINT_STOPPED",
            "EVENT_WAYPOINT_ENDED",
            "EVENT_TIMED_EVENT_TRIGGERED",
            "EVENT_UPDATE",
            "EVENT_LINK",
            "EVENT_GOSSIP_SELECT",
            "EVENT_JUST_CREATED",
            "EVENT_GOSSIP_HELLO",
            "EVENT_FOLLOW_COMPLETED",
            "EVENT_DUMMY_EFFECT ",
            "EVENT_IS_BEHIND_TARGET",
            "EVENT_GAME_EVENT_START",
            "EVENT_GAME_EVENT_END",
            "EVENT_GO_STATE_CHANGED",
            "EVENT_GO_EVENT_INFORM",
            "EVENT_ACTION_DONE",
            "EVENT_ON_SPELLCLICK",
            "EVENT_FRIENDLY_HEALTH_PCT"});
            this.comboBoxEventType.Location = new System.Drawing.Point(59, 18);
            this.comboBoxEventType.Name = "comboBoxEventType";
            this.comboBoxEventType.Size = new System.Drawing.Size(235, 21);
            this.comboBoxEventType.TabIndex = 21;
            this.comboBoxEventType.SelectedIndexChanged += new System.EventHandler(this.comboBoxEventType_SelectedIndexChanged);
            this.comboBoxEventType.MouseEnter += new System.EventHandler(this.comboBoxEventType_MouseEnter);
            // 
            // textBoxId
            // 
            this.textBoxId.Enabled = false;
            this.textBoxId.Location = new System.Drawing.Point(59, 99);
            this.textBoxId.Name = "textBoxId";
            this.textBoxId.Size = new System.Drawing.Size(41, 20);
            this.textBoxId.TabIndex = 27;
            this.textBoxId.Text = "0";
            this.textBoxId.TextChanged += new System.EventHandler(this.textBoxId_TextChanged);
            this.textBoxId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericField_KeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 128);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "Chance:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(14, 154);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(42, 13);
            this.label15.TabIndex = 24;
            this.label15.Text = "Link to:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Event:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(207, 154);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 13);
            this.label9.TabIndex = 24;
            this.label9.Text = "Link from:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Action:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(37, 102);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(19, 13);
            this.label11.TabIndex = 23;
            this.label11.Text = "Id:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(195, 102);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 13);
            this.label10.TabIndex = 25;
            this.label10.Text = "Phasemask:";
            // 
            // labelEventParam1
            // 
            this.labelEventParam1.AutoSize = true;
            this.labelEventParam1.Location = new System.Drawing.Point(1, 7);
            this.labelEventParam1.Name = "labelEventParam1";
            this.labelEventParam1.Size = new System.Drawing.Size(46, 13);
            this.labelEventParam1.TabIndex = 23;
            this.labelEventParam1.Text = "Param 1";
            this.labelEventParam1.MouseEnter += new System.EventHandler(this.labelEventParam1_MouseEnter);
            // 
            // textBoxEventParam1
            // 
            this.textBoxEventParam1.Location = new System.Drawing.Point(144, 4);
            this.textBoxEventParam1.Name = "textBoxEventParam1";
            this.textBoxEventParam1.Size = new System.Drawing.Size(70, 20);
            this.textBoxEventParam1.TabIndex = 37;
            this.textBoxEventParam1.Text = "0";
            this.textBoxEventParam1.TextChanged += new System.EventHandler(this.textBoxEventParam1_TextChanged);
            this.textBoxEventParam1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericField_KeyPress);
            // 
            // tabControlParameters
            // 
            this.tabControlParameters.Controls.Add(this.tabPageEvent);
            this.tabControlParameters.Controls.Add(this.tabPageAction);
            this.tabControlParameters.Controls.Add(this.tabPageTarget);
            this.tabControlParameters.Location = new System.Drawing.Point(8, 19);
            this.tabControlParameters.Name = "tabControlParameters";
            this.tabControlParameters.SelectedIndex = 0;
            this.tabControlParameters.Size = new System.Drawing.Size(264, 153);
            this.tabControlParameters.TabIndex = 0;
            // 
            // tabPageEvent
            // 
            this.tabPageEvent.BackColor = System.Drawing.SystemColors.Window;
            this.tabPageEvent.Controls.Add(this.buttonEventParamFourSearch);
            this.tabPageEvent.Controls.Add(this.buttonEventParamThreeSearch);
            this.tabPageEvent.Controls.Add(this.buttonEventParamTwoSearch);
            this.tabPageEvent.Controls.Add(this.buttonEventParamOneSearch);
            this.tabPageEvent.Controls.Add(this.labelEventParam4);
            this.tabPageEvent.Controls.Add(this.labelEventParam3);
            this.tabPageEvent.Controls.Add(this.labelEventParam2);
            this.tabPageEvent.Controls.Add(this.textBoxEventParam4);
            this.tabPageEvent.Controls.Add(this.textBoxEventParam3);
            this.tabPageEvent.Controls.Add(this.textBoxEventParam2);
            this.tabPageEvent.Controls.Add(this.labelEventParam1);
            this.tabPageEvent.Controls.Add(this.textBoxEventParam1);
            this.tabPageEvent.Location = new System.Drawing.Point(4, 22);
            this.tabPageEvent.Name = "tabPageEvent";
            this.tabPageEvent.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEvent.Size = new System.Drawing.Size(256, 127);
            this.tabPageEvent.TabIndex = 0;
            this.tabPageEvent.Text = "Event";
            // 
            // buttonEventParamFourSearch
            // 
            this.buttonEventParamFourSearch.Location = new System.Drawing.Point(214, 81);
            this.buttonEventParamFourSearch.Name = "buttonEventParamFourSearch";
            this.buttonEventParamFourSearch.Size = new System.Drawing.Size(24, 22);
            this.buttonEventParamFourSearch.TabIndex = 44;
            this.buttonEventParamFourSearch.Text = "...";
            this.buttonEventParamFourSearch.UseVisualStyleBackColor = true;
            this.buttonEventParamFourSearch.Visible = false;
            this.buttonEventParamFourSearch.Click += new System.EventHandler(this.buttonEventParamFourSearch_Click);
            // 
            // buttonEventParamThreeSearch
            // 
            this.buttonEventParamThreeSearch.Location = new System.Drawing.Point(214, 55);
            this.buttonEventParamThreeSearch.Name = "buttonEventParamThreeSearch";
            this.buttonEventParamThreeSearch.Size = new System.Drawing.Size(24, 22);
            this.buttonEventParamThreeSearch.TabIndex = 42;
            this.buttonEventParamThreeSearch.Text = "...";
            this.buttonEventParamThreeSearch.UseVisualStyleBackColor = true;
            this.buttonEventParamThreeSearch.Visible = false;
            this.buttonEventParamThreeSearch.Click += new System.EventHandler(this.buttonEventParamThreeSearch_Click);
            // 
            // buttonEventParamTwoSearch
            // 
            this.buttonEventParamTwoSearch.Location = new System.Drawing.Point(214, 29);
            this.buttonEventParamTwoSearch.Name = "buttonEventParamTwoSearch";
            this.buttonEventParamTwoSearch.Size = new System.Drawing.Size(24, 22);
            this.buttonEventParamTwoSearch.TabIndex = 40;
            this.buttonEventParamTwoSearch.Text = "...";
            this.buttonEventParamTwoSearch.UseVisualStyleBackColor = true;
            this.buttonEventParamTwoSearch.Visible = false;
            this.buttonEventParamTwoSearch.Click += new System.EventHandler(this.buttonEventParamTwoSearch_Click);
            // 
            // buttonEventParamOneSearch
            // 
            this.buttonEventParamOneSearch.Location = new System.Drawing.Point(214, 3);
            this.buttonEventParamOneSearch.Name = "buttonEventParamOneSearch";
            this.buttonEventParamOneSearch.Size = new System.Drawing.Size(24, 22);
            this.buttonEventParamOneSearch.TabIndex = 38;
            this.buttonEventParamOneSearch.Text = "...";
            this.buttonEventParamOneSearch.UseVisualStyleBackColor = true;
            this.buttonEventParamOneSearch.Visible = false;
            this.buttonEventParamOneSearch.Click += new System.EventHandler(this.buttonEventParamOneSearch_Click);
            // 
            // labelEventParam4
            // 
            this.labelEventParam4.AutoSize = true;
            this.labelEventParam4.Location = new System.Drawing.Point(1, 85);
            this.labelEventParam4.Name = "labelEventParam4";
            this.labelEventParam4.Size = new System.Drawing.Size(46, 13);
            this.labelEventParam4.TabIndex = 23;
            this.labelEventParam4.Text = "Param 4";
            this.labelEventParam4.MouseEnter += new System.EventHandler(this.labelEventParam4_MouseEnter);
            // 
            // labelEventParam3
            // 
            this.labelEventParam3.AutoSize = true;
            this.labelEventParam3.Location = new System.Drawing.Point(1, 59);
            this.labelEventParam3.Name = "labelEventParam3";
            this.labelEventParam3.Size = new System.Drawing.Size(46, 13);
            this.labelEventParam3.TabIndex = 23;
            this.labelEventParam3.Text = "Param 3";
            this.labelEventParam3.MouseEnter += new System.EventHandler(this.labelEventParam3_MouseEnter);
            // 
            // labelEventParam2
            // 
            this.labelEventParam2.AutoSize = true;
            this.labelEventParam2.Location = new System.Drawing.Point(1, 33);
            this.labelEventParam2.Name = "labelEventParam2";
            this.labelEventParam2.Size = new System.Drawing.Size(46, 13);
            this.labelEventParam2.TabIndex = 23;
            this.labelEventParam2.Text = "Param 2";
            this.labelEventParam2.MouseEnter += new System.EventHandler(this.labelEventParam2_MouseEnter);
            // 
            // textBoxEventParam4
            // 
            this.textBoxEventParam4.Location = new System.Drawing.Point(144, 82);
            this.textBoxEventParam4.Name = "textBoxEventParam4";
            this.textBoxEventParam4.Size = new System.Drawing.Size(70, 20);
            this.textBoxEventParam4.TabIndex = 43;
            this.textBoxEventParam4.Text = "0";
            this.textBoxEventParam4.TextChanged += new System.EventHandler(this.textBoxEventParam4_TextChanged);
            this.textBoxEventParam4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericField_KeyPress);
            // 
            // textBoxEventParam3
            // 
            this.textBoxEventParam3.Location = new System.Drawing.Point(144, 56);
            this.textBoxEventParam3.Name = "textBoxEventParam3";
            this.textBoxEventParam3.Size = new System.Drawing.Size(70, 20);
            this.textBoxEventParam3.TabIndex = 41;
            this.textBoxEventParam3.Text = "0";
            this.textBoxEventParam3.TextChanged += new System.EventHandler(this.textBoxEventParam3_TextChanged);
            this.textBoxEventParam3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericField_KeyPress);
            // 
            // textBoxEventParam2
            // 
            this.textBoxEventParam2.Location = new System.Drawing.Point(144, 30);
            this.textBoxEventParam2.Name = "textBoxEventParam2";
            this.textBoxEventParam2.Size = new System.Drawing.Size(70, 20);
            this.textBoxEventParam2.TabIndex = 39;
            this.textBoxEventParam2.Text = "0";
            this.textBoxEventParam2.TextChanged += new System.EventHandler(this.textBoxEventParam2_TextChanged);
            this.textBoxEventParam2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericField_KeyPress);
            // 
            // tabPageAction
            // 
            this.tabPageAction.BackColor = System.Drawing.SystemColors.Window;
            this.tabPageAction.Controls.Add(this.buttonActionParamSixSearch);
            this.tabPageAction.Controls.Add(this.buttonActionParamFiveSearch);
            this.tabPageAction.Controls.Add(this.buttonActionParamFourSearch);
            this.tabPageAction.Controls.Add(this.buttonActionParamThreeSearch);
            this.tabPageAction.Controls.Add(this.buttonActionParamTwoSearch);
            this.tabPageAction.Controls.Add(this.buttonActionParamOneSearch);
            this.tabPageAction.Controls.Add(this.labelActionParam6);
            this.tabPageAction.Controls.Add(this.labelActionParam5);
            this.tabPageAction.Controls.Add(this.labelActionParam4);
            this.tabPageAction.Controls.Add(this.labelActionParam3);
            this.tabPageAction.Controls.Add(this.labelActionParam2);
            this.tabPageAction.Controls.Add(this.textBoxActionParam6);
            this.tabPageAction.Controls.Add(this.textBoxActionParam5);
            this.tabPageAction.Controls.Add(this.textBoxActionParam4);
            this.tabPageAction.Controls.Add(this.textBoxActionParam3);
            this.tabPageAction.Controls.Add(this.textBoxActionParam2);
            this.tabPageAction.Controls.Add(this.labelActionParam1);
            this.tabPageAction.Controls.Add(this.textBoxActionParam1);
            this.tabPageAction.Location = new System.Drawing.Point(4, 22);
            this.tabPageAction.Name = "tabPageAction";
            this.tabPageAction.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAction.Size = new System.Drawing.Size(256, 127);
            this.tabPageAction.TabIndex = 1;
            this.tabPageAction.Text = "Action";
            // 
            // buttonActionParamSixSearch
            // 
            this.buttonActionParamSixSearch.Location = new System.Drawing.Point(214, 133);
            this.buttonActionParamSixSearch.Name = "buttonActionParamSixSearch";
            this.buttonActionParamSixSearch.Size = new System.Drawing.Size(24, 22);
            this.buttonActionParamSixSearch.TabIndex = 57;
            this.buttonActionParamSixSearch.Text = "...";
            this.buttonActionParamSixSearch.UseVisualStyleBackColor = true;
            this.buttonActionParamSixSearch.Visible = false;
            this.buttonActionParamSixSearch.Click += new System.EventHandler(this.buttonActionParamSixSearch_Click);
            // 
            // buttonActionParamFiveSearch
            // 
            this.buttonActionParamFiveSearch.Location = new System.Drawing.Point(214, 107);
            this.buttonActionParamFiveSearch.Name = "buttonActionParamFiveSearch";
            this.buttonActionParamFiveSearch.Size = new System.Drawing.Size(24, 22);
            this.buttonActionParamFiveSearch.TabIndex = 54;
            this.buttonActionParamFiveSearch.Text = "...";
            this.buttonActionParamFiveSearch.UseVisualStyleBackColor = true;
            this.buttonActionParamFiveSearch.Visible = false;
            this.buttonActionParamFiveSearch.Click += new System.EventHandler(this.buttonActionParamFiveSearch_Click);
            // 
            // buttonActionParamFourSearch
            // 
            this.buttonActionParamFourSearch.Location = new System.Drawing.Point(214, 81);
            this.buttonActionParamFourSearch.Name = "buttonActionParamFourSearch";
            this.buttonActionParamFourSearch.Size = new System.Drawing.Size(24, 22);
            this.buttonActionParamFourSearch.TabIndex = 52;
            this.buttonActionParamFourSearch.Text = "...";
            this.buttonActionParamFourSearch.UseVisualStyleBackColor = true;
            this.buttonActionParamFourSearch.Visible = false;
            this.buttonActionParamFourSearch.Click += new System.EventHandler(this.buttonActionParamFourSearch_Click);
            // 
            // buttonActionParamThreeSearch
            // 
            this.buttonActionParamThreeSearch.Location = new System.Drawing.Point(214, 55);
            this.buttonActionParamThreeSearch.Name = "buttonActionParamThreeSearch";
            this.buttonActionParamThreeSearch.Size = new System.Drawing.Size(24, 22);
            this.buttonActionParamThreeSearch.TabIndex = 50;
            this.buttonActionParamThreeSearch.Text = "...";
            this.buttonActionParamThreeSearch.UseVisualStyleBackColor = true;
            this.buttonActionParamThreeSearch.Visible = false;
            this.buttonActionParamThreeSearch.Click += new System.EventHandler(this.buttonActionParamThreeSearch_Click);
            // 
            // buttonActionParamTwoSearch
            // 
            this.buttonActionParamTwoSearch.Location = new System.Drawing.Point(214, 29);
            this.buttonActionParamTwoSearch.Name = "buttonActionParamTwoSearch";
            this.buttonActionParamTwoSearch.Size = new System.Drawing.Size(24, 22);
            this.buttonActionParamTwoSearch.TabIndex = 48;
            this.buttonActionParamTwoSearch.Text = "...";
            this.buttonActionParamTwoSearch.UseVisualStyleBackColor = true;
            this.buttonActionParamTwoSearch.Visible = false;
            this.buttonActionParamTwoSearch.Click += new System.EventHandler(this.buttonActionParamTwoSearch_Click);
            // 
            // buttonActionParamOneSearch
            // 
            this.buttonActionParamOneSearch.Location = new System.Drawing.Point(214, 3);
            this.buttonActionParamOneSearch.Name = "buttonActionParamOneSearch";
            this.buttonActionParamOneSearch.Size = new System.Drawing.Size(24, 22);
            this.buttonActionParamOneSearch.TabIndex = 46;
            this.buttonActionParamOneSearch.Text = "...";
            this.buttonActionParamOneSearch.UseVisualStyleBackColor = true;
            this.buttonActionParamOneSearch.Visible = false;
            this.buttonActionParamOneSearch.Click += new System.EventHandler(this.buttonActionParamOneSearch_Click);
            // 
            // labelActionParam6
            // 
            this.labelActionParam6.AutoSize = true;
            this.labelActionParam6.Location = new System.Drawing.Point(1, 137);
            this.labelActionParam6.Name = "labelActionParam6";
            this.labelActionParam6.Size = new System.Drawing.Size(46, 13);
            this.labelActionParam6.TabIndex = 24;
            this.labelActionParam6.Text = "Param 6";
            this.labelActionParam6.MouseEnter += new System.EventHandler(this.labelActionParam6_MouseEnter);
            // 
            // labelActionParam5
            // 
            this.labelActionParam5.AutoSize = true;
            this.labelActionParam5.Location = new System.Drawing.Point(1, 111);
            this.labelActionParam5.Name = "labelActionParam5";
            this.labelActionParam5.Size = new System.Drawing.Size(46, 13);
            this.labelActionParam5.TabIndex = 24;
            this.labelActionParam5.Text = "Param 5";
            this.labelActionParam5.MouseEnter += new System.EventHandler(this.labelActionParam5_MouseEnter);
            // 
            // labelActionParam4
            // 
            this.labelActionParam4.AutoSize = true;
            this.labelActionParam4.Location = new System.Drawing.Point(1, 85);
            this.labelActionParam4.Name = "labelActionParam4";
            this.labelActionParam4.Size = new System.Drawing.Size(46, 13);
            this.labelActionParam4.TabIndex = 24;
            this.labelActionParam4.Text = "Param 4";
            this.labelActionParam4.MouseEnter += new System.EventHandler(this.labelActionParam4_MouseEnter);
            // 
            // labelActionParam3
            // 
            this.labelActionParam3.AutoSize = true;
            this.labelActionParam3.Location = new System.Drawing.Point(1, 59);
            this.labelActionParam3.Name = "labelActionParam3";
            this.labelActionParam3.Size = new System.Drawing.Size(46, 13);
            this.labelActionParam3.TabIndex = 25;
            this.labelActionParam3.Text = "Param 3";
            this.labelActionParam3.MouseEnter += new System.EventHandler(this.labelActionParam3_MouseEnter);
            // 
            // labelActionParam2
            // 
            this.labelActionParam2.AutoSize = true;
            this.labelActionParam2.Location = new System.Drawing.Point(1, 33);
            this.labelActionParam2.Name = "labelActionParam2";
            this.labelActionParam2.Size = new System.Drawing.Size(46, 13);
            this.labelActionParam2.TabIndex = 26;
            this.labelActionParam2.Text = "Param 2";
            this.labelActionParam2.MouseEnter += new System.EventHandler(this.labelActionParam2_MouseEnter);
            // 
            // textBoxActionParam6
            // 
            this.textBoxActionParam6.Location = new System.Drawing.Point(144, 134);
            this.textBoxActionParam6.Name = "textBoxActionParam6";
            this.textBoxActionParam6.Size = new System.Drawing.Size(70, 20);
            this.textBoxActionParam6.TabIndex = 56;
            this.textBoxActionParam6.Text = "0";
            this.textBoxActionParam6.TextChanged += new System.EventHandler(this.textBoxActionParam6_TextChanged);
            this.textBoxActionParam6.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericField_KeyPress);
            // 
            // textBoxActionParam5
            // 
            this.textBoxActionParam5.Location = new System.Drawing.Point(144, 108);
            this.textBoxActionParam5.Name = "textBoxActionParam5";
            this.textBoxActionParam5.Size = new System.Drawing.Size(70, 20);
            this.textBoxActionParam5.TabIndex = 53;
            this.textBoxActionParam5.Text = "0";
            this.textBoxActionParam5.TextChanged += new System.EventHandler(this.textBoxActionParam5_TextChanged);
            this.textBoxActionParam5.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericField_KeyPress);
            // 
            // textBoxActionParam4
            // 
            this.textBoxActionParam4.Location = new System.Drawing.Point(144, 82);
            this.textBoxActionParam4.Name = "textBoxActionParam4";
            this.textBoxActionParam4.Size = new System.Drawing.Size(70, 20);
            this.textBoxActionParam4.TabIndex = 51;
            this.textBoxActionParam4.Text = "0";
            this.textBoxActionParam4.TextChanged += new System.EventHandler(this.textBoxActionParam4_TextChanged);
            this.textBoxActionParam4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericField_KeyPress);
            // 
            // textBoxActionParam3
            // 
            this.textBoxActionParam3.Location = new System.Drawing.Point(144, 56);
            this.textBoxActionParam3.Name = "textBoxActionParam3";
            this.textBoxActionParam3.Size = new System.Drawing.Size(70, 20);
            this.textBoxActionParam3.TabIndex = 49;
            this.textBoxActionParam3.Text = "0";
            this.textBoxActionParam3.TextChanged += new System.EventHandler(this.textBoxActionParam3_TextChanged);
            this.textBoxActionParam3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericField_KeyPress);
            // 
            // textBoxActionParam2
            // 
            this.textBoxActionParam2.Location = new System.Drawing.Point(144, 30);
            this.textBoxActionParam2.Name = "textBoxActionParam2";
            this.textBoxActionParam2.Size = new System.Drawing.Size(70, 20);
            this.textBoxActionParam2.TabIndex = 47;
            this.textBoxActionParam2.Text = "0";
            this.textBoxActionParam2.TextChanged += new System.EventHandler(this.textBoxActionParam2_TextChanged);
            this.textBoxActionParam2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericField_KeyPress);
            // 
            // labelActionParam1
            // 
            this.labelActionParam1.AutoSize = true;
            this.labelActionParam1.Location = new System.Drawing.Point(1, 7);
            this.labelActionParam1.Name = "labelActionParam1";
            this.labelActionParam1.Size = new System.Drawing.Size(46, 13);
            this.labelActionParam1.TabIndex = 32;
            this.labelActionParam1.Text = "Param 1";
            this.labelActionParam1.MouseEnter += new System.EventHandler(this.labelActionParam1_MouseEnter);
            // 
            // textBoxActionParam1
            // 
            this.textBoxActionParam1.Location = new System.Drawing.Point(144, 4);
            this.textBoxActionParam1.Name = "textBoxActionParam1";
            this.textBoxActionParam1.Size = new System.Drawing.Size(70, 20);
            this.textBoxActionParam1.TabIndex = 45;
            this.textBoxActionParam1.Text = "0";
            this.textBoxActionParam1.TextChanged += new System.EventHandler(this.textBoxActionParam1_TextChanged);
            this.textBoxActionParam1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericField_KeyPress);
            // 
            // tabPageTarget
            // 
            this.tabPageTarget.BackColor = System.Drawing.SystemColors.Window;
            this.tabPageTarget.Controls.Add(this.buttonTargetParamSevenSearch);
            this.tabPageTarget.Controls.Add(this.buttonTargetParamSixSearch);
            this.tabPageTarget.Controls.Add(this.buttonTargetParamFiveSearch);
            this.tabPageTarget.Controls.Add(this.buttonTargetParamFourSearch);
            this.tabPageTarget.Controls.Add(this.buttonTargetParamThreeSearch);
            this.tabPageTarget.Controls.Add(this.buttonTargetParamTwoSearch);
            this.tabPageTarget.Controls.Add(this.buttonTargetParamOneSearch);
            this.tabPageTarget.Controls.Add(this.labelTargetO);
            this.tabPageTarget.Controls.Add(this.labelTargetZ);
            this.tabPageTarget.Controls.Add(this.labelTargetY);
            this.tabPageTarget.Controls.Add(this.labelTargetX);
            this.tabPageTarget.Controls.Add(this.textBoxTargetO);
            this.tabPageTarget.Controls.Add(this.textBoxTargetZ);
            this.tabPageTarget.Controls.Add(this.labelTargetParam3);
            this.tabPageTarget.Controls.Add(this.textBoxTargetY);
            this.tabPageTarget.Controls.Add(this.labelTargetParam2);
            this.tabPageTarget.Controls.Add(this.textBoxTargetX);
            this.tabPageTarget.Controls.Add(this.textBoxTargetParam3);
            this.tabPageTarget.Controls.Add(this.textBoxTargetParam2);
            this.tabPageTarget.Controls.Add(this.labelTargetParam1);
            this.tabPageTarget.Controls.Add(this.textBoxTargetParam1);
            this.tabPageTarget.Location = new System.Drawing.Point(4, 22);
            this.tabPageTarget.Name = "tabPageTarget";
            this.tabPageTarget.Size = new System.Drawing.Size(256, 127);
            this.tabPageTarget.TabIndex = 2;
            this.tabPageTarget.Text = "Target";
            // 
            // buttonTargetParamSevenSearch
            // 
            this.buttonTargetParamSevenSearch.Location = new System.Drawing.Point(214, 159);
            this.buttonTargetParamSevenSearch.Name = "buttonTargetParamSevenSearch";
            this.buttonTargetParamSevenSearch.Size = new System.Drawing.Size(24, 22);
            this.buttonTargetParamSevenSearch.TabIndex = 46;
            this.buttonTargetParamSevenSearch.Text = "...";
            this.buttonTargetParamSevenSearch.UseVisualStyleBackColor = true;
            this.buttonTargetParamSevenSearch.Visible = false;
            this.buttonTargetParamSevenSearch.Click += new System.EventHandler(this.buttonTargetParamSevenSearch_Click);
            // 
            // buttonTargetParamSixSearch
            // 
            this.buttonTargetParamSixSearch.Location = new System.Drawing.Point(214, 133);
            this.buttonTargetParamSixSearch.Name = "buttonTargetParamSixSearch";
            this.buttonTargetParamSixSearch.Size = new System.Drawing.Size(24, 22);
            this.buttonTargetParamSixSearch.TabIndex = 69;
            this.buttonTargetParamSixSearch.Text = "...";
            this.buttonTargetParamSixSearch.UseVisualStyleBackColor = true;
            this.buttonTargetParamSixSearch.Visible = false;
            this.buttonTargetParamSixSearch.Click += new System.EventHandler(this.buttonTargetParamSixSearch_Click);
            // 
            // buttonTargetParamFiveSearch
            // 
            this.buttonTargetParamFiveSearch.Location = new System.Drawing.Point(214, 107);
            this.buttonTargetParamFiveSearch.Name = "buttonTargetParamFiveSearch";
            this.buttonTargetParamFiveSearch.Size = new System.Drawing.Size(24, 22);
            this.buttonTargetParamFiveSearch.TabIndex = 67;
            this.buttonTargetParamFiveSearch.Text = "...";
            this.buttonTargetParamFiveSearch.UseVisualStyleBackColor = true;
            this.buttonTargetParamFiveSearch.Visible = false;
            this.buttonTargetParamFiveSearch.Click += new System.EventHandler(this.buttonTargetParamFiveSearch_Click);
            // 
            // buttonTargetParamFourSearch
            // 
            this.buttonTargetParamFourSearch.Location = new System.Drawing.Point(214, 81);
            this.buttonTargetParamFourSearch.Name = "buttonTargetParamFourSearch";
            this.buttonTargetParamFourSearch.Size = new System.Drawing.Size(24, 22);
            this.buttonTargetParamFourSearch.TabIndex = 65;
            this.buttonTargetParamFourSearch.Text = "...";
            this.buttonTargetParamFourSearch.UseVisualStyleBackColor = true;
            this.buttonTargetParamFourSearch.Visible = false;
            this.buttonTargetParamFourSearch.Click += new System.EventHandler(this.buttonTargetParamFourSearch_Click);
            // 
            // buttonTargetParamThreeSearch
            // 
            this.buttonTargetParamThreeSearch.Location = new System.Drawing.Point(214, 55);
            this.buttonTargetParamThreeSearch.Name = "buttonTargetParamThreeSearch";
            this.buttonTargetParamThreeSearch.Size = new System.Drawing.Size(24, 22);
            this.buttonTargetParamThreeSearch.TabIndex = 63;
            this.buttonTargetParamThreeSearch.Text = "...";
            this.buttonTargetParamThreeSearch.UseVisualStyleBackColor = true;
            this.buttonTargetParamThreeSearch.Visible = false;
            this.buttonTargetParamThreeSearch.Click += new System.EventHandler(this.buttonTargetParamThreeSearch_Click);
            // 
            // buttonTargetParamTwoSearch
            // 
            this.buttonTargetParamTwoSearch.Location = new System.Drawing.Point(214, 29);
            this.buttonTargetParamTwoSearch.Name = "buttonTargetParamTwoSearch";
            this.buttonTargetParamTwoSearch.Size = new System.Drawing.Size(24, 22);
            this.buttonTargetParamTwoSearch.TabIndex = 61;
            this.buttonTargetParamTwoSearch.Text = "...";
            this.buttonTargetParamTwoSearch.UseVisualStyleBackColor = true;
            this.buttonTargetParamTwoSearch.Visible = false;
            this.buttonTargetParamTwoSearch.Click += new System.EventHandler(this.buttonTargetParamTwoSearch_Click);
            // 
            // buttonTargetParamOneSearch
            // 
            this.buttonTargetParamOneSearch.Location = new System.Drawing.Point(214, 3);
            this.buttonTargetParamOneSearch.Name = "buttonTargetParamOneSearch";
            this.buttonTargetParamOneSearch.Size = new System.Drawing.Size(24, 22);
            this.buttonTargetParamOneSearch.TabIndex = 59;
            this.buttonTargetParamOneSearch.Text = "...";
            this.buttonTargetParamOneSearch.UseVisualStyleBackColor = true;
            this.buttonTargetParamOneSearch.Visible = false;
            this.buttonTargetParamOneSearch.Click += new System.EventHandler(this.buttonTargetParamOneSearch_Click);
            // 
            // labelTargetO
            // 
            this.labelTargetO.AutoSize = true;
            this.labelTargetO.Location = new System.Drawing.Point(1, 163);
            this.labelTargetO.Name = "labelTargetO";
            this.labelTargetO.Size = new System.Drawing.Size(49, 13);
            this.labelTargetO.TabIndex = 34;
            this.labelTargetO.Text = "Target O";
            this.labelTargetO.MouseEnter += new System.EventHandler(this.labelTargetO_MouseEnter);
            // 
            // labelTargetZ
            // 
            this.labelTargetZ.AutoSize = true;
            this.labelTargetZ.Location = new System.Drawing.Point(1, 137);
            this.labelTargetZ.Name = "labelTargetZ";
            this.labelTargetZ.Size = new System.Drawing.Size(48, 13);
            this.labelTargetZ.TabIndex = 34;
            this.labelTargetZ.Text = "Target Z";
            this.labelTargetZ.MouseEnter += new System.EventHandler(this.labelTargetZ_MouseEnter);
            // 
            // labelTargetY
            // 
            this.labelTargetY.AutoSize = true;
            this.labelTargetY.Location = new System.Drawing.Point(1, 111);
            this.labelTargetY.Name = "labelTargetY";
            this.labelTargetY.Size = new System.Drawing.Size(48, 13);
            this.labelTargetY.TabIndex = 34;
            this.labelTargetY.Text = "Target Y";
            this.labelTargetY.MouseEnter += new System.EventHandler(this.labelTargetY_MouseEnter);
            // 
            // labelTargetX
            // 
            this.labelTargetX.AutoSize = true;
            this.labelTargetX.Location = new System.Drawing.Point(1, 85);
            this.labelTargetX.Name = "labelTargetX";
            this.labelTargetX.Size = new System.Drawing.Size(48, 13);
            this.labelTargetX.TabIndex = 34;
            this.labelTargetX.Text = "Target X";
            this.labelTargetX.MouseEnter += new System.EventHandler(this.labelTargetX_MouseEnter);
            // 
            // textBoxTargetO
            // 
            this.textBoxTargetO.Location = new System.Drawing.Point(144, 160);
            this.textBoxTargetO.Name = "textBoxTargetO";
            this.textBoxTargetO.Size = new System.Drawing.Size(70, 20);
            this.textBoxTargetO.TabIndex = 36;
            this.textBoxTargetO.Text = "0";
            this.textBoxTargetO.TextChanged += new System.EventHandler(this.textBoxTargetO_TextChanged);
            this.textBoxTargetO.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericField_KeyPress);
            // 
            // textBoxTargetZ
            // 
            this.textBoxTargetZ.Location = new System.Drawing.Point(144, 134);
            this.textBoxTargetZ.Name = "textBoxTargetZ";
            this.textBoxTargetZ.Size = new System.Drawing.Size(70, 20);
            this.textBoxTargetZ.TabIndex = 68;
            this.textBoxTargetZ.Text = "0";
            this.textBoxTargetZ.TextChanged += new System.EventHandler(this.textBoxTargetZ_TextChanged);
            this.textBoxTargetZ.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericField_KeyPress);
            // 
            // labelTargetParam3
            // 
            this.labelTargetParam3.AutoSize = true;
            this.labelTargetParam3.Location = new System.Drawing.Point(1, 59);
            this.labelTargetParam3.Name = "labelTargetParam3";
            this.labelTargetParam3.Size = new System.Drawing.Size(46, 13);
            this.labelTargetParam3.TabIndex = 34;
            this.labelTargetParam3.Text = "Param 3";
            this.labelTargetParam3.MouseEnter += new System.EventHandler(this.labelTargetParam3_MouseEnter);
            // 
            // textBoxTargetY
            // 
            this.textBoxTargetY.Location = new System.Drawing.Point(144, 108);
            this.textBoxTargetY.Name = "textBoxTargetY";
            this.textBoxTargetY.Size = new System.Drawing.Size(70, 20);
            this.textBoxTargetY.TabIndex = 66;
            this.textBoxTargetY.Text = "0";
            this.textBoxTargetY.TextChanged += new System.EventHandler(this.textBoxTargetY_TextChanged);
            this.textBoxTargetY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericField_KeyPress);
            // 
            // labelTargetParam2
            // 
            this.labelTargetParam2.AutoSize = true;
            this.labelTargetParam2.Location = new System.Drawing.Point(1, 33);
            this.labelTargetParam2.Name = "labelTargetParam2";
            this.labelTargetParam2.Size = new System.Drawing.Size(46, 13);
            this.labelTargetParam2.TabIndex = 35;
            this.labelTargetParam2.Text = "Param 2";
            this.labelTargetParam2.MouseEnter += new System.EventHandler(this.labelTargetParam2_MouseEnter);
            // 
            // textBoxTargetX
            // 
            this.textBoxTargetX.Location = new System.Drawing.Point(144, 82);
            this.textBoxTargetX.Name = "textBoxTargetX";
            this.textBoxTargetX.Size = new System.Drawing.Size(70, 20);
            this.textBoxTargetX.TabIndex = 64;
            this.textBoxTargetX.Text = "0";
            this.textBoxTargetX.TextChanged += new System.EventHandler(this.textBoxTargetX_TextChanged);
            this.textBoxTargetX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericField_KeyPress);
            // 
            // textBoxTargetParam3
            // 
            this.textBoxTargetParam3.Location = new System.Drawing.Point(144, 56);
            this.textBoxTargetParam3.Name = "textBoxTargetParam3";
            this.textBoxTargetParam3.Size = new System.Drawing.Size(70, 20);
            this.textBoxTargetParam3.TabIndex = 62;
            this.textBoxTargetParam3.Text = "0";
            this.textBoxTargetParam3.TextChanged += new System.EventHandler(this.textBoxTargetParam3_TextChanged);
            this.textBoxTargetParam3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericField_KeyPress);
            // 
            // textBoxTargetParam2
            // 
            this.textBoxTargetParam2.Location = new System.Drawing.Point(144, 30);
            this.textBoxTargetParam2.Name = "textBoxTargetParam2";
            this.textBoxTargetParam2.Size = new System.Drawing.Size(70, 20);
            this.textBoxTargetParam2.TabIndex = 60;
            this.textBoxTargetParam2.Text = "0";
            this.textBoxTargetParam2.TextChanged += new System.EventHandler(this.textBoxTargetParam2_TextChanged);
            this.textBoxTargetParam2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericField_KeyPress);
            // 
            // labelTargetParam1
            // 
            this.labelTargetParam1.AutoSize = true;
            this.labelTargetParam1.Location = new System.Drawing.Point(1, 7);
            this.labelTargetParam1.Name = "labelTargetParam1";
            this.labelTargetParam1.Size = new System.Drawing.Size(46, 13);
            this.labelTargetParam1.TabIndex = 38;
            this.labelTargetParam1.Text = "Param 1";
            this.labelTargetParam1.MouseEnter += new System.EventHandler(this.labelTargetParam1_MouseEnter);
            // 
            // textBoxTargetParam1
            // 
            this.textBoxTargetParam1.Location = new System.Drawing.Point(144, 4);
            this.textBoxTargetParam1.Name = "textBoxTargetParam1";
            this.textBoxTargetParam1.Size = new System.Drawing.Size(70, 20);
            this.textBoxTargetParam1.TabIndex = 58;
            this.textBoxTargetParam1.Text = "0";
            this.textBoxTargetParam1.TextChanged += new System.EventHandler(this.textBoxTargetParam1_TextChanged);
            this.textBoxTargetParam1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericField_KeyPress);
            // 
            // groupBoxParameters
            // 
            this.groupBoxParameters.Controls.Add(this.tabControlParameters);
            this.groupBoxParameters.Location = new System.Drawing.Point(649, 31);
            this.groupBoxParameters.Name = "groupBoxParameters";
            this.groupBoxParameters.Size = new System.Drawing.Size(278, 178);
            this.groupBoxParameters.TabIndex = 23;
            this.groupBoxParameters.TabStop = false;
            this.groupBoxParameters.Text = "Parameters";
            this.groupBoxParameters.Visible = false;
            // 
            // panelPermanentTooltipParameters
            // 
            this.panelPermanentTooltipParameters.BackColor = System.Drawing.Color.White;
            this.panelPermanentTooltipParameters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelPermanentTooltipParameters.Controls.Add(this.labelPermanentTooltipTextParameters);
            this.panelPermanentTooltipParameters.Controls.Add(this.labelPermanentTooltipParameterTitleTypes);
            this.panelPermanentTooltipParameters.Controls.Add(this.pictureBox1);
            this.panelPermanentTooltipParameters.Location = new System.Drawing.Point(12, 428);
            this.panelPermanentTooltipParameters.Name = "panelPermanentTooltipParameters";
            this.panelPermanentTooltipParameters.Size = new System.Drawing.Size(915, 30);
            this.panelPermanentTooltipParameters.TabIndex = 25;
            this.panelPermanentTooltipParameters.Visible = false;
            // 
            // labelPermanentTooltipTextParameters
            // 
            this.labelPermanentTooltipTextParameters.AutoSize = true;
            this.labelPermanentTooltipTextParameters.Location = new System.Drawing.Point(26, 15);
            this.labelPermanentTooltipTextParameters.Name = "labelPermanentTooltipTextParameters";
            this.labelPermanentTooltipTextParameters.Size = new System.Drawing.Size(75, 13);
            this.labelPermanentTooltipTextParameters.TabIndex = 30;
            this.labelPermanentTooltipTextParameters.Text = "Parameter info";
            // 
            // labelPermanentTooltipParameterTitleTypes
            // 
            this.labelPermanentTooltipParameterTitleTypes.AutoSize = true;
            this.labelPermanentTooltipParameterTitleTypes.Location = new System.Drawing.Point(26, 2);
            this.labelPermanentTooltipParameterTitleTypes.Name = "labelPermanentTooltipParameterTitleTypes";
            this.labelPermanentTooltipParameterTitleTypes.Size = new System.Drawing.Size(181, 13);
            this.labelPermanentTooltipParameterTitleTypes.TabIndex = 47;
            this.labelPermanentTooltipParameterTitleTypes.Text = "Event type, action type or target type";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(6, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(14, 14);
            this.pictureBox1.TabIndex = 29;
            this.pictureBox1.TabStop = false;
            // 
            // buttonNewLine
            // 
            this.buttonNewLine.Enabled = false;
            this.buttonNewLine.Location = new System.Drawing.Point(649, 215);
            this.buttonNewLine.Name = "buttonNewLine";
            this.buttonNewLine.Size = new System.Drawing.Size(83, 23);
            this.buttonNewLine.TabIndex = 70;
            this.buttonNewLine.Text = "New line";
            this.buttonNewLine.UseVisualStyleBackColor = true;
            this.buttonNewLine.Visible = false;
            this.buttonNewLine.Click += new System.EventHandler(this.buttonNewLine_Click);
            // 
            // buttonGenerateSql
            // 
            this.buttonGenerateSql.Enabled = false;
            this.buttonGenerateSql.Location = new System.Drawing.Point(844, 215);
            this.buttonGenerateSql.Name = "buttonGenerateSql";
            this.buttonGenerateSql.Size = new System.Drawing.Size(83, 23);
            this.buttonGenerateSql.TabIndex = 72;
            this.buttonGenerateSql.Text = "Generate SQL";
            this.buttonGenerateSql.UseVisualStyleBackColor = true;
            this.buttonGenerateSql.Visible = false;
            this.buttonGenerateSql.Click += new System.EventHandler(this.buttonGenerateSql_Click);
            // 
            // buttonGenerateComments
            // 
            this.buttonGenerateComments.Enabled = false;
            this.buttonGenerateComments.Location = new System.Drawing.Point(733, 215);
            this.buttonGenerateComments.Name = "buttonGenerateComments";
            this.buttonGenerateComments.Size = new System.Drawing.Size(110, 23);
            this.buttonGenerateComments.TabIndex = 71;
            this.buttonGenerateComments.Text = "Generate comments";
            this.buttonGenerateComments.UseVisualStyleBackColor = true;
            this.buttonGenerateComments.Visible = false;
            this.buttonGenerateComments.Click += new System.EventHandler(this.buttonGenerateComments_Click);
            // 
            // timerExpandOrContract
            // 
            this.timerExpandOrContract.Interval = 4;
            this.timerExpandOrContract.Tick += new System.EventHandler(this.timerExpandOrContract_Tick);
            // 
            // timerShowPermanentTooltips
            // 
            this.timerShowPermanentTooltips.Interval = 4;
            this.timerShowPermanentTooltips.Tick += new System.EventHandler(this.timerShowPermanentTooltips_Tick);
            // 
            // pictureBoxPermanentTooltip
            // 
            this.pictureBoxPermanentTooltip.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxPermanentTooltip.Image")));
            this.pictureBoxPermanentTooltip.Location = new System.Drawing.Point(6, 7);
            this.pictureBoxPermanentTooltip.Name = "pictureBoxPermanentTooltip";
            this.pictureBoxPermanentTooltip.Size = new System.Drawing.Size(14, 14);
            this.pictureBoxPermanentTooltip.TabIndex = 29;
            this.pictureBoxPermanentTooltip.TabStop = false;
            // 
            // labelPermanentTooltipTextTypes
            // 
            this.labelPermanentTooltipTextTypes.AutoSize = true;
            this.labelPermanentTooltipTextTypes.Location = new System.Drawing.Point(26, 15);
            this.labelPermanentTooltipTextTypes.Name = "labelPermanentTooltipTextTypes";
            this.labelPermanentTooltipTextTypes.Size = new System.Drawing.Size(144, 13);
            this.labelPermanentTooltipTextTypes.TabIndex = 30;
            this.labelPermanentTooltipTextTypes.Text = "Event/action/target type text";
            // 
            // labelPermanentTooltipTitleTypes
            // 
            this.labelPermanentTooltipTitleTypes.AutoSize = true;
            this.labelPermanentTooltipTitleTypes.Location = new System.Drawing.Point(26, 1);
            this.labelPermanentTooltipTitleTypes.Name = "labelPermanentTooltipTitleTypes";
            this.labelPermanentTooltipTitleTypes.Size = new System.Drawing.Size(181, 13);
            this.labelPermanentTooltipTitleTypes.TabIndex = 31;
            this.labelPermanentTooltipTitleTypes.Text = "Event type, action type or target type";
            // 
            // panelPermanentTooltipTypes
            // 
            this.panelPermanentTooltipTypes.BackColor = System.Drawing.Color.White;
            this.panelPermanentTooltipTypes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelPermanentTooltipTypes.Controls.Add(this.labelPermanentTooltipTitleTypes);
            this.panelPermanentTooltipTypes.Controls.Add(this.labelPermanentTooltipTextTypes);
            this.panelPermanentTooltipTypes.Controls.Add(this.pictureBoxPermanentTooltip);
            this.panelPermanentTooltipTypes.Location = new System.Drawing.Point(12, 395);
            this.panelPermanentTooltipTypes.Name = "panelPermanentTooltipTypes";
            this.panelPermanentTooltipTypes.Size = new System.Drawing.Size(915, 30);
            this.panelPermanentTooltipTypes.TabIndex = 25;
            this.panelPermanentTooltipTypes.Visible = false;
            // 
            // listViewSmartScripts
            // 
            this.listViewSmartScripts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listViewSmartScripts.EnablePhaseHighlighting = true;
            this.listViewSmartScripts.FullRowSelect = true;
            this.listViewSmartScripts.Location = new System.Drawing.Point(12, 244);
            this.listViewSmartScripts.MultiSelect = false;
            this.listViewSmartScripts.Name = "listViewSmartScripts";
            this.listViewSmartScripts.Size = new System.Drawing.Size(915, 213);
            this.listViewSmartScripts.TabIndex = 73;
            this.listViewSmartScripts.UseCompatibleStateImageBehavior = false;
            this.listViewSmartScripts.View = System.Windows.Forms.View.Details;
            this.listViewSmartScripts.Visible = false;
            this.listViewSmartScripts.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewSmartScripts_ColumnClick);
            this.listViewSmartScripts.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listViewSmartScripts_ItemSelectionChanged);
            this.listViewSmartScripts.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listViewSmartScripts_MouseClick);
            // 
            // pictureBoxCreateScript
            // 
            this.pictureBoxCreateScript.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxCreateScript.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxCreateScript.Image")));
            this.pictureBoxCreateScript.Location = new System.Drawing.Point(235, 19);
            this.pictureBoxCreateScript.Name = "pictureBoxCreateScript";
            this.pictureBoxCreateScript.ResourceImageStr = "icon_create_script";
            this.pictureBoxCreateScript.Size = new System.Drawing.Size(24, 20);
            this.pictureBoxCreateScript.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxCreateScript.TabIndex = 16;
            this.pictureBoxCreateScript.TabStop = false;
            this.LoadTooltip.SetToolTip(this.pictureBoxCreateScript, "Create a new script using the given source type and entry or guid");
            this.pictureBoxCreateScript.Click += new System.EventHandler(this.pictureBoxCreateScript_Click);
            // 
            // pictureBoxLoadScript
            // 
            this.pictureBoxLoadScript.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxLoadScript.Enabled = false;
            this.pictureBoxLoadScript.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxLoadScript.Image")));
            this.pictureBoxLoadScript.Location = new System.Drawing.Point(260, 19);
            this.pictureBoxLoadScript.Name = "pictureBoxLoadScript";
            this.pictureBoxLoadScript.ResourceImageStr = "icon_load_script";
            this.pictureBoxLoadScript.Size = new System.Drawing.Size(24, 20);
            this.pictureBoxLoadScript.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLoadScript.TabIndex = 16;
            this.pictureBoxLoadScript.TabStop = false;
            this.LoadTooltip.SetToolTip(this.pictureBoxLoadScript, "Load the script(s) using the given source type and entry or guid");
            this.pictureBoxLoadScript.Click += new System.EventHandler(this.pictureBoxLoadScript_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1318, 468);
            this.Controls.Add(this.buttonGenerateComments);
            this.Controls.Add(this.buttonGenerateSql);
            this.Controls.Add(this.buttonNewLine);
            this.Controls.Add(this.panelPermanentTooltipParameters);
            this.Controls.Add(this.panelPermanentTooltipTypes);
            this.Controls.Add(this.panelLoginBox);
            this.Controls.Add(this.listViewSmartScripts);
            this.Controls.Add(this.groupBoxParameters);
            this.Controls.Add(this.groupBoxScriptInfo);
            this.Controls.Add(this.groupBoxPreferences);
            this.Controls.Add(this.groupBoxStaticScriptInfo);
            this.Controls.Add(this.menuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SAI Editor: Login";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.contextMenuStripListView.ResumeLayout(false);
            this.panelLoginBox.ResumeLayout(false);
            this.panelLoginBox.PerformLayout();
            this.groupBoxLogin.ResumeLayout(false);
            this.groupBoxLogin.PerformLayout();
            this.groupBoxStaticScriptInfo.ResumeLayout(false);
            this.groupBoxStaticScriptInfo.PerformLayout();
            this.groupBoxPreferences.ResumeLayout(false);
            this.groupBoxPreferences.PerformLayout();
            this.groupBoxScriptInfo.ResumeLayout(false);
            this.groupBoxScriptInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxEventChance)).EndInit();
            this.tabControlParameters.ResumeLayout(false);
            this.tabPageEvent.ResumeLayout(false);
            this.tabPageEvent.PerformLayout();
            this.tabPageAction.ResumeLayout(false);
            this.tabPageAction.PerformLayout();
            this.tabPageTarget.ResumeLayout(false);
            this.tabPageTarget.PerformLayout();
            this.groupBoxParameters.ResumeLayout(false);
            this.panelPermanentTooltipParameters.ResumeLayout(false);
            this.panelPermanentTooltipParameters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPermanentTooltip)).EndInit();
            this.panelPermanentTooltipTypes.ResumeLayout(false);
            this.panelPermanentTooltipTypes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCreateScript)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoadScript)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuHeaderFiles;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripListView;
        private System.Windows.Forms.ToolStripMenuItem menuItemDeleteSelectedRowListView;
        private System.Windows.Forms.ToolStripMenuItem menuItemSettings;
        private System.Windows.Forms.Panel panelLoginBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelEntryOrGuid;
        private System.Windows.Forms.Button buttonSearchForEntryOrGuid;
        private System.Windows.Forms.GroupBox groupBoxPreferences;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBoxLockEventId;
        private System.Windows.Forms.GroupBox groupBoxScriptInfo;
        private System.Windows.Forms.TabControl tabControlParameters;
        private System.Windows.Forms.TabPage tabPageEvent;
        private System.Windows.Forms.TabPage tabPageAction;
        private System.Windows.Forms.TextBox textBoxComments;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tabPageTarget;
        private System.Windows.Forms.ComboBox comboBoxEventType;
        private System.Windows.Forms.TextBox textBoxEventType;
        private System.Windows.Forms.TextBox textBoxActionType;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ToolStripMenuItem menuItemReconnect;
        private System.Windows.Forms.Label labelEventParam1;
        private System.Windows.Forms.TextBox textBoxEventParam1;
        private System.Windows.Forms.GroupBox groupBoxParameters;
        public System.Windows.Forms.GroupBox groupBoxStaticScriptInfo;
        public System.Windows.Forms.TextBox textBoxEntryOrGuid;
        private System.Windows.Forms.CheckBox checkBoxAllowChangingEntryAndSourceType;
        private System.Windows.Forms.CheckBox checkBoxListActionlistsOrEntries;
        private System.Windows.Forms.CheckBox checkBoxShowBasicInfo;
        private System.Windows.Forms.CheckBox checkBoxScriptByGuid;
        private System.Windows.Forms.Button buttonSearchEventFlags;
        private System.Windows.Forms.NumericUpDown textBoxEventChance;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxId;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button buttonSearchPhasemask;
        private System.Windows.Forms.Button buttonLinkFrom;
        private System.Windows.Forms.Label labelEventParam2;
        private System.Windows.Forms.TextBox textBoxEventParam4;
        private System.Windows.Forms.TextBox textBoxEventParam3;
        private System.Windows.Forms.TextBox textBoxEventParam2;
        private System.Windows.Forms.Label labelEventParam4;
        private System.Windows.Forms.Label labelEventParam3;
        private System.Windows.Forms.Label labelActionParam6;
        private System.Windows.Forms.Label labelActionParam5;
        private System.Windows.Forms.Label labelActionParam4;
        private System.Windows.Forms.Label labelActionParam3;
        private System.Windows.Forms.Label labelActionParam2;
        private System.Windows.Forms.TextBox textBoxActionParam6;
        private System.Windows.Forms.TextBox textBoxActionParam5;
        private System.Windows.Forms.TextBox textBoxActionParam4;
        private System.Windows.Forms.TextBox textBoxActionParam3;
        private System.Windows.Forms.TextBox textBoxActionParam2;
        private System.Windows.Forms.Label labelActionParam1;
        private System.Windows.Forms.TextBox textBoxActionParam1;
        private System.Windows.Forms.Label labelTargetX;
        private System.Windows.Forms.Label labelTargetParam3;
        private System.Windows.Forms.Label labelTargetParam2;
        private System.Windows.Forms.TextBox textBoxTargetX;
        private System.Windows.Forms.TextBox textBoxTargetParam3;
        private System.Windows.Forms.TextBox textBoxTargetParam2;
        private System.Windows.Forms.Label labelTargetParam1;
        private System.Windows.Forms.TextBox textBoxTargetParam1;
        private System.Windows.Forms.TextBox textBoxTargetType;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label labelTargetZ;
        private System.Windows.Forms.Label labelTargetY;
        private System.Windows.Forms.TextBox textBoxTargetZ;
        private System.Windows.Forms.TextBox textBoxTargetY;
        public System.Windows.Forms.ComboBox comboBoxSourceType;
        private System.Windows.Forms.ComboBox comboBoxActionType;
        private System.Windows.Forms.ComboBox comboBoxTargetType;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItemAbout;
        private System.Windows.Forms.Button buttonLinkTo;
        private System.Windows.Forms.Button buttonSelectEventFlag;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItemDeleteSelectedRow;
        private System.Windows.Forms.TextBox textBoxTargetO;
        private System.Windows.Forms.Label labelTargetO;
        public SAI_Editor.Classes.PictureBoxDisableable pictureBoxLoadScript;
        public System.Windows.Forms.TextBox textBoxEventPhasemask;
        public System.Windows.Forms.TextBox textBoxEventFlags;
        public System.Windows.Forms.TextBox textBoxLinkFrom;
        public System.Windows.Forms.TextBox textBoxLinkTo;
        private System.Windows.Forms.ToolTip LoadTooltip;
        private System.Windows.Forms.Button buttonEventParamOneSearch;
        private System.Windows.Forms.Button buttonEventParamTwoSearch;
        private System.Windows.Forms.Button buttonEventParamFourSearch;
        private System.Windows.Forms.Button buttonEventParamThreeSearch;
        private System.Windows.Forms.Button buttonActionParamSixSearch;
        private System.Windows.Forms.Button buttonActionParamFiveSearch;
        private System.Windows.Forms.Button buttonActionParamFourSearch;
        private System.Windows.Forms.Button buttonActionParamThreeSearch;
        private System.Windows.Forms.Button buttonActionParamTwoSearch;
        private System.Windows.Forms.Button buttonActionParamOneSearch;
        private System.Windows.Forms.Button buttonTargetParamSevenSearch;
        private System.Windows.Forms.Button buttonTargetParamSixSearch;
        private System.Windows.Forms.Button buttonTargetParamFiveSearch;
        private System.Windows.Forms.Button buttonTargetParamFourSearch;
        private System.Windows.Forms.Button buttonTargetParamThreeSearch;
        private System.Windows.Forms.Button buttonTargetParamTwoSearch;
        private System.Windows.Forms.Button buttonTargetParamOneSearch;
        private System.Windows.Forms.ToolStripMenuItem otherToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem smartAIWikiToolStripMenuItem;
        private System.Windows.Forms.Panel panelPermanentTooltipParameters;
        private System.Windows.Forms.Label labelPermanentTooltipTextParameters;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonNewLine;
        private System.Windows.Forms.Button buttonGenerateSql;
        private System.Windows.Forms.ToolStripMenuItem menuItemGenerateSql;
        public SAI_Editor.Classes.SmartScriptListView listViewSmartScripts;
        private System.Windows.Forms.Button buttonGenerateComments;
        public System.Windows.Forms.ToolStripMenuItem menuItemRevertQuery;
        public SAI_Editor.Classes.PictureBoxDisableable pictureBoxCreateScript;
        private System.Windows.Forms.ToolStripMenuItem menuItemGenerateCommentListView;
        private System.Windows.Forms.ToolStripMenuItem menuItemLoadSelectedEntryListView;
        private System.Windows.Forms.Label labelPermanentTooltipParameterTitleTypes;
        private System.Windows.Forms.Timer timerExpandOrContract;
        private System.Windows.Forms.Timer timerShowPermanentTooltips;
        private System.Windows.Forms.PictureBox pictureBoxPermanentTooltip;
        private System.Windows.Forms.Label labelPermanentTooltipTextTypes;
        private System.Windows.Forms.Label labelPermanentTooltipTitleTypes;
        private System.Windows.Forms.Panel panelPermanentTooltipTypes;
        private System.Windows.Forms.GroupBox groupBoxLogin;
        private System.Windows.Forms.RadioButton radioButtonDontUseDatabase;
        private System.Windows.Forms.RadioButton radioButtonConnectToMySql;
        private System.Windows.Forms.Button buttonSearchWorldDb;
        public System.Windows.Forms.CheckBox checkBoxAutoConnect;
        public System.Windows.Forms.TextBox textBoxHost;
        public System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.Label labelUser;
        public System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox textBoxWorldDatabase;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Label labelHost;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.Label labelDontUseDatabaseWarning;
        private System.Windows.Forms.ToolStripMenuItem menuItemRetrieveLastDeletedRow;
        private System.Windows.Forms.ToolStripMenuItem menuItemDuplicateSelectedRowListView;
        private System.Windows.Forms.ToolStripMenuItem menuitemLoadSelectedEntry;
        private System.Windows.Forms.ToolStripMenuItem menuItemGenerateComment;
        private System.Windows.Forms.ToolStripMenuItem menuItemDuplicateRow;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuItemCopySelectedRowListView;
        private System.Windows.Forms.ToolStripMenuItem menuItemCopySelectedRow;
        private System.Windows.Forms.ToolStripMenuItem menuItemPasteLastCopiedRow;
    }
}

