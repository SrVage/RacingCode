using Code.Components;
using Code.Enums;
using Code.GameConfigDescription;
using Code.StatesSwitcher.States;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Systems.CarControlsSystems
{
    public class AlternativeSteeringSystem:IEcsRunSystem
    {
        private readonly EcsFilter<PlayState> _playState;
        private readonly EcsFilter<Steering> _steering;
        private readonly EcsFilter<Car, Speed, CarDirection>.Exclude<Stuck> _car;
        private readonly WheelCfg _wheelCfg;
        private readonly CarCfg _carCfg;
        private readonly EcsWorld _world;
        private float parameter;
        private Direction _direction;

        public void Run()
        {
            if (_playState.IsEmpty())
                return;
            foreach (var i in _steering)
            {
                ref var steering = ref _steering.Get1(i).Force;
                foreach (var car in _car)
                {
                    ref var physics = ref _car.Get2(car).Rigidbody;
                    _direction = _car.Get3(car).Direction;
                    Parameter(physics.velocity.magnitude);
                    var angle = Mathf.Clamp(steering, -MaxAngle(), MaxAngle());
                    ref var fr = ref _car.Get1(car).FrontRightWheelCol;
                    ref var fl = ref _car.Get1(car).FrontLeftWheelCol;
                    if (Mathf.Abs(fr.steerAngle) < Mathf.Abs(angle))
                    {
                        fr.steerAngle = Mathf.Lerp(fr.steerAngle, angle, Time.deltaTime*SpeedTurn());
                        fl.steerAngle = Mathf.Lerp(fr.steerAngle, angle, Time.deltaTime*SpeedTurn());
                    }

                    else if (Mathf.Abs(fr.steerAngle) > Mathf.Abs(angle))
                    {
                        fr.steerAngle = Mathf.Lerp(fr.steerAngle, angle, Time.deltaTime*SpeedReturn());
                        fl.steerAngle = Mathf.Lerp(fr.steerAngle, angle, Time.deltaTime*SpeedReturn());
                    }
                }
            }
        }

        private float MaxAngle()
        {
                float lowSpeedMaxAngle = 0;
                float highSpeedMaxAngle = 0;
                if (_direction == Direction.Backward)
                {
                    lowSpeedMaxAngle = _wheelCfg.LowSpeedMaxAngleBack;
                    highSpeedMaxAngle = _wheelCfg.HighSpeedMaxAngleBack;
                }
                else
                {
                    lowSpeedMaxAngle = _wheelCfg.LowSpeedMaxAngle;
                    highSpeedMaxAngle = _wheelCfg.HighSpeedMaxAngle;
                }
                var angle =  Mathf.Lerp(lowSpeedMaxAngle, highSpeedMaxAngle, parameter);
                return angle;
            }

            private float SpeedReturn()
            {
                float returnSpeedOnLowSpeed = 0;
                float returnSpeedOnHighSpeed = 0;
                if (_direction == Direction.Backward)
                {
                    returnSpeedOnLowSpeed = _wheelCfg.ReturnSpeedOnLowSpeedBack;
                    returnSpeedOnHighSpeed = _wheelCfg.ReturnSpeedOnHighSpeedBack;
                }
                else
                {
                    returnSpeedOnLowSpeed = _wheelCfg.ReturnSpeedOnLowSpeed;
                    returnSpeedOnHighSpeed = _wheelCfg.ReturnSpeedOnHighSpeed;
                }
                return Mathf.Lerp(returnSpeedOnLowSpeed, returnSpeedOnHighSpeed, parameter);
            }        
            private float SpeedTurn()
            {
                float turnSpeedOnLowSpeed = 0;
                float turnSpeedOnHighSpeed = 0;
                if (_direction == Direction.Backward)
                {
                    turnSpeedOnLowSpeed = _wheelCfg.TurnSpeedOnLowSpeedBack;
                    turnSpeedOnHighSpeed = _wheelCfg.TurnSpeedOnHighSpeedBack;
                }
                else
                {
                    turnSpeedOnLowSpeed = _wheelCfg.TurnSpeedOnLowSpeed;
                    turnSpeedOnHighSpeed = _wheelCfg.TurnSpeedOnHighSpeed;
                }
                return Mathf.Lerp(turnSpeedOnLowSpeed, turnSpeedOnHighSpeed, parameter);
            }
        

            private void Parameter(float speed)
            {
                float lowSpeedLimit = 0;
                float highSpeedLimit = 0;
                float maximumSpeed = 0;
                if (_direction == Direction.Backward)
                {
                    lowSpeedLimit = _wheelCfg.LowSpeedLimitBack;
                    highSpeedLimit = _wheelCfg.HighSpeedLimitBack;
                    maximumSpeed = _carCfg.MaximumSpeedBack;
                }
                else
                {
                    lowSpeedLimit = _wheelCfg.LowSpeedLimit;
                    highSpeedLimit = _wheelCfg.HighSpeedLimit;
                    maximumSpeed = _carCfg.MaximumSpeed;
                }
                if (speed < lowSpeedLimit)
                    parameter = 0;
                else if (speed > highSpeedLimit)
                    parameter = 1;
                else
                    parameter = (speed-lowSpeedLimit) / (maximumSpeed-highSpeedLimit);
            }
        }
    }