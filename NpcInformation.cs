namespace RoleplayingQuestCore
{
    public class NpcInformation
    {
        string _npcName = "NPC Name Here";
        string _appearanceData = "none.mcdf";

        public string NpcName { get => _npcName; set => _npcName = value; }
        public string AppearanceData { get => _appearanceData; set => _appearanceData = value; }
    }
}
