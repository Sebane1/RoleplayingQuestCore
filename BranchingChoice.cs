namespace RoleplayingQuestCore
{
    public class BranchingChoice
    {
        string _choiceText = "";
        BranchingChoiceType _choiceType;
        RoleplayingQuest _roleplayingQuest = new RoleplayingQuest();
        int _eventToJumpTo = 0;
        int _eventToJumpToFailure = 0;
        int _minimumDiceRoll = 0;
        public int EventToJumpTo { get => _eventToJumpTo; set => _eventToJumpTo = value; }
        public RoleplayingQuest RoleplayingQuest { get => _roleplayingQuest; set => _roleplayingQuest = value; }
        public BranchingChoiceType ChoiceType { get => _choiceType; set => _choiceType = value; }
        public string ChoiceText { get => _choiceText; set => _choiceText = value; }

        #region Legacy
        [Obsolete("This is no longer used. Please use EventToJumpTo property")]
        public int DialogueToJumpTo { set => _eventToJumpTo = value; }
        public int EventToJumpToFailure { get => _eventToJumpToFailure; set => _eventToJumpToFailure = value; }
        public int MinimumDiceRoll { get => _minimumDiceRoll; set => _minimumDiceRoll = value; }
        #endregion

        public enum BranchingChoiceType
        {
            SkipToEventNumber,
            BranchingQuestline,
            RollD20ThenSkipToEventNumber
        }
    }
}
