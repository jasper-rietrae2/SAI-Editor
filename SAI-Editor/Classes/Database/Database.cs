using System;
using System.Threading;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;

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

            //! Using TaskEx makes all sync calls async.
            await TaskEx.Run(() =>
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
                            exceptionOccurred = true;

                            try
                            {
                                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                transaction.Rollback();
                            }
                            catch (Exception ex2)
                            {
                                MessageBox.Show(ex2.Message, "Something went wrong while rolling back!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            //! Using TaskEx makes all sync calls async.
            return await TaskEx.Run(() =>
            {
                using (Connection conn = (Connection)Activator.CreateInstance(typeof(Connection), connectionString.ToString()))
                {
                    conn.Open();

                    using (Command cmd = (Command)Activator.CreateInstance(typeof(Command), query, conn))
                    {
                        foreach (var param in parameters)
                            cmd.Parameters.Add(param);

                        try
                        {
                            var reader = cmd.ExecuteReader();

                            if (token.IsCancellationRequested)
                                token.ThrowIfCancellationRequested();

                            var dt = new DataTable();
                            dt.Load(reader);
                            conn.Close();
                            return dt;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return new DataTable();
                        }
                    }
                }
            });
        }
    }
}
