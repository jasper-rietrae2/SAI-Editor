namespace SAI_Editor.Classes.Database.Classes
{
    public class Creature
    {
        public int guid { get; set; }
        public int id { get; set; }
        public int map { get; set; }
        public int spawnMask { get; set; }
        public int phaseMask { get; set; }
        public int modelid { get; set; }
        public int equipment_id { get; set; }
        public int position_x { get; set; }
        public int position_y { get; set; }
        public int position_z { get; set; }
        public int orientation { get; set; }
        public int spawntimesecs { get; set; }
        public int spawndist { get; set; }
        public int currentwaypoint { get; set; }
        public int curhealth { get; set; }
        public int curmana { get; set; }
        public int movementType { get; set; }
        public int npcflag { get; set; }
        public int unit_flags { get; set; }
        public int dynamicflags { get; set; }
    }
}
