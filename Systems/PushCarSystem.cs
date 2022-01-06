using Code.Components;
using Code.GameConfigDescription;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Systems
{
    public class PushCarSystem:IEcsRunSystem
    {
        private readonly EcsFilter<Car, Speed, Gliding, Stuck> _car;
        private readonly EcsFilter<Accelerate> _accelerate;
        private readonly EcsFilter<Steering> _steering;
        private readonly CarCfg _carCfg;

        public void Run()
        {
            foreach (var car in _car)
            {
                ref var physic = ref _car.Get2(car).Rigidbody;
                ref var forcePoint = ref _car.Get3(car).Point;
                foreach (var steering in _steering)
                {
                    ref var sideForce = ref _steering.Get1(steering).Force;
                    physic.AddForceAtPosition(physic.transform.right*sideForce*_carCfg.SideStuckForce, forcePoint.position, ForceMode.Impulse);
                }
                foreach (var accelerate in _accelerate)
                {
                    ref var forwardForce = ref _accelerate.Get1(accelerate).Force;
                    physic.AddForceAtPosition(physic.transform.forward*forwardForce*_carCfg.ForwardStuckForce, forcePoint.position, ForceMode.Impulse);
                }
            }
        }
    }
}