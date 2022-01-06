using Code.Components;
using Code.Components.Collectable;
using Code.GameConfigDescription;
using Code.LevelsLoader;
using Code.Services;
using Code.StatesSwitcher.Events;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Systems
{
    public class LiveSystem:IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsWorld _world;
        private readonly EcsFilter<ResetGame> _reset;
        private readonly EcsFilter<CarSpawnPoint> _spawn;
        private EcsEntity _live;
        private readonly PlayerCfg _playerCfg;
        private SaveLoadLives _saveLoadLives;
        
        public void Init()
        {
            _live = _world.NewEntity();
            _saveLoadLives = new SaveLoadLives(_playerCfg.StartLives);
            _live.Get<Lives>().MaximumLive = _playerCfg.MaxLives;
            _live.Get<Lives>().Live = _saveLoadLives.Get();
        }

        public void Run()
        {
            if (!_reset.IsEmpty())
            {
                ref var live = ref _live.Get<Lives>().Live;
                live--;
                if (live < 0)
                {
                    SaveSpawnPointService.Save(0);
                    _saveLoadLives.Clear();
                    _saveLoadLives = null;
                    _world.NewEntity().Get<LostLives>();
                    _saveLoadLives = new SaveLoadLives(0);
                    _live.Get<Lives>().Live = _saveLoadLives.Get();
                }
            }

            if (_live.Has<Increase>())
            {
                ref var live = ref _live.Get<Lives>().Live;
                if (live < _playerCfg.MaxLives)
                {
                    live++;
                    _saveLoadLives.Save(live);
                }
                _live.Del<Increase>();
            }
        }
    }
}