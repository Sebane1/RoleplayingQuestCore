using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RoleplayingQuestCore
{
    public class Collider
    {
        private float _minimumX;
        private float _maximumX;
        private float _minimumY;
        private float _maximumY;
        private float _minimumZ;
        private float _maximumZ;

        public float MinimumX { get => _minimumX; set => _minimumX = value; }
        public float MaximumX { get => _maximumX; set => _maximumX = value; }
        public float MinimumY { get => _minimumY; set => _minimumY = value; }
        public float MaximumY { get => _maximumY; set => _maximumY = value; }
        public float MinimumZ { get => _minimumZ; set => _minimumZ = value; }
        public float MaximumZ { get => _maximumZ; set => _maximumZ = value; }

        public bool IsPointInsideCollider(Vector3 point)
        {
            return
              point.X >= _minimumX &&
              point.X <= _maximumX &&
              point.Y >= _minimumY &&
              point.Y <= _maximumY &&
              point.Z >= _minimumZ &&
              point.Z <= _maximumZ;
        }
        public bool IsColliderInsiderCollider(Collider collider)
        {
            return 
                collider.MinimumX <= _maximumX &&
                collider.MaximumX >= _minimumX &&
                collider.MinimumY <= _maximumY &&
                collider.MaximumY >= _minimumY &&
                collider.MinimumZ <= _maximumZ &&
                collider.MaximumZ >= _minimumZ;
        }
    }
}
