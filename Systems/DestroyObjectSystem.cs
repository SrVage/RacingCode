using Code.Components;
using Code.Components.Collectable;
using Code.Extensions;
using Code.Services;
using DG.Tweening;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Systems
{
    public class DestroyObjectSystem:IEcsRunSystem
    {
        private readonly EcsFilter<Coin, GameObjectRef, CollectableID, Destroy> _coin;
        private readonly EcsFilter<Live, GameObjectRef, CollectableID, Destroy> _live;
        private readonly EcsFilter<Key, GameObjectRef, CollectableID, Destroy> _key;
        private readonly EcsFilter<SpawnPoint, GameObjectRef, Destroy> _spawn;
        private readonly EcsFilter<DeadlyTouch, Trigger, Destroy> _deadlyTouch;
        private ISaveLoadCollectable _saveLoadCollectable;

        public DestroyObjectSystem(ISaveLoadCollectable saveLoadCollectable)
        {
            _saveLoadCollectable = saveLoadCollectable;
        }

        public void Run()
        {
            foreach (var idx in _key)
            {
                ref var id = ref _key.Get3(idx).ID;
                _saveLoadCollectable.ChangeCollectableByID(id);
                ref var transform = ref _key.Get2(idx).Transform;
                var entity = _key.GetEntity(idx);
                ScaleAndDestroy(ref transform, entity);
            }
            foreach (var idx in _coin)
            {
                ref var id = ref _coin.Get3(idx).ID;
                _saveLoadCollectable.ChangeCollectableByID(id);
                ref var transform = ref _coin.Get2(idx).Transform;
                var entity = _coin.GetEntity(idx);
                ScaleAndDestroy(ref transform, entity);
            }
            foreach (var idx in _live)
            {
                ref var id = ref _live.Get3(idx).ID;
                _saveLoadCollectable.ChangeCollectableByID(id);
                ref var transform = ref _live.Get2(idx).Transform;
                var entity = _live.GetEntity(idx);
                ScaleAndDestroy(ref transform, entity);
            }
            foreach (var idx in _deadlyTouch)
            {
                ref var entity = ref _deadlyTouch.GetEntity(idx);
                entity.Del<Trigger>();
                entity.Del<Destroy>();
            }
            foreach (var spawn in _spawn)
            {
                ref var transform = ref _spawn.Get2(spawn).Transform;
                var entity = _spawn.GetEntity(spawn);
                ScaleAndDestroy(ref transform, entity);
            }
        }

        private static void ScaleAndDestroy(ref Transform transform, EcsEntity entity)
        {
            transform.DOScale(new Vector3(0, 0, 0), 0.5f).OnComplete((() =>
                entity.DestroyWithGameObject()));
        }
    }
}