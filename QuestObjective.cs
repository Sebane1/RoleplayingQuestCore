using System.Numerics;
namespace RoleplayingQuestCore
{
    public class QuestObjective
    {
        string _id = "";
        int _territoryId = 0;
        bool _usesTerritoryDiscriminator = false;
        string _territoryDiscriminator = "";
        int _index = 0;
        string _objective = "Quest Objective Here";
        Vector3 _coordinates = new Vector3();
        Vector3 _rotation = new Vector3();
        QuestPointType _typeOfQuestPoint = QuestPointType.NPC;
        ObjectiveStatusType _objectiveStatus = ObjectiveStatusType.Complete;
        ObjectiveTriggerType _typeOfObjectiveTrigger = ObjectiveTriggerType.NormalInteraction;
        Collider _collider = new Collider();
        Dictionary<string, Transform> _npcStartingPositions = new Dictionary<string, Transform>();
        List<QuestEvent> _questText = new List<QuestEvent>();
        List<QuestObjective> _subObjectives = new List<QuestObjective>();

        string triggerText = "";
        uint triggerMonsterIndex = 0;
        bool _objectiveCompleted = false;
        private bool _isAPrimaryObjective = true;
        private bool _objectiveImmediatelySatisfiesParent = false;
        private bool _dontShowOnMap = false;
        private float _maximum3dIndicatorDistance = 48;
        private bool _playerPositionIsLockedDuringEvents = true;
        private bool _objectiveTriggersCutscene = false;

        public QuestObjective()
        {
            _id = Guid.NewGuid().ToString();
        }

        public int TerritoryId { get => _territoryId; set => _territoryId = value; }
        public string Objective { get => _objective; set => _objective = value; }
        public Vector3 Coordinates { get => _coordinates; set => _coordinates = value; }
        public List<QuestEvent> QuestText { get => _questText; set => _questText = value; }
        public QuestPointType TypeOfQuestPoint { get => _typeOfQuestPoint; set => _typeOfQuestPoint = value; }
        public ObjectiveStatusType ObjectiveStatus { get => _objectiveStatus; set => _objectiveStatus = value; }
        public ObjectiveTriggerType TypeOfObjectiveTrigger { get => _typeOfObjectiveTrigger; set => _typeOfObjectiveTrigger = value; }
        public string TriggerText { get => triggerText; set => triggerText = value; }
        public Dictionary<string, Transform> NpcStartingPositions { get => _npcStartingPositions; set => _npcStartingPositions = value; }
        public Vector3 Rotation { get => _rotation; set => _rotation = value; }
        public List<QuestObjective> SubObjectives { get => _subObjectives; set => _subObjectives = value; }
        public bool ObjectiveCompleted { get => _objectiveCompleted; }
        public bool IsAPrimaryObjective { get => _isAPrimaryObjective; set => _isAPrimaryObjective = value; }
        public float Maximum3dIndicatorDistance { get => _maximum3dIndicatorDistance; set => _maximum3dIndicatorDistance = value; }
        public bool DontShowOnMap { get => _dontShowOnMap; set => _dontShowOnMap = value; }
        public bool Invalidate { get; set; }
        public bool ObjectiveImmediatelySatisfiesParent { get => _objectiveImmediatelySatisfiesParent; set => _objectiveImmediatelySatisfiesParent = value; }
        public int Index { get => _index; set => _index = value; }
        public string Id { get => _id; set => _id = value; }
        public string TerritoryDiscriminator { get => _territoryDiscriminator; set => _territoryDiscriminator = value; }
        public bool UsesTerritoryDiscriminator { get => _usesTerritoryDiscriminator; set => _usesTerritoryDiscriminator = value; }
        public Collider Collider { get => _collider; set => _collider = value; }
        public bool PlayerPositionIsLockedDuringEvents { get => _playerPositionIsLockedDuringEvents; set => _playerPositionIsLockedDuringEvents = value; }
        public uint TriggerMonsterIndex { get => triggerMonsterIndex; set => triggerMonsterIndex = value; }
        public bool ObjectiveTriggersCutscene { get => _objectiveTriggersCutscene; set => _objectiveTriggersCutscene = value; }

        public List<string> EnumerateCharactersAtObjective()
        {
            List<string> charactersAtObjective = new List<string>();
            foreach (QuestEvent questText in _questText)
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
            int count = 0;
            if (_subObjectives.Count > 0)
            {
                foreach (var item in _subObjectives)
                {
                    if (item.ObjectiveCompleted && item.SubObjectivesComplete())
                    {
                        count++;
                    }
                    if (item.ObjectiveImmediatelySatisfiesParent && item.ObjectiveCompleted)
                    {
                        foreach (var subObjective in _subObjectives)
                        {
                            subObjective.TriggerObjectiveCompletion();
                        }
                        return true;
                    }
                }
            }
            return count == _subObjectives.Count;
        }

        public List<QuestObjective> GetAllSubObjectives()
        {
            var subObjectives = new List<QuestObjective>();
            if (_isAPrimaryObjective)
            {
                subObjectives.Add(this);
            }
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
            SayPhrase = 2,
            SubObjectivesFinished = 3,
            KillEnemy = 4,
            BoundingTrigger = 5,
        }
        public override string ToString()
        {
            return _objective;
        }

        internal void TriggerObjectiveCompletion()
        {
            _objectiveCompleted = true;
        }

        public void ClearProgression()
        {
            foreach (var item in GetAllSubObjectives())
            {
                item._objectiveCompleted = false;
            }
        }
    }
}
