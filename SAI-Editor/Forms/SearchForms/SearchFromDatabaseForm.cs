using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using SAI_Editor.Classes;

namespace SAI_Editor.Forms.SearchForms
{
    public enum DatabaseSearchFormType
    {
        DatabaseSearchFormTypeSpell,
        DatabaseSearchFormTypeFaction,
        DatabaseSearchFormTypeEmote,
        DatabaseSearchFormTypeQuest,
        DatabaseSearchFormTypeMap,
        DatabaseSearchFormTypeZone,
        DatabaseSearchFormTypeCreatureEntry,
        DatabaseSearchFormTypeGameobjectEntry,
        DatabaseSearchFormTypeSound,
        DatabaseSearchFormTypeAreaTrigger,
        DatabaseSearchFormTypeCreatureGuid,
        DatabaseSearchFormTypeGameobjectGuid,
        DatabaseSearchFormTypeGameEvent,
        DatabaseSearchFormTypeItemEntry,
        DatabaseSearchFormTypeSummonsId,
        DatabaseSearchFormTypeTaxiPath,
        DatabaseSearchFormTypeEquipTemplate,
        DatabaseSearchFormTypeWaypoint,
        DatabaseSearchFormTypeNpcText,
        DatabaseSearchFormTypeGossipMenuOption,
        DatabaseSearchFormTypeGossipOptionId,
    }

    public partial class SearchFromDatabaseForm : Form
    {
        private Thread searchThread = null;
        private readonly MySqlConnectionStringBuilder connectionString;
        private readonly ListViewColumnSorter lvwColumnSorter = new ListViewColumnSorter();
        private readonly TextBox textBoxToChange = null;
        private readonly DatabaseSearchFormType databaseSearchFormType;
        private string baseQuery;
        private string[] columns = new string[7];
        private bool useMySQL = false;
        private int amountOfListviewColumns = 2, listViewItemIndexToCopy = 0;

        public SearchFromDatabaseForm(MySqlConnectionStringBuilder connectionString, TextBox textBoxToChange, DatabaseSearchFormType databaseSearchFormType)
        {
            this.InitializeComponent();

            this.connectionString = connectionString;
            this.textBoxToChange = textBoxToChange;
            this.databaseSearchFormType = databaseSearchFormType;

            this.MinimumSize = new Size(this.Width, this.Height);
            this.MaximumSize = new Size(this.Width, this.Height + 800);
            this.textBoxCriteria.Text = textBoxToChange.Text;

            switch (databaseSearchFormType)
            {
                case DatabaseSearchFormType.DatabaseSearchFormTypeSpell:
                    this.Text = "Search for a spell";
                    this.listViewEntryResults.Columns.Add("Id", 45);
                    this.listViewEntryResults.Columns.Add("Name", 284);
                    this.comboBoxSearchType.Items.Add("Spell id");
                    this.comboBoxSearchType.Items.Add("Spell name");
                    this.baseQuery = "SELECT id, spellName FROM spells";
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeFaction:
                    this.Text = "Search for a faction";
                    this.listViewEntryResults.Columns.Add("Id", 45);
                    this.listViewEntryResults.Columns.Add("Name", 284);
                    this.comboBoxSearchType.Items.Add("Faction id");
                    this.comboBoxSearchType.Items.Add("Faction name");
                    this.baseQuery = "SELECT m_ID, m_name_lang_1 FROM factions";
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeEmote:
                    this.Text = "Search for an emote";
                    this.listViewEntryResults.Columns.Add("Id", 45);
                    this.listViewEntryResults.Columns.Add("Name", 284);
                    this.comboBoxSearchType.Items.Add("Emote id");
                    this.comboBoxSearchType.Items.Add("Emote name");
                    this.baseQuery = "SELECT field0, field1 FROM emotes";
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeQuest:
                    this.Text = "Search for a quest";
                    this.listViewEntryResults.Columns.Add("Id", 45);
                    this.listViewEntryResults.Columns.Add("Name", 284);
                    this.comboBoxSearchType.Items.Add("Quest id");
                    this.comboBoxSearchType.Items.Add("Quest name");
                    this.baseQuery = "SELECT id, title FROM quest_template";
                    this.useMySQL = true;
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeMap:
                    this.Text = "Search for a map id";
                    this.listViewEntryResults.Columns.Add("Id", 45);
                    this.listViewEntryResults.Columns.Add("Name", 284);
                    this.comboBoxSearchType.Items.Add("Map id");
                    this.comboBoxSearchType.Items.Add("Map name");
                    this.baseQuery = "SELECT m_ID, m_MapName_lang1 FROM maps";
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeZone:
                    this.Text = "Search for a zone id";
                    this.listViewEntryResults.Columns.Add("Id", 45);
                    this.listViewEntryResults.Columns.Add("Name", 284);
                    this.comboBoxSearchType.Items.Add("Zone id");
                    this.comboBoxSearchType.Items.Add("Zone name");
                    this.baseQuery = "SELECT m_ID, m_AreaName_lang FROM areas_and_zones";
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry:
                    this.Text = "Search for a creature";
                    this.listViewEntryResults.Columns.Add("Id", 45);
                    this.listViewEntryResults.Columns.Add("Name", 284);
                    this.comboBoxSearchType.Items.Add("Creature entry");
                    this.comboBoxSearchType.Items.Add("Creature name");
                    this.baseQuery = "SELECT entry, name FROM creature_template";
                    this.useMySQL = true;
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectEntry:
                    this.Text = "Search for a gameobject";
                    this.listViewEntryResults.Columns.Add("Id", 45);
                    this.listViewEntryResults.Columns.Add("Name", 284);
                    this.comboBoxSearchType.Items.Add("Gameobject entry");
                    this.comboBoxSearchType.Items.Add("Gameobject name");
                    this.baseQuery = "SELECT entry, name FROM gameobject_template";
                    this.useMySQL = true;
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeSound:
                    this.Text = "Search for a sound id";
                    this.listViewEntryResults.Columns.Add("Id", 45);
                    this.listViewEntryResults.Columns.Add("Name", 284);
                    this.comboBoxSearchType.Items.Add("Sound id");
                    this.comboBoxSearchType.Items.Add("Sound name");
                    this.baseQuery = "SELECT id, name FROM sound_entries";
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeAreaTrigger:
                    this.Text = "Search for a sound id";
                    this.listViewEntryResults.Columns.Add("Id", 52);
                    this.listViewEntryResults.Columns.Add("Mapid", 52);
                    this.listViewEntryResults.Columns.Add("X", 75);
                    this.listViewEntryResults.Columns.Add("Y", 75);
                    this.listViewEntryResults.Columns.Add("Z", 75);
                    this.comboBoxSearchType.Items.Add("Areatrigger id");
                    this.comboBoxSearchType.Items.Add("Areatrigger map id");
                    this.baseQuery = "SELECT m_id, m_mapId, m_posX, m_posY, m_posZ FROM areatriggers";
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeCreatureGuid:
                    this.Text = "Search for a creature";
                    this.listViewEntryResults.Columns.Add("Guid", 45);
                    this.listViewEntryResults.Columns.Add("Name", 284);
                    this.comboBoxSearchType.Items.Add("Creature guid");
                    this.comboBoxSearchType.Items.Add("Creature name");
                    this.baseQuery = "SELECT c.guid, ct.name FROM creature c JOIN creature_template ct ON ct.entry = c.id";
                    this.useMySQL = true;
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectGuid:
                    this.Text = "Search for a gameobject";
                    this.listViewEntryResults.Columns.Add("Guid", 45);
                    this.listViewEntryResults.Columns.Add("Name", 284);
                    this.comboBoxSearchType.Items.Add("Gameobject guid");
                    this.comboBoxSearchType.Items.Add("Gameobject name");
                    this.baseQuery = "SELECT g.guid, gt.name FROM gameobject g JOIN gameobject_template gt ON gt.entry = g.id";
                    this.useMySQL = true;
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeGameEvent:
                    this.Text = "Search for a game event";
                    this.listViewEntryResults.Columns.Add("Entry", 45);
                    this.listViewEntryResults.Columns.Add("Description", 284);
                    this.comboBoxSearchType.Items.Add("Game event entry");
                    this.comboBoxSearchType.Items.Add("Game event description");
                    this.baseQuery = "SELECT eventEntry, description FROM game_event";
                    this.useMySQL = true;
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeItemEntry:
                    this.Text = "Search for an item";
                    this.listViewEntryResults.Columns.Add("Id", 45);
                    this.listViewEntryResults.Columns.Add("Name", 284);
                    this.comboBoxSearchType.Items.Add("Item id");
                    this.comboBoxSearchType.Items.Add("Item name");
                    this.baseQuery = "SELECT entry, name FROM item_template";
                    this.useMySQL = true;
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeSummonsId:
                    this.Text = "Search for a summons id";
                    this.listViewEntryResults.Columns.Add("Owner", 63);
                    this.listViewEntryResults.Columns.Add("Entry", 63);
                    this.listViewEntryResults.Columns.Add("Group", 66);
                    this.listViewEntryResults.Columns.Add("Type", 66);
                    this.listViewEntryResults.Columns.Add("Time", 66);
                    this.comboBoxSearchType.Items.Add("Owner entry");
                    this.comboBoxSearchType.Items.Add("Target entry");
                    this.baseQuery = "SELECT summonerId, entry, groupId, summonType, summonTime FROM creature_summon_groups";
                    this.useMySQL = true;
                    this.listViewItemIndexToCopy = 2;
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeTaxiPath:
                    this.Text = "Search for a taxi path";
                    this.listViewEntryResults.Columns.Add("Id", 45);
                    this.listViewEntryResults.Columns.Add("Name", 284);
                    this.comboBoxSearchType.Items.Add("Taxi id");
                    this.comboBoxSearchType.Items.Add("Taxi name");
                    this.baseQuery = "SELECT id, taxiName FROM taxi_nodes";
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeEquipTemplate:
                    this.Text = "Search for creature equipment";
                    this.listViewEntryResults.Columns.Add("Entry", 63);
                    this.listViewEntryResults.Columns.Add("Id", 63);
                    this.listViewEntryResults.Columns.Add("Item 1", 66);
                    this.listViewEntryResults.Columns.Add("Item 2", 66);
                    this.listViewEntryResults.Columns.Add("Item 3", 66);
                    this.comboBoxSearchType.Items.Add("Entry");
                    this.comboBoxSearchType.Items.Add("Item entries");
                    this.baseQuery = "SELECT entry, id, itemEntry1, itemEntry2, itemEntry3 FROM creature_equip_template";
                    this.useMySQL = true;
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeWaypoint:
                    this.Text = "Search for a waypoints path";
                    this.listViewEntryResults.Columns.Add("Entry", 52);
                    this.listViewEntryResults.Columns.Add("Point", 52);
                    this.listViewEntryResults.Columns.Add("X", 75);
                    this.listViewEntryResults.Columns.Add("Y", 75);
                    this.listViewEntryResults.Columns.Add("Z", 75);
                    this.comboBoxSearchType.Items.Add("Creature entry");
                    this.baseQuery = "SELECT entry, pointid, position_x, position_y, position_z FROM waypoints";
                    this.useMySQL = true;
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeNpcText:
                    this.Text = "Search for an npc_text entry";
                    this.listViewEntryResults.Columns.Add("Id", 45);
                    this.listViewEntryResults.Columns.Add("Text", 284);
                    this.comboBoxSearchType.Items.Add("Id");
                    this.comboBoxSearchType.Items.Add("Text");
                    this.baseQuery = "SELECT id, text0_0 FROM npc_text";
                    this.useMySQL = true;
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeGossipOptionId:
                    this.listViewItemIndexToCopy = 1;
                    goto case DatabaseSearchFormType.DatabaseSearchFormTypeGossipMenuOption;
                case DatabaseSearchFormType.DatabaseSearchFormTypeGossipMenuOption:
                    this.Text = "Search for a gossip item";
                    this.listViewEntryResults.Columns.Add("Menu id", 45);
                    this.listViewEntryResults.Columns.Add("Id", 284);
                    this.listViewEntryResults.Columns.Add("Text", 284);
                    this.comboBoxSearchType.Items.Add("Menu");
                    this.comboBoxSearchType.Items.Add("Id");
                    this.comboBoxSearchType.Items.Add("Text");
                    this.baseQuery = "SELECT menu_id, id, option_text FROM gossip_menu_option";
                    this.useMySQL = true;
                    break;
                default:
                    break;
            }

            string[] columnsSplit = this.baseQuery.Replace("SELECT ", "").Replace(" FROM " + this.baseQuery.Split(' ').LastOrDefault(), "").Split(',');
            this.columns = new string[columnsSplit.Length + 1];

            for (int i = 0; i < columnsSplit.Length; ++i)
                this.columns[i] = columnsSplit[i];

            this.amountOfListviewColumns = this.columns.Length - 1;
            this.comboBoxSearchType.SelectedIndex = 0;
            this.FillListView(true);
        }

        private void listViewEntryResults_DoubleClick(object sender, EventArgs e)
        {
            this.StopRunningThread();

            if (this.listViewItemIndexToCopy > 0)
                this.textBoxToChange.Text = this.listViewEntryResults.SelectedItems[0].SubItems[this.listViewItemIndexToCopy].Text;
            else
                this.textBoxToChange.Text = this.listViewEntryResults.SelectedItems[0].Text;

            this.Close();
        }

        private async void FillListView(bool limit = false)
        {
            try
            {
                string queryToExecute = this.baseQuery;

                if (!limit)
                {
                    switch (this.GetSelectedIndexOfComboBox(this.comboBoxSearchType))
                    {
                        case 0: //! First column
                            if (this.checkBoxFieldContainsCriteria.Checked)
                                queryToExecute += " WHERE " + this.columns[0] + " LIKE '%" + this.textBoxCriteria.Text + "%'";
                            else
                                queryToExecute += " WHERE " + this.columns[0] + " = " + this.textBoxCriteria.Text;

                            break;
                        case 1: //! Second column
                            switch (this.databaseSearchFormType)
                            {
                                case DatabaseSearchFormType.DatabaseSearchFormTypeEquipTemplate:
                                {
                                    if (this.checkBoxFieldContainsCriteria.Checked)
                                        queryToExecute += " WHERE itemEntry1 LIKE '%" + this.textBoxCriteria.Text + "%' OR itemEntry2 LIKE '%" + this.textBoxCriteria.Text + "%' OR itemEntry3 LIKE '%" + this.textBoxCriteria.Text + "%'";
                                    else
                                        queryToExecute += " WHERE itemEntry1 = " + this.textBoxCriteria.Text + " OR itemEntry2 = " + this.textBoxCriteria.Text + " OR itemEntry3 = " + this.textBoxCriteria.Text;

                                    break;
                                }
                                default:
                                {
                                    if (this.checkBoxFieldContainsCriteria.Checked)
                                        queryToExecute += " WHERE " + this.columns[1] + " LIKE '%" + this.textBoxCriteria.Text + "%'";
                                    else
                                        queryToExecute += " WHERE " + this.columns[1] + " = " + this.textBoxCriteria.Text;

                                    break;
                                }
                            }
                            break;
                        case 2: //! Third column
                            if (this.checkBoxFieldContainsCriteria.Checked)
                                queryToExecute += " WHERE " + this.columns[2] + " LIKE '%" + this.textBoxCriteria.Text + "%'";
                            else
                                queryToExecute += " WHERE " + this.columns[2] + " = " + this.textBoxCriteria.Text;

                            break;
                        default:
                            return;
                    }

                    if (this.databaseSearchFormType == DatabaseSearchFormType.DatabaseSearchFormTypeZone)
                        queryToExecute += " AND m_ParentAreaID = 0";
                }
                else if (this.databaseSearchFormType == DatabaseSearchFormType.DatabaseSearchFormTypeZone)
                    queryToExecute += " WHERE m_ParentAreaID = 0";

                queryToExecute += " ORDER BY " + this.columns[0];

                if (limit)
                    queryToExecute += " LIMIT 1000";

                DataTable dt = this.useMySQL ? await SAI_Editor_Manager.Instance.worldDatabase.ExecuteQuery(queryToExecute) : await SAI_Editor_Manager.Instance.sqliteDatabase.ExecuteQuery(queryToExecute);

                if (dt.Rows.Count > 0)
                {
                    List<ListViewItem> items = new List<ListViewItem>();

                    foreach (DataRow row in dt.Rows)
                    {
                        ListViewItem listViewItem = new ListViewItem(row.ItemArray[0].ToString());

                        for (int i = 1; i < this.amountOfListviewColumns; ++i)
                            listViewItem.SubItems.Add(row.ItemArray[i].ToString());

                        items.Add(listViewItem);
                    }

                    //listViewEntryResults.Items.AddRange(items.ToArray());
                    this.AddItemRangeToListView(this.listViewEntryResults, items.ToArray());
                }
            }
            catch (ObjectDisposedException)
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            this.searchThread = new Thread(this.StartSearching);
            this.searchThread.Start();
        }

        private void StartSearching()
        {
            try
            {
                this.SetEnabledOfControl(this.buttonSearch, false);
                this.SetEnabledOfControl(this.buttonStopSearching, true);
                this.ClearItemsOfListView(this.listViewEntryResults);

                try
                {
                    this.FillListView();
                }
                finally
                {
                    this.SetEnabledOfControl(this.buttonSearch, true);
                    this.SetEnabledOfControl(this.buttonStopSearching, false);
                }
            }
            catch (ThreadAbortException) //! Don't show a message when the thread was already cancelled
            {
                this.SetEnabledOfControl(this.buttonSearch, true);
                this.SetEnabledOfControl(this.buttonStopSearching, false);
            }
            catch (Exception ex)
            {
                this.SetEnabledOfControl(this.buttonSearch, true);
                this.SetEnabledOfControl(this.buttonStopSearching, false);
                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SearchFromDatabaseForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                {
                    if (this.listViewEntryResults.SelectedItems.Count > 0 && this.listViewEntryResults.Focused)
                    {
                        if (this.listViewItemIndexToCopy > 0)
                            this.textBoxToChange.Text = this.listViewEntryResults.SelectedItems[0].SubItems[this.listViewItemIndexToCopy].Text;
                        else
                            this.textBoxToChange.Text = this.listViewEntryResults.SelectedItems[this.listViewItemIndexToCopy].Text;

                        this.Close();
                    }
                    else
                    {
                        if (this.buttonSearch.Enabled)
                            this.buttonSearch.PerformClick();
                    }

                    break;
                }
                case Keys.Escape:
                {
                    this.Close();
                    break;
                }
            }
        }

        private void buttonStopSearchResults_Click(object sender, EventArgs e)
        {
            this.StopRunningThread();
        }

        private void StopRunningThread()
        {
            if (this.searchThread != null && this.searchThread.IsAlive)
            {
                this.searchThread.Abort();
                this.searchThread = null;
            }
        }

        private void comboBoxSearchType_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true; //! Disallow changing content of the combobox, but setting it to 3D looks like shit
        }

        private int GetSelectedIndexOfComboBox(ComboBox comboBox)
        {
            if (comboBox.InvokeRequired)
                return (int)comboBox.Invoke(new Func<int>(() => this.GetSelectedIndexOfComboBox(comboBox)));

            return comboBox.SelectedIndex;
        }

        private void AddItemRangeToListView(ListView listView, ListViewItem[] items)
        {
            if (listView.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    listView.Items.AddRange(items);
                });
                return;
            }

            listView.Items.AddRange(items);
        }

        private void SetEnabledOfControl(Control control, bool enable)
        {
            if (control.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    control.Enabled = enable;
                });
                return;
            }

            control.Enabled = enable;
        }

        private void ClearItemsOfListView(ListView listView)
        {
            if (listView.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    listView.Items.Clear();
                });
                return;
            }

            listView.Items.Clear();
        }

        private void listViewEntryResults_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            var myListView = (ListView)sender;
            myListView.ListViewItemSorter = this.lvwColumnSorter;
            //! Determine if clicked column is already the column that is being sorted
            if (e.Column != this.lvwColumnSorter.SortColumn)
            {
                //! Set the column number that is to be sorted; default to ascending
                this.lvwColumnSorter.SortColumn = e.Column;
                this.lvwColumnSorter.Order = SortOrder.Ascending;
            }
            else
                //! Reverse the current sort direction for this column
                this.lvwColumnSorter.Order = this.lvwColumnSorter.Order == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;

            //! Perform the sort with these new sort options
            myListView.Sort();
        }

        private void SearchFromDatabaseForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StopRunningThread();
        }
    }
}
