using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SAI_Editor.Classes;
using System.Linq;

namespace SAI_Editor
{
    public enum MultiSelectFormType
    {
        MultiSelectFormTypePhaseMask = 0,
        MultiSelectFormTypeEventFlag = 1,
        MultiSelectFormTypeCastFlag = 2,
        MultiSelectFormTypeUnitFlag = 3,
        MultiSelectFormTypeUnitFlag2 = 4,
        MultiSelectFormTypeGoFlag = 5,
        MultiSelectFormTypeDynamicFlag = 6,
        MultiSelectFormTypeNpcFlag = 7,
    }

    public partial class MultiSelectForm<T> : Form where T : struct, IConvertible
    {

        private readonly ListViewColumnSorter lvwColumnSorter = new ListViewColumnSorter();
        private readonly TextBox textBoxToChange = null;

        public MultiSelectForm(TextBox textBoxToChange)
        {
            InitializeComponent();

            this.textBoxToChange = textBoxToChange;
            listViewSelectableItems.Columns.Add("", 20, HorizontalAlignment.Left);

            int bitmask = XConverter.ToInt32(textBoxToChange.Text);

            listViewSelectableItems.Columns.Add(typeof(T).Name, 235, HorizontalAlignment.Left);

            foreach (var en in Enum.GetNames(typeof(T)))
                listViewSelectableItems.Items.Add("").SubItems.Add(en);

            bool anyFlag = false;

            foreach (ListViewItem item in listViewSelectableItems.Items)
            {
                foreach (var en in Enum.GetNames(typeof(T)))
                {
                    if (en.Equals(item.SubItems[1].Text))
                    {
                        object enu = Enum.Parse(typeof(T), en);
                        
                        if ((bitmask & Convert.ToInt32(enu)) == Convert.ToInt32(enu))
                        {
                            anyFlag = true;
                            item.Checked = true;
                        }
                    }
                }
            }

            if (!anyFlag)
                foreach (ListViewItem item in listViewSelectableItems.Items)
                    if (item.Index > 0)
                        item.Checked = false;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonContinue_Click(object sender, EventArgs e)
        {
            List<Enum> vals = Enum.GetValues(typeof(T)).OfType<Enum>().ToList();
            int mask = 0;

            foreach (ListViewItem item in listViewSelectableItems.CheckedItems)
                foreach (var en in Enum.GetNames(typeof(T)))
                    if (en.Equals(item.SubItems[1].Text))
                        mask += XConverter.ToInt32(Enum.Parse(typeof(T), en));

            textBoxToChange.Text = mask.ToString();
            Close();
        }

        private void MultiSelectForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Close();
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
                if (listViewSelectableItems.CheckedItems.Count <= 0)
                    listViewSelectableItems.Items[0].Checked = true;

                if (e.Item.Checked && e.Item.Index > 0)
                    listViewSelectableItems.Items[0].Checked = false;
            }
        }

        private void listViewSelectableItems_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            var myListView = (ListView)sender;
            myListView.ListViewItemSorter = lvwColumnSorter;

            if (e.Column != lvwColumnSorter.SortColumn)
            {
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }
            else
                lvwColumnSorter.Order = lvwColumnSorter.Order == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;

            myListView.Sort();
        }
    }
}
