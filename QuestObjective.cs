using System.Numerics;
namespace RoleplayingQuestCore
{
    public class QuestObjective
    {
        int territoryId = 0;
        string _objective = "";
        Vector3 _coordinates = new Vector3();
        Vector3 _rotation = new Vector3();
        QuestPointType typeOfQuestPoint = QuestPointType.NPC;
        ObjectiveStatusType _objectiveStatus = ObjectiveStatusType.Complete;
        ObjectiveTriggerType _typeOfObjectiveTrigger = ObjectiveTriggerType.NormalInteraction;
        ObjectiveTaskType _typeOfObjectiveTaskType = ObjectiveTaskType.PeacefulInteraction;
        Dictionary<string, Transform> _npcStartingPositions = new Dictionary<string, Transform>();
        List<QuestText> _questText = new List<QuestText>();
        List<QuestObjective> _subObjectives = new List<QuestObjective>();
        string triggerText = "";
        bool _objectiveCompleted = false;
        private bool _isAPrimaryObjective = true;

        public int TerritoryId { get => territoryId; set => territoryId = value; }
        public string Objective { get => _objective; set => _objective = value; }
        public Vector3 Coordinates { get => _coordinates; set => _coordinates = value; }
        public List<QuestText> QuestText { get => _questText; set => _questText = value; }
        public QuestPointType TypeOfQuestPoint { get => typeOfQuestPoint; set => typeOfQuestPoint = value; }
        public ObjectiveStatusType ObjectiveStatus { get => _objectiveStatus; set => _objectiveStatus = value; }
        public ObjectiveTriggerType TypeOfObjectiveTrigger { get => _typeOfObjectiveTrigger; set => _typeOfObjectiveTrigger = value; }
        public string TriggerText { get => triggerText; set => triggerText = value; }
        public Dictionary<string, Transform> NpcStartingPositions { get => _npcStartingPositions; set => _npcStartingPositions = value; }
        public Vector3 Rotation { get => _rotation; set => _rotation = value; }
        public List<QuestObjective> SubObjectives { get => _subObjectives; set => _subObjectives = value; }
        public bool ObjectiveCompleted { get => _objectiveCompleted; set => _objectiveCompleted = value; }
        public bool IsAPrimaryObjective { get => _isAPrimaryObjective; set => _isAPrimaryObjective = value; }

        public List<string> EnumerateCharactersAtObjective()
        {
            List<string> charactersAtObjective = new List<string>();
            foreach (QuestText questText in _questText)
            {
                if (!charactersAtObjective.Contains(questText.NpcName))
                {
                    charactersAtObjective.Add(questText.NpcName);
                }
            }
            return charactersAtObjective;
        }

        public bool SubObjectivesComplete()
        {
            bool completed = true;
            if (_subObjectives.Count > 0)
            {
                foreach (var item in _subObjectives)
                {
                    if (!item.ObjectiveCompleted)
                    {
                        completed = false;
                        break;
                    }
                    completed = item.SubObjectivesComplete();
                }
            }
            return completed;
        }

        public List<QuestObjective> GetAllSubObjectives()
        {
            var subObjectives = new List<QuestObjective>();
            subObjectives.Add(this);
            if (_subObjectives.Count > 0)
            {
                subObjectives.AddRange(_subObjectives);
                foreach (var item in _subObjectives)
                {
                    subObjectives.AddRange(item.GetAllSubObjectives());
                }
            }
            return subObjectives;
        }

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
        public enum ObjectiveTaskType
        {
            PeacefulInteraction = 0,
            SearchArea = 1,
            KillEnemy = 2
        }
        public override string ToString()
        {
            return _objective;
        }
    }
}
