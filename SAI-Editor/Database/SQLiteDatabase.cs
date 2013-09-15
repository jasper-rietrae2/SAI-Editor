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

        public async Task<ParameterInfo> GetParameterInfoById(int id)
        {
            DataTable dt = await ExecuteQuery("SELECT * FROM parameter_info WHERE id = @id", new SQLiteParameter("@id", id));

            if (dt.Rows.Count == 0)
                return null;

            return BuildParameterInfo(dt.Rows[0]); //! Always take first index; should not be possible to have multiple instances per id, but still
        }

        public async Task<ScriptTypeInformation> GetScriptTypeInformationByParameterInfoId(int parameterInfoId)
        {
            DataTable dt = await ExecuteQuery("SELECT * FROM script_type_information WHERE parameterInfoId = @parameterInfoId", new SQLiteParameter("@parameterInfoId", parameterInfoId));

            if (dt.Rows.Count == 0)
                return null;

            return BuildScriptTypeInformation(dt.Rows[0]); //! Always take first index; should not be possible to have multiple instances per id, but still
        }

        public async Task<List<ScriptTypeInformation>> GetScriptTypeInformationsByTypeId(ScriptTypeId scriptTypeId)
        {
            DataTable dt = await ExecuteQuery("SELECT * FROM script_type_information WHERE scriptTypeId = @scriptTypeId", new SQLiteParameter("@scriptTypeId", scriptTypeId));

            if (dt.Rows.Count == 0)
                return null;

            List<ScriptTypeInformation> scriptTypeInformation = new List<ScriptTypeInformation>();

            foreach (DataRow row in dt.Rows)
                scriptTypeInformation.Add(BuildScriptTypeInformation(row));

            return scriptTypeInformation;
        }

        public async Task<string> GetTooltipByParameterId(int parameterInfoId)
        {
            DataTable dt = await ExecuteQuery("SELECT toolTip FROM script_type_information WHERE parameterInfoId = @parameterInfoId", new SQLiteParameter("@parameterInfoId", parameterInfoId));

            if (dt.Rows.Count == 0)
                return String.Empty;

            return dt.Rows[0]["toolTip"].ToString();
        }

        public async Task<int[]> GetParametersByParameterId(int id)
        {
            DataTable dt = await ExecuteQuery("SELECT * FROM parameter_info WHERE id = @id", new SQLiteParameter("@id", id));

            if (dt.Rows.Count == 0)
                return null;

            int[] parameters = new int[6];
            parameters[0] = dt.Rows[0]["param1"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["param1"]) : 0;
            parameters[0] = dt.Rows[0]["param2"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["param2"]) : 0;
            parameters[0] = dt.Rows[0]["param3"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["param3"]) : 0;
            parameters[0] = dt.Rows[0]["param4"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["param4"]) : 0;
            parameters[0] = dt.Rows[0]["param5"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["param5"]) : 0;
            parameters[0] = dt.Rows[0]["param6"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["param6"]) : 0;
            return parameters;
        }

        public async Task<int> GetParameterByParameterId(int id, int param)
        {
            DataTable dt = await ExecuteQuery("SELECT * FROM parameter_info WHERE id = @id", new SQLiteParameter("@id", id));

            if (dt.Rows.Count == 0)
                return 0;

            switch (param)
            {
                case 1:
                    return dt.Rows[0]["param1"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["param1"]) : 0;
                case 2:
                    return dt.Rows[0]["param2"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["param2"]) : 0;
                case 3:
                    return dt.Rows[0]["param3"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["param3"]) : 0;
                case 4:
                    return dt.Rows[0]["param4"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["param4"]) : 0;
                case 5:
                    return dt.Rows[0]["param5"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["param5"]) : 0;
                case 6:
                    return dt.Rows[0]["param6"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["param6"]) : 0;
            }

            return 0;
        }

        private ParameterInfo BuildParameterInfo(DataRow row)
        {
            var parameterInfo = new ParameterInfo();
            parameterInfo.id = row["id"] != DBNull.Value ? Convert.ToInt32(row["id"]) : -1;
            parameterInfo.param1 = row["param1"] != DBNull.Value ? Convert.ToInt32(row["param1"]) : 0;
            parameterInfo.param2 = row["param2"] != DBNull.Value ? Convert.ToInt32(row["param2"]) : 0;
            parameterInfo.param3 = row["param3"] != DBNull.Value ? Convert.ToInt32(row["param3"]) : 0;
            parameterInfo.param4 = row["param4"] != DBNull.Value ? Convert.ToInt32(row["param4"]) : 0;
            parameterInfo.param5 = row["param5"] != DBNull.Value ? Convert.ToInt32(row["param5"]) : 0;
            parameterInfo.param6 = row["param6"] != DBNull.Value ? Convert.ToInt32(row["param6"]) : 0;
            return parameterInfo;
        }

        private ScriptTypeInformation BuildScriptTypeInformation(DataRow row)
        {
            var scriptTypeInformation = new ScriptTypeInformation();
            scriptTypeInformation.parameterInfoId = row["parameterInfoId"] != DBNull.Value ? Convert.ToInt32(row["parameterInfoId"]) : -1;
            scriptTypeInformation.scriptType = row["scriptType"] != DBNull.Value ? Convert.ToInt32(row["scriptType"]) : 0;
            scriptTypeInformation.id = row["id"] != DBNull.Value ? Convert.ToInt32(row["id"]) : 0;
            scriptTypeInformation.toolTip = row["toolTip"] != DBNull.Value ? (string)row["toolTip"] : String.Empty;
            return scriptTypeInformation;
        }
    }
}
