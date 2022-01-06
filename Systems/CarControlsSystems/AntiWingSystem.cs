using Code.Components;
using Code.GameConfigDescription;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Systems.CarControlsSystems
{
    public class AntiWingSystem:IEcsRunSystem
    {
        private readonly EcsFilter<AntiWing, Speed> _antiwing;
        private readonly CarCfg _carCfg;
        public void Run()
        {
            foreach (var i in _antiwing)
            {
                ref var carPhysic = ref _antiwing.Get2(i).Rigidbody;
                var speed = carPhysic.velocity.magnitude;
                var force = Mathf.Lerp(_carCfg.MinSpeedAntiWingForce, _carCfg.MaxSpeedAntiWingForce,
                    speed / _carCfg.MaximumSpeed);
                ref var antiWing = ref _antiwing.Get1(i).Spoiler;
                carPhysic.AddForceAtPosition(new Vector3(0, -force, 0), antiWing.position, ForceMode.Acceleration);
            }
        }
    }
}