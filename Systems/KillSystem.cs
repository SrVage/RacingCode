using Code.Components;
using Code.StatesSwitcher.Events;
using Code.StatesSwitcher.States;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Systems
{
    public class KillSystem:IEcsRunSystem
    {
        private readonly EcsFilter<DeadlyTouch, Trigger>.Exclude<Destroy> _deadlyTouch;
        private readonly EcsFilter<PlayState> _play;
        private EcsWorld _world;

        public void Run()
        {
            if (_play.IsEmpty())
                return;
            foreach (var idx in _deadlyTouch)
            {
                ref var obj = ref _deadlyTouch.Get2(idx).Entity;
                if (!obj.IsAlive() || obj.IsNull())
                {
                    ref var entit = ref _deadlyTouch.GetEntity(idx);
                    entit.Get<Destroy>();
                    return;
                }
                if (!obj.Has<Car>())
                    return;
                _world.NewEntity().Get<ResetGame>();
                ref var entity = ref _deadlyTouch.GetEntity(idx);
                entity.Get<Destroy>();
            }
        }
    }
}