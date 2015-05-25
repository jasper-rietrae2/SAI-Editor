namespace SAI_Editor.Forms
{
    partial class SqlOutputForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SqlOutputForm));
            this.richTextBoxSqlOutput = new FastColoredTextBoxNS.FastColoredTextBox();
            this.buttonExecuteScript = new System.Windows.Forms.Button();
            this.buttonSaveToFile = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.buttonUploadToPastebin = new System.Windows.Forms.Button();
            this.buttonViewPastebins = new System.Windows.Forms.Button();
            this.buttonAppendToFile = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.richTextBoxSqlOutput)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBoxSqlOutput
            // 
            this.richTextBoxSqlOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxSqlOutput.AutoScrollMinSize = new System.Drawing.Size(27, 14);
            this.richTextBoxSqlOutput.BackBrush = null;
            this.richTextBoxSqlOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBoxSqlOutput.CharHeight = 14;
            this.richTextBoxSqlOutput.CharWidth = 8;
            this.richTextBoxSqlOutput.CommentPrefix = "--";
            this.richTextBoxSqlOutput.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.richTextBoxSqlOutput.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.richTextBoxSqlOutput.IsReplaceMode = false;
            this.richTextBoxSqlOutput.Language = FastColoredTextBoxNS.Language.SQL;
            this.richTextBoxSqlOutput.LeftBracket = '(';
            this.richTextBoxSqlOutput.Location = new System.Drawing.Point(12, 12);
            this.richTextBoxSqlOutput.Name = "richTextBoxSqlOutput";
            this.richTextBoxSqlOutput.Paddings = new System.Windows.Forms.Padding(0);
            this.richTextBoxSqlOutput.RightBracket = ')';
            this.richTextBoxSqlOutput.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.richTextBoxSqlOutput.Size = new System.Drawing.Size(819, 322);
            this.richTextBoxSqlOutput.TabIndex = 0;
            this.richTextBoxSqlOutput.Zoom = 100;
            // 
            // buttonExecuteScript
            // 
            this.buttonExecuteScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExecuteScript.Location = new System.Drawing.Point(746, 340);
            this.buttonExecuteScript.Name = "buttonExecuteScript";
            this.buttonExecuteScript.Size = new System.Drawing.Size(85, 23);
            this.buttonExecuteScript.TabIndex = 1;
            this.buttonExecuteScript.Text = "Execute SQL";
            this.buttonExecuteScript.UseVisualStyleBackColor = true;
            this.buttonExecuteScript.Click += new System.EventHandler(this.buttonExecuteScript_Click);
            // 
            // buttonSaveToFile
            // 
            this.buttonSaveToFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveToFile.Location = new System.Drawing.Point(655, 340);
            this.buttonSaveToFile.Name = "buttonSaveToFile";
            this.buttonSaveToFile.Size = new System.Drawing.Size(85, 23);
            this.buttonSaveToFile.TabIndex = 2;
            this.buttonSaveToFile.Text = "Save to file";
            this.buttonSaveToFile.UseVisualStyleBackColor = true;
            this.buttonSaveToFile.Click += new System.EventHandler(this.buttonSaveToFile_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog_FileOk);
            // 
            // buttonUploadToPastebin
            // 
            this.buttonUploadToPastebin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonUploadToPastebin.Location = new System.Drawing.Point(12, 340);
            this.buttonUploadToPastebin.Name = "buttonUploadToPastebin";
            this.buttonUploadToPastebin.Size = new System.Drawing.Size(154, 23);
            this.buttonUploadToPastebin.TabIndex = 3;
            this.buttonUploadToPastebin.Text = "Upload to Pastebin.com";
            this.buttonUploadToPastebin.UseVisualStyleBackColor = true;
            this.buttonUploadToPastebin.Click += new System.EventHandler(this.buttonUploadToPastebin_Click);
            // 
            // buttonViewPastebins
            // 
            this.buttonViewPastebins.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonViewPastebins.Location = new System.Drawing.Point(172, 340);
            this.buttonViewPastebins.Name = "buttonViewPastebins";
            this.buttonViewPastebins.Size = new System.Drawing.Size(136, 23);
            this.buttonViewPastebins.TabIndex = 4;
            this.buttonViewPastebins.Text = "View all my pastebins";
            this.buttonViewPastebins.UseVisualStyleBackColor = true;
            this.buttonViewPastebins.Click += new System.EventHandler(this.buttonViewPastebins_Click);
            // 
            // buttonAppendToFile
            // 
            this.buttonAppendToFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAppendToFile.Location = new System.Drawing.Point(564, 340);
            this.buttonAppendToFile.Name = "buttonAppendToFile";
            this.buttonAppendToFile.Size = new System.Drawing.Size(85, 23);
            this.buttonAppendToFile.TabIndex = 5;
            this.buttonAppendToFile.Text = "Append to file";
            this.buttonAppendToFile.UseVisualStyleBackColor = true;
            this.buttonAppendToFile.Click += new System.EventHandler(this.buttonAppendToFile_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            this.openFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog_FileOk);
            // 
            // SqlOutputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 368);
            this.Controls.Add(this.buttonAppendToFile);
            this.Controls.Add(this.buttonViewPastebins);
            this.Controls.Add(this.buttonUploadToPastebin);
            this.Controls.Add(this.buttonSaveToFile);
            this.Controls.Add(this.buttonExecuteScript);
            this.Controls.Add(this.richTextBoxSqlOutput);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "SqlOutputForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SQL output";
            this.Load += new System.EventHandler(this.SqlOutputForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SqlOutputForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.richTextBoxSqlOutput)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private FastColoredTextBoxNS.FastColoredTextBox richTextBoxSqlOutput;
        private System.Windows.Forms.Button buttonExecuteScript;
        private System.Windows.Forms.Button buttonSaveToFile;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Button buttonUploadToPastebin;
        private System.Windows.Forms.Button buttonViewPastebins;
        private System.Windows.Forms.Button buttonAppendToFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}