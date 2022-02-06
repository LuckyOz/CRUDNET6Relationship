namespace EFCoreRelationshipTutorial
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public int Demage { get; set; }
        [JsonIgnore]
        public List<Character> Characters { get; set; }
    }
}
