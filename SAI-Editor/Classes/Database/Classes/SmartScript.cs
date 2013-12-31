namespace SAI_Editor.Classes.Database.Classes
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
            newSmartScript.entryorguid = this.entryorguid;
            newSmartScript.source_type = this.source_type;
            newSmartScript.id = this.id;
            newSmartScript.link = this.link;
            newSmartScript.event_type = this.event_type;
            newSmartScript.event_phase_mask = this.event_phase_mask;
            newSmartScript.event_chance = this.event_chance;
            newSmartScript.event_flags = this.event_flags;
            newSmartScript.event_param1 = this.event_param1;
            newSmartScript.event_param2 = this.event_param2;
            newSmartScript.event_param3 = this.event_param3;
            newSmartScript.event_param4 = this.event_param4;
            newSmartScript.action_type = this.action_type;
            newSmartScript.action_param1 = this.action_param1;
            newSmartScript.action_param2 = this.action_param2;
            newSmartScript.action_param2 = this.action_param3;
            newSmartScript.action_param4 = this.action_param4;
            newSmartScript.action_param5 = this.action_param5;
            newSmartScript.action_param6 = this.action_param6;
            newSmartScript.target_type = this.target_type;
            newSmartScript.target_param1 = this.target_param1;
            newSmartScript.target_param2 = this.target_param2;
            newSmartScript.target_param3 = this.target_param3;
            newSmartScript.target_x = this.target_x;
            newSmartScript.target_y = this.target_y;
            newSmartScript.target_z = this.target_z;
            newSmartScript.target_o = this.target_o;
            newSmartScript.comment = this.comment;
            return newSmartScript;
        }
    }
}
