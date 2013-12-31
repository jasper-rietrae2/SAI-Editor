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
        private List<SmartScript> smartScripts;
        private readonly TextBox textBoxToChange = null;

        public SearchForLinkForm(List<SmartScript> smartScripts, int indexOfLineToDisable, TextBox textBoxToChange)
        {
            this.InitializeComponent();

            this.smartScripts = smartScripts;
            this.indexOfLineToDisable = indexOfLineToDisable;
            this.textBoxToChange = textBoxToChange;

            foreach (SmartScript smartScript in smartScripts)
                this.listViewScripts.AddSmartScript(smartScript);

            foreach (ColumnHeader header in this.listViewScripts.Columns)
                header.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);

            if (this.listViewScripts.Items.Count > indexOfLineToDisable)
                this.listViewScripts.Items[indexOfLineToDisable].BackColor = SystemColors.Control;
        }

        private void buttonContinue_Click(object sender, EventArgs e)
        {
            //! Shouldn't be able to happen
            if (this.listViewScripts.SelectedItems.Count <= 0)
            {
                this.buttonContinue.Enabled = false;
                return;
            }

            this.textBoxToChange.Text = this.listViewScripts.SelectedItems[0].SubItems[2].Text;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listViewScripts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listViewScripts.SelectedItems.Count > 0)
            {
                if (this.listViewScripts.SelectedItems[0].Index == this.indexOfLineToDisable)
                {
                    this.listViewScripts.SelectedItems[0].Selected = false;
                    return;
                }

                this.buttonContinue.Enabled = true;
                return;
            }

            this.buttonContinue.Enabled = false;
        }

        private void listViewScripts_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            var myListView = (ListView)sender;
            myListView.ListViewItemSorter = this.lvwColumnSorter;

            //! Determine if clicked column is already the column that is being sorted
            if (e.Column != this.lvwColumnSorter.SortColumn)
            {
                //! Set the column number that is to be sorted; default to ascending
                this.lvwColumnSorter.SortColumn = e.Column;
                this.lvwColumnSorter.Order = SortOrder.Ascending;
            }
            else
                //! Reverse the current sort direction for this column
                this.lvwColumnSorter.Order = this.lvwColumnSorter.Order == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;

            //! Perform the sort with these new sort options
            myListView.Sort();
        }

        private void listViewScripts_DoubleClick(object sender, EventArgs e)
        {
            this.buttonContinue.PerformClick();
        }

        private void SearchForLinkForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.Close();
                    break;
            }
        }
    }
}
