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
using SAI_Editor.Classes.Database.Classes;
using SAI_Editor.Classes;

namespace SAI_Editor.Forms
{
    public partial class ConditionForm : Form
    {
        private List<Condition> conditions = new List<Condition>();
        public bool formHidden = false;

        public ConditionForm()
        {
            InitializeComponent();

            comboBoxConditionSourceTypes.SelectedIndex = 0;
            comboBoxConditionTypes.SelectedIndex = 0;
        }

        private void comboBoxConditionSourceTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            //! Reset the values
            labelSourceGroup.Text = " - ";
            labelSourceEntry.Text = " - ";
            labelElseGroup.Text = " - ";
            SetConditionTargetValues(null);

            //! Source id is only available for SMART_EVENT cond type
            ConditionSourceTypes selectedType = (ConditionSourceTypes)comboBoxConditionSourceTypes.SelectedIndex;
            buttonSearchSourceId.Enabled = selectedType == ConditionSourceTypes.CONDITION_SOURCE_TYPE_SMART_EVENT;

            switch (selectedType)
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
                    SetSourceEntryValues("Item entry", true);
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
                    SetSourceGroupValues(" - ");
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
                    SetSourceIdValues("Smart script sourcetype", true);
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
                default:
                    break;
            }
        }

        private void comboBoxesConditions_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar) || Char.IsDigit(e.KeyChar))
                e.Handled = true; //! Disallow changing content of the combobox, but setting it to 3D looks like shit
        }

        private void SetSourceGroupValues(string value, bool searchable = false)
        {
            labelSourceGroup.Text = value;
            buttonSearchSourceGroup.Enabled = searchable;
        }

        private void SetSourceEntryValues(string value, bool searchable = false)
        {
            labelSourceEntry.Text = value;
            buttonSearchSourceEntry.Enabled = searchable;
        }

        private void SetSourceIdValues(string value, bool searchable = false)
        {
            labelSourceId.Text = value;
            buttonSearchSourceId.Enabled = searchable;
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
            //! Reset the values
            SetConditionValues(new string[] { "", "", "", "" }, new bool[] { false, false, false, false });

            switch ((ConditionTypes)comboBoxConditionTypes.SelectedIndex)
            {
                case ConditionTypes.CONDITION_AURA:
                    SetConditionValues(new string[] { "Spell entry", "Spell effect index", "", "" }, new bool[] { true, true, false, false });
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
                    SetConditionValues(new string[] { "World state index", "World state value", "", "" }, new bool[] { false, false, false, false });
                    break;
                case ConditionTypes.CONDITION_ACTIVE_EVENT:
                    SetConditionValues(new string[] { "Game event entry", "", "", "" }, new bool[] { true, false, false, false });
                    break;
                case ConditionTypes.CONDITION_INSTANCE_INFO:
                    SetConditionValues(new string[] { "Data value", "Data outcome", "Compare type", "" }, new bool[] { false, false, true, false });
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
                    SetConditionValues(new string[] { "Creature type", "", "", "" }, new bool[] { true, false, false, false });
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
                    SetConditionValues(new string[] { "Condition target", "Relation type", "", "" }, new bool[] { false, true, false, false });
                    break;
                case ConditionTypes.CONDITION_REACTION_TO:
                    SetConditionValues(new string[] { "Condition target", "Rank mask", "", "" }, new bool[] { false, true, false, false });
                    break;
                case ConditionTypes.CONDITION_DISTANCE_TO:
                    SetConditionValues(new string[] { "Condition target", "Distance", "Compare type", "" }, new bool[] { false, false, true, false });
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
                condValues[(i + 1).ToString() + "lbl"].Text = String.IsNullOrWhiteSpace(values[i]) ? " - " : values[i]; //! Label
                condValues[(i + 1).ToString() + "btn"].Enabled = searchables[i]; //! Button
            }
        }

        private void buttonSearchSourceGroup_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxSourceGroup;

            switch ((ConditionSourceTypes)comboBoxConditionSourceTypes.SelectedIndex)
            {
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_CREATURE_LOOT_TEMPLATE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureLootTemplateEntry);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_DISENCHANT_LOOT_TEMPLATE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeDisenchantLootTemplateEntry);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_FISHING_LOOT_TEMPLATE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeFishingLootTemplateEntry);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_GAMEOBJECT_LOOT_TEMPLATE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectLootTemplateEntry);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_ITEM_LOOT_TEMPLATE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeItemLootTemplateEntry);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_MAIL_LOOT_TEMPLATE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeMailLootTemplateEntry);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_MILLING_LOOT_TEMPLATE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeMillingLootTemplateEntry);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_PICKPOCKETING_LOOT_TEMPLATE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypePickpocketingLootTemplateEntry);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_PROSPECTING_LOOT_TEMPLATE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeProspectingLootTemplateEntry);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_REFERENCE_LOOT_TEMPLATE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeReferenceLootTemplateEntry);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SKINNING_LOOT_TEMPLATE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSkinningLootTemplateEntry);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SPELL_LOOT_TEMPLATE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSpellLootTemplateEntry);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SPELL_IMPLICIT_TARGET:
                    ShowSelectForm("SpellEffIndex", textBoxToChange);
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
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSmartScriptId);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_NPC_VENDOR:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeVendorEntry);
                    break;
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
            TextBox textBoxToChange = textBoxSourceEntry;

            switch ((ConditionSourceTypes)comboBoxConditionSourceTypes.SelectedIndex)
            {
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_CREATURE_LOOT_TEMPLATE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureLootTemplateItem);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_DISENCHANT_LOOT_TEMPLATE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeDisenchantLootTemplateItem);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_FISHING_LOOT_TEMPLATE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeFishingLootTemplateItem);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_GAMEOBJECT_LOOT_TEMPLATE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectLootTemplateItem);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_ITEM_LOOT_TEMPLATE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeItemLootTemplateItem);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_MAIL_LOOT_TEMPLATE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeMailLootTemplateItem);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_MILLING_LOOT_TEMPLATE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeMillingLootTemplateItem);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_PICKPOCKETING_LOOT_TEMPLATE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypePickpocketingLootTemplateItem);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_PROSPECTING_LOOT_TEMPLATE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeProspectingLootTemplateItem);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_REFERENCE_LOOT_TEMPLATE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeReferenceLootTemplateItem);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SKINNING_LOOT_TEMPLATE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSkinningLootTemplateItem);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SPELL_LOOT_TEMPLATE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSpellLootTemplateItem);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_GOSSIP_MENU:
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_GOSSIP_MENU_OPTION:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGossipMenuOptionId);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_CREATURE_TEMPLATE_VEHICLE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SPELL_IMPLICIT_TARGET:
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SPELL:
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SPELL_PROC:
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SPELL_CLICK_EVENT:
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_VEHICLE_SPELL:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSpell);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_QUEST_ACCEPT:
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_QUEST_SHOW_MARK:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeQuest);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SMART_EVENT:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSmartScriptEntryOrGuid);
                    break;
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_NPC_VENDOR:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeVendorItemEntry);
                    break;
                default:
                    break;
            }
        }

        private void buttonSearchSourceId_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxElseGroup;

            switch ((ConditionSourceTypes)comboBoxConditionSourceTypes.SelectedIndex)
            {
                case ConditionSourceTypes.CONDITION_SOURCE_TYPE_SMART_EVENT:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSmartScriptSourceType);
                    break;
            }
        }

        private void buttonSearchConditionValue1_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxCondValue1;

            switch ((ConditionTypes)comboBoxConditionTypes.SelectedIndex)
            {
                case ConditionTypes.CONDITION_AURA:
                case ConditionTypes.CONDITION_SPELL:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSpell);
                    break;
                case ConditionTypes.CONDITION_ITEM:
                case ConditionTypes.CONDITION_ITEM_EQUIPPED:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeItemEntry);
                    break;
                case ConditionTypes.CONDITION_ZONEID:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeAreaOrZone);
                    break;
                case ConditionTypes.CONDITION_REPUTATION_RANK:
                case ConditionTypes.CONDITION_TEAM: //! Team id and faction rank are the same, apparently. Both use Faction.dbc
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeFaction);
                    break;
                case ConditionTypes.CONDITION_SKILL:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSkill);
                    break;
                case ConditionTypes.CONDITION_QUESTREWARDED:
                case ConditionTypes.CONDITION_QUESTTAKEN:
                case ConditionTypes.CONDITION_QUEST_NONE:
                case ConditionTypes.CONDITION_QUEST_COMPLETE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeQuest);
                    break;
                case ConditionTypes.CONDITION_DRUNKENSTATE:
                    ShowSelectForm("DrunkenState", textBoxToChange);
                    break;
                case ConditionTypes.CONDITION_ACTIVE_EVENT:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGameEvent);
                    break;
                case ConditionTypes.CONDITION_CLASS:
                    ShowSelectForm("Classes", textBoxToChange);
                    break;
                case ConditionTypes.CONDITION_RACE:
                    ShowSelectForm("Races", textBoxToChange);
                    break;
                case ConditionTypes.CONDITION_ACHIEVEMENT:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeAchievement);
                    break;
                case ConditionTypes.CONDITION_TITLE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypePlayerTitles);
                    break;
                case ConditionTypes.CONDITION_SPAWNMASK:
                    ShowSelectForm("SpawnMask", textBoxToChange);
                    break;
                case ConditionTypes.CONDITION_GENDER:
                    ShowSelectForm("Gender", textBoxToChange);
                    break;
                case ConditionTypes.CONDITION_UNIT_STATE:
                    ShowSelectForm("UnitState", textBoxToChange);
                    break;
                case ConditionTypes.CONDITION_MAPID:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeMap);
                    break;
                case ConditionTypes.CONDITION_AREAID:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeAreaOrZone);
                    break;
                case ConditionTypes.CONDITION_CREATURE_TYPE:
                    ShowSelectForm("CreatureType", textBoxToChange);
                    break;
                case ConditionTypes.CONDITION_PHASEMASK:
                    ShowSelectForm("PhaseMasks", textBoxToChange);
                    break;
                case ConditionTypes.CONDITION_NEAR_CREATURE:
                    ShowSearchFromDatabaseForm(textBoxSourceEntry, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry);
                    break;
                case ConditionTypes.CONDITION_NEAR_GAMEOBJECT:
                    ShowSearchFromDatabaseForm(textBoxSourceEntry, DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectEntry);
                    break;
                case ConditionTypes.CONDITION_OBJECT_ENTRY:
                    ShowSelectForm("TypeID", textBoxToChange);
                    break;
                case ConditionTypes.CONDITION_TYPE_MASK:
                    ShowSelectForm("TypeMask", textBoxToChange);
                    break;
                default:
                    break;
            }
        }

        private void buttonSearchConditionValue2_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxCondValue2;

            switch ((ConditionTypes)comboBoxConditionTypes.SelectedIndex)
            {
                case ConditionTypes.CONDITION_AURA:
                    ShowSelectForm("SpellEffIndex", textBoxToChange);
                    break;
                case ConditionTypes.CONDITION_REPUTATION_RANK:
                    ShowSelectForm("ReputationRank", textBoxToChange);
                    break;
                case ConditionTypes.CONDITION_OBJECT_ENTRY:
                    string condValue1 = textBoxCondValue1.Text;
                    bool showError = false;

                    if (!String.IsNullOrWhiteSpace(condValue1))
                    {
                        int intCondValue1;

                        if (Int32.TryParse(condValue1, out intCondValue1))
                        {
                            if (intCondValue1 < 0 || intCondValue1 > (int)TypeID.TYPEID_CORPSE)
                                showError = true;
                            else
                            {
                                switch ((TypeID)intCondValue1)
                                {
                                    case TypeID.TYPEID_UNIT:
                                        ShowSearchFromDatabaseForm(textBoxSourceEntry, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry);
                                        break;
                                    case TypeID.TYPEID_GAMEOBJECT:
                                        ShowSearchFromDatabaseForm(textBoxSourceEntry, DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectEntry);
                                        break;
                                    case TypeID.TYPEID_PLAYER:
                                    case TypeID.TYPEID_CORPSE:
                                        MessageBox.Show("The type ID's for players and corpses don't require an entry.", "Not required!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        break;
                                    default:
                                        showError = true;
                                        break;
                                }
                            }
                        }
                    }
                    else
                        showError = true;

                    if (showError)
                        MessageBox.Show("This condition type requires the condition value 1 to be a proper Type ID in order to search for the second parameter.", "Required field missing!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case ConditionTypes.CONDITION_RELATION_TO:
                    ShowSelectForm("CondRelationType", textBoxToChange);
                    break;
                case ConditionTypes.CONDITION_REACTION_TO:
                    ShowSelectForm("ReputationRankMask", textBoxToChange);
                    break;
                case ConditionTypes.CONDITION_HP_VAL:
                case ConditionTypes.CONDITION_HP_PCT:
                    ShowSelectForm("ComparisionType", textBoxToChange);
                    break;
                default:
                    break;
            }
        }

        private void buttonSearchConditionValue3_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxCondValue3;

            switch ((ConditionTypes)comboBoxConditionTypes.SelectedIndex)
            {
                case ConditionTypes.CONDITION_INSTANCE_INFO:
                    ShowSelectForm("InstanceInfo", textBoxToChange);
                    break;
                case ConditionTypes.CONDITION_DISTANCE_TO:
                    ShowSelectForm("ComparisionType", textBoxToChange);
                    break;
            }
        }

        private void ShowSelectForm(string formTemplate, TextBox textBoxToChange)
        {
            using (Form selectForm = (Form)Activator.CreateInstance(SAI_Editor_Manager.SearchFormsContainer[formTemplate], new object[] { textBoxToChange }))
                selectForm.ShowDialog(this);
        }

        private void ConditionForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Hide();
                    break;
            }
        }

        private void buttonGenerateSql_Click(object sender, EventArgs e)
        {
            string sql = String.Empty;

            switch (conditions.Count)
            {
                case 0:
                    MessageBox.Show("There are no conditions in this session.", "No conditions!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                case 1:
                    sql  = "DELETE FROM `conditions` WHERE `SourceTypeOrReferenceId`=" + conditions[0].SourceTypeOrReferenceId;
                    sql += " AND `SourceGroup`=" + conditions[0].SourceGroup + " AND `SourceEntry`=" + conditions[0].SourceEntry + ";\n";
                    sql += "INSERT INTO `conditions` (`SourceTypeOrReferenceId`,`SourceGroup`,`SourceEntry`,`SourceId`,`ElseGroup`,`ConditionTypeOrReference`,`ConditionTarget`,`ConditionValue1`,`ConditionValue2`,`ConditionValue3`,`ErrorTextId`,`ScriptName`,`Comment`) VALUES\n";
                    sql += "(" + conditions[0].SourceTypeOrReferenceId + "," + conditions[0].SourceGroup + "," + conditions[0].SourceEntry;
                    sql += "," + conditions[0].SourceId + "," + conditions[0].ElseGroup + "," + conditions[0].ConditionTypeOrReference;
                    sql += "," + conditions[0].ConditionTarget + "," + conditions[0].ConditionValue1;
                    sql += "," + conditions[0].ConditionValue2 + "," + conditions[0].ConditionValue3 + "," + conditions[0].ErrorTextId;
                    sql += "," + '"' + conditions[0].ScriptName + '"' + "," + '"' + conditions[0].Comment + '"' + ");";
                    break;
                default:
                    string deleteFromString = String.Empty;
                    sql =  "_replaceThisWithDeleteFrom_";
                    sql += "INSERT INTO `conditions` (`SourceTypeOrReferenceId`,`SourceGroup`,`SourceEntry`,`SourceId`,`ElseGroup`,`ConditionTypeOrReference`,`ConditionTarget`,`ConditionValue1`,`ConditionValue2`,`ConditionValue3`,`ErrorTextId`,`ScriptName`,`Comment`) VALUES\n";

                    for (int i = 0; i < conditions.Count; ++i)
                    {
                        Condition condition = conditions[i];

                        deleteFromString += "DELETE FROM `conditions` WHERE `SourceTypeOrReferenceId`=" + condition.SourceTypeOrReferenceId;
                        deleteFromString += " AND `SourceGroup`=" + condition.SourceGroup + " AND `SourceEntry`=" + condition.SourceEntry + ";\n";

                        sql += "(" + condition.SourceTypeOrReferenceId + "," + condition.SourceGroup + "," + condition.SourceEntry;
                        sql += "," + condition.SourceId + "," + condition.ElseGroup + "," + condition.ConditionTypeOrReference;
                        sql += "," + condition.ConditionTarget + "," + condition.ConditionValue1;
                        sql += "," + condition.ConditionValue2 + "," + condition.ConditionValue3 + "," + condition.ErrorTextId;
                        sql += "," + '"' + condition.ScriptName + '"' + "," + '"' + condition.Comment + '"' + ")";
                        sql += (i != conditions.Count - 1) ? ",\n" : ";\n";
                    }

                    sql = sql.Replace("_replaceThisWithDeleteFrom_", deleteFromString);
                    break;
            }

            using (SqlOutputForm sqlOutputForm = new SqlOutputForm(sql, false))
                sqlOutputForm.ShowDialog();
        }

        private void buttonGenerateComment_Click(object sender, EventArgs e)
        {

        }

        private void buttonSaveCondition_Click(object sender, EventArgs e)
        {
            Condition condition = new Condition();
            condition.SourceTypeOrReferenceId = comboBoxConditionSourceTypes.SelectedIndex;
            condition.SourceGroup = XConverter.ToInt32(textBoxSourceGroup.Text);
            condition.SourceEntry = XConverter.ToInt32(textBoxSourceEntry.Text);
            condition.SourceId = XConverter.ToInt32(textBoxSourceId.Text);
            condition.ElseGroup = XConverter.ToInt32(textBoxElseGroup.Text);
            condition.ConditionTypeOrReference = comboBoxConditionTypes.SelectedIndex;
            condition.ConditionTarget = comboBoxConditionTarget.SelectedIndex;
            condition.ConditionValue1 = XConverter.ToInt32(textBoxCondValue1.Text);
            condition.ConditionValue2 = XConverter.ToInt32(textBoxCondValue2.Text);
            condition.ConditionValue3 = XConverter.ToInt32(textBoxCondValue3.Text);
            condition.NegativeCondition = XConverter.ToInt32(textBoxCondValue4.Text);
            condition.ErrorType = XConverter.ToInt32(textBoxErrorType.Text);
            condition.ErrorTextId = XConverter.ToInt32(textBoxErrorTextId.Text);
            condition.ScriptName = textBoxScriptName.Text;
            condition.Comment = textBoxComment.Text;
            conditions.Add(condition);

            listViewConditions.AddCondition(condition, selectNewItem: true);
            tabControl.SelectedIndex = 1;

            ClearAllFields();
        }

        private void ClearAllFields()
        {
            foreach (Control control in tabControl.TabPages[0].Controls)
                if (control is TextBox)
                    control.Text = "0";

            labelSourceGroup.Text = " - ";
            labelSourceEntry.Text = " - ";
            labelCondValue1.Text = " - ";
            labelCondValue2.Text = " - ";
            labelCondValue3.Text = " - ";
            labelCondValue4.Text = " - ";
        }

        private void buttonDeleteCondition_Click(object sender, EventArgs e)
        {
            conditions.Remove(listViewConditions.SelectedCondition);

            listViewConditions.RemoveCondition(listViewConditions.SelectedCondition);

            if (listViewConditions.Items.Count > 0)
                listViewConditions.Items[0].Selected = true;
        }

        private void buttonLoadCondition_Click(object sender, EventArgs e)
        {
            ClearAllFields();

            Condition selectedCond = listViewConditions.SelectedCondition;
            comboBoxConditionSourceTypes.SelectedIndex = listViewConditions.SelectedCondition.SourceTypeOrReferenceId;
            textBoxSourceGroup.Text = selectedCond.SourceGroup.ToString();
            textBoxSourceEntry.Text = selectedCond.SourceEntry.ToString();
            textBoxElseGroup.Text = selectedCond.ElseGroup.ToString();
            textBoxSourceId.Text = selectedCond.SourceId.ToString();
            comboBoxConditionTypes.SelectedIndex = listViewConditions.SelectedCondition.ConditionTypeOrReference;
            comboBoxConditionTarget.SelectedIndex = selectedCond.ConditionTarget;
            textBoxCondValue1.Text = selectedCond.ConditionValue1.ToString();
            textBoxCondValue2.Text = selectedCond.ConditionValue2.ToString();
            textBoxCondValue3.Text = selectedCond.ConditionValue3.ToString();
            textBoxCondValue4.Text = selectedCond.NegativeCondition.ToString();
            textBoxErrorType.Text = selectedCond.ErrorType.ToString();
            textBoxErrorTextId.Text = selectedCond.ErrorTextId.ToString();
            textBoxScriptName.Text = selectedCond.ScriptName;
            textBoxComment.Text = selectedCond.Comment;

            tabControl.SelectedIndex = 0;
        }

        private void buttonDuplicateCondition_Click(object sender, EventArgs e)
        {
            listViewConditions.AddCondition(listViewConditions.SelectedCondition, selectNewItem: true);
        }

        private void listViewConditions_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonDeleteCondition.Enabled = listViewConditions.SelectedIndices.Count > 0;
            buttonDuplicateCondition.Enabled = listViewConditions.SelectedIndices.Count > 0;
            buttonLoadCondition.Enabled = listViewConditions.SelectedIndices.Count > 0;
        }

        private void buttonSearchErrorType_Click(object sender, EventArgs e)
        {
            ShowSelectForm("SpellCastResult", textBoxErrorType);
        }

        private void buttonSearchErrorTextId_Click(object sender, EventArgs e)
        {
            ShowSelectForm("SpellCustomErrors", textBoxErrorTextId);
        }

        private void textBoxesConditionEditor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsControl(e.KeyChar) && !Char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '-')
                e.Handled = true;
        }

        private void textBoxesConditionEditor_Leave(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (String.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "0";
                textBox.SelectionStart = 1;
            }
        }

        private void buttonResetSession_Click(object sender, EventArgs e)
        {
            ResetSession();
        }

        private void ResetSession()
        {
            conditions.Clear();
            listViewConditions.ReplaceConditions(new List<Condition>());
            labelSourceGroup.Text = " - ";
            labelSourceEntry.Text = " - ";
            labelElseGroup.Text = " - ";
            SetConditionTargetValues(null);
            SetConditionValues(new string[] { "", "", "", "" }, new bool[] { false, false, false, false });
            listViewConditions.Items.Clear();

            foreach (Control control in tabControl.TabPages[0].Controls)
            {
                if (control is TextBox && control.Name != "textBoxScriptName" && control.Name != "textBoxComment")
                    control.Text = "0";
                else if (control is ComboBox)
                    (control as ComboBox).SelectedIndex = 0;
                else if (control is Button && control.Text == "...")
                    control.Enabled = false;
            }
        }

        //! Don't allow closign the condition editor. It will automatically hide itself so the session is never lost.
        private void ConditionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formHidden)
                return;

            e.Cancel = true;
            Hide();
            formHidden = true;
        }
    }
}
