using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Data;
using System.Windows.Forms;

namespace SAI_Editor
{
    public class Database<Connection, StrBuilder, Parameter, Command> 
        where Connection : DbConnection
        where Command : DbCommand
    {
        public StrBuilder ConnectionString { get; set; }

        public async Task ExecuteNonQuery(string nonQuery, params Parameter[] parameters)
        {
            await Task.Run(() =>
            {
                using (Connection conn = (Connection)Activator.CreateInstance(typeof(Connection), ConnectionString.ToString()))
                {
                    conn.Open();

                    using (Command cmd = (Command)Activator.CreateInstance(typeof(Command), nonQuery, conn))
                    {
                        foreach (var param in parameters)
                            cmd.Parameters.Add(param);

                        cmd.ExecuteNonQuery();
                    }

                    conn.Close();
                }
            });
        }

        public async Task<DataTable> ExecuteQuery(string query, params Parameter[] parameters)
        {
            return await Task.Run(() =>
            {
                using (Connection conn = (Connection)Activator.CreateInstance(typeof(Connection), ConnectionString.ToString()))
                {
                    MessageBox.Show((conn != null).ToString());
                    conn.Open();

                    using (Command cmd = (Command)Activator.CreateInstance(typeof(Command), query, conn))
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

        public async Task<object> ExecuteScalar(string query, params Parameter[] parameters)
        {
            return await Task.Run(() =>
            {
                using (Connection conn = (Connection)Activator.CreateInstance(typeof(Connection), ConnectionString.ToString()))
                {
                    conn.Open();

                    using (Command cmd = (Command)Activator.CreateInstance(typeof(Command), query, conn))
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

    }
}
