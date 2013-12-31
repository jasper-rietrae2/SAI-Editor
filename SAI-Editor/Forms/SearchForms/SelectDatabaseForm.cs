using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SAI_Editor.Classes;

namespace SAI_Editor.Forms.SearchForms
{
    public partial class SelectDatabaseForm : Form
    {
        private readonly List<string> databaseNames = new List<string>();
        private readonly ListViewColumnSorter lvwColumnSorter = new ListViewColumnSorter();
        private readonly TextBox textBoxToChange = null;

        public SelectDatabaseForm(List<string> databaseNames, TextBox textBoxToChange)
        {
            this.InitializeComponent();

            this.databaseNames = databaseNames;
            this.textBoxToChange = textBoxToChange;

            for (int i = 0; i < databaseNames.Count; ++i)
            {
                this.listViewDatabases.Items.Add(databaseNames[i]);

                //! Select the currently used database (if any)
                if (textBoxToChange.Text == databaseNames[i])
                    this.listViewDatabases.Items[i].Selected = true;
            }
        }

        private void SelectDatabaseForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.Close();
                    break;
            }
        }

        private void buttonContinue_Click(object sender, EventArgs e)
        {
            if (this.listViewDatabases.SelectedItems.Count == 0)
                return;

            this.textBoxToChange.Text = this.listViewDatabases.SelectedItems[0].Text;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listViewDatabases_DoubleClick(object sender, EventArgs e)
        {
            this.buttonContinue.PerformClick();
        }

        private void listViewDatabases_ColumnClick(object sender, ColumnClickEventArgs e)
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

        private void listViewDatabases_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.buttonContinue.Enabled = this.listViewDatabases.SelectedItems.Count > 0;
        }
    }
}
