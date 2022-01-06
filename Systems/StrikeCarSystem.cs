using System;
using Code.Components;
using Code.Components.Obstacles;
using Leopotam.Ecs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Systems
{
    public class StrikeCarSystem:IEcsRunSystem
    {
        private readonly EcsFilter<Speed, Strike> _car;
        public void Run()
        {
            foreach (var car in _car)
            {
                ref var carPhysic = ref _car.Get1(car).Rigidbody;
                carPhysic.AddForce(new Vector3(Random.Range(-1f,1f), Random.Range(-1f,1f), Random.Range(-1f,1f))*100, ForceMode.Impulse);
                ref var entity = ref _car.GetEntity(car);
                entity.Del<Strike>();
            }
        }
    }
}