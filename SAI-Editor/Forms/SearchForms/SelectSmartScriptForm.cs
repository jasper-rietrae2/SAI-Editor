using SAI_Editor.Classes;
using SAI_Editor.Database.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAI_Editor
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
        }

        private void SelectSmartScriptForm_Load(object sender, EventArgs e)
        {
            foreach (ColumnHeader header in listViewSmartScripts.Columns)
                header.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);

            buttonLoadScript.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            listViewSmartScripts.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Left;
        }

        private void listBoxGuids_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxGuids.SelectedIndex == -1)
                listBoxGuids.SelectedIndex = 0;

            listViewSmartScripts.Items.Clear();
            listViewSmartScripts.AddSmartScripts(items[listBoxGuids.SelectedIndex]);

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

            ((MainForm)Owner).textBoxEntryOrGuid.Text = (-XConverter.ToInt32(listBoxGuids.SelectedItem.ToString())).ToString();
            ((MainForm)Owner).comboBoxSourceType.SelectedIndex = (int)SourceTypes.SourceTypeCreature;
            ((MainForm)Owner).TryToLoadScript(true);
            Close();
        }

        private void listBoxGuids_DoubleClick(object sender, EventArgs e)
        {
            LoadScript();
        }
    }
}
