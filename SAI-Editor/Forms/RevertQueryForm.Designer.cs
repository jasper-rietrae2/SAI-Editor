namespace SAI_Editor.Forms
{
    partial class RevertQueryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RevertQueryForm));
            this.listViewScripts = new System.Windows.Forms.ListView();
            this.columnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button1 = new System.Windows.Forms.Button();
            this.calenderScriptsToRevert = new System.Windows.Forms.MonthCalendar();
            this.SuspendLayout();
            // 
            // listViewScripts
            // 
            this.listViewScripts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader});
            this.listViewScripts.FullRowSelect = true;
            this.listViewScripts.Location = new System.Drawing.Point(201, 18);
            this.listViewScripts.Name = "listViewScripts";
            this.listViewScripts.Size = new System.Drawing.Size(381, 162);
            this.listViewScripts.TabIndex = 2;
            this.listViewScripts.UseCompatibleStateImageBehavior = false;
            this.listViewScripts.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader
            // 
            this.columnHeader.Text = "Scripts";
            this.columnHeader.Width = 359;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(433, 186);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(149, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Execute selected script";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // calenderScriptsToRevert
            // 
            this.calenderScriptsToRevert.Location = new System.Drawing.Point(18, 18);
            this.calenderScriptsToRevert.Name = "calenderScriptsToRevert";
            this.calenderScriptsToRevert.TabIndex = 4;
            // 
            // RevertQueryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 221);
            this.Controls.Add(this.calenderScriptsToRevert);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listViewScripts);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "RevertQueryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select a script to revert to";
            this.Load += new System.EventHandler(this.RevertQueryForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewScripts;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.MonthCalendar calenderScriptsToRevert;
        private System.Windows.Forms.ColumnHeader columnHeader;
    }
}