using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace SAI_Editor
{
    public partial class SearchForCreatureForm : Form
    {
        MySqlConnectionStringBuilder connectionString = null;

        public SearchForCreatureForm(MySqlConnectionStringBuilder connectionString)
        {
            InitializeComponent();
            this.connectionString = connectionString;
        }

        private void SearchForCreatureForm_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
            MinimizeBox = true;
            MinimumSize = new Size(Width, Height);
            MaximumSize = new Size(Width, Height + 800);

            listViewCreatureResults.View = View.Details;
            listViewCreatureResults.Columns.Add("Entry", 60, HorizontalAlignment.Right);
            listViewCreatureResults.Columns.Add("Name", 275, HorizontalAlignment.Left);

            listViewCreatureResults.FullRowSelect = true; //! This will make clicking on a row in the results select the full row.

            listViewCreatureResults.DoubleClick += listViewCreatureResults_DoubleClick;
            listViewCreatureResults.MultiSelect = false;

            //listViewCreatureResults.ListViewItemSorter = lvwColumnSorter;
            //listViewCreatureResults.ColumnClick += new ColumnClickEventHandler(listViewCreatureResults_ColumnClick);

            listViewCreatureResults.Anchor = AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left;
            SelectFromCreatureTemplate("SELECT entry, name FROM creature_template LIMIT 1000");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSearchForCreatureEntry.Checked)
            {
                if (Regex.IsMatch(textBoxCreatureCriteria.Text, @"^[a-zA-Z]+$"))
                {
                    if (MessageBox.Show("The criteria contains characters. Do you wish to clean the field? If you choose 'No', the checkbox will be unchecked again.", "Something went wrong!", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                        textBoxCreatureCriteria.Text = "";
                    else
                    {
                        checkBoxSearchForCreatureEntry.Checked = false;
                        return;
                    }
                }
            }
            //else //! No need to check for numbers because some creature names have numbers in there

            if (checkBoxSearchForCreatureEntry.Checked)
                labelCreatureSearchInfo.Text = "Creature entry (part)";
            else
                labelCreatureSearchInfo.Text = "Creature name (part)";
        }

        private void listViewCreatureResults_DoubleClick(object sender, EventArgs e)
        {
            string selectedEntry = listViewCreatureResults.SelectedItems[0].Text;
        }

        private void SelectFromCreatureTemplate(string queryToExecute)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString.ToString()))
                {
                    connection.Open();

                    using (var query = new MySqlCommand(queryToExecute, connection))
                        using (var reader = query.ExecuteReader())
                            while (reader != null && reader.Read())
                                listViewCreatureResults.Items.Add(reader.GetInt32(0).ToString()).SubItems.Add(reader.GetString(1));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void buttonSearchCreature_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxCreatureCriteria.Text) || String.IsNullOrWhiteSpace(textBoxCreatureCriteria.Text))
                return;

            listViewCreatureResults.Clear();
            SelectFromCreatureTemplate(String.Format("SELECT entry, name FROM creature_template WHERE {0} LIKE '%{1}%'", (checkBoxSearchForCreatureEntry.Checked ? "entry" : "name"), textBoxCreatureCriteria.Text));
        }
    }
}
