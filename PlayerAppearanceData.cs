using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoleplayingQuestCore
{
    public class PlayerAppearanceData
    {
        string _questId = "";
        string _appearanceData = "";
        bool _appearanceReplacesBodyTraits = false;

        public string QuestId { get => _questId; set => _questId = value; }
        public string AppearanceData { get => _appearanceData; set => _appearanceData = value; }
        public bool AppearanceReplacesBodyTraits { get => _appearanceReplacesBodyTraits; set => _appearanceReplacesBodyTraits = value; }
    }
}
