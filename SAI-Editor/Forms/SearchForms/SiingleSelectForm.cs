using System;
using System.Windows.Forms;
using SAI_Editor.Classes;

namespace SAI_Editor.Forms.SearchForms
{
    public partial class SingleSelectForm<T> : Form where T : struct, IConvertible
    {
        private readonly ListViewColumnSorter lvwColumnSorter = new ListViewColumnSorter();
        private readonly TextBox textBoxToChange = null;

        public SingleSelectForm(TextBox textBoxToChange)
        {
            this.InitializeComponent();

            this.textBoxToChange = textBoxToChange;
            this.listViewSelectableItems.Columns.Add(typeof(T).Name, 235, HorizontalAlignment.Left);

            foreach (var en in Enum.GetNames(typeof(T)))
                this.listViewSelectableItems.Items.Add(en);

            this.listViewSelectableItems.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            if (!String.IsNullOrWhiteSpace(textBoxToChange.Text) && textBoxToChange.Text != "0")
            {
                foreach (ListViewItem item in this.listViewSelectableItems.Items)
                {
                    if (item.Index > 0 && textBoxToChange.Text == item.Index.ToString())
                    {
                        item.Selected = true;
                        item.EnsureVisible();
                        break; //! It's a SINGLE select form so only one item can be selected anyway.
                    }
                }
            }
            else
                this.listViewSelectableItems.Items[0].Selected = true;
        }

        private void buttonContinue_Click(object sender, EventArgs e)
        {
            if (this.listViewSelectableItems.SelectedItems.Count == 0)
                return;

            string index = this.listViewSelectableItems.SelectedItems[0].Index.ToString();

            if (index == "7" && typeof(T).Name == "PowerTypes") //! POWER_HEALTH
                index = "-2";

            this.textBoxToChange.Text = index;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listViewSelectableItems_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            var myListView = (ListView)sender;
            myListView.ListViewItemSorter = this.lvwColumnSorter;

            if (e.Column != this.lvwColumnSorter.SortColumn)
            {
                this.lvwColumnSorter.SortColumn = e.Column;
                this.lvwColumnSorter.Order = SortOrder.Ascending;
            }
            else
                this.lvwColumnSorter.Order = this.lvwColumnSorter.Order == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;

            myListView.Sort();
        }

        private void SingleSelectForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.Close();
                    break;
                case Keys.Enter:
                    if (this.listViewSelectableItems.SelectedItems.Count > 0)
                        this.buttonContinue.PerformClick();

                    break;
            }
        }

        private void listViewSelectableItems_DoubleClick(object sender, EventArgs e)
        {
            this.buttonContinue.PerformClick();
        }

        private void listViewSelectableItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.listViewSelectableItems.Items[0].Checked = this.listViewSelectableItems.CheckedItems.Count == 0;
        }
    }
}
