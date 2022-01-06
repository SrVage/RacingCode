using Code.Components;
using Code.GameConfigDescription;
using Leopotam.Ecs;
using UnityEngine;
using PlayState = Code.StatesSwitcher.States.PlayState;

namespace Code.Systems.CarControlsSystems
{
    public class BrakeSystem:IEcsRunSystem
    {
        private readonly EcsFilter<PlayState> _playState;
        private readonly EcsFilter<Brake> _brake;
        private readonly EcsFilter<Car, Speed> _car;
        private readonly CarCfg _carCfg;
        public void Run()
        {
            if (_playState.IsEmpty())
                return;
            if (_brake.IsEmpty())
            {
                foreach (var car in _car)
                {
                    ref var br = ref _car.Get1(car).BackRightWheelCol;
                    ref var bl = ref _car.Get1(car).BackLeftWheelCol;
                    ref var fr = ref _car.Get1(car).FrontRightWheelCol;
                    ref var fl = ref _car.Get1(car).FrontLeftWheelCol;
                    br.brakeTorque =0;
                    bl.brakeTorque = 0;
                    fl.brakeTorque = 0;
                    fr.brakeTorque = 0;
                }
                return;
            }
            
            foreach (var brake in _brake)
            {
                ref var brakeForc = ref _brake.Get1(brake).Force;
                var brakeForce = Mathf.Abs(brakeForc);
                foreach (var car in _car)
                {
                    ref var carPhysic = ref _car.Get2(car).Rigidbody;
                    var velocity = carPhysic.velocity.magnitude;
                    var brakeKoeff = Mathf.Lerp(_carCfg.MinSpeedBrakeForce, _carCfg.MaxSpeedBrakeForce,
                        velocity / _carCfg.MaximumSpeed);
                    
                    ref var br = ref _car.Get1(car).BackRightWheelCol;
                    ref var bl = ref _car.Get1(car).BackLeftWheelCol;
                    ref var fr = ref _car.Get1(car).FrontRightWheelCol;
                    ref var fl = ref _car.Get1(car).FrontLeftWheelCol;
                    br.brakeTorque = brakeForce*brakeKoeff;
                    bl.brakeTorque = brakeForce*brakeKoeff;
                    fl.brakeTorque = brakeForce*brakeKoeff;
                    fr.brakeTorque = brakeForce*brakeKoeff;
                }
            }
        }
    }
}