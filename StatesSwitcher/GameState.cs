using Code.StatesSwitcher.States;
using Leopotam.Ecs;

namespace Code.StatesSwitcher
{
    public static class ChangeGameState
    {
        public static EcsWorld World;
        public static void Change(GameStates state)
        {
            World.NewEntity().Get<ChangeState>().States = state;
        }
    }
}