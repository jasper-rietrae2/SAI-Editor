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
    public partial class SearchForSpellForm : Form
    {
        private Thread searchThread = null;
        private readonly MySqlConnectionStringBuilder connectionString;
        private readonly ListViewColumnSorter lvwColumnSorter = new ListViewColumnSorter();
        private readonly TextBox textBoxToChange = null;

        public SearchForSpellForm(MySqlConnectionStringBuilder connectionString, TextBox textBoxToChange)
        {
            InitializeComponent();

            this.connectionString = connectionString;
            this.textBoxToChange = textBoxToChange;
        }

        private void SearchForSpellForm_Load(object sender, EventArgs e)
        {
            MinimumSize = new Size(Width, Height);
            MaximumSize = new Size(Width, Height + 800);
            listViewEntryResults.Anchor = AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left;
            comboBoxSearchType.SelectedIndex = 0;
            FillListViewWithSpells(true);
        }

        private void listViewEntryResults_DoubleClick(object sender, EventArgs e)
        {
            StopRunningThread();
            textBoxToChange.Text = listViewEntryResults.SelectedItems[0].Text;
            Close();
        }

        private async void FillListViewWithSpells(bool limit = false)
        {
            try
            {
                string queryToExecute = "SELECT id, spellName FROM spells";

                if (limit)
                    queryToExecute += " LIMIT 1000"; //? Looks like LIMIT doesn't work for SQLite?
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

                DataTable dt = await SAI_Editor_Manager.Instance.sqliteDatabase.ExecuteQuery(queryToExecute);

                if (dt.Rows.Count > 0)
                    foreach (DataRow row in dt.Rows)
                        AddItemToListView(listViewEntryResults, Convert.ToInt32(row["id"]).ToString(), (string)row["spellName"]);
            }
            catch (ObjectDisposedException ex)
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
                    FillListViewWithSpells();
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
                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SearchForSpellForm_KeyDown(object sender, KeyEventArgs e)
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

        private void SearchForSpellForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopRunningThread();
        }
    }
}
