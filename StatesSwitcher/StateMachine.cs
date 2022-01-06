using Code.LevelsLoader;
using Code.StatesSwitcher.States;
using Leopotam.Ecs;

namespace Code.StatesSwitcher
{
    public enum GameStates
    {
        StartState = 0,
        PlayState = 1,
        WinState = 2,
        LoseState = 3, 
        NextLevelStates = 4,
        ConfigStates = 5,
        RestartStates = 6
    }
    public class StateMachine:IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsWorld _world;
        private readonly EcsFilter<GameState> _state;
        private readonly EcsFilter<ChangeState> _changeState;

        public void Run()
        {
            if (_changeState.IsEmpty()) return;
            foreach (var change in _changeState)
            {
                ref var changeState = ref _changeState.Get1(change).States;
                foreach (var state in _state)
                {
                    _state.GetEntity(state).Destroy();
                    break;
                }

                var newState = _world.NewEntity();
                newState.Get<GameState>();
                switch (changeState)
                {
                    case GameStates.StartState:
                        newState.Get<StartState>();
                        break;
                    case GameStates.PlayState:
                        newState.Get<PlayState>();
                        break;
                    case GameStates.WinState:
                        newState.Get<WinState>();
                        break;
                    case GameStates.LoseState:
                        newState.Get<LoseState>();
                        break;
                    case GameStates.NextLevelStates:
                        newState.Get<LoadLevelSignal>();
                        //newState.Get<StartState>();
                        break;
                    case GameStates.RestartStates:
                        newState.Get<RestartLevelSignal>();
                        break;
                    case GameStates.ConfigStates:
                        break;
                }
            }
        }

        public void Init()
        {
            var entity = _world.NewEntity();
            entity.Get<GameState>();
            entity.Get<StartState>();
        }
    }
}