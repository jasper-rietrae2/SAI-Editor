using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;
using SAI_Editor.Database.Classes;

namespace SAI_Editor.Database
{
    class SQLiteDatabase : Database<SQLiteConnection, SQLiteConnectionStringBuilder, SQLiteParameter, SQLiteCommand>
    {
        public SQLiteDatabase(string file)
        {
            ConnectionString = new SQLiteConnectionStringBuilder();
            ConnectionString.DataSource = file;
            ConnectionString.Version = 3;
        }

        public async Task<string> GetParameterStringById(int type, int paramId, ScriptTypeId scriptTypeId)
        {
            BaseTypeInformation baseTypeInformation = await GetTypeByScriptTypeId(type, scriptTypeId);

            switch (paramId)
            {
                case 1:
                    return baseTypeInformation != null ? baseTypeInformation.parameterString1 : String.Empty;
                case 2:
                    return baseTypeInformation != null ? baseTypeInformation.parameterString2 : String.Empty;
                case 3:
                    return baseTypeInformation != null ? baseTypeInformation.parameterString3 : String.Empty;
                case 4:
                    return baseTypeInformation != null ? baseTypeInformation.parameterString4 : String.Empty;
                case 5:
                    return baseTypeInformation != null ? baseTypeInformation.parameterString5 : String.Empty;
                case 6:
                    return baseTypeInformation != null ? baseTypeInformation.parameterString6 : String.Empty;
                default:
                    return String.Empty;
            }
        }

        public async Task<string> GetParameterTooltipById(int type, int paramId, ScriptTypeId scriptTypeId)
        {
            BaseTypeInformation baseTypeInformation = await GetTypeByScriptTypeId(type, scriptTypeId);

            switch (paramId)
            {
                case 1:
                    return baseTypeInformation != null ? baseTypeInformation.parameterTooltip1 : String.Empty;
                case 2:
                    return baseTypeInformation != null ? baseTypeInformation.parameterTooltip2 : String.Empty;
                case 3:
                    return baseTypeInformation != null ? baseTypeInformation.parameterTooltip3 : String.Empty;
                case 4:
                    return baseTypeInformation != null ? baseTypeInformation.parameterTooltip4 : String.Empty;
                case 5:
                    return baseTypeInformation != null ? baseTypeInformation.parameterTooltip5 : String.Empty;
                case 6:
                    return baseTypeInformation != null ? baseTypeInformation.parameterTooltip6 : String.Empty;
                default:
                    return String.Empty;
            }
        }

        public async Task<string> GetScriptTypeTooltipById(int type, ScriptTypeId scriptTypeId)
        {
            BaseTypeInformation baseTypeInformation = await GetTypeByScriptTypeId(type, scriptTypeId);
            return baseTypeInformation != null ? baseTypeInformation.tooltip : String.Empty;
        }

        public async Task<EventTypeInformation> GetEventTypeInformationById(int event_type)
        {
            //DataTable dt = await ExecuteQuery("SELECT * FROM event_type_information WHERE event_type = '@event_type'", new SQLiteParameter("@event_type", event_type));
            DataTable dt = await ExecuteQuery("SELECT * FROM event_type_information WHERE event_type = '" + event_type + "'");

            if (dt.Rows.Count == 0)
                return null;

            return BuildEventTypeInformation(dt.Rows[0]); //! Always take first index; should not be possible to have multiple instances per id, but still
        }

        public async Task<ActionTypeInformation> GetActionTypeInformationById(int action_type)
        {
            //DataTable dt = await ExecuteQuery("SELECT * FROM action_type_information WHERE action_type = '@action_type'", new SQLiteParameter("@action_type", action_type));
            DataTable dt = await ExecuteQuery("SELECT * FROM action_type_information WHERE action_type = '" + action_type + "'");

            if (dt.Rows.Count == 0)
                return null;

            return BuildActionTypeInformation(dt.Rows[0]); //! Always take first index; should not be possible to have multiple instances per id, but still
        }

        public async Task<TargetTypeInformation> GetTargetTypeInformationById(int target_type)
        {
            //DataTable dt = await ExecuteQuery("SELECT * FROM target_type_information WHERE target_type = '@target_type'", new SQLiteParameter("@target_type", target_type));
            DataTable dt = await ExecuteQuery("SELECT * FROM target_type_information WHERE target_type = '" + target_type + "'");

            if (dt.Rows.Count == 0)
                return null;

            return BuildTargetTypeInformation(dt.Rows[0]); //! Always take first index; should not be possible to have multiple instances per id, but still
        }

        private async Task<BaseTypeInformation> GetTypeByScriptTypeId(int type, ScriptTypeId scriptTypeId)
        {
            switch (scriptTypeId)
            {
                case ScriptTypeId.ScriptTypeEvent:
                    return await GetEventTypeInformationById(type);
                case ScriptTypeId.ScriptTypeAction:
                    return await GetActionTypeInformationById(type);
                case ScriptTypeId.ScriptTypeTarget:
                    return await GetTargetTypeInformationById(type);
                default:
                    return null;
            }
        }

        public async Task<List<AreaTrigger>> GetAreaTriggers()
        {
            DataTable dt = await ExecuteQuery("SELECT * FROM areatriggers");

            if (dt.Rows.Count == 0)
                return null;

            List<AreaTrigger> areaTriggers = new List<AreaTrigger>();

            foreach (DataRow row in dt.Rows)
                areaTriggers.Add(BuildAreaTrigger(row));

            return areaTriggers;
        }

        public async Task<AreaTrigger> GetAreaTriggerById(int id)
        {
            //DataTable dt = await ExecuteQuery("SELECT * FROM areatriggers WHERE 'id' = @id", new SQLiteParameter("@id", id));
            DataTable dt = await ExecuteQuery("SELECT * FROM areatriggers WHERE id = '" + id + "'");

            if (dt.Rows.Count == 0)
                return null;

            return BuildAreaTrigger(dt.Rows[0]); //! Always take first index; should not be possible to have multiple instances per id, but still
        }

        //! We just do this locally because the database has a SHITLOAD of columns and we only need very few. Not going
        //! to create a class just for that...
        //public async Task<List<Spell>> GetSpells()
        //{
        //    DataTable dt = await ExecuteQuery("SELECT * FROM spells");

        //    if (dt.Rows.Count == 0)
        //        return null;

        //    List<Spell> spells = new List<Spell>();

        //    foreach (DataRow row in dt.Rows)
        //        spells.Add(BuidSpell(row));

        //    return spells;
        //}

        private EventTypeInformation BuildEventTypeInformation(DataRow row)
        {
            var eventTypeInformation = new EventTypeInformation();
            eventTypeInformation.event_type = row["event_type"] != DBNull.Value ? Convert.ToInt32(row["event_type"]) : -1;
            eventTypeInformation.tooltip = row["tooltip"] != DBNull.Value ? (string)row["tooltip"] : String.Empty;
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
            actionTypeInformation.tooltip = row["tooltip"] != DBNull.Value ? (string)row["tooltip"] : String.Empty;
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
            targetTypeInformation.tooltip = row["tooltip"] != DBNull.Value ? (string)row["tooltip"] : String.Empty;
            targetTypeInformation.parameterString1 = row["parameterString1"] != DBNull.Value ? (string)row["parameterString1"] : String.Empty;
            targetTypeInformation.parameterString2 = row["parameterString2"] != DBNull.Value ? (string)row["parameterString2"] : String.Empty;
            targetTypeInformation.parameterString3 = row["parameterString3"] != DBNull.Value ? (string)row["parameterString3"] : String.Empty;
            targetTypeInformation.parameterTooltip1 = row["parameterTooltip1"] != DBNull.Value ? (string)row["parameterTooltip1"] : String.Empty;
            targetTypeInformation.parameterTooltip2 = row["parameterTooltip2"] != DBNull.Value ? (string)row["parameterTooltip2"] : String.Empty;
            targetTypeInformation.parameterTooltip3 = row["parameterTooltip3"] != DBNull.Value ? (string)row["parameterTooltip3"] : String.Empty;
            return targetTypeInformation;
        }

        public AreaTrigger BuildAreaTrigger(DataRow row)
        {
            var areaTrigger = new AreaTrigger();
            areaTrigger.id = row["m_id"] != DBNull.Value ? Convert.ToInt32(row["m_id"]) : -1;
            areaTrigger.map_id = row["m_mapId"] != DBNull.Value ? Convert.ToInt32(row["m_mapId"]) : 0;
            areaTrigger.posX = row["m_posX"] != DBNull.Value ? (double)row["m_posX"] : 0;
            areaTrigger.posY = row["m_posY"] != DBNull.Value ? (double)row["m_posY"] : 0;
            areaTrigger.posZ = row["m_posZ"] != DBNull.Value ? (double)row["m_posZ"] : 0;
            areaTrigger.field5 = row["field5"] != DBNull.Value ? (int)row["field5"] : 0;
            areaTrigger.field6 = row["field6"] != DBNull.Value ? (int)row["field6"] : 0;
            areaTrigger.field7 = row["field7"] != DBNull.Value ? (int)row["field7"] : 0;
            areaTrigger.field8 = row["field8"] != DBNull.Value ? (double)row["field8"] : 0;
            areaTrigger.field9 = row["field9"] != DBNull.Value ? (double)row["field9"] : 0;
            return areaTrigger;
        }
    }
}
