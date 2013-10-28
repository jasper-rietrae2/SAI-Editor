using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SAI_Editor.Database.Classes;
using SAI_Editor.Classes;
using System.IO;
using System.Diagnostics;
using SAI_Editor.Properties;
using FastColoredTextBoxNS;

namespace SAI_Editor.Forms
{
    public partial class SqlOutputForm : Form
    {
        private EntryOrGuidAndSourceType originalEntryOrGuidAndSourceType = new EntryOrGuidAndSourceType();
        private readonly string revertQuery, originalSqlOutput;
        private readonly List<EntryOrGuidAndSourceType> entriesOrGuidsAndSourceTypes;

        public SqlOutputForm(string sqlOutput, string revertQuery = "", List<EntryOrGuidAndSourceType> entriesOrGuidsAndSourceTypes = null)
        {
            InitializeComponent();

            richTextBoxSqlOutput.Text = sqlOutput;
            originalSqlOutput = richTextBoxSqlOutput.Text; //! We have to assign it to the .Text instead of `sqlOutput` as it adds some linefeeds here
            this.revertQuery = revertQuery;
            this.entriesOrGuidsAndSourceTypes = entriesOrGuidsAndSourceTypes;
        }

        private void SqlOutputForm_Load(object sender, EventArgs e)
        {
            this.originalEntryOrGuidAndSourceType = ((MainForm)Owner).originalEntryOrGuidAndSourceType;

            richTextBoxSqlOutput.Anchor = AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left;
            buttonExecuteScript.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            buttonSaveToFile.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
        }

        private async void buttonExecuteScript_Click(object sender, EventArgs e)
        {
            string query = richTextBoxSqlOutput.Text;

            if (!String.IsNullOrWhiteSpace(richTextBoxSqlOutput.SelectedText))
            {
                DialogResult dialogResult = MessageBox.Show("Do you only want to execute the selected SQL?", "Selection", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                    query = richTextBoxSqlOutput.SelectedText;
            }

            if (await SAI_Editor_Manager.Instance.worldDatabase.ExecuteNonQuery(query))
            {
                string message = "The query has been executed succesfully!";

                if (Settings.Default.CreateRevertQuery)
                {
                    if (query != originalSqlOutput)
                    {
                        DialogResult dialogResult = MessageBox.Show("Changes have been made to the SQL. Do you still wish you generate a revert query to be able to reset the original SAI (for the entryorguid and source_type that opened this form) to its current state?", "Changes have been made...", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (dialogResult != DialogResult.Yes)
                            return;
                    }

                    CreateRevertQuery();
                    message += "\n\nA revert query has also been generated to reset the script back to its previous (current) state. To view all revert queries, open the 'Revert Query' form from the 'File' menu option.";
                }

                MessageBox.Show(message, "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonSaveToFile_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "SQL files (*.sql)|*.sql|All files (*.*)|*.*";
            saveFileDialog.ShowDialog(this);
        }

        private void saveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                File.WriteAllText(saveFileDialog.FileName, richTextBoxSqlOutput.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult dialogResult = MessageBox.Show("The file has been saved succesfully! Do you want to open it?", "Success!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
                StartProcess(saveFileDialog.FileName);
        }

        private void StartProcess(string filename, string argument = "")
        {
            try
            {
                Process.Start(filename, argument);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(String.Format("The process '{0}' could not be opened!", Path.GetFileName(filename)), "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SqlOutputForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Close();
                    break;
            }
        }

        private void CreateRevertQuery()
        {
            //! Example filename:
            //! [Creature] [33303] 3-10-2013 15.32.40.sql
            //! [Creature] [33303 - 3330300] 3-10-2013 15.32.40.sql
            string filename = @"Reverts\[" + GetSourceTypeString(originalEntryOrGuidAndSourceType.sourceType) + "] [";

            if (entriesOrGuidsAndSourceTypes != null && entriesOrGuidsAndSourceTypes.Count > 1)
            {
                for (int i = 0; i < entriesOrGuidsAndSourceTypes.Count; ++i)
                {
                    filename += entriesOrGuidsAndSourceTypes[i].entryOrGuid;

                    if (i < entriesOrGuidsAndSourceTypes.Count - 1)
                        filename += " - ";
                }
            }
            else
                filename += originalEntryOrGuidAndSourceType.entryOrGuid;;

            filename += "] " + DateTime.Now.ToString() + ".sql";

            if (!Directory.Exists("Reverts"))
                Directory.CreateDirectory("Reverts");

            filename = filename.Replace(":", ";");

            try
            {
                File.WriteAllText(filename, revertQuery);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetSourceTypeString(SourceTypes sourceType)
        {
            switch (sourceType)
            {
                case SourceTypes.SourceTypeCreature:
                    return "Creature";
                case SourceTypes.SourceTypeGameobject:
                    return "Gameobject";
                case SourceTypes.SourceTypeAreaTrigger:
                    return "Areatrigger";
                case SourceTypes.SourceTypeScriptedActionlist:
                    return "Actionlist";
                default:
                    return "Unknown";
            }
        }
    }
}
