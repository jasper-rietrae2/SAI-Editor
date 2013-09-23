using System;
using System.Windows.Forms;

namespace SAI_Editor
{
    public enum MultiSelectFormType
    {
        MultiSelectFormTypePhaseMask = 0,
        MultiSelectFormTypeEventFlag = 1,
        MultiSelectFormTypeCastFlag = 2,
    };

    public partial class MultiSelectForm : Form
    {
        private readonly ListViewColumnSorter lvwColumnSorter = new ListViewColumnSorter();
        private readonly TextBox textBoxToChange = null;

        public MultiSelectForm(MultiSelectFormType searchType, TextBox textBoxToChange)
        {
            InitializeComponent();

            this.textBoxToChange = textBoxToChange;
            listViewSelectableItems.Columns.Add("", 20, HorizontalAlignment.Left);
            listViewSelectableItems.ColumnClick += listViewSelectableItems_ColumnClick;

            switch (searchType)
            {
                case MultiSelectFormType.MultiSelectFormTypePhaseMask:
                    Text = "Select a phasemask";
                    listViewSelectableItems.Columns.Add("Phase", 235, HorizontalAlignment.Left);
                    listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_PHASE_ALWAYS"); // 0
                    listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_PHASE_1");      // 1
                    listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_PHASE_2");      // 2
                    listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_PHASE_3");      // 3
                    listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_PHASE_4");      // 4
                    listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_PHASE_5");      // 5
                    listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_PHASE_6");      // 6
                    break;
                case MultiSelectFormType.MultiSelectFormTypeEventFlag:
                    Text = "Select eventflags";
                    listViewSelectableItems.Columns.Add("Flag", 235, HorizontalAlignment.Left);
                    listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_FLAG_NONE");           // 0
                    listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_FLAG_NOT_REPEATABLE"); // 1
                    listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_FLAG_DIFFICULTY_0");   // 2
                    listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_FLAG_DIFFICULTY_1");   // 3
                    listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_FLAG_DIFFICULTY_2");   // 4
                    listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_FLAG_DIFFICULTY_3");   // 5
                    listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_FLAG_DEBUG_ONLY");     // 6
                    break;
                case MultiSelectFormType.MultiSelectFormTypeCastFlag:
                    Text = "Select castflags";
                    listViewSelectableItems.Columns.Add("Flag", 235, HorizontalAlignment.Left);
                    listViewSelectableItems.Items.Add("").SubItems.Add("SMARTCAST_NONE"); // 0
                    listViewSelectableItems.Items.Add("").SubItems.Add("SMARTCAST_INTERRUPT_PREVIOUS"); // 1
                    listViewSelectableItems.Items.Add("").SubItems.Add("SMARTCAST_TRIGGERED"); // 2
                    listViewSelectableItems.Items.Add("").SubItems.Add("SMARTCAST_AURA_NOT_PRESENT"); // 3
                    break;
            }
        }

        private void MultiSelectForm_Load(object sender, EventArgs e)
        {
            if (textBoxToChange.Text == "0" || String.IsNullOrWhiteSpace(textBoxToChange.Text))
                listViewSelectableItems.Items[0].Checked = true;
            else
            {
                int bitmask = Convert.ToInt32(textBoxToChange.Text);

                foreach (ListViewItem item in listViewSelectableItems.Items)
                    if (item.Index > 0 && (bitmask & GetMaskByIndex(item.Index)) == GetMaskByIndex(item.Index))
                        item.Checked = true;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonContinue_Click(object sender, EventArgs e)
        {
            int mask = 0;

            foreach (ListViewItem item in listViewSelectableItems.CheckedItems)
                mask += GetMaskByIndex(item.Index);

            textBoxToChange.Text = mask.ToString();
            Close();
        }

        private int GetMaskByIndex(int index)
        {
            switch (index)
            {
                case 1: return 1;
                case 2: return 2;
                case 3: return 4;
                case 4: return 8;
                case 5: return 16;
                case 6: return 32;
                default: return 0;
            }
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
            if (listViewSelectableItems.CheckedItems.Count <= 0)
                listViewSelectableItems.Items[0].Checked = true;

            if (e.Item.Checked && e.Item.Index > 0)
                listViewSelectableItems.Items[0].Checked = false;
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
    }
}
