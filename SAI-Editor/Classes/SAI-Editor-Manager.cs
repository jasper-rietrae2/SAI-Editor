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

namespace SAI_Editor.Classes
{
    public enum ScriptTypeId
    {
        ScriptTypeEvent,
        ScriptTypeAction,
        ScriptTypeTarget,
    }

    class SAI_Editor_Manager
    {
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
        public SQLiteDatabase sqliteDatabase { get; set; }
        public List<EventTypeInformation> eventTypeInformations;
        public List<ActionTypeInformation> actionTypeInformations;
        public List<TargetTypeInformation> targetTypeInformations;

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
            ResetDatabases();
        }

        public void ResetDatabases()
        {
            ResetWorldDatabase();
            ResetSQLiteDatabase();
        }

        public void ResetWorldDatabase()
        {
            worldDatabase = new WorldDatabase(Settings.Default.Host, Settings.Default.Port, Settings.Default.User, GetPasswordSetting(), Settings.Default.Database);
        }

        public void ResetWorldDatabase(MySqlConnectionStringBuilder _connectionString)
        {
            worldDatabase = new WorldDatabase(_connectionString.Server, _connectionString.Port, _connectionString.UserID, _connectionString.Password, _connectionString.Database);
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

            switch (paramId)
            {
                case 1:
                    return baseTypeInformation != null ? baseTypeInformation.parameterString1 : String.Empty;
                case 2:
                    return baseTypeInformation != null ? baseTypeInformation.parameterString2 : String.Empty;
                case 3:
                    return baseTypeInformation != null ? baseTypeInformation.parameterString3 : String.Empty;
                case 4:
                    return baseTypeInformation != null ? baseTypeInformation.parameterString4 : String.Empty;
                case 5:
                    return baseTypeInformation != null ? baseTypeInformation.parameterString5 : String.Empty;
                case 6:
                    return baseTypeInformation != null ? baseTypeInformation.parameterString6 : String.Empty;
                default:
                    return String.Empty;
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

                WorldDatabase worldDatabase = _worldDatabase ?? Instance.worldDatabase;

                //! Will throw an error message itself if no connection can be made.
                if (!worldDatabase.CanConnectToDatabase(_connectionString))
                    return null;

                var databaseNames = new List<string>();

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
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            universalTime += DateTime.Now.Day + "-";
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
    }
}
