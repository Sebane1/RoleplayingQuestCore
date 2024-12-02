using Newtonsoft.Json;
using System.Numerics;

namespace RoleplayingQuestCore
{
    public class RoleplayingQuestManager
    {
        private IQuestGameObject _mainPlayer;
        public Dictionary<string, RoleplayingQuest> _questChains = new Dictionary<string, RoleplayingQuest>();
        public Dictionary<string, int> _questProgression = new Dictionary<string, int>();
        public event EventHandler<QuestDisplayObject> OnQuestTextTriggered;
        private float _minimumDistance = 3;

        public RoleplayingQuestManager(Dictionary<string, RoleplayingQuest> questChains, Dictionary<string, int> questProgression)
        {
            _questChains = questChains;
            _questProgression = questProgression;
        }

        public float MinimumDistance { get => _minimumDistance; set => _minimumDistance = value; }
        public List<QuestObjective> GetActiveQuestChainObjectives()
        {
            List<QuestObjective> list = new List<QuestObjective>();
            for (int i = 0; i < _questChains.Count; i++)
            {
                var value = _questChains.ElementAt(i);
                if (_questProgression.ContainsKey(value.Key))
                {
                    list.Add(value.Value.QuestObjectives[_questProgression[value.Key]]);
                }
                else
                {
                    _questProgression[value.Key] = 0;
                    list.Add(value.Value.QuestObjectives[_questProgression[value.Key]]);
                }
            }
            return list;
        }
        public void LoadMainQuestGameObject(IQuestGameObject gameObject)
        {
            _mainPlayer = gameObject;
        }

        public async void AddQuest(string questPath)
        {
            var questChain = JsonConvert.DeserializeObject<RoleplayingQuest>(await File.ReadAllTextAsync(questPath));
            _questChains[questChain.QuestId] = questChain;
            _questProgression[questChain.QuestId] = 0;
        }

        public async void ReplaceQuest(RoleplayingQuest quest)
        {
            _questChains[quest.QuestId] = quest;
            _questProgression[quest.QuestId] = 0;
        }

        public async void RemoveQuest(RoleplayingQuest quest)
        {
            _questChains.Remove(quest.QuestId);
            _questProgression.Remove(quest.QuestId);
        }

        public void ProgressNearestQuest()
        {
            foreach (var item in _questChains.Values)
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
