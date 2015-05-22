namespace SAI_Editor.Forms
{
    partial class ViewAllPastebinsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewAllPastebinsForm));
            this.listViewPastebins = new System.Windows.Forms.ListView();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderURL = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonVisitSelectedPastebin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listViewPastebins
            // 
            this.listViewPastebins.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderURL});
            this.listViewPastebins.FullRowSelect = true;
            this.listViewPastebins.HideSelection = false;
            this.listViewPastebins.Location = new System.Drawing.Point(12, 12);
            this.listViewPastebins.MultiSelect = false;
            this.listViewPastebins.Name = "listViewPastebins";
            this.listViewPastebins.Size = new System.Drawing.Size(471, 550);
            this.listViewPastebins.TabIndex = 0;
            this.listViewPastebins.UseCompatibleStateImageBehavior = false;
            this.listViewPastebins.View = System.Windows.Forms.View.Details;
            this.listViewPastebins.SelectedIndexChanged += new System.EventHandler(this.listViewPastebins_SelectedIndexChanged);
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Pastebin name";
            this.columnHeaderName.Width = 233;
            // 
            // columnHeaderURL
            // 
            this.columnHeaderURL.Text = "Pastebin URL";
            this.columnHeaderURL.Width = 221;
            // 
            // buttonVisitSelectedPastebin
            // 
            this.buttonVisitSelectedPastebin.Enabled = false;
            this.buttonVisitSelectedPastebin.Location = new System.Drawing.Point(12, 568);
            this.buttonVisitSelectedPastebin.Name = "buttonVisitSelectedPastebin";
            this.buttonVisitSelectedPastebin.Size = new System.Drawing.Size(471, 33);
            this.buttonVisitSelectedPastebin.TabIndex = 1;
            this.buttonVisitSelectedPastebin.Text = "Visit selected pastebin";
            this.buttonVisitSelectedPastebin.UseVisualStyleBackColor = true;
            this.buttonVisitSelectedPastebin.Click += new System.EventHandler(this.buttonVisitSelectedPastebin_Click);
            // 
            // ViewAllPastebinsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(495, 613);
            this.Controls.Add(this.buttonVisitSelectedPastebin);
            this.Controls.Add(this.listViewPastebins);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ViewAllPastebinsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "All your pastebin uploads";
            this.Load += new System.EventHandler(this.ViewAllPastebinsForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ViewAllPastebinsForm_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewPastebins;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderURL;
        private System.Windows.Forms.Button buttonVisitSelectedPastebin;
    }
}