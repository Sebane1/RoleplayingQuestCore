namespace RoleplayingQuestCore
{
    public class QuestText
    {
        int faceExpression = -1;
        int bodyExpression = -1;
        string npcName = "Name here.";
        string dialogue = "Text goes here.";

        public int FaceExpression { get => faceExpression; set => faceExpression = value; }
        public int BodyExpression { get => bodyExpression; set => bodyExpression = value; }
        public string NpcName { get => npcName; set => npcName = value; }
        public string Dialogue { get => dialogue; set => dialogue = value; }
    }
}
