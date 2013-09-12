using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using SAI_Editor.Database.Classes;

namespace SAI_Editor
{
    class WorldDatabase
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }
        public MySqlConnectionStringBuilder connectionString { get; private set; }

        public WorldDatabase(string serverHost, int port, string username, string password, string dbName)
        {
            Host = serverHost;
            Port = port;
            Username = username;
            Password = password;
            DatabaseName = dbName;

            connectionString = new MySqlConnectionStringBuilder();
            connectionString.Server = serverHost;
            connectionString.Port = (uint)port;
            connectionString.UserID = username;
            connectionString.Password = password;
            connectionString.Database = dbName;
            connectionString.AllowUserVariables = true;
            connectionString.AllowZeroDateTime = true;
        }

        async Task ExecuteNonQuery(string nonQuery, params MySqlParameter[] parameters)
        {
            await Task.Run(() =>
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString.ToString()))
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(nonQuery, conn))
                    {
                        foreach (var param in parameters)
                            cmd.Parameters.Add(param);

                        cmd.ExecuteNonQuery();
                    }

                    conn.Close();
                }
            });
        }

        async Task<DataTable> ExecuteQuery(string query, params MySqlParameter[] parameters)
        {
            return await Task.Run(() =>
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString.ToString()))
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        foreach (var param in parameters)
                            cmd.Parameters.Add(param);

                        var reader = cmd.ExecuteReader();
                        var dt = new DataTable();
                        dt.Load(reader);
                        conn.Close();
                        return dt;
                    }
                }
            });
        }

        async Task<object> ExecuteScalar(string query, params MySqlParameter[] parameters)
        {
            return await Task.Run(() =>
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString.ToString()))
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        foreach (var param in parameters)
                            cmd.Parameters.Add(param);

                        object returnVal = cmd.ExecuteScalar();
                        conn.Close();
                        return returnVal;
                    }
                }
            });
        }

        public async Task<int> GetCreatureIdByGuid(int guid)
        {
            DataTable dt = await ExecuteQuery("SELECT id FROM creature WHERE guid = @guid", new MySqlParameter("@guid", guid));

            if (dt.Rows.Count > 0)
                return 0;

            return (int)dt.Rows[0]["id"];
        }

        public async Task<string> GetCreatureNameById(int id)
        {
            DataTable dt = await ExecuteQuery("SELECT name FROM creature_template WHERE entry = @id", new MySqlParameter("@id", id));

            if (dt.Rows.Count > 0)
                return String.Empty;

            return (string)dt.Rows[0]["name"];
        }

        public async Task<string> GetCreatureNameByGuid(int guid)
        {
            DataTable dt = await ExecuteQuery("SELECT name FROM creature_template WHERE entry = @id", new MySqlParameter("@id", GetCreatureIdByGuid(guid)));

            if (dt.Rows.Count > 0)
                return String.Empty;

            return (string)dt.Rows[0]["name"];
        }

        public async Task<List<SmartScript>> GetSmartScripts(int entryorguid)
        {
            DataTable dt = await ExecuteQuery("SELECT * FROM smart_scripts WHERE entryorguid = @entryorguid", new MySqlParameter("@entryorguid", entryorguid));

            if (dt.Rows.Count == 0)
                return null;

            List<SmartScript> smartScripts = new List<SmartScript>();

            foreach (DataRow row in dt.Rows)
                smartScripts.Add(BuildSmartScript(row));

            return smartScripts;
        }

        public async Task<List<SmartScript>> GetSmartScripts(int entryorguid, int source_type)
        {
            DataTable dt = await ExecuteQuery("SELECT * FROM smart_scripts WHERE entryorguid = @entryorguid AND source_type = @source_type", new MySqlParameter("@entryorguid", entryorguid), new MySqlParameter("@source_type", source_type));

            if (dt.Rows.Count == 0)
                return null;

            List<SmartScript> smartScripts = new List<SmartScript>();

            foreach (DataRow row in dt.Rows)
                smartScripts.Add(BuildSmartScript(row));

            return smartScripts;
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
