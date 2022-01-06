using Code.Components;
using Code.UI.Components;
using Leopotam.Ecs;

namespace Code.UI.Systems
{
    public class ChangeCoinTextSystem:IEcsRunSystem
    {
        private readonly EcsFilter<CoinText> _text;
        private readonly EcsFilter<CoinNumber> _coin;
        
        public void Run()
        {
            foreach (var coin in _coin)
            {
                ref var coins = ref _coin.Get1(coin).Coins;
                foreach (var text in _text)
                {
                    ref var textSpeed = ref _text.Get1(text).Text;
                    textSpeed.text =  $"Coins: {coins}";
                }
            }
        }
    }
}