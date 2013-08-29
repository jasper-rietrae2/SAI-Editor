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
        private readonly bool searchingForCreature;

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

            listViewEntryResults.View = View.Details;
            listViewEntryResults.Columns.Add("Entry", 60, HorizontalAlignment.Right);
            listViewEntryResults.Columns.Add("Name", 275, HorizontalAlignment.Left);

            listViewEntryResults.FullRowSelect = true; //! This will make clicking on a row in the results select the full row.

            listViewEntryResults.DoubleClick += listViewEntryResults_DoubleClick;
            listViewEntryResults.MultiSelect = false;

            //listViewCreatureResults.ListViewItemSorter = lvwColumnSorter;
            //listViewCreatureResults.ColumnClick += new ColumnClickEventHandler(listViewCreatureResults_ColumnClick);

            listViewEntryResults.Anchor = AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left;
            SelectFromCreatureTemplate(String.Format("SELECT entry, name FROM {0} ORDER BY entry LIMIT 1000", (searchingForCreature ? "creature_template" : "gameobject_template")));
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSearchForEntry.Checked)
            {
                if (Regex.IsMatch(textBoxEntryCriteria.Text, @"^[a-zA-Z]+$"))
                {
                    if (MessageBox.Show("The criteria contains characters. Do you wish to clean the field? If you choose 'No', the checkbox will be unchecked again.", "Something went wrong!", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                        textBoxEntryCriteria.Text = "";
                    else
                    {
                        checkBoxSearchForEntry.Checked = false;
                        return;
                    }
                }
            }
            //else //! No need to check for numbers because some creature names have numbers in there

            labelEntrySearchInfo.Text = String.Format("{0}", (searchingForCreature ? "Creature" : "Gameobject"));

            if (checkBoxSearchForEntry.Checked)
                labelEntrySearchInfo.Text += " entry (part)";
            else
                labelEntrySearchInfo.Text += " name (part)";
        }

        private void listViewEntryResults_DoubleClick(object sender, EventArgs e)
        {
            ((MainForm)Owner).textBox1.Text = listViewEntryResults.SelectedItems[0].Text;
            Close();
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
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonSearchCreature_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxEntryCriteria.Text) || String.IsNullOrWhiteSpace(textBoxEntryCriteria.Text))
                return;

            listViewEntryResults.Items.Clear();
            SelectFromCreatureTemplate(String.Format("SELECT entry, name FROM {0} WHERE {1} LIKE '%{2}%' ORDER BY entry", (searchingForCreature ? "creature_template" : "gameobject_template"), (checkBoxSearchForEntry.Checked ? "entry" : "name"), textBoxEntryCriteria.Text));
        }

        private void SearchForEntryForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                {
                    if (listViewEntryResults.SelectedItems.Count > 0)
                    {
                        ((MainForm)Owner).textBox1.Text = listViewEntryResults.SelectedItems[0].Text;
                        Close();
                    }
                    else
                        buttonSearchCreature_Click(sender, e);

                    break;
                }
            }
        }
    }
}
