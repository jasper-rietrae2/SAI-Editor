using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SAI_Editor
{
    public partial class SelectDatabaseForm : Form
    {
        private readonly List<string> databaseNames = new List<string>();
        private readonly ListViewColumnSorter lvwColumnSorter = new ListViewColumnSorter();

        public SelectDatabaseForm(List<string> databaseNames)
        {
            InitializeComponent();

            this.databaseNames = databaseNames;
        }

        private void SelectDatabaseForm_Load(object sender, EventArgs e)
        {
            listViewDatabases.Columns.Add("Database", 198, HorizontalAlignment.Left);
            listViewDatabases.ListViewItemSorter = lvwColumnSorter;
            listViewDatabases.ColumnClick += listViewDatabases_ColumnClick;

            for (int i = 0; i < databaseNames.Count; ++i)
            {
                listViewDatabases.Items.Add(databaseNames[i]);

                //! Select the currently used database (if any)
                if (((MainForm)Owner).textBoxWorldDatabase.Text == databaseNames[i])
                    listViewDatabases.Items[i].Selected = true;
            }
        }

        private void SelectDatabaseForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Close();
                    break;
            }
        }

        private void buttonContinue_Click(object sender, EventArgs e)
        {
            ((MainForm)Owner).textBoxWorldDatabase.Text = listViewDatabases.SelectedItems[0].Text;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void listViewDatabases_DoubleClick(object sender, EventArgs e)
        {
            buttonContinue_Click(sender, e);
        }

        private void listViewDatabases_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            var myListView = (ListView)sender;

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

        private void listViewDatabases_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonContinue.Enabled = listViewDatabases.SelectedItems.Count > 0;
        }
    }
}
