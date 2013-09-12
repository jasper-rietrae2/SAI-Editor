using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MySql.Data.Types;

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
    }
}
