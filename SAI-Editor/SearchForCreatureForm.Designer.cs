namespace SAI_Editor
{
    partial class SearchForCreatureForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchForCreatureForm));
            this.textBoxCreatureCriteria = new System.Windows.Forms.TextBox();
            this.labelCreatureSearchInfo = new System.Windows.Forms.Label();
            this.checkBoxSearchForCreatureEntry = new System.Windows.Forms.CheckBox();
            this.groupBoxCreatureSearchInfo = new System.Windows.Forms.GroupBox();
            this.buttonSearchCreature = new System.Windows.Forms.Button();
            this.listViewCreatureResults = new System.Windows.Forms.ListView();
            this.groupBoxCreatureSearchInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxCreatureCriteria
            // 
            this.textBoxCreatureCriteria.Location = new System.Drawing.Point(123, 19);
            this.textBoxCreatureCriteria.Name = "textBoxCreatureCriteria";
            this.textBoxCreatureCriteria.Size = new System.Drawing.Size(206, 20);
            this.textBoxCreatureCriteria.TabIndex = 0;
            this.textBoxCreatureCriteria.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // labelCreatureSearchInfo
            // 
            this.labelCreatureSearchInfo.AutoSize = true;
            this.labelCreatureSearchInfo.Location = new System.Drawing.Point(17, 22);
            this.labelCreatureSearchInfo.Name = "labelCreatureSearchInfo";
            this.labelCreatureSearchInfo.Size = new System.Drawing.Size(106, 13);
            this.labelCreatureSearchInfo.TabIndex = 1;
            this.labelCreatureSearchInfo.Text = "Creature name (part):";
            // 
            // checkBoxSearchForCreatureEntry
            // 
            this.checkBoxSearchForCreatureEntry.AutoSize = true;
            this.checkBoxSearchForCreatureEntry.Location = new System.Drawing.Point(20, 45);
            this.checkBoxSearchForCreatureEntry.Name = "checkBoxSearchForCreatureEntry";
            this.checkBoxSearchForCreatureEntry.Size = new System.Drawing.Size(180, 17);
            this.checkBoxSearchForCreatureEntry.TabIndex = 2;
            this.checkBoxSearchForCreatureEntry.Text = "Search for creature entry instead";
            this.checkBoxSearchForCreatureEntry.UseVisualStyleBackColor = true;
            this.checkBoxSearchForCreatureEntry.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // groupBoxCreatureSearchInfo
            // 
            this.groupBoxCreatureSearchInfo.Controls.Add(this.buttonSearchCreature);
            this.groupBoxCreatureSearchInfo.Controls.Add(this.textBoxCreatureCriteria);
            this.groupBoxCreatureSearchInfo.Controls.Add(this.labelCreatureSearchInfo);
            this.groupBoxCreatureSearchInfo.Controls.Add(this.checkBoxSearchForCreatureEntry);
            this.groupBoxCreatureSearchInfo.Location = new System.Drawing.Point(12, 12);
            this.groupBoxCreatureSearchInfo.Name = "groupBoxCreatureSearchInfo";
            this.groupBoxCreatureSearchInfo.Size = new System.Drawing.Size(351, 71);
            this.groupBoxCreatureSearchInfo.TabIndex = 4;
            this.groupBoxCreatureSearchInfo.TabStop = false;
            this.groupBoxCreatureSearchInfo.Text = "Search information";
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
            // listViewCreatureResults
            // 
            this.listViewCreatureResults.Location = new System.Drawing.Point(12, 89);
            this.listViewCreatureResults.Name = "listViewCreatureResults";
            this.listViewCreatureResults.Size = new System.Drawing.Size(351, 258);
            this.listViewCreatureResults.TabIndex = 5;
            this.listViewCreatureResults.UseCompatibleStateImageBehavior = false;
            // 
            // SearchForCreatureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 359);
            this.Controls.Add(this.listViewCreatureResults);
            this.Controls.Add(this.groupBoxCreatureSearchInfo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SearchForCreatureForm";
            this.Text = "Search for a creature";
            this.Load += new System.EventHandler(this.SearchForCreatureForm_Load);
            this.groupBoxCreatureSearchInfo.ResumeLayout(false);
            this.groupBoxCreatureSearchInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelCreatureSearchInfo;
        private System.Windows.Forms.CheckBox checkBoxSearchForCreatureEntry;
        private System.Windows.Forms.GroupBox groupBoxCreatureSearchInfo;
        private System.Windows.Forms.ListView listViewCreatureResults;
        private System.Windows.Forms.Button buttonSearchCreature;
        internal System.Windows.Forms.TextBox textBoxCreatureCriteria;
    }
}