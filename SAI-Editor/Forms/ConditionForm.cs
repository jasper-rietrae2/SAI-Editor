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
using SAI_Editor.Forms.SearchForms;

namespace SAI_Editor.Forms
{
    public partial class ConditionForm : Form
    {
        public ConditionForm()
        {
            InitializeComponent();

            comboBoxConditionSourceTypes.SelectedIndex = 0;
            comboBoxConditionTypes.SelectedIndex = 0;

            panelPermanentTooltipConditionType.BackColor = Color.FromArgb(255, 255, 225);
            panelPermanentTooltipSourceType.BackColor = Color.FromArgb(255, 255, 225);
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
                    SetSourceGroupValues("Loot entry", true);
                    SetSourceEntryValues("Item entry");
                    SetConditionTargetValues(null);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SPELL_IMPLICIT_TARGET:
                    SetSourceGroupValues("Effect mask (1, 2, 4)", true);
                    SetSourceEntryValues("Spell entry", true);
                    SetConditionTargetValues(new string[] { "Normal target", "Caster" });
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_GOSSIP_MENU:
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_GOSSIP_MENU_OPTION:
                    SetSourceGroupValues("Gossip menu entry", true);
                    SetSourceEntryValues("Gossip menu text entry", true);
                    SetConditionTargetValues(new string[] { "Player who can see the gossip text", "Object providing the gossip" });
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_CREATURE_TEMPLATE_VEHICLE:
                    SetSourceGroupValues(" - ");
                    SetSourceEntryValues("Creature entry", true);
                    SetConditionTargetValues(new string[] { "Player riding the vehicle", "Vehicle itself" });
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SPELL:
                    SetSourceGroupValues(" - ");
                    SetSourceEntryValues("Spell entry", true);
                    SetConditionTargetValues(new string[] { "Caster of the spell", "Actual spell target" });
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SPELL_CLICK_EVENT:
                    SetSourceGroupValues("Creature entry", true);
                    SetSourceEntryValues("Spell entry", true);
                    SetConditionTargetValues(new string[] { "Clicker", "Spellclick target (clickee)" });
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_QUEST_ACCEPT:
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_QUEST_SHOW_MARK:
                    SetSourceGroupValues("?");
                    SetSourceEntryValues("Quest entry", true);
                    SetConditionTargetValues(null);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_VEHICLE_SPELL:
                    SetSourceGroupValues("Creature entry", true);
                    SetSourceEntryValues("Spell entry", true);
                    SetConditionTargetValues(new string[] { "Player on vehicle", "Vehicle itself" });
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SMART_EVENT:
                    SetSourceGroupValues("Smart script id", true);
                    SetSourceEntryValues("Smart script entryorguid", true);
                    SetConditionTargetValues(new string[] { "Invoker", "Object itself" });
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_NPC_VENDOR:
                    SetSourceGroupValues("Vendor entry", true);
                    SetSourceEntryValues("Item entry", true);
                    SetConditionTargetValues(null);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SPELL_PROC:
                    SetSourceGroupValues(" - ");
                    SetSourceEntryValues("Spell id of the aura", true);
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

        private void SetSourceGroupValues(string value, bool searchable = false)
        {
            labelSourceGroup.Text = value;
            textBoxSourceGroup.Enabled = value != " - ";

            if (value == " - ") //! Empty/unused source group
                textBoxSourceGroup.Text = String.Empty;

            buttonSearchSourceGroup.Enabled = searchable;
        }

        private void SetSourceEntryValues(string value, bool searchable = false)
        {
            labelSourceEntry.Text = value;
            textBoxSourceEntry.Enabled = value != " - ";

            if (value == " - ") //! Empty/unused source Entry
                textBoxSourceEntry.Text = String.Empty;

            buttonSearchSourceEntry.Enabled = searchable;
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
            switch ((ConditionTypes)comboBoxConditionTypes.SelectedIndex)
            {
                case ConditionTypes.CONDITION_AURA:
                    SetConditionValues(new string[] { "Spell entry", "Spell effect index (0-2)", "", "" }, new bool[] { true, true, false, false });
                    break;
                case ConditionTypes.CONDITION_ITEM:
                    SetConditionValues(new string[] { "Item entry", "Item count", "In bank (0/1)", "" }, new bool[] { true, false, false, false });
                    break;
                case ConditionTypes.CONDITION_ITEM_EQUIPPED:
                    SetConditionValues(new string[] { "Item entry", "", "", "" }, new bool[] { true, false, false, false });
                    break;
                case ConditionTypes.CONDITION_ZONEID:
                    SetConditionValues(new string[] { "Zone id", "", "", "" }, new bool[] { true, false, false, false });
                    break;
                case ConditionTypes.CONDITION_REPUTATION_RANK:
                    SetConditionValues(new string[] { "Faction entry", "Facion rank", "", "" }, new bool[] { true, true, false, false });
                    break;
                case ConditionTypes.CONDITION_TEAM:
                    SetConditionValues(new string[] { "Team id", "", "", "" }, new bool[] { true, false, false, false });
                    break;
                case ConditionTypes.CONDITION_SKILL:
                    SetConditionValues(new string[] { "Skill entry", "Skill value", "", "" }, new bool[] { true, false, false, false });
                    break;
                case ConditionTypes.CONDITION_QUESTREWARDED:
                    SetConditionValues(new string[] { "Quest entry", "", "", "" }, new bool[] { true, false, false, false });
                    break;
                case ConditionTypes.CONDITION_QUESTTAKEN:
                    SetConditionValues(new string[] { "Quest entry", "", "", "" }, new bool[] { true, false, false, false });
                    break;
                case ConditionTypes.CONDITION_DRUNKENSTATE:
                    SetConditionValues(new string[] { "Drunken state", "", "", "" }, new bool[] { true, false, false, false });
                    break;
                case ConditionTypes.CONDITION_WORLD_STATE:
                    SetConditionValues(new string[] { "World state index", "World state value", "", "" }, new bool[] { true, true, false, false });
                    break;
                case ConditionTypes.CONDITION_ACTIVE_EVENT:
                    SetConditionValues(new string[] { "Game event entry", "", "", "" }, new bool[] { true, false, false, false });
                    break;
                case ConditionTypes.CONDITION_INSTANCE_INFO:
                    SetConditionValues(new string[] { "Instance entry", "Instance data", "Instance type", "" }, new bool[] { true, false, true, false });
                    break;
                case ConditionTypes.CONDITION_QUEST_NONE:
                    SetConditionValues(new string[] { "Quest entry", "", "", "" }, new bool[] { true, false, false, false });
                    break;
                case ConditionTypes.CONDITION_CLASS:
                    SetConditionValues(new string[] { "Class mask", "", "", "" }, new bool[] { true, false, false, false });
                    break;
                case ConditionTypes.CONDITION_RACE:
                    SetConditionValues(new string[] { "Race mask", "", "", "" }, new bool[] { true, false, false, false });
                    break;
                case ConditionTypes.CONDITION_ACHIEVEMENT:
                    SetConditionValues(new string[] { "Achievement id", "", "", "" }, new bool[] { true, false, false, false });
                    break;
                case ConditionTypes.CONDITION_TITLE:
                    SetConditionValues(new string[] { "Title id", "", "", "" }, new bool[] { true, false, false, false });
                    break;
                case ConditionTypes.CONDITION_SPAWNMASK:
                    SetConditionValues(new string[] { "Spawnmask", "", "", "" }, new bool[] { true, false, false, false });
                    break;
                case ConditionTypes.CONDITION_GENDER:
                    SetConditionValues(new string[] { "Gender", "", "", "" }, new bool[] { true, false, false, false });
                    break;
                case ConditionTypes.CONDITION_UNIT_STATE:
                    SetConditionValues(new string[] { "Unit state", "", "", "" }, new bool[] { true, false, false, false });
                    break;
                case ConditionTypes.CONDITION_MAPID:
                    SetConditionValues(new string[] { "Map id", "", "", "" }, new bool[] { true, false, false, false });
                    break;
                case ConditionTypes.CONDITION_AREAID:
                    SetConditionValues(new string[] { "Area id", "", "", "" }, new bool[] { true, false, false, false });
                    break;
                case ConditionTypes.CONDITION_CREATURE_TYPE:
                    //SetConditionValues(new string[] { "", "", "", "" }, new bool[] { true, false, false, false });
                    break;
                case ConditionTypes.CONDITION_SPELL:
                    SetConditionValues(new string[] { "Spell entry", "", "", "" }, new bool[] { true, false, false, false });
                    break;
                case ConditionTypes.CONDITION_PHASEMASK:
                    SetConditionValues(new string[] { "Phasemask", "", "", "" }, new bool[] { true, false, false, false });
                    break;
                case ConditionTypes.CONDITION_LEVEL:
                    SetConditionValues(new string[] { "Player level", "Compare type", "", "" }, new bool[] { false, true, false, false });
                    break;
                case ConditionTypes.CONDITION_QUEST_COMPLETE:
                    SetConditionValues(new string[] { "Quest entry", "", "", "" }, new bool[] { true, false, false, false });
                    break;
                case ConditionTypes.CONDITION_NEAR_CREATURE:
                    SetConditionValues(new string[] { "Creature entry", "Distance", "", "" }, new bool[] { true, false, false, false });
                    break;
                case ConditionTypes.CONDITION_NEAR_GAMEOBJECT:
                    SetConditionValues(new string[] { "Gameobject entry", "Distance", "", "" }, new bool[] { true, false, false, false });
                    break;
                case ConditionTypes.CONDITION_OBJECT_ENTRY:
                    SetConditionValues(new string[] { "Type id", "Object entry", "", "" }, new bool[] { true, true, false, false });
                    break;
                case ConditionTypes.CONDITION_TYPE_MASK:
                    SetConditionValues(new string[] { "Typemask", "", "", "" }, new bool[] { true, false, false, false });
                    break;
                case ConditionTypes.CONDITION_RELATION_TO:
                    SetConditionValues(new string[] { "Condition target", "Relation type", "", "" }, new bool[] { true, true, false, false });
                    break;
                case ConditionTypes.CONDITION_REACTION_TO:
                    SetConditionValues(new string[] { "Condition target", "Rank mask", "", "" }, new bool[] { true, true, false, false });
                    break;
                case ConditionTypes.CONDITION_DISTANCE_TO:
                    SetConditionValues(new string[] { "Condition target", "Distance", "Compare type", "" }, new bool[] { true, true, true, false });
                    break;
                case ConditionTypes.CONDITION_ALIVE:
                    SetConditionValues(new string[] { "", "", "", "Condition (0/1)" }, new bool[] { false, false, false, false });
                    break;
                case ConditionTypes.CONDITION_HP_VAL:
                    SetConditionValues(new string[] { "Health points", "Compare type", "", "" }, new bool[] { false, true, false, false });
                    break;
                case ConditionTypes.CONDITION_HP_PCT:
                    SetConditionValues(new string[] { "Health percentage", "Compare type", "", "" }, new bool[] { false, true, false, false });
                    break;
                default:
                    SetConditionValues(new string[] { "", "", "", "" }, new bool[] { false, false, false, false });
                    break;
            }
        }

        private void SetConditionValues(string[] values, bool[] searchables)
        {
            Dictionary<string, Control> condValues = new Dictionary<string, Control>()
            {
                { "1lbl", labelCondValue1 }, { "2lbl", labelCondValue2 }, { "3lbl", labelCondValue3 }, { "4lbl", labelCondValue4 },
                { "1txt", textBoxCondValue1 }, { "2txt", textBoxCondValue2 }, { "3txt", textBoxCondValue3 }, { "4txt", textBoxCondValue4 },
                { "1btn", buttonSearchConditionValue1 }, { "2btn", buttonSearchConditionValue2 }, { "3btn", buttonSearchConditionValue3 }, { "4btn", buttonSearchConditionValue4 },
            };

            for (int i = 0; i < 4; ++i)
            {
                string value = values[i];
                bool searchable = searchables[i];
                string iKey = (i + 1).ToString();

                if (!String.IsNullOrWhiteSpace(value))
                {
                    condValues[iKey.ToString() + "lbl"].Text = value;
                    condValues[iKey.ToString() + "txt"].Enabled = value != " - ";

                    if (value == " - ") //! Empty/unused source Entry
                        condValues[iKey.ToString() + "txt"].Text = String.Empty;

                    condValues[iKey.ToString() + "btn"].Enabled = searchable;
                }
            }
        }

        private void buttonSearchSourceGroup_Click(object sender, EventArgs e)
        {
            switch ((ConditionSourceTypes)comboBoxConditionSourceTypes.SelectedIndex)
            {
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_CREATURE_LOOT_TEMPLATE:
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_DISENCHANT_LOOT_TEMPLATE:
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_FISHING_LOOT_TEMPLATE:
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_GAMEOBJECT_LOOT_TEMPLATE:
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_ITEM_LOOT_TEMPLATE:
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_MAIL_LOOT_TEMPLATE:
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_MILLING_LOOT_TEMPLATE:
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_PICKPOCKETING_LOOT_TEMPLATE:
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_PROSPECTING_LOOT_TEMPLATE:
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_REFERENCE_LOOT_TEMPLATE:
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SKINNING_LOOT_TEMPLATE:
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SPELL_LOOT_TEMPLATE:
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SPELL_IMPLICIT_TARGET:
                    using (SingleSelectForm<SpellEffIndex> singleSelectForm = new SingleSelectForm<SpellEffIndex>(textBoxSourceGroup))
                        singleSelectForm.ShowDialog(this);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_GOSSIP_MENU:
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_GOSSIP_MENU_OPTION:
                    ShowSearchFromDatabaseForm(textBoxSourceGroup, DatabaseSearchFormType.DatabaseSearchFormTypeGossipMenuOptionId);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SPELL_CLICK_EVENT:
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_VEHICLE_SPELL:
                    ShowSearchFromDatabaseForm(textBoxSourceGroup, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_QUEST_ACCEPT:
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_QUEST_SHOW_MARK:
                    //????
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SMART_EVENT:
                    //????
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_NPC_VENDOR:
                    //ShowSearchFromDatabaseForm(textBoxSourceGroup, DatabaseSearchFormType.Vendor);
                    break;
                //case ConditionSourceTypes.CONDITION_SOURCE_TYPE_PHASE_DEFINITION:
                //    break;
                default:
                    break;
            }
        }

        private void ShowSearchFromDatabaseForm(TextBox textBoxToChange, DatabaseSearchFormType searchType)
        {
            using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(textBoxToChange, searchType))
                searchFromDatabaseForm.ShowDialog(this);
        }

        private void buttonSearchSourceEntry_Click(object sender, EventArgs e)
        {
            switch ((ConditionSourceTypes)comboBoxConditionSourceTypes.SelectedIndex)
            {
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_CREATURE_LOOT_TEMPLATE:
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_DISENCHANT_LOOT_TEMPLATE:
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_FISHING_LOOT_TEMPLATE:
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_GAMEOBJECT_LOOT_TEMPLATE:
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_ITEM_LOOT_TEMPLATE:
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_MAIL_LOOT_TEMPLATE:
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_MILLING_LOOT_TEMPLATE:
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_PICKPOCKETING_LOOT_TEMPLATE:
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_PROSPECTING_LOOT_TEMPLATE:
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_REFERENCE_LOOT_TEMPLATE:
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SKINNING_LOOT_TEMPLATE:
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SPELL_LOOT_TEMPLATE:
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_GOSSIP_MENU:
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_GOSSIP_MENU_OPTION:
                    ShowSearchFromDatabaseForm(textBoxSourceEntry, DatabaseSearchFormType.DatabaseSearchFormTypeGossipMenuOptionId);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_CREATURE_TEMPLATE_VEHICLE:
                    ShowSearchFromDatabaseForm(textBoxSourceEntry, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SPELL_IMPLICIT_TARGET:
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SPELL:
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SPELL_PROC:
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SPELL_CLICK_EVENT:
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_VEHICLE_SPELL:
                    ShowSearchFromDatabaseForm(textBoxSourceEntry, DatabaseSearchFormType.DatabaseSearchFormTypeSpell);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_QUEST_ACCEPT:
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_QUEST_SHOW_MARK:
                    ShowSearchFromDatabaseForm(textBoxSourceEntry, DatabaseSearchFormType.DatabaseSearchFormTypeQuest);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SMART_EVENT:
                    //ShowSearchFromDatabaseForm(textBoxSourceEntry, DatabaseSearchFormType.);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_NPC_VENDOR:
                    ShowSearchFromDatabaseForm(textBoxSourceEntry, DatabaseSearchFormType.DatabaseSearchFormTypeItemEntry);
                    break;
                //case ConditionSourceTypes.CONDITION_SOURCE_TYPE_PHASE_DEFINITION:
                //    break;
                default:
                    break;
            }
        }

        private void buttonSearchConditionValue1_Click(object sender, EventArgs e)
        {

        }

        private void buttonSearchConditionValue2_Click(object sender, EventArgs e)
        {

        }

        private void buttonSearchConditionValue3_Click(object sender, EventArgs e)
        {

        }
    }
}
