using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SAI_Editor
{
    public partial class SelectDatabaseForm : Form
    {
        private readonly List<string> databaseNames = new List<string>();

        public SelectDatabaseForm(List<string> databaseNames)
        {
            InitializeComponent();

            this.databaseNames = databaseNames;
        }

        private void SelectDatabaseForm_Load(object sender, EventArgs e)
        {
            listViewDatabases.Columns.Add("Database", 198, HorizontalAlignment.Left);

            for (int i = 0; i < databaseNames.Count; ++i)
            {
                listViewDatabases.Items.Add(databaseNames[i]);

                //! Select the currently used database (if any)
                if (((MainForm)Owner).textBoxWorldDatabase.Text == databaseNames[i])
                    listViewDatabases.Items[i].Selected = true;
            }
        }

        private void SelectDatabaseForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Close();
                    break;
            }
        }

        private void buttonContinue_Click(object sender, EventArgs e)
        {
            ((MainForm)Owner).textBoxWorldDatabase.Text = listViewDatabases.SelectedItems[0].Text;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void listViewDatabases_DoubleClick(object sender, EventArgs e)
        {
            buttonContinue_Click(sender, e);
        }
    }
}
