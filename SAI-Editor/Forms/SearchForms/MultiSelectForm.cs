using System;
using System.Windows.Forms;
using SAI_Editor.Classes;

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
                    listViewSelectableItems.Columns.Add("Phases", 235, HorizontalAlignment.Left);
                    listViewSelectableItems.Items.Add("").SubItems.Add("PHASE_ALWAYS");
                    listViewSelectableItems.Items.Add("").SubItems.Add("PHASE_1");
                    listViewSelectableItems.Items.Add("").SubItems.Add("PHASE_2");
                    listViewSelectableItems.Items.Add("").SubItems.Add("PHASE_3");
                    listViewSelectableItems.Items.Add("").SubItems.Add("PHASE_4");
                    listViewSelectableItems.Items.Add("").SubItems.Add("PHASE_5");
                    listViewSelectableItems.Items.Add("").SubItems.Add("PHASE_6");
                    break;
                case MultiSelectFormType.MultiSelectFormTypeEventFlag:
                    Text = "Select event flags";
                    listViewSelectableItems.Columns.Add("Event flags", 235, HorizontalAlignment.Left);
                    listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_FLAG_NONE");
                    listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_FLAG_NOT_REPEATABLE");
                    listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_FLAG_NORMAL_DUNGEON");
                    listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_FLAG_HEROIC_DUNGEON");
                    listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_FLAG_NORMAL_RAID");
                    listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_FLAG_HEROIC_RAID");
                    listViewSelectableItems.Items.Add("").SubItems.Add("EVENT_FLAG_DEBUG_ONLY");
                    break;
                case MultiSelectFormType.MultiSelectFormTypeCastFlag:
                    Text = "Select cast flags";
                    listViewSelectableItems.Columns.Add("Cast flags", 235, HorizontalAlignment.Left);
                    listViewSelectableItems.Items.Add("").SubItems.Add("SMARTCAST_NONE");
                    listViewSelectableItems.Items.Add("").SubItems.Add("SMARTCAST_INTERRUPT_PREVIOUS");
                    listViewSelectableItems.Items.Add("").SubItems.Add("SMARTCAST_TRIGGERED");
                    listViewSelectableItems.Items.Add("").SubItems.Add("SMARTCAST_AURA_NOT_PRESENT");
                    break;
                case MultiSelectFormType.MultiSelectFormTypeUnitFlag:
                    Text = "Select unit flags";
                    listViewSelectableItems.Columns.Add("Flag", 235, HorizontalAlignment.Left);
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_NONE");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_SERVER_CONTROLLED");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_NON_ATTACKABLE");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_DISABLE_MOVE");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_PVP_ATTACKABLE");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_RENAME");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_PREPARATION");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_UNK_6");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_NOT_ATTACKABLE_1");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_IMMUNE_TO_PC");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_IMMUNE_TO_NPC");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_LOOTING");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_PET_IN_COMBAT");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_PVP");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_SILENCED");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_UNK_14");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_UNK_15");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_UNK_16");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_PACIFIED");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_STUNNED");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_IN_COMBAT");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_TAXI_FLIGHT");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_DISARMED");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_CONFUSED");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_FLEEING");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_PLAYER_CONTROLLED");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_NOT_SELECTABLE");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_SKINNABLE");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_MOUNT");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_UNK_28");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_UNK_29");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_SHEATHE");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG_UNK_31");
                    break;
                case MultiSelectFormType.MultiSelectFormTypeUnitFlag2:
                    Text = "Select unit flags2";
                    listViewSelectableItems.Columns.Add("Flag", 235, HorizontalAlignment.Left);
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG2_NONE");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG2_FEIGN_DEATH");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG2_UNK1");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG2_IGNORE_REPUTATION");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG2_COMPREHEND_LANG");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG2_MIRROR_IMAGE");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG2_INSTANTLY_APPEAR_MODEL");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG2_FORCE_MOVEMENT");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG2_DISARM_OFFHAND");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG2_DISABLE_PRED_STATS");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG2_DISARM_RANGED");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG2_REGENERATE_POWER");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG2_RESTRICT_PARTY_INTERACTION");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG2_PREVENT_SPELL_CLICK");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG2_ALLOW_ENEMY_INTERACT");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG2_DISABLE_TURN");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG2_UNK2");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG2_PLAY_DEATH_ANIM");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_FLAG2_ALLOW_CHEAT_SPELLS");
                    break;
                case MultiSelectFormType.MultiSelectFormTypeGoFlag:
                    Text = "Select gameobject flags";
                    listViewSelectableItems.Columns.Add("Gameobject flags", 235, HorizontalAlignment.Left);
                    listViewSelectableItems.Items.Add("").SubItems.Add("GO_FLAG_NONE");
                    listViewSelectableItems.Items.Add("").SubItems.Add("GO_FLAG_IN_USE");
                    listViewSelectableItems.Items.Add("").SubItems.Add("GO_FLAG_LOCKED");
                    listViewSelectableItems.Items.Add("").SubItems.Add("GO_FLAG_INTERACT_COND");
                    listViewSelectableItems.Items.Add("").SubItems.Add("GO_FLAG_TRANSPORT");
                    listViewSelectableItems.Items.Add("").SubItems.Add("GO_FLAG_NOT_SELECTABLE");
                    listViewSelectableItems.Items.Add("").SubItems.Add("GO_FLAG_NO_DESPAWN");
                    listViewSelectableItems.Items.Add("").SubItems.Add("GO_FLAG_TRIGGERED");
                    listViewSelectableItems.Items.Add("").SubItems.Add("GO_FLAG_DAMAGED");
                    listViewSelectableItems.Items.Add("").SubItems.Add("GO_FLAG_DESTROYED");
                    break;
                case MultiSelectFormType.MultiSelectFormTypeDynamicFlag:
                    Text = "Select dynamic flags";
                    listViewSelectableItems.Columns.Add("Dynamic flags", 235, HorizontalAlignment.Left);
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_DYNFLAG_NONE");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_DYNFLAG_LOOTABLE");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_DYNFLAG_TRACK_UNIT");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_DYNFLAG_TAPPED");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_DYNFLAG_TAPPED_BY_PLAYER");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_DYNFLAG_SPECIALINFO");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_DYNFLAG_DEAD");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_DYNFLAG_REFER_A_FRIEND");
                    listViewSelectableItems.Items.Add("").SubItems.Add("UNIT_DYNFLAG_TAPPED_BY_ALL_THREAT_LIST");
                    break;
            }
        }

        private void MultiSelectForm_Load(object sender, EventArgs e)
        {
            if (textBoxToChange.Text == "0" || String.IsNullOrWhiteSpace(textBoxToChange.Text))
                listViewSelectableItems.Items[0].Checked = true;
            else
            {
                int bitmask = XConverter.TryParseStringToInt32(textBoxToChange.Text);

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
