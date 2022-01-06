using Code.Components;
using Code.UnityComponents;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.MonoBehavioursComponent
{
    public class CarMonoBehaviour:MonoBehavioursEntity
    {
        [SerializeField] private WheelCollider _frontRightWheelCol;
        [SerializeField] private WheelCollider _backRightWheelCol;
        [SerializeField] private WheelCollider _frontLeftWheelCol;
        [SerializeField] private WheelCollider _backLeftWheelCol;
        [SerializeField] private Transform _frontRightWheel;
        [SerializeField] private Transform _backRightWheel;
        [SerializeField] private Transform _frontLeftWheel;
        [SerializeField] private Transform _backLeftWheel;
        [SerializeField] private Transform _centerOfMass;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Transform _antiwing;
        [SerializeField] private Transform _gliding;

        public override void Initial(EcsEntity entity, EcsWorld world)
        {
            base.Initial(entity, world);
            gameObject.AddComponent<CarTriggerListener>().Init(world, entity, true);
            entity.Get<Car>().FrontRightWheelCol = _frontRightWheelCol;
            entity.Get<Car>().BackRightWheelCol = _backRightWheelCol;
            entity.Get<Car>().FrontLeftWheelCol = _frontLeftWheelCol;
            entity.Get<Car>().BackLeftWheelCol = _backLeftWheelCol;
            entity.Get<Car>().FrontRightWheel = _frontRightWheel;
            entity.Get<Car>().BackRightWheel = _backRightWheel;
            entity.Get<Car>().FrontLeftWheel = _frontLeftWheel;
            entity.Get<Car>().BackLeftWheel = _backLeftWheel;
            entity.Get<Speed>().Rigidbody = _rb;
            entity.Get<Car>().CenterOfMass = _centerOfMass;
            entity.Get<Gliding>().Point = _gliding;

            //entity.Get<AntiWing>().Spoiler = _antiwing;
        }
    }
}