namespace RoleplayingQuestCore
{
    public class QuestEvent
    {
        private int objectiveNumberToSkipTo = 0;
        int _faceExpression = 0;
        int _bodyExpression = 0;
        bool _loopAnimation = false;
        string _npcName = "Name here.";
        string _dialogue = "Text goes here.";
        string _dialogueAudio = "none.mp3";
        string _eventBackground = "none.jpg";
        string _appearanceSwap = "none.mcdf";
        string _playerAppearanceSwap = "none.mcdf";
        AppearanceSwapType _playerAppearanceApplicationType = AppearanceSwapType.EntireAppearance;
        string _objectiveIdToComplete = "";
        int _dialogueBoxStyle = 0;


        EventBehaviourType _eventEndBehaviour = EventBehaviourType.None;
        EventBackgroundType _eventBackgroundType = EventBackgroundType.None;
        EventConditionType _conditionForEventToOccur = EventConditionType.None;

        int _eventNumberToSkipTo = 0;

        List<BranchingChoice> _branchingChoices = new List<BranchingChoice>();
        private string _npcAlias = "";

        public int FaceExpression
        {
            get => _faceExpression;

            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                _faceExpression = value;
            }
        }

        public int BodyExpression
        {
            get => _bodyExpression;

            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                _bodyExpression = value;
            }
        }
        #region Legacy

        [Obsolete("This is no longer used. Please use EventNumberToSkipTo property")]
        public int DialogueNumberToSkipTo { set => _eventNumberToSkipTo = value; }
        [Obsolete("This is no longer used. Please use EventBackground property")]
        public string DialogueBackground { set => _eventBackground = value; }
        [Obsolete("This is no longer used. Please use EventEndBehaviour property")]
        public EventBehaviourType DialogueEndBehaviour { set => _eventEndBehaviour = value; }
        [Obsolete("This is no longer used. Please use TypeOfEventBackground property")]
        public EventBackgroundType TypeOfDialogueBackground { set => _eventBackgroundType = value; }

        #endregion
        public string NpcAlias { get => _npcAlias; set => _npcAlias = value; }
        public string NpcName { get => _npcName; set => _npcName = value; }
        public string Dialogue { get => _dialogue; set => _dialogue = value; }
        public string DialogueAudio { get => _dialogueAudio; set => _dialogueAudio = value; }
        public List<BranchingChoice> BranchingChoices { get => _branchingChoices; set => _branchingChoices = value; }

        public int EventNumberToSkipTo { get => _eventNumberToSkipTo; set => _eventNumberToSkipTo = value; }
        public string EventBackground { get => _eventBackground; set => _eventBackground = value; }
        public EventBehaviourType EventEndBehaviour { get => _eventEndBehaviour; set => _eventEndBehaviour = value; }


        public EventBackgroundType TypeOfEventBackground { get => _eventBackgroundType; set => _eventBackgroundType = value; }
        public int DialogueBoxStyle { get => _dialogueBoxStyle; set => _dialogueBoxStyle = value; }
        public string AppearanceSwap { get => _appearanceSwap; set => _appearanceSwap = value; }
        public bool LoopAnimation { get => _loopAnimation; set => _loopAnimation = value; }
        public int ObjectiveNumberToSkipTo { get => objectiveNumberToSkipTo; set => objectiveNumberToSkipTo = value; }
        public EventConditionType ConditionForDialogueToOccur { get => _conditionForEventToOccur; set => _conditionForEventToOccur = value; }
        public string ObjectiveIdToComplete { get => _objectiveIdToComplete; set => _objectiveIdToComplete = value; }
        public string PlayerAppearanceSwap { get => _playerAppearanceSwap; set => _playerAppearanceSwap = value; }
        public AppearanceSwapType PlayerAppearanceSwapType { get => _playerAppearanceApplicationType; set => _playerAppearanceApplicationType = value; }

        public enum EventBehaviourType
        {
            None = 0,
            EventSkipsToDialogueNumber = 1,
            EventEndsEarlyWhenHit = 2,
            EventEndsEarlyWhenHitNoProgression = 3,
            EventEndsEarlyWhenHitAndSkipsToObjective = 4,
            EventEndsEarlyWhenHitAndNPCFollowsPlayer = 5,
            EventEndsEarlyWhenHitAndNPCStopsFollowingPlayer = 6,
            NPCFollowsPlayer = 7,
            NPCStopsFollowingPlayer = 8
        }
        public enum EventBackgroundType
        {
            None = 0,
            Image = 1,
            Video = 2,
            ImageTransparent = 3,
        }
        public enum EventConditionType
        {
            None = 0,
            CompletedSpecificObjectiveId = 1,
        }
        public enum AppearanceSwapType
        {
            EntireAppearance = 0,
            RevertAppearance = 1,
            PreserveRace = 2,
            PreserveMasculinityAndFemininity = 3,
            PreserveAllPhysicalTraits = 4,
            OnlyGlamourerData = 5,
            OnlyCustomizeData = 6
        }
    }
}
