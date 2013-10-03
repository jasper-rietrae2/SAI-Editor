using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAI_Editor.Forms
{
    public partial class RevertQueryForm : Form
    {
        public RevertQueryForm()
        {
            InitializeComponent();
        }

        private void RevertQueryForm_Load(object sender, EventArgs e)
        {
            calenderScriptsToRevert.TodayDate = DateTime.Now;
            FillListViewWithDate(DateTime.Now);
        }

        private void calenderScriptsToRevert_DateChanged(object sender, DateRangeEventArgs e)
        {
            FillListViewWithDate(calenderScriptsToRevert.TodayDate);
        }

        private void FillListViewWithDate(DateTime dateTime)
        {
            listViewScripts.Items.Clear();
            string[] allFiles = Directory.GetFiles("Reverts");

            for (int i = 0; i < allFiles.Length; ++i)
            {
                if (File.GetCreationTime(allFiles[i]).CompareTo(calenderScriptsToRevert.SelectionStart) < 0 && File.GetCreationTime(allFiles[i]).CompareTo(calenderScriptsToRevert.SelectionEnd) < 0)
                    continue;

                listViewScripts.Items.Add(allFiles[i].Replace(@"Reverts\", "").Replace(".sql", ""));
            }
        }

        private async void buttonExecuteSelectedScript_Click(object sender, EventArgs e)
        {
            if (listViewScripts.SelectedItems.Count == 0)
                return;

            string fileName = "Reverts\\";
            fileName += listViewScripts.SelectedItems[0].Text + ".sql";

            if (await SAI_Editor_Manager.Instance.worldDatabase.ExecuteNonQuery(File.ReadAllText(fileName)))
                MessageBox.Show("The query has been executed succesfully!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void listViewScripts_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonExecuteSelectedScript.Enabled = listViewScripts.SelectedItems.Count > 0;
        }
    }
}
