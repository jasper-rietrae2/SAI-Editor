using System;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SAI_Editor
{
    public partial class SearchForEntryForm : Form
    {
        private readonly MySqlConnectionStringBuilder connectionString;
        private readonly bool searchingForCreature = false;
        private int previouslySelectedSearchIndex = 0;

        public SearchForEntryForm(MySqlConnectionStringBuilder connectionString, bool searchingForCreature)
        {
            InitializeComponent();

            this.connectionString = connectionString;
            this.searchingForCreature = searchingForCreature;
        }

        private void SearchForCreatureForm_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
            MinimizeBox = true;
            MinimumSize = new Size(Width, Height);
            MaximumSize = new Size(Width, Height + 800);

            KeyPreview = true;
            KeyDown += SearchForEntryForm_KeyDown;

            comboBoxSearchType.SelectedIndex = 0;
            comboBoxSearchType.KeyPress += comboBoxSearchType_KeyPress;

            textBoxCriteria.KeyPress += textBoxCriteria_KeyPress;

            listViewEntryResults.View = View.Details;
            listViewEntryResults.Columns.Add("Entry/guid", 70, HorizontalAlignment.Right);
            listViewEntryResults.Columns.Add("Name", 263, HorizontalAlignment.Left);

            listViewEntryResults.FullRowSelect = true; //! This will make clicking on a row in the results select the full row.

            listViewEntryResults.DoubleClick += listViewEntryResults_DoubleClick;
            listViewEntryResults.MultiSelect = false;

            //listViewCreatureResults.ListViewItemSorter = lvwColumnSorter;
            //listViewCreatureResults.ColumnClick += new ColumnClickEventHandler(listViewCreatureResults_ColumnClick);

            listViewEntryResults.Anchor = AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left;
            SelectFromCreatureTemplate(String.Format("SELECT entry, name FROM {0} ORDER BY entry LIMIT 1000", (searchingForCreature ? "creature_template" : "gameobject_template")));
        }

        private void listViewEntryResults_DoubleClick(object sender, EventArgs e)
        {
            FillMainFormEntryOrGuidField(sender, e);
        }

        private void SelectFromCreatureTemplate(string queryToExecute)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString.ToString()))
                {
                    connection.Open();

                    using (var query = new MySqlCommand(queryToExecute, connection))
                        using (MySqlDataReader reader = query.ExecuteReader())
                            while (reader != null && reader.Read())
                                listViewEntryResults.Items.Add(reader.GetInt32(0).ToString(CultureInfo.InvariantCulture)).SubItems.Add(reader.GetString(1));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            string query = "";
            bool emptyString = String.IsNullOrEmpty(textBoxCriteria.Text) || String.IsNullOrWhiteSpace(textBoxCriteria.Text);

            switch (comboBoxSearchType.SelectedIndex)
            {
                case 0: //! Creature name
                    query = "SELECT entry, name FROM creature_template WHERE name LIKE '%" + textBoxCriteria.Text + "%'";

                    if (checkBoxHasAiName.Checked)
                        query += " AND AIName='SmartAI'";
                    break;
                case 1: //! Creature entry
                    query = "SELECT entry, name FROM creature_template";

                    if (!emptyString)
                        query += " WHERE entry=" + textBoxCriteria.Text;

                    if (checkBoxHasAiName.Checked)
                        query += (emptyString ? " WHERE" : " AND") + " AIName='SmartAI'";

                    break;
                case 2: //! Creature guid
                    query = "SELECT c.guid, ct.name FROM creature_template ct LEFT JOIN creature c ON ct.entry = c.id WHERE c.guid=" + textBoxCriteria.Text;

                    if (checkBoxHasAiName.Checked)
                        query += " AND ct.AIName='SmartAI'";

                    query += " ORDER BY c.guid";
                    break;
                case 3: //! Gameobject name
                    query = "SELECT entry, name FROM gameobject_template WHERE name LIKE '%" + textBoxCriteria.Text + "%'";

                    if (checkBoxHasAiName.Checked)
                        query += " AND AIName='SmartGameObjectAI'";
                    break;
                case 4: //! Gameobject entry
                    query = "SELECT entry, name FROM gameobject_template";

                    if (!emptyString)
                        query += " WHERE entry=" + textBoxCriteria.Text;

                    if (checkBoxHasAiName.Checked)
                        query += (emptyString ? " WHERE" : " AND") + " AIName='SmartGameObjectAI'";
                    break;
                case 5: //! Gameobject guid
                    query = "SELECT g.guid, gt.name FROM gameobject_template gt LEFT JOIN gameobject g ON gt.entry = g.id WHERE g.guid=" + textBoxCriteria.Text;

                    if (checkBoxHasAiName.Checked)
                        query += " AND gt.AIName='SmartGameObjectAI'";

                    query += " ORDER BY g.guid";
                    break;
                default:
                    break;
            }

            //! If we're searching for GUIDs we already added this above
            if (!(comboBoxSearchType.SelectedIndex == 2 || comboBoxSearchType.SelectedIndex == 5))
                query += " ORDER BY entry";

            listViewEntryResults.Items.Clear();
            SelectFromCreatureTemplate(query);
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

        private void comboBoxSearchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxSearchType.SelectedIndex)
            {
                case 2: //! Creature guid
                case 5: //! Gameobject guid
                case 1: //! Creature entry
                case 4: //! Gameobject entry
                    textBoxCriteria.Text = "";
                    break;
                case 0: //! Creature name
                case 3: //! Gameobject name
                    //! Do nothing
                    break;
                default:
                    break;
            }

            previouslySelectedSearchIndex = comboBoxSearchType.SelectedIndex;
        }

        private void buttonClearSearchResults_Click(object sender, EventArgs e)
        {
            listViewEntryResults.Items.Clear();
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
                    if (!char.IsNumber(e.KeyChar))
                        e.Handled = e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.OemMinus;
                    break;
                case 0: //! Creature name
                case 3: //! Gameobject name
                    //! Allow any characters when searching for names
                    break;
                default:
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

            if (((MainForm)Owner).settings.GetSetting("LoadScriptInstantly", "no") == "yes")
                ((MainForm)Owner).buttonLoadScriptForEntry_Click(sender, e);

            Close();
        }
    }
}
