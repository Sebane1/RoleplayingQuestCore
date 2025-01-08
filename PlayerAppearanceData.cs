using static RoleplayingQuestCore.QuestEvent;

namespace RoleplayingQuestCore
{
    public class PlayerAppearanceData
    {
        string _questId = "";
        string _appearanceData = "";
        AppearanceSwapType _appearanceSwapType = AppearanceSwapType.EntireAppearance;

        public string QuestId { get => _questId; set => _questId = value; }
        public string AppearanceData { get => _appearanceData; set => _appearanceData = value; }
        public AppearanceSwapType AppearanceSwapType { get => _appearanceSwapType; set => _appearanceSwapType = value; }
    }
}
