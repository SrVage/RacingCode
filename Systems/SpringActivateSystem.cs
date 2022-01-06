using Code.Components.Obstacles;
using Code.Components.States;
using DG.Tweening;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Systems
{
    public class SpringActivateSystem:IEcsRunSystem
    {
        private readonly EcsFilter<Spring, Active, Fixed, Translate> _springUp;
        private readonly EcsFilter<Spring, NotActive, Fixed, Translate> _springDown;
        public void Run()
        {
            foreach (var spring in _springUp)
            {
                ref var transform = ref _springUp.Get1(spring).Transform;
                ref var distance = ref _springUp.Get1(spring).Distance;
                ref var startPosition = ref _springUp.Get1(spring).StartPosition;
                ref var liftTime = ref _springUp.Get1(spring).LiftTime;
                var entity = _springUp.GetEntity(spring);
                entity.Del<Fixed>();
                entity.Del<Translate>();
                transform.DOMove(startPosition + Vector3.up * distance, liftTime).SetEase(Ease.InOutSine)
                    .OnComplete(() => entity.Get<Fixed>());
            }
            foreach (var spring in _springDown)
            {
                ref var transform = ref _springUp.Get1(spring).Transform;
                ref var startPosition = ref _springUp.Get1(spring).StartPosition;
                ref var liftTime = ref _springUp.Get1(spring).DescentTime;
                var entity = _springUp.GetEntity(spring);
                entity.Del<Fixed>();
                entity.Del<Translate>();
                transform.DOMove(startPosition, liftTime).SetEase(Ease.InOutSine)
                    .OnComplete(() => entity.Get<Fixed>());
            }
        }
    }
}