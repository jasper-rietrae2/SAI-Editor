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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
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
            "SOURCE_TYPE_SPELL_PROC",
            "SOURCE_TYPE_PHASE_DEFINITION"});
            this.comboBoxConditionSourceTypes.Location = new System.Drawing.Point(156, 6);
            this.comboBoxConditionSourceTypes.Name = "comboBoxConditionSourceTypes";
            this.comboBoxConditionSourceTypes.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.comboBoxConditionSourceTypes.Size = new System.Drawing.Size(290, 21);
            this.comboBoxConditionSourceTypes.TabIndex = 0;
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
            this.comboBoxConditionTypes.Location = new System.Drawing.Point(156, 33);
            this.comboBoxConditionTypes.Name = "comboBoxConditionTypes";
            this.comboBoxConditionTypes.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.comboBoxConditionTypes.Size = new System.Drawing.Size(203, 21);
            this.comboBoxConditionTypes.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Source type or reference id:\r\n";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Condition type:";
            // 
            // ConditionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 377);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}