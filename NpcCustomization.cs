using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoleplayingQuestCore
{
    public class NpcCustomization
    {
        bool _gender;
        int _face;
        int _race;
        int _tribe;
        Dictionary<string, string> _miscData;

        public bool Gender { get => _gender; set => _gender = value; }
        public int Face { get => _face; set => _face = value; }
        public int Race { get => _race; set => _race = value; }
        public int Tribe { get => _tribe; set => _tribe = value; }
    }
}
