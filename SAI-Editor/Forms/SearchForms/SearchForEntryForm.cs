using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading;
using SAI_Editor.Properties;
using MySql.Data.MySqlClient;
using SAI_Editor.Database.Classes;

namespace SAI_Editor
{
    public partial class SearchForEntryForm : Form
    {
        private Thread searchThread = null;
        private readonly MySqlConnectionStringBuilder connectionString;
        private readonly SourceTypes sourceTypeToSearchFor;
        private readonly ListViewColumnSorter lvwColumnSorter = new ListViewColumnSorter();
        private int previousSearchType = 0;

        private bool _isBusy = false;

        private CancellationTokenSource _cts;

        public SearchForEntryForm(MySqlConnectionStringBuilder connectionString, string startEntryString, SourceTypes sourceTypeToSearchFor)
        {
            InitializeComponent();

            this.connectionString = connectionString;
            this.sourceTypeToSearchFor = sourceTypeToSearchFor;
            textBoxCriteria.Text = startEntryString;

            MinimumSize = new Size(Width, Height);
            MaximumSize = new Size(Width, Height + 800);

            if (sourceTypeToSearchFor != SourceTypes.SourceTypeAreaTrigger)
            {
                listViewEntryResults.Columns.Add("Entry/guid", 70, HorizontalAlignment.Right);
                listViewEntryResults.Columns.Add("Name", 260, HorizontalAlignment.Left);
            }

            listViewEntryResults.ColumnClick += listViewEntryResults_ColumnClick;
            listViewEntryResults.Anchor = AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left;

            _cts = new CancellationTokenSource();

            switch (sourceTypeToSearchFor)
            {
                case SourceTypes.SourceTypeCreature:
                    comboBoxSearchType.SelectedIndex = 0; //! Creature entry
                    FillListViewWithMySqlQuery("SELECT entry, name FROM creature_template ORDER BY entry LIMIT 1000");
                    break;
                case SourceTypes.SourceTypeGameobject:
                    comboBoxSearchType.SelectedIndex = 3; //! Gameobject entry
                    FillListViewWithMySqlQuery("SELECT entry, name FROM gameobject_template ORDER BY entry LIMIT 1000");
                    break;
                case SourceTypes.SourceTypeAreaTrigger:
                    comboBoxSearchType.SelectedIndex = 6; //! NYI
                    listViewEntryResults.Columns.Add("Id", 53, HorizontalAlignment.Right);
                    listViewEntryResults.Columns.Add("Mapid", 52, HorizontalAlignment.Left);
                    listViewEntryResults.Columns.Add("X", 75, HorizontalAlignment.Left);
                    listViewEntryResults.Columns.Add("Y", 75, HorizontalAlignment.Left);
                    listViewEntryResults.Columns.Add("Z", 75, HorizontalAlignment.Left);
                    FillListViewWithAreaTriggers(String.Empty, String.Empty, true);
                    break;
                case SourceTypes.SourceTypeScriptedActionlist:
                    checkBoxHasAiName.Enabled = false;
                    comboBoxSearchType.SelectedIndex = 8; //! Actionlist entry
                    //! We don't list 1000 actionlists like all other source types because we can't get the entry/name combination
                    //! of several sources (considering the actionlist can be called from _ANY_ source_type (including actionlists
                    //! itself). It's simply not worth the time.
                    break;
            }
        }

        private void listViewEntryResults_DoubleClick(object sender, EventArgs e)
        {
            StopRunningThread();
            FillMainFormFields(sender, e);
        }

        private void FillListViewWithMySqlQuery(string queryToExecute)
        {
            _cts = new CancellationTokenSource();

            try
            {
                using (var connection = new MySqlConnection(connectionString.ToString()))
                {
                    connection.Open();

                    List<Item> items = new List<Item>();

                    using (var query = new MySqlCommand(queryToExecute, connection))
                    {
                        using (MySqlDataReader reader = query.ExecuteReader())
                        {
                            while (reader != null && reader.Read())
                            {
                                if (_cts.IsCancellationRequested)
                                    break;

                                items.Add(new Item { ItemName = reader.GetInt32(0).ToString(CultureInfo.InvariantCulture), SubItems = new List<string> { reader.GetString(1) } });
                            }
                        }
                    }

                    AddItemToListView(listViewEntryResults, items);

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StopRunningThread();
            }
        }

        private async void FillListViewWithAreaTriggers(string idFilter, string mapIdFilter, bool limit)
        {
            _cts = new CancellationTokenSource();

            try
            {
                string queryToExecute = "SELECT * FROM areatriggers";

                if (idFilter.Length > 0 || mapIdFilter.Length > 0)
                {
                    if (checkBoxFieldContainsCriteria.Checked)
                    {
                        if (idFilter.Length > 0)
                        {
                            queryToExecute += " WHERE id LIKE '%" + idFilter + "%'";

                            if (mapIdFilter.Length > 0)
                                queryToExecute += " AND mapId LIKE '%" + mapIdFilter + "%'";
                        }
                        else if (mapIdFilter.Length > 0)
                        {
                            queryToExecute += " WHERE mapId LIKE '%" + mapIdFilter + "%'";

                            if (idFilter.Length > 0)
                                queryToExecute += " AND id LIKE '%" + idFilter + "%'";
                        }
                    }
                    else
                    {
                        if (idFilter.Length > 0)
                        {
                            queryToExecute += " WHERE id = " + idFilter;

                            if (mapIdFilter.Length > 0)
                                queryToExecute += " AND mapId = " + mapIdFilter;
                        }
                        else if (mapIdFilter.Length > 0)
                        {
                            queryToExecute += " WHERE mapId = " + mapIdFilter;

                            if (idFilter.Length > 0)
                                queryToExecute += " AND id = " + idFilter;
                        }
                    }
                }

                if (limit)
                    queryToExecute += " LIMIT 1000";

                DataTable dt = await SAI_Editor_Manager.Instance.sqliteDatabase.ExecuteQueryWithCancellation(_cts.Token, queryToExecute);

                if (dt.Rows.Count > 0)
                {

                    List<Item> items = new List<Item>();

                    foreach (DataRow row in dt.Rows)
                    {
                        if (_cts.IsCancellationRequested)
                            break;

                        AreaTrigger areaTrigger = SAI_Editor_Manager.Instance.sqliteDatabase.BuildAreaTrigger(row);

                        if (!checkBoxHasAiName.Checked || await SAI_Editor_Manager.Instance.worldDatabase.AreaTriggerHasSmartAI(areaTrigger.id))
                            items.Add(new Item { ItemName = areaTrigger.id.ToString(), SubItems = new List<string> { areaTrigger.map_id.ToString(), areaTrigger.posX.ToString(), areaTrigger.posY.ToString(), areaTrigger.posZ.ToString() } });
                            //AddItemToListView(listViewEntryResults, areaTrigger.id.ToString(), areaTrigger.map_id.ToString(), areaTrigger.posX.ToString(), areaTrigger.posY.ToString(), areaTrigger.posZ.ToString());

                    }

                    AddItemToListView(listViewEntryResults, items);

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StopRunningThread();
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            if (_isBusy)
                return;

            _isBusy = true;
            searchThread = new Thread(StartSearching);
            searchThread.Start();
        }

        private async void StartSearching()
        {
            _cts = new CancellationTokenSource();

            try
            {
                string query = "";
                bool criteriaLeftEmpty = String.IsNullOrEmpty(textBoxCriteria.Text) || String.IsNullOrWhiteSpace(textBoxCriteria.Text);

                if (!criteriaLeftEmpty && IsNumericIndex(GetSelectedIndexOfComboBox(comboBoxSearchType)) && Convert.ToInt32(textBoxCriteria.Text) < 0)
                {
                    bool pressedYes = true;

                    this.Invoke(new Action(() =>
                    {
                        pressedYes = MessageBox.Show("The criteria field can not be a negative value, would you like me to set it to a positive number and continue the search?", "Something went wrong!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
                    }));

                    if (!pressedYes)
                    {
                        StopRunningThread();
                        return;
                    }

                    SetTextOfControl(textBoxCriteria, (Convert.ToInt32(textBoxCriteria.Text) * -1).ToString());
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
                            if (Settings.Default.PromptExecuteQuery)
                            {
                                bool pressedYes = true;

                                this.Invoke(new Action(() =>
                                {
                                    pressedYes = MessageBox.Show(this, "Are you sure you wish to continue? This query will take a long time to execute because the criteria field was left empty!", "Are you sure you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
                                }));

                                if (!pressedYes)
                                {
                                    StopRunningThread();
                                    return;
                                }
                            }

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
                            if (Settings.Default.PromptExecuteQuery)
                            {
                                bool pressedYes = true;

                                this.Invoke(new Action(() =>
                                {
                                    pressedYes = MessageBox.Show(this, "Are you sure you wish to continue? This query will take a long time to execute because the criteria field was left empty!", "Are you sure you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
                                }));

                                if (!pressedYes)
                                {
                                    StopRunningThread();
                                    return;
                                }
                            }

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
                    case 6:
                    case 7:
                        ClearItemsOfListView(listViewEntryResults);

                        try
                        {
                            string areaTriggerIdFilter = "", areaTriggerMapIdFilter = "";

                            if (GetSelectedIndexOfComboBox(comboBoxSearchType) == 6)
                                areaTriggerIdFilter = textBoxCriteria.Text;
                            else
                                areaTriggerMapIdFilter = textBoxCriteria.Text;

                            FillListViewWithAreaTriggers(areaTriggerIdFilter, areaTriggerMapIdFilter, false);
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
                            StopRunningThread();
                        }
                        finally
                        {
                            SetEnabledOfControl(buttonSearch, true);
                            SetEnabledOfControl(buttonStopSearching, false);
                        }
                        return;
                    case 8: //! Actionlist entry
                        ClearItemsOfListView(listViewEntryResults);

                        try
                        {
                            List<SmartScript> smartScriptActionlists = await SAI_Editor_Manager.Instance.worldDatabase.GetSmartScriptActionLists(textBoxCriteria.Text, checkBoxFieldContainsCriteria.Checked);

                            if (smartScriptActionlists != null)
                            {

                                List<Item> items = new List<Item>();

                                foreach (SmartScript smartScript in smartScriptActionlists)
                                {
                                    if (_cts.IsCancellationRequested)
                                        break;

                                    int entryorguid = smartScript.entryorguid;
                                    int source_type = smartScript.source_type;

                                    //! If the entryorguid is below 0 it means the script is for a creature. We need to get
                                    //! the creature_template.entry by the guid in order to obtain the creature_template.name
                                    //! field now.
                                    if (entryorguid < 0)
                                        entryorguid = await SAI_Editor_Manager.Instance.worldDatabase.GetObjectIdByGuidAndSourceType(entryorguid * -1, source_type);

                                    string name = await SAI_Editor_Manager.Instance.worldDatabase.GetObjectNameByIdAndSourceType(entryorguid, source_type);
                                    int actionParam1 = smartScript.action_param1;
                                    int actionParam2 = smartScript.action_param2;

                                    switch ((SmartAction)smartScript.action_type) //! action type
                                    {
                                        case SmartAction.SMART_ACTION_CALL_TIMED_ACTIONLIST:
                                            items.Add(new Item { ItemName = actionParam1.ToString(), SubItems = new List<string>() { name } });
                                            //AddItemToListView(listViewEntryResults, actionParam1.ToString(), name);
                                            break;
                                        case SmartAction.SMART_ACTION_CALL_RANDOM_TIMED_ACTIONLIST:

                                            items.Add(new Item { ItemName = smartScript.action_param1.ToString(), SubItems = new List<string> { name } });
                                            items.Add(new Item { ItemName = smartScript.action_param2.ToString(), SubItems = new List<string> { name } });


                                            if (smartScript.action_param3 > 0)
                                                items.Add(new Item { ItemName = smartScript.action_param3.ToString(), SubItems = new List<string> { name } });
                                                //AddItemToListView(listViewEntryResults, smartScript.action_param3.ToString(), name);

                                            if (smartScript.action_param4 > 0)
                                                items.Add(new Item { ItemName = smartScript.action_param4.ToString(), SubItems = new List<string> { name } });
                                                //AddItemToListView(listViewEntryResults, smartScript.action_param4.ToString(), name);

                                            if (smartScript.action_param5 > 0)
                                                items.Add(new Item { ItemName = smartScript.action_param5.ToString(), SubItems = new List<string> { name } });
                                                //AddItemToListView(listViewEntryResults, smartScript.action_param5.ToString(), name);

                                            if (smartScript.action_param6 > 0)
                                                items.Add(new Item { ItemName = smartScript.action_param6.ToString(), SubItems = new List<string> { name } });
                                                //AddItemToListView(listViewEntryResults, smartScript.action_param6.ToString(), name);

                                            break;
                                        case SmartAction.SMART_ACTION_CALL_RANDOM_RANGE_TIMED_ACTIONLIST:
                                            for (int i = actionParam1; i <= actionParam2; ++i)
                                                items.Add(new Item { ItemName = i.ToString(), SubItems = new List<string> { name } });
                                                //AddItemToListView(listViewEntryResults, i.ToString(), name);
                                            break;
                                    }
                                }

                                AddItemToListView(listViewEntryResults, items);

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
                            StopRunningThread();
                        }
                        finally
                        {
                            _isBusy = false;
                            SetEnabledOfControl(buttonSearch, true);
                            SetEnabledOfControl(buttonStopSearching, false);
                        }

                        return; //! We did everything in the switch block (we only do this for actionlists)
                    default:
                        MessageBox.Show("An unknown index was found in the search type box!", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        StopRunningThread();
                        return;
                }

                ClearItemsOfListView(listViewEntryResults);

                try
                {
                    FillListViewWithMySqlQuery(query);
                }
                finally
                {
                    SetEnabledOfControl(buttonSearch, true);
                    SetEnabledOfControl(buttonStopSearching, false);
                    _isBusy = false;
                }
            }
            catch (ThreadAbortException) //! Don't show a message when the thread was already cancelled
            {
                SetEnabledOfControl(buttonSearch, true);
                SetEnabledOfControl(buttonStopSearching, false);
                StopRunningThread();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StopRunningThread();
            }
        }

        private void SearchForEntryForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                {
                    if (listViewEntryResults.SelectedItems.Count > 0 && listViewEntryResults.Focused)
                        FillMainFormFields(sender, e);
                    else
                        buttonSearch.PerformClick();

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
            if (searchThread != null && _cts != null && searchThread.IsAlive)
                _cts.Cancel();

            _isBusy = false;
        }

        private void textBoxCriteria_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxCriteria.Text) || String.IsNullOrWhiteSpace(textBoxCriteria.Text))
                return;

            switch (comboBoxSearchType.SelectedIndex)
            {
                case 0: //! Creature entry
                case 2: //! Creature guid
                case 3: //! Gameobject entry
                case 5: //! Gameobject guid
                case 6: //! Areatrigger id
                case 7: //! Areatrigger map id
                case 8: //! Actionlist entry
                    //if (!Char.IsNumber(e.KeyChar))
                    //    e.Handled = e.KeyChar != (Char)Keys.Back && e.KeyChar != (Char)Keys.OemMinus;
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

        private void FillMainFormFields(object sender, EventArgs e)
        {
            string entryToPlace = "";

            //! If we're searching for a creature guid or gameobject guid we have to make the value negative.
            if (comboBoxSearchType.SelectedIndex == 2 || comboBoxSearchType.SelectedIndex == 5)
                entryToPlace = "-";

            entryToPlace += listViewEntryResults.SelectedItems[0].Text;
            ((MainForm)Owner).textBoxEntryOrGuid.Text = entryToPlace;

            switch (comboBoxSearchType.SelectedIndex)
            {
                case 0: //! Creature entry
                case 1: //! Creature name
                case 2: //! Creature guid
                    ((MainForm)Owner).comboBoxSourceType.SelectedIndex = 0;
                    break;
                case 3: //! Gameobject entry
                case 4: //! Gameobject name
                case 5: //! Gameobject guid
                    ((MainForm)Owner).comboBoxSourceType.SelectedIndex = 1;
                    break;
                case 6: //! Areatrigger id
                case 7: //! Areatrigger map id
                    ((MainForm)Owner).comboBoxSourceType.SelectedIndex = 2;
                    break;
                case 8: //! Actionlist entry
                    ((MainForm)Owner).comboBoxSourceType.SelectedIndex = 3;
                    break;
            }

            if (Settings.Default.LoadScriptInstantly && ((MainForm)Owner).pictureBoxLoadScript.Enabled)
                ((MainForm)Owner).TryToLoadScript(-1, SourceTypes.SourceTypeNone, true, true);

            Close();
        }

        private bool IsNumericIndex(int index)
        {
            switch (index)
            {
                case 1: //! Creature name
                case 4: //! Gameobject name
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

        private void AddItemToListView(ListView listView, IEnumerable<Item> items)
        {

            List<ListViewItem> lvItems = new List<ListViewItem>();

            Invoke((MethodInvoker)delegate
            {

                foreach (var item in items)
                {

                    var lvi = new ListViewItem(item.ItemName);

                    foreach (string subItem in item.SubItems)
                    {
                        lvi.SubItems.Add(subItem);
                    }

                    lvItems.Add(lvi);

                }

                listView.Items.AddRange(lvItems.ToArray());
            });

        }

        //private void AddItemToListView(ListView listView, string item, string subItem1, string subItem2, string subItem3, string subItem4)
        //{
        //    if (listView.InvokeRequired)
        //    {
        //        Invoke((MethodInvoker)delegate
        //        {
        //            ListViewItem listViewItem = listView.Items.Add(item);
        //            listViewItem.SubItems.Add(subItem1);
        //            listViewItem.SubItems.Add(subItem2);
        //            listViewItem.SubItems.Add(subItem3);
        //            listViewItem.SubItems.Add(subItem4);
        //        });
        //        return;
        //    }

        //    ListViewItem listViewItem2 = listView.Items.Add(item);
        //    listViewItem2.SubItems.Add(subItem1);
        //    listViewItem2.SubItems.Add(subItem2);
        //    listViewItem2.SubItems.Add(subItem3);
        //    listViewItem2.SubItems.Add(subItem4);
        //}

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
            checkBoxHasAiName.Enabled = comboBoxSearchType.SelectedIndex != 8;
            listViewEntryResults.Columns.Clear();

            switch (comboBoxSearchType.SelectedIndex)
            {
                case 0: //! Creature entry
                case 2: //! Creature guid
                case 3: //! Gameobject entry
                case 5: //! Gameobject guid
                case 8: //! Actionlist
                    if (previousSearchType == 6 || previousSearchType == 7)
                    {
                        StopRunningThread();
                        listViewEntryResults.Items.Clear();
                    }

                    textBoxCriteria.Text = Regex.Replace(textBoxCriteria.Text, "[^.0-9]", "");
                    listViewEntryResults.Columns.Add("Entry/guid", 70, HorizontalAlignment.Right);
                    listViewEntryResults.Columns.Add("Name", 260, HorizontalAlignment.Left);
                    break;
                case 6: //! Areatrigger id
                case 7: //! Areatrigger map id
                    if (!(previousSearchType == 6 || previousSearchType == 7))
                    {
                        StopRunningThread();
                        listViewEntryResults.Items.Clear();
                    }

                    textBoxCriteria.Text = Regex.Replace(textBoxCriteria.Text, "[^.0-9]", "");
                    listViewEntryResults.Columns.Add("Id", 53, HorizontalAlignment.Right);
                    listViewEntryResults.Columns.Add("Mapid", 52, HorizontalAlignment.Left);
                    listViewEntryResults.Columns.Add("X", 75, HorizontalAlignment.Left);
                    listViewEntryResults.Columns.Add("Y", 75, HorizontalAlignment.Left);
                    listViewEntryResults.Columns.Add("Z", 75, HorizontalAlignment.Left);
                    break;
                case 1: //! Creature name
                case 4: //! Gameobject name
                    if (previousSearchType == 6 || previousSearchType == 7)
                    {
                        StopRunningThread();
                        listViewEntryResults.Items.Clear();
                    }

                    listViewEntryResults.Columns.Add("Entry/guid", 70, HorizontalAlignment.Right);
                    listViewEntryResults.Columns.Add("Name", 260, HorizontalAlignment.Left);
                    break;
            }

            previousSearchType = comboBoxSearchType.SelectedIndex;
        }

        private void SearchForEntryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopRunningThread();
        }

        public class Item
        {

            public string ItemName { get; set; }

            public List<string> SubItems { get; set; }

        }

    }
}
