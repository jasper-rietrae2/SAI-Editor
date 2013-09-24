using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAI_Editor.SearchForms
{
    public enum SingleSelectFormType
    {
        SingleSelectFormTypeReactState = 0,
        SingleSelectFormTypeRespawnType = 1,
    };

    public partial class SingleSelectForm : Form
    {
        private readonly ListViewColumnSorter lvwColumnSorter = new ListViewColumnSorter();
        private readonly TextBox textBoxToChange = null;
        private readonly SingleSelectFormType searchType;

        public SingleSelectForm(TextBox textBoxToChange, SingleSelectFormType searchType)
        {
            InitializeComponent();

            this.searchType = searchType;
            this.textBoxToChange = textBoxToChange;

            listViewSelectableItems.ColumnClick += listViewSelectableItems_ColumnClick;

            switch (searchType)
            {
                case SingleSelectFormType.SingleSelectFormTypeReactState:
                    Text = "Select a reactstate";
                    listViewSelectableItems.Columns.Add("Reactstate", 278, HorizontalAlignment.Left);
                    listViewSelectableItems.Items.Add("REACT_NONE"); // 0
                    listViewSelectableItems.Items.Add("REACT_PASSIVE"); // 1
                    listViewSelectableItems.Items.Add("REACT_DEFENSIVE"); // 2
                    listViewSelectableItems.Items.Add("REACT_AGGRESSIVE"); // 3
                    break;
                case SingleSelectFormType.SingleSelectFormTypeRespawnType:
                    Text = "Select a respawn condition";
                    listViewSelectableItems.Columns.Add("Respawn condition", 278, HorizontalAlignment.Left);
                    listViewSelectableItems.Items.Add("RESPAWN_CONDITION_NONE"); // 0
                    listViewSelectableItems.Items.Add("RESPAWN_CONDITION_MAP"); // 1
                    listViewSelectableItems.Items.Add("RESPAWN_CONDITION_AREA"); // 2
                    break;
            }
        }

        private void SingleSelectForm_Load(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxToChange.Text) || textBoxToChange.Text == "0")
                listViewSelectableItems.Items[0].Selected = true;
            else
            {
                foreach (ListViewItem item in listViewSelectableItems.Items)
                    if (item.Index > 0 && textBoxToChange.Text == item.Index.ToString())
                        item.Selected = true;
            }
        }

        private void buttonContinue_Click(object sender, EventArgs e)
        {
            textBoxToChange.Text = listViewSelectableItems.SelectedItems[0].Index.ToString();
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void listViewSelectableItems_ColumnClick(object sender, ColumnClickEventArgs e)
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

        private void SingleSelectForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Close();
                    break;
            }
        }

        private void listViewSelectableItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            listViewSelectableItems.Items[0].Checked = listViewSelectableItems.CheckedItems.Count == 0;
        }

        private void listViewSelectableItems_DoubleClick(object sender, EventArgs e)
        {
            buttonContinue.PerformClick();
        }
    }
}
