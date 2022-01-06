using Code.Components;
using Code.StatesSwitcher;
using Code.StatesSwitcher.Events;
using Leopotam.Ecs;
using UnityEngine;
namespace Code.Systems

{
    public class ChangeStateSystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsWorld _world;
        private readonly EcsFilter<TapToStart> _tts;
        private readonly EcsFilter<ResetGame> _reset;
        private readonly EcsFilter<NextLevel> _next;
        private readonly EcsFilter<StartConfig> _config;
        private readonly EcsFilter<LostLives> _lost;

        public void Run()
        {
            if (!_tts.IsEmpty())
            {
                foreach (var tts in _tts)
                {
                    _tts.GetEntity(tts).Destroy();
                }
                ChangeGameState.Change(GameStates.PlayState);
            }
            if (!_reset.IsEmpty())
            {
                foreach (var reset in _reset)
                {
                    _reset.GetEntity(reset).Destroy();
                }
                ChangeGameState.Change(GameStates.StartState);
            }
            if (!_next.IsEmpty())
            {
                foreach (var next in _next)
                {
                    _next.GetEntity(next).Destroy();
                }
                ChangeGameState.Change(GameStates.NextLevelStates);
            }           
            if (!_config.IsEmpty())
            {
                foreach (var config in _config)
                {
                    _config.GetEntity(config).Destroy();
                }
                ChangeGameState.Change(GameStates.ConfigStates);
            }

            if (!_lost.IsEmpty())
            {
                foreach (var lost in _lost)
                {
                    _lost.GetEntity(lost).Destroy();
                }
                ChangeGameState.Change(GameStates.RestartStates);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                ChangeGameState.Change(GameStates.LoseState);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                ChangeGameState.Change(GameStates.WinState);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
               // ChangeGameState.Change(GameStates.PlayState);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                ChangeGameState.Change(GameStates.StartState);
            }
            
        }

        public void Init()
        {
            ChangeGameState.Change(GameStates.StartState);
        }
    }
}