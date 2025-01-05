using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoleplayingQuestCore
{
    public class NpcPartyMember
    {
        string _questId = "";
        string _npcName = "";
        List<int> _zoneWhiteList = new List<int>();

        public string QuestId { get => _questId; set => _questId = value; }
        public string NpcName { get => _npcName; set => _npcName = value; }
        public List<int> ZoneWhiteList { get => _zoneWhiteList; set => _zoneWhiteList = value; }

        public bool IsWhitelistedForZone(int currentZone)
        {
            foreach (var zone in _zoneWhiteList)
            {
                if (zone == currentZone)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
