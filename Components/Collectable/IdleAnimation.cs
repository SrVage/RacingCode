using System.Numerics;
using Vector3 = UnityEngine.Vector3;

namespace Code.Components.Collectable
{
    public struct IdleAnimation
    {
        public Enums.IdleAnimation AnimationType;
        public float AnimationSpeed;
        public Vector3 Distance;
        public Vector3 StartPosition;
    }
}