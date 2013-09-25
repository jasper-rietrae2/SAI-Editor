using System;
using System.Windows.Forms;

namespace SAI_Editor.SearchForms
{
    public enum SingleSelectFormType
    {
        SingleSelectFormTypeReactState = 0,
        SingleSelectFormTypeRespawnType = 1,
        SingleSelectFormTypeGoState = 2,
        SingleSelectFormTypePowerType = 3,
        SingleSelectFormTypeTargetType = 4,
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
                case SingleSelectFormType.SingleSelectFormTypeGoState:
                    Text = "Select a gameobject state";
                    listViewSelectableItems.Columns.Add("Gameobject state", 278, HorizontalAlignment.Left);
                    listViewSelectableItems.Items.Add("GAMEOBJECT_NOT_READY"); // 0
                    listViewSelectableItems.Items.Add("GAMEOBJECT_READY"); // 1
                    listViewSelectableItems.Items.Add("GAMEOBJECT_ACTIVATED"); // 2
                    listViewSelectableItems.Items.Add("GAMEOBJECT_JUST_DEACTIVATED"); // 3
                    break;
                case SingleSelectFormType.SingleSelectFormTypePowerType:
                    Text = "Select a power type";
                    listViewSelectableItems.Columns.Add("Power type", 278, HorizontalAlignment.Left);
                    listViewSelectableItems.Items.Add("POWER_MANA"); // 0
                    listViewSelectableItems.Items.Add("POWER_RAGE"); // 1
                    listViewSelectableItems.Items.Add("POWER_FOCUS"); // 2
                    listViewSelectableItems.Items.Add("POWER_ENERGY"); // 3
                    listViewSelectableItems.Items.Add("POWER_HAPPINESS"); // 4
                    listViewSelectableItems.Items.Add("POWER_RUNE"); // 5
                    listViewSelectableItems.Items.Add("POWER_RUNIC_POWER"); // 6
                    listViewSelectableItems.Items.Add("POWER_HEALTH"); // -2...
                    break;
                case SingleSelectFormType.SingleSelectFormTypeTargetType:
                    Text = "Select a target type";
                    listViewSelectableItems.Columns.Add("Target type", 278, HorizontalAlignment.Left);
                    listViewSelectableItems.Items.Add("SMART_TARGET_NONE"); // 0
                    listViewSelectableItems.Items.Add("SMART_TARGET_SELF"); // 1
                    listViewSelectableItems.Items.Add("SMART_TARGET_VICTIM"); // 2
                    listViewSelectableItems.Items.Add("SMART_TARGET_HOSTILE_SECOND_AGGRO"); // 3
                    listViewSelectableItems.Items.Add("SMART_TARGET_HOSTILE_LAST_AGGRO"); // 4
                    listViewSelectableItems.Items.Add("SMART_TARGET_HOSTILE_RANDOM"); // 5
                    listViewSelectableItems.Items.Add("SMART_TARGET_HOSTILE_RANDOM_NOT_TOP"); // 6
                    listViewSelectableItems.Items.Add("SMART_TARGET_ACTION_INVOKER"); // 7
                    listViewSelectableItems.Items.Add("SMART_TARGET_POSITION"); // 8
                    listViewSelectableItems.Items.Add("SMART_TARGET_CREATURE_RANGE"); // 9
                    listViewSelectableItems.Items.Add("SMART_TARGET_CREATURE_GUID"); // 10
                    listViewSelectableItems.Items.Add("SMART_TARGET_CREATURE_DISTANCE"); // 11
                    listViewSelectableItems.Items.Add("SMART_TARGET_STORED"); // 12
                    listViewSelectableItems.Items.Add("SMART_TARGET_GAMEOBJECT_RANGE"); // 13
                    listViewSelectableItems.Items.Add("SMART_TARGET_GAMEOBJECT_GUID"); // 14
                    listViewSelectableItems.Items.Add("SMART_TARGET_GAMEOBJECT_DISTANCE"); // 15
                    listViewSelectableItems.Items.Add("SMART_TARGET_INVOKER_PARTY"); // 16
                    listViewSelectableItems.Items.Add("SMART_TARGET_PLAYER_RANGE"); // 17
                    listViewSelectableItems.Items.Add("SMART_TARGET_PLAYER_DISTANCE"); // 18
                    listViewSelectableItems.Items.Add("SMART_TARGET_CLOSEST_CREATURE"); // 19
                    listViewSelectableItems.Items.Add("SMART_TARGET_CLOSEST_GAMEOBJECT"); // 20
                    listViewSelectableItems.Items.Add("SMART_TARGET_CLOSEST_PLAYER"); // 22
                    listViewSelectableItems.Items.Add("SMART_TARGET_ACTION_INVOKER_VEHICLE"); // 23
                    listViewSelectableItems.Items.Add("SMART_TARGET_OWNER_OR_SUMMONER"); // 24
                    listViewSelectableItems.Items.Add("SMART_TARGET_THREAT_LIST"); // 25
                    listViewSelectableItems.Items.Add("SMART_TARGET_CLOSEST_ENEMY"); // 26
                    listViewSelectableItems.Items.Add("SMART_TARGET_CLOSEST_FRIENDLY"); // 27
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
            string index = listViewSelectableItems.SelectedItems[0].Index.ToString();

            if (index == "7" && searchType == SingleSelectFormType.SingleSelectFormTypePowerType) //! POWER_HEALTH
                index = "-2";

            textBoxToChange.Text = index;
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
