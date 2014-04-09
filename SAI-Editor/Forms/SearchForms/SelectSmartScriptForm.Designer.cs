namespace SAI_Editor.Forms.SearchForms
{
    partial class SelectSmartScriptForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectSmartScriptForm));
            this.listBoxGuids = new System.Windows.Forms.ListBox();
            this.listViewSmartScripts = new SAI_Editor.Classes.CustomControls.SmartScriptListView();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonLoadScript = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBoxGuids
            // 
            this.listBoxGuids.FormattingEnabled = true;
            this.listBoxGuids.Location = new System.Drawing.Point(12, 25);
            this.listBoxGuids.Name = "listBoxGuids";
            this.listBoxGuids.Size = new System.Drawing.Size(49, 238);
            this.listBoxGuids.TabIndex = 1;
            this.listBoxGuids.SelectedIndexChanged += new System.EventHandler(this.listBoxGuids_SelectedIndexChanged);
            this.listBoxGuids.DoubleClick += new System.EventHandler(this.listBoxGuids_DoubleClick);
            // 
            // listViewSmartScripts
            // 
            this.listViewSmartScripts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewSmartScripts.EnablePhaseHighlighting = false;
            this.listViewSmartScripts.FullRowSelect = true;
            this.listViewSmartScripts.Location = new System.Drawing.Point(67, 25);
            this.listViewSmartScripts.MultiSelect = false;
            this.listViewSmartScripts.Name = "listViewSmartScripts";
            this.listViewSmartScripts.Size = new System.Drawing.Size(961, 237);
            this.listViewSmartScripts.TabIndex = 2;
            this.listViewSmartScripts.UseCompatibleStateImageBehavior = false;
            this.listViewSmartScripts.View = System.Windows.Forms.View.Details;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(771, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Select a guid in the listbox below which you want to watch. Once a selection is m" +
    "ade, you can press the button to select this script and load it into the main re" +
    "sults.";
            // 
            // buttonLoadScript
            // 
            this.buttonLoadScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLoadScript.Location = new System.Drawing.Point(880, 268);
            this.buttonLoadScript.Name = "buttonLoadScript";
            this.buttonLoadScript.Size = new System.Drawing.Size(148, 23);
            this.buttonLoadScript.TabIndex = 4;
            this.buttonLoadScript.Text = "Load selected script";
            this.buttonLoadScript.UseVisualStyleBackColor = true;
            this.buttonLoadScript.Click += new System.EventHandler(this.buttonLoadScript_Click);
            // 
            // SelectSmartScriptForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1040, 298);
            this.Controls.Add(this.buttonLoadScript);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listViewSmartScripts);
            this.Controls.Add(this.listBoxGuids);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1056, 336);
            this.Name = "SelectSmartScriptForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select a script to load";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxGuids;
        private SAI_Editor.Classes.CustomControls.SmartScriptListView listViewSmartScripts;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonLoadScript;

    }
}