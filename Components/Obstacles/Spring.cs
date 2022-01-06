using UnityEngine;

namespace Code.Components.Obstacles
{
    public struct Spring
    {
        public Transform Transform;
        public Vector3 StartPosition;
        public float Distance;
        public float LiftTime;
        public float DescentTime;
    }
}