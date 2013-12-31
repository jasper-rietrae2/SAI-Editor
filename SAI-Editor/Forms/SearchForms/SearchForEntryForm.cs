using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using SAI_Editor.Classes.Database.Classes;
using SAI_Editor.Enumerators;
using SAI_Editor.Properties;
using SAI_Editor.Classes;

namespace SAI_Editor.Forms.SearchForms
{

    public partial class SearchForEntryForm : Form
    {
        private Thread searchThread = null;
        private readonly MySqlConnectionStringBuilder connectionString;
        private readonly SourceTypes sourceTypeToSearchFor;
        private readonly ListViewColumnSorter lvwColumnSorter = new ListViewColumnSorter();
        private CancellationTokenSource _cts;
        private int previousSearchType = 0;
        private bool _isBusy = false;

        public class Item
        {
            public string ItemName { get; set; }
            public List<string> SubItems { get; set; }
        }

        public SearchForEntryForm(MySqlConnectionStringBuilder connectionString, string startEntryString, SourceTypes sourceTypeToSearchFor)
        {
            this.InitializeComponent();

            this.connectionString = connectionString;
            this.sourceTypeToSearchFor = sourceTypeToSearchFor;
            this.textBoxCriteria.Text = startEntryString;

            this.MinimumSize = new Size(this.Width, this.Height);
            this.MaximumSize = new Size(this.Width, this.Height + 800);

            if (sourceTypeToSearchFor != SourceTypes.SourceTypeAreaTrigger)
            {
                this.listViewEntryResults.Columns.Add("Entry/guid", 70, HorizontalAlignment.Right);
                this.listViewEntryResults.Columns.Add("Name", 260, HorizontalAlignment.Left);
            }

            this._cts = new CancellationTokenSource();
        }

        private void SearchForEntryForm_Load(object sender, EventArgs e)
        {
            switch (this.sourceTypeToSearchFor)
            {
                case SourceTypes.SourceTypeCreature:
                    this.comboBoxSearchType.SelectedIndex = 0; //! Creature entry
                    this.FillListViewWithMySqlQuery("SELECT entry, name FROM creature_template ORDER BY entry LIMIT 1000");
                    break;
                case SourceTypes.SourceTypeGameobject:
                    this.comboBoxSearchType.SelectedIndex = 3; //! Gameobject entry
                    this.FillListViewWithMySqlQuery("SELECT entry, name FROM gameobject_template ORDER BY entry LIMIT 1000");
                    break;
                case SourceTypes.SourceTypeAreaTrigger:
                    this.comboBoxSearchType.SelectedIndex = 6; //! Areatrigger entry
                    this.listViewEntryResults.Columns.Add("Id", 53, HorizontalAlignment.Right);
                    this.listViewEntryResults.Columns.Add("Mapid", 52, HorizontalAlignment.Left);
                    this.listViewEntryResults.Columns.Add("X", 75, HorizontalAlignment.Left);
                    this.listViewEntryResults.Columns.Add("Y", 75, HorizontalAlignment.Left);
                    this.listViewEntryResults.Columns.Add("Z", 75, HorizontalAlignment.Left);
                    this.FillListViewWithAreaTriggers(String.Empty, String.Empty, true);
                    break;
                case SourceTypes.SourceTypeScriptedActionlist:
                    this.checkBoxHasAiName.Enabled = false;
                    this.comboBoxSearchType.SelectedIndex = 8; //! Actionlist entry
                    //! We don't list 1000 actionlists like all other source types because we can't get the entry/name combination
                    //! of several sources (considering the actionlist can be called from _ANY_ source_type (including actionlists
                    //! itself). It's simply not worth the time.
                    break;
            }
        }

        private void listViewEntryResults_DoubleClick(object sender, EventArgs e)
        {
            this.StopRunningThread();
            this.FillMainFormFields(sender, e);
        }

        private void FillListViewWithMySqlQuery(string queryToExecute)
        {
            this._cts = new CancellationTokenSource();

            try
            {
                using (var connection = new MySqlConnection(this.connectionString.ToString()))
                {
                    connection.Open();

                    List<Item> items = new List<Item>();

                    using (var query = new MySqlCommand(queryToExecute, connection))
                    {
                        using (MySqlDataReader reader = query.ExecuteReader())
                        {
                            while (reader != null && reader.Read())
                            {
                                if (this._cts.IsCancellationRequested)
                                    break;

                                items.Add(new Item { ItemName = reader.GetInt32(0).ToString(CultureInfo.InvariantCulture), SubItems = new List<string> { reader.GetString(1) } });
                            }
                        }
                    }

                    this.AddItemToListView(this.listViewEntryResults, items);

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.StopRunningThread();
            }
        }

        private async void FillListViewWithAreaTriggers(string idFilter, string mapIdFilter, bool limit)
        {
            this._cts = new CancellationTokenSource();

            try
            {
                string queryToExecute = "SELECT * FROM areatriggers";

                if (idFilter.Length > 0 || mapIdFilter.Length > 0)
                {
                    if (this.checkBoxFieldContainsCriteria.Checked)
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

                DataTable dt = await SAI_Editor_Manager.Instance.sqliteDatabase.ExecuteQueryWithCancellation(this._cts.Token, queryToExecute);

                if (dt.Rows.Count > 0)
                {
                    List<Item> items = new List<Item>();

                    foreach (DataRow row in dt.Rows)
                    {
                        if (this._cts.IsCancellationRequested)
                            break;

                        AreaTrigger areaTrigger = SAI_Editor_Manager.Instance.sqliteDatabase.BuildAreaTrigger(row);

                        if (!this.checkBoxHasAiName.Checked || await SAI_Editor_Manager.Instance.worldDatabase.AreaTriggerHasSmartAI(areaTrigger.id))
                            items.Add(new Item { ItemName = areaTrigger.id.ToString(), SubItems = new List<string> { areaTrigger.map_id.ToString(), areaTrigger.posX.ToString(), areaTrigger.posY.ToString(), areaTrigger.posZ.ToString() } });
                    }

                    this.AddItemToListView(this.listViewEntryResults, items);
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.StopRunningThread();
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            if (this._isBusy)
                return;

            this._isBusy = true;
            this.searchThread = new Thread(this.StartSearching);
            this.searchThread.Start();
        }

        private async void StartSearching()
        {
            this._cts = new CancellationTokenSource();

            try
            {
                string query = "";
                bool criteriaLeftEmpty = String.IsNullOrEmpty(this.textBoxCriteria.Text) || String.IsNullOrWhiteSpace(this.textBoxCriteria.Text);

                if (!criteriaLeftEmpty && this.IsNumericIndex(this.GetSelectedIndexOfComboBox(this.comboBoxSearchType)) && Convert.ToInt32(this.textBoxCriteria.Text) < 0)
                {
                    bool pressedYes = true;

                    this.Invoke(new Action(() =>
                    {
                        pressedYes = MessageBox.Show("The criteria field can not be a negative value, would you like me to set it to a positive number and continue the search?", "Something went wrong!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
                    }));

                    if (!pressedYes)
                    {
                        this.StopRunningThread();
                        return;
                    }

                    this.SetTextOfControl(this.textBoxCriteria, (Convert.ToInt32(this.textBoxCriteria.Text) * -1).ToString());
                }

                this.SetEnabledOfControl(this.buttonSearch, false);
                this.SetEnabledOfControl(this.buttonStopSearching, true);

                switch (this.GetSelectedIndexOfComboBox(this.comboBoxSearchType))
                {
                    case 0: //! Creature entry
                        query = "SELECT entry, name FROM creature_template";

                        if (!criteriaLeftEmpty)
                        {
                            if (this.checkBoxFieldContainsCriteria.Checked)
                                query += " WHERE entry LIKE '%" + this.textBoxCriteria.Text + "%'";
                            else
                                query += " WHERE entry=" + this.textBoxCriteria.Text;
                        }

                        if (this.checkBoxHasAiName.Checked)
                            query += (criteriaLeftEmpty ? " WHERE" : " AND") + " AIName='SmartAI'";

                        query += " ORDER BY entry";
                        break;
                    case 1: //! Creature name
                        query = "SELECT entry, name FROM creature_template WHERE name LIKE '%" + this.textBoxCriteria.Text + "%'";

                        if (this.checkBoxHasAiName.Checked)
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
                                    this.StopRunningThread();
                                    return;
                                }
                            }

                            if (this.checkBoxHasAiName.Checked)
                                query = "SELECT c.guid, ct.name FROM creature_template ct JOIN creature c ON ct.entry = c.id JOIN smart_scripts ss ON ss.entryorguid < 0 AND ss.entryorguid = -c.guid AND ss.source_type = 0";
                            else
                                query = "SELECT c.guid, ct.name FROM creature_template ct JOIN creature c ON ct.entry = c.id";
                        }
                        else
                        {
                            if (this.checkBoxHasAiName.Checked)
                            {
                                if (this.checkBoxFieldContainsCriteria.Checked)
                                    query = "SELECT c.guid, ct.name FROM creature c JOIN creature_template ct ON ct.entry = c.id JOIN smart_scripts ss ON ss.entryorguid = -c.guid WHERE c.guid LIKE '%" + this.textBoxCriteria.Text + "%' AND ss.source_type = 0";
                                else
                                    query = "SELECT c.guid, ct.name FROM creature_template ct JOIN creature c ON ct.entry = c.id JOIN smart_scripts ss ON ss.entryorguid = -c.guid WHERE c.guid = " + this.textBoxCriteria.Text;
                            }
                            else
                            {
                                if (this.checkBoxFieldContainsCriteria.Checked)
                                    query = "SELECT c.guid, ct.name FROM creature c JOIN creature_template ct ON ct.entry = c.id WHERE c.guid LIKE '%" + this.textBoxCriteria.Text + "%'";
                                else
                                    query = "SELECT c.guid, ct.name FROM creature_template ct JOIN creature c ON ct.entry = c.id WHERE c.guid = " + this.textBoxCriteria.Text;
                            }
                        }

                        query += " ORDER BY c.guid";
                        break;
                    case 3: //! Gameobject entry
                        query = "SELECT entry, name FROM gameobject_template";

                        if (!criteriaLeftEmpty)
                        {
                            if (this.checkBoxFieldContainsCriteria.Checked)
                                query += " WHERE entry LIKE '%" + this.textBoxCriteria.Text + "%'";
                            else
                                query += " WHERE entry=" + this.textBoxCriteria.Text;
                        }

                        if (this.checkBoxHasAiName.Checked)
                            query += (criteriaLeftEmpty ? " WHERE" : " AND") + " AIName='SmartGameObjectAI'";

                        query += " ORDER BY entry";
                        break;
                    case 4: //! Gameobject name
                        query = "SELECT entry, name FROM gameobject_template WHERE name LIKE '%" + this.textBoxCriteria.Text + "%'";

                        if (this.checkBoxHasAiName.Checked)
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
                                    this.StopRunningThread();
                                    return;
                                }
                            }

                            if (this.checkBoxHasAiName.Checked)
                                query = "SELECT g.guid, gt.name FROM gameobject_template gt JOIN gameobject g ON gt.entry = g.id JOIN smart_scripts ss ON ss.entryorguid < 0 AND ss.entryorguid = -g.guid AND ss.source_type = 1";
                            else
                                query = "SELECT g.guid, gt.name FROM gameobject_template gt JOIN gameobject g ON gt.entry = g.id";
                        }
                        else
                        {
                            if (this.checkBoxHasAiName.Checked)
                            {
                                if (this.checkBoxFieldContainsCriteria.Checked)
                                    query = "SELECT g.guid, gt.name FROM gameobject g JOIN gameobject_template gt ON gt.entry = g.id JOIN smart_scripts ss ON ss.entryorguid = -g.guid WHERE g.guid LIKE '%" + this.textBoxCriteria.Text + "%' AND ss.source_type = 1";
                                else
                                    query = "SELECT g.guid, gt.name FROM gameobject_template gt JOIN gameobject g ON gt.entry = g.id JOIN smart_scripts ss ON ss.entryorguid = -g.guid WHERE g.guid = " + this.textBoxCriteria.Text + " AND ss.source_type = 1";
                            }
                            else
                            {
                                if (this.checkBoxFieldContainsCriteria.Checked)
                                    query = "SELECT g.guid, gt.name FROM gameobject g JOIN gameobject_template gt ON gt.entry = g.id WHERE g.guid LIKE '%" + this.textBoxCriteria.Text + "%'";
                                else
                                    query = "SELECT g.guid, gt.name FROM gameobject_template gt JOIN gameobject g ON gt.entry = g.id WHERE g.guid = " + this.textBoxCriteria.Text;
                            }
                        }

                        query += " ORDER BY g.guid";
                        break;
                    case 6: //! Areatrigger id
                    case 7: //! Areatrigger map id
                        this.ClearItemsOfListView(this.listViewEntryResults);

                        try
                        {
                            string areaTriggerIdFilter = "", areaTriggerMapIdFilter = "";

                            if (this.GetSelectedIndexOfComboBox(this.comboBoxSearchType) == 6)
                                areaTriggerIdFilter = this.textBoxCriteria.Text;
                            else
                                areaTriggerMapIdFilter = this.textBoxCriteria.Text;

                            this.FillListViewWithAreaTriggers(areaTriggerIdFilter, areaTriggerMapIdFilter, false);
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
                            this.StopRunningThread();
                        }
                        finally
                        {
                            this._isBusy = false;
                            this.SetEnabledOfControl(this.buttonSearch, true);
                            this.SetEnabledOfControl(this.buttonStopSearching, false);
                        }
                        return;
                    case 8: //! Actionlist entry
                        this.ClearItemsOfListView(this.listViewEntryResults);

                        try
                        {
                            List<SmartScript> smartScriptActionlists = await SAI_Editor_Manager.Instance.worldDatabase.GetSmartScriptActionLists(this.textBoxCriteria.Text, this.checkBoxFieldContainsCriteria.Checked);

                            if (smartScriptActionlists != null)
                            {

                                List<Item> items = new List<Item>();

                                foreach (SmartScript smartScript in smartScriptActionlists)
                                {
                                    if (this._cts.IsCancellationRequested)
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

                                this.AddItemToListView(this.listViewEntryResults, items);

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
                            this.StopRunningThread();
                        }
                        finally
                        {
                            this._isBusy = false;
                            this.SetEnabledOfControl(this.buttonSearch, true);
                            this.SetEnabledOfControl(this.buttonStopSearching, false);
                        }

                        return; //! We did everything in the switch block (we only do this for actionlists)
                    default:
                        MessageBox.Show("An unknown index was found in the search type box!", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.StopRunningThread();
                        return;
                }

                this.ClearItemsOfListView(this.listViewEntryResults);

                try
                {
                    this.FillListViewWithMySqlQuery(query);
                }
                finally
                {
                    this.SetEnabledOfControl(this.buttonSearch, true);
                    this.SetEnabledOfControl(this.buttonStopSearching, false);
                    this._isBusy = false;
                }
            }
            catch (ThreadAbortException) //! Don't show a message when the thread was already cancelled
            {
                this.SetEnabledOfControl(this.buttonSearch, true);
                this.SetEnabledOfControl(this.buttonStopSearching, false);
                this.StopRunningThread();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.StopRunningThread();
            }
        }

        private void SearchForEntryForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                {
                    if (this.listViewEntryResults.SelectedItems.Count > 0 && this.listViewEntryResults.Focused)
                        this.FillMainFormFields(sender, e);
                    else
                        this.buttonSearch.PerformClick();

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
            if (this.searchThread != null && this._cts != null && this.searchThread.IsAlive)
                this._cts.Cancel();

            this._isBusy = false;
        }

        private void textBoxCriteria_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (String.IsNullOrEmpty(this.textBoxCriteria.Text) || String.IsNullOrWhiteSpace(this.textBoxCriteria.Text))
                return;

            switch (this.comboBoxSearchType.SelectedIndex)
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
            if (this.comboBoxSearchType.SelectedIndex == 2 || this.comboBoxSearchType.SelectedIndex == 5)
                entryToPlace = "-";

            entryToPlace += this.listViewEntryResults.SelectedItems[0].Text;
            ((MainForm)this.Owner).textBoxEntryOrGuid.Text = entryToPlace;

            switch (this.comboBoxSearchType.SelectedIndex)
            {
                case 0: //! Creature entry
                case 1: //! Creature name
                case 2: //! Creature guid
                    ((MainForm)this.Owner).comboBoxSourceType.SelectedIndex = 0;
                    break;
                case 3: //! Gameobject entry
                case 4: //! Gameobject name
                case 5: //! Gameobject guid
                    ((MainForm)this.Owner).comboBoxSourceType.SelectedIndex = 1;
                    break;
                case 6: //! Areatrigger id
                case 7: //! Areatrigger map id
                    ((MainForm)this.Owner).comboBoxSourceType.SelectedIndex = 2;
                    break;
                case 8: //! Actionlist entry
                    ((MainForm)this.Owner).comboBoxSourceType.SelectedIndex = 3;
                    break;
            }

            if (((MainForm)this.Owner).pictureBoxLoadScript.Enabled)
                ((MainForm)this.Owner).TryToLoadScript(-1, SourceTypes.SourceTypeNone, true, true);

            this.Close();
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
                return (int)comboBox.Invoke(new Func<int>(() => this.GetSelectedIndexOfComboBox(comboBox)));

            return comboBox.SelectedIndex;
        }

        private void AddItemToListView(ListView listView, IEnumerable<Item> items)
        {
            try
            {
                List<ListViewItem> lvItems = new List<ListViewItem>();

                this.Invoke((MethodInvoker)delegate
                {
                    foreach (var item in items)
                    {
                        var lvi = new ListViewItem(item.ItemName);

                        foreach (string subItem in item.SubItems)
                            lvi.SubItems.Add(subItem);

                        lvItems.Add(lvi);
                    }

                    listView.Items.AddRange(lvItems.ToArray());
                });
            }
            catch (InvalidOperationException)
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void SetTextOfControl(Control control, string text)
        {
            if (control.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
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

        private void comboBoxSearchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //! Disable the 'has ainame' checkbox when the user selected actionlist for search type
            this.checkBoxHasAiName.Enabled = this.comboBoxSearchType.SelectedIndex != 8;
            this.listViewEntryResults.Columns.Clear();
            bool previousSearchForAreaTrigger = this.previousSearchType == 6 || this.previousSearchType == 7;

            switch (this.comboBoxSearchType.SelectedIndex)
            {
                case 0: //! Creature entry
                case 2: //! Creature guid
                case 3: //! Gameobject entry
                case 5: //! Gameobject guid
                case 8: //! Actionlist
                    if (previousSearchForAreaTrigger)
                    {
                        this.StopRunningThread();
                        this.listViewEntryResults.Items.Clear();
                    }

                    this.textBoxCriteria.Text = Regex.Replace(this.textBoxCriteria.Text, "[^.0-9]", "");
                    this.listViewEntryResults.Columns.Add("Entry/guid", 70, HorizontalAlignment.Right);
                    this.listViewEntryResults.Columns.Add("Name", 260, HorizontalAlignment.Left);
                    break;
                case 1: //! Creature name
                case 4: //! Gameobject name
                    if (previousSearchForAreaTrigger)
                    {
                        this.StopRunningThread();
                        this.listViewEntryResults.Items.Clear();
                    }

                    this.listViewEntryResults.Columns.Add("Entry/guid", 70, HorizontalAlignment.Right);
                    this.listViewEntryResults.Columns.Add("Name", 260, HorizontalAlignment.Left);
                    break;
                case 6: //! Areatrigger id
                case 7: //! Areatrigger map id
                    if (!previousSearchForAreaTrigger)
                    {
                        this.StopRunningThread();
                        this.listViewEntryResults.Items.Clear();
                    }

                    this.textBoxCriteria.Text = Regex.Replace(this.textBoxCriteria.Text, "[^.0-9]", "");
                    this.listViewEntryResults.Columns.Add("Id", 53, HorizontalAlignment.Right);
                    this.listViewEntryResults.Columns.Add("Mapid", 52, HorizontalAlignment.Left);
                    this.listViewEntryResults.Columns.Add("X", 75, HorizontalAlignment.Left);
                    this.listViewEntryResults.Columns.Add("Y", 75, HorizontalAlignment.Left);
                    this.listViewEntryResults.Columns.Add("Z", 75, HorizontalAlignment.Left);
                    break;
            }

            this.previousSearchType = this.comboBoxSearchType.SelectedIndex;
        }

        private void SearchForEntryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StopRunningThread();
        }
    }
}
