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
    };

    public partial class SearchFromDatabaseForm : Form
    {
        private Thread searchThread = null;
        private readonly MySqlConnectionStringBuilder connectionString;
        private readonly ListViewColumnSorter lvwColumnSorter = new ListViewColumnSorter();
        private readonly TextBox textBoxToChange = null;
        private readonly DatabaseSearchFormType databaseSearchFormType;

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
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeFaction:
                    Text = "Search for a faction";
                    listViewEntryResults.Columns.Add("Id", 45);
                    listViewEntryResults.Columns.Add("Name", 284);
                    comboBoxSearchType.Items.Add("Faction id");
                    comboBoxSearchType.Items.Add("Faction name");
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeEmote:
                    Text = "Search for an emote";
                    listViewEntryResults.Columns.Add("Id", 45);
                    listViewEntryResults.Columns.Add("Name", 284);
                    comboBoxSearchType.Items.Add("Emote id");
                    comboBoxSearchType.Items.Add("Emote name");
                    break;
                case DatabaseSearchFormType.DatabaseSearchFormTypeQuest:
                    Text = "Search for a quest";
                    listViewEntryResults.Columns.Add("Id", 45);
                    listViewEntryResults.Columns.Add("Name", 284);
                    comboBoxSearchType.Items.Add("Quest id");
                    comboBoxSearchType.Items.Add("Quest name");
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
                string queryToExecute = String.Empty;
                DataTable dt = null;

                switch (databaseSearchFormType)
                {
                    case DatabaseSearchFormType.DatabaseSearchFormTypeSpell:
                        queryToExecute = "SELECT id, spellName FROM spells";

                        if (limit)
                            queryToExecute += " LIMIT 1000";
                        else
                        {
                            switch (GetSelectedIndexOfComboBox(comboBoxSearchType))
                            {
                                case 0: //! Spell entry
                                    if (checkBoxFieldContainsCriteria.Checked)
                                        queryToExecute += " WHERE id LIKE '%" + textBoxCriteria.Text + "%'";
                                    else
                                        queryToExecute += " WHERE id = " + textBoxCriteria.Text;

                                    break;
                                case 1: //! Spell name
                                    if (checkBoxFieldContainsCriteria.Checked)
                                        queryToExecute += " WHERE spellName LIKE '%" + textBoxCriteria.Text + "%'";
                                    else
                                        queryToExecute += " WHERE spellName = " + textBoxCriteria.Text;

                                    break;
                                default:
                                    return;
                            }
                        }

                        dt = await SAI_Editor_Manager.Instance.sqliteDatabase.ExecuteQuery(queryToExecute);

                        if (dt.Rows.Count > 0)
                            foreach (DataRow row in dt.Rows)
                                AddItemToListView(listViewEntryResults, Convert.ToInt32(row["id"]).ToString(), (string)row["spellName"]);
                        break;
                    case DatabaseSearchFormType.DatabaseSearchFormTypeFaction:
                        queryToExecute = "SELECT m_ID, m_name_lang_1 FROM factions";

                        if (limit)
                            queryToExecute += " LIMIT 1000";
                        else
                        {
                            switch (GetSelectedIndexOfComboBox(comboBoxSearchType))
                            {
                                case 0: //! Faction entry
                                    if (checkBoxFieldContainsCriteria.Checked)
                                        queryToExecute += " WHERE m_ID LIKE '%" + textBoxCriteria.Text + "%'";
                                    else
                                        queryToExecute += " WHERE m_ID = " + textBoxCriteria.Text;

                                    break;
                                case 1: //! Faction name
                                    if (checkBoxFieldContainsCriteria.Checked)
                                        queryToExecute += " WHERE m_name_lang_1 LIKE '%" + textBoxCriteria.Text + "%'";
                                    else
                                        queryToExecute += " WHERE m_name_lang_1 = " + textBoxCriteria.Text;

                                    break;
                                default:
                                    return;
                            }
                        }

                        dt = await SAI_Editor_Manager.Instance.sqliteDatabase.ExecuteQuery(queryToExecute);

                        if (dt.Rows.Count > 0)
                            foreach (DataRow row in dt.Rows)
                                AddItemToListView(listViewEntryResults, Convert.ToInt32(row["m_ID"]).ToString(), (string)row["m_name_lang_1"]);
                        break;
                    case DatabaseSearchFormType.DatabaseSearchFormTypeEmote:
                        queryToExecute = "SELECT field0, field1 FROM emotes";

                        if (limit)
                            queryToExecute += " LIMIT 1000";
                        else
                        {
                            switch (GetSelectedIndexOfComboBox(comboBoxSearchType))
                            {
                                case 0: //! Emote entry
                                    if (checkBoxFieldContainsCriteria.Checked)
                                        queryToExecute += " WHERE field0 LIKE '%" + textBoxCriteria.Text + "%'";
                                    else
                                        queryToExecute += " WHERE field0 = " + textBoxCriteria.Text;

                                    break;
                                case 1: //! Emote name
                                    if (checkBoxFieldContainsCriteria.Checked)
                                        queryToExecute += " WHERE field1 LIKE '%" + textBoxCriteria.Text + "%'";
                                    else
                                        queryToExecute += " WHERE field1 = " + textBoxCriteria.Text;

                                    break;
                                default:
                                    return;
                            }
                        }

                        dt = await SAI_Editor_Manager.Instance.sqliteDatabase.ExecuteQuery(queryToExecute);

                        if (dt.Rows.Count > 0)
                            foreach (DataRow row in dt.Rows)
                                AddItemToListView(listViewEntryResults, Convert.ToInt32(row["field0"]).ToString(), (string)row["field1"]);
                        break;
                    case DatabaseSearchFormType.DatabaseSearchFormTypeQuest:
                        queryToExecute = "SELECT id, title FROM quest_template";

                        if (limit)
                            queryToExecute += " LIMIT 1000";
                        else
                        {
                            switch (GetSelectedIndexOfComboBox(comboBoxSearchType))
                            {
                                case 0: //! Quest entry
                                    if (checkBoxFieldContainsCriteria.Checked)
                                        queryToExecute += " WHERE id LIKE '%" + textBoxCriteria.Text + "%'";
                                    else
                                        queryToExecute += " WHERE id = " + textBoxCriteria.Text;

                                    break;
                                case 1: //! Quest name
                                    if (checkBoxFieldContainsCriteria.Checked)
                                        queryToExecute += " WHERE title LIKE '%" + textBoxCriteria.Text + "%'";
                                    else
                                        queryToExecute += " WHERE title = " + textBoxCriteria.Text;

                                    break;
                                default:
                                    return;
                            }
                        }

                        dt = await SAI_Editor_Manager.Instance.worldDatabase.ExecuteQuery(queryToExecute);

                        if (dt.Rows.Count > 0)
                            foreach (DataRow row in dt.Rows)
                                AddItemToListView(listViewEntryResults, Convert.ToInt32(row["id"]).ToString(), (string)row["title"]);
                        break;
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
