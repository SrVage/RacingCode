using Code.Components;
using Code.StatesSwitcher;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Systems
{
    public class WinInitializeSystem:IEcsRunSystem
    {
        private readonly EcsFilter<Trigger, Finish> _trigger;
        public void Run()
        {
            if (_trigger.IsEmpty()) 
                return;
            foreach (var trigger in _trigger)
            {
                ref var entity = ref _trigger.Get1(trigger).Entity;
                if (entity.Has<Car>())
                {
                    ChangeGameState.Change(GameStates.WinState);
                    _trigger.GetEntity(trigger).Del<Trigger>();
                }
            }
        }
    }
}