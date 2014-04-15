using System;

namespace SAI_Editor.Forms.SearchForms
{
    partial class SingleSelectForm<T> where T : struct, IConvertible
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonContinue = new System.Windows.Forms.Button();
            this.listViewSelectableItems = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(219, 194);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonContinue
            // 
            this.buttonContinue.Location = new System.Drawing.Point(12, 194);
            this.buttonContinue.Name = "buttonContinue";
            this.buttonContinue.Size = new System.Drawing.Size(75, 23);
            this.buttonContinue.TabIndex = 5;
            this.buttonContinue.Text = "Continue";
            this.buttonContinue.UseVisualStyleBackColor = true;
            this.buttonContinue.Click += new System.EventHandler(this.buttonContinue_Click);
            // 
            // listViewSelectableItems
            // 
            this.listViewSelectableItems.FullRowSelect = true;
            this.listViewSelectableItems.Location = new System.Drawing.Point(12, 12);
            this.listViewSelectableItems.MultiSelect = false;
            this.listViewSelectableItems.Name = "listViewSelectableItems";
            this.listViewSelectableItems.Size = new System.Drawing.Size(282, 176);
            this.listViewSelectableItems.TabIndex = 4;
            this.listViewSelectableItems.UseCompatibleStateImageBehavior = false;
            this.listViewSelectableItems.View = System.Windows.Forms.View.Details;
            this.listViewSelectableItems.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewSelectableItems_ColumnClick);
            this.listViewSelectableItems.SelectedIndexChanged += new System.EventHandler(this.listViewSelectableItems_SelectedIndexChanged);
            this.listViewSelectableItems.DoubleClick += new System.EventHandler(this.listViewSelectableItems_DoubleClick);
            // 
            // SingleSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 229);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonContinue);
            this.Controls.Add(this.listViewSelectableItems);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SingleSelectForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select something...";
            this.Load += new System.EventHandler(this.SingleSelectForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SingleSelectForm_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonContinue;
        private System.Windows.Forms.ListView listViewSelectableItems;
    }
}