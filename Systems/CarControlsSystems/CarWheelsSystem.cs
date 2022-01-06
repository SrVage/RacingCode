using Code.Components;
using Code.GameConfigDescription;
using Code.StatesSwitcher.States;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Systems.CarControlsSystems
{
    public class CarWheelsSystem : IEcsRunSystem
    {
        private readonly EcsFilter<Car> _car;
        private readonly WheelCfg _wheelCfg;
        private readonly EcsFilter<PlayState> _playState;
        private bool _isSet;
        private int _contactCount = 0;

        public void Run()
        {
            if (_playState.IsEmpty())
            {
                _isSet = false;
                return;
            }

            if (!_isSet)
            {
                _isSet = true;
                foreach (var car in _car)
                {

                    ref var blc = ref _car.Get1(car).BackLeftWheelCol;
                    ChangeWheels(ref blc, false);
                    ref var brc = ref _car.Get1(car).BackRightWheelCol;
                    ChangeWheels(ref brc, false);
                    ref var flc = ref _car.Get1(car).FrontLeftWheelCol;
                    ChangeWheels(ref flc, true);
                    ref var frc = ref _car.Get1(car).FrontRightWheelCol;
                    ChangeWheels(ref frc, true);
                }
            }

            _contactCount = 0;
            foreach (var car in _car)
            {
                var pos = Vector3.zero;
                var rot = Quaternion.identity;
                
                ref var blc = ref _car.Get1(car).BackLeftWheelCol;
                blc.GetWorldPose(out pos, out rot);
                ref var bl = ref _car.Get1(car).BackLeftWheel;
                SetWheelTransform(ref bl, pos, rot);
                if (!blc.isGrounded)
                    _contactCount++;

                ref var brc = ref _car.Get1(car).BackRightWheelCol;
                brc.GetWorldPose(out pos, out rot);
                ref var br = ref _car.Get1(car).BackRightWheel;
                SetWheelTransform(ref br, pos, rot);
                if (!brc.isGrounded)
                    _contactCount++;

                ref var flc = ref _car.Get1(car).FrontLeftWheelCol;
                flc.GetWorldPose(out pos, out rot);
                ref var fl = ref _car.Get1(car).FrontLeftWheel;
                SetWheelTransform(ref fl, pos, rot);
                if (!flc.isGrounded)
                    _contactCount++;

                ref var frc = ref _car.Get1(car).FrontRightWheelCol;
                frc.GetWorldPose(out pos, out rot);
                ref var fr = ref _car.Get1(car).FrontRightWheel;
                SetWheelTransform(ref fr, pos, rot);
                if (!frc.isGrounded)
                    _contactCount++;
                ref var carEntity = ref _car.GetEntity(car);
                if (_contactCount >= 4)
                {
                    carEntity.Get<Flying>();
                }
                else
                {
                    if (carEntity.Has<Flying>())
                        carEntity.Del<Flying>();
                }
            }
        }

        private void ChangeWheels(ref WheelCollider wheel, bool forward)
        {
            wheel.mass = _wheelCfg.Mass;
            if (forward)
                wheel.suspensionDistance = _wheelCfg.ForwardClearance;
            else
                wheel.suspensionDistance = _wheelCfg.BackClearance;
            JointSpring spring = CreateSpring(forward);
            wheel.suspensionSpring = spring;
            if (forward)
                wheel.wheelDampingRate = _wheelCfg.Roll;
            else
                wheel.wheelDampingRate = 0.1f;
            WheelFrictionCurve curve = CreateCurve(forward, false);
            wheel.sidewaysFriction = curve;
            WheelFrictionCurve curve2 = CreateCurve(forward, true);
            wheel.forwardFriction = curve2;
        }

        private JointSpring CreateSpring(bool forward)
        {
            JointSpring spring = new JointSpring();
            if (forward)
            {
                spring.damper = _wheelCfg.ForwardDamping;
                spring.spring = _wheelCfg.ForwardSpring;
            }
            else
            {
                spring.damper = _wheelCfg.BackDamping;
                spring.spring = _wheelCfg.BackSpring;
            }
            spring.targetPosition = 0.5f;
            return spring;
        }

        private WheelFrictionCurve CreateCurve(bool forward, bool side)
        {
            WheelFrictionCurve curve = new WheelFrictionCurve();
            if (!side)
            {
                if (forward)
                    curve.stiffness = _wheelCfg.ForwardSideFriction;
                else
                    curve.stiffness = _wheelCfg.BackSideFriction;
            }
            else
            {
                if (forward)
                    curve.stiffness = _wheelCfg.ForwardFriction;
                else
                    curve.stiffness = _wheelCfg.BackFriction;
            }
            curve.extremumSlip = 0.4f;
            curve.extremumValue = 1;
            curve.asymptoteSlip = 1.6f;
            curve.asymptoteValue = 1;
            return curve;
        }

        private void SetWheelTransform(ref Transform wheel, Vector3 pos, Quaternion rot)
        {
            wheel.position = pos;
            wheel.rotation = rot;
        }
    }
}