using Code.Components;
using Code.Components.Boosters;
using Code.GameConfigDescription;
using Code.StatesSwitcher.States;
using Leopotam.Ecs;
using UnityEngine;
using Direction = Code.Enums.Direction;

namespace Code.Systems.CarControlsSystems
{
    public class AccelerateSystem:IEcsRunSystem
    {
        private readonly EcsFilter<PlayState> _playState;
        private readonly EcsFilter<Accelerate> _accelerate;
        private readonly EcsFilter<Car, Speed, CarDirection>.Exclude<Stuck> _car;
        private readonly CarCfg _carCfg;
        private readonly N2OCfg _n2OCfg;

        public void Run()
        {
            if (_playState.IsEmpty())
                return;
            if (_accelerate.IsEmpty())
            {
                GetWheelColliders(0);
                return;
            }
            foreach (var i in _accelerate)
            {
                ref var accelerate = ref _accelerate.Get1(i).Force;
                GetWheelColliders(accelerate);
            }
        }

        private void GetWheelColliders(float accelerate)
        {
            foreach (var car in _car)
            {
                ref var physic = ref _car.Get2(car).Rigidbody;
                ref var direction = ref _car.Get3(car).Direction;
                float maxSpeed = CalculateMaxSpeed(direction);
                var speed = physic.velocity.magnitude;
                ref var br = ref _car.Get1(car).BackRightWheelCol;
                ref var bl = ref _car.Get1(car).BackLeftWheelCol;
                ref var fr = ref _car.Get1(car).FrontRightWheelCol;
                ref var fl = ref _car.Get1(car).FrontLeftWheelCol;
                ref var entity = ref _car.GetEntity(car);
                if (entity.Has<N2OEntity>())
                    maxSpeed *= _n2OCfg.MaxSpeedMultiplier;
                if (speed >= maxSpeed)
                    break;
                Accelerate(ref br, accelerate, ref bl, ref fl, ref fr, ref physic, direction, maxSpeed, ref entity);
            }
        }

        private float CalculateMaxSpeed(Direction direction)
        {
            if (direction == Direction.Forward || direction == Direction.Null)
            {
                return _carCfg.MaximumSpeed;
            }
            else
            {
                return _carCfg.MaximumSpeedBack;
            }
        }

        private void Accelerate(ref WheelCollider br, float accelerate, ref WheelCollider bl, ref WheelCollider fl,  ref WheelCollider fr,ref Rigidbody physic, Direction direction, float maxSpeed, ref EcsEntity entity)
        {
            physic.AddForce(physic.transform.forward * accelerate * _carCfg.ForwardForceMultiply, ForceMode.Acceleration);
            CalculateAccelerate(direction, out float minAccelerate, out float maxAccelerate);
            if (br.isGrounded && bl.isGrounded)
            {
                br.motorTorque = BackWheelAccelerate(accelerate, physic, maxSpeed, minAccelerate, maxAccelerate);
                bl.motorTorque = BackWheelAccelerate(accelerate, physic, maxSpeed, minAccelerate, maxAccelerate);
            }
            else
            {
                entity.Get<Stuck>();
                br.motorTorque = 0;
                bl.motorTorque = 0;
            }
            float multiply = 1;
            if (entity.Has<N2OEntity>())
                multiply = _n2OCfg.ForwardAccelerationMultiplier;
            if (fr.isGrounded)
                fr.motorTorque = multiply*FrontWheelAccelerate(accelerate, physic, maxSpeed, minAccelerate, maxAccelerate);
            if (fl.isGrounded)
                fl.motorTorque = multiply*FrontWheelAccelerate(accelerate, physic, maxSpeed, minAccelerate, maxAccelerate);
        }

        private float FrontWheelAccelerate(float accelerate, Rigidbody physic, float maxSpeed, float minAccelerate, float maxAccelerate)
        {
            return accelerate * Mathf.Lerp(minAccelerate, maxAccelerate,physic.velocity.magnitude/maxSpeed)*_carCfg.AxisRatio;
        }

        private float BackWheelAccelerate(float accelerate, Rigidbody physic, float maxSpeed, float minAccelerate, float maxAccelerate)
        {
            return accelerate * Mathf.Lerp(minAccelerate,
                maxAccelerate,
                physic.velocity.magnitude / maxSpeed) * (1 - _carCfg.AxisRatio);
        }

        private void CalculateAccelerate(Direction direction, out float minAccelerate, out float maxAccelerate)
        {
            if (direction == Direction.Forward || direction == Direction.Null)
            {
                minAccelerate = _carCfg.ZeroSpeedAccelerate;
                maxAccelerate = _carCfg.MaxSpeedAccelerate;
            }
            else
            {
                minAccelerate = _carCfg.ZeroSpeedAccelerateBack;
                maxAccelerate = _carCfg.MaxSpeedAccelerateBack;
            }
        }
    }
}