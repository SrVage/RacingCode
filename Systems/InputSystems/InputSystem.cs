using Code.Components;
using Code.GameConfigDescription;
using Code.StatesSwitcher.States;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Systems.InputSystems
{
    public class InputSystem:IEcsRunSystem
    {
        //private EcsEntity _clickEntity;
        private readonly EcsWorld _world;
        private readonly EcsFilter<PlayState> _playState;
        private readonly EcsFilter<Steering> _steering;
        private readonly ControlCfg _controlCfg;
        private Vector2 _lastPosition = Vector2.zero;
        public void Run()
        {
            if (_playState.IsEmpty())
                return;
            if (Input.GetMouseButtonDown(0))
            {
                _lastPosition = Input.mousePosition;
            }
            if (Input.GetMouseButton(0))
            {
                if (Vector2.Distance(_lastPosition, Input.mousePosition)<1)
                    return;
                ref var clickEntity = ref _world.NewEntity().Get<InputVector>(); 
                clickEntity.EndPoint = _lastPosition;
                clickEntity.CurrentPoint = Input.mousePosition;
                _lastPosition = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                //_lastPosition = Vector2.zero;
                foreach (var steering in _steering)
                {
                    ref var steer = ref _steering.Get1(steering).Force;
                    steer = 0;
                    break;
                }
            }
        }
    }
}