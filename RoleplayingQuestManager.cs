using Newtonsoft.Json;
using System.Numerics;

namespace RoleplayingQuestCore
{
    public class RoleplayingQuestManager
    {
        private IQuestGameObject _mainPlayer;
        public List<RoleplayingQuest> _questChains = new List<RoleplayingQuest>();
        public Dictionary<string, int> _questProgression = new Dictionary<string, int>();
        public event EventHandler<QuestDisplayObject> OnQuestTextTriggered;
        private float _minimumDistance = 3;

        public float MinimumDistance { get => _minimumDistance; set => _minimumDistance = value; }

        public void LoadMainQuestGameObject(IQuestGameObject gameObject)
        {
            _mainPlayer = gameObject;
        }

        public async void AddQuest(string questPath)
        {
            var questChain = JsonConvert.DeserializeObject<RoleplayingQuest>(await File.ReadAllTextAsync(questPath));
            _questChains.Add(questChain);
            if (!_questProgression.ContainsKey(questChain.QuestId))
            {
                _questProgression[questChain.QuestId] = 0;
            }
        }

        public async void RemoveQuest(RoleplayingQuest quest)
        {
            _questChains.Remove(quest);
            _questProgression.Remove(quest.QuestId);
        }

        public void ProgressNearestQuest()
        {
            foreach (var item in _questChains)
            {
                if (_questProgression.ContainsKey(item.QuestId))
                {
                    var objective = item.QuestObjectives[_questProgression[item.QuestId]];
                    if (objective.TerritoryId == _mainPlayer.TerritoryId)
                    {
                        if (Vector3.Distance(objective.Coordinates, _mainPlayer.Position) < _minimumDistance)
                        {
                            _questProgression[item.QuestId]++;
                            bool objectivesCompleted = _questProgression[item.QuestId] >= item.QuestObjectives.Count;
                            OnQuestTextTriggered?.Invoke(this, new QuestDisplayObject(objective, objectivesCompleted, item.NpcCharacteristics));

                            if (objectivesCompleted)
                            {
                                _questProgression.Remove(item.QuestId);
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
}
