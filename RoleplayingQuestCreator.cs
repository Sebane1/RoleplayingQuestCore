using AQuestReborn;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RoleplayingQuestCore
{
    public class RoleplayingQuestCreator
    {
        RoleplayingQuest _currentQuest = new RoleplayingQuest();

        public RoleplayingQuest CurrentQuest { get => _currentQuest; }

        public void SaveQuest(string savePath)
        {
            if (_currentQuest != null)
            {
                foreach (var objective in _currentQuest.QuestObjectives)
                {
                    GenerateObjectiveNPCPositions(objective);
                }
                Directory.CreateDirectory(savePath);
                string path = Path.Combine(savePath, "main.quest");
                if (File.Exists(path))
                {
                    try
                    {
                        File.Move(path, path.Replace(".quest", ".quest.bak"), true);
                    }
                    catch
                    {
                        File.Move(path, path.Replace(".quest", ".quest.bak2"), true);
                    }
                }
                try
                {
                    using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(fileStream))
                        {
                            streamWriter.Write(JsonConvert.SerializeObject(_currentQuest));
                        }
                    }
                }
                catch
                {
                    using (FileStream fileStream = new FileStream(path.Replace(".quest", ".quest.bak"), FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(fileStream))
                        {
                            streamWriter.Write(JsonConvert.SerializeObject(_currentQuest));
                        }
                    }
                }
            }
        }

        public void SaveQuestline(RoleplayingQuest roleplayingQuest, string savePath)
        {
            if (roleplayingQuest != null)
            {
                foreach (var objective in _currentQuest.QuestObjectives)
                {
                    GenerateObjectiveNPCPositions(objective);
                }
                File.WriteAllText(Path.Combine(savePath), JsonConvert.SerializeObject(roleplayingQuest));
            }
        }

        public string ObjectiveToStoryScriptFormat(QuestObjective questObjective)
        {
            string storyScript = "";
            foreach (var objective in questObjective.QuestText)
            {
                storyScript += objective.NpcName + "\r\n";
                storyScript += objective.Dialogue + "\r\n";
            }
            return storyScript;
        }

        public void StoryScriptToObjectiveEvents(string script, QuestObjective questObjective)
        {
            string[] storyScriptItems = script.Split("\r\n");
            int index = 0;
            for (int i = 0; i < storyScriptItems.Length; i += 2)
            {
                try
                {
                    while (string.IsNullOrWhiteSpace(storyScriptItems[i]))
                    {
                        i++;
                    }
                    if (index >= questObjective.QuestText.Count || questObjective.QuestText.Count == 0)
                    {
                        var questText = new QuestEvent();
                        questText.NpcName = storyScriptItems[i].Trim().Replace(":", null);
                        questText.Dialogue = storyScriptItems[i + 1].Trim();
                        questObjective.QuestText.Add(questText);
                        index++;
                    }
                    else
                    {
                        questObjective.QuestText[index].NpcName = storyScriptItems[i].Trim().Replace(":", null);
                        questObjective.QuestText[index].Dialogue = storyScriptItems[i + 1].Trim();
                        index++;
                    }
                }
                catch
                {

                }
            }
        }


        public RoleplayingQuest ImportQuestline(string questPath)
        {
            return JsonConvert.DeserializeObject<RoleplayingQuest>(File.ReadAllText(questPath));
        }
        public void EditQuest(RoleplayingQuest currentQuest)
        {
            _currentQuest = currentQuest;
        }
        public void EditQuest(string openPath)
        {
            _currentQuest = JsonConvert.DeserializeObject<RoleplayingQuest>(File.ReadAllText(openPath));
        }
        public void GenerateObjectiveNPCPositions(QuestObjective questObjective)
        {
            var npcsInDialogue = questObjective.EnumerateCharactersAtObjective();
            foreach (var npc in npcsInDialogue)
            {
                if (!questObjective.NpcStartingPositions.ContainsKey(npc))
                {
                    var newTransform = new Transform() { Name = npc };
                    SetStartingTransformData(questObjective, newTransform);
                    questObjective.NpcStartingPositions[npc] = newTransform;
                }
            }
            for (int i = questObjective.NpcStartingPositions.Count - 1; i > -1; i--)
            {
                var transform = questObjective.NpcStartingPositions.ElementAt(i);
                if (!npcsInDialogue.Contains(transform.Key))
                {
                    questObjective.NpcStartingPositions.Remove(transform.Key);
                }
            }
        }

        public void SetStartingTransformData(QuestObjective questObjective, Transform item)
        {
            item.Position = questObjective.Coordinates;
            item.EulerRotation = questObjective.Rotation;
        }

        public void SetStartingTransformDataToPlayer(IQuestGameObject questGameObject, Transform item)
        {
            item.Position = questGameObject.Position;
            item.EulerRotation = new Vector3(0, CoordinateUtility.ConvertRadiansToDegrees(questGameObject.Rotation.Y) + 180, 0);
        }

        public void AddQuestObjective(QuestObjective questObjective)
        {
            _currentQuest.QuestObjectives.Add(questObjective);
        }

        public void AddQuestText(int selectedObjective, QuestEvent questText)
        {
            _currentQuest.QuestObjectives[selectedObjective].QuestText.Add(questText);
        }
    }
}
