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

        bool _dialogueEndsEarlyWhenHit = false;
        bool _dialogueSkipsToDialogueNumber = false;
        int _dialogueNumberToSkipTo = 0;

        List<BranchingChoice> _branchingChoices = new List<BranchingChoice>();

        public int FaceExpression { get => _faceExpression; set => _faceExpression = value; }
        public int BodyExpression { get => _bodyExpression; set => _bodyExpression = value; }
        public string NpcName { get => _npcName; set => _npcName = value; }
        public string Dialogue { get => _dialogue; set => _dialogue = value; }
        public string DialogueAudio { get => _dialogueAudio; set => _dialogueAudio = value; }
        public bool DialogueEndsEarlyWhenHit { get => _dialogueEndsEarlyWhenHit; set => _dialogueEndsEarlyWhenHit = value; }
        public List<BranchingChoice> BranchingChoices { get => _branchingChoices; set => _branchingChoices = value; }
        public bool DialogueSkipsToDialogueNumber { get => _dialogueSkipsToDialogueNumber; set => _dialogueSkipsToDialogueNumber = value; }
        public int DialogueNumberToSkipTo { get => _dialogueNumberToSkipTo; set => _dialogueNumberToSkipTo = value; }
        public string DialogueBackground { get => _dialogueBackground; set => _dialogueBackground = value; }
    }
}
