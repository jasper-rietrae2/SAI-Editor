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
            listViewSmartScripts.Columns.Add("entryorguid", 67, HorizontalAlignment.Left);  // 0
            listViewSmartScripts.Columns.Add("source_type", 70, HorizontalAlignment.Right); // 1
            listViewSmartScripts.Columns.Add("id", 20, HorizontalAlignment.Right); // 2
            listViewSmartScripts.Columns.Add("link", 30, HorizontalAlignment.Right); // 3
            listViewSmartScripts.Columns.Add("event_type", 66, HorizontalAlignment.Right); // 4
            listViewSmartScripts.Columns.Add("event_phase", 74, HorizontalAlignment.Right); // 5
            listViewSmartScripts.Columns.Add("event_chance", 81, HorizontalAlignment.Right); // 6
            listViewSmartScripts.Columns.Add("event_flags", 69, HorizontalAlignment.Right); // 7
            listViewSmartScripts.Columns.Add("p1", 24, HorizontalAlignment.Right); // 8
            listViewSmartScripts.Columns.Add("p2", 24, HorizontalAlignment.Right); // 9
            listViewSmartScripts.Columns.Add("p3", 24, HorizontalAlignment.Right); // 10
            listViewSmartScripts.Columns.Add("p4", 24, HorizontalAlignment.Right); // 11
            listViewSmartScripts.Columns.Add("action_type", 67, HorizontalAlignment.Right); // 12
            listViewSmartScripts.Columns.Add("p1", 24, HorizontalAlignment.Right); // 13
            listViewSmartScripts.Columns.Add("p2", 24, HorizontalAlignment.Right); // 14
            listViewSmartScripts.Columns.Add("p3", 24, HorizontalAlignment.Right); // 15
            listViewSmartScripts.Columns.Add("p4", 24, HorizontalAlignment.Right); // 16
            listViewSmartScripts.Columns.Add("p5", 24, HorizontalAlignment.Right); // 17
            listViewSmartScripts.Columns.Add("p6", 24, HorizontalAlignment.Right); // 18
            listViewSmartScripts.Columns.Add("target_type", 67, HorizontalAlignment.Right); // 19
            listViewSmartScripts.Columns.Add("p1", 24, HorizontalAlignment.Right); // 20
            listViewSmartScripts.Columns.Add("p2", 24, HorizontalAlignment.Right); // 21
            listViewSmartScripts.Columns.Add("p3", 24, HorizontalAlignment.Right); // 22
            listViewSmartScripts.Columns.Add("x", 20, HorizontalAlignment.Right); // 23
            listViewSmartScripts.Columns.Add("y", 20, HorizontalAlignment.Right); // 24
            listViewSmartScripts.Columns.Add("z", 20, HorizontalAlignment.Right); // 25
            listViewSmartScripts.Columns.Add("o", 20, HorizontalAlignment.Right); // 26
            listViewSmartScripts.Columns.Add("comment", 400, HorizontalAlignment.Left); // 27 (width 56 to fit)

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

            foreach (SmartScript smartScript in items[listBoxGuids.SelectedIndex])
            {
                ListViewItem listViewItem = new ListViewItem();
                listViewItem.Text = smartScript.entryorguid.ToString();
                listViewItem.SubItems.Add(smartScript.source_type.ToString());
                listViewItem.SubItems.Add(smartScript.id.ToString());
                listViewItem.SubItems.Add(smartScript.link.ToString());
                listViewItem.SubItems.Add(smartScript.event_type.ToString());
                listViewItem.SubItems.Add(smartScript.event_phase_mask.ToString());
                listViewItem.SubItems.Add(smartScript.event_chance.ToString());
                listViewItem.SubItems.Add(smartScript.event_flags.ToString());
                listViewItem.SubItems.Add(smartScript.event_param1.ToString());
                listViewItem.SubItems.Add(smartScript.event_param2.ToString());
                listViewItem.SubItems.Add(smartScript.event_param3.ToString());
                listViewItem.SubItems.Add(smartScript.event_param4.ToString());
                listViewItem.SubItems.Add(smartScript.action_type.ToString());
                listViewItem.SubItems.Add(smartScript.action_param1.ToString());
                listViewItem.SubItems.Add(smartScript.action_param2.ToString());
                listViewItem.SubItems.Add(smartScript.action_param3.ToString());
                listViewItem.SubItems.Add(smartScript.action_param4.ToString());
                listViewItem.SubItems.Add(smartScript.action_param5.ToString());
                listViewItem.SubItems.Add(smartScript.action_param6.ToString());
                listViewItem.SubItems.Add(smartScript.target_type.ToString());
                listViewItem.SubItems.Add(smartScript.target_param1.ToString());
                listViewItem.SubItems.Add(smartScript.target_param2.ToString());
                listViewItem.SubItems.Add(smartScript.target_param3.ToString());
                listViewItem.SubItems.Add(smartScript.target_x.ToString());
                listViewItem.SubItems.Add(smartScript.target_y.ToString());
                listViewItem.SubItems.Add(smartScript.target_z.ToString());
                listViewItem.SubItems.Add(smartScript.target_o.ToString());
                listViewItem.SubItems.Add(smartScript.comment);
                listViewSmartScripts.Items.Add(listViewItem);
            }

            foreach (ColumnHeader header in listViewSmartScripts.Columns)
                header.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void buttonLoadScript_Click(object sender, EventArgs e)
        {
            ((MainForm)Owner).textBoxEntryOrGuid.Text = (-XConverter.ToInt32(listBoxGuids.SelectedItem.ToString())).ToString();
            ((MainForm)Owner).comboBoxSourceType.SelectedIndex = (int)SourceTypes.SourceTypeCreature;
            ((MainForm)Owner).TryToLoadScript(true);
            Close();
        }
    }
}
