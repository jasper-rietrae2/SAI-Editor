using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;
using SAI_Editor.Classes.Database.Classes;

namespace SAI_Editor.Classes.Database
{

    class SQLiteDatabase : Database<SQLiteConnection, SQLiteConnectionStringBuilder, SQLiteParameter, SQLiteCommand, SQLiteTransaction>
    {
        public SQLiteDatabase(string file)
        {
            this.connectionString = new SQLiteConnectionStringBuilder();
            this.connectionString.DataSource = file;
            this.connectionString.Version = 3;
        }

        public async Task<List<EventTypeInformation>> GetEventTypeInformation()
        {
            DataTable dt = await this.ExecuteQuery("SELECT * FROM event_type_information");

            if (dt.Rows.Count == 0)
                return null;

            List<EventTypeInformation> eventTypeInformations = new List<EventTypeInformation>();

            foreach (DataRow row in dt.Rows)
                eventTypeInformations.Add(this.BuildEventTypeInformation(row));

            return eventTypeInformations;
        }

        public async Task<List<ActionTypeInformation>> GetActionTypeInformation()
        {
            DataTable dt = await this.ExecuteQuery("SELECT * FROM action_type_information");

            if (dt.Rows.Count == 0)
                return null;

            List<ActionTypeInformation> actionTypeInformations = new List<ActionTypeInformation>();

            foreach (DataRow row in dt.Rows)
                actionTypeInformations.Add(this.BuildActionTypeInformation(row));

            return actionTypeInformations;
        }

        public async Task<List<TargetTypeInformation>> GetTargetTypeInformation()
        {
            DataTable dt = await this.ExecuteQuery("SELECT * FROM target_type_information");

            if (dt.Rows.Count == 0)
                return null;

            List<TargetTypeInformation> targetTypeInformations = new List<TargetTypeInformation>();

            foreach (DataRow row in dt.Rows)
                targetTypeInformations.Add(this.BuildTargetTypeInformation(row));

            return targetTypeInformations;
        }

        public async Task<List<AreaTrigger>> GetAreaTriggers()
        {
            DataTable dt = await this.ExecuteQuery("SELECT * FROM areatriggers");

            if (dt.Rows.Count == 0)
                return null;

            List<AreaTrigger> areaTriggers = new List<AreaTrigger>();

            foreach (DataRow row in dt.Rows)
                areaTriggers.Add(this.BuildAreaTrigger(row));

            return areaTriggers;
        }

        public async Task<AreaTrigger> GetAreaTriggerById(int id)
        {
            //DataTable dt = await ExecuteQuery("SELECT * FROM areatriggers WHERE 'id' = @id", new SQLiteParameter("@id", id));
            DataTable dt = await this.ExecuteQuery("SELECT * FROM areatriggers WHERE id = '" + id + "'");

            if (dt.Rows.Count == 0)
                return null;

            return this.BuildAreaTrigger(dt.Rows[0]); //! Always take first index; should not be possible to have multiple instances per id, but still
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

        public async Task<string> GetSpellNameById(int id)
        {
            DataTable dt = await this.ExecuteQuery("SELECT spellName FROM spells WHERE id = '" + id + "'");

            if (dt.Rows.Count == 0)
                return "<Spell not found!>";

            return (string)dt.Rows[0]["spellName"]; //! Always take first index; should not be possible to have multiple instances per id, but still
        }

        private EventTypeInformation BuildEventTypeInformation(DataRow row)
        {
            var eventTypeInformation = new EventTypeInformation();
            eventTypeInformation.event_type = row["event_type"] != DBNull.Value ? XConverter.ToInt32(row["event_type"]) : -1;
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
            actionTypeInformation.action_type = row["action_type"] != DBNull.Value ? XConverter.ToInt32(row["action_type"]) : -1;
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
            targetTypeInformation.target_type = row["target_type"] != DBNull.Value ? XConverter.ToInt32(row["target_type"]) : -1;
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
            areaTrigger.id = row["m_id"] != DBNull.Value ? XConverter.ToInt32(row["m_id"]) : -1;
            areaTrigger.map_id = row["m_mapId"] != DBNull.Value ? XConverter.ToInt32(row["m_mapId"]) : 0;
            areaTrigger.posX = row["m_posX"] != DBNull.Value ? (double)row["m_posX"] : 0;
            areaTrigger.posY = row["m_posY"] != DBNull.Value ? (double)row["m_posY"] : 0;
            areaTrigger.posZ = row["m_posZ"] != DBNull.Value ? (double)row["m_posZ"] : 0;
            return areaTrigger;
        }
    }
}
