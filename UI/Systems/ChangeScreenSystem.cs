using System;
using Code.Components;
using Code.GameConfigDescription;
using Code.MonoBehavioursComponent;
using Code.StatesSwitcher;
using Code.StatesSwitcher.States;
using Leopotam.Ecs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.UI.Systems
{
    public class ChangeScreenSystem:IEcsRunSystem, IEcsInitSystem
    {
        private readonly UIScreen _uiScreen;
        private readonly EcsFilter<UIScreens> _uiScreens;
        private readonly EcsFilter<ChangeState> _state;
        private readonly EcsWorld _world;
        private Transform _canvas;
        
        public void Run()
        {
            if (_state.IsEmpty()) return;
            foreach (var state in _state)
            {
                ref var st = ref _state.Get1(state).States;
                switch (st)
                {
                    case GameStates.StartState:
                        StartScreen(_uiScreen.TTSScreen, true);
                        break;
                    case GameStates.PlayState:
                        StartScreen(_uiScreen.GameplayScreen, true);
                        break;
                    case GameStates.WinState:
                        StartScreen(_uiScreen.WinScreen, true);
                        break;
                    case GameStates.LoseState:
                        break;
                    case GameStates.NextLevelStates:
                        break;
                    case GameStates.RestartStates:
                        break;
                    case GameStates.ConfigStates:
                        StartScreen(_uiScreen.ConfigScreen, true);
                        break;
                        
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void StartScreen(GameObject screenPrefab, bool destroyCurrent)
        {
            var screen = GameObject.Instantiate(screenPrefab, _canvas);
            screen.GetComponent<UIEntity>().Initial(_world);
            foreach (var ui in _uiScreens)
            {
                ref var scree = ref _uiScreens.Get1(ui).Screens;
                scree = Screens.Win;
                ref var go = ref _uiScreens.Get1(ui).Screen;
                if (go != null&&destroyCurrent)
                    GameObject.Destroy(go);
                go = screen;
            }
        }

        public void Init()
        {
            _world.NewEntity().Get<UIScreens>();
            _canvas = Object.FindObjectOfType<Canvas>().transform.GetChild(0).transform;
        }
    }
}