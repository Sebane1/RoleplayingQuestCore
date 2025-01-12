using Newtonsoft.Json;
using System.IO.Compression;
using System.Numerics;
using static RoleplayingQuestCore.QuestEvent;

namespace RoleplayingQuestCore
{
    public class RoleplayingQuestManager
    {
        private IQuestGameObject _mainPlayer;
        private Dictionary<string, RoleplayingQuest> _questChains = new Dictionary<string, RoleplayingQuest>();
        private Dictionary<string, List<string>> _completedObjectives = new Dictionary<string, List<string>>();
        private Dictionary<string, string> _completedQuestChains = new Dictionary<string, string>();
        private Dictionary<string, int> _questProgression = new Dictionary<string, int>();
        private Dictionary<string, Dictionary<string, NpcPartyMember>> _npcPartyMembers = new Dictionary<string, Dictionary<string, NpcPartyMember>>();
        private Dictionary<string, PlayerAppearanceData> _playerAppearanceData = new Dictionary<string, PlayerAppearanceData>();
        private string _questInstallFolder = "";

        private float _minimumDistance = 3;
        public event EventHandler<QuestDisplayObject> OnQuestTextTriggered;
        public event EventHandler<RoleplayingQuest> OnQuestStarted;
        public event EventHandler<RoleplayingQuest> OnQuestCompleted;
        public event EventHandler<QuestObjective> OnObjectiveCompleted;
        public event EventHandler<RoleplayingQuest> OnQuestAcceptancePopup;

        public RoleplayingQuestManager(
            Dictionary<string, RoleplayingQuest> questChains, Dictionary<string, int> questProgression,
            Dictionary<string, List<string>> completedObjectives,
            Dictionary<string, Dictionary<string, NpcPartyMember>> npcPartyMembers,
            Dictionary<string, PlayerAppearanceData> playerAppearanceData,
            string questInstallFolder)
        {
            if (questChains != null)
            {
                _questChains = questChains;
            }
            if (questProgression != null)
            {
                _questProgression = questProgression;
            }
            if (completedObjectives != null)
            {
                _completedObjectives = completedObjectives;
            }
            if (!string.IsNullOrEmpty(questInstallFolder))
            {
                _questInstallFolder = questInstallFolder;
            }
            if (npcPartyMembers != null)
            {
                _npcPartyMembers = npcPartyMembers;
            }
            if (playerAppearanceData != null)
            {
                _playerAppearanceData = playerAppearanceData;
            }
        }

        public float MinimumDistance { get => _minimumDistance; set => _minimumDistance = value; }
        public Dictionary<string, RoleplayingQuest> QuestChains { get => _questChains; set => _questChains = value; }
        public Dictionary<string, string> CompletedQuestChains { get => _completedQuestChains; set => _completedQuestChains = value; }
        public Dictionary<string, int> QuestProgression { get => _questProgression; set => _questProgression = value; }
        public IQuestGameObject MainPlayer { get => _mainPlayer; set => _mainPlayer = value; }
        public string QuestInstallFolder
        {
            get
            {
                if (_questInstallFolder.Contains("Program Files"))
                {
                    _questInstallFolder = "";
                }
                return _questInstallFolder;
            }
            set => _questInstallFolder = value;
        }

        public Dictionary<string, Dictionary<string, NpcPartyMember>> NpcPartyMembers { get => _npcPartyMembers; set => _npcPartyMembers = value; }

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

        public void AddPartyMember(NpcPartyMember npcPartyMember)
        {
            if (!_npcPartyMembers.ContainsKey(npcPartyMember.QuestId))
            {
                _npcPartyMembers[npcPartyMember.QuestId] = new Dictionary<string, NpcPartyMember>();
            }
            if (!_npcPartyMembers[npcPartyMember.QuestId].ContainsKey(npcPartyMember.NpcName))
            {
                _npcPartyMembers[npcPartyMember.QuestId][npcPartyMember.NpcName] = npcPartyMember;
            }
            else
            {
                _npcPartyMembers[npcPartyMember.QuestId][npcPartyMember.NpcName].ZoneWhiteList.AddRange(npcPartyMember.ZoneWhiteList);
            }
        }

        public NpcPartyMember GetNpcPartyMember(string questId, string npcName)
        {
            if (_npcPartyMembers.ContainsKey(questId))
            {
                if (_npcPartyMembers[questId].ContainsKey(npcName))
                {
                    return _npcPartyMembers[questId][npcName];
                }
            }
            return null;
        }

        public List<NpcPartyMember> GetPartyMembersForZone(int territory, string discriminator)
        {
            List<NpcPartyMember> list = new List<NpcPartyMember>();
            foreach (var item in _npcPartyMembers)
            {
                foreach (var npcPartyMember in item.Value)
                {
                    if (npcPartyMember.Value.IsWhitelistedForZone(territory) || QuestIdInArea(territory, discriminator, npcPartyMember.Value.QuestId))
                    {
                        list.Add(npcPartyMember.Value);
                    }
                }
            }
            return list;
        }
        public PlayerAppearanceData GetPlayerAppearanceForZone(int territory, string discriminator)
        {
            foreach (var item in _playerAppearanceData)
            {
                if (QuestIdInArea(territory, discriminator, item.Value.QuestId))
                {
                    return item.Value;
                }
            }
            return null;
        }

        public bool QuestIdInArea(int territory, string discriminator, string questId)
        {
            foreach (var item in GetActiveQuestChainObjectivesInZone(territory, discriminator))
            {
                if (item.Item3.QuestId == questId)
                {
                    return true;
                }
            }
            return false;
        }

        public NpcInformation GetNpcInformation(string questId, string name)
        {
            if (_questChains.ContainsKey(questId))
            {
                foreach (var item in _questChains[questId].NpcCustomization)
                {
                    if (item.Value.NpcName == name)
                    {
                        return item.Value;
                    }
                }
            }
            return null;
        }

        public void RemovePartyMember(NpcPartyMember npcPartyMember)
        {
            if (_npcPartyMembers.ContainsKey(npcPartyMember.QuestId))
            {
                _npcPartyMembers[npcPartyMember.QuestId].Remove(npcPartyMember.NpcName);
            }
        }

        public void AddCompletedObjective(RoleplayingQuest quest, QuestObjective questObjective)
        {
            if (!_completedObjectives.ContainsKey(quest.QuestId))
            {
                _completedObjectives[quest.QuestId] = new List<string>();
            }
            var completedObjectiveList = _completedObjectives[quest.QuestId];
            if (!completedObjectiveList.Contains(questObjective.Id))
            {
                completedObjectiveList.Add(questObjective.Id);
            }
        }

        public bool CompletedObjectiveExists(string objectiveId)
        {
            var trimmedObjective = objectiveId.Trim();
            foreach (var item in _completedObjectives)
            {
                if (item.Value.Contains(trimmedObjective))
                {
                    return true;
                }
            }
            return false;
        }

        public void ClearCompletedQuestObjectives(RoleplayingQuest quest)
        {
            _completedObjectives[quest.QuestId] = new List<string>();
        }

        public void RecoverDeletedQuest(RoleplayingQuest quest, string savePath)
        {
            Directory.CreateDirectory(savePath);
            File.WriteAllText(Path.Combine(savePath, "main.quest"), JsonConvert.SerializeObject(quest));
        }
        public void OpenQuestPack(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                ZipFile.ExtractToDirectory(path, Path.Combine(_questInstallFolder, Path.GetFileNameWithoutExtension(path)), true);
            }
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

        public List<Tuple<int, QuestObjective, RoleplayingQuest>> GetActiveQuestChainObjectivesInZone(int territoryId, string discriminator)
        {
            List<Tuple<int, QuestObjective, RoleplayingQuest>> list = new List<Tuple<int, QuestObjective, RoleplayingQuest>>();
            for (int i = 0; i < _questChains.Count; i++)
            {
                var value = _questChains.ElementAt(i);
                if (!_completedQuestChains.ContainsKey(value.Key))
                {
                    var progressIndex = 0;
                    if (_questProgression.ContainsKey(value.Key))
                    {
                        progressIndex = _questProgression[value.Key];
                    }
                    else
                    {
                        _questProgression[value.Key] = progressIndex;
                    }
                    if (progressIndex < value.Value.QuestObjectives.Count)
                    {
                        var questObjectives = value.Value.QuestObjectives[progressIndex].GetAllSubObjectives();
                        foreach (var objective in questObjectives)
                        {
                            if (objective.TerritoryId == territoryId && (objective.UsesTerritoryDiscriminator ? (objective.TerritoryDiscriminator == discriminator) : true))
                            {
                                if (objective.SubObjectivesComplete())
                                {
                                    list.Add(new Tuple<int, QuestObjective, RoleplayingQuest>(progressIndex, objective, value.Value));
                                }
                            }
                        }
                    }
                }
            }
            return list;
        }

        public void LoadMainQuestGameObject(IQuestGameObject gameObject)
        {
            _mainPlayer = gameObject;
        }

        public bool SwapAppearanceData(RoleplayingQuest roleplayingQuest, string name, string appearanceData)
        {
            bool appearanceDataWasReplaced = false;
            bool nameMatchFound = false;
            for (int i = 0; i < roleplayingQuest.NpcCustomization.Count; i++)
            {
                try
                {
                    if (roleplayingQuest.NpcCustomization.ContainsKey(i))
                    {
                        if (roleplayingQuest.NpcCustomization[i].NpcName == name)
                        {
                            nameMatchFound = true;
                            if (roleplayingQuest.NpcCustomization[i].AppearanceData != appearanceData)
                            {
                                roleplayingQuest.NpcCustomization[i].AppearanceData = appearanceData;
                                appearanceDataWasReplaced = true;
                            }
                            break;
                        }
                    }
                }
                catch
                {

                }
            }
            if (!nameMatchFound)
            {
                roleplayingQuest.NpcCustomization[roleplayingQuest.NpcCustomization.Count] = new NpcInformation()
                {
                    NpcName = name,
                    AppearanceData = appearanceData
                };
                appearanceDataWasReplaced = true;
            }
            return appearanceDataWasReplaced;
        }

        public void AddQuest(string questPath, bool resetsProgress = true, bool reloadQuestData = false)
        {
            var quest = JsonConvert.DeserializeObject<RoleplayingQuest>(File.ReadAllText(questPath));
            quest.FoundPath = Path.GetDirectoryName(questPath);
            if (!_questChains.ContainsKey(quest.QuestId) || resetsProgress || reloadQuestData)
            {
                _questChains[quest.QuestId] = quest;
            }
            if (resetsProgress)
            {
                _questProgression[quest.QuestId] = 0;
                if (_completedQuestChains.ContainsKey(quest.QuestId))
                {
                    _completedQuestChains.Remove(quest.QuestId);
                }
                ClearCompletedQuestObjectives(quest);
                foreach (var objective in quest.QuestObjectives)
                {
                    objective.IsAPrimaryObjective = true;
                }
                if (_npcPartyMembers.ContainsKey(quest.QuestId))
                {
                    _npcPartyMembers.Remove(quest.QuestId);
                }
                if (_playerAppearanceData.ContainsKey(quest.QuestId))
                {
                    _playerAppearanceData.Remove(quest.QuestId);
                }
            }
            else if (!_questProgression.ContainsKey(quest.QuestId))
            {
                _questProgression[quest.QuestId] = 0;
            }
        }

        public async void ReplaceQuest(RoleplayingQuest quest)
        {
            quest.NpcCustomizations = _questChains[quest.QuestId].NpcCustomization;
            _questChains[quest.QuestId] = quest;
            _questProgression[quest.QuestId] = 0;
        }

        public async void RemoveQuest(RoleplayingQuest quest)
        {
            _questChains.Remove(quest.QuestId);
            _questProgression.Remove(quest.QuestId);
        }
        public List<Tuple<int, QuestObjective, RoleplayingQuest>> GetCurrentObjectives()
        {
            List<Tuple<int, QuestObjective, RoleplayingQuest>> list = new List<Tuple<int, QuestObjective, RoleplayingQuest>>();
            for (int i = 0; i < _questChains.Count; i++)
            {
                var value = _questChains.ElementAt(i);
                if (!_completedQuestChains.ContainsKey(value.Key))
                {
                    var progressIndex = 0;
                    if (_questProgression.ContainsKey(value.Key))
                    {
                        progressIndex = _questProgression[value.Key];
                    }
                    else
                    {
                        _questProgression[value.Key] = progressIndex;
                    }
                    if (progressIndex < value.Value.QuestObjectives.Count)
                    {
                        var questObjectives = value.Value.QuestObjectives[progressIndex].GetAllSubObjectives();
                        foreach (var objective in questObjectives)
                        {
                            if (objective.SubObjectivesComplete())
                            {
                                list.Add(new Tuple<int, QuestObjective, RoleplayingQuest>(progressIndex, objective, value.Value));
                            }
                        }
                    }
                }
            }
            return list;
        }

        public void SkipToObjective(RoleplayingQuest roleplayingQuest, int objectiveIndex)
        {
            bool firstObjective = !_questProgression.ContainsKey(roleplayingQuest.QuestId) ? true : _questProgression[roleplayingQuest.QuestId] == 0;
            _questProgression[roleplayingQuest.QuestId] = objectiveIndex;
            if (firstObjective)
            {
                OnQuestStarted?.Invoke(this, roleplayingQuest);
            }
        }

        public void AttemptProgressingQuestObjective(QuestObjective.ObjectiveTriggerType triggerType = QuestObjective.ObjectiveTriggerType.NormalInteraction, string triggerPhrase = "", bool ignoreDistance = false)
        {
            foreach (var item in _questChains.Values)
            {
                if (_questProgression.ContainsKey(item.QuestId))
                {
                    var value = _questProgression[item.QuestId];
                    if (value < item.QuestObjectives.Count)
                    {
                        foreach (var objective in item.QuestObjectives[value].GetAllSubObjectives())
                        {
                            if (!objective.ObjectiveCompleted)
                            {
                                if (objective.TerritoryId == _mainPlayer.TerritoryId && triggerType == objective.TypeOfObjectiveTrigger)
                                {
                                    if (Vector3.Distance(objective.Coordinates, _mainPlayer.Position) < _minimumDistance || ignoreDistance)
                                    {
                                        bool conditionsToProceedWereMet = false;
                                        switch (objective.TypeOfObjectiveTrigger)
                                        {
                                            case QuestObjective.ObjectiveTriggerType.NormalInteraction:
                                                conditionsToProceedWereMet = objective.SubObjectivesComplete();
                                                break;
                                            case QuestObjective.ObjectiveTriggerType.DoEmote:
                                                conditionsToProceedWereMet = objective.TriggerText == triggerPhrase && objective.SubObjectivesComplete();
                                                break;
                                            case QuestObjective.ObjectiveTriggerType.SayPhrase:
                                                conditionsToProceedWereMet =
                                                objective.TriggerText.ToLower().Replace(" ", "").Contains(triggerPhrase.ToLower().Replace(" ", ""))
                                                && objective.SubObjectivesComplete();
                                                break;
                                            case QuestObjective.ObjectiveTriggerType.SubObjectivesFinished:
                                                conditionsToProceedWereMet = objective.SubObjectivesComplete();
                                                break;
                                            case QuestObjective.ObjectiveTriggerType.KillEnemy:
                                                conditionsToProceedWereMet =
                                               triggerPhrase.ToLower().Replace(" ", "").Contains(objective.TriggerText.ToLower().Replace(" ", ""))
                                                && objective.SubObjectivesComplete();
                                                break;
                                            case QuestObjective.ObjectiveTriggerType.BoundingTrigger:
                                                ignoreDistance = true;
                                                conditionsToProceedWereMet = objective.Collider.IsPointInsideCollider(_mainPlayer.Position);
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
                                                        OnQuestStarted?.Invoke(this, knownQuestItem);
                                                    }
                                                    knownObjective.TriggerObjectiveCompletion();
                                                    AddCompletedObjective(item, knownObjective);
                                                    if (knownObjective.IsAPrimaryObjective)
                                                    {
                                                        _questProgression[knownQuestItem.QuestId]++;
                                                    }
                                                    bool objectivesCompleted = _questProgression[item.QuestId] >= item.QuestObjectives.Count;
                                                    if (objectivesCompleted)
                                                    {
                                                        _completedQuestChains[knownQuestItem.QuestId] = knownQuestItem.SubQuestId;
                                                        OnQuestCompleted?.Invoke(this, item);
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
                        }
                    }
                    else
                    {

                    }
                }
            }
        }

        public void AddPlayerAppearance(string questId, string customPlayerMcdfPath, AppearanceSwapType apearanceSwapType)
        {
            _playerAppearanceData[questId] = new PlayerAppearanceData()
            {
                AppearanceData = customPlayerMcdfPath,
                QuestId = questId,
                AppearanceSwapType = apearanceSwapType
            };
        }
        public void RemovePlayerAppearance(string questId)
        {
            _playerAppearanceData.Remove(questId);
        }
    }
}
