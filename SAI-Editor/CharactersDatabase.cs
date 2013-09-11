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
    class CharactersDatabase : MySqlDatabase
    {
        public CharactersDatabase(string serverHost, int port, string username, string password, string dbName) : base(serverHost, port, username, password, dbName) { }
    }
}
