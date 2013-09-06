using System;
using System.Windows.Forms;

namespace SAI_Editor
{
    public partial class MultiSelectForm : Form
    {
        private bool searchingForPhasemask;

        public MultiSelectForm(bool searchingForPhasemask)
        {
            InitializeComponent();

            this.searchingForPhasemask = searchingForPhasemask;
        }

        private void MultiSelectForm_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
            MinimizeBox = true;
            FormBorderStyle = FormBorderStyle.Fixed3D;

            KeyPreview = true;
            KeyDown += MultiSelectForm_KeyDown;

            listViewSelectableItems.View = View.Details;
            listViewSelectableItems.FullRowSelect = true;
            listViewSelectableItems.Columns.Add("", 20, HorizontalAlignment.Left);
            listViewSelectableItems.ItemChecked += listViewSelectableItems_ItemChecked;

            if (searchingForPhasemask)
            {
                Text = "Select phasemask";
                listViewSelectableItems.Columns.Add("Phase", 195, HorizontalAlignment.Left);
                listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_PHASE_ALWAYS"); // 0
                listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_PHASE_1");      // 1
                listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_PHASE_2");      // 2
                listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_PHASE_3");      // 3
                listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_PHASE_4");      // 4
                listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_PHASE_5");      // 5
                listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_PHASE_6");      // 6
            }
            else
            {
                Text = "Select eventflag";
                listViewSelectableItems.Columns.Add("Flag", 195, HorizontalAlignment.Left);
                listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_FLAG_NONE");           // 0
                listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_FLAG_NOT_REPEATABLE"); // 1
                listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_FLAG_DIFFICULTY_0");   // 2
                listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_FLAG_DIFFICULTY_1");   // 3
                listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_FLAG_DIFFICULTY_2");   // 4
                listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_FLAG_DIFFICULTY_3");   // 5
                listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_FLAG_DEBUG_ONLY");     // 6
            }

            int bitmask = Convert.ToInt32(searchingForPhasemask ? ((MainForm)Owner).textBoxEventPhasemask.Text : ((MainForm)Owner).textBoxEventFlags.Text);

            if (bitmask == 0)
                listViewSelectableItems.Items[0].Checked = true;
            else
            {
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

            if (searchingForPhasemask)
                ((MainForm)Owner).textBoxEventPhasemask.Text = mask.ToString();
            else
                ((MainForm)Owner).textBoxEventFlags.Text = mask.ToString();

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
            if (e.Item.Checked && e.Item.Index > 0)
                listViewSelectableItems.Items[0].Checked = false;
        }
    }
}
