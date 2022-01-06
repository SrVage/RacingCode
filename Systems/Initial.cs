using Cinemachine;
using Code.Components;
using Code.GameConfigDescription;
using Code.LevelsLoader;
using Code.MonoBehavioursComponent;
using Code.StatesSwitcher.Events;
using Code.StatesSwitcher.States;
using Leopotam.Ecs;
using TMPro;
using UnityEngine;

namespace Code.Systems
{
    public class Initial:IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly EcsFilter<TapToStart> _play;
        private readonly EcsFilter<Steering> _steering;
        private readonly EcsFilter<InputVector> _input;
        private readonly EcsFilter<CoinNumber> _coins;
        private readonly EcsFilter<Keys> _keys;
        private readonly EcsFilter<LevelFailed> _restartignal;
        private readonly CarCfg _carCfg;
        public void Init()
        {
            var objects = Object.FindObjectsOfType<MonoBehavioursEntity>();
            foreach (var obj in objects)
            {
                var entity = _world.NewEntity();
                obj.Initial(entity, _world);
            }
        }

        public void Run()
        {
            if (_play.IsEmpty())
                return;
            var objects = Object.FindObjectsOfType<MonoBehavioursEntity>();
            foreach (var i in _steering)
            {
                _steering.GetEntity(i).Destroy();
            }            
            foreach (var i in _input)
            {
                _input.GetEntity(i).Destroy();
            }
            
            _world.NewEntity().Get<Steering>();
            foreach (var obj in objects)
            {
                var entity = _world.NewEntity();
                obj.Initial(entity, _world);
            }

            if (_coins.IsEmpty())
            {
                _world.NewEntity().Get<CoinNumber>();
            }
            if (_keys.IsEmpty())
            {
                _world.NewEntity().Get<Keys>();
            }
        }
    }
}