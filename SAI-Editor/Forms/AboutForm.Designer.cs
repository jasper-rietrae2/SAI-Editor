namespace SAI_Editor.Forms
{
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.labelCompanyName = new System.Windows.Forms.Label();
            this.buttonTrinitycore = new System.Windows.Forms.Button();
            this.buttonGithub = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBoxMitch = new System.Windows.Forms.PictureBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMitch)).BeginInit();
            this.SuspendLayout();
            // 
            // labelCompanyName
            // 
            this.labelCompanyName.Location = new System.Drawing.Point(15, 9);
            this.labelCompanyName.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.labelCompanyName.Name = "labelCompanyName";
            this.labelCompanyName.Size = new System.Drawing.Size(439, 17);
            this.labelCompanyName.TabIndex = 28;
            this.labelCompanyName.Text = "Written by Discover- and Mitch528";
            this.labelCompanyName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonTrinitycore
            // 
            this.buttonTrinitycore.Location = new System.Drawing.Point(135, 378);
            this.buttonTrinitycore.Name = "buttonTrinitycore";
            this.buttonTrinitycore.Size = new System.Drawing.Size(114, 23);
            this.buttonTrinitycore.TabIndex = 27;
            this.buttonTrinitycore.Text = "GitHub TrinityCore";
            this.buttonTrinitycore.UseVisualStyleBackColor = true;
            this.buttonTrinitycore.Click += new System.EventHandler(this.buttonTrinitycore_Click);
            // 
            // buttonGithub
            // 
            this.buttonGithub.Location = new System.Drawing.Point(15, 378);
            this.buttonGithub.Name = "buttonGithub";
            this.buttonGithub.Size = new System.Drawing.Size(114, 23);
            this.buttonGithub.TabIndex = 25;
            this.buttonGithub.Text = "GitHub Project";
            this.buttonGithub.UseVisualStyleBackColor = true;
            this.buttonGithub.Click += new System.EventHandler(this.buttonGithub_Click);
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.okButton.Location = new System.Drawing.Point(255, 378);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(62, 23);
            this.okButton.TabIndex = 30;
            this.okButton.Text = "&OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(323, 53);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(131, 78);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 31;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(324, 137);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(130, 129);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 31;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBoxDiscover_Click);
            // 
            // pictureBoxMitch
            // 
            this.pictureBoxMitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxMitch.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxMitch.Image")));
            this.pictureBoxMitch.Location = new System.Drawing.Point(324, 272);
            this.pictureBoxMitch.Name = "pictureBoxMitch";
            this.pictureBoxMitch.Size = new System.Drawing.Size(130, 129);
            this.pictureBoxMitch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxMitch.TabIndex = 31;
            this.pictureBoxMitch.TabStop = false;
            this.pictureBoxMitch.Click += new System.EventHandler(this.pictureBoxMitch_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Enabled = false;
            this.richTextBox1.Location = new System.Drawing.Point(15, 53);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(302, 319);
            this.richTextBox1.TabIndex = 32;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 411);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.pictureBoxMitch);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.labelCompanyName);
            this.Controls.Add(this.buttonTrinitycore);
            this.Controls.Add(this.buttonGithub);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About SAI-Editor";
            this.Load += new System.EventHandler(this.AboutForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AboutForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMitch)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelCompanyName;
        private System.Windows.Forms.Button buttonTrinitycore;
        private System.Windows.Forms.Button buttonGithub;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBoxMitch;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}