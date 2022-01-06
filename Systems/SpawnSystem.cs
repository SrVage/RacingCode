using Code.Components;
using Code.Components.Collectable;
using Code.Components.States;
using Code.LevelsLoader;
using Code.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Systems
{
    public class SpawnSystem:IEcsRunSystem
    {
        private readonly EcsFilter<SpawnPoint, Trigger, Active, GameObjectRef, CollectableID> _trigger;
        private readonly EcsFilter<CarSpawnPoint> _spawn;
        private ISaveLoadCollectable _saveLoadCollectable;

        public SpawnSystem(ISaveLoadCollectable saveLoadCollectable)
        {
            _saveLoadCollectable = saveLoadCollectable;
        }

        public void Run()
        {
            foreach (var trigger in _trigger)
            {
                ref var triggerObject = ref _trigger.Get2(trigger).Entity;
                if (triggerObject.Has<Car>())
                {
                    ref var entity = ref _trigger.GetEntity(trigger);
                    ref var currentPosition = ref _trigger.Get1(trigger).Point;
                    ref var currentNumber = ref _trigger.Get1(trigger).Number;
                    ref var gameObject = ref _trigger.Get4(trigger).GameObject;
                    ref var id = ref _trigger.Get5(trigger).ID;
                    _saveLoadCollectable.ChangeCollectableByID(id);
                    gameObject.GetComponent<BoxCollider>().enabled = false;
                    entity.Del<Active>();
                    entity.Del<IdleAnimation>();
                    entity.Del<Trigger>();
                    entity.Get<Translate>();
                    
                    foreach (var spawn in _spawn)
                    {
                        ref var spawnPoint = ref _spawn.Get1(spawn).SpawnPosition;
                        spawnPoint = currentPosition;
                        SaveSpawnPointService.Save(currentNumber);
                    }
                }
            }
        }
    }
}