namespace SAI_Editor.Classes.Database.Classes
{
    public class Condition
    {
        public int SourceTypeOrReferenceId { get; set; }
        public int SourceGroup { get; set; }
        public int SourceEntry { get; set; }
        public int SourceId { get; set; }
        public int ElseGroup { get; set; }
        public int ConditionTypeOrReference { get; set; }
        public int ConditionTarget { get; set; }
        public int ConditionValue1 { get; set; }
        public int ConditionValue2 { get; set; }
        public int ConditionValue3 { get; set; }
        public int NegativeCondition { get; set; }
        public int ErrorType { get; set; }
        public int ErrorTextId { get; set; }
        public string ScriptName { get; set; }
        public string Comment { get; set; }
    }
}
