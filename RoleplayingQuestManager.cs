using Newtonsoft.Json;
using System.IO.Compression;
using System.Numerics;

namespace RoleplayingQuestCore
{
    public class RoleplayingQuestManager
    {
        private const int V = 3;
        private IQuestGameObject _mainPlayer;
        private Dictionary<string, RoleplayingQuest> _questChains = new Dictionary<string, RoleplayingQuest>();
        private Dictionary<string, string> _completedQuestChains = new Dictionary<string, string>();
        private Dictionary<string, int> _questProgression = new Dictionary<string, int>();
        private string _questInstallFolder;

        public event EventHandler<QuestDisplayObject> OnQuestTextTriggered;
        private float _minimumDistance = 3;
        public event EventHandler OnQuestStarted;
        public event EventHandler OnQuestCompleted;
        public event EventHandler<QuestObjective> OnObjectiveCompleted;
        public event EventHandler<RoleplayingQuest> OnQuestAcceptancePopup;

        public RoleplayingQuestManager(Dictionary<string, RoleplayingQuest> questChains, Dictionary<string, int> questProgression, string questInstallFolder)
        {
            _questChains = questChains;
            _questProgression = questProgression;
            _questInstallFolder = questInstallFolder;
        }

        public float MinimumDistance { get => _minimumDistance; set => _minimumDistance = value; }
        public Dictionary<string, RoleplayingQuest> QuestChains { get => _questChains; set => _questChains = value; }
        public Dictionary<string, string> CompletedQuestChains { get => _completedQuestChains; set => _completedQuestChains = value; }
        public Dictionary<string, int> QuestProgression { get => _questProgression; set => _questProgression = value; }
        public void ScanDirectory()
        {
            if (!string.IsNullOrEmpty(_questInstallFolder))
            {
                foreach (var directory in Directory.EnumerateDirectories(_questInstallFolder))
                {
                    string path = Path.Combine(directory, "main.quest");
                    if (File.Exists(path))
                    {
                        AddQuest(path, false);
                    }
                }
            }
        }

        public void OpenQuestPack(string path)
        {
            ZipFile.ExtractToDirectory(path, Path.Combine(_questInstallFolder, Path.GetFileNameWithoutExtension(path)), true);
        }

        public void ExportQuestPack(string path)
        {
            string zipPath = path + ".qmp";
            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }
            ZipFile.CreateFromDirectory(path, zipPath);
        }
        public Dictionary<RoleplayingQuest, Tuple<int, QuestObjective>> GetActiveQuestChainObjectives(int territoryId)
        {
            Dictionary<RoleplayingQuest, Tuple<int, QuestObjective>> list = new Dictionary<RoleplayingQuest, Tuple<int, QuestObjective>>();
            for (int i = 0; i < _questChains.Count; i++)
            {
                var value = _questChains.ElementAt(i);
                if (!_completedQuestChains.ContainsKey(value.Key))
                {
                    if (_questProgression.ContainsKey(value.Key))
                    {
                        var value2 = _questProgression[value.Key];
                        if (value2 < value.Value.QuestObjectives.Count)
                        {
                            var objective = value.Value.QuestObjectives[value2];
                            if (objective.TerritoryId == territoryId)
                            {
                                list[value.Value] = new Tuple<int, QuestObjective>(value2, objective);
                            }
                        }
                    }
                    else
                    {
                        _questProgression[value.Key] = 0;
                        list[value.Value] = new Tuple<int, QuestObjective>(0, (value.Value.QuestObjectives[_questProgression[value.Key]]));
                    }
                }
            }
            return list;
        }
        public void LoadMainQuestGameObject(IQuestGameObject gameObject)
        {
            _mainPlayer = gameObject;
        }

        public void AddQuest(string questPath, bool resetsProgress = true)
        {
            var questChain = JsonConvert.DeserializeObject<RoleplayingQuest>(File.ReadAllText(questPath));
            questChain.FoundPath = Path.GetDirectoryName(questPath);
            _questChains[questChain.QuestId] = questChain;
            if (resetsProgress)
            {
                _questProgression[questChain.QuestId] = 0;
                if (_completedQuestChains.ContainsKey(questChain.QuestId))
                {
                    _completedQuestChains.Remove(questChain.QuestId);
                }
            }
            else if (!_questProgression.ContainsKey(questChain.QuestId))
            {
                _questProgression[questChain.QuestId] = 0;
            }
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
        public List<QuestObjective> GetCurrentObjectives()
        {
            List<QuestObjective> list = new List<QuestObjective>();
            foreach (var item in _questChains.Values)
            {
                if (_questProgression.ContainsKey(item.QuestId))
                {
                    var questProgression = _questProgression[item.QuestId];
                    if (_questProgression[item.QuestId] > 0)
                    {
                        list.Add(item.QuestObjectives[questProgression]);
                    }
                }
            }
            return list;
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
                                                ////_questChains.Remove(knownQuestItem.QuestId);
                                                ////_questProgression.Remove(knownQuestItem.QuestId);
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
