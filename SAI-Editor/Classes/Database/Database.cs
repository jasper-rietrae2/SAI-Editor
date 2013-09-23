using System;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Data;

namespace SAI_Editor
{
    public class Database<Connection, StrBuilder, Parameter, Command> where Connection : DbConnection where Command : DbCommand
    {
        public StrBuilder ConnectionString { get; set; }

        public async Task ExecuteNonQuery(string nonQuery, params Parameter[] parameters)
        {
            try
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

                            try
                            {
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }

                        conn.Close();
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public async Task<DataTable> ExecuteQuery(string query, params Parameter[] parameters)
        {
            try
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

                            try
                            {
                                var reader = cmd.ExecuteReader();
                                var dt = new DataTable();
                                dt.Load(reader);
                                conn.Close();
                                return dt;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return null;
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public async Task<object> ExecuteScalar(string query, params Parameter[] parameters)
        {
            try
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

                            try
                            {
                                object returnVal = cmd.ExecuteScalar();
                                conn.Close();
                                return returnVal;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return null;
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}
