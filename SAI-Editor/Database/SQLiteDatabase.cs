using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Data.SQLite;
using SAI_Editor.Database.Classes;

namespace SAI_Editor
{
    class SQLiteDatabase : Database<SQLiteConnection, SQLiteConnectionStringBuilder, SQLiteParameter, SQLiteCommand>
    {
        public SQLiteDatabase(string file)
        {
            ConnectionString = new SQLiteConnectionStringBuilder();
            ConnectionString.DataSource = file;
        }

        public async Task<string> GetTooltipByParameterId(int parameterInfoId)
        {
            DataTable dt = await ExecuteQuery("SELECT toolTip FROM script_type_information WHERE parameterInfoId = @parameterInfoId", new SQLiteParameter("@parameterInfoId", parameterInfoId));

            if (dt.Rows.Count == 0)
                return String.Empty;

            return dt.Rows[0]["toolTip"].ToString();
        }

        private ParameterInfo BuildParameterInfo(DataRow row)
        {
            var parameterInfo = new ParameterInfo();
            parameterInfo.id = row["id"] != DBNull.Value ? Convert.ToInt32(row["id"]) : -1;
            parameterInfo.param1 = row["param1"] != DBNull.Value ? Convert.ToInt32(row["param1"]) : -1;
            parameterInfo.param2 = row["param2"] != DBNull.Value ? Convert.ToInt32(row["param2"]) : -1;
            parameterInfo.param3 = row["param3"] != DBNull.Value ? Convert.ToInt32(row["param3"]) : -1;
            parameterInfo.param4 = row["param4"] != DBNull.Value ? Convert.ToInt32(row["param4"]) : -1;
            parameterInfo.param5 = row["param5"] != DBNull.Value ? Convert.ToInt32(row["param5"]) : -1;
            parameterInfo.param6 = row["param6"] != DBNull.Value ? Convert.ToInt32(row["param6"]) : -1;
            return parameterInfo;
        }

        private ScriptTypeInformation BuildScriptTypeInformation(DataRow row)
        {
            var scriptTypeInformation = new ScriptTypeInformation();
            scriptTypeInformation.parameterInfoId = row["parameterInfoId"] != DBNull.Value ? Convert.ToInt32(row["parameterInfoId"]) : -1;
            scriptTypeInformation.scriptType = row["scriptType"] != DBNull.Value ? Convert.ToInt32(row["scriptType"]) : -1;
            scriptTypeInformation.id = row["id"] != DBNull.Value ? Convert.ToInt32(row["id"]) : -1;
            scriptTypeInformation.toolTip = row["toolTip"] != DBNull.Value ? (string)row["toolTip"] : String.Empty;
            return scriptTypeInformation;
        }
    }
}
