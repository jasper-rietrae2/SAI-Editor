using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Threading;
using MySql.Data.MySqlClient;

namespace SAI_Editor
{
    public partial class SearchForEntryForm : Form
    {
        private Thread searchThread = null;
        private readonly MySqlConnectionStringBuilder connectionString;
        private readonly bool searchingForCreature = false;
        private readonly ListViewColumnSorter lvwColumnSorter = new ListViewColumnSorter();

        public SearchForEntryForm(MySqlConnectionStringBuilder connectionString, string startEntryString, bool searchingForCreature)
        {
            InitializeComponent();

            this.connectionString = connectionString;
            this.searchingForCreature = searchingForCreature;
            textBoxCriteria.Text = startEntryString;
        }

        private void SearchForCreatureForm_Load(object sender, EventArgs e)
        {
            MinimumSize = new Size(Width, Height);
            MaximumSize = new Size(Width, Height + 800);

            comboBoxSearchType.SelectedIndex = searchingForCreature ? 0 : 3;

            listViewEntryResults.Columns.Add("Entry/guid", 70, HorizontalAlignment.Right);
            listViewEntryResults.Columns.Add("Name", 260, HorizontalAlignment.Left);

            listViewEntryResults.ColumnClick += listViewEntryResults_ColumnClick;

            listViewEntryResults.Anchor = AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left;
            SelectFromCreatureTemplate(String.Format("SELECT entry, name FROM {0} ORDER BY entry LIMIT 1000", (searchingForCreature ? "creature_template" : "gameobject_template")), false);
        }

        private void listViewEntryResults_DoubleClick(object sender, EventArgs e)
        {
            TryToStopRunningThread();
            FillMainFormEntryOrGuidField(sender, e);
        }

        private void SelectFromCreatureTemplate(string queryToExecute, bool crossThread)
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

        private void StartSearching()
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
                case 0: //! Creature name
                    query = "SELECT entry, name FROM creature_template WHERE name LIKE '%" + textBoxCriteria.Text + "%'";

                    if (checkBoxHasAiName.Checked)
                        query += " AND AIName='SmartAI'";

                    query += " ORDER BY entry";
                    break;
                case 1: //! Creature entry
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
                case 2: //! Creature guid
                    if (criteriaLeftEmpty)
                    {
                        if (MessageBox.Show("Are you sure you wish to continue? This query will take a long time to execute because the criteria field was left empty!", "Are you sure you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
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
                case 3: //! Gameobject name
                    query = "SELECT entry, name FROM gameobject_template WHERE name LIKE '%" + textBoxCriteria.Text + "%'";

                    if (checkBoxHasAiName.Checked)
                        query += " AND AIName='SmartGameObjectAI'";

                    query += " ORDER BY entry";
                    break;
                case 4: //! Gameobject entry
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
                case 5: //! Gameobject guid
                    if (criteriaLeftEmpty)
                    {
                        if (MessageBox.Show("Are you sure you wish to continue? This query will take a long time to execute because the criteria field was left empty!", "Are you sure you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
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
                default:
                    MessageBox.Show("An unknown index was found in the search type box!", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

            ClearItemsOfListView(listViewEntryResults);

            try
            {
                SelectFromCreatureTemplate(query, true);
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

        private void buttonClearSearchResults_Click(object sender, EventArgs e)
        {
            TryToStopRunningThread();
        }

        private void TryToStopRunningThread()
        {
            try
            {
                if (searchThread != null && searchThread.IsAlive)
                {
                    searchThread.Abort();
                    searchThread = null;
                }
            }
            catch { } //! No need to report anything
        }

        private void textBoxCriteria_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxCriteria.Text) || String.IsNullOrWhiteSpace(textBoxCriteria.Text))
                return;

            switch (comboBoxSearchType.SelectedIndex)
            {
                case 2: //! Creature guid
                case 5: //! Gameobject guid
                case 1: //! Creature entry
                case 4: //! Gameobject entry
                    if (!Char.IsNumber(e.KeyChar))
                        e.Handled = e.KeyChar != (Char)Keys.Back && e.KeyChar != (Char)Keys.OemMinus;
                    break;
                case 0: //! Creature name
                case 3: //! Gameobject name
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
            ((MainForm)Owner).comboBoxSourceType.SelectedIndex = comboBoxSearchType.SelectedIndex > 2 ? 1 : 0;

            if (Settings.Default.LoadScriptInstantly)
                ((MainForm)Owner).pictureBox1_Click(sender, e);

            Close();
        }

        private bool IsNumericIndex(int index)
        {
            switch (index)
            {
                case 0: //! Creature name
                case 3: //! Gameobject name:
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
            else
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
    }
}
