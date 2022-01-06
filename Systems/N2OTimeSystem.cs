using Code.Components;
using Code.Components.Boosters;
using Code.GameConfigDescription;
using Code.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Systems
{
    public class N2OTimeSystem:IEcsRunSystem
    {
        private readonly EcsFilter<Car, N2OEffect> _car;
        private readonly N2OCfg _n2OCfg;
        public void Run()
        {
            foreach (var car in _car)
            {
                ref var time = ref _car.Get2(car).Time;
                if (time >= 10)
                {
                    time = _n2OCfg.Duration;
                }
                time -= TimeService.deltaTime;
                if (time <= 0)
                {
                    ref var entity = ref _car.GetEntity(car);
                        entity.Del<N2OEffect>();
                }
            }
        }
    }
}