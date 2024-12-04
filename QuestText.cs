namespace RoleplayingQuestCore
{
    public class QuestText
    {
        int _faceExpression = -1;
        int _bodyExpression = -1;
        string _npcName = "Name here.";
        string _dialogue = "Text goes here.";
        string _dialogueAudio = "none.mp3";
        string _dialogueBackground = "none.jpg";

        DialogueEndBehaviourType dialogueEndBehaviour = DialogueEndBehaviourType.None;
        DialogueBackgroundType _dialogueBackgroundType = DialogueBackgroundType.None;

        int _dialogueNumberToSkipTo = 0;

        List<BranchingChoice> _branchingChoices = new List<BranchingChoice>();

        public int FaceExpression { get => _faceExpression; set => _faceExpression = value; }
        public int BodyExpression { get => _bodyExpression; set => _bodyExpression = value; }
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

        public enum DialogueEndBehaviourType
        {
            None = 0,
            DialogueSkipsToDialogueNumber = 1,
            DialogueEndsEarlyWhenHit = 2,
        }
        public enum DialogueBackgroundType
        {
            None = 0,
            Image = 1,
            Video = 2,
        }
    }
}
