using UnityEngine;

namespace Code.Components
{
    public struct Car
    {
        public WheelCollider FrontRightWheelCol;
        public WheelCollider BackRightWheelCol;
        public WheelCollider FrontLeftWheelCol;
        public WheelCollider BackLeftWheelCol;
        public Transform FrontRightWheel;
        public Transform BackRightWheel;
        public Transform FrontLeftWheel;
        public Transform BackLeftWheel;
        public Transform CenterOfMass;
    }

}
