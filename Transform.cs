using System.Numerics;

namespace RoleplayingQuestCore
{
    public class Transform
    {
        string _name = "";
        int _defaultAnimationId = 0;
        Vector3 _position = new Vector3();
        Vector3 _eulerRotation = new Vector3();
        Vector3 _transformScale = new Vector3(1,1,1);
        bool _cleanupNpcOnceNoLongerVisible = false;

        public Vector3 Position { get => _position; set => _position = value; }
        public Vector3 EulerRotation { get => _eulerRotation; set => _eulerRotation = value; }
        public Vector3 TransformScale { get => _transformScale; set => _transformScale = value; }
        public string Name { get => _name; set => _name = value; }
        public int DefaultAnimationId { get => _defaultAnimationId; set => _defaultAnimationId = value; }
        public bool CleanupNpcOnceNoLongerVisible { get => _cleanupNpcOnceNoLongerVisible; set => _cleanupNpcOnceNoLongerVisible = value; }
    }
}
