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
                File.WriteAllText(Path.Combine(savePath, "main.quest"), JsonConvert.SerializeObject(_currentQuest));
            }
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
            item.EulerRotation = questGameObject.Rotation;
        }

        public void AddQuestObjective(QuestObjective questObjective)
        {
            _currentQuest.QuestObjectives.Add(questObjective);
        }

        public void AddQuestText(int selectedObjective, QuestText questText)
        {
            _currentQuest.QuestObjectives[selectedObjective].QuestText.Add(questText);
        }
    }
}
