using System;
using Code.Components;
using Code.Components.Collectable;
using Code.GameConfigDescription;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Systems
{
    public class CollectionSystem:IEcsRunSystem
    {
        private readonly EcsFilter<Coin, GameObjectRef, Trigger>.Exclude<Destroy> _coin;
        private readonly EcsFilter<Live, GameObjectRef, Trigger>.Exclude<Destroy> _live;
        private readonly EcsFilter<Key, GameObjectRef, Trigger>.Exclude<Destroy> _key;
        private readonly EcsFilter<Lives> _lives;
        private readonly EcsFilter<Keys> _keys;
        private readonly EcsFilter<CoinNumber> _coinsNumber;
        private readonly PlayerCfg _playerCfg;
        public void Run()
        {
            foreach (var coin in _coin)
            {
                ref var obj = ref _coin.Get3(coin).Entity;
                if (!obj.Has<Car>())
                    return;
                ref var entity = ref _coin.GetEntity(coin);
                entity.Get<Destroy>();
                foreach (var coins in _coinsNumber)
                {
                    ref var number = ref _coinsNumber.Get1(coins).Coins;
                    number++;
                }
            }
            foreach (var live in _live)
            {
                ref var obj = ref _live.Get3(live).Entity;
                if (!obj.Has<Car>())
                    return;
                ref var entity = ref _live.GetEntity(live);
                entity.Get<Destroy>();
                foreach (var lives in _lives)
                {
                    ref var currentLive = ref _lives.GetEntity(lives);
                    currentLive.Get<Increase>();
                }
            }
            foreach (var key in _key)
            {
                ref var obj = ref _key.Get3(key).Entity;
                if (!obj.Has<Car>())
                    return;
                ref var entity = ref _key.GetEntity(key);
                entity.Get<Destroy>();
                foreach (var idx in _keys)
                {
                    ref var currentKey = ref _keys.Get1(idx).Key;
                    currentKey++;
                }
            }
        }
    }
}