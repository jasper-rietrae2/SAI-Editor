using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SAI_Editor.Forms.SearchForms
{

    using SAI_Editor.Classes;

    public partial class MultiSelectForm<T> : Form where T : struct, IConvertible
    {
        private readonly ListViewColumnSorter lvwColumnSorter = new ListViewColumnSorter();
        private readonly TextBox textBoxToChange = null;

        public MultiSelectForm(TextBox textBoxToChange)
        {
            this.InitializeComponent();

            this.textBoxToChange = textBoxToChange;

            this.listViewSelectableItems.Columns.Add(typeof(T).Name, 235, HorizontalAlignment.Left);

            foreach (var en in Enum.GetNames(typeof(T)))
                this.listViewSelectableItems.Items.Add("").SubItems.Add(en);

            long bitmask = XConverter.ToInt64(textBoxToChange.Text);
            bool anyFlag = false;

            foreach (ListViewItem item in this.listViewSelectableItems.Items)
            {
                foreach (var en in Enum.GetNames(typeof(T)))
                {
                    if (en.Equals(item.SubItems[1].Text))
                    {
                        object enu = Enum.Parse(typeof(T), en);
                        
                        if ((bitmask & Convert.ToInt64(enu)) == Convert.ToInt64(enu))
                        {
                            anyFlag = true;
                            item.Checked = true;
                        }
                    }
                }
            }

            if (!anyFlag)
                foreach (ListViewItem item in this.listViewSelectableItems.Items)
                    if (item.Index > 0)
                        item.Checked = false;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonContinue_Click(object sender, EventArgs e)
        {
            List<Enum> vals = Enum.GetValues(typeof(T)).OfType<Enum>().ToList();
            long mask = 0;

            foreach (ListViewItem item in this.listViewSelectableItems.CheckedItems)
                foreach (var en in Enum.GetNames(typeof(T)))
                    if (en.Equals(item.SubItems[1].Text))
                        mask += Convert.ToInt64(Enum.Parse(typeof(T), en));

            this.textBoxToChange.Text = mask.ToString();
            this.Close();
        }

        private void MultiSelectForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.Close();
                    break;
            }
        }

        private void listViewSelectableItems_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            //! TODO: Fix this. It's also called when the form loads and for some reason this if-check passes...
            //if (listViewSelectableItems.Items[0].Checked)
            //{
            //    foreach (ListViewItem item in listViewSelectableItems.Items)
            //        if (item.Index > 0)
            //            item.Checked = false;
            //}
            //else
            {
                if (this.listViewSelectableItems.CheckedItems.Count <= 0)
                    this.listViewSelectableItems.Items[0].Checked = true;

                if (e.Item.Checked && e.Item.Index > 0)
                    this.listViewSelectableItems.Items[0].Checked = false;
            }
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
    }
}
