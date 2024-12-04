namespace RoleplayingQuestCore
{
    public class QuestDisplayObject
    {
        RoleplayingQuest _roleplayingQuest = new RoleplayingQuest();
        QuestObjective _questTextList = new QuestObjective();
        Dictionary<string, NpcCustomization> _npcsPresent = new Dictionary<string, NpcCustomization>();
        private EventHandler _questEvents;

        public QuestDisplayObject(RoleplayingQuest roleplayingQuest, QuestObjective questTextList, EventHandler questEvents, Dictionary<string, NpcCustomization> npcsPresent)
        {
            _questTextList = questTextList;
            _npcsPresent = npcsPresent;
            _questEvents = questEvents;
            _roleplayingQuest = roleplayingQuest;
        }

        public QuestObjective QuestObjective { get => _questTextList; set => _questTextList = value; }
        public Dictionary<string, NpcCustomization> NpcsPresent { get => _npcsPresent; set => _npcsPresent = value; }
        public EventHandler QuestEvents { get => _questEvents; set => _questEvents = value; }
        public RoleplayingQuest RoleplayingQuest { get => _roleplayingQuest; set => _roleplayingQuest = value; }
    }
}
