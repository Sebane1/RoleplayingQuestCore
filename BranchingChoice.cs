using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoleplayingQuestCore
{
    public class BranchingChoice
    {
        string choiceText = "";
        BranchingChoiceType choiceType;
        RoleplayingQuest roleplayingQuest = new RoleplayingQuest();
        int eventToJumpTo = 0;

        public int EventToJumpTo { get => eventToJumpTo; set => eventToJumpTo = value; }
        public RoleplayingQuest RoleplayingQuest { get => roleplayingQuest; set => roleplayingQuest = value; }
        public BranchingChoiceType ChoiceType { get => choiceType; set => choiceType = value; }
        public string ChoiceText { get => choiceText; set => choiceText = value; }

        #region Legacy
        [Obsolete("This is no longer used. Please use EventToJumpTo property")]
        public int DialogueToJumpTo { set => eventToJumpTo = value; }
        #endregion

        public enum BranchingChoiceType
        {
            SkipToEventNumber,
            BranchingQuestline
        }
    }
}
