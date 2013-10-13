using System;
using System.Threading;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Data;

namespace SAI_Editor
{
    public class Database<Connection, StrBuilder, Parameter, Command, Transaction>
        where Connection : DbConnection
        where Command : DbCommand
        where Transaction : DbTransaction
    {
        public StrBuilder connectionString { get; set; }

        public bool CanConnectToDatabase(MySqlConnectionStringBuilder _connectionString, bool showErrorMessage = true)
        {
            try
            {
                //! Close the connection again since this is just a try-connection function. We actually connect
                //! when the mainform is opened (this happens automatically because we use 'using').
                using (MySqlConnection connection = new MySqlConnection(_connectionString.ToString()))
                    connection.Open();
            }
            catch (MySqlException ex)
            {
                if (showErrorMessage)
                    MessageBox.Show(ex.Message, "Could not connect", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }

            return true;
        }

        public async Task<bool> ExecuteNonQuery(string nonQuery, params Parameter[] parameters)
        {
            bool exceptionOccurred = false;

            await Task.Run(() =>
            {
                using (Connection conn = (Connection)Activator.CreateInstance(typeof(Connection), connectionString.ToString()))
                {
                    conn.Open();
                    var transaction = conn.BeginTransaction();

                    using (Command cmd = (Command)Activator.CreateInstance(typeof(Command), nonQuery, conn))
                    {
                        cmd.Transaction = transaction;

                        foreach (var param in parameters)
                            cmd.Parameters.Add(param);

                        try
                        {
                            cmd.ExecuteNonQuery();
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                exceptionOccurred = true;
                                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                transaction.Rollback();
                            }
                            catch (Exception ex2)
                            {
                                MessageBox.Show(ex2.Message, "Something went wrong while rolling back!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                exceptionOccurred = true;
                            }
                        }
                    }

                    conn.Close();
                }
            });

            return !exceptionOccurred;
        }

        public Task<DataTable> ExecuteQuery(string query, params Parameter[] parameters)
        {
            return ExecuteQueryWithCancellation(new CancellationToken(), query, parameters);
        }

        public async Task<DataTable> ExecuteQueryWithCancellation(CancellationToken token, string query, params Parameter[] parameters)
        {
            return await Task.Run(async() =>
            {
                using (Connection conn = (Connection)Activator.CreateInstance(typeof(Connection), connectionString.ToString()))
                {
                    conn.Open();
                    var transaction = conn.BeginTransaction();

                    using (Command cmd = (Command)Activator.CreateInstance(typeof(Command), query, conn))
                    {
                        cmd.Transaction = transaction;

                        foreach (var param in parameters)
                            cmd.Parameters.Add(param);

                        try
                        {
                            var reader = await cmd.ExecuteReaderAsync(token);

                            if (token.IsCancellationRequested)
                                token.ThrowIfCancellationRequested();

                            var dt = new DataTable();
                            dt.Load(reader);
                            transaction.Commit();
                            conn.Close();
                            return dt;
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                transaction.Rollback();
                                return null;
                            }
                            catch (Exception ex2)
                            {
                                MessageBox.Show(ex2.Message, "Something went wrong while rolling back!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return null;
                            }
                        }
                    }
                }
            });
        }

        public async Task<object> ExecuteScalar(string query, params Parameter[] parameters)
        {
            return await Task.Run(() =>
            {
                using (Connection conn = (Connection)Activator.CreateInstance(typeof(Connection), connectionString.ToString()))
                {
                    conn.Open();
                    var transaction = conn.BeginTransaction();

                    using (Command cmd = (Command)Activator.CreateInstance(typeof(Command), query, conn))
                    {
                        cmd.Transaction = transaction;

                        foreach (var param in parameters)
                            cmd.Parameters.Add(param);

                        try
                        {
                            object returnVal = cmd.ExecuteScalar();
                            transaction.Commit();
                            conn.Close();
                            return returnVal;
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                transaction.Rollback();
                                return null;
                            }
                            catch (Exception ex2)
                            {
                                MessageBox.Show(ex2.Message, "Something went wrong while rolling back!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return null;
                            }
                        }
                    }
                }
            });
        }
    }
}
