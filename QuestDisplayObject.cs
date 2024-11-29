namespace RoleplayingQuestCore
{
    public class QuestDisplayObject
    {
        List<QuestText> _questTextList = new List<QuestText> ();
        bool _endOfQuest = false;
        Dictionary<string, NpcCustomization> _npcsPresent = new Dictionary<string, NpcCustomization>();

        public QuestDisplayObject(List<QuestText> questTextList, bool endOfQuest, Dictionary<string, NpcCustomization> npcsPresent)
        {
            _questTextList = questTextList;
            _endOfQuest = endOfQuest;
            _npcsPresent = npcsPresent;
        }
    }
}
