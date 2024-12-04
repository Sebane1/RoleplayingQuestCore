namespace RoleplayingQuestCore
{
    public class RoleplayingQuest
    {

        string _questAuthor = "Author Name Here";
        string _questName = "Quest Name Goes Here";
        string _questDescription = "Quest Description";
        string _questId = "";
        string _questThumbnailPath = "";
        string _foundPath = "";
        QuestRewardType _typeOfReward = QuestRewardType.None;
        string _questReward = "";
        List<QuestObjective> _questObjectives = new List<QuestObjective>();
        Dictionary<string, NpcCustomization> _npcsPresent = new Dictionary<string, NpcCustomization>();
        QuestContentRating _contentRating = QuestContentRating.AllAges;
        bool _isSubQuest = false;
        bool _hasQuestAcceptancePopup = true;
        public RoleplayingQuest()
        {
            _questId = Guid.NewGuid().ToString();
            SubQuestId = _questId;
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
        public string QuestName { get => _questName; set => _questName = value; }
        public bool IsSubQuest { get => _isSubQuest; set => _isSubQuest = value; }
        public string SubQuestId { get; internal set; }
        public bool HasQuestAcceptancePopup { get => _hasQuestAcceptancePopup; set => _hasQuestAcceptancePopup = value; }
        public QuestRewardType TypeOfReward { get => _typeOfReward; set => _typeOfReward = value; }
        public string QuestReward { get => _questReward; set => _questReward = value; }
        public QuestRewardType TypeOfReward1 { get => _typeOfReward; set => _typeOfReward = value; }
        public string QuestThumbnailPath { get => _questThumbnailPath; set => _questThumbnailPath = value; }
        public string FoundPath { get => _foundPath; set => _foundPath = value; }

        public void CopyAuthorData(RoleplayingQuest currentQuest)
        {
            _questAuthor = currentQuest.QuestAuthor;
            _questName = currentQuest.QuestName;
            _questDescription = currentQuest.QuestDescription;
            _questId = currentQuest.QuestId;
            SubQuestId = Guid.NewGuid().ToString();
        }

        public enum QuestContentRating
        {
            AllAges = 0,
            Teen = 1,
            AdultsOnly = 2
        }
        public enum QuestRewardType
        {
            None = 0,
            SecretMessage = 1,
            DownloadLink = 2,
            MediaFile = 3
        }
    }
}
