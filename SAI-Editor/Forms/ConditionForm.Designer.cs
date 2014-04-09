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
            this.lineSeparator1 = new SAI_Editor.Classes.LineSeparator();
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
            this.comboBoxConditionSourceTypes.Location = new System.Drawing.Point(119, 13);
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
            this.comboBoxConditionTypes.Location = new System.Drawing.Point(119, 119);
            this.comboBoxConditionTypes.Name = "comboBoxConditionTypes";
            this.comboBoxConditionTypes.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.comboBoxConditionTypes.Size = new System.Drawing.Size(290, 21);
            this.comboBoxConditionTypes.TabIndex = 1;
            // 
            // labelSourceType
            // 
            this.labelSourceType.AutoSize = true;
            this.labelSourceType.Location = new System.Drawing.Point(12, 16);
            this.labelSourceType.Name = "labelSourceType";
            this.labelSourceType.Size = new System.Drawing.Size(67, 13);
            this.labelSourceType.TabIndex = 2;
            this.labelSourceType.Text = "Source type:";
            // 
            // labelConditionType
            // 
            this.labelConditionType.AutoSize = true;
            this.labelConditionType.Location = new System.Drawing.Point(12, 122);
            this.labelConditionType.Name = "labelConditionType";
            this.labelConditionType.Size = new System.Drawing.Size(77, 13);
            this.labelConditionType.TabIndex = 3;
            this.labelConditionType.Text = "Condition type:";
            // 
            // labelCondValue1
            // 
            this.labelCondValue1.AutoSize = true;
            this.labelCondValue1.Location = new System.Drawing.Point(12, 152);
            this.labelCondValue1.Name = "labelCondValue1";
            this.labelCondValue1.Size = new System.Drawing.Size(75, 13);
            this.labelCondValue1.TabIndex = 4;
            this.labelCondValue1.Text = "lblCondValue1";
            // 
            // labelCondValue2
            // 
            this.labelCondValue2.AutoSize = true;
            this.labelCondValue2.Location = new System.Drawing.Point(391, 210);
            this.labelCondValue2.Name = "labelCondValue2";
            this.labelCondValue2.Size = new System.Drawing.Size(75, 13);
            this.labelCondValue2.TabIndex = 5;
            this.labelCondValue2.Text = "lblCondValue2";
            // 
            // labelCondValue3
            // 
            this.labelCondValue3.AutoSize = true;
            this.labelCondValue3.Location = new System.Drawing.Point(391, 223);
            this.labelCondValue3.Name = "labelCondValue3";
            this.labelCondValue3.Size = new System.Drawing.Size(75, 13);
            this.labelCondValue3.TabIndex = 6;
            this.labelCondValue3.Text = "lblCondValue3";
            // 
            // labelSourceGroup
            // 
            this.labelSourceGroup.AutoSize = true;
            this.labelSourceGroup.Location = new System.Drawing.Point(422, 16);
            this.labelSourceGroup.Name = "labelSourceGroup";
            this.labelSourceGroup.Size = new System.Drawing.Size(80, 13);
            this.labelSourceGroup.TabIndex = 7;
            this.labelSourceGroup.Text = "lblSourceGroup";
            // 
            // labelSourceEntry
            // 
            this.labelSourceEntry.AutoSize = true;
            this.labelSourceEntry.Location = new System.Drawing.Point(422, 43);
            this.labelSourceEntry.Name = "labelSourceEntry";
            this.labelSourceEntry.Size = new System.Drawing.Size(75, 13);
            this.labelSourceEntry.TabIndex = 8;
            this.labelSourceEntry.Text = "lblSourceEntry";
            // 
            // labelConditionTarget
            // 
            this.labelConditionTarget.AutoSize = true;
            this.labelConditionTarget.Location = new System.Drawing.Point(12, 43);
            this.labelConditionTarget.Name = "labelConditionTarget";
            this.labelConditionTarget.Size = new System.Drawing.Size(84, 13);
            this.labelConditionTarget.TabIndex = 9;
            this.labelConditionTarget.Text = "Condition target:";
            // 
            // comboBoxConditionTarget
            // 
            this.comboBoxConditionTarget.FormattingEnabled = true;
            this.comboBoxConditionTarget.Location = new System.Drawing.Point(119, 40);
            this.comboBoxConditionTarget.Name = "comboBoxConditionTarget";
            this.comboBoxConditionTarget.Size = new System.Drawing.Size(290, 21);
            this.comboBoxConditionTarget.TabIndex = 10;
            this.comboBoxConditionTarget.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBoxConditionTarget_KeyPress);
            // 
            // textBoxSourceGroup
            // 
            this.textBoxSourceGroup.Location = new System.Drawing.Point(552, 13);
            this.textBoxSourceGroup.Name = "textBoxSourceGroup";
            this.textBoxSourceGroup.Size = new System.Drawing.Size(90, 20);
            this.textBoxSourceGroup.TabIndex = 11;
            // 
            // textBoxSourceEntry
            // 
            this.textBoxSourceEntry.Location = new System.Drawing.Point(552, 40);
            this.textBoxSourceEntry.Name = "textBoxSourceEntry";
            this.textBoxSourceEntry.Size = new System.Drawing.Size(90, 20);
            this.textBoxSourceEntry.TabIndex = 12;
            // 
            // labelComment
            // 
            this.labelComment.AutoSize = true;
            this.labelComment.Location = new System.Drawing.Point(12, 70);
            this.labelComment.Name = "labelComment";
            this.labelComment.Size = new System.Drawing.Size(54, 13);
            this.labelComment.TabIndex = 14;
            this.labelComment.Text = "Comment:";
            // 
            // textBoxComment
            // 
            this.textBoxComment.Location = new System.Drawing.Point(119, 67);
            this.textBoxComment.Name = "textBoxComment";
            this.textBoxComment.Size = new System.Drawing.Size(523, 20);
            this.textBoxComment.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(537, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(10, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = ":";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(537, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(10, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = ":";
            // 
            // lineSeparator1
            // 
            this.lineSeparator1.Location = new System.Drawing.Point(-1, 102);
            this.lineSeparator1.MaximumSize = new System.Drawing.Size(2000, 2);
            this.lineSeparator1.MinimumSize = new System.Drawing.Size(0, 2);
            this.lineSeparator1.Name = "lineSeparator1";
            this.lineSeparator1.Size = new System.Drawing.Size(680, 2);
            this.lineSeparator1.TabIndex = 13;
            // 
            // ConditionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 377);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxComment);
            this.Controls.Add(this.labelComment);
            this.Controls.Add(this.lineSeparator1);
            this.Controls.Add(this.textBoxSourceEntry);
            this.Controls.Add(this.textBoxSourceGroup);
            this.Controls.Add(this.comboBoxConditionTarget);
            this.Controls.Add(this.labelConditionTarget);
            this.Controls.Add(this.labelSourceEntry);
            this.Controls.Add(this.labelSourceGroup);
            this.Controls.Add(this.labelCondValue3);
            this.Controls.Add(this.labelCondValue2);
            this.Controls.Add(this.labelCondValue1);
            this.Controls.Add(this.labelConditionType);
            this.Controls.Add(this.labelSourceType);
            this.Controls.Add(this.comboBoxConditionTypes);
            this.Controls.Add(this.comboBoxConditionSourceTypes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConditionForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Condition Editor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxConditionSourceTypes;
        private System.Windows.Forms.ComboBox comboBoxConditionTypes;
        private System.Windows.Forms.Label labelSourceType;
        private System.Windows.Forms.Label labelConditionType;
        private System.Windows.Forms.Label labelCondValue1;
        private System.Windows.Forms.Label labelCondValue2;
        private System.Windows.Forms.Label labelCondValue3;
        private System.Windows.Forms.Label labelSourceGroup;
        private System.Windows.Forms.Label labelSourceEntry;
        private System.Windows.Forms.Label labelConditionTarget;
        private System.Windows.Forms.ComboBox comboBoxConditionTarget;
        private System.Windows.Forms.TextBox textBoxSourceGroup;
        private System.Windows.Forms.TextBox textBoxSourceEntry;
        private Classes.LineSeparator lineSeparator1;
        private System.Windows.Forms.Label labelComment;
        private System.Windows.Forms.TextBox textBoxComment;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}