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
        public int target_x { get; set; }
        public int target_y { get; set; }
        public int target_z { get; set; }
        public int target_o { get; set; }
        public string comment { get; set; }
    }
}
