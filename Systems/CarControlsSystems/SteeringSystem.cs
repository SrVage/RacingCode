using Code.Components;
using Code.GameConfigDescription;
using Code.Services;
using Code.StatesSwitcher.States;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Systems.CarControlsSystems
{
    public class SteeringSystem:IEcsRunSystem
    {
        private readonly EcsFilter<PlayState> _playState;
        private readonly EcsFilter<Steering> _steering;
        private readonly EcsFilter<Car, Speed> _car;
        private readonly WheelCfg _wheelCfg;
        private readonly CarCfg _carCfg;
        private readonly EcsWorld _world;
        private float parameter;
        public void Run()
        {
            if (_playState.IsEmpty() || _steering.IsEmpty())
                return;
            foreach (var i in _steering)
            {
                ref var steering = ref _steering.Get1(i).Force;
                foreach (var car in _car)
                {
                    ref var physics = ref _car.Get2(car).Rigidbody;
                    Parameter(physics.velocity.magnitude);
                    var angle = Mathf.Clamp(steering*20/physics.velocity.magnitude,  -MaxAngle(), MaxAngle());
                    ref var fr = ref _car.Get1(car).FrontRightWheelCol;
                    ref var fl = ref _car.Get1(car).FrontLeftWheelCol;
                    if (Mathf.Abs(fr.steerAngle) < Mathf.Abs(angle))
                    {
                        fr.steerAngle = Mathf.LerpAngle(fr.steerAngle, angle, TimeService.deltaTime*SpeedTurn());
                        fl.steerAngle = Mathf.LerpAngle(fr.steerAngle, angle, TimeService.deltaTime*SpeedTurn());
                    }
                    else if (Mathf.Abs(fr.steerAngle) > Mathf.Abs(angle))
                    {
                        fr.steerAngle = Mathf.LerpAngle(fr.steerAngle, angle, TimeService.deltaTime*SpeedReturn());
                        fl.steerAngle = Mathf.LerpAngle(fr.steerAngle, angle, TimeService.deltaTime*SpeedReturn());
                    }
                }
            }
        }

        private float MaxAngle()
        { 
            var angle =  Mathf.Lerp(_wheelCfg.LowSpeedMaxAngle, _wheelCfg.HighSpeedMaxAngle, parameter);
            return angle;
        }

        private float SpeedReturn()
        {
            return Mathf.Lerp(_wheelCfg.ReturnSpeedOnLowSpeed, _wheelCfg.ReturnSpeedOnHighSpeed, parameter);
        }        
        private float SpeedTurn()
        {
            return Mathf.Lerp(_wheelCfg.TurnSpeedOnLowSpeed, _wheelCfg.TurnSpeedOnHighSpeed, parameter);
        }
        

        private void Parameter(float speed)
        {
            if (speed < _wheelCfg.LowSpeedLimit)
                parameter = 0;
            else if (speed > _wheelCfg.HighSpeedLimit)
                parameter = 1;
            else
                parameter = (speed-_wheelCfg.LowSpeedLimit) / (_carCfg.MaximumSpeed-_wheelCfg.HighSpeedLimit);
        }
    }
}