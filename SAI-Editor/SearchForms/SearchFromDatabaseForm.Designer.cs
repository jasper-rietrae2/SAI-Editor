namespace SAI_Editor
{
    partial class SearchFromDatabaseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchFromDatabaseForm));
            this.listViewEntryResults = new System.Windows.Forms.ListView();
            this.groupBoxSearchInfo = new System.Windows.Forms.GroupBox();
            this.buttonStopSearching = new System.Windows.Forms.Button();
            this.checkBoxFieldContainsCriteria = new System.Windows.Forms.CheckBox();
            this.textBoxCriteria = new System.Windows.Forms.TextBox();
            this.comboBoxSearchType = new System.Windows.Forms.ComboBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.groupBoxSearchInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewEntryResults
            // 
            this.listViewEntryResults.FullRowSelect = true;
            this.listViewEntryResults.Location = new System.Drawing.Point(12, 99);
            this.listViewEntryResults.MultiSelect = false;
            this.listViewEntryResults.Name = "listViewEntryResults";
            this.listViewEntryResults.Size = new System.Drawing.Size(351, 317);
            this.listViewEntryResults.TabIndex = 13;
            this.listViewEntryResults.UseCompatibleStateImageBehavior = false;
            this.listViewEntryResults.View = System.Windows.Forms.View.Details;
            this.listViewEntryResults.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewEntryResults_ColumnClick);
            this.listViewEntryResults.DoubleClick += new System.EventHandler(this.listViewEntryResults_DoubleClick);
            // 
            // groupBoxSearchInfo
            // 
            this.groupBoxSearchInfo.Controls.Add(this.buttonStopSearching);
            this.groupBoxSearchInfo.Controls.Add(this.checkBoxFieldContainsCriteria);
            this.groupBoxSearchInfo.Controls.Add(this.textBoxCriteria);
            this.groupBoxSearchInfo.Controls.Add(this.comboBoxSearchType);
            this.groupBoxSearchInfo.Controls.Add(this.buttonSearch);
            this.groupBoxSearchInfo.Location = new System.Drawing.Point(12, 12);
            this.groupBoxSearchInfo.Name = "groupBoxSearchInfo";
            this.groupBoxSearchInfo.Size = new System.Drawing.Size(351, 81);
            this.groupBoxSearchInfo.TabIndex = 12;
            this.groupBoxSearchInfo.TabStop = false;
            this.groupBoxSearchInfo.Text = "Search information";
            // 
            // buttonStopSearching
            // 
            this.buttonStopSearching.Enabled = false;
            this.buttonStopSearching.Location = new System.Drawing.Point(173, 46);
            this.buttonStopSearching.Name = "buttonStopSearching";
            this.buttonStopSearching.Size = new System.Drawing.Size(75, 23);
            this.buttonStopSearching.TabIndex = 10;
            this.buttonStopSearching.Text = "Stop";
            this.buttonStopSearching.UseVisualStyleBackColor = true;
            this.buttonStopSearching.Click += new System.EventHandler(this.buttonStopSearchResults_Click);
            // 
            // checkBoxFieldContainsCriteria
            // 
            this.checkBoxFieldContainsCriteria.AutoSize = true;
            this.checkBoxFieldContainsCriteria.Checked = true;
            this.checkBoxFieldContainsCriteria.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFieldContainsCriteria.Location = new System.Drawing.Point(19, 52);
            this.checkBoxFieldContainsCriteria.Name = "checkBoxFieldContainsCriteria";
            this.checkBoxFieldContainsCriteria.Size = new System.Drawing.Size(125, 17);
            this.checkBoxFieldContainsCriteria.TabIndex = 9;
            this.checkBoxFieldContainsCriteria.Text = "Field contains criteria";
            this.checkBoxFieldContainsCriteria.UseVisualStyleBackColor = true;
            // 
            // textBoxCriteria
            // 
            this.textBoxCriteria.Location = new System.Drawing.Point(139, 19);
            this.textBoxCriteria.Name = "textBoxCriteria";
            this.textBoxCriteria.Size = new System.Drawing.Size(190, 20);
            this.textBoxCriteria.TabIndex = 8;
            // 
            // comboBoxSearchType
            // 
            this.comboBoxSearchType.FormattingEnabled = true;
            this.comboBoxSearchType.Items.AddRange(new object[] {
            "Spell entry",
            "Spell name"});
            this.comboBoxSearchType.Location = new System.Drawing.Point(20, 19);
            this.comboBoxSearchType.Name = "comboBoxSearchType";
            this.comboBoxSearchType.Size = new System.Drawing.Size(113, 21);
            this.comboBoxSearchType.TabIndex = 7;
            this.comboBoxSearchType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBoxSearchType_KeyPress);
            // 
            // buttonSearch
            // 
            this.buttonSearch.Location = new System.Drawing.Point(254, 46);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(75, 23);
            this.buttonSearch.TabIndex = 3;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // SearchFromDatabaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 428);
            this.Controls.Add(this.listViewEntryResults);
            this.Controls.Add(this.groupBoxSearchInfo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "SearchFromDatabaseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Search for a spell";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SearchFromDatabaseForm_FormClosing);
            this.Load += new System.EventHandler(this.SearchFromDatabaseForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchFromDatabaseForm_KeyDown);
            this.groupBoxSearchInfo.ResumeLayout(false);
            this.groupBoxSearchInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewEntryResults;
        private System.Windows.Forms.GroupBox groupBoxSearchInfo;
        private System.Windows.Forms.Button buttonStopSearching;
        private System.Windows.Forms.CheckBox checkBoxFieldContainsCriteria;
        private System.Windows.Forms.TextBox textBoxCriteria;
        private System.Windows.Forms.ComboBox comboBoxSearchType;
        private System.Windows.Forms.Button buttonSearch;

    }
}