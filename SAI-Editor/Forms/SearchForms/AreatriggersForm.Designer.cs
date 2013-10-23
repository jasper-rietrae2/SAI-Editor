namespace SAI_Editor
{
    partial class AreatriggersForm
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
            this.zoomPanImageBox1 = new Smallwisdom.Windows.Forms.ZoomPanImageBox();
            this.SuspendLayout();
            // 
            // zoomPanImageBox1
            // 
            this.zoomPanImageBox1.Image = null;
            this.zoomPanImageBox1.Location = new System.Drawing.Point(12, 12);
            this.zoomPanImageBox1.Name = "zoomPanImageBox1";
            this.zoomPanImageBox1.Size = new System.Drawing.Size(366, 313);
            this.zoomPanImageBox1.TabIndex = 0;
            // 
            // AreatriggersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 337);
            this.Controls.Add(this.zoomPanImageBox1);
            this.Name = "AreatriggersForm";
            this.Text = "AreatriggersForm";
            this.ResumeLayout(false);

        }

        #endregion

        private Smallwisdom.Windows.Forms.ZoomPanImageBox zoomPanImageBox1;

    }
}