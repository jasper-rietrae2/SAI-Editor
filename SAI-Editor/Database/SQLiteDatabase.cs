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

        public async Task<EventTypeInformation> GetEventTypeInformationById(int event_type)
        {
            DataTable dt = await ExecuteQuery("SELECT * FROM event_type_information WHERE event_type = @event_type", new SQLiteParameter("@event_type", event_type));

            if (dt.Rows.Count == 0)
                return null;

            return BuildEventTypeInformation(dt.Rows[0]); //! Always take first index; should not be possible to have multiple instances per id, but still
        }

        public async Task<ActionTypeInformation> GetActionTypeInformationById(int action_type)
        {
            DataTable dt = await ExecuteQuery("SELECT * FROM action_type_information WHERE action_type = @action_type", new SQLiteParameter("@action_type", action_type));

            if (dt.Rows.Count == 0)
                return null;

            return BuildActionTypeInformation(dt.Rows[0]); //! Always take first index; should not be possible to have multiple instances per id, but still
        }

        public async Task<TargetTypeInformation> GetTargetTypeInformationById(int target_type)
        {
            DataTable dt = await ExecuteQuery("SELECT * FROM target_type_information WHERE target_type = @target_type", new SQLiteParameter("@target_type", target_type));

            if (dt.Rows.Count == 0)
                return null;

            return BuildTargetTypeInformation(dt.Rows[0]); //! Always take first index; should not be possible to have multiple instances per id, but still
        }

        private EventTypeInformation BuildEventTypeInformation(DataRow row)
        {
            var eventTypeInformation = new EventTypeInformation();
            eventTypeInformation.event_type = row["event_type"] != DBNull.Value ? Convert.ToInt32(row["event_type"]) : -1;
            eventTypeInformation.parameterString1 = row["parameterString1"] != DBNull.Value ? (string)row["parameterString1"] : String.Empty;
            eventTypeInformation.parameterString2 = row["parameterString2"] != DBNull.Value ? (string)row["parameterString2"] : String.Empty;
            eventTypeInformation.parameterString3 = row["parameterString3"] != DBNull.Value ? (string)row["parameterString3"] : String.Empty;
            eventTypeInformation.parameterString4 = row["parameterString4"] != DBNull.Value ? (string)row["parameterString4"] : String.Empty;
            eventTypeInformation.parameterTooltip1 = row["parameterTooltip1"] != DBNull.Value ? (string)row["parameterTooltip1"] : String.Empty;
            eventTypeInformation.parameterTooltip2 = row["parameterTooltip2"] != DBNull.Value ? (string)row["parameterTooltip2"] : String.Empty;
            eventTypeInformation.parameterTooltip3 = row["parameterTooltip3"] != DBNull.Value ? (string)row["parameterTooltip3"] : String.Empty;
            eventTypeInformation.parameterTooltip4 = row["parameterTooltip4"] != DBNull.Value ? (string)row["parameterTooltip4"] : String.Empty;
            return eventTypeInformation;
        }

        private ActionTypeInformation BuildActionTypeInformation(DataRow row)
        {
            var actionTypeInformation = new ActionTypeInformation();
            actionTypeInformation.action_type = row["action_type"] != DBNull.Value ? Convert.ToInt32(row["action_type"]) : -1;
            actionTypeInformation.parameterString1 = row["parameterString1"] != DBNull.Value ? (string)row["parameterString1"] : String.Empty;
            actionTypeInformation.parameterString2 = row["parameterString2"] != DBNull.Value ? (string)row["parameterString2"] : String.Empty;
            actionTypeInformation.parameterString3 = row["parameterString3"] != DBNull.Value ? (string)row["parameterString3"] : String.Empty;
            actionTypeInformation.parameterString4 = row["parameterString4"] != DBNull.Value ? (string)row["parameterString4"] : String.Empty;
            actionTypeInformation.parameterString5 = row["parameterString5"] != DBNull.Value ? (string)row["parameterString5"] : String.Empty;
            actionTypeInformation.parameterString6 = row["parameterString6"] != DBNull.Value ? (string)row["parameterString6"] : String.Empty;
            actionTypeInformation.parameterTooltip1 = row["parameterTooltip1"] != DBNull.Value ? (string)row["parameterTooltip1"] : String.Empty;
            actionTypeInformation.parameterTooltip2 = row["parameterTooltip2"] != DBNull.Value ? (string)row["parameterTooltip2"] : String.Empty;
            actionTypeInformation.parameterTooltip3 = row["parameterTooltip3"] != DBNull.Value ? (string)row["parameterTooltip3"] : String.Empty;
            actionTypeInformation.parameterTooltip4 = row["parameterTooltip4"] != DBNull.Value ? (string)row["parameterTooltip4"] : String.Empty;
            actionTypeInformation.parameterTooltip5 = row["parameterTooltip5"] != DBNull.Value ? (string)row["parameterTooltip5"] : String.Empty;
            actionTypeInformation.parameterTooltip6 = row["parameterTooltip6"] != DBNull.Value ? (string)row["parameterTooltip6"] : String.Empty;
            return actionTypeInformation;
        }

        private TargetTypeInformation BuildTargetTypeInformation(DataRow row)
        {
            var targetTypeInformation = new TargetTypeInformation();
            targetTypeInformation.target_type = row["target_type"] != DBNull.Value ? Convert.ToInt32(row["target_type"]) : -1;
            targetTypeInformation.parameterString1 = row["parameterString1"] != DBNull.Value ? (string)row["parameterString1"] : String.Empty;
            targetTypeInformation.parameterString2 = row["parameterString2"] != DBNull.Value ? (string)row["parameterString2"] : String.Empty;
            targetTypeInformation.parameterString3 = row["parameterString3"] != DBNull.Value ? (string)row["parameterString3"] : String.Empty;
            targetTypeInformation.parameterTooltip1 = row["parameterTooltip1"] != DBNull.Value ? (string)row["parameterTooltip1"] : String.Empty;
            targetTypeInformation.parameterTooltip2 = row["parameterTooltip2"] != DBNull.Value ? (string)row["parameterTooltip2"] : String.Empty;
            targetTypeInformation.parameterTooltip3 = row["parameterTooltip3"] != DBNull.Value ? (string)row["parameterTooltip3"] : String.Empty;
            return targetTypeInformation;
        }
    }
}
