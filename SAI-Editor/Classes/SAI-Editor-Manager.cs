using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using SAI_Editor.Database;
using SAI_Editor.Database.Classes;
using SAI_Editor.Properties;
using System.Security.Cryptography;
using SAI_Editor.Security;

namespace SAI_Editor
{
    public struct TimedActionListOrEntries
    {
        public List<string> entries;
        public SourceTypes sourceTypeOfEntry;
    }

    public enum ScriptTypeId
    {
        ScriptTypeEvent,
        ScriptTypeAction,
        ScriptTypeTarget,
    }

    class SAI_Editor_Manager
    {
        public WorldDatabase worldDatabase { get; set; }
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
                {
                    if (_instance == null)
                        _instance = new SAI_Editor_Manager();

                    return _instance;
                }
            }
        }

        public void ResetDatabases()
        {
            string password = Settings.Default.Password;

            if (password.Length > 0)
                password = SecurityExtensions.DecryptString(password, Encoding.Unicode.GetBytes(Settings.Default.Entropy)).ToInsecureString();

            worldDatabase = new WorldDatabase(Settings.Default.Host, Settings.Default.Port, Settings.Default.User, password, Settings.Default.Database);
            sqliteDatabase = new SQLiteDatabase("Resources/sqlite_database.db");
        }

        public SAI_Editor_Manager()
        {
            ResetDatabases();
        }

        public async Task<bool> LoadSQLiteDatabaseInfo()
        {
            eventTypeInformations = await sqliteDatabase.GetEventTypeInformation();
            actionTypeInformations = await sqliteDatabase.GetActionTypeInformation();
            targetTypeInformations = await sqliteDatabase.GetTargetTypeInformation();
            return true;
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

            foreach (EventTypeInformation eventTypeInformation in eventTypeInformations)
                if (eventTypeInformation.event_type == event_type)
                    return eventTypeInformation;

            return null;
        }

        public ActionTypeInformation GetActionTypeInformationById(int action_type)
        {
            if (actionTypeInformations == null)
                return null;

            foreach (ActionTypeInformation actionTypeInformation in actionTypeInformations)
                if (actionTypeInformation.action_type == action_type)
                    return actionTypeInformation;

            return null;
        }

        public TargetTypeInformation GetTargetTypeInformationById(int target_type)
        {
            if (targetTypeInformations == null)
                return null;

            foreach (TargetTypeInformation targetTypeInformation in targetTypeInformations)
                if (targetTypeInformation.target_type == target_type)
                    return targetTypeInformation;

            return null;
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

        public async Task<TimedActionListOrEntries> GetTimedActionlistsOrEntries(SmartScript smartScript, SourceTypes sourceType)
        {
            TimedActionListOrEntries timedActionListOrEntries = new TimedActionListOrEntries();
            timedActionListOrEntries.entries = new List<string>();
            timedActionListOrEntries.sourceTypeOfEntry = SourceTypes.SourceTypeScriptedActionlist;

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
                                    timedActionListOrEntries.entries.Add(_smartScript.entryorguid.ToString());
                                    timedActionListOrEntries.sourceTypeOfEntry = (SourceTypes)_smartScript.source_type;
                                }

                                break;
                            case SmartAction.SMART_ACTION_CALL_RANDOM_RANGE_TIMED_ACTIONLIST:
                                for (int i = _smartScript.action_param1; i <= _smartScript.action_param2; ++i)
                                {
                                    if (i == smartScript.entryorguid && !timedActionListOrEntries.entries.Contains(i.ToString()))
                                    {
                                        timedActionListOrEntries.entries.Add(_smartScript.entryorguid.ToString());
                                        timedActionListOrEntries.sourceTypeOfEntry = (SourceTypes)_smartScript.source_type;
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
                        timedActionListOrEntries.entries.Add(smartScript.action_param1.ToString());
                        break;
                    case SmartAction.SMART_ACTION_CALL_RANDOM_TIMED_ACTIONLIST:
                        timedActionListOrEntries.entries.Add(smartScript.action_param1.ToString());
                        timedActionListOrEntries.entries.Add(smartScript.action_param2.ToString());

                        if (smartScript.action_param3 > 0)
                            timedActionListOrEntries.entries.Add(smartScript.action_param3.ToString());

                        if (smartScript.action_param4 > 0)
                            timedActionListOrEntries.entries.Add(smartScript.action_param4.ToString());

                        if (smartScript.action_param5 > 0)
                            timedActionListOrEntries.entries.Add(smartScript.action_param5.ToString());

                        if (smartScript.action_param6 > 0)
                            timedActionListOrEntries.entries.Add(smartScript.action_param6.ToString());

                        break;
                    case SmartAction.SMART_ACTION_CALL_RANDOM_RANGE_TIMED_ACTIONLIST:
                        for (int i = smartScript.action_param1; i <= smartScript.action_param2; ++i)
                            timedActionListOrEntries.entries.Add(i.ToString());
                        break;
                }
            }

            return timedActionListOrEntries;
        }

        public Task<List<string>> GetDatabasesInConnection(string host, string username, uint port, string password = "")
        {

            return Task.Run(() =>
            {
                if (host.Length <= 0 || username.Length <= 0 || port <= 0)
                {
                    MessageBox.Show("You must fill all fields except for the world database field in order to search for your world database (we need to establish a connection to list your databases)!", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                var databaseNames = new List<string>();

                MySqlConnectionStringBuilder _connectionString = new MySqlConnectionStringBuilder();
                _connectionString.Server = host;
                _connectionString.UserID = username;
                _connectionString.Port = port;

                if (password.Length > 0)
                    _connectionString.Password = password;

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

                        foreach (DataRow row in dataTable.Rows)
                            for (int i = 0; i < row.ItemArray.Length; i++)
                                databaseNames.Add(row.ItemArray[i].ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return databaseNames;

            });

        }
    }
}
