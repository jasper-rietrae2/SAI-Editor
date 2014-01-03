namespace Updater
{
    partial class Updater
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Updater));
            this.statusLabel = new System.Windows.Forms.Label();
            this.buttonUpdateToLatest = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.listBoxFilesToUpdate = new System.Windows.Forms.ListBox();
            this.textBoxChangelog = new System.Windows.Forms.RichTextBox();
            this.buttonCheckForUpdates = new System.Windows.Forms.Button();
            this.timerCheckForSaiEditorRunning = new System.Windows.Forms.Timer(this.components);
            this.timerStartSearchingOnLaunch = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.ForeColor = System.Drawing.Color.DarkGreen;
            this.statusLabel.Location = new System.Drawing.Point(7, 8);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(215, 25);
            this.statusLabel.TabIndex = 0;
            this.statusLabel.Text = "LOADING UPDATER";
            // 
            // buttonUpdateToLatest
            // 
            this.buttonUpdateToLatest.Enabled = false;
            this.buttonUpdateToLatest.Location = new System.Drawing.Point(328, 124);
            this.buttonUpdateToLatest.Name = "buttonUpdateToLatest";
            this.buttonUpdateToLatest.Size = new System.Drawing.Size(120, 23);
            this.buttonUpdateToLatest.TabIndex = 1;
            this.buttonUpdateToLatest.Text = "Update to latest";
            this.buttonUpdateToLatest.UseVisualStyleBackColor = true;
            this.buttonUpdateToLatest.Click += new System.EventHandler(this.buttonUpdateToLatest_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(138, 124);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(184, 23);
            this.progressBar.TabIndex = 4;
            // 
            // listBoxFilesToUpdate
            // 
            this.listBoxFilesToUpdate.FormattingEnabled = true;
            this.listBoxFilesToUpdate.Location = new System.Drawing.Point(12, 36);
            this.listBoxFilesToUpdate.Name = "listBoxFilesToUpdate";
            this.listBoxFilesToUpdate.Size = new System.Drawing.Size(436, 82);
            this.listBoxFilesToUpdate.TabIndex = 5;
            // 
            // textBoxChangelog
            // 
            this.textBoxChangelog.Location = new System.Drawing.Point(12, 153);
            this.textBoxChangelog.Name = "textBoxChangelog";
            this.textBoxChangelog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.textBoxChangelog.Size = new System.Drawing.Size(436, 126);
            this.textBoxChangelog.TabIndex = 6;
            this.textBoxChangelog.Text = "";
            this.textBoxChangelog.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.changelog_KeyPress);
            // 
            // buttonCheckForUpdates
            // 
            this.buttonCheckForUpdates.Enabled = false;
            this.buttonCheckForUpdates.Location = new System.Drawing.Point(12, 124);
            this.buttonCheckForUpdates.Name = "buttonCheckForUpdates";
            this.buttonCheckForUpdates.Size = new System.Drawing.Size(120, 23);
            this.buttonCheckForUpdates.TabIndex = 7;
            this.buttonCheckForUpdates.Text = "Check for updates";
            this.buttonCheckForUpdates.UseVisualStyleBackColor = true;
            this.buttonCheckForUpdates.Click += new System.EventHandler(this.buttonCheckForUpdates_Click);
            // 
            // timerCheckForSaiEditorRunning
            // 
            this.timerCheckForSaiEditorRunning.Enabled = true;
            this.timerCheckForSaiEditorRunning.Interval = 1000;
            this.timerCheckForSaiEditorRunning.Tick += new System.EventHandler(this.timerCheckForSaiEditorRunning_Tick);
            // 
            // timerStartSearchingOnLaunch
            // 
            this.timerStartSearchingOnLaunch.Enabled = true;
            this.timerStartSearchingOnLaunch.Interval = 1000;
            this.timerStartSearchingOnLaunch.Tick += new System.EventHandler(this.timerStartSearchingOnLaunch_Tick);
            // 
            // Updater
            // 
            this.AcceptButton = this.buttonUpdateToLatest;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 291);
            this.Controls.Add(this.buttonCheckForUpdates);
            this.Controls.Add(this.textBoxChangelog);
            this.Controls.Add(this.listBoxFilesToUpdate);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.buttonUpdateToLatest);
            this.Controls.Add(this.statusLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Updater";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SAI-Editor: Updater";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Updater_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Button buttonUpdateToLatest;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ListBox listBoxFilesToUpdate;
        private System.Windows.Forms.RichTextBox textBoxChangelog;
        private System.Windows.Forms.Button buttonCheckForUpdates;
        private System.Windows.Forms.Timer timerCheckForSaiEditorRunning;
        private System.Windows.Forms.Timer timerStartSearchingOnLaunch;

    }
}