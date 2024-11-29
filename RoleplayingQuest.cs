namespace RoleplayingQuestCore
{
    public class RoleplayingQuest
    {
        string _questId = "";
        List<QuestObjective> _questObjectives = new List<QuestObjective>();
        Dictionary<string, NpcCustomization> _npcsPresent = new Dictionary<string, NpcCustomization>();

        public string QuestId { get => _questId; set => _questId = value; }
        public List<QuestObjective> QuestObjectives { get => _questObjectives; set => _questObjectives = value; }
        public Dictionary<string, NpcCustomization> NpcCharacteristics { get => _npcsPresent; set => _npcsPresent = value; }
    }
}
