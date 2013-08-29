namespace SAI_Editor
{
    partial class SearchForEntryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchForEntryForm));
            this.textBoxEntryCriteria = new System.Windows.Forms.TextBox();
            this.labelEntrySearchInfo = new System.Windows.Forms.Label();
            this.checkBoxSearchForEntry = new System.Windows.Forms.CheckBox();
            this.groupBoxSearchInfo = new System.Windows.Forms.GroupBox();
            this.buttonSearchCreature = new System.Windows.Forms.Button();
            this.listViewEntryResults = new System.Windows.Forms.ListView();
            this.groupBoxSearchInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxEntryCriteria
            // 
            this.textBoxEntryCriteria.Location = new System.Drawing.Point(123, 19);
            this.textBoxEntryCriteria.Name = "textBoxEntryCriteria";
            this.textBoxEntryCriteria.Size = new System.Drawing.Size(206, 20);
            this.textBoxEntryCriteria.TabIndex = 0;
            // 
            // labelEntrySearchInfo
            // 
            this.labelEntrySearchInfo.AutoSize = true;
            this.labelEntrySearchInfo.Location = new System.Drawing.Point(17, 22);
            this.labelEntrySearchInfo.Name = "labelEntrySearchInfo";
            this.labelEntrySearchInfo.Size = new System.Drawing.Size(106, 13);
            this.labelEntrySearchInfo.TabIndex = 1;
            this.labelEntrySearchInfo.Text = "Creature name (part):";
            // 
            // checkBoxSearchForEntry
            // 
            this.checkBoxSearchForEntry.AutoSize = true;
            this.checkBoxSearchForEntry.Location = new System.Drawing.Point(20, 45);
            this.checkBoxSearchForEntry.Name = "checkBoxSearchForEntry";
            this.checkBoxSearchForEntry.Size = new System.Drawing.Size(138, 17);
            this.checkBoxSearchForEntry.TabIndex = 2;
            this.checkBoxSearchForEntry.Text = "Search for entry instead";
            this.checkBoxSearchForEntry.UseVisualStyleBackColor = true;
            this.checkBoxSearchForEntry.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // groupBoxSearchInfo
            // 
            this.groupBoxSearchInfo.Controls.Add(this.buttonSearchCreature);
            this.groupBoxSearchInfo.Controls.Add(this.textBoxEntryCriteria);
            this.groupBoxSearchInfo.Controls.Add(this.labelEntrySearchInfo);
            this.groupBoxSearchInfo.Controls.Add(this.checkBoxSearchForEntry);
            this.groupBoxSearchInfo.Location = new System.Drawing.Point(12, 12);
            this.groupBoxSearchInfo.Name = "groupBoxSearchInfo";
            this.groupBoxSearchInfo.Size = new System.Drawing.Size(351, 71);
            this.groupBoxSearchInfo.TabIndex = 4;
            this.groupBoxSearchInfo.TabStop = false;
            this.groupBoxSearchInfo.Text = "Search information";
            // 
            // buttonSearchCreature
            // 
            this.buttonSearchCreature.Location = new System.Drawing.Point(254, 42);
            this.buttonSearchCreature.Name = "buttonSearchCreature";
            this.buttonSearchCreature.Size = new System.Drawing.Size(75, 23);
            this.buttonSearchCreature.TabIndex = 3;
            this.buttonSearchCreature.Text = "Search";
            this.buttonSearchCreature.UseVisualStyleBackColor = true;
            this.buttonSearchCreature.Click += new System.EventHandler(this.buttonSearchCreature_Click);
            // 
            // listViewEntryResults
            // 
            this.listViewEntryResults.Location = new System.Drawing.Point(12, 89);
            this.listViewEntryResults.Name = "listViewEntryResults";
            this.listViewEntryResults.Size = new System.Drawing.Size(351, 258);
            this.listViewEntryResults.TabIndex = 5;
            this.listViewEntryResults.UseCompatibleStateImageBehavior = false;
            // 
            // SearchForEntryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 359);
            this.Controls.Add(this.listViewEntryResults);
            this.Controls.Add(this.groupBoxSearchInfo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SearchForEntryForm";
            this.Text = "Search for an entry";
            this.Load += new System.EventHandler(this.SearchForCreatureForm_Load);
            this.groupBoxSearchInfo.ResumeLayout(false);
            this.groupBoxSearchInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelEntrySearchInfo;
        private System.Windows.Forms.CheckBox checkBoxSearchForEntry;
        private System.Windows.Forms.GroupBox groupBoxSearchInfo;
        private System.Windows.Forms.ListView listViewEntryResults;
        private System.Windows.Forms.Button buttonSearchCreature;
        internal System.Windows.Forms.TextBox textBoxEntryCriteria;
    }
}