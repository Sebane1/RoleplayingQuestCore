namespace RoleplayingQuestCore
{
    public class RoleplayingQuest
    {

        string _questAuthor = "Author Name Here";
        string questName = "Quest Name Goes Here";
        string _questDescription = "Quest Description";
        string _questId = "";
        List<QuestObjective> _questObjectives = new List<QuestObjective>();
        Dictionary<string, NpcCustomization> _npcsPresent = new Dictionary<string, NpcCustomization>();
        QuestContentRating _contentRating = QuestContentRating.AllAges;

        public RoleplayingQuest()
        {
            _questId = Guid.NewGuid().ToString();
        }

        public string QuestAuthor { get => _questAuthor; set => _questAuthor = value; }
        public string QuestId { get => _questId; set => _questId = value; }
        public List<QuestObjective> QuestObjectives { get => _questObjectives; set => _questObjectives = value; }
        public string QuestDescription { get => _questDescription; set => _questDescription = value; }
        public Dictionary<string, NpcCustomization> NpcCharacteristics { get => _npcsPresent; set => _npcsPresent = value; }

        /// <summary>
        /// Yes, content ratings are needed.
        /// </summary>
        public QuestContentRating ContentRating { get => _contentRating; set => _contentRating = value; }
        public string QuestName { get => questName; set => questName = value; }

        public enum QuestContentRating
        {
            AllAges = 0,
            Teen = 1,
            AdultsOnly = 2
        }
    }
}
