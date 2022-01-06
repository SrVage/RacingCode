using Code.Components.Obstacles;
using Code.Services;
using Leopotam.Ecs;

namespace Code.Systems
{
    public class DelaySystem:IEcsRunSystem
    {
        private readonly EcsFilter<Delay> _delay;
        
        public void Run()
        {
            foreach (var delay in _delay)
            {
                ref var delayTime = ref _delay.Get1(delay).Time;
                delayTime -= TimeService.deltaTime;
                if (delayTime <= 0)
                    _delay.GetEntity(delay).Del<Delay>();
            }
        }
    }
}