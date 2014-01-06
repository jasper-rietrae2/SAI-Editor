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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RevertQueryForm));
            this.listViewScripts = new System.Windows.Forms.ListView();
            this.columnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonExecuteSelectedScript = new System.Windows.Forms.Button();
            this.calenderScriptsToRevert = new System.Windows.Forms.MonthCalendar();
            this.listViewContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileWithToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDirectoryOfFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteRevertQueryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelWarningSettingOff = new System.Windows.Forms.Label();
            this.listViewContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewScripts
            // 
            this.listViewScripts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader});
            this.listViewScripts.FullRowSelect = true;
            this.listViewScripts.Location = new System.Drawing.Point(225, 18);
            this.listViewScripts.MultiSelect = false;
            this.listViewScripts.Name = "listViewScripts";
            this.listViewScripts.Size = new System.Drawing.Size(357, 162);
            this.listViewScripts.TabIndex = 2;
            this.listViewScripts.UseCompatibleStateImageBehavior = false;
            this.listViewScripts.View = System.Windows.Forms.View.Details;
            this.listViewScripts.SelectedIndexChanged += new System.EventHandler(this.listViewScripts_SelectedIndexChanged);
            this.listViewScripts.DoubleClick += new System.EventHandler(this.listViewScripts_DoubleClick);
            this.listViewScripts.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listViewScripts_MouseClick);
            // 
            // columnHeader
            // 
            this.columnHeader.Text = "Scripts";
            this.columnHeader.Width = 336;
            // 
            // buttonExecuteSelectedScript
            // 
            this.buttonExecuteSelectedScript.Location = new System.Drawing.Point(441, 186);
            this.buttonExecuteSelectedScript.Name = "buttonExecuteSelectedScript";
            this.buttonExecuteSelectedScript.Size = new System.Drawing.Size(141, 23);
            this.buttonExecuteSelectedScript.TabIndex = 3;
            this.buttonExecuteSelectedScript.Text = "Execute selected script";
            this.buttonExecuteSelectedScript.UseVisualStyleBackColor = true;
            this.buttonExecuteSelectedScript.Click += new System.EventHandler(this.buttonExecuteSelectedScript_Click);
            // 
            // calenderScriptsToRevert
            // 
            this.calenderScriptsToRevert.Location = new System.Drawing.Point(18, 18);
            this.calenderScriptsToRevert.MaxSelectionCount = 30;
            this.calenderScriptsToRevert.Name = "calenderScriptsToRevert";
            this.calenderScriptsToRevert.TabIndex = 4;
            this.calenderScriptsToRevert.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.calenderScriptsToRevert_DateChanged);
            // 
            // listViewContextMenu
            // 
            this.listViewContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileToolStripMenuItem,
            this.openFileWithToolStripMenuItem,
            this.openDirectoryOfFileToolStripMenuItem,
            this.deleteRevertQueryToolStripMenuItem});
            this.listViewContextMenu.Name = "listViewContextMenu";
            this.listViewContextMenu.Size = new System.Drawing.Size(187, 92);
            this.listViewContextMenu.Text = "derp";
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.openFileToolStripMenuItem.Text = "Open file";
            this.openFileToolStripMenuItem.Click += new System.EventHandler(this.openFileToolStripMenuItem_Click);
            // 
            // openFileWithToolStripMenuItem
            // 
            this.openFileWithToolStripMenuItem.Name = "openFileWithToolStripMenuItem";
            this.openFileWithToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.openFileWithToolStripMenuItem.Text = "Open file with...";
            this.openFileWithToolStripMenuItem.Click += new System.EventHandler(this.openFileWithToolStripMenuItem_Click);
            // 
            // openDirectoryOfFileToolStripMenuItem
            // 
            this.openDirectoryOfFileToolStripMenuItem.Name = "openDirectoryOfFileToolStripMenuItem";
            this.openDirectoryOfFileToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.openDirectoryOfFileToolStripMenuItem.Text = "Open directory of file";
            this.openDirectoryOfFileToolStripMenuItem.Click += new System.EventHandler(this.openDirectoryOfFileToolStripMenuItem_Click);
            // 
            // deleteRevertQueryToolStripMenuItem
            // 
            this.deleteRevertQueryToolStripMenuItem.Name = "deleteRevertQueryToolStripMenuItem";
            this.deleteRevertQueryToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.deleteRevertQueryToolStripMenuItem.Text = "Delete revert query";
            this.deleteRevertQueryToolStripMenuItem.Click += new System.EventHandler(this.deleteRevertQueryToolStripMenuItem_Click);
            // 
            // labelWarningSettingOff
            // 
            this.labelWarningSettingOff.AutoSize = true;
            this.labelWarningSettingOff.ForeColor = System.Drawing.Color.Red;
            this.labelWarningSettingOff.Location = new System.Drawing.Point(18, 191);
            this.labelWarningSettingOff.Name = "labelWarningSettingOff";
            this.labelWarningSettingOff.Size = new System.Drawing.Size(418, 13);
            this.labelWarningSettingOff.TabIndex = 5;
            this.labelWarningSettingOff.Text = "Warning: revert queries are not currently being generated because the setting is " +
    "not on.";
            this.labelWarningSettingOff.Visible = false;
            // 
            // RevertQueryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 217);
            this.Controls.Add(this.labelWarningSettingOff);
            this.Controls.Add(this.calenderScriptsToRevert);
            this.Controls.Add(this.buttonExecuteSelectedScript);
            this.Controls.Add(this.listViewScripts);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "RevertQueryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select a script to revert to";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RevertQueryForm_KeyDown);
            this.listViewContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewScripts;
        private System.Windows.Forms.Button buttonExecuteSelectedScript;
        private System.Windows.Forms.MonthCalendar calenderScriptsToRevert;
        private System.Windows.Forms.ColumnHeader columnHeader;
        private System.Windows.Forms.ContextMenuStrip listViewContextMenu;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileWithToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openDirectoryOfFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteRevertQueryToolStripMenuItem;
        private System.Windows.Forms.Label labelWarningSettingOff;
    }
}