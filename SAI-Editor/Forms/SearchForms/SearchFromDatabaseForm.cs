using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using SAI_Editor.Classes;
using SAI_Editor.Properties;

namespace SAI_Editor
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
    }

    public partial class SearchFromDatabaseForm : Form
    {
        private Thread searchThread = null;
        private readonly MySqlConnectionStringBuilder connectionString;
        private readonly ListViewColumnSorter lvwColumnSorter = new ListViewColumnSorter();
        private readonly TextBox textBoxToChange = null;
        private readonly DatabaseSearchFormType databaseSearchFormType;
        private string baseQuery, columnOne, columnTwo;
        private bool useMySQL = false;
        private int amountOfListviewColumns = 2, listViewItemIndexToCopy = 0;

        public SearchFromDatabaseForm(MySqlConnectionStringBuilder connectionString, TextBox textBoxToChange, DatabaseSearchFormType databaseSearchFormType)
        {
            InitializeComponent();

            this.connectionString = connectionString;
            this.textBoxToChange = textBoxToChange;
            this.databaseSearchFormType = databaseSearchFormType;
        }

        private void SearchFromDatabaseForm_Load(object sender, EventArgs e)
        {
            MinimumSize = new Size(Width, Height);
            MaximumSize = new Size(Width, Height + 800);
            textBoxCriteria.Text = textBoxToChange.Text;

            switch (databaseSearchFormType)
            {
                case DatabaseSearchFormType.DatabaseSearchFormTypeSpell:
                    Text = "Search for a spell";
                    listViewEntryResults.Columns.Add("Id", 45);
                    listViewEntryResults.Columns.Add("Name", 284);
                    comboBoxSearchType.Items.Add("Spell id");
                    comboBoxSearchType.Items.Add("Spell name");
                    baseQuery = "SELECT id, spellName FROM spells";
                    columnOne = "id";
                    columnTwo = "spellName";
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeFaction:
                    Text = "Search for a faction";
                    listViewEntryResults.Columns.Add("Id", 45);
                    listViewEntryResults.Columns.Add("Name", 284);
                    comboBoxSearchType.Items.Add("Faction id");
                    comboBoxSearchType.Items.Add("Faction name");
                    baseQuery = "SELECT m_ID, m_name_lang_1 FROM factions";
                    columnOne = "m_ID";
                    columnTwo = "m_name_lang_1";
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeEmote:
                    Text = "Search for an emote";
                    listViewEntryResults.Columns.Add("Id", 45);
                    listViewEntryResults.Columns.Add("Name", 284);
                    comboBoxSearchType.Items.Add("Emote id");
                    comboBoxSearchType.Items.Add("Emote name");
                    baseQuery = "SELECT field0, field1 FROM emotes";
                    columnOne = "field0";
                    columnTwo = "field1";
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeQuest:
                    Text = "Search for a quest";
                    listViewEntryResults.Columns.Add("Id", 45);
                    listViewEntryResults.Columns.Add("Name", 284);
                    comboBoxSearchType.Items.Add("Quest id");
                    comboBoxSearchType.Items.Add("Quest name");
                    baseQuery = "SELECT id, title FROM quest_template";
                    columnOne = "id";
                    columnTwo = "title";
                    useMySQL = true;
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeMap:
                    Text = "Search for a map id";
                    listViewEntryResults.Columns.Add("Id", 45);
                    listViewEntryResults.Columns.Add("Name", 284);
                    comboBoxSearchType.Items.Add("Map id");
                    comboBoxSearchType.Items.Add("Map name");
                    baseQuery = "SELECT m_ID, m_MapName_lang1 FROM maps";
                    columnOne = "m_ID";
                    columnTwo = "m_MapName_lang1";
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeZone:
                    Text = "Search for a zone id";
                    listViewEntryResults.Columns.Add("Id", 45);
                    listViewEntryResults.Columns.Add("Name", 284);
                    comboBoxSearchType.Items.Add("Zone id");
                    comboBoxSearchType.Items.Add("Zone name");
                    baseQuery = "SELECT m_ID, m_AreaName_lang FROM areas_and_zones";
                    columnOne = "m_ID";
                    columnTwo = "m_AreaName_lang";
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry:
                    Text = "Search for a creature";
                    listViewEntryResults.Columns.Add("Id", 45);
                    listViewEntryResults.Columns.Add("Name", 284);
                    comboBoxSearchType.Items.Add("Creature entry");
                    comboBoxSearchType.Items.Add("Creature name");
                    baseQuery = "SELECT entry, name FROM creature_template";
                    columnOne = "entry";
                    columnTwo = "name";
                    useMySQL = true;
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectEntry:
                    Text = "Search for a gameobject";
                    listViewEntryResults.Columns.Add("Id", 45);
                    listViewEntryResults.Columns.Add("Name", 284);
                    comboBoxSearchType.Items.Add("Gameobject entry");
                    comboBoxSearchType.Items.Add("Gameobject name");
                    baseQuery = "SELECT entry, name FROM gameobject_template";
                    columnOne = "entry";
                    columnTwo = "name";
                    useMySQL = true;
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeSound:
                    Text = "Search for a sound id";
                    listViewEntryResults.Columns.Add("Id", 45);
                    listViewEntryResults.Columns.Add("Name", 284);
                    comboBoxSearchType.Items.Add("Sound id");
                    comboBoxSearchType.Items.Add("Sound name");
                    baseQuery = "SELECT id, name FROM sound_entries";
                    columnOne = "id";
                    columnTwo = "name";
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeAreaTrigger:
                    Text = "Search for a sound id";
                    listViewEntryResults.Columns.Add("Id", 52);
                    listViewEntryResults.Columns.Add("Mapid", 52);
                    listViewEntryResults.Columns.Add("X", 75);
                    listViewEntryResults.Columns.Add("Y", 75);
                    listViewEntryResults.Columns.Add("Z", 75);
                    comboBoxSearchType.Items.Add("Areatrigger id");
                    comboBoxSearchType.Items.Add("Areatrigger map id");
                    baseQuery = "SELECT m_id, m_mapId, m_posX, m_posY, m_posZ FROM areatriggers";
                    columnOne = "m_id";
                    columnTwo = "m_mapId";
                    amountOfListviewColumns = 5;
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeCreatureGuid:
                    Text = "Search for a creature";
                    listViewEntryResults.Columns.Add("Guid", 45);
                    listViewEntryResults.Columns.Add("Name", 284);
                    comboBoxSearchType.Items.Add("Creature guid");
                    comboBoxSearchType.Items.Add("Creature name");
                    baseQuery = "SELECT c.guid, ct.name FROM creature c JOIN creature_template ct ON ct.entry = c.id";
                    columnOne = "c.guid";
                    columnTwo = "ct.name";
                    useMySQL = true;
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectGuid:
                    Text = "Search for a gameobject";
                    listViewEntryResults.Columns.Add("Guid", 45);
                    listViewEntryResults.Columns.Add("Name", 284);
                    comboBoxSearchType.Items.Add("Gameobject guid");
                    comboBoxSearchType.Items.Add("Gameobject name");
                    baseQuery = "SELECT g.guid, gt.name FROM gameobject g JOIN gameobject_template gt ON gt.entry = g.id";
                    columnOne = "g.guid";
                    columnTwo = "gt.name";
                    useMySQL = true;
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeGameEvent:
                    Text = "Search for a game event";
                    listViewEntryResults.Columns.Add("Entry", 45);
                    listViewEntryResults.Columns.Add("Description", 284);
                    comboBoxSearchType.Items.Add("Game event entry");
                    comboBoxSearchType.Items.Add("Game event description");
                    baseQuery = "SELECT eventEntry, description FROM game_event";
                    columnOne = "eventEntry";
                    columnTwo = "description";
                    useMySQL = true;
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeItemEntry:
                    Text = "Search for an item";
                    listViewEntryResults.Columns.Add("Id", 45);
                    listViewEntryResults.Columns.Add("Name", 284);
                    comboBoxSearchType.Items.Add("Item id");
                    comboBoxSearchType.Items.Add("Item name");
                    baseQuery = "SELECT entry, name FROM item_template";
                    columnOne = "entry";
                    columnTwo = "name";
                    useMySQL = true;
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeSummonsId:
                    Text = "Search for a summons id";
                    listViewEntryResults.Columns.Add("Owner", 63);
                    listViewEntryResults.Columns.Add("Entry", 63);
                    listViewEntryResults.Columns.Add("Group", 66);
                    listViewEntryResults.Columns.Add("Type", 66);
                    listViewEntryResults.Columns.Add("Time", 66);
                    comboBoxSearchType.Items.Add("Owner entry");
                    comboBoxSearchType.Items.Add("Target entry");
                    baseQuery = "SELECT summonerId, entry, groupId, summonType, summonTime FROM creature_summon_groups";
                    columnOne = "summonerId";
                    columnTwo = "entry";
                    useMySQL = true;
                    amountOfListviewColumns = 5;
                    listViewItemIndexToCopy = 2;
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeTaxiPath:
                    Text = "Search for a taxi path";
                    listViewEntryResults.Columns.Add("Id", 45);
                    listViewEntryResults.Columns.Add("Name", 284);
                    comboBoxSearchType.Items.Add("Taxi id");
                    comboBoxSearchType.Items.Add("Taxi name");
                    baseQuery = "SELECT id, taxiName FROM taxi_nodes";
                    columnOne = "id";
                    columnTwo = "taxiName";
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeEquipTemplate:
                    Text = "Search for creature equipment";
                    listViewEntryResults.Columns.Add("Entry", 63);
                    listViewEntryResults.Columns.Add("Id", 63);
                    listViewEntryResults.Columns.Add("Item 1", 66);
                    listViewEntryResults.Columns.Add("Item 2", 66);
                    listViewEntryResults.Columns.Add("Item 3", 66);
                    comboBoxSearchType.Items.Add("Entry");
                    comboBoxSearchType.Items.Add("Item entries");
                    baseQuery = "SELECT entry, id, itemEntry1, itemEntry2, itemEntry3 FROM creature_equip_template";
                    columnOne = "entry";
                    columnTwo = "itemEntry1";
                    useMySQL = true;
                    amountOfListviewColumns = 5;
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeWaypoint:
                    Text = "Search for a waypoints path";
                    listViewEntryResults.Columns.Add("Entry", 52);
                    listViewEntryResults.Columns.Add("Point", 52);
                    listViewEntryResults.Columns.Add("X", 75);
                    listViewEntryResults.Columns.Add("Y", 75);
                    listViewEntryResults.Columns.Add("Z", 75);
                    comboBoxSearchType.Items.Add("Creature entry");
                    baseQuery = "SELECT entry, pointid, position_x, position_y, position_z FROM waypoints";
                    columnOne = "entry";
                    columnTwo = "pointid";
                    useMySQL = true;
                    amountOfListviewColumns = 5;
                    break;
            }

            listViewEntryResults.Anchor = AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left;
            comboBoxSearchType.SelectedIndex = 0;
            FillListView(true);
        }

        private void listViewEntryResults_DoubleClick(object sender, EventArgs e)
        {
            StopRunningThread();

            if (listViewItemIndexToCopy > 0)
                textBoxToChange.Text = listViewEntryResults.SelectedItems[0].SubItems[listViewItemIndexToCopy].Text;
            else
                textBoxToChange.Text = listViewEntryResults.SelectedItems[0].Text;

            Close();
        }

        private async void FillListView(bool limit = false)
        {
            try
            {
                DataTable dt = null;
                string queryToExecute = baseQuery;

                if (!limit)
                {
                    switch (GetSelectedIndexOfComboBox(comboBoxSearchType))
                    {
                        case 0: //! First column
                            if (checkBoxFieldContainsCriteria.Checked)
                                queryToExecute += " WHERE " + columnOne + " LIKE '%" + textBoxCriteria.Text + "%'";
                            else
                                queryToExecute += " WHERE " + columnOne + " = " + textBoxCriteria.Text;

                            break;
                        case 1: //! Second column
                            switch (databaseSearchFormType)
                            {
                                //! No second column exists for search type
                                case DatabaseSearchFormType.DatabaseSearchFormTypeWaypoint:
                                    return;
                                case DatabaseSearchFormType.DatabaseSearchFormTypeEquipTemplate:
                                {
                                    if (checkBoxFieldContainsCriteria.Checked)
                                        queryToExecute += " WHERE itemEntry1 LIKE '%" + textBoxCriteria.Text + "%' OR itemEntry2 LIKE '%" + textBoxCriteria.Text + "%' OR itemEntry3 LIKE '%" + textBoxCriteria.Text + "%'";
                                    else
                                        queryToExecute += " WHERE itemEntry1 = " + textBoxCriteria.Text + " OR itemEntry2 = " + textBoxCriteria.Text + " OR itemEntry3 = " + textBoxCriteria.Text;

                                    break;
                                }
                                default:
                                {
                                    if (checkBoxFieldContainsCriteria.Checked)
                                        queryToExecute += " WHERE " + columnTwo + " LIKE '%" + textBoxCriteria.Text + "%'";
                                    else
                                        queryToExecute += " WHERE " + columnTwo + " = " + textBoxCriteria.Text;

                                    break;
                                }
                            }

                            break;
                        default:
                            return;
                    }

                    if (databaseSearchFormType == DatabaseSearchFormType.DatabaseSearchFormTypeZone)
                        queryToExecute += " AND m_ParentAreaID = 0";
                }
                else if (databaseSearchFormType == DatabaseSearchFormType.DatabaseSearchFormTypeZone)
                    queryToExecute += " WHERE m_ParentAreaID = 0";

                queryToExecute += " ORDER BY " + columnOne;

                if (limit)
                    queryToExecute += " LIMIT 1000";

                dt = useMySQL ? await SAI_Editor_Manager.Instance.worldDatabase.ExecuteQuery(queryToExecute) : await SAI_Editor_Manager.Instance.sqliteDatabase.ExecuteQuery(queryToExecute);

                if (dt.Rows.Count > 0)
                {
                    List<ListViewItem> items = new List<ListViewItem>();

                    foreach (DataRow row in dt.Rows)
                    {
                        ListViewItem listViewItem = new ListViewItem(row.ItemArray[0].ToString());

                        for (int i = 1; i < amountOfListviewColumns; ++i)
                            listViewItem.SubItems.Add(row.ItemArray[i].ToString());

                        items.Add(listViewItem);
                    }

                    //listViewEntryResults.Items.AddRange(items.ToArray());
                    AddItemRangeToListView(listViewEntryResults, items.ToArray());
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
            searchThread = new Thread(StartSearching);
            searchThread.Start();
        }

        private void StartSearching()
        {
            try
            {
                SetEnabledOfControl(buttonSearch, false);
                SetEnabledOfControl(buttonStopSearching, true);
                ClearItemsOfListView(listViewEntryResults);

                try
                {
                    FillListView();
                }
                finally
                {
                    SetEnabledOfControl(buttonSearch, true);
                    SetEnabledOfControl(buttonStopSearching, false);
                }
            }
            catch (ThreadAbortException) //! Don't show a message when the thread was already cancelled
            {
                SetEnabledOfControl(buttonSearch, true);
                SetEnabledOfControl(buttonStopSearching, false);
            }
            catch (Exception ex)
            {
                SetEnabledOfControl(buttonSearch, true);
                SetEnabledOfControl(buttonStopSearching, false);
                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SearchFromDatabaseForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                {
                    if (listViewEntryResults.SelectedItems.Count > 0 && listViewEntryResults.Focused)
                    {
                        if (listViewItemIndexToCopy > 0)
                            textBoxToChange.Text = listViewEntryResults.SelectedItems[0].SubItems[listViewItemIndexToCopy].Text;
                        else
                            textBoxToChange.Text = listViewEntryResults.SelectedItems[listViewItemIndexToCopy].Text;

                        Close();
                    }
                    else
                    {
                        if (buttonSearch.Enabled)
                            buttonSearch.PerformClick();
                    }

                    break;
                }
                case Keys.Escape:
                {
                    Close();
                    break;
                }
            }
        }

        private void buttonStopSearchResults_Click(object sender, EventArgs e)
        {
            StopRunningThread();
        }

        private void StopRunningThread()
        {
            if (searchThread != null && searchThread.IsAlive)
            {
                searchThread.Abort();
                searchThread = null;
            }
        }

        private void comboBoxSearchType_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true; //! Disallow changing content of the combobox, but setting it to 3D looks like shit
        }

        private int GetSelectedIndexOfComboBox(ComboBox comboBox)
        {
            if (comboBox.InvokeRequired)
                return (int)comboBox.Invoke(new Func<int>(() => GetSelectedIndexOfComboBox(comboBox)));

            return comboBox.SelectedIndex;
        }

        private void AddItemRangeToListView(ListView listView, ListViewItem[] items)
        {
            if (listView.InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
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
                Invoke((MethodInvoker)delegate
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
                Invoke((MethodInvoker)delegate
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

        private void SearchFromDatabaseForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopRunningThread();
        }
    }
}
