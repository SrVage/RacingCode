using Code.Components;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Systems.UnityHelpersSystems
{
    public class PauseSystem:IEcsRunSystem
    {
        private readonly EcsFilter<Car> _car;
        public void Run()
        {
            if (Input.GetKeyDown(KeyCode.P))
                Time.timeScale = 0.01f;
            if (Input.GetKeyDown(KeyCode.O))
                Time.timeScale = 1f;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                foreach (var car in _car)
                {
                    ref var entity = ref _car.GetEntity(car);
                    if (entity.Has<Stuck>())
                        entity.Del<Stuck>();
                    else
                        entity.Get<Stuck>();
                }
            }
        }
    }
}