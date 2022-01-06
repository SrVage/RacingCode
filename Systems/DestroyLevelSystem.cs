using Code.Components;
using Code.StatesSwitcher.Events;
using Code.UI.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Systems
{
    public class DestroyLevelSystem:IEcsRunSystem
    {
        private readonly EcsFilter<ResetGame> _reset;
        private readonly EcsFilter<InputVector> _inputVector;
        private readonly EcsFilter<Steering> _steering;
        private readonly EcsFilter<Car> _car;
        private readonly EcsFilter<SpeedText> _speedText;
        private readonly EcsFilter<LivesText> _livesText;
        private readonly EcsFilter<CoinText> _coinText;
        public void Run()
        {
            if (_reset.IsEmpty())
                return;
            foreach (var i in _car)
            {
                ref var entity = ref _car.GetEntity(i);
                GameObject.Destroy(entity.Get<GameObjectRef>().GameObject);
                entity.Destroy();
            }
            foreach (var i in _steering)
            {
                _steering.GetEntity(i).Destroy();
            }
            foreach (var speed in _speedText)
            {
                _speedText.GetEntity(speed).Destroy();
            }
            foreach (var lives in _livesText)
            {
                _livesText.GetEntity(lives).Destroy();
            }            
            foreach (var coin in _coinText)
            {
                _coinText.GetEntity(coin).Destroy();
            }
        }
    }
}