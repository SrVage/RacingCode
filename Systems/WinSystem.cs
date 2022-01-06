using Code.Components;
using Code.LevelsLoader;
using Code.StatesSwitcher.States;
using Code.UI.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Systems
{
    public class WinSystem:IEcsRunSystem
    {
        private readonly EcsFilter<WinState> _win;
        private readonly EcsFilter<Car> _car;
        private readonly EcsFilter<CarSpawnPoint> _carSpawnPoint;
        private readonly EcsFilter<SpeedText> _speedText;
        private readonly EcsFilter<CoinText> _coinText;
        public void Run()
        {
            if (_win.IsEmpty())
                return;
            foreach (var i in _car)
            {
                ref var car = ref _car.GetEntity(i);
                if (car.Has<MoveTo>())
                    break;
                SaveSpawnPointService.Save(0);
                foreach (var j in _carSpawnPoint)
                {
                    ref var target = ref _carSpawnPoint.Get1(j).NextPoint;
                    car.Get<MoveTo>().Target = target;
                }
            }
            
            foreach (var speed in _speedText)
            {
                _speedText.GetEntity(speed).Destroy();
            }
            foreach (var coin in _coinText)
            {
                _coinText.GetEntity(coin).Destroy();
            }
        }
    }
}