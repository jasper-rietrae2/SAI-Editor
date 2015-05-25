using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using SAI_Editor.Classes.Database;
using SAI_Editor.Classes.Database.Classes;
using SAI_Editor.Enumerators;
using SAI_Editor.Forms;
using SAI_Editor.Properties;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.IO;
using SAI_Editor.Forms.SearchForms;
using System.Diagnostics;

namespace SAI_Editor.Classes
{
    public struct EntryOrGuidAndSourceType
    {
        public EntryOrGuidAndSourceType(int _entryOrGuid, SourceTypes _sourceType) { entryOrGuid = _entryOrGuid; sourceType = _sourceType; }

        public int entryOrGuid;
        public SourceTypes sourceType;
    }

    class SAI_Editor_Manager
    {
        public WowExpansion Expansion = WowExpansion.ExpansionWotlk;

        public WorldDatabase _worldDatabase = null;
        public WorldDatabase worldDatabase
        {
            get
            {
                if (Settings.Default.UseWorldDatabase)
                    return _worldDatabase;

                MessageBox.Show("The world database could not be opened as it was never opened.", "No database!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            set
            {
                _worldDatabase = value;
            }
        }

        public static Dictionary<string, Type> SearchFormsContainer = new Dictionary<string, Type>()
        {
            { "GoFlags", typeof(MultiSelectForm<GoFlags>)},
            { "UnitFlags", typeof(MultiSelectForm<UnitFlags>)},
            { "UnitFlags2", typeof(MultiSelectForm<UnitFlags2>)},
            { "DynamicFlags", typeof(MultiSelectForm<DynamicFlags>)},
            { "NpcFlags", typeof(MultiSelectForm<NpcFlags>)},
            { "UnitBytes1_Flags", typeof(MultiSelectForm<UnitBytes1_Flags>)},
            { "SmartEventFlags", typeof(MultiSelectForm<SmartEventFlags>)},
            { "SmartPhaseMasks", typeof(MultiSelectForm<SmartPhaseMasks>)},
            { "SmartCastFlags", typeof(MultiSelectForm<SmartCastFlags>)},
            { "SmartAiTemplates", typeof(SingleSelectForm<SmartAiTemplates>)},
            { "SmartRespawnCondition", typeof(SingleSelectForm<SmartRespawnCondition>)},
            { "SmartActionlistTimerUpdateType", typeof(SingleSelectForm<SmartActionlistTimerUpdateType>)},
            { "GoStates", typeof(SingleSelectForm<GoStates>)},
            { "ReactState", typeof(SingleSelectForm<ReactState>)},
            { "SheathState", typeof(SingleSelectForm<SheathState>)},
            { "MovementGeneratorType", typeof(SingleSelectForm<MovementGeneratorType>)},
            { "PowerTypes", typeof(SingleSelectForm<PowerTypes>)},
            { "UnitStandStateType", typeof(SingleSelectForm<UnitStandStateType>)},
            { "UnitStandFlags", typeof(MultiSelectForm<UnitStandFlags>)},
            { "TempSummonType", typeof(SingleSelectForm<TempSummonType>)},
            { "SpellEffIndex", typeof(SingleSelectForm<SpellEffIndex>)},
            { "SpellSchools", typeof(SingleSelectForm<SpellSchools>)},
            { "SpellCastResult", typeof(SingleSelectForm<SpellCastResult>)},
            { "SpellCustomErrors", typeof(SingleSelectForm<SpellCustomErrors>)},
            { "ReputationRank", typeof(SingleSelectForm<ReputationRank>)},
            { "DrunkenState", typeof(SingleSelectForm<DrunkenState>)},
            { "InstanceInfo", typeof(SingleSelectForm<InstanceInfo>)},
            { "Classes", typeof(SingleSelectForm<PlayerClasses>)},
            { "Races", typeof(SingleSelectForm<Races>)},
            { "SpawnMask", typeof(MultiSelectForm<SpawnMask>)},
            { "Gender", typeof(SingleSelectForm<Gender>)},
            { "UnitState", typeof(MultiSelectForm<UnitState>)},
            { "CreatureType", typeof(SingleSelectForm<CreatureType>)}, //! SingleSelectForm because the cond checks with == operator
            { "PhaseMasks", typeof(MultiSelectForm<PhaseMasks>)},
            { "TypeID", typeof(SingleSelectForm<TypeID>)},
            { "TypeMask", typeof(MultiSelectForm<TypeMask>)},
            { "CondRelationType", typeof(SingleSelectForm<CondRelationType>)},
            { "ComparisionType", typeof(SingleSelectForm<ComparisionType>)},
            { "ReputationRankMask", typeof(MultiSelectForm<ReputationRankMask>)},
            { "SmartEvent", typeof(SingleSelectForm<SmartEvent>)},
            { "SmartAction", typeof(SingleSelectForm<SmartAction>)},
            { "SmartTarget", typeof(SingleSelectForm<SmartTarget>)},
            { "UnitFieldBytes1Types", typeof(SingleSelectForm<UnitFieldBytes1Types>)},
        };

        public SQLiteDatabase sqliteDatabase { get; set; }
        public List<EventTypeInformation> eventTypeInformations;
        public List<ActionTypeInformation> actionTypeInformations;
        public List<TargetTypeInformation> targetTypeInformations;

        public static FormState FormState = FormState.FormStateLogin;

        public MySqlConnectionStringBuilder connString;

        private static object _lock = new object();
        private static SAI_Editor_Manager _instance;

        public static SAI_Editor_Manager Instance
        {
            get
            {
                lock (_lock)
                    return _instance ?? (_instance = new SAI_Editor_Manager());
            }
        }

        public SAI_Editor_Manager()
        {
            ResetSQLiteDatabase();
        }

        public void ResetDatabases()
        {
            ResetWorldDatabase(false);
            ResetSQLiteDatabase();
        }

        public async Task<DataTable> ExecuteQuery(bool useWorldDatabase, string queryToExecute)
        {
            if (useWorldDatabase)
                return await SAI_Editor_Manager.Instance.worldDatabase.ExecuteQuery(queryToExecute);

            return await SAI_Editor_Manager.Instance.sqliteDatabase.ExecuteQuery(queryToExecute);
        }

        public void ResetWorldDatabase(bool useConnStr)
        {
            if (useConnStr)
                worldDatabase = new WorldDatabase(SAI_Editor_Manager.Instance.connString.Server, SAI_Editor_Manager.Instance.connString.Port, SAI_Editor_Manager.Instance.connString.UserID, SAI_Editor_Manager.Instance.connString.Password, SAI_Editor_Manager.Instance.connString.Database);
            else
                worldDatabase = new WorldDatabase(Settings.Default.Host, Settings.Default.Port, Settings.Default.User, GetPasswordSetting(), Settings.Default.Database);
        }

        public void ResetSQLiteDatabase()
        {
            sqliteDatabase = new SQLiteDatabase("sqlite_database.db");
        }

        public async Task LoadSQLiteDatabaseInfo()
        {
            eventTypeInformations = await sqliteDatabase.GetEventTypeInformation();
            actionTypeInformations = await sqliteDatabase.GetActionTypeInformation();
            targetTypeInformations = await sqliteDatabase.GetTargetTypeInformation();
        }

        private BaseTypeInformation GetTypeByScriptTypeId(int type, ScriptTypeId scriptTypeId)
        {
            switch (scriptTypeId)
            {
                case ScriptTypeId.ScriptTypeEvent:
                    return GetEventTypeInformationById(type);
                case ScriptTypeId.ScriptTypeAction:
                    return GetActionTypeInformationById(type);
                case ScriptTypeId.ScriptTypeTarget:
                    return GetTargetTypeInformationById(type);
                default:
                    return null;
            }
        }

        public EventTypeInformation GetEventTypeInformationById(int event_type)
        {
            if (eventTypeInformations == null)
                return null;

            return eventTypeInformations.FirstOrDefault(eventTypeInformation => eventTypeInformation.event_type == event_type);
        }

        public ActionTypeInformation GetActionTypeInformationById(int action_type)
        {
            if (actionTypeInformations == null)
                return null;

            return actionTypeInformations.FirstOrDefault(actionTypeInformation => actionTypeInformation.action_type == action_type);
        }

        public TargetTypeInformation GetTargetTypeInformationById(int target_type)
        {
            if (targetTypeInformations == null)
                return null;

            return targetTypeInformations.FirstOrDefault(targetTypeInformation => targetTypeInformation.target_type == target_type);
        }

        public string GetScriptTypeTooltipById(int type, ScriptTypeId scriptTypeId)
        {
            BaseTypeInformation baseTypeInformation = GetTypeByScriptTypeId(type, scriptTypeId);
            return baseTypeInformation != null ? baseTypeInformation.tooltip : String.Empty;
        }

        public string GetParameterTooltipById(int type, int paramId, ScriptTypeId scriptTypeId)
        {
            BaseTypeInformation baseTypeInformation = GetTypeByScriptTypeId(type, scriptTypeId);

            switch (paramId)
            {
                case 1:
                    return baseTypeInformation != null ? baseTypeInformation.parameterTooltip1 : String.Empty;
                case 2:
                    return baseTypeInformation != null ? baseTypeInformation.parameterTooltip2 : String.Empty;
                case 3:
                    return baseTypeInformation != null ? baseTypeInformation.parameterTooltip3 : String.Empty;
                case 4:
                    return baseTypeInformation != null ? baseTypeInformation.parameterTooltip4 : String.Empty;
                case 5:
                    return baseTypeInformation != null ? baseTypeInformation.parameterTooltip5 : String.Empty;
                case 6:
                    return baseTypeInformation != null ? baseTypeInformation.parameterTooltip6 : String.Empty;
                default:
                    return String.Empty;
            }
        }

        public string GetParameterStringById(int type, int paramId, ScriptTypeId scriptTypeId)
        {
            BaseTypeInformation baseTypeInformation = GetTypeByScriptTypeId(type, scriptTypeId);

            if (baseTypeInformation == null)
                return "Unused Parameter";

            switch (paramId)
            {
                case 1:
                    return !String.IsNullOrEmpty(baseTypeInformation.parameterString1) ? baseTypeInformation.parameterString1 : "Unused Parameter";
                case 2:
                    return !String.IsNullOrEmpty(baseTypeInformation.parameterString2) ? baseTypeInformation.parameterString2 : "Unused Parameter";
                case 3:
                    return !String.IsNullOrEmpty(baseTypeInformation.parameterString3) ? baseTypeInformation.parameterString3 : "Unused Parameter";
                case 4:
                    return !String.IsNullOrEmpty(baseTypeInformation.parameterString4) ? baseTypeInformation.parameterString4 : "Unused Parameter";
                case 5:
                    return !String.IsNullOrEmpty(baseTypeInformation.parameterString5) ? baseTypeInformation.parameterString5 : "Unused Parameter";
                case 6:
                    return !String.IsNullOrEmpty(baseTypeInformation.parameterString6) ? baseTypeInformation.parameterString6 : "Unused Parameter";
                default:
                    return "Unused Parameter";
            }
        }

        public bool IsNumericString(string str)
        {
            try
            {
                Int32 strInt = Int32.Parse(str);
            }
            catch (FormatException)
            {
                return false;
            }

            return true;
        }

        public async Task<List<EntryOrGuidAndSourceType>> GetTimedActionlistsOrEntries(SmartScript smartScript, SourceTypes sourceType)
        {
            List<EntryOrGuidAndSourceType> timedActionListOrEntries = new List<EntryOrGuidAndSourceType>();

            if (sourceType == SourceTypes.SourceTypeScriptedActionlist)
            {
                List<SmartScript> smartScriptsCallingActionlist = await worldDatabase.GetSmartScriptsCallingActionLists();

                if (smartScriptsCallingActionlist != null)
                {
                    foreach (SmartScript _smartScript in smartScriptsCallingActionlist)
                    {
                        switch ((SmartAction)_smartScript.action_type)
                        {
                            case SmartAction.SMART_ACTION_CALL_TIMED_ACTIONLIST:
                            case SmartAction.SMART_ACTION_CALL_RANDOM_TIMED_ACTIONLIST:
                                if (_smartScript.action_param1 == smartScript.entryorguid ||
                                    _smartScript.action_param2 == smartScript.entryorguid ||
                                    _smartScript.action_param3 == smartScript.entryorguid ||
                                    _smartScript.action_param4 == smartScript.entryorguid ||
                                    _smartScript.action_param5 == smartScript.entryorguid ||
                                    _smartScript.action_param6 == smartScript.entryorguid)
                                {
                                    timedActionListOrEntries.Add(new EntryOrGuidAndSourceType(_smartScript.entryorguid, (SourceTypes)_smartScript.source_type));
                                }

                                break;
                            case SmartAction.SMART_ACTION_CALL_RANDOM_RANGE_TIMED_ACTIONLIST:
                                for (int param = _smartScript.action_param1; param <= _smartScript.action_param2; ++param)
                                {
                                    EntryOrGuidAndSourceType entryOrGuidAndSourceType = new EntryOrGuidAndSourceType(param, (SourceTypes)_smartScript.source_type);

                                    if (param == smartScript.entryorguid && !timedActionListOrEntries.Contains(entryOrGuidAndSourceType))
                                    {
                                        timedActionListOrEntries.Add(entryOrGuidAndSourceType);
                                        break;
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            else
            {
                switch ((SmartAction)smartScript.action_type)
                {
                    case SmartAction.SMART_ACTION_CALL_TIMED_ACTIONLIST:
                        timedActionListOrEntries.Add(new EntryOrGuidAndSourceType(smartScript.action_param1, SourceTypes.SourceTypeScriptedActionlist));
                        break;
                    case SmartAction.SMART_ACTION_CALL_RANDOM_TIMED_ACTIONLIST:
                        timedActionListOrEntries.Add(new EntryOrGuidAndSourceType(smartScript.action_param1, SourceTypes.SourceTypeScriptedActionlist));
                        timedActionListOrEntries.Add(new EntryOrGuidAndSourceType(smartScript.action_param2, SourceTypes.SourceTypeScriptedActionlist));

                        if (smartScript.action_param3 > 0)
                            timedActionListOrEntries.Add(new EntryOrGuidAndSourceType(smartScript.action_param3, SourceTypes.SourceTypeScriptedActionlist));

                        if (smartScript.action_param4 > 0)
                            timedActionListOrEntries.Add(new EntryOrGuidAndSourceType(smartScript.action_param4, SourceTypes.SourceTypeScriptedActionlist));

                        if (smartScript.action_param5 > 0)
                            timedActionListOrEntries.Add(new EntryOrGuidAndSourceType(smartScript.action_param5, SourceTypes.SourceTypeScriptedActionlist));

                        if (smartScript.action_param6 > 0)
                            timedActionListOrEntries.Add(new EntryOrGuidAndSourceType(smartScript.action_param6, SourceTypes.SourceTypeScriptedActionlist));

                        break;
                    case SmartAction.SMART_ACTION_CALL_RANDOM_RANGE_TIMED_ACTIONLIST:
                        for (int param = smartScript.action_param1; param <= smartScript.action_param2; ++param)
                            timedActionListOrEntries.Add(new EntryOrGuidAndSourceType(param, SourceTypes.SourceTypeScriptedActionlist));

                        break;
                }
            }

            return timedActionListOrEntries;
        }

        public Task<List<string>> GetDatabasesInConnection(string host, string username, uint port, string password = "", WorldDatabase _worldDatabase = null)
        {
            return Task.Run(() =>
            {
                if (host.Length <= 0 || username.Length <= 0 || port <= 0)
                {
                    MessageBox.Show("You must fill a host, username and port in order to search for your world database (we need to establish a connection to list your databases)!", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                MySqlConnectionStringBuilder _connectionString = new MySqlConnectionStringBuilder();
                _connectionString.Server = host;
                _connectionString.UserID = username;
                _connectionString.Port = port;

                if (password.Length > 0)
                    _connectionString.Password = password;

                //! Will throw an error message itself if no connection can be made.
                if (!(_worldDatabase ?? Instance.worldDatabase).CanConnectToDatabase(_connectionString))
                    return null;

                List<string> databaseNames = new List<string>();

                try
                {
                    using (var connection = new MySqlConnection(_connectionString.ToString()))
                    {
                        connection.Open();
                        var returnVal = new MySqlDataAdapter("SHOW DATABASES", connection);
                        var dataTable = new DataTable();
                        returnVal.Fill(dataTable);

                        if (dataTable.Rows.Count <= 0)
                        {
                            MessageBox.Show("Your connection contains no databases!", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return null;
                        }

                        foreach (DataRow row in dataTable.Rows) databaseNames.AddRange(row.ItemArray.Select(t => t.ToString()));
                    }
                }
                catch
                {
                    MessageBox.Show("The databases could not be displayed.", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return databaseNames;
            });
        }

        public bool IsAiNameSmartAi(string aiName)
        {
            return aiName == "SmartAI" || aiName == "SmartGameObjectAI";
        }

        public List<EntryOrGuidAndSourceType> GetUniqueEntriesOrGuidsAndSourceTypes(List<SmartScript> smartScripts)
        {
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

            return entriesOrGuidsAndSourceTypes;
        }

        public string GetDefaultCommentForSourceType(SourceTypes sourceType)
        {
            switch (sourceType)
            {
                case SourceTypes.SourceTypeCreature:
                    return "Npc - Event - Action (phase) (dungeon difficulty)";
                case SourceTypes.SourceTypeGameobject:
                    return "Gameobject - Event - Action (phase) (dungeon difficulty)";
                case SourceTypes.SourceTypeAreaTrigger:
                    return "Areatrigger - Event - Action (phase) (dungeon difficulty)";
                case SourceTypes.SourceTypeScriptedActionlist:
                    return "Source - Event - Action (phase) (dungeon difficulty)";
            }

            return String.Empty;
        }

        public string GetPasswordSetting()
        {
            string password = Settings.Default.Password;

            if (password.Length > 150)
                password = password.DecryptString(Encoding.Unicode.GetBytes(Settings.Default.Entropy)).ToInsecureString();

            return password;
        }

        //! Will return something like '27-12-2013 19;55;22'
        public string GetUniversalTimeStamp()
        {
            string universalTime = String.Empty;
            universalTime  = DateTime.Now.Day + "-";
            universalTime += DateTime.Now.Month + "-";
            universalTime += DateTime.Now.Year + " ";
            universalTime += DateTime.Now.Hour + ";";
            universalTime += DateTime.Now.Minute + ";";
            universalTime += DateTime.Now.Second;
            return universalTime;
        }

        public bool HasInternetConnectionWithCurrentNetwork()
        {
            return NetworkInterface.GetAllNetworkInterfaces().Any(x => x.OperationalStatus == OperationalStatus.Up);
        }

        public bool HasInternetConnection()
        {
            try
            {
                using (WebClient client = new WebClient())
                    using (Stream stream = client.OpenRead("http://www.google.com"))
                        return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DoesUrlExist(string url)
        {
            try
            {
                WebRequest req = WebRequest.Create(url);
                req.GetResponse();
                return true;
            }
            catch (WebException)
            {
                return false;
            }
        }

        public bool StartProcess(string filename, string argument = "", bool showException = false)
        {
            bool isWebsiteUrl = filename.Contains("www.") || filename.Contains("http");

            try
            {
                Process.Start(filename, argument);
            }
            catch (Exception ex)
            {
                if (showException)
                    return false;

                DialogResult dialogResult = MessageBox.Show(String.Format("The {0} '{1}' could not be opened! Do you wish to see the error thrown by the application?",
                    isWebsiteUrl ? "website" : "process", Path.GetFileName(filename)), "An error has occurred!", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

                if (dialogResult == DialogResult.Yes)
                    MessageBox.Show(ex.Message, "An exception was thrown!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }

            return true;
        }

        public string GetSourceTypeString(SourceTypes sourceType)
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

        public static string GetPrefixTableName()
        {
            switch (Instance.Expansion)
            {
                case WowExpansion.ExpansionWotlk:
                    return "wotlk";
                case WowExpansion.ExpansionCata:
                    return "cata";
                case WowExpansion.ExpansionMop:
                    return "mop";
                case WowExpansion.ExpansionWod:
                    return "wod";
            }

            return String.Empty;
        }

        public static string GetSpellTableName()
        {
            return "spells_" + GetPrefixTableName();
        }

        public static string GetAreatriggerTableName()
        {
            return "areatriggers_" + GetPrefixTableName();
        }

        public static string GetFactionsTableName()
        {
            return "factions_" + GetPrefixTableName();
        }

        public static string GetEmotesTableName()
        {
            return "emotes_" + GetPrefixTableName();
        }

        public static string GetMapsTableName()
        {
            return "maps_" + GetPrefixTableName();
        }

        public static string GetAreasAndZonesTableName()
        {
            return "areas_and_zones_" + GetPrefixTableName();
        }

        public static string GetSoundEntriesTableName()
        {
            return "sound_entries_" + GetPrefixTableName();
        }

        public static string GetSkillsTableName()
        {
            return "skills_" + GetPrefixTableName();
        }

        public static string GetAchievementsTableName()
        {
            return "achievements_" + GetPrefixTableName();
        }

        public static string GetPlayerTitlesTableName()
        {
            return "player_titles_" + GetPrefixTableName();
        }

        public static string GetTaxiNodesTableName()
        {
            return "taxi_nodes_" + GetPrefixTableName();
        }
    }
}
