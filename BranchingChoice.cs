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
        int dialogueToJumpTo = 0;

        public int EventToJumpTo { get => dialogueToJumpTo; set => dialogueToJumpTo = value; }
        public RoleplayingQuest RoleplayingQuest { get => roleplayingQuest; set => roleplayingQuest = value; }
        public BranchingChoiceType ChoiceType { get => choiceType; set => choiceType = value; }
        public string ChoiceText { get => choiceText; set => choiceText = value; }

        public enum BranchingChoiceType
        {
            SkipToDialogueNumber,
            BranchingQuestline
        }
    }
}
