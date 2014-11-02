using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SAI_Editor.Classes;
using SAI_Editor.Classes.Database.Classes;
using SAI_Editor.Enumerators;

namespace SAI_Editor.Forms.SearchForms
{
    public partial class SelectSmartScriptForm : Form
    {
        private readonly List<List<SmartScript>> items;

        public SelectSmartScriptForm(List<List<SmartScript>> items)
        {
            InitializeComponent();

            this.items = items;

            foreach (List<SmartScript> smartScripts in items)
                listBoxGuids.Items.Add(-smartScripts[0].entryorguid);

            listBoxGuids.SelectedIndex = 0;

            foreach (ColumnHeader header in listViewSmartScripts.Columns)
                header.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void listBoxGuids_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxGuids.SelectedIndex == -1)
                listBoxGuids.SelectedIndex = 0;

            listViewSmartScripts.Items.Clear();
            listViewSmartScripts.AddScripts(items[listBoxGuids.SelectedIndex]);

            foreach (ColumnHeader header in listViewSmartScripts.Columns)
                header.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void buttonLoadScript_Click(object sender, EventArgs e)
        {
            LoadScript();
        }

        private void LoadScript()
        {
            if (listBoxGuids.SelectedItem == null)
                return;

            ((MainForm)Owner).GetActiveUserControl().textBoxEntryOrGuid.Text = (-XConverter.ToInt32(listBoxGuids.SelectedItem.ToString())).ToString();
            ((MainForm)Owner).GetActiveUserControl().comboBoxSourceType.SelectedIndex = (int)SourceTypes.SourceTypeCreature;
            ((MainForm)Owner).GetActiveUserControl().TryToLoadScript();
            Close();
        }

        private void listBoxGuids_DoubleClick(object sender, EventArgs e)
        {
            LoadScript();
        }
    }
}
