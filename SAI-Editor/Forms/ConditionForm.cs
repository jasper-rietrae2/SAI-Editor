using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SAI_Editor.Enumerators;

namespace SAI_Editor.Forms
{
    public partial class ConditionForm : Form
    {
        public ConditionForm()
        {
            InitializeComponent();

            comboBoxConditionSourceTypes.SelectedIndex = 0;
            comboBoxConditionTypes.SelectedIndex = 0;
        }

        private void comboBoxConditionSourceTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((ConditionSourceTypes)comboBoxConditionSourceTypes.SelectedIndex)
            {
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_CREATURE_LOOT_TEMPLATE:
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_DISENCHANT_LOOT_TEMPLATE:
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_FISHING_LOOT_TEMPLATE:
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_GAMEOBJECT_LOOT_TEMPLATE:
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_ITEM_LOOT_TEMPLATE:
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_MAIL_LOOT_TEMPLATE:
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_MILLING_LOOT_TEMPLATE:
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_PICKPOCKETING_LOOT_TEMPLATE:
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_PROSPECTING_LOOT_TEMPLATE:
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_REFERENCE_LOOT_TEMPLATE:
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SKINNING_LOOT_TEMPLATE:
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SPELL_LOOT_TEMPLATE:
                    SetSourceGroupValues("Loot entry");
                    SetSourceEntryValues("Item entry");
                    SetConditionTargetValues(null);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SPELL_IMPLICIT_TARGET:
                    SetSourceGroupValues("Effect mask (1, 2, 4)");
                    SetSourceEntryValues("Spell entry");
                    SetConditionTargetValues(new string[] { "Normal target", "Caster" });
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_GOSSIP_MENU:
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_GOSSIP_MENU_OPTION:
                    SetSourceGroupValues("Gossip menu entry");
                    SetSourceEntryValues("Gossip menu text entry");
                    SetConditionTargetValues(new string[] { "Player who can see the gossip text", "Object providing the gossip" });
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_CREATURE_TEMPLATE_VEHICLE:
                    SetSourceGroupValues(" - ");
                    SetSourceEntryValues("Creature entry");
                    SetConditionTargetValues(new string[] { "Player riding the vehicle", "Vehicle itself" });
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SPELL:
                    SetSourceGroupValues(" - ");
                    SetSourceEntryValues("Spell entry");
                    SetConditionTargetValues(new string[] { "Caster of the spell", "Actual spell target" });
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SPELL_CLICK_EVENT:
                    SetSourceGroupValues("Creature entry");
                    SetSourceEntryValues("Spell entry");
                    SetConditionTargetValues(new string[] { "Clicker", "Spellclick target (clickee)" });
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_QUEST_ACCEPT:
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_QUEST_SHOW_MARK:
                    SetSourceGroupValues("?");
                    SetSourceEntryValues("Quest entry");
                    SetConditionTargetValues(null);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_VEHICLE_SPELL:
                    SetSourceGroupValues("Creature entry");
                    SetSourceEntryValues("Spell entry");
                    SetConditionTargetValues(new string[] { "Player on vehicle", "Vehicle itself" });
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SMART_EVENT:
                    SetSourceGroupValues("Smart script id");
                    SetSourceEntryValues("Smart script entryorguid");
                    SetConditionTargetValues(new string[] { "Invoker", "Object itself" });
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_NPC_VENDOR:
                    SetSourceGroupValues("Vendor entry");
                    SetSourceEntryValues("Item entry");
                    SetConditionTargetValues(null);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SPELL_PROC:
                    SetSourceGroupValues(" - ");
                    SetSourceEntryValues("Spell id of the aura");
                    SetConditionTargetValues(new string[] { "Actor", "Action target" });
                    break;
                //case ConditionSourceTypes.CONDITION_SOURCE_TYPE_PHASE_DEFINITION:
                //    SetSourceGroupValues("";
                //    SetSourceEntryValues("";
                //    SetConditionTargetValues(new string[] { "", "" });
                //    break;
                default:
                    SetSourceGroupValues(" - ");
                    SetSourceEntryValues(" - ");
                    SetConditionTargetValues(null);
                    break;
            }
        }

        private void comboBoxesConditions_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar) || Char.IsNumber(e.KeyChar))
                e.Handled = true; //! Disallow changing content of the combobox, but setting it to 3D looks like shit
        }

        private void SetSourceGroupValues(string value)
        {
            labelSourceGroup.Text = value;
            textBoxSourceGroup.Enabled = value != " - ";

            if (value == " - ") //! Empty/unused source group
                textBoxSourceGroup.Text = String.Empty;
        }

        private void SetSourceEntryValues(string value)
        {
            labelSourceEntry.Text = value;
            textBoxSourceEntry.Enabled = value != " - ";

            if (value == " - ") //! Empty/unused source Entry
                textBoxSourceEntry.Text = String.Empty;
        }

        private void SetConditionTargetValues(string[] items)
        {
            comboBoxConditionTarget.Items.Clear();
            comboBoxConditionTarget.Enabled = items != null;

            if (items != null && items.Length > 0)
            {
                foreach (string item in items)
                    comboBoxConditionTarget.Items.Add(item);
            }
            else
                comboBoxConditionTarget.Items.Add("Always 0");

            if (comboBoxConditionTarget.Items.Count > 0)
                comboBoxConditionTarget.SelectedIndex = 0;
        }

        private void comboBoxConditionTypes_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
