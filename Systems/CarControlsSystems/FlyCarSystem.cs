using Code.Components;
using Code.GameConfigDescription;
using Leopotam.Ecs;
using UnityEditor;
using UnityEngine;

namespace Code.Systems.CarControlsSystems
{
    public class FlyCarSystem:IEcsRunSystem
    {
        private readonly EcsFilter<Car, Speed, Flying> _car;
        private readonly EcsFilter<Accelerate> _accelerate;
        private readonly EcsFilter<Steering> _steering;
        private readonly CarCfg _carCfg;
        public void Run()
        {
            foreach (var car in _car)
            {
                ref var physic = ref _car.Get2(car).Rigidbody;
                foreach (var steering in _steering)
                {
                    ref var sideForce = ref _steering.Get1(steering).Force;
                    Vector3 force = new Vector3(physic.transform.right.x, 0, physic.transform.right.z) * sideForce *
                                    _carCfg.SideFlyingForce;
                    
                    physic.AddForce(force, ForceMode.Impulse);
                }
                foreach (var accelerate in _accelerate)
                {
                    ref var forwardForce = ref _accelerate.Get1(accelerate).Force;
                    Vector3 force;
                    if (forwardForce > 0)
                    {
                        force = new Vector3(physic.transform.forward.x, 0, physic.transform.forward.z) * forwardForce * _carCfg.ForwardFlyingForce;
                    }
                    else
                    {
                        force = new Vector3(physic.transform.forward.x, 0, physic.transform.forward.z) * forwardForce * _carCfg.BackwardFlyingForce;
                    }
                    Debug.Log(force);
                    physic.AddForce(force, ForceMode.Impulse);
                }
            }
        }
    }
}