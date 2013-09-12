using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Threading;
using SAI_Editor.Properties;
using MySql.Data.MySqlClient;

namespace SAI_Editor
{
    public partial class SearchForEntryForm : Form
    {
        private Thread searchThread = null;
        private readonly MySqlConnectionStringBuilder connectionString;
        private readonly SourceTypes sourceTypeToSearchFor;
        private readonly ListViewColumnSorter lvwColumnSorter = new ListViewColumnSorter();

        public SearchForEntryForm(MySqlConnectionStringBuilder connectionString, string startEntryString, SourceTypes sourceTypeToSearchFor)
        {
            InitializeComponent();

            this.connectionString = connectionString;
            this.sourceTypeToSearchFor = sourceTypeToSearchFor;
            textBoxCriteria.Text = startEntryString;
        }

        private void SearchForEntryForm_Load(object sender, EventArgs e)
        {
            MinimumSize = new Size(Width, Height);
            MaximumSize = new Size(Width, Height + 800);

            switch (sourceTypeToSearchFor)
            {
                case SourceTypes.SourceTypeCreature:
                    comboBoxSearchType.SelectedIndex = 0; //! Creature entry
                    FillListViewWithQuery("SELECT entry, name FROM creature_template ORDER BY entry LIMIT 1000", false);
                    break;
                case SourceTypes.SourceTypeGameobject:
                    comboBoxSearchType.SelectedIndex = 3; //! Gameobject entry
                    FillListViewWithQuery("SELECT entry, name FROM gameobject_template ORDER BY entry LIMIT 1000", false);
                    break;
                case SourceTypes.SourceTypeAreaTrigger:
                    //comboBoxSearchType.SelectedIndex = 7; //! NYI
                    //FillListViewWithQuery("SELECT entry, name FROM gameobject_template ORDER BY entry LIMIT 1000", false);
                    break;
                case SourceTypes.SourceTypeScriptedActionlist:
                    checkBoxHasAiName.Enabled = false;
                    comboBoxSearchType.SelectedIndex = 6; //! Actionlist entry
                    //FillListViewWithQuery("SELECT entry, name FROM creature_template ORDER BY entry LIMIT 1000", false);
                    break;
            }

            listViewEntryResults.Columns.Add("Entry/guid", 70, HorizontalAlignment.Right);
            listViewEntryResults.Columns.Add("Name", 260, HorizontalAlignment.Left);
            listViewEntryResults.ColumnClick += listViewEntryResults_ColumnClick;
            listViewEntryResults.Anchor = AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left;
        }

        private void listViewEntryResults_DoubleClick(object sender, EventArgs e)
        {
            StopRunningThread();
            FillMainFormEntryOrGuidField(sender, e);
        }

        private void FillListViewWithQuery(string queryToExecute, bool crossThread)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString.ToString()))
                {
                    connection.Open();

                    using (var query = new MySqlCommand(queryToExecute, connection))
                    {
                        using (MySqlDataReader reader = query.ExecuteReader())
                        {
                            while (reader != null && reader.Read())
                            {
                                if (crossThread)
                                    AddItemToListView(listViewEntryResults, reader.GetInt32(0).ToString(CultureInfo.InvariantCulture), reader.GetString(1));
                                else
                                    listViewEntryResults.Items.Add(reader.GetInt32(0).ToString(CultureInfo.InvariantCulture)).SubItems.Add(reader.GetString(1));
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            searchThread = new Thread(StartSearching);
            searchThread.Start();
        }

        private async void StartSearching()
        {
            string query = "";
            bool criteriaLeftEmpty = String.IsNullOrEmpty(textBoxCriteria.Text) || String.IsNullOrWhiteSpace(textBoxCriteria.Text);

            if (!criteriaLeftEmpty && IsNumericIndex(GetSelectedIndexOfComboBox(comboBoxSearchType)) && Convert.ToInt32(textBoxCriteria.Text) < 0)
            {
                if (MessageBox.Show("The criteria field can not be a negative value, would you like me to set it to a positive number?", "Something went wrong!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    SetTextOfControl(textBoxCriteria, (Convert.ToInt32(textBoxCriteria.Text) * -1).ToString());
                else
                    return;
            }

            SetEnabledOfControl(buttonSearch, false);
            SetEnabledOfControl(buttonStopSearching, true);

            switch (GetSelectedIndexOfComboBox(comboBoxSearchType))
            {
                case 0: //! Creature entry
                    query = "SELECT entry, name FROM creature_template";

                    if (!criteriaLeftEmpty)
                    {
                        if (checkBoxFieldContainsCriteria.Checked)
                            query += " WHERE entry LIKE '%" + textBoxCriteria.Text + "%'";
                        else
                            query += " WHERE entry=" + textBoxCriteria.Text;
                    }

                    if (checkBoxHasAiName.Checked)
                        query += (criteriaLeftEmpty ? " WHERE" : " AND") + " AIName='SmartAI'";

                    query += " ORDER BY entry";
                    break;
                case 1: //! Creature name
                    query = "SELECT entry, name FROM creature_template WHERE name LIKE '%" + textBoxCriteria.Text + "%'";

                    if (checkBoxHasAiName.Checked)
                        query += " AND AIName='SmartAI'";

                    query += " ORDER BY entry";
                    break;
                case 2: //! Creature guid
                    if (criteriaLeftEmpty)
                    {
                        if (Settings.Default.PromptExecuteQuery && MessageBox.Show("Are you sure you wish to continue? This query will take a long time to execute because the criteria field was left empty!", "Are you sure you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                            return;

                        if (checkBoxHasAiName.Checked)
                            query = "SELECT c.guid, ct.name FROM creature_template ct JOIN creature c ON ct.entry = c.id JOIN smart_scripts ss ON ss.entryorguid < 0 AND ss.entryorguid = -c.guid AND ss.source_type = 0";
                        else
                            query = "SELECT c.guid, ct.name FROM creature_template ct JOIN creature c ON ct.entry = c.id";
                    }
                    else
                    {
                        if (checkBoxHasAiName.Checked)
                        {
                            if (checkBoxFieldContainsCriteria.Checked)
                                query = "SELECT c.guid, ct.name FROM creature c JOIN creature_template ct ON ct.entry = c.id JOIN smart_scripts ss ON ss.entryorguid = -c.guid WHERE c.guid LIKE '%" + textBoxCriteria.Text + "%' AND ss.source_type = 0";
                            else
                                query = "SELECT c.guid, ct.name FROM creature_template ct JOIN creature c ON ct.entry = c.id JOIN smart_scripts ss ON ss.entryorguid = -c.guid WHERE c.guid = " + textBoxCriteria.Text;
                        }
                        else
                        {
                            if (checkBoxFieldContainsCriteria.Checked)
                                query = "SELECT c.guid, ct.name FROM creature c JOIN creature_template ct ON ct.entry = c.id WHERE c.guid LIKE '%" + textBoxCriteria.Text + "%'";
                            else
                                query = "SELECT c.guid, ct.name FROM creature_template ct JOIN creature c ON ct.entry = c.id WHERE c.guid = " + textBoxCriteria.Text;
                        }
                    }

                    query += " ORDER BY c.guid";
                    break;
                case 3: //! Gameobject entry
                    query = "SELECT entry, name FROM gameobject_template";

                    if (!criteriaLeftEmpty)
                    {
                        if (checkBoxFieldContainsCriteria.Checked)
                            query += " WHERE entry LIKE '%" + textBoxCriteria.Text + "%'";
                        else
                            query += " WHERE entry=" + textBoxCriteria.Text;
                    }

                    if (checkBoxHasAiName.Checked)
                        query += (criteriaLeftEmpty ? " WHERE" : " AND") + " AIName='SmartGameObjectAI'";

                    query += " ORDER BY entry";
                    break;
                case 4: //! Gameobject name
                    query = "SELECT entry, name FROM gameobject_template WHERE name LIKE '%" + textBoxCriteria.Text + "%'";

                    if (checkBoxHasAiName.Checked)
                        query += " AND AIName='SmartGameObjectAI'";

                    query += " ORDER BY entry";
                    break;
                case 5: //! Gameobject guid
                    if (criteriaLeftEmpty)
                    {
                        if (Settings.Default.PromptExecuteQuery && MessageBox.Show("Are you sure you wish to continue? This query will take a long time to execute because the criteria field was left empty!", "Are you sure you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                            return;

                        if (checkBoxHasAiName.Checked)
                            query = "SELECT g.guid, gt.name FROM gameobject_template gt JOIN gameobject g ON gt.entry = g.id JOIN smart_scripts ss ON ss.entryorguid < 0 AND ss.entryorguid = -g.guid AND ss.source_type = 1";
                        else
                            query = "SELECT g.guid, gt.name FROM gameobject_template gt JOIN gameobject g ON gt.entry = g.id";
                    }
                    else
                    {
                        if (checkBoxHasAiName.Checked)
                        {
                            if (checkBoxFieldContainsCriteria.Checked)
                                query = "SELECT g.guid, gt.name FROM gameobject g JOIN gameobject_template gt ON gt.entry = g.id JOIN smart_scripts ss ON ss.entryorguid = -g.guid WHERE g.guid LIKE '%" + textBoxCriteria.Text + "%' AND ss.source_type = 1";
                            else
                                query = "SELECT g.guid, gt.name FROM gameobject_template gt JOIN gameobject g ON gt.entry = g.id JOIN smart_scripts ss ON ss.entryorguid = -g.guid WHERE g.guid = " + textBoxCriteria.Text + " AND ss.source_type = 1";
                        }
                        else
                        {
                            if (checkBoxFieldContainsCriteria.Checked)
                                query = "SELECT g.guid, gt.name FROM gameobject g JOIN gameobject_template gt ON gt.entry = g.id WHERE g.guid LIKE '%" + textBoxCriteria.Text + "%'";
                            else
                                query = "SELECT g.guid, gt.name FROM gameobject_template gt JOIN gameobject g ON gt.entry = g.id WHERE g.guid = " + textBoxCriteria.Text;
                        }
                    }

                    query += " ORDER BY g.guid";
                    break;
                case 6: //! Actionlist entry
                    ClearItemsOfListView(listViewEntryResults);

                    try
                    {
                        using (var connection = new MySqlConnection(connectionString.ToString()))
                        {
                            connection.Open();

                            string queryToExecute = "SELECT entryorguid, action_type, action_param1, action_param2, action_param3, action_param4, action_param5, action_param6 FROM smart_scripts WHERE action_type IN (80,87,88) AND source_type != 9";

                            if (!criteriaLeftEmpty)
                            {
                                if (checkBoxFieldContainsCriteria.Checked)
                                    queryToExecute += " AND entryorguid LIKE '%" + textBoxCriteria.Text + "%'";
                                else
                                    queryToExecute += " AND entryorguid = " + textBoxCriteria.Text;
                            }

                            queryToExecute += " ORDER BY entryorguid";

                            var returnVal = new MySqlDataAdapter(queryToExecute, connection);
                            var dataTable = new DataTable();
                            returnVal.Fill(dataTable);

                            if (dataTable.Rows.Count > 0)
                            {
                                foreach (DataRow row in dataTable.Rows)
                                {
                                    int entryorguid = Convert.ToInt32(row.ItemArray[0].ToString());
                                    int entry = entryorguid;

                                    if (entryorguid < 0)
                                        entry = await SAI_Editor_Manager.Instance.worldDatabase.GetCreatureIdByGuid(entryorguid * -1);

                                    string name = await SAI_Editor_Manager.Instance.worldDatabase.GetCreatureNameById(entry);
                                    int actionParam1 = Convert.ToInt32(row.ItemArray[2].ToString());
                                    int actionParam2 = Convert.ToInt32(row.ItemArray[3].ToString());

                                    switch ((SmartAction)Convert.ToInt32(row.ItemArray[1].ToString())) //! action type
                                    {
                                        case SmartAction.SMART_ACTION_CALL_TIMED_ACTIONLIST:
                                            AddItemToListView(listViewEntryResults, actionParam1.ToString(), name);
                                            break;
                                        case SmartAction.SMART_ACTION_CALL_RANDOM_TIMED_ACTIONLIST:
                                            for (int i = 2; i < 8; ++i)
                                            {
                                                if (row.ItemArray[i].ToString() == "0")
                                                    break; //! Once the first 0 is reached we can stop looking for other scripts, no gaps allowed

                                                AddItemToListView(listViewEntryResults, row.ItemArray[i].ToString(), name);
                                            }
                                            break;
                                        case SmartAction.SMART_ACTION_CALL_RANDOM_RANGE_TIMED_ACTIONLIST:
                                            for (int i = actionParam1; i <= actionParam2; ++i)
                                                AddItemToListView(listViewEntryResults, i.ToString(), name);
                                            break;
                                    }
                                }
                            }
                        }

                        SetEnabledOfControl(buttonSearch, true);
                        SetEnabledOfControl(buttonStopSearching, false);
                    }
                    catch (ThreadAbortException ex)
                    {
                        SetEnabledOfControl(buttonSearch, true);
                        SetEnabledOfControl(buttonStopSearching, false);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    return; //! We did everything in the switch block (we only do this for actionlists)
                default:
                    MessageBox.Show("An unknown index was found in the search type box!", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

            ClearItemsOfListView(listViewEntryResults);

            try
            {
                FillListViewWithQuery(query, true);
            }
            finally
            {
                SetEnabledOfControl(buttonSearch, true);
                SetEnabledOfControl(buttonStopSearching, false);
            }
        }

        private void SearchForEntryForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                {
                    if (listViewEntryResults.SelectedItems.Count > 0 && listViewEntryResults.Focused)
                        FillMainFormEntryOrGuidField(sender, e);
                    else
                        buttonSearch_Click(sender, e);

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

        private void textBoxCriteria_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxCriteria.Text) || String.IsNullOrWhiteSpace(textBoxCriteria.Text))
                return;

            switch (comboBoxSearchType.SelectedIndex)
            {
                case 2: //! Creature guid
                case 5: //! Gameobject guid
                case 0: //! Creature entry
                case 3: //! Gameobject entry
                    if (!Char.IsNumber(e.KeyChar))
                        e.Handled = e.KeyChar != (Char)Keys.Back && e.KeyChar != (Char)Keys.OemMinus;
                    break;
                case 1: //! Creature name
                case 4: //! Gameobject name
                    //! Allow any characters when searching for names
                    break;
            }
        }

        private void comboBoxSearchType_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true; //! Disallow changing content of the combobox, but setting it to 3D looks like shit
        }

        private void FillMainFormEntryOrGuidField(object sender, EventArgs e)
        {
            string entryToPlace = "";

            if (comboBoxSearchType.SelectedIndex == 2 || comboBoxSearchType.SelectedIndex == 5)
                entryToPlace = "-";

            entryToPlace += listViewEntryResults.SelectedItems[0].Text;
            ((MainForm)Owner).textBoxEntryOrGuid.Text = entryToPlace;

            //! Above 2 means it's a gameobject
            switch (comboBoxSearchType.SelectedIndex)
            {
                case 0: //! Creature
                case 1:
                case 2:
                    ((MainForm)Owner).comboBoxSourceType.SelectedIndex = 0;
                    break;
                case 3: //! Gameobject
                case 4:
                case 5:
                    ((MainForm)Owner).comboBoxSourceType.SelectedIndex = 1;
                    break;
                //case : //! Areatrigger
                //    ((MainForm)Owner).comboBoxSourceType.SelectedIndex = 2;
                //    break;
                case 6: //! Actionlist
                    ((MainForm)Owner).comboBoxSourceType.SelectedIndex = 3;
                    break;
            }

            if (Settings.Default.LoadScriptInstantly)
                ((MainForm)Owner).pictureBox1_Click(sender, e);

            Close();
        }

        private bool IsNumericIndex(int index)
        {
            switch (index)
            {
                case 1: //! Creature name
                case 4: //! Gameobject name:
                    return false;
                default:
                    return true;
            }
        }

        //! Cross-thread functions:
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

        private void SetTextOfControl(Control control, string text)
        {
            if (control.InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    control.Text = text;
                });
                return;
            }

            control.Text = text;
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

        private void comboBoxSearchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //! Disable the 'has ainame' checkbox when the user selected actionlist for search type
            checkBoxHasAiName.Enabled = comboBoxSearchType.SelectedIndex != 6;
        }

        private void SearchForEntryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopRunningThread();
        }
    }
}
