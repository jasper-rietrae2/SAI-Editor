using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using SAI_Editor.Properties;
using SAI_Editor.Classes;

namespace SAI_Editor.Forms
{
    public partial class SqlOutputForm : Form
    {
        private EntryOrGuidAndSourceType originalEntryOrGuidAndSourceType = new EntryOrGuidAndSourceType();
        private readonly string revertQuery, originalSqlOutput;
        private readonly List<EntryOrGuidAndSourceType> entriesOrGuidsAndSourceTypes;
        private readonly bool sqlForSmartScripts, saveToFile;

        public SqlOutputForm(string sqlOutput, bool sqlForSmartScripts, string revertQuery = "", List<EntryOrGuidAndSourceType> entriesOrGuidsAndSourceTypes = null, bool saveToFile = true)
        {
            InitializeComponent();

            richTextBoxSqlOutput.Text = sqlOutput;
            originalSqlOutput = richTextBoxSqlOutput.Text; //! We have to assign it to the .Text instead of `sqlOutput` as it adds some linefeeds here
            this.revertQuery = revertQuery;
            this.entriesOrGuidsAndSourceTypes = entriesOrGuidsAndSourceTypes;
            this.sqlForSmartScripts = sqlForSmartScripts;
            this.saveToFile = saveToFile;
        }

        private void SqlOutputForm_Load(object sender, EventArgs e)
        {
            if (sqlForSmartScripts)
                originalEntryOrGuidAndSourceType = ((MainForm)Owner).originalEntryOrGuidAndSourceType;

            buttonSaveToFile.Enabled = saveToFile;
            richTextBoxSqlOutput.Anchor = AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left;
            buttonExecuteScript.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            buttonSaveToFile.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            buttonExecuteScript.Enabled = Settings.Default.UseWorldDatabase;
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
                string message = "The SQL has been executed succesfully!";

                if (Settings.Default.CreateRevertQuery && sqlForSmartScripts)
                {
                    if (query != originalSqlOutput)
                    {
                        DialogResult dialogResult = MessageBox.Show("Changes have been made to the SQL. Do you still wish you generate a revert query to be able to reset the original SAI (for the entryorguid and source_type that opened this form) to its current state?", "Changes have been made...", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (dialogResult != DialogResult.Yes)
                            goto SkipRevertQueryGenerating;
                    }

                    CreateRevertQuery();
                    message += "\n\nA revert query has also been generated to reset the script back to its previous (current) state. To view all revert queries, open the 'Revert Query' form from the 'File' menu option.";
                }

            SkipRevertQueryGenerating:
                MessageBox.Show(message, "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void buttonSaveToFile_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "SQL files (*.sql)|*.sql|All files (*.*)|*.*";

            switch (originalEntryOrGuidAndSourceType.sourceType)
            {
                case SourceTypes.SourceTypeCreature:
                case SourceTypes.SourceTypeGameobject:
                case SourceTypes.SourceTypeAreaTrigger:
                case SourceTypes.SourceTypeScriptedActionlist:
                    if (sqlForSmartScripts)
                    {
                        saveFileDialog.FileName = "SAI for " + SAI_Editor_Manager.Instance.GetSourceTypeString(originalEntryOrGuidAndSourceType.sourceType).ToLower() + " ";

                        if (Settings.Default.UseWorldDatabase)
                            saveFileDialog.FileName += await SAI_Editor_Manager.Instance.worldDatabase.GetObjectNameByIdOrGuidAndSourceType(originalEntryOrGuidAndSourceType);
                        else
                            saveFileDialog.FileName += originalEntryOrGuidAndSourceType.entryOrGuid;
                    }
                    else
                        saveFileDialog.FileName = "Condition SQL";

                    break;
            }

            saveFileDialog.ShowDialog(this);
        }

        private void saveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                File.WriteAllText(saveFileDialog.FileName, richTextBoxSqlOutput.Text);
            }
            catch
            {
                MessageBox.Show("The file could not be saved.", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult dialogResult = MessageBox.Show("The file has been saved succesfully! Do you want to open it?", "Success!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
                SAI_Editor_Manager.Instance.StartProcess(saveFileDialog.FileName);
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
            if (!sqlForSmartScripts)
                return;

            //! Example filename:
            //! [Creature] [33303] 3-10-2013 15.32.40.sql
            //! [Creature] [33303 - 3330300] 3-10-2013 15.32.40.sql
            string filename = @"Reverts\[" + SAI_Editor_Manager.Instance.GetSourceTypeString(originalEntryOrGuidAndSourceType.sourceType) + "] [";

            if (entriesOrGuidsAndSourceTypes != null)
            {
                for (int i = 0; i < entriesOrGuidsAndSourceTypes.Count; ++i)
                {
                    filename += entriesOrGuidsAndSourceTypes[i].entryOrGuid;

                    if (i < entriesOrGuidsAndSourceTypes.Count - 1)
                        filename += " - ";
                }
            }
            else
                filename += originalEntryOrGuidAndSourceType.entryOrGuid;

            //! GetUniversalTimeStamp will return something like '27-12-2013 19;55;22'
            filename += "] " + SAI_Editor_Manager.Instance.GetUniversalTimeStamp() + ".sql";

            if (!Directory.Exists("Reverts"))
                Directory.CreateDirectory("Reverts");

            try
            {
                File.WriteAllText(filename, revertQuery);
            }
            catch
            {
                MessageBox.Show("The revert query could not be generated.", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
