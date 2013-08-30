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
            this.groupBoxSearchInfo = new System.Windows.Forms.GroupBox();
            this.comboBoxSearchType = new System.Windows.Forms.ComboBox();
            this.checkBoxHasAiName = new System.Windows.Forms.CheckBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.listViewEntryResults = new System.Windows.Forms.ListView();
            this.textBoxCriteria = new System.Windows.Forms.TextBox();
            this.checkBoxUseSqlLike = new System.Windows.Forms.CheckBox();
            this.buttonClearSearchResults = new System.Windows.Forms.Button();
            this.groupBoxSearchInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxSearchInfo
            // 
            this.groupBoxSearchInfo.Controls.Add(this.buttonClearSearchResults);
            this.groupBoxSearchInfo.Controls.Add(this.checkBoxUseSqlLike);
            this.groupBoxSearchInfo.Controls.Add(this.textBoxCriteria);
            this.groupBoxSearchInfo.Controls.Add(this.comboBoxSearchType);
            this.groupBoxSearchInfo.Controls.Add(this.checkBoxHasAiName);
            this.groupBoxSearchInfo.Controls.Add(this.buttonSearch);
            this.groupBoxSearchInfo.Location = new System.Drawing.Point(12, 12);
            this.groupBoxSearchInfo.Name = "groupBoxSearchInfo";
            this.groupBoxSearchInfo.Size = new System.Drawing.Size(351, 103);
            this.groupBoxSearchInfo.TabIndex = 4;
            this.groupBoxSearchInfo.TabStop = false;
            this.groupBoxSearchInfo.Text = "Search information";
            // 
            // comboBoxSearchType
            // 
            this.comboBoxSearchType.FormattingEnabled = true;
            this.comboBoxSearchType.Items.AddRange(new object[] {
            "Creature name",
            "Creature entry",
            "Creature guid",
            "Gameobject name",
            "Gameobject entry",
            "Gameobject guid"});
            this.comboBoxSearchType.Location = new System.Drawing.Point(20, 19);
            this.comboBoxSearchType.Name = "comboBoxSearchType";
            this.comboBoxSearchType.Size = new System.Drawing.Size(113, 21);
            this.comboBoxSearchType.TabIndex = 7;
            this.comboBoxSearchType.SelectedIndexChanged += new System.EventHandler(this.comboBoxSearchType_SelectedIndexChanged);
            // 
            // checkBoxHasAiName
            // 
            this.checkBoxHasAiName.AutoSize = true;
            this.checkBoxHasAiName.Location = new System.Drawing.Point(19, 50);
            this.checkBoxHasAiName.Name = "checkBoxHasAiName";
            this.checkBoxHasAiName.Size = new System.Drawing.Size(114, 17);
            this.checkBoxHasAiName.TabIndex = 6;
            this.checkBoxHasAiName.Text = "AIName is SmartAI";
            this.checkBoxHasAiName.UseVisualStyleBackColor = true;
            // 
            // buttonSearch
            // 
            this.buttonSearch.Location = new System.Drawing.Point(254, 67);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(75, 23);
            this.buttonSearch.TabIndex = 3;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // listViewEntryResults
            // 
            this.listViewEntryResults.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewEntryResults.Location = new System.Drawing.Point(12, 121);
            this.listViewEntryResults.Name = "listViewEntryResults";
            this.listViewEntryResults.Size = new System.Drawing.Size(351, 295);
            this.listViewEntryResults.TabIndex = 5;
            this.listViewEntryResults.UseCompatibleStateImageBehavior = false;
            // 
            // textBoxCriteria
            // 
            this.textBoxCriteria.Location = new System.Drawing.Point(139, 19);
            this.textBoxCriteria.Multiline = true;
            this.textBoxCriteria.Name = "textBoxCriteria";
            this.textBoxCriteria.Size = new System.Drawing.Size(190, 21);
            this.textBoxCriteria.TabIndex = 8;
            // 
            // checkBoxUseSqlLike
            // 
            this.checkBoxUseSqlLike.AutoSize = true;
            this.checkBoxUseSqlLike.Enabled = false;
            this.checkBoxUseSqlLike.Location = new System.Drawing.Point(19, 73);
            this.checkBoxUseSqlLike.Name = "checkBoxUseSqlLike";
            this.checkBoxUseSqlLike.Size = new System.Drawing.Size(142, 17);
            this.checkBoxUseSqlLike.TabIndex = 9;
            this.checkBoxUseSqlLike.Text = "Use SQL \'LIKE\' keyword";
            this.checkBoxUseSqlLike.UseVisualStyleBackColor = true;
            // 
            // buttonClearSearchResults
            // 
            this.buttonClearSearchResults.Location = new System.Drawing.Point(173, 67);
            this.buttonClearSearchResults.Name = "buttonClearSearchResults";
            this.buttonClearSearchResults.Size = new System.Drawing.Size(75, 23);
            this.buttonClearSearchResults.TabIndex = 10;
            this.buttonClearSearchResults.Text = "Clear";
            this.buttonClearSearchResults.UseVisualStyleBackColor = true;
            this.buttonClearSearchResults.Click += new System.EventHandler(this.buttonClearSearchResults_Click);
            // 
            // SearchForEntryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 428);
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

        private System.Windows.Forms.GroupBox groupBoxSearchInfo;
        private System.Windows.Forms.ListView listViewEntryResults;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.CheckBox checkBoxHasAiName;
        private System.Windows.Forms.ComboBox comboBoxSearchType;
        private System.Windows.Forms.TextBox textBoxCriteria;
        private System.Windows.Forms.CheckBox checkBoxUseSqlLike;
        private System.Windows.Forms.Button buttonClearSearchResults;
    }
}