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

namespace SAI_Editor.Forms
{
    public partial class SqlOutputForm : Form
    {
        private List<SmartScript> smartScripts = null;
        private EntryOrGuidAndSourceType originalEntryOrGuidAndSourceType = new EntryOrGuidAndSourceType();

        public SqlOutputForm(List<SmartScript> smartScripts)
        {
            InitializeComponent();

            this.smartScripts = smartScripts;
        }

        public void SqlOutputForm_Load(object sender, EventArgs e)
        {
            this.originalEntryOrGuidAndSourceType = ((MainForm)Owner).originalEntryOrGuidAndSourceType;
            richTextBoxSqlOutput.Anchor = AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left;
            buttonExecuteScript.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            buttonSaveToFile.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            ExportSqlToTextbox();
        }

        private async void ExportSqlToTextbox()
        {
            if (smartScripts == null || smartScripts.Count == 0)
                return;

            List<EntryOrGuidAndSourceType> entriesOrGuidsAndSourceTypes = new List<EntryOrGuidAndSourceType>();

            foreach (SmartScript smartScript in smartScripts)
            {
                EntryOrGuidAndSourceType entryOrGuidAndSourceType = new EntryOrGuidAndSourceType();
                entryOrGuidAndSourceType.entryOrGuid = smartScript.entryorguid;
                entryOrGuidAndSourceType.sourceType = (SourceTypes)smartScript.source_type;

                if (entriesOrGuidsAndSourceTypes.Contains(entryOrGuidAndSourceType))
                    continue;

                entriesOrGuidsAndSourceTypes.Add(entryOrGuidAndSourceType);
            }

            string sourceName = await SAI_Editor_Manager.Instance.worldDatabase.GetCreatureNameByIdOrGuid(XConverter.TryParseStringToInt32(originalEntryOrGuidAndSourceType.entryOrGuid));
            string sourceSet = originalEntryOrGuidAndSourceType.entryOrGuid < 0 ? "@GUID" : "@ENTRY";

            richTextBoxSqlOutput.Text += "-- " + sourceName + " SAI\n";
            richTextBoxSqlOutput.Text += "SET " + sourceSet + " := " + originalEntryOrGuidAndSourceType.entryOrGuid + ";\n";

            if (entriesOrGuidsAndSourceTypes.Count == 1)
            {
                switch ((SourceTypes)originalEntryOrGuidAndSourceType.sourceType)
                {
                    case SourceTypes.SourceTypeCreature:
                        richTextBoxSqlOutput.Text += "UPDATE `creature_template` SET `AIName`=" + '"' + "SmartAI" + '"' + " WHERE `entry`=" + sourceSet + ";\n";
                        break;
                    case SourceTypes.SourceTypeGameobject:
                        richTextBoxSqlOutput.Text += "UPDATE `gameobject_template` SET `AIName`=" + '"' + "SmartGameObjectAI" + '"' + " WHERE `entry`=" + sourceSet + ";\n";
                        break;
                    case SourceTypes.SourceTypeAreaTrigger:
                        richTextBoxSqlOutput.Text += "DELETE FROM `areatrigger_scripts` WHERE `entry`=" + sourceSet + ";\n";
                        richTextBoxSqlOutput.Text += "INSERT INTO areatrigger_scripts VALUES (" + sourceSet + "," + '"' + "SmartTrigger" + '"' + ");\n";
                        break;
                    case SourceTypes.SourceTypeScriptedActionlist:
                        // todo
                        break;
                }

                richTextBoxSqlOutput.Text += "DELETE FROM `smart_scripts` WHERE `entryorguid`=" + sourceSet + " AND `source_type`=" + originalEntryOrGuidAndSourceType.sourceType + ";\n";
            }
            else
            {
                foreach (EntryOrGuidAndSourceType entryOrGuidAndSourceType in entriesOrGuidsAndSourceTypes)
                {
                    switch ((SourceTypes)entryOrGuidAndSourceType.sourceType)
                    {
                        case SourceTypes.SourceTypeCreature:
                            richTextBoxSqlOutput.Text += "UPDATE `creature_template` SET `AIName`=" + '"' + "SmartAI" + '"' + " WHERE `entry`=" + sourceSet + ";\n";
                            break;
                        case SourceTypes.SourceTypeGameobject:
                            richTextBoxSqlOutput.Text += "UPDATE `gameobject_template` SET `AIName`=" + '"' + "SmartGameObjectAI" + '"' + " WHERE `entry`=" + sourceSet + ";\n";
                            break;
                        case SourceTypes.SourceTypeAreaTrigger:
                            richTextBoxSqlOutput.Text += "DELETE FROM `areatrigger_scripts` WHERE `entry`=" + sourceSet + ";\n";
                            richTextBoxSqlOutput.Text += "INSERT INTO areatrigger_scripts VALUES (" + sourceSet + "," + '"' + "SmartTrigger" + '"' + ");\n";
                            break;
                        case SourceTypes.SourceTypeScriptedActionlist:
                            // todo
                            break;
                    }
                }

                richTextBoxSqlOutput.Text += "DELETE FROM `smart_scripts` WHERE `entryorguid` IN (";

                for (int i = 0; i < entriesOrGuidsAndSourceTypes.Count; ++i)
                {
                    if (i == entriesOrGuidsAndSourceTypes.Count - 1)
                        richTextBoxSqlOutput.Text += entriesOrGuidsAndSourceTypes[i].entryOrGuid + ")";
                    else
                        richTextBoxSqlOutput.Text += entriesOrGuidsAndSourceTypes[i].entryOrGuid + ",";
                }

                richTextBoxSqlOutput.Text += " AND `source_type` IN (";

                for (int i = 0; i < entriesOrGuidsAndSourceTypes.Count; ++i)
                {
                    if (i == entriesOrGuidsAndSourceTypes.Count - 1)
                        richTextBoxSqlOutput.Text += entriesOrGuidsAndSourceTypes[i].sourceType + ")";
                    else
                        richTextBoxSqlOutput.Text += entriesOrGuidsAndSourceTypes[i].sourceType + ",";
                }

                richTextBoxSqlOutput.Text += ";\n";
            }

            richTextBoxSqlOutput.Text += "INSERT INTO `smart_scripts` (`entryorguid`,`source_type`,`id`,`link`,`event_type`,`event_phase_mask`,`event_chance`,`event_flags`,`event_param1`,`event_param2`,`event_param3`,`event_param4`,`action_type`,`action_param1`,`action_param2`,`action_param3`,`action_param4`,`action_param5`,`action_param6`,`target_type`,`target_param1`,`target_param2`,`target_param3`,`target_x`,`target_y`,`target_z`,`target_o`,`comment`) VALUES\n";

            for (int i = 0; i < smartScripts.Count; ++i)
            {
                SmartScript smartScript = smartScripts[i];
                string actualSourceSet = sourceSet;

                if (originalEntryOrGuidAndSourceType.entryOrGuid != smartScripts[i].entryorguid)
                    actualSourceSet = smartScripts[i].entryorguid.ToString();

                richTextBoxSqlOutput.Text += "(" + actualSourceSet + "," + smartScript.source_type + "," + smartScript.id + "," + smartScript.link + "," + smartScript.event_type + "," +
                                              smartScript.event_phase_mask + "," + smartScript.event_chance + "," + smartScript.event_flags + "," + smartScript.event_param1 + "," +
                                              smartScript.event_param2 + "," + smartScript.event_param3 + "," + smartScript.event_param4 + "," + smartScript.action_type + "," +
                                              smartScript.action_param1 + "," + smartScript.action_param2 + "," + smartScript.action_param3 + "," + smartScript.action_param4 + "," +
                                              smartScript.action_param5 + "," + smartScript.action_param6 + "," + smartScript.target_type + "," + smartScript.target_param1 + "," +
                                              smartScript.target_param2 + "," + smartScript.target_param3 + "," + smartScript.target_x + "," + smartScript.target_y + "," +
                                              smartScript.target_z + "," + smartScript.target_o + "," + '"' + smartScript.comment + '"' + ")";

                if (i == smartScripts.Count - 1)
                    richTextBoxSqlOutput.Text += ";";
                else
                    richTextBoxSqlOutput.Text += ",";

                richTextBoxSqlOutput.Text += "\n"; //! White line at end of script to make it easier to select
            }
        }

        private async void buttonExecuteScript_Click(object sender, EventArgs e)
        {
            if (await SAI_Editor_Manager.Instance.worldDatabase.ExecuteNonQuery(richTextBoxSqlOutput.Text))
                MessageBox.Show("The query has been executed succesfully!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonSaveToFile_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "SQL files (*.sql)|*.sql|All files (*.*)|*.*";
            saveFileDialog.ShowDialog(this);
        }

        private void saveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            File.WriteAllText(saveFileDialog.FileName, richTextBoxSqlOutput.Text);
        }
    }
}
