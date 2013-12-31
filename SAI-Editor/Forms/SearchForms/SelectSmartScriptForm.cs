using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SAI_Editor.Classes;
using SAI_Editor.Classes.Database.Classes;

namespace SAI_Editor.Forms.SearchForms
{
    public partial class SelectSmartScriptForm : Form
    {
        private readonly List<List<SmartScript>> items;

        public SelectSmartScriptForm(List<List<SmartScript>> items)
        {
            this.InitializeComponent();

            this.items = items;

            foreach (List<SmartScript> smartScripts in items)
                this.listBoxGuids.Items.Add(-smartScripts[0].entryorguid);

            this.listBoxGuids.SelectedIndex = 0;

            foreach (ColumnHeader header in this.listViewSmartScripts.Columns)
                header.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void listBoxGuids_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxGuids.SelectedIndex == -1)
                this.listBoxGuids.SelectedIndex = 0;

            this.listViewSmartScripts.Items.Clear();
            this.listViewSmartScripts.AddSmartScripts(this.items[this.listBoxGuids.SelectedIndex]);

            foreach (ColumnHeader header in this.listViewSmartScripts.Columns)
                header.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void buttonLoadScript_Click(object sender, EventArgs e)
        {
            this.LoadScript();
        }

        private void LoadScript()
        {
            if (this.listBoxGuids.SelectedItem == null)
                return;

            ((MainForm)this.Owner).textBoxEntryOrGuid.Text = (-XConverter.ToInt32(this.listBoxGuids.SelectedItem.ToString())).ToString();
            ((MainForm)this.Owner).comboBoxSourceType.SelectedIndex = (int)SourceTypes.SourceTypeCreature;
            ((MainForm)this.Owner).TryToLoadScript();
            this.Close();
        }

        private void listBoxGuids_DoubleClick(object sender, EventArgs e)
        {
            this.LoadScript();
        }
    }
}
