namespace RoleplayingQuestCore
{
    public class QuestText
    {
        private int objectiveNumberToSkipTo = 0;
        int _faceExpression = 0;
        int _bodyExpression = 0;
        bool _loopAnimation = false;
        string _npcName = "Name here.";
        string _dialogue = "Text goes here.";
        string _dialogueAudio = "none.mp3";
        string _dialogueBackground = "none.jpg";
        string _appearanceSwap = "none.mcdf";
        string _objectiveIdToComplete = "";
        int _dialogueBoxStyle = 0;
        

        DialogueEndBehaviourType dialogueEndBehaviour = DialogueEndBehaviourType.None;
        DialogueBackgroundType _dialogueBackgroundType = DialogueBackgroundType.None;
        DialogueConditionType _conditionForDialogueToOccur = DialogueConditionType.None;

        int _dialogueNumberToSkipTo = 0;

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
        public string NpcAlias { get => _npcAlias; set => _npcAlias = value; }
        public string NpcName { get => _npcName; set => _npcName = value; }
        public string Dialogue { get => _dialogue; set => _dialogue = value; }
        public string DialogueAudio { get => _dialogueAudio; set => _dialogueAudio = value; }
        public List<BranchingChoice> BranchingChoices { get => _branchingChoices; set => _branchingChoices = value; }

        public int DialogueNumberToSkipTo { get => _dialogueNumberToSkipTo; set => _dialogueNumberToSkipTo = value; }
        public string DialogueBackground { get => _dialogueBackground; set => _dialogueBackground = value; }
        public DialogueEndBehaviourType DialogueEndBehaviour { get => dialogueEndBehaviour; set => dialogueEndBehaviour = value; }

        [Obsolete("This is no longer used. Please use DialogueEndBehavior property")]
        public bool DialogueSkipsToDialogueNumber
        {
            set
            {
                if (value)
                {
                    dialogueEndBehaviour = DialogueEndBehaviourType.DialogueSkipsToDialogueNumber;
                }
            }
        }

        public DialogueBackgroundType TypeOfDialogueBackground { get => _dialogueBackgroundType; set => _dialogueBackgroundType = value; }
        public int DialogueBoxStyle { get => _dialogueBoxStyle; set => _dialogueBoxStyle = value; }
        public string AppearanceSwap { get => _appearanceSwap; set => _appearanceSwap = value; }
        public bool LoopAnimation { get => _loopAnimation; set => _loopAnimation = value; }
        public int ObjectiveNumberToSkipTo { get => objectiveNumberToSkipTo; set => objectiveNumberToSkipTo = value; }
        public DialogueConditionType ConditionForDialogueToOccur { get => _conditionForDialogueToOccur; set => _conditionForDialogueToOccur = value; }
        public string ObjectiveIdToComplete { get => _objectiveIdToComplete; set => _objectiveIdToComplete = value; }

        public enum DialogueEndBehaviourType
        {
            None = 0,
            DialogueSkipsToDialogueNumber = 1,
            DialogueEndsEarlyWhenHit = 2,
            DialogueEndsEarlyWhenHitNoProgression = 3,
            DialogueEndsEarlyWhenHitAndSkipsToObjective = 4,
        }
        public enum DialogueBackgroundType
        {
            None = 0,
            Image = 1,
            Video = 2,
        }
        public enum DialogueConditionType
        {
            None = 0,
            CompletedSpecificObjectiveId = 1,
        }
    }
}
