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

            listViewSelectableItems.View = View.Details;
            listViewSelectableItems.FullRowSelect = true;
            listViewSelectableItems.Columns.Add("", 20, HorizontalAlignment.Left);

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
                listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_FLAG_NOT_REPEATABLE"); // 0
                listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_FLAG_DIFFICULTY_0");   // 1
                listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_FLAG_DIFFICULTY_1");   // 2
                listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_FLAG_DIFFICULTY_2");   // 3
                listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_FLAG_DIFFICULTY_3");   // 4
                listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_FLAG_DEBUG_ONLY");     // 5
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonContinue_Click(object sender, EventArgs e)
        {
            int phase = 0;

            foreach (ListViewItem item in listViewSelectableItems.CheckedItems)
                phase = (phase << 1) | 1;

            MessageBox.Show(phase.ToString());

            Close();
        }

        //private int GetMaskByIndex(int index)
        //{
            
        //}
    }
}
