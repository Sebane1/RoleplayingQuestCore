namespace RoleplayingQuestCore
{
    public class QuestDisplayObject
    {
        QuestObjective _questTextList = new QuestObjective();
        bool _endOfQuest = false;
        Dictionary<string, NpcCustomization> _npcsPresent = new Dictionary<string, NpcCustomization>();

        public QuestDisplayObject(QuestObjective questTextList, bool endOfQuest, Dictionary<string, NpcCustomization> npcsPresent)
        {
            _questTextList = questTextList;
            _endOfQuest = endOfQuest;
            _npcsPresent = npcsPresent;
        }

        public QuestObjective QuestObjective { get => _questTextList; set => _questTextList = value; }
        public bool EndOfQuest { get => _endOfQuest; set => _endOfQuest = value; }
        public Dictionary<string, NpcCustomization> NpcsPresent { get => _npcsPresent; set => _npcsPresent = value; }
    }
}
