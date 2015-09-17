using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using SAI_Editor.Classes;
using SAI_Editor.Classes.Database.Classes;
using SAI_Editor.Enumerators;
using SAI_Editor.Forms.SearchForms;
using SAI_Editor.Properties;
using System.IO;
using System.Reflection;
using System.Net;
using System.Threading;
using System.Runtime.InteropServices;
using SAI_Editor.Classes.CustomControls;
using SAI_Editor.Forms;
using BrightIdeasSoftware;

namespace SAI_Editor
{
    public partial class UserControlSAI : UserControl
    {
        public int lastSmartScriptIdOfScript = 0, previousLinkFrom = -1;
        public EntryOrGuidAndSourceType originalEntryOrGuidAndSourceType = new EntryOrGuidAndSourceType();
        private bool updatingFieldsBasedOnSelectedScript = false;
        public bool expandingListView = false, contractingListView = false;
        public const int expandAndContractSpeedListView = 2;
        private int customObjectListViewHeightToChangeTo;
        private List<SmartScript> lastDeletedSmartScripts = new List<SmartScript>(), smartScriptsOnClipBoard = new List<SmartScript>();
        private string applicationVersion = String.Empty;
        private System.Windows.Forms.Timer timerCheckForInternetConnection = new System.Windows.Forms.Timer();
        private MainForm MainForm;

        public readonly SAIUserControlState DefaultState = new SAIUserControlState();

        public readonly List<SAIUserControlState> States = new List<SAIUserControlState>();

        private SAIUserControlState _currentState;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        public SAIUserControlState CurrentState
        {
            get
            {
                return _currentState;
            }
            set
            {
                if (_currentState != null)
                    _currentState.Save(this);

                if (_currentState == value)
                    return;

                _currentState = value;
                _currentState.Load();
            }
        }

        public SmartScriptList ListViewList
        {
            get
            {
                return (SmartScriptList)customObjectListView.List;
            }
        }

        public CustomObjectListView ListView
        {
            get
            {
                return customObjectListView;
            }
        }

        public UserControlSAI()
        {
            InitializeComponent();
        }

        public void LoadUserControl()
        {
            MainForm = Parent as MainForm;

            customObjectListView.List = new SmartScriptList(customObjectListView);
            customObjectListView.CellRightClick += customObjectListView_CellRightClick;

            comboBoxSourceType.SelectedIndex = 0;
            comboBoxEventType.SelectedIndex = 0;
            comboBoxActionType.SelectedIndex = 0;
            comboBoxTargetType.SelectedIndex = 0;

            ChangeParameterFieldsBasedOnType();

            textBoxComments.GotFocus += textBoxComments_GotFocus;
            textBoxComments.LostFocus += textBoxComments_LostFocus;

            panelStaticTooltipTypes.BackColor = Color.FromArgb(255, 255, 225);
            panelStaticTooltipParameters.BackColor = Color.FromArgb(255, 255, 225);
            labelStaticTooltipTextTypes.BackColor = Color.FromArgb(255, 255, 225);

            pictureBoxLoadScript.Enabled = textBoxEntryOrGuid.Text.Length > 0 && Settings.Default.UseWorldDatabase;
            pictureBoxCreateScript.Enabled = textBoxEntryOrGuid.Text.Length > 0;

            textBoxEventType.MouseWheel += textBoxEventType_MouseWheel;
            textBoxActionType.MouseWheel += textBoxActionType_MouseWheel;
            textBoxTargetType.MouseWheel += textBoxTargetType_MouseWheel;

            UpdateStaticTooltipOfTypes(comboBoxEventType, ScriptTypeId.ScriptTypeEvent);
            UpdateStaticTooltipOfParameter(labelEventParam1, 1, comboBoxEventType, ScriptTypeId.ScriptTypeEvent);

            buttonNewLine.Enabled = textBoxEntryOrGuid.Text.Length > 0;

            checkBoxShowBasicInfo.Checked = Settings.Default.ShowBasicInfo;
            checkBoxLockEventId.Checked = Settings.Default.LockSmartScriptId;
            checkBoxListActionlistsOrEntries.Checked = Settings.Default.ListActionLists;
            checkBoxAllowChangingEntryAndSourceType.Checked = Settings.Default.AllowChangingEntryAndSourceType;
            checkBoxUsePhaseColors.Checked = false;// Settings.Default.PhaseHighlighting;
            checkBoxUseStaticTooltips.Checked = Settings.Default.ShowTooltipsStaticly;

            DefaultState.Save(this);
        }

        public void AddWorkSpace()
        {
            SAIUserControlState newState = (SAIUserControlState)DefaultState.Clone();
            States.Add(newState);
            CurrentState = newState;
        }

        public void UserControlSAI_KeyDown(object sender, KeyEventArgs e)
        {
            if (!textBoxEntryOrGuid.Focused)
                return;

            //! Load the script of the entryorguid when no world database can be used. Otherwise create a new one.
            if (Settings.Default.UseWorldDatabase)
                TryToLoadScript();
            else
                TryToCreateScript();
        }

        public void buttonSearchForEntryOrGuid_Click(object sender, EventArgs e)
        {
            //! Just keep it in main thread; no purpose starting a new thread for this
            using (SearchForEntryForm entryForm = new SearchForEntryForm(textBoxEntryOrGuid.Text, GetSourceTypeByIndex()))
                entryForm.ShowDialog(this);
        }

        public SourceTypes GetSourceTypeByIndex()
        {
            switch (comboBoxSourceType.SelectedIndex)
            {
                case 0: //! Creature
                case 1: //! Gameobject
                case 2: //! Areatrigger
                    return (SourceTypes)comboBoxSourceType.SelectedIndex;
                case 3: //! Actionlist
                    return SourceTypes.SourceTypeScriptedActionlist;
                default:
                    return SourceTypes.SourceTypeCreature; //! Default...
            }
        }

        private async void comboBoxEventType_SelectedIndexChanged(object sender, EventArgs e)
        {
            await HandleComboBoxTypeIdSelectedIndexChanged(comboBoxEventType, textBoxEventType, ScriptTypeId.ScriptTypeEvent);
        }

        private async void comboBoxActionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            await HandleComboBoxTypeIdSelectedIndexChanged(comboBoxActionType, textBoxActionType, ScriptTypeId.ScriptTypeAction);
        }

        private async void comboBoxTargetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            await HandleComboBoxTypeIdSelectedIndexChanged(comboBoxTargetType, textBoxTargetType, ScriptTypeId.ScriptTypeTarget);
        }

        private async Task HandleComboBoxTypeIdSelectedIndexChanged(ComboBox comboBox, TextBox textBox, ScriptTypeId scriptTypeId)
        {
            try
            {
                textBox.Text = comboBox.SelectedIndex.ToString();
                textBox.SelectionStart = 3; //! Set cursor to end of text

                if (!MainForm.runningConstructor)
                {
                    ChangeParameterFieldsBasedOnType();
                    UpdateStaticTooltipOfTypes(comboBox, scriptTypeId);
                }

                SmartScriptList list = (SmartScriptList)customObjectListView.List;

                if (customObjectListView.SelectedObjects.Count > 0)
                {
                    switch (scriptTypeId)
                    {
                        case ScriptTypeId.ScriptTypeEvent:
                            list.SelectedScript.event_type = comboBox.SelectedIndex;
                            break;
                        case ScriptTypeId.ScriptTypeAction:
                            list.SelectedScript.action_type = comboBox.SelectedIndex;
                            break;
                        case ScriptTypeId.ScriptTypeTarget:
                            list.SelectedScript.target_type = comboBox.SelectedIndex;
                            break;
                    }

                    customObjectListView.List.ReplaceScript(list.SelectedScript);
                    await GenerateCommentForSmartScript(list.SelectedScript);
                }
            }
            catch (Exception)
            {

            }
        }

        public void ChangeParameterFieldsBasedOnType()
        {
            //! Event parameters
            int event_type = comboBoxEventType.SelectedIndex;
            labelEventParam1.Text = SAI_Editor_Manager.Instance.GetParameterStringById(event_type, 1, ScriptTypeId.ScriptTypeEvent);
            labelEventParam2.Text = SAI_Editor_Manager.Instance.GetParameterStringById(event_type, 2, ScriptTypeId.ScriptTypeEvent);
            labelEventParam3.Text = SAI_Editor_Manager.Instance.GetParameterStringById(event_type, 3, ScriptTypeId.ScriptTypeEvent);
            labelEventParam4.Text = SAI_Editor_Manager.Instance.GetParameterStringById(event_type, 4, ScriptTypeId.ScriptTypeEvent);

            if (!Settings.Default.ShowTooltipsStaticly)
            {
                AddTooltip(comboBoxEventType, comboBoxEventType.SelectedItem.ToString(), SAI_Editor_Manager.Instance.GetScriptTypeTooltipById(event_type, ScriptTypeId.ScriptTypeEvent));
                AddTooltip(labelEventParam1, labelEventParam1.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(event_type, 1, ScriptTypeId.ScriptTypeEvent));
                AddTooltip(labelEventParam2, labelEventParam2.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(event_type, 2, ScriptTypeId.ScriptTypeEvent));
                AddTooltip(labelEventParam3, labelEventParam3.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(event_type, 3, ScriptTypeId.ScriptTypeEvent));
                AddTooltip(labelEventParam4, labelEventParam4.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(event_type, 4, ScriptTypeId.ScriptTypeEvent));
            }

            //! Action parameters
            int action_type = comboBoxActionType.SelectedIndex;
            labelActionParam1.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 1, ScriptTypeId.ScriptTypeAction);
            labelActionParam2.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 2, ScriptTypeId.ScriptTypeAction);
            labelActionParam3.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 3, ScriptTypeId.ScriptTypeAction);
            labelActionParam4.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 4, ScriptTypeId.ScriptTypeAction);
            labelActionParam5.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 5, ScriptTypeId.ScriptTypeAction);
            labelActionParam6.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 6, ScriptTypeId.ScriptTypeAction);

            if (!Settings.Default.ShowTooltipsStaticly)
            {
                AddTooltip(comboBoxActionType, comboBoxActionType.SelectedItem.ToString(), SAI_Editor_Manager.Instance.GetScriptTypeTooltipById(action_type, ScriptTypeId.ScriptTypeAction));
                AddTooltip(labelActionParam1, labelActionParam1.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 1, ScriptTypeId.ScriptTypeAction));
                AddTooltip(labelActionParam2, labelActionParam2.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 2, ScriptTypeId.ScriptTypeAction));
                AddTooltip(labelActionParam3, labelActionParam3.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 3, ScriptTypeId.ScriptTypeAction));
                AddTooltip(labelActionParam4, labelActionParam4.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 4, ScriptTypeId.ScriptTypeAction));
                AddTooltip(labelActionParam5, labelActionParam5.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 5, ScriptTypeId.ScriptTypeAction));
                AddTooltip(labelActionParam6, labelActionParam6.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 6, ScriptTypeId.ScriptTypeAction));
            }

            //! Target parameters
            int target_type = comboBoxTargetType.SelectedIndex;
            labelTargetParam1.Text = SAI_Editor_Manager.Instance.GetParameterStringById(target_type, 1, ScriptTypeId.ScriptTypeTarget);
            labelTargetParam2.Text = SAI_Editor_Manager.Instance.GetParameterStringById(target_type, 2, ScriptTypeId.ScriptTypeTarget);
            labelTargetParam3.Text = SAI_Editor_Manager.Instance.GetParameterStringById(target_type, 3, ScriptTypeId.ScriptTypeTarget);

            if (!Settings.Default.ShowTooltipsStaticly)
            {
                AddTooltip(comboBoxTargetType, comboBoxTargetType.SelectedItem.ToString(), SAI_Editor_Manager.Instance.GetScriptTypeTooltipById(target_type, ScriptTypeId.ScriptTypeTarget));
                AddTooltip(labelTargetParam1, labelTargetParam1.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(target_type, 1, ScriptTypeId.ScriptTypeTarget));
                AddTooltip(labelTargetParam2, labelTargetParam2.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(target_type, 2, ScriptTypeId.ScriptTypeTarget));
                AddTooltip(labelTargetParam3, labelTargetParam3.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(target_type, 3, ScriptTypeId.ScriptTypeTarget));
            }

            AdjustAllParameterFields(event_type, action_type, target_type);
        }

        public void checkBoxLockEventId_CheckedChanged(object sender, EventArgs e)
        {
            textBoxId.Enabled = !checkBoxLockEventId.Checked;
        }

        bool ListContainsSmartScript(List<SmartScript> smartScriptsToReturn, SmartScript item)
        {
            return smartScriptsToReturn.Any(itemToReturn => itemToReturn.entryorguid == item.entryorguid && itemToReturn.id == item.id);
        }

        private async Task<List<SmartScript>> GetSmartScriptsForEntryAndSourceType(string entryOrGuid, SourceTypes sourceType, bool showError = true, bool promptCreateIfNoneFound = false)
        {
            List<SmartScript> smartScriptsToReturn = new List<SmartScript>();

            try
            {
                List<SmartScript> smartScripts = await SAI_Editor_Manager.Instance.worldDatabase.GetSmartScripts(CustomConverter.ToInt32(entryOrGuid), (int)sourceType);

                if (smartScripts == null)
                {
                    if (showError)
                    {
                        bool showNormalErrorMessage = false;
                        string message = String.Format("The entryorguid '{0}' could not be found in the smart_scripts table for the given source_type!", entryOrGuid);
                        smartScripts = await SAI_Editor_Manager.Instance.worldDatabase.GetSmartScriptsWithoutSourceType(CustomConverter.ToInt32(entryOrGuid), (int)sourceType);

                        if (smartScripts != null)
                        {
                            message += "\n\nA script was found with this entry using sourcetype " + smartScripts[0].source_type + " (" + SAI_Editor_Manager.Instance.GetSourceTypeString((SourceTypes)smartScripts[0].source_type).ToLower() + "). Do you wish to load this instead?";
                            DialogResult dialogResult = MessageBox.Show(message, "No scripts found!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                            if (dialogResult == DialogResult.Yes)
                            {
                                textBoxEntryOrGuid.Text = smartScripts[0].entryorguid.ToString();
                                comboBoxSourceType.SelectedIndex = GetIndexBySourceType((SourceTypes)smartScripts[0].source_type);
                                TryToLoadScript(forced: true);
                            }
                        }
                        else
                        {
                            switch (sourceType)
                            {
                                case SourceTypes.SourceTypeCreature:
                                    //! Get `id` from `creature` and check it for SAI
                                    if (CustomConverter.ToInt32(entryOrGuid) < 0) //! Guid
                                    {
                                        int entry = await SAI_Editor_Manager.Instance.worldDatabase.GetCreatureIdByGuid(-CustomConverter.ToInt32(entryOrGuid));
                                        smartScripts = await SAI_Editor_Manager.Instance.worldDatabase.GetSmartScripts(entry, (int)SourceTypes.SourceTypeCreature);

                                        if (smartScripts != null)
                                        {
                                            message += "\n\nA script was not found for this guid but we did find one using the entry of the guid (" + smartScripts[0].entryorguid + "). Do you wish to load this instead?";
                                            DialogResult dialogResult = MessageBox.Show(message, "No scripts found!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                                            if (dialogResult == DialogResult.Yes)
                                            {
                                                textBoxEntryOrGuid.Text = smartScripts[0].entryorguid.ToString();
                                                comboBoxSourceType.SelectedIndex = GetIndexBySourceType(SourceTypes.SourceTypeCreature);
                                                TryToLoadScript();
                                            }
                                        }
                                        else
                                            showNormalErrorMessage = true;
                                    }
                                    //! Get all `guid` instances from `creature` for the given `id` and allow user to select a script
                                    else //! Non-guid (entry)
                                    {
                                        int actualEntry = CustomConverter.ToInt32(entryOrGuid);
                                        List<Creature> creatures = await SAI_Editor_Manager.Instance.worldDatabase.GetCreaturesById(actualEntry);

                                        if (creatures != null)
                                        {
                                            List<List<SmartScript>> creaturesWithSmartAi = new List<List<SmartScript>>();

                                            foreach (Creature creature in creatures)
                                            {
                                                smartScripts = await SAI_Editor_Manager.Instance.worldDatabase.GetSmartScripts(-creature.guid, (int)SourceTypes.SourceTypeCreature);

                                                if (smartScripts != null)
                                                    creaturesWithSmartAi.Add(smartScripts);
                                            }

                                            if (creaturesWithSmartAi.Count > 0)
                                            {
                                                message += "\n\nA script was not found for this entry but we did find script(s) for guid(s) spawned under this entry. Do you wish to select one of these instead? (you can pick one out of all guid-scripts for this entry)";
                                                DialogResult dialogResult = MessageBox.Show(message, "No scripts found!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                                                if (dialogResult == DialogResult.Yes)
                                                    using (SelectSmartScriptForm selectSmartScriptForm = new SelectSmartScriptForm(creaturesWithSmartAi))
                                                        selectSmartScriptForm.ShowDialog(this);
                                            }
                                            else
                                                showNormalErrorMessage = true;
                                        }
                                        else
                                            showNormalErrorMessage = true;
                                    }
                                    break;
                                case SourceTypes.SourceTypeGameobject:
                                    //! Get `id` from `gameobject` and check it for SAI
                                    if (CustomConverter.ToInt32(entryOrGuid) < 0) //! Guid
                                    {
                                        int entry = await SAI_Editor_Manager.Instance.worldDatabase.GetGameobjectIdByGuid(-CustomConverter.ToInt32(entryOrGuid));
                                        smartScripts = await SAI_Editor_Manager.Instance.worldDatabase.GetSmartScripts(entry, (int)SourceTypes.SourceTypeGameobject);

                                        if (smartScripts != null)
                                        {
                                            message += "\n\nA script was not found for this guid but we did find one using the entry of the guid (" + smartScripts[0].entryorguid + "). Do you wish to load this instead?";
                                            DialogResult dialogResult = MessageBox.Show(message, "No scripts found!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                                            if (dialogResult == DialogResult.Yes)
                                            {
                                                textBoxEntryOrGuid.Text = smartScripts[0].entryorguid.ToString();
                                                comboBoxSourceType.SelectedIndex = GetIndexBySourceType(SourceTypes.SourceTypeGameobject);
                                                TryToLoadScript();
                                            }
                                        }
                                        else
                                            showNormalErrorMessage = true;
                                    }
                                    //! Get all `guid` instances from `gameobject` for the given `id` and allow user to select a script
                                    else //! Non-guid (entry)
                                    {
                                        int actualEntry = CustomConverter.ToInt32(entryOrGuid);
                                        List<Gameobject> gameobjects = await SAI_Editor_Manager.Instance.worldDatabase.GetGameobjectsById(actualEntry);

                                        if (gameobjects != null)
                                        {
                                            List<List<SmartScript>> gameobjectsWithSmartAi = new List<List<SmartScript>>();

                                            foreach (Gameobject gameobject in gameobjects)
                                            {
                                                smartScripts = await SAI_Editor_Manager.Instance.worldDatabase.GetSmartScripts(-gameobject.guid, (int)SourceTypes.SourceTypeGameobject);

                                                if (smartScripts != null)
                                                    gameobjectsWithSmartAi.Add(smartScripts);
                                            }

                                            if (gameobjectsWithSmartAi.Count > 0)
                                            {
                                                message += "\n\nA script was not found for this entry but we did find script(s) for guid(s) spawned under this entry. Do you wish to select one of these instead? (you can pick one out of all guid-scripts for this entry)";
                                                DialogResult dialogResult = MessageBox.Show(message, "No scripts found!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                                                if (dialogResult == DialogResult.Yes)
                                                    using (SelectSmartScriptForm selectSmartScriptForm = new SelectSmartScriptForm(gameobjectsWithSmartAi))
                                                        selectSmartScriptForm.ShowDialog(this);
                                            }
                                            else
                                                showNormalErrorMessage = true;
                                        }
                                        else
                                            showNormalErrorMessage = true;
                                    }
                                    break;
                                default:
                                    showNormalErrorMessage = true;
                                    break;
                            }
                        }

                        if (showNormalErrorMessage)
                        {
                            if (promptCreateIfNoneFound)
                            {
                                DialogResult dialogResult = MessageBox.Show(message + "\n\nDo you want to create a new script using this entryorguid?", "No scripts found!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                                if (dialogResult == DialogResult.Yes)
                                    TryToCreateScript();
                            }
                            else
                                MessageBox.Show(message, "No scripts found!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    pictureBoxLoadScript.Enabled = textBoxEntryOrGuid.Text.Length > 0 && Settings.Default.UseWorldDatabase;
                    pictureBoxCreateScript.Enabled = textBoxEntryOrGuid.Text.Length > 0;
                    return new List<SmartScript>();
                }

                for (int i = 0; i < smartScripts.Count; ++i)
                {
                    smartScriptsToReturn.Add(smartScripts[i]);

                    if (!checkBoxListActionlistsOrEntries.Checked || !checkBoxListActionlistsOrEntries.Enabled)
                        continue;

                    if (i == smartScripts.Count - 1 && originalEntryOrGuidAndSourceType.sourceType == SourceTypes.SourceTypeScriptedActionlist)
                    {
                        List<EntryOrGuidAndSourceType> timedActionListOrEntries = await SAI_Editor_Manager.Instance.GetTimedActionlistsOrEntries(smartScripts[i], sourceType);

                        //if (timedActionListOrEntries.sourceTypeOfEntry != SourceTypes.SourceTypeScriptedActionlist)
                        {
                            foreach (EntryOrGuidAndSourceType entryOrGuidAndSourceType in timedActionListOrEntries)
                            {
                                if (entryOrGuidAndSourceType.sourceType == SourceTypes.SourceTypeScriptedActionlist)
                                    continue;

                                List<SmartScript> newSmartScripts = await GetSmartScriptsForEntryAndSourceType(entryOrGuidAndSourceType.entryOrGuid.ToString(), entryOrGuidAndSourceType.sourceType);

                                if (newSmartScripts != null)
                                    foreach (SmartScript item in newSmartScripts.Where(item => !ListContainsSmartScript(smartScriptsToReturn, item)))
                                        smartScriptsToReturn.Add(item);

                                pictureBoxCreateScript.Enabled = textBoxEntryOrGuid.Text.Length > 0;
                            }
                        }
                    }

                    if (sourceType == originalEntryOrGuidAndSourceType.sourceType && originalEntryOrGuidAndSourceType.sourceType != SourceTypes.SourceTypeScriptedActionlist)
                    {
                        List<EntryOrGuidAndSourceType> timedActionListOrEntries = await SAI_Editor_Manager.Instance.GetTimedActionlistsOrEntries(smartScripts[i], sourceType);

                        foreach (EntryOrGuidAndSourceType entryOrGuidAndSourceType in timedActionListOrEntries)
                        {
                            List<SmartScript> newSmartScripts = await GetSmartScriptsForEntryAndSourceType(entryOrGuidAndSourceType.entryOrGuid.ToString(), entryOrGuidAndSourceType.sourceType);

                            foreach (SmartScript item in newSmartScripts.Where(item => !ListContainsSmartScript(smartScriptsToReturn, item)))
                                smartScriptsToReturn.Add(item);

                            pictureBoxCreateScript.Enabled = textBoxEntryOrGuid.Text.Length > 0;
                        }
                    }
                }
            }
            catch
            {
                if (showError)
                    MessageBox.Show("An exception was thrown while trying to load the smart_scripts for entryorguid " + entryOrGuid.ToString() + ".", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            pictureBoxLoadScript.Enabled = textBoxEntryOrGuid.Text.Length > 0 && Settings.Default.UseWorldDatabase;
            pictureBoxCreateScript.Enabled = textBoxEntryOrGuid.Text.Length > 0;
            return smartScriptsToReturn;
        }

        private void customObjectListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            MainForm.menuItemDeleteSelectedRow.Enabled = customObjectListView.SelectedObjects.Count > 0;
            MainForm.menuItemGenerateSql.Enabled = customObjectListView.SelectedObjects.Count > 0;
            buttonGenerateSql.Enabled = customObjectListView.SelectedObjects.Count > 0;
            MainForm.menuitemLoadSelectedEntry.Enabled = customObjectListView.SelectedObjects.Count > 0;
            MainForm.menuItemDuplicateRow.Enabled = customObjectListView.SelectedObjects.Count > 0;
            MainForm.menuItemGenerateComment.Enabled = customObjectListView.SelectedObjects.Count > 0;
            MainForm.menuItemCopySelectedRow.Enabled = customObjectListView.SelectedObjects.Count > 0;

            if (!e.IsSelected)
                return;

            FillFieldsBasedOnSelectedScript();

            if (checkBoxAllowChangingEntryAndSourceType.Checked)
                checkBoxListActionlistsOrEntries.Text = ((SmartScript)customObjectListView.SelectedObjects[0]).source_type == 9 ? "List entries too" : "List actionlists too";
        }

        public void FillFieldsBasedOnSelectedScript()
        {
            try
            {
                updatingFieldsBasedOnSelectedScript = true;
                SmartScript selectedScript = ((SmartScriptList)customObjectListView.List).SelectedScript;

                if (checkBoxAllowChangingEntryAndSourceType.Checked)
                {
                    textBoxEntryOrGuid.Text = selectedScript.entryorguid.ToString();
                    comboBoxSourceType.SelectedIndex = GetIndexBySourceType((SourceTypes)selectedScript.source_type);
                }

                textBoxId.Text = selectedScript.id.ToString();
                textBoxLinkTo.Text = selectedScript.link.ToString();
                textBoxLinkFrom.Text = GetLinkFromForSelection();

                int event_type = selectedScript.event_type;
                comboBoxEventType.SelectedIndex = event_type;
                textBoxEventPhasemask.Text = selectedScript.event_phase_mask.ToString();
                textBoxEventChance.Text = selectedScript.event_chance.ToString();
                textBoxEventFlags.Text = selectedScript.event_flags.ToString();

                //! Event parameters
                textBoxEventParam1.Text = selectedScript.event_param1.ToString();
                textBoxEventParam2.Text = selectedScript.event_param2.ToString();
                textBoxEventParam3.Text = selectedScript.event_param3.ToString();
                textBoxEventParam4.Text = selectedScript.event_param4.ToString();
                labelEventParam1.Text = SAI_Editor_Manager.Instance.GetParameterStringById(event_type, 1, ScriptTypeId.ScriptTypeEvent);
                labelEventParam2.Text = SAI_Editor_Manager.Instance.GetParameterStringById(event_type, 2, ScriptTypeId.ScriptTypeEvent);
                labelEventParam3.Text = SAI_Editor_Manager.Instance.GetParameterStringById(event_type, 3, ScriptTypeId.ScriptTypeEvent);
                labelEventParam4.Text = SAI_Editor_Manager.Instance.GetParameterStringById(event_type, 4, ScriptTypeId.ScriptTypeEvent);

                if (!Settings.Default.ShowTooltipsStaticly)
                {
                    AddTooltip(comboBoxEventType, comboBoxEventType.SelectedItem.ToString(), SAI_Editor_Manager.Instance.GetScriptTypeTooltipById(event_type, ScriptTypeId.ScriptTypeEvent));
                    AddTooltip(labelEventParam1, labelEventParam1.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(event_type, 1, ScriptTypeId.ScriptTypeEvent));
                    AddTooltip(labelEventParam2, labelEventParam2.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(event_type, 2, ScriptTypeId.ScriptTypeEvent));
                    AddTooltip(labelEventParam3, labelEventParam3.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(event_type, 3, ScriptTypeId.ScriptTypeEvent));
                    AddTooltip(labelEventParam4, labelEventParam4.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(event_type, 4, ScriptTypeId.ScriptTypeEvent));
                }

                //! Action parameters
                int action_type = selectedScript.action_type;
                comboBoxActionType.SelectedIndex = action_type;
                textBoxActionParam1.Text = selectedScript.action_param1.ToString();
                textBoxActionParam2.Text = selectedScript.action_param2.ToString();
                textBoxActionParam3.Text = selectedScript.action_param3.ToString();
                textBoxActionParam4.Text = selectedScript.action_param4.ToString();
                textBoxActionParam5.Text = selectedScript.action_param5.ToString();
                textBoxActionParam6.Text = selectedScript.action_param6.ToString();
                labelActionParam1.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 1, ScriptTypeId.ScriptTypeAction);
                labelActionParam2.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 2, ScriptTypeId.ScriptTypeAction);
                labelActionParam3.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 3, ScriptTypeId.ScriptTypeAction);
                labelActionParam4.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 4, ScriptTypeId.ScriptTypeAction);
                labelActionParam5.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 5, ScriptTypeId.ScriptTypeAction);
                labelActionParam6.Text = SAI_Editor_Manager.Instance.GetParameterStringById(action_type, 6, ScriptTypeId.ScriptTypeAction);

                if (!Settings.Default.ShowTooltipsStaticly)
                {
                    AddTooltip(comboBoxActionType, comboBoxActionType.SelectedItem.ToString(), SAI_Editor_Manager.Instance.GetScriptTypeTooltipById(action_type, ScriptTypeId.ScriptTypeAction));
                    AddTooltip(labelActionParam1, labelActionParam1.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 1, ScriptTypeId.ScriptTypeAction));
                    AddTooltip(labelActionParam2, labelActionParam2.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 2, ScriptTypeId.ScriptTypeAction));
                    AddTooltip(labelActionParam3, labelActionParam3.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 3, ScriptTypeId.ScriptTypeAction));
                    AddTooltip(labelActionParam4, labelActionParam4.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 4, ScriptTypeId.ScriptTypeAction));
                    AddTooltip(labelActionParam5, labelActionParam5.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 5, ScriptTypeId.ScriptTypeAction));
                    AddTooltip(labelActionParam6, labelActionParam6.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(action_type, 6, ScriptTypeId.ScriptTypeAction));
                }

                //! Target parameters
                int target_type = selectedScript.target_type;
                comboBoxTargetType.SelectedIndex = target_type;
                textBoxTargetParam1.Text = selectedScript.target_param1.ToString();
                textBoxTargetParam2.Text = selectedScript.target_param2.ToString();
                textBoxTargetParam3.Text = selectedScript.target_param3.ToString();
                labelTargetParam1.Text = SAI_Editor_Manager.Instance.GetParameterStringById(target_type, 1, ScriptTypeId.ScriptTypeTarget);
                labelTargetParam2.Text = SAI_Editor_Manager.Instance.GetParameterStringById(target_type, 2, ScriptTypeId.ScriptTypeTarget);
                labelTargetParam3.Text = SAI_Editor_Manager.Instance.GetParameterStringById(target_type, 3, ScriptTypeId.ScriptTypeTarget);

                if (!Settings.Default.ShowTooltipsStaticly)
                {
                    AddTooltip(comboBoxTargetType, comboBoxTargetType.SelectedItem.ToString(), SAI_Editor_Manager.Instance.GetScriptTypeTooltipById(target_type, ScriptTypeId.ScriptTypeTarget));
                    AddTooltip(labelTargetParam1, labelTargetParam1.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(target_type, 1, ScriptTypeId.ScriptTypeTarget));
                    AddTooltip(labelTargetParam2, labelTargetParam2.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(target_type, 2, ScriptTypeId.ScriptTypeTarget));
                    AddTooltip(labelTargetParam3, labelTargetParam3.Text, SAI_Editor_Manager.Instance.GetParameterTooltipById(target_type, 3, ScriptTypeId.ScriptTypeTarget));
                }

                textBoxTargetX.Text = selectedScript.target_x;
                textBoxTargetY.Text = selectedScript.target_y;
                textBoxTargetZ.Text = selectedScript.target_z;
                textBoxTargetO.Text = selectedScript.target_o;
                textBoxComments.Text = selectedScript.comment;

                AdjustAllParameterFields(event_type, action_type, target_type);
                updatingFieldsBasedOnSelectedScript = false;
            }
            catch
            {
                updatingFieldsBasedOnSelectedScript = false;
                MessageBox.Show("Something went wrong while attempting to edit the fields based on the new selection.", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetLinkFromForSelection()
        {
            SmartScript selectedScript = ListViewList.SelectedScript;

            foreach (SmartScript smartScript in ListViewList.SmartScripts)
            {
                if (smartScript.entryorguid != selectedScript.entryorguid || smartScript.source_type != selectedScript.source_type)
                    continue;

                if (smartScript.link > 0 && smartScript.link == ListViewList.SelectedScript.id)
                    return smartScript.id.ToString();
            }

            return "None";
        }

        public void AdjustAllParameterFields(int event_type, int action_type, int target_type)
        {
            SetVisibilityOfAllParamButtons(false);

            switch ((SmartEvent)event_type)
            {
                case SmartEvent.SMART_EVENT_SPELLHIT: //! Spell entry & Spell school
                case SmartEvent.SMART_EVENT_SPELLHIT_TARGET: //! Spell entry & Spell school
                case SmartEvent.SMART_EVENT_GOSSIP_SELECT: //! Gossip menu id & gossip id
                case SmartEvent.SMART_EVENT_DISTANCE_CREATURE: //! Creature guid
                case SmartEvent.SMART_EVENT_DISTANCE_GAMEOBJECT: //! Gameobject guid & entry
                    buttonEventParamOneSearch.Visible = true;
                    buttonEventParamTwoSearch.Visible = true;
                    break;
                case SmartEvent.SMART_EVENT_RESPAWN:
                    buttonEventParamOneSearch.Visible = true; //! Respawn condition (SMART_SCRIPT_RESPAWN_CONDITION_MAP / SMART_SCRIPT_RESPAWN_CONDITION_AREA)
                    buttonEventParamTwoSearch.Visible = true; //! Map entry
                    buttonEventParamThreeSearch.Visible = true; //! Zone entry
                    break;
                case SmartEvent.SMART_EVENT_AREATRIGGER_ONTRIGGER: //! Areatrigger entry
                case SmartEvent.SMART_EVENT_GO_STATE_CHANGED: //! Go state
                case SmartEvent.SMART_EVENT_GAME_EVENT_START: //! Game event entry
                case SmartEvent.SMART_EVENT_GAME_EVENT_END: //! Game event entry
                case SmartEvent.SMART_EVENT_MOVEMENTINFORM: //! Movement type
                case SmartEvent.SMART_EVENT_FRIENDLY_MISSING_BUFF: //! Spell id
                case SmartEvent.SMART_EVENT_HAS_AURA: //! Spell id
                case SmartEvent.SMART_EVENT_TARGET_BUFFED: //! Spell id
                case SmartEvent.SMART_EVENT_SUMMON_DESPAWNED: //! Creature entry
                case SmartEvent.SMART_EVENT_SUMMONED_UNIT: //! Creature entry
                case SmartEvent.SMART_EVENT_ACCEPTED_QUEST: //! Quest id
                case SmartEvent.SMART_EVENT_REWARD_QUEST: //! Quest id
                case SmartEvent.SMART_EVENT_RECEIVE_EMOTE: //! Emote id
                    buttonEventParamOneSearch.Visible = true;
                    break;
                case SmartEvent.SMART_EVENT_TEXT_OVER: //! Creature entry
                    buttonEventParamTwoSearch.Visible = true;
                    break;
                case SmartEvent.SMART_EVENT_VICTIM_CASTING: //! Spell id
                    buttonEventParamThreeSearch.Visible = true;
                    break;
                case SmartEvent.SMART_EVENT_KILL: //! Creature entry
                    buttonEventParamFourSearch.Visible = true;
                    break;
            }

            switch ((SmartAction)action_type)
            {
                case SmartAction.SMART_ACTION_CAST: //! Spell entry & Cast flags
                case SmartAction.SMART_ACTION_INVOKER_CAST: //! Spell entry & Cast flags
                case SmartAction.SMART_ACTION_CALL_CASTEDCREATUREORGO: //! Creature entry & Spell entry
                case SmartAction.SMART_ACTION_SUMMON_CREATURE: //! Creature entry & Summon type
                case SmartAction.SMART_ACTION_SET_UNIT_FIELD_BYTES_1: //! Bytes1flags & Type
                case SmartAction.SMART_ACTION_REMOVE_UNIT_FIELD_BYTES_1: //! Bytes1flags & Type
                case SmartAction.SMART_ACTION_RANDOM_PHASE_RANGE: //! Event phase 1 & 2
                    buttonActionParamOneSearch.Visible = true;
                    buttonActionParamTwoSearch.Visible = true;
                    break;
                case SmartAction.SMART_ACTION_CROSS_CAST:
                    buttonActionParamOneSearch.Visible = true; //! Spell entry
                    buttonActionParamTwoSearch.Visible = true; //! Cast flags
                    buttonActionParamThreeSearch.Visible = true; //! Target type
                    break;
                case SmartAction.SMART_ACTION_WP_STOP: //! Quest entry
                case SmartAction.SMART_ACTION_INTERRUPT_SPELL: //! Spell entry
                case SmartAction.SMART_ACTION_SEND_GOSSIP_MENU: //! Gossip menu id & npc_text.id
                case SmartAction.SMART_ACTION_CALL_TIMED_ACTIONLIST: //! Timer type
                    buttonActionParamTwoSearch.Visible = true;
                    break;
                case SmartAction.SMART_ACTION_WP_START:
                    buttonActionParamTwoSearch.Visible = true; //! Waypoint entry
                    buttonActionParamFourSearch.Visible = true; //! Quest entry
                    buttonActionParamSixSearch.Visible = true; //! React state
                    break;
                case SmartAction.SMART_ACTION_FOLLOW:
                    buttonActionParamThreeSearch.Visible = true; //! Creature entry
                    buttonActionParamFourSearch.Visible = true; //! Creature entry
                    break;
                case SmartAction.SMART_ACTION_RANDOM_PHASE:  //! Event phase 1-6
                case SmartAction.SMART_ACTION_RANDOM_EMOTE: //! Emote entry 1-6
                case SmartAction.SMART_ACTION_START_CLOSEST_WAYPOINT: //! Wp 1-6
                    buttonActionParamOneSearch.Visible = true;
                    buttonActionParamTwoSearch.Visible = true;
                    buttonActionParamThreeSearch.Visible = true;
                    buttonActionParamFourSearch.Visible = true;
                    buttonActionParamFiveSearch.Visible = true;
                    buttonActionParamSixSearch.Visible = true;
                    break;
                case SmartAction.SMART_ACTION_EQUIP:
                    buttonActionParamOneSearch.Visible = true; //! Equipment entry
                    buttonActionParamThreeSearch.Visible = true; //! Item entry 1
                    buttonActionParamFourSearch.Visible = true; //! Item entry 2
                    buttonActionParamFiveSearch.Visible = true; //! Item entry 3
                    break;
                case SmartAction.SMART_ACTION_SET_FACTION: //! Faction entry
                case SmartAction.SMART_ACTION_EMOTE: //! Emote entry
                case SmartAction.SMART_ACTION_SET_EMOTE_STATE: //! Emote entry
                case SmartAction.SMART_ACTION_FAIL_QUEST: //! Quest entry
                case SmartAction.SMART_ACTION_ADD_QUEST: //! Quest entry
                case SmartAction.SMART_ACTION_CALL_AREAEXPLOREDOREVENTHAPPENS: //! Quest entry
                case SmartAction.SMART_ACTION_CALL_GROUPEVENTHAPPENS: //! Quest entry
                case SmartAction.SMART_ACTION_SET_REACT_STATE: //! Reactstate
                case SmartAction.SMART_ACTION_SOUND: //! Sound entry
                case SmartAction.SMART_ACTION_MORPH_TO_ENTRY_OR_MODEL: //! Creature entry
                case SmartAction.SMART_ACTION_KILLED_MONSTER: //! Creature entry
                case SmartAction.SMART_ACTION_UPDATE_TEMPLATE: //! Creature entry
                case SmartAction.SMART_ACTION_MOUNT_TO_ENTRY_OR_MODEL: //! Creature entry
                case SmartAction.SMART_ACTION_GO_SET_LOOT_STATE: //! Gameobject state
                case SmartAction.SMART_ACTION_SET_POWER: //! Power type
                case SmartAction.SMART_ACTION_ADD_POWER: //! Power type
                case SmartAction.SMART_ACTION_REMOVE_POWER: //! Power type
                case SmartAction.SMART_ACTION_SUMMON_GO: //! Gameobject entry
                case SmartAction.SMART_ACTION_SET_EVENT_PHASE: //! Event phase
                case SmartAction.SMART_ACTION_SET_PHASE_MASK: //! Ingame phase
                case SmartAction.SMART_ACTION_ADD_ITEM: //! Item entry
                case SmartAction.SMART_ACTION_REMOVE_ITEM: //! Item entry
                case SmartAction.SMART_ACTION_TELEPORT: //! Map id
                case SmartAction.SMART_ACTION_SUMMON_CREATURE_GROUP: //! Summons group id
                case SmartAction.SMART_ACTION_REMOVEAURASFROMSPELL: //! Spell id
                case SmartAction.SMART_ACTION_SET_SHEATH: //! Sheath state
                case SmartAction.SMART_ACTION_ACTIVATE_TAXI: //! Taxi path id
                case SmartAction.SMART_ACTION_SET_UNIT_FLAG: //! Unit flags
                case SmartAction.SMART_ACTION_REMOVE_UNIT_FLAG: //! Unit flags
                case SmartAction.SMART_ACTION_SET_GO_FLAG: //! Gameobject flags
                case SmartAction.SMART_ACTION_ADD_GO_FLAG: //! Gameobject flags
                case SmartAction.SMART_ACTION_REMOVE_GO_FLAG: //! Gameobject flags
                case SmartAction.SMART_ACTION_SET_DYNAMIC_FLAG: //! Dynamic flags
                case SmartAction.SMART_ACTION_ADD_DYNAMIC_FLAG: //! Dynamic flags
                case SmartAction.SMART_ACTION_REMOVE_DYNAMIC_FLAG: //! Dynamic flags
                case SmartAction.SMART_ACTION_ADD_AURA: //! Spell id
                case SmartAction.SMART_ACTION_SET_NPC_FLAG: //! Npc flags
                case SmartAction.SMART_ACTION_ADD_NPC_FLAG: //! Npc flags
                case SmartAction.SMART_ACTION_REMOVE_NPC_FLAG: //! Npc flags
                case SmartAction.SMART_ACTION_INSTALL_AI_TEMPLATE: //! AI template
                case SmartAction.SMART_ACTION_GAME_EVENT_START: //! Game event entry
                case SmartAction.SMART_ACTION_GAME_EVENT_STOP: //! Game event entry
                    buttonActionParamOneSearch.Visible = true;
                    break;
            }

            switch ((SmartTarget)target_type)
            {
                case SmartTarget.SMART_TARGET_CREATURE_GUID:
                    buttonTargetParamOneSearch.Visible = true; //! Creature guid
                    buttonTargetParamTwoSearch.Visible = true; //! Creature entry
                    break;
                case SmartTarget.SMART_TARGET_GAMEOBJECT_GUID:
                    buttonTargetParamOneSearch.Visible = true; //! Gameobject guid
                    buttonTargetParamTwoSearch.Visible = true; //! Gameobject entry
                    break;
                case SmartTarget.SMART_TARGET_CREATURE_RANGE: //! Creature entry
                case SmartTarget.SMART_TARGET_CREATURE_DISTANCE: //! Creature entry
                case SmartTarget.SMART_TARGET_CLOSEST_CREATURE: //! Creature entry
                case SmartTarget.SMART_TARGET_GAMEOBJECT_RANGE: //! Gameobject entry
                case SmartTarget.SMART_TARGET_GAMEOBJECT_DISTANCE: //! Gameobject entry
                case SmartTarget.SMART_TARGET_CLOSEST_GAMEOBJECT: //! Gameobject entry
                    buttonTargetParamOneSearch.Visible = true;
                    break;
            }
        }

        public void AddTooltip(Control control, string title, string text, ToolTipIcon icon = ToolTipIcon.Info, bool isBallon = true, bool active = true, int autoPopDelay = 2100000000, bool showAlways = true)
        {
            if (String.IsNullOrWhiteSpace(title) || String.IsNullOrWhiteSpace(text))
            {
                DetailedToolTip toolTipExistent = ToolTipHelper.GetExistingToolTip(control);

                if (toolTipExistent != null)
                    toolTipExistent.Active = false;

                return;
            }

            DetailedToolTip toolTip = ToolTipHelper.GetControlToolTip(control);
            toolTip.ToolTipIcon = icon;
            toolTip.ToolTipTitle = title;
            toolTip.IsBalloon = isBallon;
            toolTip.Active = active;
            toolTip.AutoPopDelay = autoPopDelay;
            toolTip.ShowAlways = showAlways;
            toolTip.SetToolTipText(control, text);
        }

        public void textBoxEventTypeId_TextChanged(object sender, EventArgs e)
        {
            HandleTextBoxTypeIdTextChanged(textBoxEventType, comboBoxEventType, (int)SmartEvent.SMART_EVENT_MAX - 1);
        }

        public void textBoxActionTypeId_TextChanged(object sender, EventArgs e)
        {
            HandleTextBoxTypeIdTextChanged(textBoxActionType, comboBoxActionType, (int)SmartAction.SMART_ACTION_MAX - 1);
        }

        public void textBoxTargetTypeId_TextChanged(object sender, EventArgs e)
        {
            HandleTextBoxTypeIdTextChanged(textBoxTargetType, comboBoxTargetType, (int)SmartTarget.SMART_TARGET_MAX - 1);
        }

        public void HandleTextBoxTypeIdTextChanged(TextBox textBox, ComboBox comboBox, int max)
        {
            if (String.IsNullOrEmpty(textBox.Text))
            {
                comboBox.SelectedIndex = 0;
                textBox.Text = "0";
                textBox.SelectionStart = 3; //! Set cursor position to end of the line
            }
            else
            {
                int targetType;
                Int32.TryParse(textBox.Text, out targetType);

                if (targetType > max)
                {
                    comboBox.SelectedIndex = max;
                    textBox.Text = (max).ToString();
                    textBox.SelectionStart = 3; //! Set cursor position to end of the line
                }
                else
                    comboBox.SelectedIndex = targetType;
            }
        }

        public void DuplicateSelectedRow()
        {
            if (((SmartScriptList)customObjectListView.List).SelectedScript == null)
                return;

            SmartScript clonedSmartScript = ((SmartScriptList)customObjectListView.List).SelectedScript.Clone();

            if (!Settings.Default.DuplicatePrimaryFields)
                clonedSmartScript.id = ++lastSmartScriptIdOfScript;

            customObjectListView.List.AddScript(clonedSmartScript, selectNewItem: true);

            customObjectListView.HideSelection = false;
            customObjectListView.SelectObject(clonedSmartScript);
            customObjectListView.EnsureModelVisible(clonedSmartScript);
            customObjectListView.Select();
        }

        public void DeleteSelectedRow()
        {
            if (customObjectListView.SelectedObjects.Count == 0)
                return;

            int prevSelectedIndex = customObjectListView.SelectedIndex;

            if (((SmartScript)customObjectListView.SelectedObjects[0]).entryorguid == originalEntryOrGuidAndSourceType.entryOrGuid)
                if (((SmartScript)customObjectListView.SelectedObjects[0]).id == lastSmartScriptIdOfScript)
                    lastSmartScriptIdOfScript--;

            lastDeletedSmartScripts.Add(((SmartScriptList)customObjectListView.List).SelectedScript.Clone());
            customObjectListView.List.RemoveScript(((SmartScriptList)customObjectListView.List).SelectedScript);
            SetGenerateCommentsEnabled(customObjectListView.Items.Count > 0 && Settings.Default.UseWorldDatabase);

            if (customObjectListView.Items.Count <= 0)
                ResetFieldsToDefault(checkBoxAllowChangingEntryAndSourceType.Checked);
            else
                ReSelectListViewItemWithPrevIndex(prevSelectedIndex);

            //! Need to do this if static info is changed
            pictureBoxCreateScript.Enabled = textBoxEntryOrGuid.Text.Length > 0;
        }

        public void SetGenerateCommentsEnabled(bool enabled)
        {
            buttonGenerateComments.Enabled = enabled;
            MainForm.menuItemGenerateComment.Enabled = enabled;
        }

        public void ReSelectListViewItemWithPrevIndex(int prevIndex)
        {
            if (customObjectListView.Items.Count > prevIndex)
                customObjectListView.Items[prevIndex].Selected = true;
            else if (customObjectListView.Items.Count > 0)
                customObjectListView.Items[prevIndex - 1].Selected = true;
        }

        private async void checkBoxListActionlistsOrEntries_CheckedChanged(object sender, EventArgs e)
        {
            if (customObjectListView.Items.Count == 0)
                return;

            buttonGenerateSql.Enabled = false;
            MainForm.menuItemGenerateSql.Enabled = false;
            //int prevSelectedIndex = customObjectListView.SelectedObjects.Count > 0 ? customObjectListView.SelectedObjects[0].Index : 0;

            if (checkBoxListActionlistsOrEntries.Checked)
            {
                List<SmartScript> smartScripts = await GetSmartScriptsForEntryAndSourceType(originalEntryOrGuidAndSourceType.entryOrGuid.ToString(), originalEntryOrGuidAndSourceType.sourceType);

                //! Only add the new smartscript if it doesn't yet exist
                foreach (SmartScript newSmartScript in smartScripts)
                {
                    bool _continue = false;

                    for (int i = 0; i < customObjectListView.Items.Count; ++i)
                    {
                        SmartScript script = customObjectListView.List.Scripts[i] as SmartScript;

                        if (script.entryorguid == newSmartScript.entryorguid && script.id == newSmartScript.id)
                        {
                            _continue = true;
                            break;
                        }
                    }

                    if (_continue)
                        continue;

                    customObjectListView.List.AddScript(newSmartScript);
                }

                pictureBoxCreateScript.Enabled = textBoxEntryOrGuid.Text.Length > 0;
            }
            else
                RemoveNonOriginalScriptsFromView();

            HandleShowBasicInfo();

            //if (customObjectListView.Items.Count > prevSelectedIndex)
            //    customObjectListView.Items[prevSelectedIndex].Selected = true;

            if (customObjectListView.Items.Count > 0)
                customObjectListView.SelectObject(customObjectListView.SelectedObjects.Count > 0 ? customObjectListView.SelectedObjects[0] : customObjectListView.Objects.Cast<object>().ElementAt(0));

            buttonGenerateSql.Enabled = customObjectListView.Items.Count > 0;
            MainForm.menuItemGenerateSql.Enabled = customObjectListView.Items.Count > 0;
        }

        public void RemoveNonOriginalScriptsFromView()
        {
            List<DatabaseClass> smartScriptsToRemove = ListViewList.SmartScripts.Where(smartScript => smartScript.source_type != (int)originalEntryOrGuidAndSourceType.sourceType).Cast<DatabaseClass>().ToList();

            foreach (SmartScript smartScript in smartScriptsToRemove.Cast<SmartScript>())
                customObjectListView.List.RemoveScript(smartScript);
        }

        public int GetIndexBySourceType(SourceTypes sourceType)
        {
            switch (sourceType)
            {
                case SourceTypes.SourceTypeCreature:
                case SourceTypes.SourceTypeGameobject:
                case SourceTypes.SourceTypeAreaTrigger:
                    return (int)sourceType;
                case SourceTypes.SourceTypeScriptedActionlist:
                    return 3;
                default:
                    return -1;
            }
        }

        public void pictureBoxLoadScript_Click(object sender, EventArgs e)
        {
            TryToLoadScript();
        }

        public void pictureBoxCreateScript_Click(object sender, EventArgs e)
        {
            TryToCreateScript();
        }

        public async void TryToCreateScript(bool fromNewLine = false)
        {
            if (!pictureBoxCreateScript.Enabled || String.IsNullOrWhiteSpace(textBoxEntryOrGuid.Text) || comboBoxSourceType.SelectedIndex == -1)
                return;

            if (customObjectListView.Items.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show("There is already a script loaded at this moment. Do you want to overwrite this?\n\nWarning: overwriting means local unsaved changes will also be discarded!", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult != DialogResult.Yes)
                    return;

                ResetFieldsToDefault();
            }

            int entryorguid;
            try
            {
                entryorguid = Int32.Parse(textBoxEntryOrGuid.Text);
            }
            catch (OverflowException)
            {
                MessageBox.Show("The entryorguid is either too big or too small.", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (FormatException)
            {
                MessageBox.Show("The entryorguid field does not contain a valid number.", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lastSmartScriptIdOfScript = 0;
            int source_type = (int)GetSourceTypeByIndex();
            string sourceTypeString = SAI_Editor_Manager.Instance.GetSourceTypeString((SourceTypes)source_type).ToLower();

            if (!Settings.Default.UseWorldDatabase)
                goto SkipWorldDatabaseChecks;

            string aiName = await SAI_Editor_Manager.Instance.worldDatabase.GetObjectAiName(entryorguid, source_type);
            List<SmartScript> smartScripts = await SAI_Editor_Manager.Instance.worldDatabase.GetSmartScripts(entryorguid, source_type);

            //! Allow adding new lines even if the AIName is already set
            if ((SourceTypes)source_type == SourceTypes.SourceTypeAreaTrigger)
            {
                if (aiName != String.Empty)
                {
                    string errorMessage = "This areatrigger already has its ";

                    if (aiName != "SmartTrigger")
                        errorMessage += "ScriptName set to '" + aiName + "'";
                    else
                        errorMessage += "AIName set (for SmartAI)! Do you want to load it instead?";

                    DialogResult dialogResult = MessageBox.Show(errorMessage, "Something went wrong", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (dialogResult == DialogResult.Yes)
                        TryToLoadScript();

                    return;
                }
            }
            else
            {
                if (aiName != String.Empty)
                {
                    string strAlreadyHasAiName = String.Empty;
                    bool aiNameIsSmart = SAI_Editor_Manager.Instance.IsAiNameSmartAi(aiName);

                    if (aiNameIsSmart)
                    {
                        if (smartScripts == null || smartScripts.Count == 0)
                            goto SkipWorldDatabaseChecks;

                        if (fromNewLine)
                            goto SkipAiNameAndScriptNameChecks;

                        strAlreadyHasAiName += "This " + sourceTypeString + " already has its AIName set to '" + aiName + "'";
                        strAlreadyHasAiName += "! Do you want to load it instead?";
                    }
                    else
                    {
                        strAlreadyHasAiName += "This " + sourceTypeString + " already has its AIName set to '" + aiName + "'";
                        strAlreadyHasAiName += " and can therefore not have any SmartAI. Do you want to get rid of this AIName right now?";
                    }

                    DialogResult dialogResult = MessageBox.Show(strAlreadyHasAiName, "Something went wrong", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (dialogResult == DialogResult.Yes)
                    {
                        if (!aiNameIsSmart)
                        {
                            //! We don't have to target areatrigger_scripts here, as we've already done this a few lines up
                            string sqlOutput = "UPDATE `" + GetTemplateTableBySourceType((SourceTypes)source_type) + "` SET `AIName`=" + '"' + '"' + " WHERE `entry`=" + entryorguid + ";\n";

                            using (SqlOutputForm sqlOutputForm = new SqlOutputForm(sqlOutput, false, saveToFile: false))
                                sqlOutputForm.ShowDialog(this);
                        }
                        else
                            TryToLoadScript();
                    }

                    return;
                }

                string scriptName = await SAI_Editor_Manager.Instance.worldDatabase.GetObjectScriptName(entryorguid, source_type);

                if (scriptName != String.Empty)
                {
                    MessageBox.Show("This " + sourceTypeString + " already has its ScriptName set (to '" + scriptName + "')!", "Something went wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

        SkipAiNameAndScriptNameChecks:

            if (smartScripts != null && smartScripts.Count > 0)
            {
                string errorMessage = "This " + sourceTypeString + " already has smart scripts";// (without its AIName set to SmartAI)! Do you want to load it instead?";

                if ((SourceTypes)source_type != SourceTypes.SourceTypeScriptedActionlist)
                    errorMessage += " (without its AIName set to SmartAI)!";
                else
                    errorMessage += "!";

                errorMessage += " Do you want to load it instead?";
                DialogResult dialogResult = MessageBox.Show(errorMessage, "Something went wrong", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.Yes)
                    TryToLoadScript();

                return;
            }

        SkipWorldDatabaseChecks:
            buttonNewLine.Enabled = false;
            checkBoxListActionlistsOrEntries.Text = GetSourceTypeByIndex() == SourceTypes.SourceTypeScriptedActionlist ? "List entries too" : "List actionlists too";
            pictureBoxLoadScript.Enabled = false;
            pictureBoxCreateScript.Enabled = false;

            originalEntryOrGuidAndSourceType.entryOrGuid = entryorguid;
            originalEntryOrGuidAndSourceType.sourceType = (SourceTypes)source_type;

            ListViewList.ClearScripts();

            SmartScript newSmartScript = new SmartScript();
            newSmartScript.entryorguid = entryorguid;
            newSmartScript.source_type = source_type;

            if (checkBoxLockEventId.Checked)
                newSmartScript.id = 0;
            else
                newSmartScript.id = -1;

            newSmartScript.link = CustomConverter.ToInt32(textBoxLinkTo.Text);
            newSmartScript.event_type = CustomConverter.ToInt32(textBoxEventType.Text);
            newSmartScript.event_phase_mask = CustomConverter.ToInt32(textBoxEventPhasemask.Text);
            newSmartScript.event_chance = CustomConverter.ToInt32(textBoxEventChance.Value);
            newSmartScript.event_flags = CustomConverter.ToInt32(textBoxEventFlags.Text);
            newSmartScript.event_param1 = CustomConverter.ToInt32(textBoxEventParam1.Text);
            newSmartScript.event_param2 = CustomConverter.ToInt32(textBoxEventParam2.Text);
            newSmartScript.event_param3 = CustomConverter.ToInt32(textBoxEventParam3.Text);
            newSmartScript.event_param4 = CustomConverter.ToInt32(textBoxEventParam4.Text);
            newSmartScript.action_type = CustomConverter.ToInt32(textBoxActionType.Text);
            newSmartScript.action_param1 = CustomConverter.ToInt32(textBoxActionParam1.Text);
            newSmartScript.action_param2 = CustomConverter.ToInt32(textBoxActionParam2.Text);
            newSmartScript.action_param3 = CustomConverter.ToInt32(textBoxActionParam3.Text);
            newSmartScript.action_param4 = CustomConverter.ToInt32(textBoxActionParam4.Text);
            newSmartScript.action_param5 = CustomConverter.ToInt32(textBoxActionParam5.Text);
            newSmartScript.action_param6 = CustomConverter.ToInt32(textBoxActionParam6.Text);
            newSmartScript.target_type = CustomConverter.ToInt32(textBoxTargetType.Text);
            newSmartScript.target_param1 = CustomConverter.ToInt32(textBoxTargetParam1.Text);
            newSmartScript.target_param2 = CustomConverter.ToInt32(textBoxTargetParam2.Text);
            newSmartScript.target_param3 = CustomConverter.ToInt32(textBoxTargetParam3.Text);
            newSmartScript.target_x = textBoxTargetX.Text;
            newSmartScript.target_y = textBoxTargetY.Text;
            newSmartScript.target_z = textBoxTargetZ.Text;
            newSmartScript.target_o = textBoxTargetO.Text;

            if (Settings.Default.GenerateComments && Settings.Default.UseWorldDatabase)
                newSmartScript.comment = await CommentGenerator.Instance.GenerateCommentFor(newSmartScript, originalEntryOrGuidAndSourceType);
            else if (textBoxComments.Text.Contains(" - Event - Action (phase) (dungeon difficulty)"))
                newSmartScript.comment = SAI_Editor_Manager.Instance.GetDefaultCommentForSourceType((SourceTypes)newSmartScript.source_type);
            else
                newSmartScript.comment = textBoxComments.Text;

            ListViewList.AddScript(newSmartScript, selectNewItem: true);

            HandleShowBasicInfo(true);

            //! Call the select stuff after calling HandleShowBasicInfo. Reason is it tampers with the property fields
            customObjectListView.HideSelection = false;
            customObjectListView.SelectObject(newSmartScript);
            customObjectListView.EnsureModelVisible(newSmartScript);
            customObjectListView.Select();

            buttonNewLine.Enabled = textBoxEntryOrGuid.Text.Length > 0;
            SetGenerateCommentsEnabled(customObjectListView.Items.Count > 0 && Settings.Default.UseWorldDatabase);
            pictureBoxLoadScript.Enabled = textBoxEntryOrGuid.Text.Length > 0 && Settings.Default.UseWorldDatabase;
            pictureBoxCreateScript.Enabled = textBoxEntryOrGuid.Text.Length > 0;
        }

        private string GetTemplateTableBySourceType(SourceTypes sourceType)
        {
            switch (sourceType)
            {
                case SourceTypes.SourceTypeCreature:
                    return "creature_template";
                case SourceTypes.SourceTypeGameobject:
                    return "gameobject_template";
            }

            return "<unknown template table>";
        }

        public async void TryToLoadScript(int entryorguid = -1, SourceTypes sourceType = SourceTypes.SourceTypeNone, bool showErrorIfNoneFound = true, bool promptCreateIfNoneFound = false, bool forced = false)
        {
            if ((!forced && !pictureBoxLoadScript.Enabled) || !Settings.Default.UseWorldDatabase)
                return;

            ListViewList.ClearScripts();
            ResetFieldsToDefault();

            if (String.IsNullOrEmpty(textBoxEntryOrGuid.Text))
                return;

            buttonGenerateSql.Enabled = false;
            MainForm.menuItemGenerateSql.Enabled = false;
            pictureBoxLoadScript.Enabled = false;
            pictureBoxCreateScript.Enabled = false;
            lastSmartScriptIdOfScript = 0;

            if (entryorguid != -1 && sourceType != SourceTypes.SourceTypeNone)
            {
                originalEntryOrGuidAndSourceType.entryOrGuid = entryorguid;
                originalEntryOrGuidAndSourceType.sourceType = sourceType;
                textBoxEntryOrGuid.Text = entryorguid.ToString();
                comboBoxSourceType.SelectedIndex = GetIndexBySourceType(sourceType);
            }
            else
            {
                try
                {
                    originalEntryOrGuidAndSourceType.entryOrGuid = Int32.Parse(textBoxEntryOrGuid.Text);
                }
                catch (OverflowException)
                {
                    MessageBox.Show("The entryorguid is either too big or too small.", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (FormatException)
                {
                    MessageBox.Show("The entryorguid field does not contain a valid number.", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                originalEntryOrGuidAndSourceType.sourceType = GetSourceTypeByIndex();
            }

            List<SmartScript> smartScripts = await GetSmartScriptsForEntryAndSourceType(originalEntryOrGuidAndSourceType.entryOrGuid.ToString(), originalEntryOrGuidAndSourceType.sourceType, showErrorIfNoneFound, promptCreateIfNoneFound);
            ListViewList.ReplaceScripts(smartScripts.Cast<DatabaseClass>().ToList());
            checkBoxListActionlistsOrEntries.Text = originalEntryOrGuidAndSourceType.sourceType == SourceTypes.SourceTypeScriptedActionlist ? "List entries too" : "List actionlists too";

            buttonNewLine.Enabled = false;
            SetGenerateCommentsEnabled(customObjectListView.Items.Count > 0 && Settings.Default.UseWorldDatabase);
            HandleShowBasicInfo();

            if (customObjectListView.Items.Count > 0)
            {
                if (customObjectListView.AllColumns.Count > 0)
                    customObjectListView.Sort(customObjectListView.AllColumns.ElementAt(1), SortOrder.Ascending);

                customObjectListView.Items[0].Selected = true;
                customObjectListView.Select(); //! Sets the focus on the listview

                if (checkBoxListActionlistsOrEntries.Enabled && checkBoxListActionlistsOrEntries.Checked)
                {
                    for (int i = 0; i < customObjectListView.Items.Count; ++i)
                    {
                        ListViewItem item = customObjectListView.Items[i];

                        if (item.Text == originalEntryOrGuidAndSourceType.entryOrGuid.ToString())
                            lastSmartScriptIdOfScript = CustomConverter.ToInt32(item.SubItems[2].Text);
                    }
                }
                else
                    lastSmartScriptIdOfScript = CustomConverter.ToInt32(customObjectListView.Items[customObjectListView.Items.Count - 1].SubItems[2].Text);
            }

            buttonNewLine.Enabled = textBoxEntryOrGuid.Text.Length > 0;
            buttonGenerateSql.Enabled = customObjectListView.Items.Count > 0;
            MainForm.menuItemGenerateSql.Enabled = customObjectListView.Items.Count > 0;
            pictureBoxCreateScript.Enabled = textBoxEntryOrGuid.Text.Length > 0;
            ResizeColumns();
        }

        public void buttonSearchPhasemask_Click(object sender, EventArgs e)
        {
            ShowSelectForm("SmartPhaseMasks", textBoxEventPhasemask);
        }

        public void buttonSelectEventFlag_Click(object sender, EventArgs e)
        {
            ShowSelectForm("SmartEventFlags", textBoxEventFlags);
        }

        private ListView.ListViewItemCollection GetItemsBasedOnSelection(ListView listView)
        {
            ListView listViewScriptsCopy = new ListView();

            foreach (ListViewItem item in listView.Items)
                if (item.SubItems[1].Text == listView.SelectedItems[0].SubItems[1].Text)
                    listViewScriptsCopy.Items.Add((ListViewItem)item.Clone());

            return listViewScriptsCopy.Items;
        }

        public void buttonLinkTo_Click(object sender, EventArgs e)
        {
            TryToOpenLinkForm(textBoxLinkTo);
        }

        public void buttonLinkFrom_Click(object sender, EventArgs e)
        {
            TryToOpenLinkForm(textBoxLinkFrom);
        }

        public void TryToOpenLinkForm(TextBox textBoxToChange)
        {
            if (customObjectListView.Items.Count <= 1)
            {
                MessageBox.Show("There are not enough items in the listview in order to link!", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (customObjectListView.SelectedObjects.Count == 0)
            {
                MessageBox.Show("You must first select a line in the script", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SearchForLinkForm searchForLinkForm = new SearchForLinkForm(ListViewList.SmartScripts, customObjectListView.SelectedIndex, textBoxToChange))
                searchForLinkForm.ShowDialog(this);
        }

        public void ResetFieldsToDefault(bool withStatic = false)
        {
            if (withStatic)
            {
                textBoxEntryOrGuid.Text = String.Empty;
                comboBoxSourceType.SelectedIndex = 0;
            }

            comboBoxEventType.SelectedIndex = 0;
            comboBoxActionType.SelectedIndex = 0;
            comboBoxTargetType.SelectedIndex = 0;
            textBoxEventType.Text = "0";
            textBoxActionType.Text = "0";
            textBoxTargetType.Text = "0";
            textBoxEventChance.Text = "100";
            textBoxId.Text = "-1";
            textBoxLinkFrom.Text = "None";
            textBoxLinkTo.Text = "0";
            textBoxComments.Text = SAI_Editor_Manager.Instance.GetDefaultCommentForSourceType(GetSourceTypeByIndex());
            textBoxEventPhasemask.Text = "0";
            textBoxEventFlags.Text = "0";

            foreach (TabPage page in tabControlParameters.TabPages)
                foreach (Control control in page.Controls)
                    if (control is TextBox)
                        control.Text = "0";

            SetVisibilityOfAllParamButtons(false);
        }

        public void SetVisibilityOfAllParamButtons(bool visible)
        {
            foreach (TabPage page in tabControlParameters.TabPages)
                foreach (Control control in page.Controls)
                    if (control is Button)
                        control.Visible = visible;
        }

        public void SetVisibilityOfAllParamButtonsInTab(string tabText, bool visible)
        {
            foreach (TabPage page in tabControlParameters.TabPages)
                if (page.Text == tabText)
                    foreach (Control control in page.Controls)
                        if (control is Button)
                            control.Visible = visible;
        }

        public void ShowSearchFromDatabaseForm(TextBox textBoxToChange, DatabaseSearchFormType searchType)
        {
            if (!Settings.Default.UseWorldDatabase)
            {
                MessageBox.Show("You are unable to search for this in the creator-mode because a database connection is required.", "Can't search in creator-mode", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SearchFromDatabaseForm searchFromDatabaseForm = new SearchFromDatabaseForm(textBoxToChange, searchType))
                searchFromDatabaseForm.ShowDialog(this);

            textBoxToChange.Focus();
        }

        public void ShowSelectForm(string formTemplate, TextBox textBoxToChange)
        {
            using (Form selectForm = (Form)Activator.CreateInstance(SAI_Editor_Manager.SearchFormsContainer[formTemplate], new object[] { textBoxToChange }))
                selectForm.ShowDialog(this);

            textBoxToChange.Focus();
        }

        public void buttonEventParamOneSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxEventParam1;

            switch ((SmartEvent)comboBoxEventType.SelectedIndex)
            {
                case SmartEvent.SMART_EVENT_SPELLHIT: //! Spell id
                case SmartEvent.SMART_EVENT_FRIENDLY_MISSING_BUFF: //! Spell id
                case SmartEvent.SMART_EVENT_HAS_AURA: //! Spell id
                case SmartEvent.SMART_EVENT_TARGET_BUFFED: //! Spell id
                case SmartEvent.SMART_EVENT_SPELLHIT_TARGET: //! Spell id
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSpell);
                    break;
                case SmartEvent.SMART_EVENT_RESPAWN: //! Respawn condition
                    ShowSelectForm("SmartRespawnCondition", textBoxToChange);
                    break;
                case SmartEvent.SMART_EVENT_SUMMON_DESPAWNED: //! Creature entry
                case SmartEvent.SMART_EVENT_SUMMONED_UNIT: //! Creature entry
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry);
                    break;
                case SmartEvent.SMART_EVENT_AREATRIGGER_ONTRIGGER: //! Areatrigger entry
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeAreaTrigger);
                    break;
                case SmartEvent.SMART_EVENT_GO_STATE_CHANGED: //! Go state
                    ShowSelectForm("GoStates", textBoxToChange);
                    break;
                case SmartEvent.SMART_EVENT_GAME_EVENT_START: //! Game event entry
                case SmartEvent.SMART_EVENT_GAME_EVENT_END: //! Game event entry
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGameEvent);
                    break;
                case SmartEvent.SMART_EVENT_MOVEMENTINFORM: //! Motion type
                    ShowSelectForm("MovementGeneratorType", textBoxToChange);
                    break;
                case SmartEvent.SMART_EVENT_ACCEPTED_QUEST: //! Quest id
                case SmartEvent.SMART_EVENT_REWARD_QUEST: //! Quest id
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeQuest);
                    break;
                case SmartEvent.SMART_EVENT_RECEIVE_EMOTE: //! Emote id
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeEmote);
                    break;
                case SmartEvent.SMART_EVENT_GOSSIP_SELECT: //! Gossip menu id
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGossipMenuOptionMenuId);
                    break;
                case SmartEvent.SMART_EVENT_DISTANCE_CREATURE: //! Creature guid
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureGuid);
                    break;
                case SmartEvent.SMART_EVENT_DISTANCE_GAMEOBJECT: //! Gameobject guid
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectGuid);
                    break;
            }
        }

        public void buttonEventParamTwoSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxEventParam2;

            switch ((SmartEvent)comboBoxEventType.SelectedIndex)
            {
                case SmartEvent.SMART_EVENT_SPELLHIT: //! Spell school
                case SmartEvent.SMART_EVENT_SPELLHIT_TARGET: //! Spell school
                    ShowSelectForm("SpellSchools", textBoxToChange);
                    break;
                case SmartEvent.SMART_EVENT_RESPAWN: //! Map
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeMap);
                    break;
                case SmartEvent.SMART_EVENT_TEXT_OVER: //! Creature entry
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry);
                    break;
                case SmartEvent.SMART_EVENT_GOSSIP_SELECT: //! Gossip id
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGossipMenuOptionId);
                    break;
                case SmartEvent.SMART_EVENT_DISTANCE_CREATURE: //! Creature entry
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry);
                    break;
                case SmartEvent.SMART_EVENT_DISTANCE_GAMEOBJECT: //! Gameobject entry
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectEntry);
                    break;
            }
        }

        public void buttonEventParamThreeSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxEventParam3;

            switch ((SmartEvent)comboBoxEventType.SelectedIndex)
            {
                case SmartEvent.SMART_EVENT_RESPAWN: //! Zone
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeAreaOrZone);
                    break;
                case SmartEvent.SMART_EVENT_VICTIM_CASTING: //! Spell id
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSpell);
                    break;
            }
        }

        public void buttonEventParamFourSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxEventParam4;

            switch ((SmartEvent)comboBoxEventType.SelectedIndex)
            {
                case SmartEvent.SMART_EVENT_KILL: //! Creature entry
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry);
                    break;
            }
        }

        public void buttonTargetParamOneSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxTargetParam1;

            switch ((SmartTarget)comboBoxTargetType.SelectedIndex)
            {
                case SmartTarget.SMART_TARGET_CREATURE_RANGE: //! Creature entry
                case SmartTarget.SMART_TARGET_CREATURE_DISTANCE:
                case SmartTarget.SMART_TARGET_CLOSEST_CREATURE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry);
                    break;
                case SmartTarget.SMART_TARGET_CREATURE_GUID: //! Creature guid
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureGuid);
                    break;
                case SmartTarget.SMART_TARGET_GAMEOBJECT_RANGE:
                case SmartTarget.SMART_TARGET_GAMEOBJECT_DISTANCE:
                case SmartTarget.SMART_TARGET_CLOSEST_GAMEOBJECT: //! Gameobject entry
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectEntry);
                    break;
                case SmartTarget.SMART_TARGET_GAMEOBJECT_GUID: //! Gameobject guid
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectGuid);
                    break;
            }

            switch ((SmartAction)comboBoxActionType.SelectedIndex)
            {
                case SmartAction.SMART_ACTION_INSTALL_AI_TEMPLATE:
                    //! This button is different based on the number in the first parameter field
                    switch ((SmartAiTemplates)CustomConverter.ToInt32(textBoxActionParam1.Text))
                    {
                        case SmartAiTemplates.SMARTAI_TEMPLATE_CASTER:
                        case SmartAiTemplates.SMARTAI_TEMPLATE_TURRET:
                            ShowSelectForm("SmartCastFlags", textBoxToChange);
                            break;
                    }
                    break;
            }
        }

        public void buttonTargetParamTwoSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxTargetParam2;

            switch ((SmartTarget)comboBoxTargetType.SelectedIndex)
            {
                case SmartTarget.SMART_TARGET_CREATURE_GUID: //! Creature entry
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry);
                    break;
                case SmartTarget.SMART_TARGET_GAMEOBJECT_GUID: //! Gameobject entry
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectEntry);
                    break;
            }
        }

        public void buttonActionParamOneSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxActionParam1;

            switch ((SmartAction)comboBoxActionType.SelectedIndex)
            {
                case SmartAction.SMART_ACTION_CAST:
                case SmartAction.SMART_ACTION_INVOKER_CAST:
                case SmartAction.SMART_ACTION_CROSS_CAST:
                case SmartAction.SMART_ACTION_REMOVEAURASFROMSPELL:
                case SmartAction.SMART_ACTION_ADD_AURA:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSpell);
                    break;
                case SmartAction.SMART_ACTION_SET_FACTION:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeFaction);
                    break;
                case SmartAction.SMART_ACTION_EMOTE:
                case SmartAction.SMART_ACTION_RANDOM_EMOTE:
                case SmartAction.SMART_ACTION_SET_EMOTE_STATE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeEmote);
                    break;
                case SmartAction.SMART_ACTION_FAIL_QUEST:
                case SmartAction.SMART_ACTION_ADD_QUEST:
                case SmartAction.SMART_ACTION_CALL_AREAEXPLOREDOREVENTHAPPENS:
                case SmartAction.SMART_ACTION_CALL_GROUPEVENTHAPPENS:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeQuest);
                    break;
                case SmartAction.SMART_ACTION_SET_REACT_STATE:
                    ShowSelectForm("ReactState", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_SOUND:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSound);
                    break;
                case SmartAction.SMART_ACTION_MORPH_TO_ENTRY_OR_MODEL:
                case SmartAction.SMART_ACTION_SUMMON_CREATURE:
                case SmartAction.SMART_ACTION_CALL_CASTEDCREATUREORGO:
                case SmartAction.SMART_ACTION_KILLED_MONSTER:
                case SmartAction.SMART_ACTION_UPDATE_TEMPLATE:
                case SmartAction.SMART_ACTION_MOUNT_TO_ENTRY_OR_MODEL:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry);
                    break;
                case SmartAction.SMART_ACTION_GO_SET_LOOT_STATE:
                    ShowSelectForm("GoStates", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_SET_POWER:
                case SmartAction.SMART_ACTION_ADD_POWER:
                case SmartAction.SMART_ACTION_REMOVE_POWER:
                    ShowSelectForm("PowerTypes", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_SUMMON_GO:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectEntry);
                    break;
                case SmartAction.SMART_ACTION_SET_EVENT_PHASE:
                case SmartAction.SMART_ACTION_RANDOM_PHASE:
                case SmartAction.SMART_ACTION_RANDOM_PHASE_RANGE:
                    ShowSelectForm("SmartPhaseMasks", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_SET_PHASE_MASK:
                    ShowSelectForm("PhaseMasks", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_ADD_ITEM:
                case SmartAction.SMART_ACTION_REMOVE_ITEM:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeItemEntry);
                    break;
                case SmartAction.SMART_ACTION_TELEPORT:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeMap);
                    break;
                case SmartAction.SMART_ACTION_SUMMON_CREATURE_GROUP:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSummonsId);
                    break;
                case SmartAction.SMART_ACTION_SET_SHEATH:
                    ShowSelectForm("SheathState", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_ACTIVATE_TAXI:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeTaxiPath);
                    break;
                case SmartAction.SMART_ACTION_SET_UNIT_FLAG:
                case SmartAction.SMART_ACTION_REMOVE_UNIT_FLAG:
                    //! There should be a different form opened based on parameter 2. If parameter two is set to '0' it means
                    //! we target UNIT_FIELD_FLAGS. If it's above 0 it means we target UNIT_FIELD_FLAGS2 (notice the 2).
                    if (textBoxActionParam2.Text == "0" || String.IsNullOrWhiteSpace(textBoxActionParam2.Text))
                        ShowSelectForm("UnitFlags", textBoxToChange);
                    else
                        ShowSelectForm("UnitFlags2", textBoxToChange);

                    break;
                case SmartAction.SMART_ACTION_SET_GO_FLAG:
                case SmartAction.SMART_ACTION_ADD_GO_FLAG:
                case SmartAction.SMART_ACTION_REMOVE_GO_FLAG:
                    ShowSelectForm("GoFlags", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_SET_DYNAMIC_FLAG:
                case SmartAction.SMART_ACTION_ADD_DYNAMIC_FLAG:
                case SmartAction.SMART_ACTION_REMOVE_DYNAMIC_FLAG:
                    ShowSelectForm("DynamicFlags", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_EQUIP:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeEquipTemplate);
                    break;
                case SmartAction.SMART_ACTION_SET_NPC_FLAG:
                case SmartAction.SMART_ACTION_ADD_NPC_FLAG:
                case SmartAction.SMART_ACTION_REMOVE_NPC_FLAG:
                    ShowSelectForm("NpcFlags", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_INSTALL_AI_TEMPLATE:
                    ShowSelectForm("SmartAiTemplates", textBoxToChange);
                    ParameterInstallAiTemplateChanged();
                    break;
                case SmartAction.SMART_ACTION_SET_UNIT_FIELD_BYTES_1:
                case SmartAction.SMART_ACTION_REMOVE_UNIT_FIELD_BYTES_1:
                    int searchType;

                    if (Int32.TryParse(textBoxActionParam2.Text, out searchType))
                    {
                        switch (searchType)
                        {
                            case 0:
                                ShowSelectForm("UnitStandStateType", textBoxToChange);
                                break;
                            //case 1:
                            //    break;
                            case 2:
                                ShowSelectForm("UnitStandFlags", textBoxToChange);
                                break;
                            case 3:
                                ShowSelectForm("UnitBytes1_Flags", textBoxToChange);
                                break;
                            default:
                                MessageBox.Show("The second parameter (type) must be set to a valid search type (0, 2 or 3).", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                        }
                    }

                    break;
                case SmartAction.SMART_ACTION_GAME_EVENT_START: //! Game event entry
                case SmartAction.SMART_ACTION_GAME_EVENT_STOP: //! Game event entry
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGameEvent);
                    break;
                case SmartAction.SMART_ACTION_START_CLOSEST_WAYPOINT:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeWaypoint);
                    break;
            }
        }

        public void ParameterInstallAiTemplateChanged()
        {
            SetVisibilityOfAllParamButtonsInTab("Action", false);
            SetVisibilityOfAllParamButtonsInTab("Target", false);

            labelActionParam1.Text = String.Empty;
            labelActionParam2.Text = String.Empty;
            labelActionParam3.Text = String.Empty;
            labelActionParam4.Text = String.Empty;
            labelActionParam5.Text = String.Empty;
            labelActionParam6.Text = String.Empty;

            labelActionParam1.Text = "Template entry";
            buttonActionParamOneSearch.Visible = true;
            int newTemplateId = CustomConverter.ToInt32(textBoxActionParam1.Text);

            switch ((SmartAiTemplates)newTemplateId)
            {
                case SmartAiTemplates.SMARTAI_TEMPLATE_BASIC:
                case SmartAiTemplates.SMARTAI_TEMPLATE_PASSIVE:
                    break;
                case SmartAiTemplates.SMARTAI_TEMPLATE_CASTER:
                case SmartAiTemplates.SMARTAI_TEMPLATE_TURRET:
                    labelActionParam2.Text = "Spell id";
                    buttonActionParamTwoSearch.Visible = true; //! Spell id
                    labelActionParam3.Text = "RepeatMin";
                    labelActionParam4.Text = "RepeatMax";
                    labelActionParam5.Text = "Range";
                    labelActionParam6.Text = "Mana pct";

                    labelTargetParam1.Text = "Castflag";
                    buttonTargetParamOneSearch.Visible = true;
                    break;
                case SmartAiTemplates.SMARTAI_TEMPLATE_CAGED_GO_PART:
                    labelActionParam2.Text = "Creature entry";
                    buttonActionParamTwoSearch.Visible = true; //! Creature entry
                    labelActionParam3.Text = "Credit at end (0/1)";
                    break;
                case SmartAiTemplates.SMARTAI_TEMPLATE_CAGED_NPC_PART:
                    labelActionParam2.Text = "Gameobject entry";
                    buttonActionParamTwoSearch.Visible = true; //! Gameobject entry
                    labelActionParam3.Text = "Despawn time";
                    labelActionParam4.Text = "Walk/run (0/1)";
                    labelActionParam5.Text = "Distance";
                    labelActionParam6.Text = "Group id";
                    break;
            }
        }

        public void buttonActionParamTwoSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxActionParam2;

            switch ((SmartAction)comboBoxActionType.SelectedIndex)
            {
                case SmartAction.SMART_ACTION_CAST:
                case SmartAction.SMART_ACTION_INVOKER_CAST:
                case SmartAction.SMART_ACTION_CROSS_CAST:
                    ShowSelectForm("SmartCastFlags", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_WP_STOP:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeQuest);
                    break;
                case SmartAction.SMART_ACTION_INTERRUPT_SPELL:
                case SmartAction.SMART_ACTION_CALL_CASTEDCREATUREORGO:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSpell);
                    break;
                case SmartAction.SMART_ACTION_SUMMON_CREATURE:
                    ShowSelectForm("TempSummonType", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_RANDOM_PHASE:
                case SmartAction.SMART_ACTION_RANDOM_PHASE_RANGE:
                    ShowSelectForm("SmartPhaseMasks", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_RANDOM_EMOTE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeEmote);
                    break;
                case SmartAction.SMART_ACTION_INSTALL_AI_TEMPLATE:
                    //! This button is different based on the number in the first parameter field
                    switch ((SmartAiTemplates)CustomConverter.ToInt32(textBoxActionParam1.Text))
                    {
                        case SmartAiTemplates.SMARTAI_TEMPLATE_CASTER:
                        case SmartAiTemplates.SMARTAI_TEMPLATE_TURRET:
                            ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeSpell);
                            break;
                        case SmartAiTemplates.SMARTAI_TEMPLATE_CAGED_GO_PART:
                            ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry);
                            break;
                        case SmartAiTemplates.SMARTAI_TEMPLATE_CAGED_NPC_PART:
                            ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeGameobjectEntry);
                            break;
                    }
                    break;
                case SmartAction.SMART_ACTION_WP_START:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeWaypoint);
                    break;
                case SmartAction.SMART_ACTION_SEND_GOSSIP_MENU:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeNpcText);
                    break;
                case SmartAction.SMART_ACTION_SET_UNIT_FIELD_BYTES_1:
                case SmartAction.SMART_ACTION_REMOVE_UNIT_FIELD_BYTES_1:
                    ShowSelectForm("UnitFieldBytes1Types", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_CALL_TIMED_ACTIONLIST:
                    ShowSelectForm("SmartActionlistTimerUpdateType", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_START_CLOSEST_WAYPOINT:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeWaypoint);
                    break;
            }
        }

        public void buttonActionParamThreeSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxActionParam3;

            switch ((SmartAction)comboBoxActionType.SelectedIndex)
            {
                case SmartAction.SMART_ACTION_FOLLOW:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry);
                    break;
                case SmartAction.SMART_ACTION_CROSS_CAST:
                    ShowSelectForm("SmartTarget", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_RANDOM_PHASE:
                    ShowSelectForm("SmartPhaseMasks", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_EQUIP:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeItemEntry);
                    break;
                case SmartAction.SMART_ACTION_RANDOM_EMOTE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeEmote);
                    break;
                case SmartAction.SMART_ACTION_START_CLOSEST_WAYPOINT:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeWaypoint);
                    break;
            }
        }

        public void buttonActionParamFourSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxActionParam4;

            switch ((SmartAction)comboBoxActionType.SelectedIndex)
            {
                case SmartAction.SMART_ACTION_FOLLOW:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeCreatureEntry);
                    break;
                case SmartAction.SMART_ACTION_WP_START:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeQuest);
                    break;
                case SmartAction.SMART_ACTION_RANDOM_PHASE:
                    ShowSelectForm("SmartPhaseMasks", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_EQUIP:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeItemEntry);
                    break;
                case SmartAction.SMART_ACTION_RANDOM_EMOTE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeEmote);
                    break;
                case SmartAction.SMART_ACTION_START_CLOSEST_WAYPOINT:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeWaypoint);
                    break;
            }
        }

        public void buttonActionParamFiveSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxActionParam5;

            switch ((SmartAction)comboBoxActionType.SelectedIndex)
            {
                case SmartAction.SMART_ACTION_RANDOM_PHASE:
                    ShowSelectForm("SmartPhaseMasks", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_EQUIP:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeItemEntry);
                    break;
                case SmartAction.SMART_ACTION_RANDOM_EMOTE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeEmote);
                    break;
                case SmartAction.SMART_ACTION_START_CLOSEST_WAYPOINT:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeWaypoint);
                    break;
            }
        }

        public void buttonActionParamSixSearch_Click(object sender, EventArgs e)
        {
            TextBox textBoxToChange = textBoxActionParam6;

            switch ((SmartAction)comboBoxActionType.SelectedIndex)
            {
                case SmartAction.SMART_ACTION_WP_START:
                    ShowSelectForm("ReactState", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_RANDOM_PHASE:
                    ShowSelectForm("SmartPhaseMasks", textBoxToChange);
                    break;
                case SmartAction.SMART_ACTION_RANDOM_EMOTE:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeEmote);
                    break;
                case SmartAction.SMART_ACTION_START_CLOSEST_WAYPOINT:
                    ShowSearchFromDatabaseForm(textBoxToChange, DatabaseSearchFormType.DatabaseSearchFormTypeWaypoint);
                    break;
            }
        }

        public void textBoxComments_GotFocus(object sender, EventArgs e)
        {
            if (textBoxComments.Text == SAI_Editor_Manager.Instance.GetDefaultCommentForSourceType(GetSourceTypeByIndex()))
                textBoxComments.Text = String.Empty;
        }

        public void textBoxComments_LostFocus(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxComments.Text))
                textBoxComments.Text = SAI_Editor_Manager.Instance.GetDefaultCommentForSourceType(GetSourceTypeByIndex());
        }

        public void comboBoxEventType_MouseEnter(object sender, EventArgs e)
        {
            UpdateStaticTooltipOfTypes(sender as ComboBox, ScriptTypeId.ScriptTypeEvent);
        }

        public void comboBoxActionType_MouseEnter(object sender, EventArgs e)
        {
            UpdateStaticTooltipOfTypes(sender as ComboBox, ScriptTypeId.ScriptTypeAction);
        }

        public void comboBoxTargetType_MouseEnter(object sender, EventArgs e)
        {
            UpdateStaticTooltipOfTypes(sender as ComboBox, ScriptTypeId.ScriptTypeTarget);
        }

        public void UpdateStaticTooltipOfTypes(ComboBox comboBoxToTarget, ScriptTypeId scriptTypeId)
        {
            string toolTipOfType = SAI_Editor_Manager.Instance.GetScriptTypeTooltipById(comboBoxToTarget.SelectedIndex, scriptTypeId);
            string toolTipTitleOfType = comboBoxToTarget.SelectedItem.ToString();

            if (!String.IsNullOrWhiteSpace(toolTipOfType) && !String.IsNullOrWhiteSpace(toolTipTitleOfType))
            {
                labelStaticTooltipTextTypes.Text = toolTipOfType;
                labelStaticTooltipTitleTypes.Text = toolTipTitleOfType;
            }
        }

        private int GetSelectedIndexByScriptTypeId(ScriptTypeId scriptTypeId)
        {
            switch (scriptTypeId)
            {
                case ScriptTypeId.ScriptTypeEvent:
                    return comboBoxEventType.SelectedIndex;
                case ScriptTypeId.ScriptTypeAction:
                    return comboBoxActionType.SelectedIndex;
                case ScriptTypeId.ScriptTypeTarget:
                    return comboBoxTargetType.SelectedIndex;
            }

            return 0;
        }

        private string GetSelectedItemByScriptTypeId(ScriptTypeId scriptTypeId)
        {
            switch (scriptTypeId)
            {
                case ScriptTypeId.ScriptTypeEvent:
                    return comboBoxEventType.SelectedItem.ToString();
                case ScriptTypeId.ScriptTypeAction:
                    return comboBoxActionType.SelectedItem.ToString();
                case ScriptTypeId.ScriptTypeTarget:
                    return comboBoxTargetType.SelectedItem.ToString();
            }

            return String.Empty;
        }

        public void UpdateStaticTooltipOfParameter(Label labelToTarget, int paramId, ComboBox comboBoxToTarget, ScriptTypeId scriptTypeId)
        {
            string toolTipOfType = SAI_Editor_Manager.Instance.GetParameterTooltipById(comboBoxToTarget.SelectedIndex, paramId, scriptTypeId);

            if (!String.IsNullOrWhiteSpace(toolTipOfType))
            {
                labelStaticTooltipTextParameters.Text = toolTipOfType;
                labelStaticTooltipParameterTitleTypes.Text = comboBoxToTarget.SelectedItem + " - " + labelToTarget.Text;
            }
        }

        public void labelEventParams_MouseEnter(object sender, EventArgs e)
        {
            UpdateStaticTooltipOfParameter(sender as Label, (sender as LabelWithTooltip).TooltipParameterId, comboBoxEventType, ScriptTypeId.ScriptTypeEvent);
        }

        public void labelActionParams_MouseEnter(object sender, EventArgs e)
        {
            UpdateStaticTooltipOfParameter(sender as Label, (sender as LabelWithTooltip).TooltipParameterId, comboBoxActionType, ScriptTypeId.ScriptTypeAction);
        }

        public void labelTargetParams_MouseEnter(object sender, EventArgs e)
        {
            UpdateStaticTooltipOfParameter(sender as Label, (sender as LabelWithTooltip).TooltipParameterId, comboBoxTargetType, ScriptTypeId.ScriptTypeTarget);
        }

        private async void buttonNewLine_Click(object sender, EventArgs e)
        {
            if (customObjectListView.Items.Count == 0)
            {
                if (!Settings.Default.UseWorldDatabase)
                {
                    TryToCreateScript(true);
                    return;
                }

                string aiName = await SAI_Editor_Manager.Instance.worldDatabase.GetObjectAiName(CustomConverter.ToInt32(textBoxEntryOrGuid.Text), (int)GetSourceTypeByIndex());

                if (!SAI_Editor_Manager.Instance.IsAiNameSmartAi(aiName))
                {
                    DialogResult dialogResult = MessageBox.Show("Are you sure you want to create a new script for the given entry and sourcetype?", "Something went wrong", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (dialogResult == DialogResult.Yes)
                        TryToCreateScript(true);
                }
                else
                    TryToCreateScript(true);

                return;
            }

            buttonNewLine.Enabled = false;
            SmartScript newSmartScript = new SmartScript();
            newSmartScript.entryorguid = originalEntryOrGuidAndSourceType.entryOrGuid;
            newSmartScript.source_type = (int)originalEntryOrGuidAndSourceType.sourceType;

            if (checkBoxLockEventId.Checked)
                newSmartScript.id = ++lastSmartScriptIdOfScript;
            else
                newSmartScript.id = -1;

            if (Settings.Default.GenerateComments && Settings.Default.UseWorldDatabase)
                newSmartScript.comment = await CommentGenerator.Instance.GenerateCommentFor(newSmartScript, originalEntryOrGuidAndSourceType);
            else
                newSmartScript.comment = SAI_Editor_Manager.Instance.GetDefaultCommentForSourceType((SourceTypes)newSmartScript.source_type);

            newSmartScript.event_chance = 100;

            //! All strings have to be initialized otherwise they become null and give cause an exception in the future.
            newSmartScript.target_x = "0";
            newSmartScript.target_y = "0";
            newSmartScript.target_z = "0";
            newSmartScript.target_o = "0";

            ListViewList.AddScript(newSmartScript, selectNewItem: true);

            HandleShowBasicInfo(true);

            //! Call the select stuff after calling HandleShowBasicInfo. Reason is it tampers with the property fields
            customObjectListView.HideSelection = false;
            customObjectListView.SelectObject(newSmartScript);
            customObjectListView.EnsureModelVisible(newSmartScript);
            customObjectListView.Select();

            ResizeColumns();

            buttonNewLine.Enabled = textBoxEntryOrGuid.Text.Length > 0;
        }

        private async void textBoxLinkTo_TextChanged(object sender, EventArgs e)
        {
            if (customObjectListView.SelectedObjects.Count > 0)
            {
                if (!updatingFieldsBasedOnSelectedScript && ListViewList.SelectedScript.id.ToString() == textBoxLinkTo.Text)
                {
                    MessageBox.Show("You can not link to or from the same id you're linking to.", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxLinkFrom.Text = GetLinkFromForSelection();
                    textBoxLinkTo.Text = "0";
                    return;
                }

                int linkTo = CustomConverter.ToInt32(textBoxLinkTo.Text);
                ListViewList.SelectedScript.link = linkTo;
                ListViewList.ReplaceScript(ListViewList.SelectedScript);
                await GenerateCommentForSmartScript(ListViewList.SelectedScript);

                foreach (SmartScript smartScript in ListViewList.SmartScripts)
                {
                    if (smartScript.id == linkTo)
                    {
                        if ((SmartEvent)smartScript.event_type == SmartEvent.SMART_EVENT_LINK)
                            await GenerateCommentForSmartScript(smartScript, false);

                        break;
                    }
                }
            }
        }

        public void textBoxComments_TextChanged(object sender, EventArgs e)
        {
            if (customObjectListView.SelectedObjects.Count > 0)
            {
                ListViewList.SelectedScript.comment = textBoxComments.Text;
                ListViewList.ReplaceScript(ListViewList.SelectedScript);
                ResizeColumns();
            }
        }

        private async void textBoxEventPhasemask_TextChanged(object sender, EventArgs e)
        {
            if (customObjectListView.SelectedObjects.Count > 0)
            {
                ListViewList.SelectedScript.event_phase_mask = CustomConverter.ToInt32(textBoxEventPhasemask.Text);
                ListViewList.ReplaceScript(ListViewList.SelectedScript);
                await GenerateCommentForSmartScript(ListViewList.SelectedScript);
            }

            customObjectListView.List.Apply(); //! Refreshes colors and whatnot
        }

        private async void textBoxEventChance_ValueChanged(object sender, EventArgs e)
        {
            if (customObjectListView.SelectedObjects.Count > 0)
            {
                ListViewList.SelectedScript.event_chance = (int)textBoxEventChance.Value; //! Using .Text propert results in wrong value
                ListViewList.ReplaceScript(ListViewList.SelectedScript);
                await GenerateCommentForSmartScript(ListViewList.SelectedScript);
            }
        }

        private async void textBoxEventFlags_TextChanged(object sender, EventArgs e)
        {
            if (customObjectListView.SelectedObjects.Count > 0)
            {
                ListViewList.SelectedScript.event_flags = CustomConverter.ToInt32(textBoxEventFlags.Text);
                ListViewList.ReplaceScript(ListViewList.SelectedScript);
                await GenerateCommentForSmartScript(ListViewList.SelectedScript);
            }
        }

        private async void textBoxLinkFrom_TextChanged(object sender, EventArgs e)
        {
            if (ListViewList.SelectedScript == null)
                return;

            int newLinkFrom = 0;// CustomConverter.ToInt32(textBoxLinkFrom.Text);

            try
            {
                newLinkFrom = Int32.Parse(textBoxLinkFrom.Text);
            }
            catch
            {
                previousLinkFrom = -1;
                return;
            }

            //! Only if the property was changed by hand (by user) and not by selecting a line
            if (!updatingFieldsBasedOnSelectedScript)
            {
                if (newLinkFrom == ListViewList.SelectedScript.id)
                {
                    MessageBox.Show("You can not link to or from the same id you're linking to.", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxLinkFrom.Text = GetLinkFromForSelection();
                    previousLinkFrom = -1;
                    return;
                }

                if (previousLinkFrom == newLinkFrom)
                    return;

                for (int i = 0; i < ListViewList.SmartScripts.Count; ++i)
                {
                    SmartScript smartScript = ListViewList.SmartScripts[i];

                    if (smartScript.entryorguid != originalEntryOrGuidAndSourceType.entryOrGuid || smartScript.source_type != (int)originalEntryOrGuidAndSourceType.sourceType)
                        continue;

                    if (smartScript.link == previousLinkFrom)
                    {
                        smartScript.link = 0;
                        await GenerateCommentForSmartScript(smartScript, false);
                    }

                    if (smartScript.id == newLinkFrom && ListViewList.SelectedScript != null)
                    {
                        smartScript.link = ListViewList.SelectedScript.id;
                        await GenerateCommentForSmartScript(smartScript, false);
                    }
                }

                ListViewList.Apply(true);
            }

            previousLinkFrom = newLinkFrom;
        }

        private async void textBoxEventParam1_Leave(object sender, EventArgs e)
        {
            if (customObjectListView.SelectedObjects.Count > 0)
            {
                ListViewList.SelectedScript.event_param1 = CustomConverter.ToInt32(textBoxEventParam1.Text);
                ListViewList.ReplaceScript(ListViewList.SelectedScript);
                await GenerateCommentForSmartScript(ListViewList.SelectedScript);
            }
        }

        private async void textBoxEventParam2_Leave(object sender, EventArgs e)
        {
            if (customObjectListView.SelectedObjects.Count > 0)
            {
                ListViewList.SelectedScript.event_param2 = CustomConverter.ToInt32(textBoxEventParam2.Text);
                ListViewList.ReplaceScript(ListViewList.SelectedScript);
                await GenerateCommentForSmartScript(ListViewList.SelectedScript);
            }
        }

        private async void textBoxEventParam3_Leave(object sender, EventArgs e)
        {
            if (customObjectListView.SelectedObjects.Count > 0)
            {
                ListViewList.SelectedScript.event_param3 = CustomConverter.ToInt32(textBoxEventParam3.Text);
                ListViewList.ReplaceScript(ListViewList.SelectedScript);
                await GenerateCommentForSmartScript(ListViewList.SelectedScript);
            }
        }

        private async void textBoxEventParam4_Leave(object sender, EventArgs e)
        {
            if (customObjectListView.SelectedObjects.Count > 0)
            {
                ListViewList.SelectedScript.event_param4 = CustomConverter.ToInt32(textBoxEventParam4.Text);
                ListViewList.ReplaceScript(ListViewList.SelectedScript);
                await GenerateCommentForSmartScript(ListViewList.SelectedScript);
            }
        }

        private async void textBoxActionParam1_Leave(object sender, EventArgs e)
        {
            if ((SmartAction)comboBoxActionType.SelectedIndex == SmartAction.SMART_ACTION_INSTALL_AI_TEMPLATE)
                ParameterInstallAiTemplateChanged();

            if (customObjectListView.SelectedObjects.Count > 0)
            {
                ListViewList.SelectedScript.action_param1 = CustomConverter.ToInt32(textBoxActionParam1.Text);
                ListViewList.ReplaceScript(ListViewList.SelectedScript);
                await GenerateCommentForSmartScript(ListViewList.SelectedScript);
            }
        }

        private async void textBoxActionParam2_Leave(object sender, EventArgs e)
        {
            if (customObjectListView.SelectedObjects.Count > 0)
            {
                ListViewList.SelectedScript.action_param2 = CustomConverter.ToInt32(textBoxActionParam2.Text);
                ListViewList.ReplaceScript(ListViewList.SelectedScript);
                await GenerateCommentForSmartScript(ListViewList.SelectedScript);
            }
        }

        private async void textBoxActionParam3_Leave(object sender, EventArgs e)
        {
            if (customObjectListView.SelectedObjects.Count > 0)
            {
                ListViewList.SelectedScript.action_param3 = CustomConverter.ToInt32(textBoxActionParam3.Text);
                ListViewList.ReplaceScript(ListViewList.SelectedScript);
                await GenerateCommentForSmartScript(ListViewList.SelectedScript);
            }
        }

        private async void textBoxActionParam4_Leave(object sender, EventArgs e)
        {
            if (customObjectListView.SelectedObjects.Count > 0)
            {
                ListViewList.SelectedScript.action_param4 = CustomConverter.ToInt32(textBoxActionParam4.Text);
                ListViewList.ReplaceScript(ListViewList.SelectedScript);
                await GenerateCommentForSmartScript(ListViewList.SelectedScript);
            }
        }

        private async void textBoxActionParam5_Leave(object sender, EventArgs e)
        {
            if (customObjectListView.SelectedObjects.Count > 0)
            {
                ListViewList.SelectedScript.action_param5 = CustomConverter.ToInt32(textBoxActionParam5.Text);
                ListViewList.ReplaceScript(ListViewList.SelectedScript);
                await GenerateCommentForSmartScript(ListViewList.SelectedScript);
            }
        }

        private async void textBoxActionParam6_Leave(object sender, EventArgs e)
        {
            if (customObjectListView.SelectedObjects.Count > 0)
            {
                ListViewList.SelectedScript.action_param6 = CustomConverter.ToInt32(textBoxActionParam6.Text);
                ListViewList.ReplaceScript(ListViewList.SelectedScript);
                await GenerateCommentForSmartScript(ListViewList.SelectedScript);
            }
        }

        private async void textBoxTargetParam1_Leave(object sender, EventArgs e)
        {
            if (customObjectListView.SelectedObjects.Count > 0)
            {
                ListViewList.SelectedScript.target_param1 = CustomConverter.ToInt32(textBoxTargetParam1.Text);
                ListViewList.ReplaceScript(ListViewList.SelectedScript);
                await GenerateCommentForSmartScript(ListViewList.SelectedScript);
            }
        }

        private async void textBoxTargetParam2_Leave(object sender, EventArgs e)
        {
            if (customObjectListView.SelectedObjects.Count > 0)
            {
                ListViewList.SelectedScript.target_param2 = CustomConverter.ToInt32(textBoxTargetParam2.Text);
                ListViewList.ReplaceScript(ListViewList.SelectedScript);
                await GenerateCommentForSmartScript(ListViewList.SelectedScript);
            }
        }

        private async void textBoxTargetParam3_Leave(object sender, EventArgs e)
        {
            if (customObjectListView.SelectedObjects.Count > 0)
            {
                ListViewList.SelectedScript.target_param3 = CustomConverter.ToInt32(textBoxTargetParam3.Text);
                ListViewList.ReplaceScript(ListViewList.SelectedScript);
                await GenerateCommentForSmartScript(ListViewList.SelectedScript);
            }
        }

        public void textBoxTargetCoordinateParams_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsPunctuation(e.KeyChar) && !Char.IsDigit(e.KeyChar))
                return;

            double result;

            if (!Double.TryParse((sender as TextBox).Text + e.KeyChar, out result))
                e.Handled = true;
        }

        private async void textBoxTargetX_Leave(object sender, EventArgs e)
        {
            if (customObjectListView.SelectedObjects.Count > 0)
            {
                textBoxTargetX.Text = textBoxTargetX.Text.Replace(".", ",");
                textBoxTargetX.SelectionStart = textBoxTargetX.Text.Length + 1; //! Set cursor to end of text
                ListViewList.SelectedScript.target_x = textBoxTargetX.Text;
                ListViewList.ReplaceScript(ListViewList.SelectedScript);
                await GenerateCommentForSmartScript(ListViewList.SelectedScript);
            }
        }

        private async void textBoxTargetY_Leave(object sender, EventArgs e)
        {
            if (customObjectListView.SelectedObjects.Count > 0)
            {
                textBoxTargetY.Text = textBoxTargetY.Text.Replace(".", ",");
                textBoxTargetY.SelectionStart = textBoxTargetY.Text.Length + 1; //! Set cursor to end of text
                ListViewList.SelectedScript.target_y = textBoxTargetY.Text;
                ListViewList.ReplaceScript(ListViewList.SelectedScript);
                await GenerateCommentForSmartScript(ListViewList.SelectedScript);
            }
        }

        private async void textBoxTargetZ_Leave(object sender, EventArgs e)
        {
            if (customObjectListView.SelectedObjects.Count > 0)
            {
                textBoxTargetZ.Text = textBoxTargetZ.Text.Replace(".", ",");
                textBoxTargetZ.SelectionStart = textBoxTargetZ.Text.Length + 1; //! Set cursor to end of text
                ListViewList.SelectedScript.target_z = textBoxTargetZ.Text;
                ListViewList.ReplaceScript(ListViewList.SelectedScript);
                await GenerateCommentForSmartScript(ListViewList.SelectedScript);
            }
        }

        private async void textBoxTargetO_Leave(object sender, EventArgs e)
        {
            if (customObjectListView.SelectedObjects.Count > 0)
            {
                textBoxTargetO.Text = textBoxTargetO.Text.Replace(".", ",");
                textBoxTargetO.SelectionStart = textBoxTargetO.Text.Length + 1; //! Set cursor to end of text
                ListViewList.SelectedScript.target_o = textBoxTargetO.Text;
                ListViewList.ReplaceScript(ListViewList.SelectedScript);
                await GenerateCommentForSmartScript(ListViewList.SelectedScript);
            }
        }

        private async void textBoxEntryOrGuid_TextChanged(object sender, EventArgs e)
        {
            pictureBoxLoadScript.Enabled = textBoxEntryOrGuid.Text.Length > 0 && Settings.Default.UseWorldDatabase;
            pictureBoxCreateScript.Enabled = textBoxEntryOrGuid.Text.Length > 0;
            buttonNewLine.Enabled = textBoxEntryOrGuid.Text.Length > 0;

            if (checkBoxAllowChangingEntryAndSourceType.Checked && customObjectListView.SelectedObjects.Count > 0)
            {
                ListViewList.SelectedScript.entryorguid = CustomConverter.ToInt32(textBoxEntryOrGuid.Text);
                ListViewList.ReplaceScript(ListViewList.SelectedScript);
                await GenerateCommentForSmartScript(ListViewList.SelectedScript);

                //! When all entryorguids are the same, also adjust the originalEntryOrGuid data
                List<EntryOrGuidAndSourceType> uniqueEntriesOrGuidsAndSourceTypes = SAI_Editor_Manager.Instance.GetUniqueEntriesOrGuidsAndSourceTypes(ListViewList.SmartScripts);

                if (uniqueEntriesOrGuidsAndSourceTypes != null && uniqueEntriesOrGuidsAndSourceTypes.Count == 1)
                {
                    originalEntryOrGuidAndSourceType.entryOrGuid = uniqueEntriesOrGuidsAndSourceTypes[0].entryOrGuid;
                    originalEntryOrGuidAndSourceType.sourceType = uniqueEntriesOrGuidsAndSourceTypes[0].sourceType;
                }
            }
        }

        private async void comboBoxSourceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SourceTypes newSourceType = GetSourceTypeByIndex();

            if (customObjectListView.Items.Count == 0)
                textBoxComments.Text = SAI_Editor_Manager.Instance.GetDefaultCommentForSourceType(newSourceType);

            if (checkBoxAllowChangingEntryAndSourceType.Checked && customObjectListView.SelectedObjects.Count > 0)
            {
                ListViewList.SelectedScript.source_type = (int)newSourceType;
                ListViewList.ReplaceScript(ListViewList.SelectedScript);
                await GenerateCommentForSmartScript(ListViewList.SelectedScript);
            }

            //! When no database connection can be made, only enable the search button if
            //! we're searching for areatriggers.
            buttonSearchForEntryOrGuid.Enabled = Settings.Default.UseWorldDatabase || newSourceType == SourceTypes.SourceTypeAreaTrigger;
        }

        public async Task<string> GenerateSmartAiSqlFromListView()
        {
            List<EntryOrGuidAndSourceType> entriesOrGuidsAndSourceTypes = SAI_Editor_Manager.Instance.GetUniqueEntriesOrGuidsAndSourceTypes(ListViewList.SmartScripts);
            string generatedSql = String.Empty, sourceName = String.Empty;

            Dictionary<SourceTypes, List<EntryOrGuidAndSourceType>> entriesOrGuidsAndSourceTypesPerSourceType = new Dictionary<SourceTypes, List<EntryOrGuidAndSourceType>>();

            if (entriesOrGuidsAndSourceTypes.Count > 1)
            {
                foreach (EntryOrGuidAndSourceType entryOrGuidAndSourceType in entriesOrGuidsAndSourceTypes)
                {
                    if (!entriesOrGuidsAndSourceTypesPerSourceType.ContainsKey(entryOrGuidAndSourceType.sourceType))
                    {
                        List<EntryOrGuidAndSourceType> _newEntryOrGuidAndSourceType = new List<EntryOrGuidAndSourceType>();
                        _newEntryOrGuidAndSourceType.Add(entryOrGuidAndSourceType);
                        entriesOrGuidsAndSourceTypesPerSourceType[entryOrGuidAndSourceType.sourceType] = _newEntryOrGuidAndSourceType;
                    }
                    else
                        entriesOrGuidsAndSourceTypesPerSourceType[entryOrGuidAndSourceType.sourceType].Add(entryOrGuidAndSourceType);
                }
            }

            switch (originalEntryOrGuidAndSourceType.sourceType)
            {
                case SourceTypes.SourceTypeCreature:
                case SourceTypes.SourceTypeGameobject:
                    if (!Settings.Default.UseWorldDatabase)
                    {
                        sourceName = " <Could not generate name>";
                        break;
                    }

                    sourceName = " " + await SAI_Editor_Manager.Instance.worldDatabase.GetObjectNameByIdOrGuidAndSourceType(originalEntryOrGuidAndSourceType.sourceType, originalEntryOrGuidAndSourceType.entryOrGuid);
                    break;
                case SourceTypes.SourceTypeAreaTrigger:
                    sourceName = " Areatrigger";
                    break;
                case SourceTypes.SourceTypeScriptedActionlist:
                    if (entriesOrGuidsAndSourceTypes.Count > 1)
                    {
                        if (!Settings.Default.UseWorldDatabase)
                        {
                            sourceName = " <Could not generate name>";
                            break;
                        }

                        foreach (List<EntryOrGuidAndSourceType> listEntryOrGuidAndSourceTypes in entriesOrGuidsAndSourceTypesPerSourceType.Values)
                        {
                            foreach (EntryOrGuidAndSourceType entryOrGuidAndSourceType in listEntryOrGuidAndSourceTypes)
                            {
                                if (entryOrGuidAndSourceType.sourceType != SourceTypes.SourceTypeGameobject && entryOrGuidAndSourceType.sourceType != SourceTypes.SourceTypeCreature)
                                    continue;

                                sourceName = " " + await SAI_Editor_Manager.Instance.worldDatabase.GetObjectNameByIdOrGuidAndSourceType(entryOrGuidAndSourceType.sourceType, entryOrGuidAndSourceType.entryOrGuid);
                                break;
                            }
                        }
                    }
                    else
                        sourceName = " Actionlist";

                    break;
                default:
                    sourceName = " <Could not generate name>";
                    break;
            }

            bool originalEntryIsGuid = originalEntryOrGuidAndSourceType.entryOrGuid < 0;
            string sourceSet = originalEntryIsGuid ? "@GUID" : "@ENTRY";

            generatedSql += "--" + sourceName + " SAI\n";
            generatedSql += "SET " + sourceSet + " := " + originalEntryOrGuidAndSourceType.entryOrGuid + ";\n";

            if (entriesOrGuidsAndSourceTypes.Count == 1)
            {
                switch (originalEntryOrGuidAndSourceType.sourceType)
                {
                    case SourceTypes.SourceTypeCreature:
                        if (!Settings.Default.UseWorldDatabase)
                        {
                            generatedSql += "-- No changes to the AIName were made because there is no world database connection.\n";
                            break;
                        }

                        if (originalEntryIsGuid)
                        {
                            int actualEntry = await SAI_Editor_Manager.Instance.worldDatabase.GetCreatureIdByGuid(-originalEntryOrGuidAndSourceType.entryOrGuid);
                            generatedSql += "UPDATE `creature_template` SET `AIName`=" + '"' + "SmartAI" + '"' + " WHERE `entry`=" + actualEntry + ";\n";
                        }
                        else
                            generatedSql += "UPDATE `creature_template` SET `AIName`=" + '"' + "SmartAI" + '"' + " WHERE `entry`=" + sourceSet + ";\n";

                        break;
                    case SourceTypes.SourceTypeGameobject:
                        if (!Settings.Default.UseWorldDatabase)
                        {
                            generatedSql += "-- No changes to the AIName were made because there is no world database connection.\n";
                            break;
                        }

                        if (originalEntryIsGuid)
                        {
                            int actualEntry = await SAI_Editor_Manager.Instance.worldDatabase.GetGameobjectIdByGuid(-originalEntryOrGuidAndSourceType.entryOrGuid);
                            generatedSql += "UPDATE `gameobject_template` SET `AIName`=" + '"' + "SmartGameObjectAI" + '"' + " WHERE `entry`=" + actualEntry + ";\n";
                        }
                        else
                            generatedSql += "UPDATE `gameobject_template` SET `AIName`=" + '"' + "SmartGameObjectAI" + '"' + " WHERE `entry`=" + sourceSet + ";\n";

                        break;
                    case SourceTypes.SourceTypeAreaTrigger:
                        generatedSql += "DELETE FROM `areatrigger_scripts` WHERE `entry`=" + sourceSet + ";\n";
                        generatedSql += "INSERT INTO `areatrigger_scripts` (`entry`,`ScriptName`) VALUES (" + sourceSet + "," + '"' + "SmartTrigger" + '"' + ");\n";
                        break;
                    case SourceTypes.SourceTypeScriptedActionlist:
                        // todo
                        break;
                }

                generatedSql += "DELETE FROM `smart_scripts` WHERE `entryorguid`=" + sourceSet + " AND `source_type`=" + (int)originalEntryOrGuidAndSourceType.sourceType + ";\n";
            }
            else if (entriesOrGuidsAndSourceTypes.Count > 1)
            {
                foreach (List<EntryOrGuidAndSourceType> listEntryOrGuidAndSourceTypes in entriesOrGuidsAndSourceTypesPerSourceType.Values)
                {
                    bool generatedInitialUpdateQuery = false;

                    for (int i = 0; i < listEntryOrGuidAndSourceTypes.Count; ++i)
                    {
                        EntryOrGuidAndSourceType entryOrGuidAndSourceType = listEntryOrGuidAndSourceTypes[i];

                        switch (entryOrGuidAndSourceType.sourceType)
                        {
                            case SourceTypes.SourceTypeCreature:
                            case SourceTypes.SourceTypeGameobject:
                                string entryOrGuidToUse = entryOrGuidAndSourceType.entryOrGuid.ToString();
                                bool sourceTypeIsCreature = entryOrGuidAndSourceType.sourceType == SourceTypes.SourceTypeCreature;
                                string tableToTarget = sourceTypeIsCreature ? "creature_template" : "gameobject_template";
                                string newAiName = sourceTypeIsCreature ? "SmartAI" : "SmartGameObjectAI";

                                if (entryOrGuidAndSourceType.entryOrGuid == originalEntryOrGuidAndSourceType.entryOrGuid)
                                    entryOrGuidToUse = sourceSet;

                                if (entryOrGuidAndSourceType.entryOrGuid < 0)
                                {
                                    if (!Settings.Default.UseWorldDatabase)
                                    {
                                        generatedSql += "-- No changes to the AIName were made because there is no world database connection.";
                                        break;
                                    }

                                    entryOrGuidToUse = (await SAI_Editor_Manager.Instance.worldDatabase.GetObjectIdByGuidAndSourceType(-entryOrGuidAndSourceType.entryOrGuid, (int)entryOrGuidAndSourceType.sourceType)).ToString();

                                    if (entryOrGuidToUse == "0")
                                    {
                                        string sourceTypeString = SAI_Editor_Manager.Instance.GetSourceTypeString(entryOrGuidAndSourceType.sourceType).ToLower();
                                        string message = "While generating a script for your SmartAI, the " + sourceTypeString + " guid ";
                                        message += -entryOrGuidAndSourceType.entryOrGuid + " was not spawned in your current database which means the AIName was not properly set.";
                                        message += "\n\nThis is only a warning, which means the AIName of entry 0 will be set in `" + sourceTypeString + "_template` and this has no effect.";
                                        MessageBox.Show(message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }

                                if (listEntryOrGuidAndSourceTypes.Count > 1)
                                {
                                    if (!generatedInitialUpdateQuery)
                                    {
                                        generatedSql += "UPDATE `" + tableToTarget + "` SET `AIName`=" + '"' + newAiName + '"' + " WHERE `entry` IN (" + entryOrGuidToUse;
                                        generatedInitialUpdateQuery = true;
                                    }
                                    else
                                        generatedSql += "," + entryOrGuidToUse;

                                    if (i == listEntryOrGuidAndSourceTypes.Count - 1)
                                        generatedSql += ");\n";
                                }
                                else
                                    generatedSql += "UPDATE `" + tableToTarget + "` SET `AIName`=" + '"' + newAiName + '"' + " WHERE `entry`=" + entryOrGuidToUse + ";\n";

                                break;
                            case SourceTypes.SourceTypeAreaTrigger:
                                generatedSql += "DELETE FROM `areatrigger_scripts` WHERE `entry`=" + sourceSet + ";\n";
                                generatedSql += "INSERT INTO `areatrigger_scripts` VALUES (" + sourceSet + "," + '"' + "SmartTrigger" + '"' + ");\n";
                                break;
                            case SourceTypes.SourceTypeScriptedActionlist:
                                // todo
                                break;
                        }
                    }
                }

                foreach (List<EntryOrGuidAndSourceType> listEntryOrGuidAndSourceTypes in entriesOrGuidsAndSourceTypesPerSourceType.Values)
                {
                    //! If there are more entries
                    if (listEntryOrGuidAndSourceTypes.Count > 1)
                    {
                        generatedSql += "DELETE FROM `smart_scripts` WHERE `entryorguid` IN (";
                        SourceTypes sourceTypeOfSource = SourceTypes.SourceTypeNone;

                        for (int i = 0; i < listEntryOrGuidAndSourceTypes.Count; ++i)
                        {
                            EntryOrGuidAndSourceType entryOrGuidAndSourceType = listEntryOrGuidAndSourceTypes[i];
                            sourceTypeOfSource = entryOrGuidAndSourceType.sourceType;

                            if (entryOrGuidAndSourceType.entryOrGuid == originalEntryOrGuidAndSourceType.entryOrGuid)
                                generatedSql += sourceSet;
                            else
                                generatedSql += entryOrGuidAndSourceType.entryOrGuid;

                            if (i != listEntryOrGuidAndSourceTypes.Count - 1)
                                generatedSql += ",";
                            else
                                generatedSql += ")";
                        }

                        generatedSql += " AND `source_type`=" + (int)sourceTypeOfSource + ";\n";
                    }
                    else if (listEntryOrGuidAndSourceTypes.Count == 1)
                    {
                        generatedSql += "DELETE FROM `smart_scripts` WHERE `entryorguid`=";

                        if (listEntryOrGuidAndSourceTypes[0].entryOrGuid == originalEntryOrGuidAndSourceType.entryOrGuid)
                            generatedSql += sourceSet;
                        else
                            generatedSql += listEntryOrGuidAndSourceTypes[0].entryOrGuid;

                        generatedSql += " AND `source_type`=" + (int)listEntryOrGuidAndSourceTypes[0].sourceType + ";\n";
                    }
                    else
                        generatedSql += "-- No 'DELETE FROM `smart_scripts` WHERE ...' query could be generated as the size of listEntryOrGuidAndSourceTypes is not correct (" + listEntryOrGuidAndSourceTypes.Count + ").";
                }
            }

            generatedSql += "INSERT INTO `smart_scripts` (`entryorguid`,`source_type`,`id`,`link`,`event_type`,`event_phase_mask`,`event_chance`,`event_flags`,`event_param1`,`event_param2`,`event_param3`,`event_param4`,`action_type`,`action_param1`,`action_param2`,`action_param3`,`action_param4`,`action_param5`,`action_param6`,`target_type`,`target_param1`,`target_param2`,`target_param3`,`target_x`,`target_y`,`target_z`,`target_o`,`comment`) VALUES\n";

            List<SmartScript> smartScripts = ListViewList.SmartScripts;
            smartScripts = smartScripts.OrderBy(smartScript => smartScript.entryorguid).ToList();

            for (int i = 0; i < smartScripts.Count; ++i)
            {
                SmartScript smartScript = smartScripts[i];
                string actualSourceSet = sourceSet;

                if (originalEntryOrGuidAndSourceType.entryOrGuid != smartScripts[i].entryorguid)
                    actualSourceSet = smartScripts[i].entryorguid.ToString();

                int[] eventParameters = new int[4];
                eventParameters[0] = smartScript.event_param1;
                eventParameters[1] = smartScript.event_param2;
                eventParameters[2] = smartScript.event_param3;
                eventParameters[3] = smartScript.event_param4;

                int[] actionParameters = new int[6];
                actionParameters[0] = smartScript.action_param1;
                actionParameters[1] = smartScript.action_param2;
                actionParameters[2] = smartScript.action_param3;
                actionParameters[3] = smartScript.action_param4;
                actionParameters[4] = smartScript.action_param5;
                actionParameters[5] = smartScript.action_param6;

                int[] targetParameters = new int[3];
                targetParameters[0] = smartScript.target_param1;
                targetParameters[1] = smartScript.target_param2;
                targetParameters[2] = smartScript.target_param3;

                for (int x = 0; x < 6; ++x)
                {
                    if (x < 4)
                        if (eventParameters[x].ToString() == sourceSet)
                            eventParameters[x] = CustomConverter.ToInt32(sourceSet);

                    if (actionParameters[x].ToString() == sourceSet)
                        actionParameters[x] = CustomConverter.ToInt32(sourceSet);

                    if (x < 3)
                        if (targetParameters[x].ToString() == sourceSet)
                            targetParameters[x] = CustomConverter.ToInt32(sourceSet);
                }

                //! SQL accepts a period instead of a comma for float/double values
                string target_x = smartScript.target_x.Replace(",", ".");
                string target_y = smartScript.target_y.Replace(",", ".");
                string target_z = smartScript.target_z.Replace(",", ".");
                string target_o = smartScript.target_o.Replace(",", ".");

                generatedSql += "(" + actualSourceSet + "," + smartScript.source_type + "," + smartScript.id + "," + smartScript.link + "," + smartScript.event_type + "," +
                                              smartScript.event_phase_mask + "," + smartScript.event_chance + "," + smartScript.event_flags + "," + eventParameters[0] + "," +
                                              eventParameters[1] + "," + eventParameters[2] + "," + eventParameters[3] + "," + smartScript.action_type + "," +
                                              actionParameters[0] + "," + actionParameters[1] + "," + actionParameters[2] + "," + actionParameters[3] + "," +
                                              actionParameters[4] + "," + actionParameters[5] + "," + smartScript.target_type + "," + targetParameters[0] + "," +
                                              targetParameters[1] + "," + targetParameters[2] + "," + target_x + "," + target_y + "," + target_z + "," + target_o + "," +
                                              '"' + smartScript.comment + '"' + ")";

                if (i == smartScripts.Count - 1)
                    generatedSql += ";";
                else
                    generatedSql += ",";

                generatedSql += "\n"; //! White line at end of script to make it easier to select
            }

            //! Replaces '<entry>0[id]' ([id] being like 00, 01, 02, 03, etc.) by '@ENTRY*100+03' for example.
            //! Example: replaces 2891401 by @ENTRY*100+01 if original entryorguid is 28914.
            for (int i = 0; i < 50; ++i) // Regex.Matches(generatedSql, originalEntryOrGuidAndSourceType.entryOrGuid + "0" + i.ToString()).Count
            {
                string[] charactersToReplace = new string[3] { ",", ")", " " };

                for (int j = 0; j < 3; ++j)
                {
                    string characterToReplace = charactersToReplace[j];
                    string stringToReplace = originalEntryOrGuidAndSourceType.entryOrGuid + "0" + i.ToString() + characterToReplace;

                    if (!generatedSql.Contains(stringToReplace))
                    {
                        stringToReplace = originalEntryOrGuidAndSourceType.entryOrGuid + "0" + i.ToString() + ")";

                        if (!generatedSql.Contains(stringToReplace))
                            continue;
                    }

                    string _i = i.ToString();

                    if (i < 10)
                        _i = "0" + _i.Substring(0);

                    generatedSql = generatedSql.Replace(stringToReplace, sourceSet + "*100+" + _i + characterToReplace);
                }
            }

            return generatedSql;
        }

        public async Task<string> GenerateSmartAiRevertQuery()
        {
            if (!Settings.Default.UseWorldDatabase)
                return String.Empty;

            string revertQuery = String.Empty;
            List<EntryOrGuidAndSourceType> entriesOrGuidsAndSourceTypes = SAI_Editor_Manager.Instance.GetUniqueEntriesOrGuidsAndSourceTypes(ListViewList.SmartScripts);

            foreach (EntryOrGuidAndSourceType entryOrGuidAndSourceType in entriesOrGuidsAndSourceTypes)
            {
                List<SmartScript> smartScripts = await SAI_Editor_Manager.Instance.worldDatabase.GetSmartScripts(entryOrGuidAndSourceType.entryOrGuid, (int)entryOrGuidAndSourceType.sourceType);
                string scriptName = String.Empty, aiName = String.Empty;

                switch (entryOrGuidAndSourceType.sourceType)
                {
                    case SourceTypes.SourceTypeCreature:
                        scriptName = await SAI_Editor_Manager.Instance.worldDatabase.GetCreatureScriptNameById(entryOrGuidAndSourceType.entryOrGuid);
                        aiName = await SAI_Editor_Manager.Instance.worldDatabase.GetCreatureAiNameById(entryOrGuidAndSourceType.entryOrGuid);

                        revertQuery += "UPDATE creature_template SET Ainame='" + aiName + "',Scriptname='" + scriptName + "' WHERE entry=" + entryOrGuidAndSourceType.entryOrGuid + ";";
                        break;
                    case SourceTypes.SourceTypeGameobject:
                        scriptName = await SAI_Editor_Manager.Instance.worldDatabase.GetGameobjectScriptNameById(entryOrGuidAndSourceType.entryOrGuid);
                        aiName = await SAI_Editor_Manager.Instance.worldDatabase.GetGameobjectAiNameById(entryOrGuidAndSourceType.entryOrGuid);

                        revertQuery += "UPDATE gameobject_template SET Ainame='" + aiName + "',Scriptname='" + await SAI_Editor_Manager.Instance.worldDatabase.GetGameobjectScriptNameById(entryOrGuidAndSourceType.entryOrGuid) + "' WHERE entry=" + entryOrGuidAndSourceType.entryOrGuid + ";";
                        break;
                    case SourceTypes.SourceTypeAreaTrigger:
                        scriptName = await SAI_Editor_Manager.Instance.worldDatabase.GetAreaTriggerScriptNameById(entryOrGuidAndSourceType.entryOrGuid);

                        if (scriptName != String.Empty)
                            revertQuery += "UPDATE areatrigger_scripts SET Scriptname='" + scriptName + "' WHERE entry=" + entryOrGuidAndSourceType.entryOrGuid + ";";
                        else
                            revertQuery += "DELETE FROM areatrigger_scripts WHERE entry=" + entryOrGuidAndSourceType.entryOrGuid + ";";

                        break;
                }

                if (smartScripts != null && smartScripts.Count > 0)
                {
                    revertQuery += "DELETE FROM smart_scripts WHERE entryorguid=" + smartScripts[0].entryorguid.ToString() + ";";
                    revertQuery += "REPLACE INTO smart_scripts VALUES ";

                    for (int i = 0; i < smartScripts.Count; ++i)
                    {
                        SmartScript smartScript = smartScripts[i];
                        revertQuery += "(";
                        revertQuery += String.Format("{0},{1},{2},{3},", smartScript.entryorguid, smartScript.source_type, smartScript.id, smartScript.link);
                        revertQuery += String.Format("{0},{1},{2},{3},", smartScript.event_type, smartScript.event_phase_mask, smartScript.event_chance, smartScript.event_flags);
                        revertQuery += String.Format("{0},{1},{2},{3},", smartScript.event_param1, smartScript.event_param2, smartScript.event_param3, smartScript.event_param4);
                        revertQuery += String.Format("{0},{1},{2},{3},", smartScript.action_type, smartScript.action_param1, smartScript.action_param2, smartScript.action_param3);
                        revertQuery += String.Format("{0},{1},{2},{3},", smartScript.action_param4, smartScript.action_param5, smartScript.action_param6, smartScript.target_type);
                        revertQuery += String.Format("{0},{1},{2},{3},", smartScript.target_param1, smartScript.target_param2, smartScript.target_param3, smartScript.target_x);
                        revertQuery += String.Format("{0},{1},{2}," + '"' + "{3}" + '"', smartScript.target_y, smartScript.target_z, smartScript.target_o, smartScript.comment);
                        revertQuery += ")";

                        if (i == smartScripts.Count - 1)
                            revertQuery += ";";
                        else
                            revertQuery += ",";
                    }
                }
                else
                    revertQuery += "DELETE FROM smart_scripts WHERE entryorguid=" + entryOrGuidAndSourceType.entryOrGuid + ";";
            }

            return revertQuery;
        }

        public async void GenerateCommentsForAllItems()
        {
            if (ListViewList.SmartScripts.Count == 0)
                return;

            for (int i = 0; i < ListViewList.SmartScripts.Count; ++i)
            {
                SmartScript smartScript = ListViewList.SmartScripts[i];
                string newComment = await CommentGenerator.Instance.GenerateCommentFor(smartScript, originalEntryOrGuidAndSourceType, true, GetInitialSmartScriptLink(smartScript));
                smartScript.comment = newComment;
                ListViewList.ReplaceScript(smartScript);
                FillFieldsBasedOnSelectedScript();
            }

            textBoxComments.Text = ListViewList.SelectedScript.comment;
        }

        public void buttonGenerateComments_Click(object sender, EventArgs e)
        {
            if (!Settings.Default.UseWorldDatabase)
                return;

            GenerateCommentsForAllItems();
            ResizeColumns();
            customObjectListView.Select();
        }

        public void ResizeColumns()
        {
            foreach (OLVColumn header in customObjectListView.AllColumns)
                header.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private async Task GenerateCommentForSmartScript(SmartScript smartScript, bool resize = true)
        {
            if (smartScript == null || !Settings.Default.GenerateComments)
                return;

            string newComment = smartScript.comment;

            if (!updatingFieldsBasedOnSelectedScript)
            {
                newComment = await CommentGenerator.Instance.GenerateCommentFor(smartScript, originalEntryOrGuidAndSourceType, true, GetInitialSmartScriptLink(smartScript));

                if (smartScript.link != 0 && (SmartEvent)smartScript.event_type != SmartEvent.SMART_EVENT_LINK)
                    await GenerateCommentForAllEventsLinkingFromSmartScript(smartScript);
            }

            //! For some reason we have to re-check it here...
            if (customObjectListView.SelectedObjects.Count == 0)
                return;

            string oldComment = smartScript.comment;
            smartScript.comment = newComment;
            ListViewList.ReplaceScript(smartScript);

            if (!updatingFieldsBasedOnSelectedScript)
                FillFieldsBasedOnSelectedScript();

            if (oldComment != newComment)
                ResizeColumns();
        }

        private async Task GenerateCommentForAllEventsLinkingFromSmartScript(SmartScript smartScript)
        {
            if (smartScript == null || !Settings.Default.GenerateComments || smartScript.link == 0)
                return;

            List<SmartScript> smartScriptsLinkedFrom = GetAllSmartScriptThatLinkFrom(smartScript);

            if (smartScriptsLinkedFrom == null || smartScriptsLinkedFrom.Count == 0)
                return;

            for (int i = 0; i < smartScriptsLinkedFrom.Count; ++i)
            {
                SmartScript smartScriptListView = smartScriptsLinkedFrom[i];

                if (smartScriptListView.entryorguid != smartScript.entryorguid)
                    continue;

                smartScriptListView.comment = await CommentGenerator.Instance.GenerateCommentFor(smartScriptListView, originalEntryOrGuidAndSourceType, true, smartScript);
                ListViewList.ReplaceScript(smartScriptListView);
            }
        }

        private async Task GenerateCommentForAllEventsLinkingFromAndToSmartScript(SmartScript smartScript)
        {
            if (smartScript == null || !Settings.Default.GenerateComments)
                return;

            for (int i = 0; i < ListViewList.SmartScripts.Count; ++i)
            {
                SmartScript smartScriptListView = ListViewList.SmartScripts[i];

                if (smartScriptListView.entryorguid != smartScript.entryorguid)
                    continue;

                if (smartScript.link == smartScriptListView.id)
                {
                    smartScriptListView.comment = await CommentGenerator.Instance.GenerateCommentFor(smartScriptListView, originalEntryOrGuidAndSourceType, true, GetInitialSmartScriptLink(smartScriptListView));
                    ListViewList.ReplaceScript(smartScriptListView);
                }
                else if (smartScriptListView.link == smartScript.id)
                {
                    smartScript.comment = await CommentGenerator.Instance.GenerateCommentFor(smartScript, originalEntryOrGuidAndSourceType, true, GetInitialSmartScriptLink(smartScript));
                    ListViewList.ReplaceScript(smartScript);
                }
            }
        }

        private SmartScript GetInitialSmartScriptLink(SmartScript smartScript)
        {
            if (smartScript == null || (SmartEvent)smartScript.event_type != SmartEvent.SMART_EVENT_LINK)
                return null;

            SmartScript smartScriptLink = null;
            int idToCheck = smartScript.id;

            GetLinkForCurrentSmartScriptLink(idToCheck, ref smartScriptLink);

            while (smartScriptLink != null && (SmartEvent)smartScriptLink.event_type == SmartEvent.SMART_EVENT_LINK)
            {
                idToCheck = smartScriptLink.id;
                smartScriptLink = null;
                GetLinkForCurrentSmartScriptLink(idToCheck, ref smartScriptLink);
            }

            return smartScriptLink;
        }

        private SmartScript GetLinkForCurrentSmartScriptLink(int idToCheck, ref SmartScript smartScriptLink)
        {
            foreach (SmartScript smartScriptInListView in ListViewList.SmartScripts)
            {
                if (smartScriptInListView.link == idToCheck)
                {
                    smartScriptLink = smartScriptInListView;
                    break;
                }
            }

            return null;
        }

        //! MUST take initial smartscript of linkings
        private List<SmartScript> GetAllSmartScriptThatLinkFrom(SmartScript smartScriptInitial)
        {
            if (smartScriptInitial == null || smartScriptInitial.link == 0)
                return null;

            List<SmartScript> smartScriptsLinking = new List<SmartScript>();
            smartScriptsLinking.Add(smartScriptInitial);
            SmartScript lastInitialSmartScript = smartScriptInitial;

            foreach (SmartScript smartScriptInListView in ListViewList.SmartScripts)
            {
                if ((SmartEvent)smartScriptInListView.event_type != SmartEvent.SMART_EVENT_LINK)
                    continue;

                if (smartScriptInListView.id == lastInitialSmartScript.link)
                {
                    smartScriptsLinking.Add(smartScriptInListView);
                    lastInitialSmartScript = smartScriptInListView;
                }
            }

            return smartScriptsLinking;
        }

        public void checkBoxShowBasicInfo_CheckedChanged(object sender, EventArgs e)
        {
            HandleShowBasicInfo();
            ResizeColumns();
        }

        public void HandleShowBasicInfo(bool calledAfterNewScriptAdded = false)
        {
            //int prevSelectedIndex = customObjectListView.SelectedObjects.Count > 0 ? customObjectListView.SelectedObjects[0].Index : 0;

            List<string> properties = new List<string>();

            properties.Add("event_phase_mask");
            properties.Add("event_chance");
            properties.Add("event_flags");
            properties.Add("event_param1");
            properties.Add("event_param2");
            properties.Add("event_param3");
            properties.Add("event_param4");
            properties.Add("action_param1");
            properties.Add("action_param2");
            properties.Add("action_param3");
            properties.Add("action_param4");
            properties.Add("action_param5");
            properties.Add("action_param6");
            properties.Add("target_param1");
            properties.Add("target_param2");
            properties.Add("target_param3");
            properties.Add("target_x");
            properties.Add("target_y");
            properties.Add("target_z");
            properties.Add("target_o");

            //TODO: Performance check
            if (checkBoxShowBasicInfo.Checked)
                customObjectListView.List.ExcludeProperties(properties);
            else
                ListViewList.IncludeProperties(properties);

            if (!calledAfterNewScriptAdded && customObjectListView.Items.Count > 0)
            {
                object obj = customObjectListView.SelectedObjects.Count > 0 ? customObjectListView.SelectedObjects[0] : customObjectListView.Objects.Cast<object>().ElementAt(0);

                if (customObjectListView.SelectedObjects.Count == 0)
                {
                    customObjectListView.HideSelection = false;
                    customObjectListView.SelectObject(obj);
                }

                customObjectListView.EnsureModelVisible(obj);
            }

            customObjectListView.Select(); //! Sets the focus on the listview
        }

        public void textBoxEventType_MouseWheel(object sender, MouseEventArgs e)
        {
            int newNumber = CustomConverter.ToInt32(textBoxEventType.Text);

            if (e.Delta > 0)
                newNumber--;
            else
                newNumber++;

            if (newNumber < 0)
                newNumber = 0;

            if (newNumber > (int)SmartEvent.SMART_EVENT_MAX - 1)
                newNumber = (int)SmartEvent.SMART_EVENT_MAX - 1;

            textBoxEventType.Text = newNumber.ToString();
        }

        public void textBoxActionType_MouseWheel(object sender, MouseEventArgs e)
        {
            int newNumber = CustomConverter.ToInt32(textBoxActionType.Text);

            if (e.Delta > 0)
                newNumber--;
            else
                newNumber++;

            if (newNumber < 0)
                newNumber = 0;

            if (newNumber > (int)SmartAction.SMART_ACTION_MAX - 1)
                newNumber = (int)SmartAction.SMART_ACTION_MAX - 1;

            textBoxActionType.Text = newNumber.ToString();
        }

        public void textBoxTargetType_MouseWheel(object sender, MouseEventArgs e)
        {
            int newNumber = CustomConverter.ToInt32(textBoxTargetType.Text);

            if (e.Delta > 0)
                newNumber--;
            else
                newNumber++;

            if (newNumber < 0)
                newNumber = 0;

            if (newNumber > (int)SmartTarget.SMART_TARGET_MAX - 1)
                newNumber = (int)SmartTarget.SMART_TARGET_MAX - 1;

            textBoxTargetType.Text = newNumber.ToString();
        }

        private async void textBoxId_TextChanged(object sender, EventArgs e)
        {
            if (customObjectListView.SelectedObjects.Count > 0)
            {
                ListViewList.SelectedScript.id = CustomConverter.ToInt32(textBoxId.Text);
                ListViewList.ReplaceScript(ListViewList.SelectedScript);
                await GenerateCommentForSmartScript(ListViewList.SelectedScript);
            }
        }

        public void checkBoxUsePhaseColors_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.PhaseHighlighting = false;// checkBoxUsePhaseColors.Checked;
            Settings.Default.Save();

            ListViewList.Apply(true);
        }

        public void checkBoxUseStaticTooltips_CheckedChanged(object sender, EventArgs e)
        {
            if (SAI_Editor_Manager.FormState != FormState.FormStateMain)
                return;

            checkBoxUseStaticTooltips.Enabled = false;
            Settings.Default.ShowTooltipsStaticly = checkBoxUseStaticTooltips.Checked;
            Settings.Default.Save();

            ExpandOrContractToShowStaticTooltips(!checkBoxUseStaticTooltips.Checked);
        }

        public void FinishedExpandingOrContracting(bool expanding)
        {
            panelStaticTooltipTypes.Visible = expanding && Settings.Default.ShowTooltipsStaticly;
            panelStaticTooltipParameters.Visible = expanding && Settings.Default.ShowTooltipsStaticly;
            checkBoxShowBasicInfo.Checked = Settings.Default.ShowBasicInfo;
            checkBoxLockEventId.Checked = Settings.Default.LockSmartScriptId;
            checkBoxListActionlistsOrEntries.Checked = Settings.Default.ListActionLists;
            checkBoxAllowChangingEntryAndSourceType.Checked = Settings.Default.AllowChangingEntryAndSourceType;
            checkBoxUsePhaseColors.Checked = false;// Settings.Default.PhaseHighlighting;
            checkBoxUseStaticTooltips.Checked = Settings.Default.ShowTooltipsStaticly;

            if (expanding)
            {
                if (checkBoxUseStaticTooltips.Checked)
                    ExpandOrContractToShowStaticTooltips(false);

                if (MainForm.radioButtonConnectToMySql.Checked)
                    TryToLoadScript(showErrorIfNoneFound: false);

                ResizeColumns();
            }
        }

        private void customObjectListView_CellRightClick(object sender, BrightIdeasSoftware.CellRightClickEventArgs e)
        {
            if (customObjectListView.FocusedItem.Bounds.Contains(e.Location))
                MainForm.contextMenuStripListView.Show(Cursor.Position);
        }

        public void ExpandOrContractToShowStaticTooltips(bool expand)
        {
            if (expandingListView == expand && contractingListView == !expand)
                return;

            expandingListView = expand;
            contractingListView = !expand;

            if (expand)
                customObjectListViewHeightToChangeTo = customObjectListView.Height + (int)SaiEditorSizes.ListViewHeightContract;
            else
                customObjectListViewHeightToChangeTo = customObjectListView.Height - (int)SaiEditorSizes.ListViewHeightContract;

            timerShowStaticTooltips.Enabled = true;
            checkBoxUseStaticTooltips.Checked = !expand;
            ToolTipHelper.DisableOrEnableAllToolTips(false);

            if (expand)
            {
                panelStaticTooltipTypes.Visible = false;
                panelStaticTooltipParameters.Visible = false;
                customObjectListViewHeightToChangeTo = customObjectListView.Height + (int)SaiEditorSizes.ListViewHeightContract;
                ChangeParameterFieldsBasedOnType();
            }
            else
            {
                customObjectListViewHeightToChangeTo = customObjectListView.Height - (int)SaiEditorSizes.ListViewHeightContract;
                //ChangeParameterFieldsBasedOnType();
            }
        }

        public void timerShowStaticTooltips_Tick(object sender, EventArgs e)
        {
            if (expandingListView)
            {
                if (customObjectListView.Height < customObjectListViewHeightToChangeTo)
                    customObjectListView.Height += expandAndContractSpeedListView;
                else
                {
                    customObjectListView.Height = customObjectListViewHeightToChangeTo;
                    timerShowStaticTooltips.Enabled = false;
                    expandingListView = false;
                    ToolTipHelper.DisableOrEnableAllToolTips(true);
                    checkBoxUseStaticTooltips.Enabled = true;
                }
            }
            else if (contractingListView)
            {
                if (customObjectListView.Height > customObjectListViewHeightToChangeTo)
                    customObjectListView.Height -= expandAndContractSpeedListView;
                else
                {
                    customObjectListView.Height = customObjectListViewHeightToChangeTo;
                    timerShowStaticTooltips.Enabled = false;
                    contractingListView = false;
                    panelStaticTooltipTypes.Visible = true;
                    panelStaticTooltipParameters.Visible = true;
                    checkBoxUseStaticTooltips.Enabled = true;
                }
            }
        }

        public async Task GenerateCommentListView()
        {
            if (ListViewList.SelectedScript == null)
                return;

            for (int i = 0; i < ListViewList.SmartScripts.Count; ++i)
            {
                SmartScript smartScript = ListViewList.SmartScripts[i];

                if (smartScript != ListViewList.SelectedScript)
                    continue;

                string newComment = await CommentGenerator.Instance.GenerateCommentFor(smartScript, originalEntryOrGuidAndSourceType, true, GetInitialSmartScriptLink(smartScript));
                smartScript.comment = newComment;
                ListViewList.ReplaceScript(smartScript);
                FillFieldsBasedOnSelectedScript();
            }

            textBoxComments.Text = ListViewList.SelectedScript.comment;
        }

        public void LoadSelectedEntry()
        {
            if (ListViewList.SelectedScript == null)
                return;

            int entryorguid = ListViewList.SelectedScript.entryorguid;
            SourceTypes source_type = (SourceTypes)ListViewList.SelectedScript.source_type;
            ListViewList.ClearScripts();
            customObjectListView.Items.Clear();
            TryToLoadScript(entryorguid, source_type);
        }

        private async void buttonGenerateSql_Click(object sender, EventArgs e)
        {
            using (SqlOutputForm sqlOutputForm = new SqlOutputForm(await GenerateSmartAiSqlFromListView(), true, await GenerateSmartAiRevertQuery()))
                sqlOutputForm.ShowDialog(this);
        }

        private void checkBoxAllowChangingEntryAndSourceType_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBoxEntryOrGuid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                TryToLoadScript();
        }
    }
}
