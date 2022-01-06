using Code.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Systems
{
    public class ZeroingSystem:IEcsRunSystem
    {
        private readonly EcsFilter<Speed> _car;
        public void Run()
        {
            if (_car.IsEmpty()) return;
            foreach (var i in _car)
            {
                ref var physic = ref _car.Get1(i).Rigidbody;
                if (physic.velocity.magnitude<0.01f)
                    physic.velocity.Set(0,0,0);
            }
        }
    }
}