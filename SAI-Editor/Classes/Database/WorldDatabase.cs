using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using SAI_Editor.Database.Classes;

namespace SAI_Editor
{
    class WorldDatabase : Database<MySqlConnection, MySqlConnectionStringBuilder, MySqlParameter, MySqlCommand>
    {
        public WorldDatabase(string host, int port, string username, string password, string databaseName)
        {
            ConnectionString = new MySqlConnectionStringBuilder();
            ConnectionString.Server = host;
            ConnectionString.Port = (uint)port;
            ConnectionString.UserID = username;
            ConnectionString.Password = password;
            ConnectionString.Database = databaseName;
            ConnectionString.AllowUserVariables = true;
            ConnectionString.AllowZeroDateTime = true;
        }

        public async Task<int> GetCreatureIdByGuid(int guid)
        {
            //DataTable dt = await ExecuteQuery("SELECT id FROM creature WHERE guid = '@guid'", new MySqlParameter("@guid", guid));
            DataTable dt = await ExecuteQuery("SELECT id FROM creature WHERE guid = '" + guid + "'");

            if (dt.Rows.Count == 0)
                return 0;

            return Convert.ToInt32(dt.Rows[0]["id"]);
        }

        public async Task<int> GetGameobjectIdByGuid(int guid)
        {
            //DataTable dt = await ExecuteQuery("SELECT id FROM gameobject WHERE guid = '@guid'", new MySqlParameter("@guid", guid));
            DataTable dt = await ExecuteQuery("SELECT id FROM gameobject WHERE guid = '" + guid + "'");

            if (dt.Rows.Count == 0)
                return 0;

            return Convert.ToInt32(dt.Rows[0]["id"]);
        }

        public async Task<int> GetObjectIdByGuidAndSourceType(int guid, int source_type)
        {
            switch ((SourceTypes)source_type)
            {
                case SourceTypes.SourceTypeCreature:
                    return await GetCreatureIdByGuid(guid);
                case SourceTypes.SourceTypeGameobject:
                    return await GetGameobjectIdByGuid(guid);
            }

            return 0;
        }

        public async Task<string> GetCreatureNameById(int id)
        {
            //DataTable dt = await ExecuteQuery("SELECT name FROM creature_template WHERE entry = '@id'", new MySqlParameter("@id", id));
            DataTable dt = await ExecuteQuery("SELECT name FROM creature_template WHERE entry = '" + id + "'");

            if (dt.Rows.Count == 0)
                return String.Empty;

            return dt.Rows[0]["name"].ToString();
        }

        public async Task<string> GetCreatureNameByGuid(int guid)
        {
            //DataTable dt = await ExecuteQuery("SELECT name FROM creature_template WHERE entry = '@id'", new MySqlParameter("@id", GetCreatureIdByGuid(guid)));
            DataTable dt = await ExecuteQuery("SELECT name FROM creature_template WHERE entry = '" + GetCreatureIdByGuid(guid) + "'");

            if (dt.Rows.Count == 0)
                return String.Empty;

            return dt.Rows[0]["name"].ToString();
        }

        public async Task<string> GetGameobjectNameById(int id)
        {
            //DataTable dt = await ExecuteQuery("SELECT name FROM gameobject_template WHERE entry = '@id'", new MySqlParameter("@id", id));
            DataTable dt = await ExecuteQuery("SELECT name FROM gameobject_template WHERE entry = '" + id + "'");

            if (dt.Rows.Count == 0)
                return String.Empty;

            return dt.Rows[0]["name"].ToString();
        }

        public async Task<string> GetGameobjectNameByGuid(int guid)
        {
            //DataTable dt = await ExecuteQuery("SELECT name FROM gameobject_template WHERE entry = '@id'", new MySqlParameter("@id", GetGameobjectIdByGuid(guid)));
            DataTable dt = await ExecuteQuery("SELECT name FROM gameobject_template WHERE entry = '" + GetGameobjectIdByGuid(guid) + "'");

            if (dt.Rows.Count == 0)
                return String.Empty;

            return dt.Rows[0]["name"].ToString();
        }

        public async Task<string> GetObjectNameByIdAndSourceType(int id, int source_type)
        {
            switch ((SourceTypes)source_type)
            {
                case SourceTypes.SourceTypeCreature:
                    return await GetCreatureNameById(id);
                case SourceTypes.SourceTypeGameobject:
                    return await GetGameobjectNameById(id);
            }

            return String.Empty;
        }

        public async Task<string> GetObjectNameByGuidAndSourceType(int guid, int source_type)
        {
            switch ((SourceTypes)source_type)
            {
                case SourceTypes.SourceTypeCreature:
                    return await GetCreatureNameByGuid(guid);
                case SourceTypes.SourceTypeGameobject:
                    return await GetGameobjectNameByGuid(guid);
            }

            return String.Empty;
        }

        public async Task<List<SmartScript>> GetSmartScripts(int entryorguid)
        {
            //DataTable dt = await ExecuteQuery("SELECT * FROM smart_scripts WHERE entryorguid = '@entryorguid'", new MySqlParameter("@entryorguid", entryorguid));
            DataTable dt = await ExecuteQuery("SELECT * FROM smart_scripts WHERE entryorguid = '" + entryorguid + "'");

            if (dt.Rows.Count == 0)
                return null;

            List<SmartScript> smartScripts = new List<SmartScript>();

            foreach (DataRow row in dt.Rows)
                smartScripts.Add(BuildSmartScript(row));

            return smartScripts;
        }

        public async Task<List<SmartScript>> GetSmartScripts(int entryorguid, int source_type)
        {
            //DataTable dt = await ExecuteQuery("SELECT * FROM smart_scripts WHERE entryorguid = '@entryorguid' AND source_type = '@source_type'", new MySqlParameter("@entryorguid", entryorguid), new MySqlParameter("@source_type", source_type));
            DataTable dt = await ExecuteQuery("SELECT * FROM smart_scripts WHERE entryorguid = '" + entryorguid + "' AND source_type = '" + source_type + "'");

            if (dt.Rows.Count == 0)
                return null;

            List<SmartScript> smartScripts = new List<SmartScript>();

            foreach (DataRow row in dt.Rows)
                smartScripts.Add(BuildSmartScript(row));

            return smartScripts;
        }

        public async Task<List<SmartScript>> GetSmartScriptActionLists(string criteria, bool useLikeStatement)
        {
            DataTable dt = await ExecuteQuery("SELECT * FROM smart_scripts WHERE action_type IN (80,87,88) AND source_type != 9 ORDER BY entryorguid");

            if (dt.Rows.Count == 0)
                return null;

            List<SmartScript> smartScripts = new List<SmartScript>();
            List<SmartScript> smartScriptsClean = new List<SmartScript>();

            foreach (DataRow row in dt.Rows)
                smartScripts.Add(BuildSmartScript(row));

            if (criteria.Length > 0)
            {
                foreach (SmartScript smartScript in smartScripts)
                {
                    var timedActionlistEntries = new List<string>();

                    switch ((SmartAction)smartScript.action_type)
                    {
                        case SmartAction.SMART_ACTION_CALL_TIMED_ACTIONLIST:
                            timedActionlistEntries.Add(smartScript.action_param1.ToString());
                            break;
                        case SmartAction.SMART_ACTION_CALL_RANDOM_TIMED_ACTIONLIST:
                            timedActionlistEntries.Add(smartScript.action_param1.ToString());
                            timedActionlistEntries.Add(smartScript.action_param2.ToString());

                            if (smartScript.action_param3 > 0)
                                timedActionlistEntries.Add(smartScript.action_param3.ToString());

                            if (smartScript.action_param4 > 0)
                                timedActionlistEntries.Add(smartScript.action_param4.ToString());

                            if (smartScript.action_param5 > 0)
                                timedActionlistEntries.Add(smartScript.action_param5.ToString());

                            if (smartScript.action_param6 > 0)
                                timedActionlistEntries.Add(smartScript.action_param6.ToString());

                            break;
                        case SmartAction.SMART_ACTION_CALL_RANDOM_RANGE_TIMED_ACTIONLIST:
                            for (int i = smartScript.action_param1; i <= smartScript.action_param2; ++i)
                                timedActionlistEntries.Add(i.ToString());
                            break;
                    }

                    foreach (string scriptEntry in timedActionlistEntries)
                    {
                        if (useLikeStatement)
                        {
                            if (scriptEntry.IndexOf(criteria, StringComparison.OrdinalIgnoreCase) >= 0)
                                smartScriptsClean.Add(smartScript);
                        }
                        else if (scriptEntry.IndexOf(criteria) >= 0)
                            smartScriptsClean.Add(smartScript);
                    }
                }
            }
            else
                smartScriptsClean = smartScripts;

            return smartScriptsClean;
        }

        public async Task<List<SmartScript>> GetSmartScriptsCallingActionLists()
        {
            DataTable dt = await ExecuteQuery("SELECT * FROM smart_scripts WHERE action_type IN (80,87,88) AND source_type != 9 ORDER BY entryorguid");

            if (dt.Rows.Count == 0)
                return null;

            List<SmartScript> smartScripts = new List<SmartScript>();

            foreach (DataRow row in dt.Rows)
                smartScripts.Add(BuildSmartScript(row));

            return smartScripts;
        }

        public async Task<bool> AreaTriggerHasSmartAI(int entry)
        {
            //DataTable dt = await SAI_Editor_Manager.Instance.worldDatabase.ExecuteQuery("SELECT * FROM areatrigger_scripts WHERE ScriptName = 'SmartTrigger' AND entry = '@entry'", new MySqlParameter("@entry", entry));
            DataTable dt = await SAI_Editor_Manager.Instance.worldDatabase.ExecuteQuery("SELECT * FROM areatrigger_scripts WHERE ScriptName = 'SmartTrigger' AND entry = '" + entry + "'");
            return dt.Rows.Count > 0;
        }

        private SmartScript BuildSmartScript(DataRow row)
        {
            var smartScript = new SmartScript();
            smartScript.entryorguid = row["entryorguid"] != DBNull.Value ? Convert.ToInt32(row["entryorguid"]) : -1;
            smartScript.source_type = row["source_type"] != DBNull.Value ? Convert.ToInt32(row["source_type"]) : 0;
            smartScript.id = row["id"] != DBNull.Value ? Convert.ToInt32(row["id"]) : 0;
            smartScript.link = row["link"] != DBNull.Value ? Convert.ToInt32(row["link"]) : 0;
            smartScript.event_type = row["event_type"] != DBNull.Value ? Convert.ToInt32(row["event_type"]) : 0;
            smartScript.event_phase_mask = row["event_phase_mask"] != DBNull.Value ? Convert.ToInt32(row["event_phase_mask"]) : 0;
            smartScript.event_chance = row["event_chance"] != DBNull.Value ? Convert.ToInt32(row["event_chance"]) : 0;
            smartScript.event_flags = row["event_flags"] != DBNull.Value ? Convert.ToInt32(row["event_flags"]) : 0;
            smartScript.event_param1 = row["event_param1"] != DBNull.Value ? Convert.ToInt32(row["event_param1"]) : 0;
            smartScript.event_param2 = row["event_param2"] != DBNull.Value ? Convert.ToInt32(row["event_param2"]) : 0;
            smartScript.event_param3 = row["event_param3"] != DBNull.Value ? Convert.ToInt32(row["event_param3"]) : 0;
            smartScript.event_param4 = row["event_param4"] != DBNull.Value ? Convert.ToInt32(row["event_param4"]) : 0;
            smartScript.action_type = row["action_type"] != DBNull.Value ? Convert.ToInt32(row["action_type"]) : 0;
            smartScript.action_param1 = row["action_param1"] != DBNull.Value ? Convert.ToInt32(row["action_param1"]) : 0;
            smartScript.action_param2 = row["action_param2"] != DBNull.Value ? Convert.ToInt32(row["action_param2"]) : 0;
            smartScript.action_param3 = row["action_param3"] != DBNull.Value ? Convert.ToInt32(row["action_param3"]) : 0;
            smartScript.action_param4 = row["action_param4"] != DBNull.Value ? Convert.ToInt32(row["action_param4"]) : 0;
            smartScript.action_param5 = row["action_param5"] != DBNull.Value ? Convert.ToInt32(row["action_param5"]) : 0;
            smartScript.action_param6 = row["action_param6"] != DBNull.Value ? Convert.ToInt32(row["action_param6"]) : 0;
            smartScript.target_type = row["target_type"] != DBNull.Value ? Convert.ToInt32(row["target_type"]) : 0;
            smartScript.target_param1 = row["target_param1"] != DBNull.Value ? Convert.ToInt32(row["target_param1"]) : 0;
            smartScript.target_param2 = row["target_param2"] != DBNull.Value ? Convert.ToInt32(row["target_param2"]) : 0;
            smartScript.target_param3 = row["target_param3"] != DBNull.Value ? Convert.ToInt32(row["target_param3"]) : 0;
            smartScript.target_x = row["target_x"] != DBNull.Value ? Convert.ToInt32(row["target_x"]) : 0;
            smartScript.target_y = row["target_y"] != DBNull.Value ? Convert.ToInt32(row["target_y"]) : 0;
            smartScript.target_z = row["target_z"] != DBNull.Value ? Convert.ToInt32(row["target_z"]) : 0;
            smartScript.target_o = row["target_o"] != DBNull.Value ? Convert.ToInt32(row["target_o"]) : 0;
            smartScript.comment = row["comment"] != DBNull.Value ? (string)row["comment"] : String.Empty;
            return smartScript;
        }
    }
}
