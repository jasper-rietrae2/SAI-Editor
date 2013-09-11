using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace SAI_Editor
{
    abstract class MySqlDatabase
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }
        public string ConnectionString { get; private set; }

        protected MySqlDatabase(string serverHost, int port, string username, string password, string dbName)
        {
            Host = serverHost;
            Port = port;
            Username = username;
            Password = password;
            DatabaseName = dbName;

            var connStr = new MySqlConnectionStringBuilder();
            connStr.Server = serverHost;
            connStr.Port = (uint)port;
            connStr.UserID = username;
            connStr.Password = password;
            connStr.Database = dbName;
            connStr.AllowUserVariables = true;
            connStr.AllowZeroDateTime = true;
            ConnectionString = connStr.ToString();
        }

        protected async Task ExecuteNonQuery(string nonQuery, params MySqlParameter[] mParams)
        {
            await Task.Run(() =>
            {
                using (MySqlConnection conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(nonQuery, conn))
                    {
                        foreach (var param in mParams)
                            cmd.Parameters.Add(param);

                        cmd.ExecuteNonQuery();
                    }

                    conn.Close();
                }
            });
        }

        protected async Task<DataTable> ExecuteQuery(string query, params MySqlParameter[] mParams)
        {
            return await Task.Run(() =>
            {
                using (MySqlConnection conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        foreach (var param in mParams)
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

        protected async Task<object> ExecuteScalar(string query, params MySqlParameter[] mParams)
        {
            return await Task.Run(() =>
            {
                using (MySqlConnection conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        foreach (var param in mParams)
                            cmd.Parameters.Add(param);

                        object returnVal = cmd.ExecuteScalar();
                        conn.Close();
                        return returnVal;
                    }
                }
            });
        }
    }
}
