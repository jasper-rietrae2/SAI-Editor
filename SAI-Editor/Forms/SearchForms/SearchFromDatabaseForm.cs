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
using SAI_Editor.Properties;

namespace SAI_Editor
{
    public enum DatabaseSearchFormType
    {
        DatabaseSearchFormTypeSpell = 0,
        DatabaseSearchFormTypeFaction = 1,
        DatabaseSearchFormTypeEmote = 2,
        DatabaseSearchFormTypeQuest = 3,
        DatabaseSearchFormTypeMap = 4,
        DatabaseSearchFormTypeZone = 5,
        DatabaseSearchFormTypeCreatureEntry = 6,
        DatabaseSearchFormTypeGameobjectEntry = 7,
        DatabaseSearchFormTypeSound = 8,
        DatabaseSearchFormTypeAreaTrigger = 9,
    };

    public partial class SearchFromDatabaseForm : Form
    {
        private Thread searchThread = null;
        private readonly MySqlConnectionStringBuilder connectionString;
        private readonly ListViewColumnSorter lvwColumnSorter = new ListViewColumnSorter();
        private readonly TextBox textBoxToChange = null;
        private readonly DatabaseSearchFormType databaseSearchFormType;
        private string baseQuery, columnOne, columnTwo;
        private bool useMySQL = false;

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
                    comboBoxSearchType.Items.Add("Creature id");
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
                    comboBoxSearchType.Items.Add("Gameobject id");
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
                    break;
            }

            listViewEntryResults.Anchor = AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left;
            comboBoxSearchType.SelectedIndex = 0;
            FillListView(true);
        }

        private void listViewEntryResults_DoubleClick(object sender, EventArgs e)
        {
            StopRunningThread();
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
                            if (checkBoxFieldContainsCriteria.Checked)
                                queryToExecute += " WHERE " + columnTwo + " LIKE '%" + textBoxCriteria.Text + "%'";
                            else
                                queryToExecute += " WHERE " + columnTwo + " = " + textBoxCriteria.Text;

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
                    foreach (DataRow row in dt.Rows)
                    {
                        if (databaseSearchFormType == DatabaseSearchFormType.DatabaseSearchFormTypeAreaTrigger)
                            AddItemToListView(listViewEntryResults, Convert.ToInt32(row["m_Id"]).ToString(), Convert.ToInt32(row["m_mapId"]).ToString(), Convert.ToInt32(row["m_posX"]).ToString(), Convert.ToInt32(row["m_posY"]).ToString(), Convert.ToInt32(row["m_posZ"]).ToString());
                        else
                            AddItemToListView(listViewEntryResults, Convert.ToInt32(row[columnOne]).ToString(), (string)row[columnTwo]);
                    }
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
                        textBoxToChange.Text = listViewEntryResults.SelectedItems[0].Text;
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

        private void AddItemToListView(ListView listView, string item, string subItem)
        {
            if (listView.InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    listView.Items.Add(item).SubItems.Add(subItem);
                });
                return;
            }

            listView.Items.Add(item).SubItems.Add(subItem);
        }



        private void AddItemToListView(ListView listView, string item, string subItem1, string subItem2, string subItem3, string subItem4)
        {
            if (listView.InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    ListViewItem listViewItem = listView.Items.Add(item);
                    listViewItem.SubItems.Add(subItem1);
                    listViewItem.SubItems.Add(subItem2);
                    listViewItem.SubItems.Add(subItem3);
                    listViewItem.SubItems.Add(subItem4);
                });
                return;
            }

            ListViewItem listViewItem2 = listView.Items.Add(item);
            listViewItem2.SubItems.Add(subItem1);
            listViewItem2.SubItems.Add(subItem2);
            listViewItem2.SubItems.Add(subItem3);
            listViewItem2.SubItems.Add(subItem4);
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
