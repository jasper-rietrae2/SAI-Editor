using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SAI_Editor.Classes;
using SAI_Editor.Classes.Database.Classes;

namespace SAI_Editor.Forms.SearchForms
{
    public partial class SearchForLinkForm : Form
    {
        private readonly ListViewColumnSorter lvwColumnSorter = new ListViewColumnSorter();
        private int indexOfLineToDisable = 0;
        private readonly TextBox textBoxToChange = null;

        public SearchForLinkForm(List<SmartScript> smartScripts, int indexOfLineToDisable, TextBox textBoxToChange)
        {
            InitializeComponent();

            this.indexOfLineToDisable = indexOfLineToDisable;
            this.textBoxToChange = textBoxToChange;

            foreach (SmartScript smartScript in smartScripts)
                listViewScripts.AddScript(smartScript);

            foreach (ColumnHeader header in listViewScripts.Columns)
                header.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);

            if (listViewScripts.Items.Count > indexOfLineToDisable)
                listViewScripts.Items[indexOfLineToDisable].BackColor = SystemColors.Control;
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
            buttonContinue.PerformClick();
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
