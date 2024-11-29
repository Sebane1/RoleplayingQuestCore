using System.Numerics;
namespace RoleplayingQuestCore
{
    public class QuestObjective
    {
        int territoryId = 0;
        string _objective = "";
        Vector3 _coordinates = new Vector3();
        QuestPointType typeOfQuestPoint = QuestPointType.NPC;
        List<QuestText> _questText = new List<QuestText>();


        public int TerritoryId { get => territoryId; set => territoryId = value; }
        public string Objective { get => _objective; set => _objective = value; }
        public Vector3 Coordinates { get => _coordinates; set => _coordinates = value; }
        public List<QuestText> QuestText { get => _questText; set => _questText = value; }
        public QuestPointType TypeOfQuestPoint { get => typeOfQuestPoint; set => typeOfQuestPoint = value; }

        public enum QuestPointType
        {
            NPC,
            GroundItem,
            TallItem,
            StandAndWait
        }
    }
}
