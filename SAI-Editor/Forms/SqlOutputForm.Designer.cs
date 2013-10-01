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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SqlOutputForm));
            this.richTextBoxSqlOutput = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // richTextBoxSqlOutput
            // 
            this.richTextBoxSqlOutput.Location = new System.Drawing.Point(12, 12);
            this.richTextBoxSqlOutput.Name = "richTextBoxSqlOutput";
            this.richTextBoxSqlOutput.Size = new System.Drawing.Size(819, 322);
            this.richTextBoxSqlOutput.TabIndex = 0;
            this.richTextBoxSqlOutput.Text = "";
            // 
            // SqlOutputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 346);
            this.Controls.Add(this.richTextBoxSqlOutput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "SqlOutputForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Output";
            this.Load += new System.EventHandler(this.SqlOutputForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxSqlOutput;
    }
}