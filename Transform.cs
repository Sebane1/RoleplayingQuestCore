using System.Numerics;

namespace RoleplayingQuestCore
{
    public class Transform
    {
        string _name = "";
        Vector3 _position = new Vector3();
        Vector3 _eulerRotation = new Vector3();
        Vector3 _scale = new Vector3();
        bool _cleanupNpcOnceNoLongerVisible = true;

        public Vector3 Position { get => _position; set => _position = value; }
        public Vector3 EulerRotation { get => _eulerRotation; set => _eulerRotation = value; }
        public Vector3 Scale { get => _scale; set => _scale = value; }
        public string Name { get => _name; set => _name = value; }
        public bool CleanupNpcOnceNoLongerVisible { get => _cleanupNpcOnceNoLongerVisible; set => _cleanupNpcOnceNoLongerVisible = value; }
    }
}
