using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SAI_Editor
{
    public partial class SearchForLinkForm : Form
    {
        private readonly ListViewColumnSorter lvwColumnSorter = new ListViewColumnSorter();
        private int indexOfLineToDisable = 0;
        private ListView.ListViewItemCollection items;
        private readonly TextBox textBoxToChange = null;

        public SearchForLinkForm(ListView.ListViewItemCollection items, int indexOfLineToDisable, TextBox textBoxToChange)
        {
            InitializeComponent();

            this.items = items;
            this.indexOfLineToDisable = indexOfLineToDisable;
            this.textBoxToChange = textBoxToChange;
        }

        private void SearchForLinkForm_Load(object sender, EventArgs e)
        {
            listViewScripts.Columns.Add("entryorguid", 67, HorizontalAlignment.Left);  // 0
            listViewScripts.Columns.Add("source_type", 70, HorizontalAlignment.Right); // 1
            listViewScripts.Columns.Add("id",          20, HorizontalAlignment.Right); // 2
            listViewScripts.Columns.Add("link",        30, HorizontalAlignment.Right); // 3
            listViewScripts.Columns.Add("event_type",  66, HorizontalAlignment.Right); // 4
            listViewScripts.Columns.Add("event_phase", 74, HorizontalAlignment.Right); // 5
            listViewScripts.Columns.Add("event_chance",81, HorizontalAlignment.Right); // 6
            listViewScripts.Columns.Add("event_flags", 69, HorizontalAlignment.Right); // 7
            listViewScripts.Columns.Add("p1",          24, HorizontalAlignment.Right); // 8
            listViewScripts.Columns.Add("p2",          24, HorizontalAlignment.Right); // 9
            listViewScripts.Columns.Add("p3",          24, HorizontalAlignment.Right); // 10
            listViewScripts.Columns.Add("p4",          24, HorizontalAlignment.Right); // 11
            listViewScripts.Columns.Add("action_type", 67, HorizontalAlignment.Right); // 12
            listViewScripts.Columns.Add("p1",          24, HorizontalAlignment.Right); // 13
            listViewScripts.Columns.Add("p2",          24, HorizontalAlignment.Right); // 14
            listViewScripts.Columns.Add("p3",          24, HorizontalAlignment.Right); // 15
            listViewScripts.Columns.Add("p4",          24, HorizontalAlignment.Right); // 16
            listViewScripts.Columns.Add("p5",          24, HorizontalAlignment.Right); // 17
            listViewScripts.Columns.Add("p6",          24, HorizontalAlignment.Right); // 18
            listViewScripts.Columns.Add("target_type", 67, HorizontalAlignment.Right); // 19
            listViewScripts.Columns.Add("p1",          24, HorizontalAlignment.Right); // 20
            listViewScripts.Columns.Add("p2",          24, HorizontalAlignment.Right); // 21
            listViewScripts.Columns.Add("p3",          24, HorizontalAlignment.Right); // 22
            listViewScripts.Columns.Add("x",           20, HorizontalAlignment.Right); // 23
            listViewScripts.Columns.Add("y",           20, HorizontalAlignment.Right); // 24
            listViewScripts.Columns.Add("z",           20, HorizontalAlignment.Right); // 25
            listViewScripts.Columns.Add("o",           20, HorizontalAlignment.Right); // 26
            listViewScripts.Columns.Add("comment",     400, HorizontalAlignment.Left); // 27 (width 56 to fit)

            foreach (ListViewItem item in items)
            {
                listViewScripts.Items.Add((ListViewItem)item.Clone());

                if (item.Index == indexOfLineToDisable)
                    listViewScripts.Items[indexOfLineToDisable].BackColor = SystemColors.Control;
            }

            //! Set auto-resize for the parameter columns based on the item with highest width
            for (int i = 8; i <= 11; ++i)
                listViewScripts.Columns[i].Width = -1;

            for (int i = 13; i <= 18; ++i)
                listViewScripts.Columns[i].Width = -1;

            for (int i = 20; i <= 26; ++i)
                listViewScripts.Columns[i].Width = -1;
        }

        private void buttonContinue_Click(object sender, EventArgs e)
        {
            //! Shouldn't be able to happen
            if (listViewScripts.SelectedItems.Count <= 0)
            {
                buttonContinue.Enabled = false;
                return;
            }

            textBoxToChange.Text = listViewScripts.SelectedItems[0].SubItems[2].Text;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void listViewScripts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewScripts.SelectedItems.Count > 0)
            {
                if (listViewScripts.SelectedItems[0].Index == indexOfLineToDisable)
                {
                    listViewScripts.SelectedItems[0].Selected = false;
                    return;
                }

                buttonContinue.Enabled = true;
                return;
            }

            buttonContinue.Enabled = false;
        }

        private void listViewScripts_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            var myListView = (ListView)sender;
            myListView.ListViewItemSorter = lvwColumnSorter;

            //! Determine if clicked column is already the column that is being sorted
            if (e.Column != lvwColumnSorter.SortColumn)
            {
                //! Set the column number that is to be sorted; default to ascending
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }
            else
                //! Reverse the current sort direction for this column
                lvwColumnSorter.Order = lvwColumnSorter.Order == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;

            //! Perform the sort with these new sort options
            myListView.Sort();
        }

        private void listViewScripts_DoubleClick(object sender, EventArgs e)
        {
            buttonContinue_Click(sender, e);
        }

        private void SearchForLinkForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Close();
                    break;
            }
        }
    }
}
