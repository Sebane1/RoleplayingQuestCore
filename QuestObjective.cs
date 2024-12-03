using System.Numerics;
namespace RoleplayingQuestCore
{
    public class QuestObjective
    {
        int territoryId = 0;
        string _objective = "";
        Vector3 _coordinates = new Vector3();
        QuestPointType typeOfQuestPoint = QuestPointType.NPC;
        ObjectiveStatusType _objectiveStatus = ObjectiveStatusType.Complete;
        ObjectiveTriggerType _typeOfObjectiveTrigger = ObjectiveTriggerType.NormalInteraction;
        List<QuestText> _questText = new List<QuestText>();
        string triggerText = "";


        public int TerritoryId { get => territoryId; set => territoryId = value; }
        public string Objective { get => _objective; set => _objective = value; }
        public Vector3 Coordinates { get => _coordinates; set => _coordinates = value; }
        public List<QuestText> QuestText { get => _questText; set => _questText = value; }
        public QuestPointType TypeOfQuestPoint { get => typeOfQuestPoint; set => typeOfQuestPoint = value; }
        public ObjectiveStatusType ObjectiveStatus { get => _objectiveStatus; set => _objectiveStatus = value; }
        public ObjectiveTriggerType TypeOfObjectiveTrigger { get => _typeOfObjectiveTrigger; set => _typeOfObjectiveTrigger = value; }
        public string TriggerText { get => triggerText; set => triggerText = value; }

        public enum QuestPointType
        {
            NPC = 0,
            GroundItem = 1,
            TallItem = 2,
            StandAndWait = 3
        }
        public enum ObjectiveStatusType
        {
            Pending = 0,
            Complete = 1
        }

        public enum ObjectiveTriggerType
        {
            NormalInteraction = 0,
            DoEmote = 1,
            SayPhrase = 2
        }

        public override string ToString()
        {
            return _objective;
        }
    }
}
