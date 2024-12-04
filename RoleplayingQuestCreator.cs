using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
