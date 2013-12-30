namespace SAI_Editor.Database.Classes
{
    public class SmartScript
    {
        public int entryorguid { get; set; }
        public int source_type { get; set; }
        public int id { get; set; }
        public int link { get; set; }
        public int event_type { get; set; }
        public int event_phase_mask { get; set; }
        public int event_chance { get; set; }
        public int event_flags { get; set; }
        public int event_param1 { get; set; }
        public int event_param2 { get; set; }
        public int event_param3 { get; set; }
        public int event_param4 { get; set; }
        public int action_type { get; set; }
        public int action_param1 { get; set; }
        public int action_param2 { get; set; }
        public int action_param3 { get; set; }
        public int action_param4 { get; set; }
        public int action_param5 { get; set; }
        public int action_param6 { get; set; }
        public int target_type { get; set; }
        public int target_param1 { get; set; }
        public int target_param2 { get; set; }
        public int target_param3 { get; set; }
        public double target_x { get; set; }
        public double target_y { get; set; }
        public double target_z { get; set; }
        public double target_o { get; set; }
        public string comment { get; set; }

        public SmartScript Clone(SmartScript smartScript)
        {
            SmartScript newSmartScript = new SmartScript();
            newSmartScript.entryorguid = smartScript.entryorguid;
            newSmartScript.source_type = smartScript.source_type;
            newSmartScript.id = smartScript.id;
            newSmartScript.link = smartScript.link;
            newSmartScript.event_type = smartScript.event_type;
            newSmartScript.event_phase_mask = smartScript.event_phase_mask;
            newSmartScript.event_chance = smartScript.event_chance;
            newSmartScript.event_flags = smartScript.event_flags;
            newSmartScript.event_param1 = smartScript.event_param1;
            newSmartScript.event_param2 = smartScript.event_param2;
            newSmartScript.event_param3 = smartScript.event_param3;
            newSmartScript.event_param4 = smartScript.event_param4;
            newSmartScript.action_type = smartScript.action_type;
            newSmartScript.action_param1 = smartScript.action_param1;
            newSmartScript.action_param2 = smartScript.action_param2;
            newSmartScript.action_param2 = smartScript.action_param3;
            newSmartScript.action_param4 = smartScript.action_param4;
            newSmartScript.action_param5 = smartScript.action_param5;
            newSmartScript.action_param6 = smartScript.action_param6;
            newSmartScript.target_type = smartScript.target_type;
            newSmartScript.target_param1 = smartScript.target_param1;
            newSmartScript.target_param2 = smartScript.target_param2;
            newSmartScript.target_param3 = smartScript.target_param3;
            newSmartScript.target_x = smartScript.target_x;
            newSmartScript.target_y = smartScript.target_y;
            newSmartScript.target_z = smartScript.target_z;
            newSmartScript.target_o = smartScript.target_o;
            newSmartScript.comment = smartScript.comment;
            return newSmartScript;
        }

        public SmartScript Clone()
        {
            SmartScript newSmartScript = new SmartScript();
            newSmartScript.entryorguid = entryorguid;
            newSmartScript.source_type = source_type;
            newSmartScript.id = id;
            newSmartScript.link = link;
            newSmartScript.event_type = event_type;
            newSmartScript.event_phase_mask = event_phase_mask;
            newSmartScript.event_chance = event_chance;
            newSmartScript.event_flags = event_flags;
            newSmartScript.event_param1 = event_param1;
            newSmartScript.event_param2 = event_param2;
            newSmartScript.event_param3 = event_param3;
            newSmartScript.event_param4 = event_param4;
            newSmartScript.action_type = action_type;
            newSmartScript.action_param1 = action_param1;
            newSmartScript.action_param2 = action_param2;
            newSmartScript.action_param2 = action_param3;
            newSmartScript.action_param4 = action_param4;
            newSmartScript.action_param5 = action_param5;
            newSmartScript.action_param6 = action_param6;
            newSmartScript.target_type = target_type;
            newSmartScript.target_param1 = target_param1;
            newSmartScript.target_param2 = target_param2;
            newSmartScript.target_param3 = target_param3;
            newSmartScript.target_x = target_x;
            newSmartScript.target_y = target_y;
            newSmartScript.target_z = target_z;
            newSmartScript.target_o = target_o;
            newSmartScript.comment = comment;
            return newSmartScript;
        }
    }
}
