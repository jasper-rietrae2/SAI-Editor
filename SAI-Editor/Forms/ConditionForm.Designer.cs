namespace SAI_Editor.Forms
{
    partial class ConditionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConditionForm));
            this.comboBoxConditionSourceTypes = new System.Windows.Forms.ComboBox();
            this.comboBoxConditionTypes = new System.Windows.Forms.ComboBox();
            this.labelSourceType = new System.Windows.Forms.Label();
            this.labelConditionType = new System.Windows.Forms.Label();
            this.labelCondValue1 = new System.Windows.Forms.Label();
            this.labelCondValue2 = new System.Windows.Forms.Label();
            this.labelCondValue3 = new System.Windows.Forms.Label();
            this.labelSourceGroup = new System.Windows.Forms.Label();
            this.labelSourceEntry = new System.Windows.Forms.Label();
            this.labelConditionTarget = new System.Windows.Forms.Label();
            this.comboBoxConditionTarget = new System.Windows.Forms.ComboBox();
            this.textBoxSourceGroup = new System.Windows.Forms.TextBox();
            this.textBoxSourceEntry = new System.Windows.Forms.TextBox();
            this.labelComment = new System.Windows.Forms.Label();
            this.textBoxComment = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxCondValue1 = new System.Windows.Forms.TextBox();
            this.textBoxCondValue2 = new System.Windows.Forms.TextBox();
            this.textBoxCondValue3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.buttonSearchSourceGroup = new System.Windows.Forms.Button();
            this.buttonSearchSourceEntry = new System.Windows.Forms.Button();
            this.panelPermanentTooltipSourceType = new System.Windows.Forms.Panel();
            this.labelPermanentTooltipSourceType = new System.Windows.Forms.Label();
            this.labelPermanentTooltipSourceTypeValues = new System.Windows.Forms.Label();
            this.pictureBoxPermanentTooltip = new System.Windows.Forms.PictureBox();
            this.panelPermanentTooltipConditionType = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonSearchConditionValue1 = new System.Windows.Forms.Button();
            this.buttonSearchConditionValue2 = new System.Windows.Forms.Button();
            this.buttonSearchConditionValue3 = new System.Windows.Forms.Button();
            this.labelCondValue4 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.textBoxCondValue4 = new System.Windows.Forms.TextBox();
            this.buttonSearchConditionValue4 = new System.Windows.Forms.Button();
            this.buttonGenerateComment = new System.Windows.Forms.Button();
            this.buttonGenerateSql = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageEditing = new System.Windows.Forms.TabPage();
            this.label14 = new System.Windows.Forms.Label();
            this.textBoxScriptName = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.buttonSaveCondition = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.buttonSearchSourceId = new System.Windows.Forms.Button();
            this.buttonSearchErrorTextId = new System.Windows.Forms.Button();
            this.buttonSearchErrorType = new System.Windows.Forms.Button();
            this.labelSourceId = new System.Windows.Forms.Label();
            this.labelElseGroup = new System.Windows.Forms.Label();
            this.textBoxSourceId = new System.Windows.Forms.TextBox();
            this.textBoxElseGroup = new System.Windows.Forms.TextBox();
            this.textBoxErrorTextId = new System.Windows.Forms.TextBox();
            this.textBoxErrorType = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.lineSeparator1 = new SAI_Editor.Classes.CustomControls.LineSeparator();
            this.label18 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.tabPageSql = new System.Windows.Forms.TabPage();
            this.richTextBoxSql = new FastColoredTextBoxNS.FastColoredTextBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.listViewConditions = new SAI_Editor.Classes.CustomControls.ConditionListView();
            this.buttonDuplicateCondition = new System.Windows.Forms.Button();
            this.buttonDeleteCondition = new System.Windows.Forms.Button();
            this.buttonLoadCondition = new System.Windows.Forms.Button();
            this.panelPermanentTooltipSourceType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPermanentTooltip)).BeginInit();
            this.panelPermanentTooltipConditionType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabPageEditing.SuspendLayout();
            this.tabPageSql.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.richTextBoxSql)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxConditionSourceTypes
            // 
            this.comboBoxConditionSourceTypes.FormattingEnabled = true;
            this.comboBoxConditionSourceTypes.Items.AddRange(new object[] {
            "SOURCE_TYPE_NONE",
            "SOURCE_TYPE_CREATURE_LOOT_TEMPLATE",
            "SOURCE_TYPE_DISENCHANT_LOOT_TEMPLATE",
            "SOURCE_TYPE_FISHING_LOOT_TEMPLATE",
            "SOURCE_TYPE_GAMEOBJECT_LOOT_TEMPLATE",
            "SOURCE_TYPE_ITEM_LOOT_TEMPLATE",
            "SOURCE_TYPE_MAIL_LOOT_TEMPLATE",
            "SOURCE_TYPE_MILLING_LOOT_TEMPLATE",
            "SOURCE_TYPE_PICKPOCKETING_LOOT_TEMPLATE",
            "SOURCE_TYPE_PROSPECTING_LOOT_TEMPLATE",
            "SOURCE_TYPE_REFERENCE_LOOT_TEMPLATE",
            "SOURCE_TYPE_SKINNING_LOOT_TEMPLATE",
            "SOURCE_TYPE_SPELL_LOOT_TEMPLATE",
            "SOURCE_TYPE_SPELL_IMPLICIT_TARGET",
            "SOURCE_TYPE_GOSSIP_MENU",
            "SOURCE_TYPE_GOSSIP_MENU_OPTION",
            "SOURCE_TYPE_CREATURE_TEMPLATE_VEHICLE",
            "SOURCE_TYPE_SPELL",
            "SOURCE_TYPE_SPELL_CLICK_EVENT",
            "SOURCE_TYPE_QUEST_ACCEPT",
            "SOURCE_TYPE_QUEST_SHOW_MARK",
            "SOURCE_TYPE_VEHICLE_SPELL",
            "SOURCE_TYPE_SMART_EVENT",
            "SOURCE_TYPE_NPC_VENDOR",
            "SOURCE_TYPE_SPELL_PROC"});
            this.comboBoxConditionSourceTypes.Location = new System.Drawing.Point(119, 7);
            this.comboBoxConditionSourceTypes.Name = "comboBoxConditionSourceTypes";
            this.comboBoxConditionSourceTypes.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.comboBoxConditionSourceTypes.Size = new System.Drawing.Size(290, 21);
            this.comboBoxConditionSourceTypes.TabIndex = 0;
            this.comboBoxConditionSourceTypes.SelectedIndexChanged += new System.EventHandler(this.comboBoxConditionSourceTypes_SelectedIndexChanged);
            // 
            // comboBoxConditionTypes
            // 
            this.comboBoxConditionTypes.FormattingEnabled = true;
            this.comboBoxConditionTypes.Items.AddRange(new object[] {
            "CONDITION_NONE",
            "CONDITION_AURA",
            "CONDITION_ITEM",
            "CONDITION_ITEM_EQUIPPED",
            "CONDITION_ZONEID",
            "CONDITION_REPUTATION_RANK",
            "CONDITION_TEAM",
            "CONDITION_SKILL",
            "CONDITION_QUESTREWARDED",
            "CONDITION_QUESTTAKEN",
            "CONDITION_DRUNKENSTATE",
            "CONDITION_WORLD_STATE",
            "CONDITION_ACTIVE_EVENT",
            "CONDITION_INSTANCE_INFO",
            "CONDITION_QUEST_NONE",
            "CONDITION_CLASS",
            "CONDITION_RACE",
            "CONDITION_ACHIEVEMENT",
            "CONDITION_TITLE",
            "CONDITION_SPAWNMASK",
            "CONDITION_GENDER",
            "CONDITION_UNIT_STATE",
            "CONDITION_MAPID",
            "CONDITION_AREAID",
            "CONDITION_CREATURE_TYPE",
            "CONDITION_SPELL",
            "CONDITION_PHASEMASK",
            "CONDITION_LEVEL",
            "CONDITION_QUEST_COMPLETE",
            "CONDITION_NEAR_CREATURE",
            "CONDITION_NEAR_GAMEOBJECT",
            "CONDITION_OBJECT_ENTRY",
            "CONDITION_TYPE_MASK",
            "CONDITION_RELATION_TO",
            "CONDITION_REACTION_TO",
            "CONDITION_DISTANCE_TO",
            "CONDITION_ALIVE",
            "CONDITION_HP_VAL",
            "CONDITION_HP_PCT"});
            this.comboBoxConditionTypes.Location = new System.Drawing.Point(119, 128);
            this.comboBoxConditionTypes.Name = "comboBoxConditionTypes";
            this.comboBoxConditionTypes.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.comboBoxConditionTypes.Size = new System.Drawing.Size(290, 21);
            this.comboBoxConditionTypes.TabIndex = 1;
            this.comboBoxConditionTypes.SelectedIndexChanged += new System.EventHandler(this.comboBoxConditionTypes_SelectedIndexChanged);
            this.comboBoxConditionTypes.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBoxesConditions_KeyPress);
            // 
            // labelSourceType
            // 
            this.labelSourceType.AutoSize = true;
            this.labelSourceType.Location = new System.Drawing.Point(12, 10);
            this.labelSourceType.Name = "labelSourceType";
            this.labelSourceType.Size = new System.Drawing.Size(64, 13);
            this.labelSourceType.TabIndex = 2;
            this.labelSourceType.Text = "Source type";
            // 
            // labelConditionType
            // 
            this.labelConditionType.AutoSize = true;
            this.labelConditionType.Location = new System.Drawing.Point(12, 131);
            this.labelConditionType.Name = "labelConditionType";
            this.labelConditionType.Size = new System.Drawing.Size(74, 13);
            this.labelConditionType.TabIndex = 3;
            this.labelConditionType.Text = "Condition type";
            // 
            // labelCondValue1
            // 
            this.labelCondValue1.AutoSize = true;
            this.labelCondValue1.Location = new System.Drawing.Point(12, 158);
            this.labelCondValue1.Name = "labelCondValue1";
            this.labelCondValue1.Size = new System.Drawing.Size(89, 13);
            this.labelCondValue1.TabIndex = 4;
            this.labelCondValue1.Text = "Condition value 1";
            // 
            // labelCondValue2
            // 
            this.labelCondValue2.AutoSize = true;
            this.labelCondValue2.Location = new System.Drawing.Point(12, 184);
            this.labelCondValue2.Name = "labelCondValue2";
            this.labelCondValue2.Size = new System.Drawing.Size(89, 13);
            this.labelCondValue2.TabIndex = 5;
            this.labelCondValue2.Text = "Condition value 2";
            // 
            // labelCondValue3
            // 
            this.labelCondValue3.AutoSize = true;
            this.labelCondValue3.Location = new System.Drawing.Point(12, 210);
            this.labelCondValue3.Name = "labelCondValue3";
            this.labelCondValue3.Size = new System.Drawing.Size(89, 13);
            this.labelCondValue3.TabIndex = 6;
            this.labelCondValue3.Text = "Condition value 3";
            // 
            // labelSourceGroup
            // 
            this.labelSourceGroup.AutoSize = true;
            this.labelSourceGroup.Location = new System.Drawing.Point(422, 10);
            this.labelSourceGroup.Name = "labelSourceGroup";
            this.labelSourceGroup.Size = new System.Drawing.Size(71, 13);
            this.labelSourceGroup.TabIndex = 7;
            this.labelSourceGroup.Text = "Source group";
            // 
            // labelSourceEntry
            // 
            this.labelSourceEntry.AutoSize = true;
            this.labelSourceEntry.Location = new System.Drawing.Point(422, 37);
            this.labelSourceEntry.Name = "labelSourceEntry";
            this.labelSourceEntry.Size = new System.Drawing.Size(67, 13);
            this.labelSourceEntry.TabIndex = 8;
            this.labelSourceEntry.Text = "Source entry";
            // 
            // labelConditionTarget
            // 
            this.labelConditionTarget.AutoSize = true;
            this.labelConditionTarget.Location = new System.Drawing.Point(12, 37);
            this.labelConditionTarget.Name = "labelConditionTarget";
            this.labelConditionTarget.Size = new System.Drawing.Size(81, 13);
            this.labelConditionTarget.TabIndex = 9;
            this.labelConditionTarget.Text = "Condition target";
            // 
            // comboBoxConditionTarget
            // 
            this.comboBoxConditionTarget.FormattingEnabled = true;
            this.comboBoxConditionTarget.Location = new System.Drawing.Point(119, 34);
            this.comboBoxConditionTarget.Name = "comboBoxConditionTarget";
            this.comboBoxConditionTarget.Size = new System.Drawing.Size(290, 21);
            this.comboBoxConditionTarget.TabIndex = 10;
            this.comboBoxConditionTarget.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBoxesConditions_KeyPress);
            // 
            // textBoxSourceGroup
            // 
            this.textBoxSourceGroup.Location = new System.Drawing.Point(552, 7);
            this.textBoxSourceGroup.Name = "textBoxSourceGroup";
            this.textBoxSourceGroup.Size = new System.Drawing.Size(66, 20);
            this.textBoxSourceGroup.TabIndex = 11;
            this.textBoxSourceGroup.Text = "0";
            this.textBoxSourceGroup.TextChanged += new System.EventHandler(this.textBoxesConditionEditor_TextChanged);
            this.textBoxSourceGroup.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxesConditionEditor_KeyPress);
            // 
            // textBoxSourceEntry
            // 
            this.textBoxSourceEntry.Location = new System.Drawing.Point(552, 34);
            this.textBoxSourceEntry.Name = "textBoxSourceEntry";
            this.textBoxSourceEntry.Size = new System.Drawing.Size(66, 20);
            this.textBoxSourceEntry.TabIndex = 12;
            this.textBoxSourceEntry.Text = "0";
            this.textBoxSourceEntry.TextChanged += new System.EventHandler(this.textBoxesConditionEditor_TextChanged);
            this.textBoxSourceEntry.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxesConditionEditor_KeyPress);
            // 
            // labelComment
            // 
            this.labelComment.AutoSize = true;
            this.labelComment.Location = new System.Drawing.Point(12, 90);
            this.labelComment.Name = "labelComment";
            this.labelComment.Size = new System.Drawing.Size(51, 13);
            this.labelComment.TabIndex = 14;
            this.labelComment.Text = "Comment";
            // 
            // textBoxComment
            // 
            this.textBoxComment.Location = new System.Drawing.Point(119, 87);
            this.textBoxComment.Name = "textBoxComment";
            this.textBoxComment.Size = new System.Drawing.Size(290, 20);
            this.textBoxComment.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(538, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(10, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = ":";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(538, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(10, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = ":";
            // 
            // textBoxCondValue1
            // 
            this.textBoxCondValue1.Location = new System.Drawing.Point(119, 155);
            this.textBoxCondValue1.Name = "textBoxCondValue1";
            this.textBoxCondValue1.Size = new System.Drawing.Size(66, 20);
            this.textBoxCondValue1.TabIndex = 18;
            this.textBoxCondValue1.Text = "0";
            this.textBoxCondValue1.TextChanged += new System.EventHandler(this.textBoxesConditionEditor_TextChanged);
            this.textBoxCondValue1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxesConditionEditor_KeyPress);
            // 
            // textBoxCondValue2
            // 
            this.textBoxCondValue2.Location = new System.Drawing.Point(119, 181);
            this.textBoxCondValue2.Name = "textBoxCondValue2";
            this.textBoxCondValue2.Size = new System.Drawing.Size(66, 20);
            this.textBoxCondValue2.TabIndex = 18;
            this.textBoxCondValue2.Text = "0";
            this.textBoxCondValue2.TextChanged += new System.EventHandler(this.textBoxesConditionEditor_TextChanged);
            this.textBoxCondValue2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxesConditionEditor_KeyPress);
            // 
            // textBoxCondValue3
            // 
            this.textBoxCondValue3.Location = new System.Drawing.Point(119, 207);
            this.textBoxCondValue3.Name = "textBoxCondValue3";
            this.textBoxCondValue3.Size = new System.Drawing.Size(66, 20);
            this.textBoxCondValue3.TabIndex = 18;
            this.textBoxCondValue3.Text = "0";
            this.textBoxCondValue3.TextChanged += new System.EventHandler(this.textBoxesConditionEditor_TextChanged);
            this.textBoxCondValue3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxesConditionEditor_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(107, 158);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(10, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = ":";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(107, 184);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(10, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = ":";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(107, 210);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(10, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = ":";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(105, 89);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(10, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = ":";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(105, 37);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(10, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = ":";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(105, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(10, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = ":";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(107, 131);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(10, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = ":";
            // 
            // buttonSearchSourceGroup
            // 
            this.buttonSearchSourceGroup.Enabled = false;
            this.buttonSearchSourceGroup.Location = new System.Drawing.Point(618, 6);
            this.buttonSearchSourceGroup.Name = "buttonSearchSourceGroup";
            this.buttonSearchSourceGroup.Size = new System.Drawing.Size(24, 22);
            this.buttonSearchSourceGroup.TabIndex = 19;
            this.buttonSearchSourceGroup.Text = "...";
            this.buttonSearchSourceGroup.UseVisualStyleBackColor = true;
            this.buttonSearchSourceGroup.Click += new System.EventHandler(this.buttonSearchSourceGroup_Click);
            // 
            // buttonSearchSourceEntry
            // 
            this.buttonSearchSourceEntry.Enabled = false;
            this.buttonSearchSourceEntry.Location = new System.Drawing.Point(618, 33);
            this.buttonSearchSourceEntry.Name = "buttonSearchSourceEntry";
            this.buttonSearchSourceEntry.Size = new System.Drawing.Size(24, 22);
            this.buttonSearchSourceEntry.TabIndex = 19;
            this.buttonSearchSourceEntry.Text = "...";
            this.buttonSearchSourceEntry.UseVisualStyleBackColor = true;
            this.buttonSearchSourceEntry.Click += new System.EventHandler(this.buttonSearchSourceEntry_Click);
            // 
            // panelPermanentTooltipSourceType
            // 
            this.panelPermanentTooltipSourceType.BackColor = System.Drawing.Color.White;
            this.panelPermanentTooltipSourceType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelPermanentTooltipSourceType.Controls.Add(this.labelPermanentTooltipSourceType);
            this.panelPermanentTooltipSourceType.Controls.Add(this.labelPermanentTooltipSourceTypeValues);
            this.panelPermanentTooltipSourceType.Controls.Add(this.pictureBoxPermanentTooltip);
            this.panelPermanentTooltipSourceType.Location = new System.Drawing.Point(15, 264);
            this.panelPermanentTooltipSourceType.Name = "panelPermanentTooltipSourceType";
            this.panelPermanentTooltipSourceType.Size = new System.Drawing.Size(627, 30);
            this.panelPermanentTooltipSourceType.TabIndex = 26;
            // 
            // labelPermanentTooltipSourceType
            // 
            this.labelPermanentTooltipSourceType.AutoSize = true;
            this.labelPermanentTooltipSourceType.Location = new System.Drawing.Point(26, 1);
            this.labelPermanentTooltipSourceType.Name = "labelPermanentTooltipSourceType";
            this.labelPermanentTooltipSourceType.Size = new System.Drawing.Size(181, 13);
            this.labelPermanentTooltipSourceType.TabIndex = 31;
            this.labelPermanentTooltipSourceType.Text = "Event type, action type or target type";
            // 
            // labelPermanentTooltipSourceTypeValues
            // 
            this.labelPermanentTooltipSourceTypeValues.AutoSize = true;
            this.labelPermanentTooltipSourceTypeValues.Location = new System.Drawing.Point(26, 15);
            this.labelPermanentTooltipSourceTypeValues.Name = "labelPermanentTooltipSourceTypeValues";
            this.labelPermanentTooltipSourceTypeValues.Size = new System.Drawing.Size(144, 13);
            this.labelPermanentTooltipSourceTypeValues.TabIndex = 30;
            this.labelPermanentTooltipSourceTypeValues.Text = "Event/action/target type text";
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
            // panelPermanentTooltipConditionType
            // 
            this.panelPermanentTooltipConditionType.BackColor = System.Drawing.Color.White;
            this.panelPermanentTooltipConditionType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelPermanentTooltipConditionType.Controls.Add(this.label10);
            this.panelPermanentTooltipConditionType.Controls.Add(this.label11);
            this.panelPermanentTooltipConditionType.Controls.Add(this.pictureBox1);
            this.panelPermanentTooltipConditionType.Location = new System.Drawing.Point(15, 300);
            this.panelPermanentTooltipConditionType.Name = "panelPermanentTooltipConditionType";
            this.panelPermanentTooltipConditionType.Size = new System.Drawing.Size(627, 30);
            this.panelPermanentTooltipConditionType.TabIndex = 26;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(26, 1);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(181, 13);
            this.label10.TabIndex = 31;
            this.label10.Text = "Event type, action type or target type";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(26, 15);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(144, 13);
            this.label11.TabIndex = 30;
            this.label11.Text = "Event/action/target type text";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(6, 7);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(14, 14);
            this.pictureBox1.TabIndex = 29;
            this.pictureBox1.TabStop = false;
            // 
            // buttonSearchConditionValue1
            // 
            this.buttonSearchConditionValue1.Enabled = false;
            this.buttonSearchConditionValue1.Location = new System.Drawing.Point(185, 154);
            this.buttonSearchConditionValue1.Name = "buttonSearchConditionValue1";
            this.buttonSearchConditionValue1.Size = new System.Drawing.Size(24, 22);
            this.buttonSearchConditionValue1.TabIndex = 19;
            this.buttonSearchConditionValue1.Text = "...";
            this.buttonSearchConditionValue1.UseVisualStyleBackColor = true;
            this.buttonSearchConditionValue1.Click += new System.EventHandler(this.buttonSearchConditionValue1_Click);
            // 
            // buttonSearchConditionValue2
            // 
            this.buttonSearchConditionValue2.Enabled = false;
            this.buttonSearchConditionValue2.Location = new System.Drawing.Point(185, 180);
            this.buttonSearchConditionValue2.Name = "buttonSearchConditionValue2";
            this.buttonSearchConditionValue2.Size = new System.Drawing.Size(24, 22);
            this.buttonSearchConditionValue2.TabIndex = 19;
            this.buttonSearchConditionValue2.Text = "...";
            this.buttonSearchConditionValue2.UseVisualStyleBackColor = true;
            this.buttonSearchConditionValue2.Click += new System.EventHandler(this.buttonSearchConditionValue2_Click);
            // 
            // buttonSearchConditionValue3
            // 
            this.buttonSearchConditionValue3.Enabled = false;
            this.buttonSearchConditionValue3.Location = new System.Drawing.Point(185, 206);
            this.buttonSearchConditionValue3.Name = "buttonSearchConditionValue3";
            this.buttonSearchConditionValue3.Size = new System.Drawing.Size(24, 22);
            this.buttonSearchConditionValue3.TabIndex = 19;
            this.buttonSearchConditionValue3.Text = "...";
            this.buttonSearchConditionValue3.UseVisualStyleBackColor = true;
            this.buttonSearchConditionValue3.Click += new System.EventHandler(this.buttonSearchConditionValue3_Click);
            // 
            // labelCondValue4
            // 
            this.labelCondValue4.AutoSize = true;
            this.labelCondValue4.Location = new System.Drawing.Point(12, 236);
            this.labelCondValue4.Name = "labelCondValue4";
            this.labelCondValue4.Size = new System.Drawing.Size(96, 13);
            this.labelCondValue4.TabIndex = 6;
            this.labelCondValue4.Text = "Negative condition";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(107, 236);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(10, 13);
            this.label13.TabIndex = 16;
            this.label13.Text = ":";
            // 
            // textBoxCondValue4
            // 
            this.textBoxCondValue4.Location = new System.Drawing.Point(119, 233);
            this.textBoxCondValue4.Name = "textBoxCondValue4";
            this.textBoxCondValue4.Size = new System.Drawing.Size(66, 20);
            this.textBoxCondValue4.TabIndex = 18;
            this.textBoxCondValue4.Text = "0";
            this.textBoxCondValue4.TextChanged += new System.EventHandler(this.textBoxesConditionEditor_TextChanged);
            this.textBoxCondValue4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxesConditionEditor_KeyPress);
            // 
            // buttonSearchConditionValue4
            // 
            this.buttonSearchConditionValue4.Enabled = false;
            this.buttonSearchConditionValue4.Location = new System.Drawing.Point(185, 232);
            this.buttonSearchConditionValue4.Name = "buttonSearchConditionValue4";
            this.buttonSearchConditionValue4.Size = new System.Drawing.Size(24, 22);
            this.buttonSearchConditionValue4.TabIndex = 19;
            this.buttonSearchConditionValue4.Text = "...";
            this.buttonSearchConditionValue4.UseVisualStyleBackColor = true;
            this.buttonSearchConditionValue4.Click += new System.EventHandler(this.buttonSearchConditionValue3_Click);
            // 
            // buttonGenerateComment
            // 
            this.buttonGenerateComment.Location = new System.Drawing.Point(517, 201);
            this.buttonGenerateComment.Name = "buttonGenerateComment";
            this.buttonGenerateComment.Size = new System.Drawing.Size(125, 23);
            this.buttonGenerateComment.TabIndex = 27;
            this.buttonGenerateComment.Text = "Generate comment";
            this.buttonGenerateComment.UseVisualStyleBackColor = true;
            this.buttonGenerateComment.Click += new System.EventHandler(this.buttonGenerateComment_Click);
            // 
            // buttonGenerateSql
            // 
            this.buttonGenerateSql.Location = new System.Drawing.Point(517, 230);
            this.buttonGenerateSql.Name = "buttonGenerateSql";
            this.buttonGenerateSql.Size = new System.Drawing.Size(125, 23);
            this.buttonGenerateSql.TabIndex = 28;
            this.buttonGenerateSql.Text = "Generate SQL";
            this.buttonGenerateSql.UseVisualStyleBackColor = true;
            this.buttonGenerateSql.Click += new System.EventHandler(this.buttonGenerateSql_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageEditing);
            this.tabControl.Controls.Add(this.tabPageSql);
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(664, 362);
            this.tabControl.TabIndex = 29;
            // 
            // tabPageEditing
            // 
            this.tabPageEditing.Controls.Add(this.label14);
            this.tabPageEditing.Controls.Add(this.textBoxScriptName);
            this.tabPageEditing.Controls.Add(this.label12);
            this.tabPageEditing.Controls.Add(this.comboBoxConditionSourceTypes);
            this.tabPageEditing.Controls.Add(this.buttonSaveCondition);
            this.tabPageEditing.Controls.Add(this.buttonGenerateSql);
            this.tabPageEditing.Controls.Add(this.comboBoxConditionTypes);
            this.tabPageEditing.Controls.Add(this.buttonGenerateComment);
            this.tabPageEditing.Controls.Add(this.labelSourceType);
            this.tabPageEditing.Controls.Add(this.panelPermanentTooltipConditionType);
            this.tabPageEditing.Controls.Add(this.labelConditionType);
            this.tabPageEditing.Controls.Add(this.panelPermanentTooltipSourceType);
            this.tabPageEditing.Controls.Add(this.label19);
            this.tabPageEditing.Controls.Add(this.label17);
            this.tabPageEditing.Controls.Add(this.labelCondValue1);
            this.tabPageEditing.Controls.Add(this.buttonSearchSourceId);
            this.tabPageEditing.Controls.Add(this.buttonSearchSourceEntry);
            this.tabPageEditing.Controls.Add(this.labelCondValue2);
            this.tabPageEditing.Controls.Add(this.buttonSearchConditionValue4);
            this.tabPageEditing.Controls.Add(this.labelCondValue3);
            this.tabPageEditing.Controls.Add(this.buttonSearchConditionValue3);
            this.tabPageEditing.Controls.Add(this.labelCondValue4);
            this.tabPageEditing.Controls.Add(this.buttonSearchConditionValue2);
            this.tabPageEditing.Controls.Add(this.buttonSearchErrorTextId);
            this.tabPageEditing.Controls.Add(this.labelSourceGroup);
            this.tabPageEditing.Controls.Add(this.buttonSearchErrorType);
            this.tabPageEditing.Controls.Add(this.buttonSearchConditionValue1);
            this.tabPageEditing.Controls.Add(this.labelSourceId);
            this.tabPageEditing.Controls.Add(this.labelElseGroup);
            this.tabPageEditing.Controls.Add(this.labelSourceEntry);
            this.tabPageEditing.Controls.Add(this.buttonSearchSourceGroup);
            this.tabPageEditing.Controls.Add(this.labelConditionTarget);
            this.tabPageEditing.Controls.Add(this.textBoxCondValue4);
            this.tabPageEditing.Controls.Add(this.comboBoxConditionTarget);
            this.tabPageEditing.Controls.Add(this.textBoxCondValue3);
            this.tabPageEditing.Controls.Add(this.textBoxSourceGroup);
            this.tabPageEditing.Controls.Add(this.textBoxSourceId);
            this.tabPageEditing.Controls.Add(this.textBoxElseGroup);
            this.tabPageEditing.Controls.Add(this.textBoxErrorTextId);
            this.tabPageEditing.Controls.Add(this.textBoxCondValue2);
            this.tabPageEditing.Controls.Add(this.textBoxErrorType);
            this.tabPageEditing.Controls.Add(this.textBoxSourceEntry);
            this.tabPageEditing.Controls.Add(this.label20);
            this.tabPageEditing.Controls.Add(this.textBoxCondValue1);
            this.tabPageEditing.Controls.Add(this.label15);
            this.tabPageEditing.Controls.Add(this.lineSeparator1);
            this.tabPageEditing.Controls.Add(this.label2);
            this.tabPageEditing.Controls.Add(this.labelComment);
            this.tabPageEditing.Controls.Add(this.label13);
            this.tabPageEditing.Controls.Add(this.textBoxComment);
            this.tabPageEditing.Controls.Add(this.label5);
            this.tabPageEditing.Controls.Add(this.label18);
            this.tabPageEditing.Controls.Add(this.label1);
            this.tabPageEditing.Controls.Add(this.label16);
            this.tabPageEditing.Controls.Add(this.label4);
            this.tabPageEditing.Controls.Add(this.label3);
            this.tabPageEditing.Controls.Add(this.label8);
            this.tabPageEditing.Controls.Add(this.label9);
            this.tabPageEditing.Controls.Add(this.label7);
            this.tabPageEditing.Controls.Add(this.label6);
            this.tabPageEditing.Location = new System.Drawing.Point(4, 22);
            this.tabPageEditing.Name = "tabPageEditing";
            this.tabPageEditing.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEditing.Size = new System.Drawing.Size(656, 336);
            this.tabPageEditing.TabIndex = 0;
            this.tabPageEditing.Text = "Condition editing";
            this.tabPageEditing.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(105, 63);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(10, 13);
            this.label14.TabIndex = 31;
            this.label14.Text = ":";
            // 
            // textBoxScriptName
            // 
            this.textBoxScriptName.Location = new System.Drawing.Point(119, 61);
            this.textBoxScriptName.Name = "textBoxScriptName";
            this.textBoxScriptName.Size = new System.Drawing.Size(290, 20);
            this.textBoxScriptName.TabIndex = 30;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 64);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(62, 13);
            this.label12.TabIndex = 29;
            this.label12.Text = "ScriptName";
            // 
            // buttonSaveCondition
            // 
            this.buttonSaveCondition.Location = new System.Drawing.Point(386, 230);
            this.buttonSaveCondition.Name = "buttonSaveCondition";
            this.buttonSaveCondition.Size = new System.Drawing.Size(125, 23);
            this.buttonSaveCondition.TabIndex = 28;
            this.buttonSaveCondition.Text = "Save condition";
            this.buttonSaveCondition.UseVisualStyleBackColor = true;
            this.buttonSaveCondition.Click += new System.EventHandler(this.buttonSaveCondition_Click);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(215, 184);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(60, 13);
            this.label19.TabIndex = 4;
            this.label19.Text = "Error text id";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(215, 158);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(52, 13);
            this.label17.TabIndex = 4;
            this.label17.Text = "Error type";
            // 
            // buttonSearchSourceId
            // 
            this.buttonSearchSourceId.Enabled = false;
            this.buttonSearchSourceId.Location = new System.Drawing.Point(618, 85);
            this.buttonSearchSourceId.Name = "buttonSearchSourceId";
            this.buttonSearchSourceId.Size = new System.Drawing.Size(24, 22);
            this.buttonSearchSourceId.TabIndex = 19;
            this.buttonSearchSourceId.Text = "...";
            this.buttonSearchSourceId.UseVisualStyleBackColor = true;
            this.buttonSearchSourceId.Click += new System.EventHandler(this.buttonSearchSourceId_Click);
            // 
            // buttonSearchErrorTextId
            // 
            this.buttonSearchErrorTextId.Location = new System.Drawing.Point(385, 180);
            this.buttonSearchErrorTextId.Name = "buttonSearchErrorTextId";
            this.buttonSearchErrorTextId.Size = new System.Drawing.Size(24, 22);
            this.buttonSearchErrorTextId.TabIndex = 19;
            this.buttonSearchErrorTextId.Text = "...";
            this.buttonSearchErrorTextId.UseVisualStyleBackColor = true;
            this.buttonSearchErrorTextId.Click += new System.EventHandler(this.buttonSearchErrorTextId_Click);
            // 
            // buttonSearchErrorType
            // 
            this.buttonSearchErrorType.Location = new System.Drawing.Point(385, 154);
            this.buttonSearchErrorType.Name = "buttonSearchErrorType";
            this.buttonSearchErrorType.Size = new System.Drawing.Size(24, 22);
            this.buttonSearchErrorType.TabIndex = 19;
            this.buttonSearchErrorType.Text = "...";
            this.buttonSearchErrorType.UseVisualStyleBackColor = true;
            this.buttonSearchErrorType.Click += new System.EventHandler(this.buttonSearchErrorType_Click);
            // 
            // labelSourceId
            // 
            this.labelSourceId.AutoSize = true;
            this.labelSourceId.Location = new System.Drawing.Point(422, 90);
            this.labelSourceId.Name = "labelSourceId";
            this.labelSourceId.Size = new System.Drawing.Size(52, 13);
            this.labelSourceId.TabIndex = 8;
            this.labelSourceId.Text = "Source id";
            // 
            // labelElseGroup
            // 
            this.labelElseGroup.AutoSize = true;
            this.labelElseGroup.Location = new System.Drawing.Point(422, 64);
            this.labelElseGroup.Name = "labelElseGroup";
            this.labelElseGroup.Size = new System.Drawing.Size(57, 13);
            this.labelElseGroup.TabIndex = 8;
            this.labelElseGroup.Text = "Else group";
            // 
            // textBoxSourceId
            // 
            this.textBoxSourceId.Location = new System.Drawing.Point(552, 86);
            this.textBoxSourceId.Name = "textBoxSourceId";
            this.textBoxSourceId.Size = new System.Drawing.Size(66, 20);
            this.textBoxSourceId.TabIndex = 12;
            this.textBoxSourceId.Text = "0";
            this.textBoxSourceId.TextChanged += new System.EventHandler(this.textBoxesConditionEditor_TextChanged);
            this.textBoxSourceId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxesConditionEditor_KeyPress);
            // 
            // textBoxElseGroup
            // 
            this.textBoxElseGroup.Location = new System.Drawing.Point(552, 60);
            this.textBoxElseGroup.Name = "textBoxElseGroup";
            this.textBoxElseGroup.Size = new System.Drawing.Size(66, 20);
            this.textBoxElseGroup.TabIndex = 12;
            this.textBoxElseGroup.Text = "0";
            this.textBoxElseGroup.TextChanged += new System.EventHandler(this.textBoxesConditionEditor_TextChanged);
            this.textBoxElseGroup.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxesConditionEditor_KeyPress);
            // 
            // textBoxErrorTextId
            // 
            this.textBoxErrorTextId.Location = new System.Drawing.Point(320, 181);
            this.textBoxErrorTextId.Name = "textBoxErrorTextId";
            this.textBoxErrorTextId.Size = new System.Drawing.Size(65, 20);
            this.textBoxErrorTextId.TabIndex = 18;
            this.textBoxErrorTextId.Text = "0";
            this.textBoxErrorTextId.TextChanged += new System.EventHandler(this.textBoxesConditionEditor_TextChanged);
            this.textBoxErrorTextId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxesConditionEditor_KeyPress);
            // 
            // textBoxErrorType
            // 
            this.textBoxErrorType.Location = new System.Drawing.Point(320, 155);
            this.textBoxErrorType.Name = "textBoxErrorType";
            this.textBoxErrorType.Size = new System.Drawing.Size(65, 20);
            this.textBoxErrorType.TabIndex = 18;
            this.textBoxErrorType.Text = "0";
            this.textBoxErrorType.TextChanged += new System.EventHandler(this.textBoxesConditionEditor_TextChanged);
            this.textBoxErrorType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxesConditionEditor_KeyPress);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(538, 89);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(10, 13);
            this.label20.TabIndex = 17;
            this.label20.Text = ":";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(538, 63);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(10, 13);
            this.label15.TabIndex = 17;
            this.label15.Text = ":";
            // 
            // lineSeparator1
            // 
            this.lineSeparator1.Location = new System.Drawing.Point(0, 117);
            this.lineSeparator1.MaximumSize = new System.Drawing.Size(2000, 2);
            this.lineSeparator1.MinimumSize = new System.Drawing.Size(0, 2);
            this.lineSeparator1.Name = "lineSeparator1";
            this.lineSeparator1.Size = new System.Drawing.Size(680, 2);
            this.lineSeparator1.TabIndex = 13;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(308, 184);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(10, 13);
            this.label18.TabIndex = 16;
            this.label18.Text = ":";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(308, 158);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(10, 13);
            this.label16.TabIndex = 16;
            this.label16.Text = ":";
            // 
            // tabPageSql
            // 
            this.tabPageSql.Controls.Add(this.richTextBoxSql);
            this.tabPageSql.Location = new System.Drawing.Point(4, 22);
            this.tabPageSql.Name = "tabPageSql";
            this.tabPageSql.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSql.Size = new System.Drawing.Size(656, 336);
            this.tabPageSql.TabIndex = 1;
            this.tabPageSql.Text = "Condition SQL";
            this.tabPageSql.UseVisualStyleBackColor = true;
            // 
            // richTextBoxSql
            // 
            this.richTextBoxSql.AutoScrollMinSize = new System.Drawing.Size(2, 14);
            this.richTextBoxSql.BackBrush = null;
            this.richTextBoxSql.CharHeight = 14;
            this.richTextBoxSql.CharWidth = 8;
            this.richTextBoxSql.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.richTextBoxSql.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.richTextBoxSql.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.richTextBoxSql.IsReplaceMode = false;
            this.richTextBoxSql.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxSql.Name = "richTextBoxSql";
            this.richTextBoxSql.Paddings = new System.Windows.Forms.Padding(0);
            this.richTextBoxSql.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.richTextBoxSql.Size = new System.Drawing.Size(656, 340);
            this.richTextBoxSql.TabIndex = 0;
            this.richTextBoxSql.Zoom = 100;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.checkBox1);
            this.tabPage1.Controls.Add(this.listViewConditions);
            this.tabPage1.Controls.Add(this.buttonDuplicateCondition);
            this.tabPage1.Controls.Add(this.buttonDeleteCondition);
            this.tabPage1.Controls.Add(this.buttonLoadCondition);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(656, 336);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Conditions overview";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(549, 255);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(101, 17);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Show basic info";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // listViewConditions
            // 
            this.listViewConditions.EnablePhaseHighlighting = false;
            this.listViewConditions.FullRowSelect = true;
            this.listViewConditions.Location = new System.Drawing.Point(0, 0);
            this.listViewConditions.MultiSelect = false;
            this.listViewConditions.Name = "listViewConditions";
            this.listViewConditions.Size = new System.Drawing.Size(656, 301);
            this.listViewConditions.TabIndex = 3;
            this.listViewConditions.UseCompatibleStateImageBehavior = false;
            this.listViewConditions.View = System.Windows.Forms.View.Details;
            this.listViewConditions.SelectedIndexChanged += new System.EventHandler(this.listViewConditions_SelectedIndexChanged);
            // 
            // buttonDuplicateCondition
            // 
            this.buttonDuplicateCondition.Enabled = false;
            this.buttonDuplicateCondition.Location = new System.Drawing.Point(8, 307);
            this.buttonDuplicateCondition.Name = "buttonDuplicateCondition";
            this.buttonDuplicateCondition.Size = new System.Drawing.Size(210, 23);
            this.buttonDuplicateCondition.TabIndex = 2;
            this.buttonDuplicateCondition.Text = "Duplicate selected condition";
            this.buttonDuplicateCondition.UseVisualStyleBackColor = true;
            this.buttonDuplicateCondition.Click += new System.EventHandler(this.buttonDuplicateCondition_Click);
            // 
            // buttonDeleteCondition
            // 
            this.buttonDeleteCondition.Enabled = false;
            this.buttonDeleteCondition.Location = new System.Drawing.Point(224, 307);
            this.buttonDeleteCondition.Name = "buttonDeleteCondition";
            this.buttonDeleteCondition.Size = new System.Drawing.Size(210, 23);
            this.buttonDeleteCondition.TabIndex = 2;
            this.buttonDeleteCondition.Text = "Delete selected condition";
            this.buttonDeleteCondition.UseVisualStyleBackColor = true;
            this.buttonDeleteCondition.Click += new System.EventHandler(this.buttonDeleteCondition_Click);
            // 
            // buttonLoadCondition
            // 
            this.buttonLoadCondition.Enabled = false;
            this.buttonLoadCondition.Location = new System.Drawing.Point(440, 307);
            this.buttonLoadCondition.Name = "buttonLoadCondition";
            this.buttonLoadCondition.Size = new System.Drawing.Size(210, 23);
            this.buttonLoadCondition.TabIndex = 1;
            this.buttonLoadCondition.Text = "Load and edit selected condition";
            this.buttonLoadCondition.UseVisualStyleBackColor = true;
            this.buttonLoadCondition.Click += new System.EventHandler(this.buttonLoadCondition_Click);
            // 
            // ConditionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(687, 382);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "ConditionForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Condition-Editor";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ConditionForm_KeyDown);
            this.panelPermanentTooltipSourceType.ResumeLayout(false);
            this.panelPermanentTooltipSourceType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPermanentTooltip)).EndInit();
            this.panelPermanentTooltipConditionType.ResumeLayout(false);
            this.panelPermanentTooltipConditionType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabPageEditing.ResumeLayout(false);
            this.tabPageEditing.PerformLayout();
            this.tabPageSql.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.richTextBoxSql)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxConditionSourceTypes;
        private System.Windows.Forms.ComboBox comboBoxConditionTypes;
        private System.Windows.Forms.Label labelSourceType;
        private System.Windows.Forms.Label labelConditionType;
        private System.Windows.Forms.Label labelSourceGroup;
        private System.Windows.Forms.Label labelSourceEntry;
        private System.Windows.Forms.Label labelConditionTarget;
        private System.Windows.Forms.ComboBox comboBoxConditionTarget;
        private System.Windows.Forms.TextBox textBoxSourceGroup;
        private System.Windows.Forms.TextBox textBoxSourceEntry;
        private Classes.CustomControls.LineSeparator lineSeparator1;
        private System.Windows.Forms.Label labelComment;
        private System.Windows.Forms.TextBox textBoxComment;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxCondValue1;
        private System.Windows.Forms.TextBox textBoxCondValue2;
        private System.Windows.Forms.TextBox textBoxCondValue3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button buttonSearchSourceGroup;
        private System.Windows.Forms.Button buttonSearchSourceEntry;
        private System.Windows.Forms.Panel panelPermanentTooltipSourceType;
        private System.Windows.Forms.Label labelPermanentTooltipSourceType;
        private System.Windows.Forms.Label labelPermanentTooltipSourceTypeValues;
        private System.Windows.Forms.PictureBox pictureBoxPermanentTooltip;
        private System.Windows.Forms.Panel panelPermanentTooltipConditionType;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonSearchConditionValue1;
        private System.Windows.Forms.Button buttonSearchConditionValue2;
        private System.Windows.Forms.Button buttonSearchConditionValue3;
        private System.Windows.Forms.Label labelCondValue1;
        private System.Windows.Forms.Label labelCondValue2;
        private System.Windows.Forms.Label labelCondValue3;
        private System.Windows.Forms.Label labelCondValue4;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBoxCondValue4;
        private System.Windows.Forms.Button buttonSearchConditionValue4;
        private System.Windows.Forms.Button buttonGenerateComment;
        private System.Windows.Forms.Button buttonGenerateSql;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageEditing;
        private System.Windows.Forms.TabPage tabPageSql;
        private FastColoredTextBoxNS.FastColoredTextBox richTextBoxSql;
        private System.Windows.Forms.Button buttonSaveCondition;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button buttonDuplicateCondition;
        private System.Windows.Forms.Button buttonDeleteCondition;
        private System.Windows.Forms.Button buttonLoadCondition;
        private Classes.CustomControls.ConditionListView listViewConditions;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBoxScriptName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label labelElseGroup;
        private System.Windows.Forms.TextBox textBoxElseGroup;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button buttonSearchErrorType;
        private System.Windows.Forms.TextBox textBoxErrorType;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button buttonSearchErrorTextId;
        private System.Windows.Forms.TextBox textBoxErrorTextId;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label labelSourceId;
        private System.Windows.Forms.TextBox textBoxSourceId;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Button buttonSearchSourceId;
    }
}