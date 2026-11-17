namespace RoleplayingQuestCore
{
    public class NpcInformation
    {
        string _npcName = "NPC Name Here";
        string _appearanceData = "none.mcdf";
        bool _hideNameplate = true;
        string _nameplateAlias = "";
        string _voice = "";

        public string NpcName { get => _npcName; set => _npcName = value; }
        public string AppearanceData { get => _appearanceData; set => _appearanceData = value; }
        public bool HideNameplate { get => _hideNameplate; set => _hideNameplate = value; }
        public string NameplateAlias { get => _nameplateAlias; set => _nameplateAlias = value; }
        public string Voice { get => _voice; set => _voice = value; }
    }
}
