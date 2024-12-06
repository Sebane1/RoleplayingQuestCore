using Newtonsoft.Json;
using System.Numerics;

namespace RoleplayingQuestCore
{
    public class RoleplayingQuestManager
    {
        private const int V = 3;
        private IQuestGameObject _mainPlayer;
        public Dictionary<string, RoleplayingQuest> _questChains = new Dictionary<string, RoleplayingQuest>();
        public Dictionary<string, string> _completedQuestChains = new Dictionary<string, string>();
        public Dictionary<string, int> _questProgression = new Dictionary<string, int>();
        public event EventHandler<QuestDisplayObject> OnQuestTextTriggered;
        private float _minimumDistance = V;
        public event EventHandler OnQuestStarted;
        public event EventHandler OnQuestCompleted;
        public event EventHandler<QuestObjective> OnObjectiveCompleted;
        public event EventHandler<RoleplayingQuest> OnQuestAcceptancePopup;

        public RoleplayingQuestManager(Dictionary<string, RoleplayingQuest> questChains, Dictionary<string, int> questProgression)
        {
            _questChains = questChains;
            _questProgression = questProgression;
        }

        public float MinimumDistance { get => _minimumDistance; set => _minimumDistance = value; }
        public Dictionary<RoleplayingQuest, QuestObjective> GetActiveQuestChainObjectives(int territoryId)
        {
            Dictionary<RoleplayingQuest, QuestObjective> list = new Dictionary<RoleplayingQuest, QuestObjective>();
            for (int i = 0; i < _questChains.Count; i++)
            {
                var value = _questChains.ElementAt(i);
                if (_questProgression.ContainsKey(value.Key))
                {
                    var value2 = _questProgression[value.Key];
                    if (value2 < value.Value.QuestObjectives.Count)
                    {
                        var objective = value.Value.QuestObjectives[value2];
                        if (objective.TerritoryId == territoryId)
                        {
                            list[value.Value] = (objective);
                        }
                    }
                }
                else
                {
                    _questProgression[value.Key] = 0;
                    list[value.Value] = (value.Value.QuestObjectives[_questProgression[value.Key]]);
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
            questChain.FoundPath = Path.GetDirectoryName(questPath);
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

        public void ProgressTriggerQuestObjective(QuestObjective.ObjectiveTriggerType triggerType = QuestObjective.ObjectiveTriggerType.NormalInteraction, string triggerPhrase = "")
        {
            foreach (var item in _questChains.Values)
            {
                if (_questProgression.ContainsKey(item.QuestId))
                {
                    var value = _questProgression[item.QuestId];
                    if (value < item.QuestObjectives.Count)
                    {
                        var objective = item.QuestObjectives[value];
                        if (objective.TerritoryId == _mainPlayer.TerritoryId)
                        {
                            if (Vector3.Distance(objective.Coordinates, _mainPlayer.Position) < _minimumDistance)
                            {
                                bool conditionsToProceedWereMet = false;
                                switch (objective.TypeOfObjectiveTrigger)
                                {
                                    case QuestObjective.ObjectiveTriggerType.NormalInteraction:
                                        conditionsToProceedWereMet = true;
                                        break;
                                    case QuestObjective.ObjectiveTriggerType.DoEmote:
                                        conditionsToProceedWereMet = objective.TriggerText == triggerPhrase;
                                        break;
                                    case QuestObjective.ObjectiveTriggerType.SayPhrase:
                                        conditionsToProceedWereMet = 
                                        objective.TriggerText.ToLower().Replace(" ", "").Contains(triggerPhrase.ToLower().Replace(" ", ""));
                                        break;
                                }
                                if (conditionsToProceedWereMet)
                                {
                                    if (!item.HasQuestAcceptancePopup)
                                    {
                                        OnQuestTextTriggered?.Invoke(this, new QuestDisplayObject(item, objective, delegate
                                        {
                                            var knownQuestItem = item;
                                            var knownObjective = objective;
                                            bool firstObjective = _questProgression[item.QuestId] == 0;
                                            if (firstObjective)
                                            {
                                                OnQuestStarted?.Invoke(this, EventArgs.Empty);
                                            }
                                            _questProgression[knownQuestItem.QuestId]++;
                                            bool objectivesCompleted = _questProgression[item.QuestId] >= item.QuestObjectives.Count;
                                            if (objectivesCompleted)
                                            {
                                                _questChains.Remove(knownQuestItem.QuestId);
                                                _questProgression.Remove(knownQuestItem.QuestId);
                                                _completedQuestChains[knownQuestItem.QuestId] = knownQuestItem.SubQuestId;
                                                OnQuestCompleted?.Invoke(this, EventArgs.Empty);
                                            }
                                            if (!firstObjective && !objectivesCompleted)
                                            {
                                                OnObjectiveCompleted?.Invoke(this, knownObjective);
                                            }
                                        }, item.NpcCustomizations));
                                    }
                                    else
                                    {
                                        OnQuestAcceptancePopup?.Invoke(this, item);
                                    }
                                }
                                break;
                            }
                        }
                    }
                    else
                    {

                    }
                }
            }
        }
    }
}
