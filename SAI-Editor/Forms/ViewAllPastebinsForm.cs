using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SAI_Editor.Classes;
using SAI_Editor.Properties;

namespace SAI_Editor.Forms
{
    public partial class ViewAllPastebinsForm : Form
    {
        public ViewAllPastebinsForm()
        {
            InitializeComponent();

            FillListViewFromSettings();
        }

        private void ViewAllPastebinsForm_Load(object sender, EventArgs e)
        {

        }

        private void FillListViewFromSettings()
        {
            listViewPastebins.Items.Clear();
            string[] allPastebinsSplit = Settings.Default.PastebinLinksStore.Split(';');

            foreach (string pastebinSplit in allPastebinsSplit)
            {
                if (String.IsNullOrWhiteSpace(pastebinSplit))
                    continue;

                string[] split = pastebinSplit.Split('|');
                string pasteUrl = split[0];
                string pasteName = split[1];

                listViewPastebins.Items.Add(pasteName).SubItems.Add(pasteUrl);
            }
        }

        private void ViewAllPastebinsForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Close();
                    break;
            }
        }

        private void listViewPastebins_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonVisitSelectedPastebin.Enabled = listViewPastebins.SelectedItems.Count > 0;
        }

        private void buttonVisitSelectedPastebin_Click(object sender, EventArgs e)
        {
            VisitSelectedPastebin();
        }

        private void VisitSelectedPastebin()
        {
            if (listViewPastebins.SelectedItems.Count == 0)
                return;

            SAI_Editor_Manager.Instance.StartProcess(listViewPastebins.SelectedItems[0].SubItems[1].Text);
        }

        private void listViewPastebins_DoubleClick(object sender, EventArgs e)
        {
            VisitSelectedPastebin();
        }
    }
}
