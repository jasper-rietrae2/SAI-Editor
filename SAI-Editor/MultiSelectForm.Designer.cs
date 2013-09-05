namespace SAI_Editor
{
    partial class MultiSelectForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultiSelectForm));
            this.listViewSelectableItems = new System.Windows.Forms.ListView();
            this.buttonContinue = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listViewSelectableItems
            // 
            this.listViewSelectableItems.CheckBoxes = true;
            this.listViewSelectableItems.Location = new System.Drawing.Point(12, 12);
            this.listViewSelectableItems.Name = "listViewSelectableItems";
            this.listViewSelectableItems.Size = new System.Drawing.Size(220, 176);
            this.listViewSelectableItems.TabIndex = 1;
            this.listViewSelectableItems.UseCompatibleStateImageBehavior = false;
            // 
            // buttonContinue
            // 
            this.buttonContinue.Location = new System.Drawing.Point(12, 194);
            this.buttonContinue.Name = "buttonContinue";
            this.buttonContinue.Size = new System.Drawing.Size(75, 23);
            this.buttonContinue.TabIndex = 2;
            this.buttonContinue.Text = "Continue";
            this.buttonContinue.UseVisualStyleBackColor = true;
            this.buttonContinue.Click += new System.EventHandler(this.buttonContinue_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(157, 194);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // MultiSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(244, 229);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonContinue);
            this.Controls.Add(this.listViewSelectableItems);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MultiSelectForm";
            this.Text = "MultiSelectForm";
            this.Load += new System.EventHandler(this.MultiSelectForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewSelectableItems;
        private System.Windows.Forms.Button buttonContinue;
        private System.Windows.Forms.Button buttonCancel;
    }
}