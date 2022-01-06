using Code.Components;
using Code.UI.Components;
using Leopotam.Ecs;

namespace Code.UI.Systems
{
    public class ChangeLivesSystem:IEcsRunSystem
    {
        private readonly EcsFilter<Lives> _lives;
        private readonly EcsFilter<LivesText> _text;
        public void Run()
        {
            foreach (var coin in _lives)
            {
                ref var currentLives = ref _lives.Get1(coin).Live;
                ref var maximumLives = ref _lives.Get1(coin).MaximumLive;
                foreach (var text in _text)
                {
                    ref var textLives = ref _text.Get1(text).Text;
                    textLives.text =  $"Lives: {currentLives}/{maximumLives}";
                }
            }
        }
    }
}