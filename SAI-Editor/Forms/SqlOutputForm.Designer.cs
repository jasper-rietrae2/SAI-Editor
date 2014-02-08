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
            ((System.ComponentModel.ISupportInitialize)(this.richTextBoxSqlOutput)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBoxSqlOutput
            // 
            this.richTextBoxSqlOutput.AutoScrollMinSize = new System.Drawing.Size(27, 14);
            this.richTextBoxSqlOutput.BackBrush = null;
            this.richTextBoxSqlOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBoxSqlOutput.CharHeight = 14;
            this.richTextBoxSqlOutput.CharWidth = 8;
            this.richTextBoxSqlOutput.CommentPrefix = "--";
            this.richTextBoxSqlOutput.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.richTextBoxSqlOutput.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.richTextBoxSqlOutput.Font = new System.Drawing.Font("Courier New", 9.75F);
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
            this.buttonExecuteScript.Location = new System.Drawing.Point(746, 340);
            this.buttonExecuteScript.Name = "buttonExecuteScript";
            this.buttonExecuteScript.Size = new System.Drawing.Size(85, 23);
            this.buttonExecuteScript.TabIndex = 1;
            this.buttonExecuteScript.Text = "Execute script";
            this.buttonExecuteScript.UseVisualStyleBackColor = true;
            this.buttonExecuteScript.Click += new System.EventHandler(this.buttonExecuteScript_Click);
            // 
            // buttonSaveToFile
            // 
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
            // SqlOutputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 368);
            this.Controls.Add(this.buttonSaveToFile);
            this.Controls.Add(this.buttonExecuteScript);
            this.Controls.Add(this.richTextBoxSqlOutput);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "SqlOutputForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sql output";
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
    }
}