using Code.Components;
using Code.Components.Obstacles;
using Code.GameConfigDescription;
using DG.Tweening;
using Leopotam.Ecs;
using UnityEngine;
using Tween = Code.Components.Obstacles.Tween;

namespace Code.Systems
{
    public class RotatorSystem:IEcsRunSystem
    {
        private readonly EcsFilter<Rotate, Single>.Exclude<Doing, Delay> _rotateSingle;
        private readonly EcsFilter<Rotate, Tween>.Exclude<Doing, Delay> _rotateTween;
        private readonly SemistaticObjectCfg _semistaticObjectCfg;
        public void Run()
        {
            foreach (var rotate in _rotateSingle)
            {
                var entity = _rotateSingle.GetEntity(rotate);
                entity.Get<Doing>();
                ref var transform = ref _rotateSingle.Get1(rotate).TransformFirst;
                ref var speed = ref _rotateSingle.Get1(rotate).Duration;
                ref var angle = ref _rotateSingle.Get1(rotate).MaxAngle;
                speed *= Random.Range(1/_semistaticObjectCfg.MultiplyDuration, _semistaticObjectCfg.MultiplyDuration);
                DOTween.Sequence().Append(transform.DORotate(angle, speed).SetEase(Ease.OutSine))
                    .AppendInterval(Random.Range(_semistaticObjectCfg.MinTimeInTopPoint, _semistaticObjectCfg.MaxTimeInTopPoint)).Append(transform.DORotate(Vector3.zero, speed).SetEase(Ease.InSine))
                    .Append(transform.DORotate(-angle, speed).SetEase(Ease.OutSine))
                    .AppendInterval(Random.Range(_semistaticObjectCfg.MinTimeInTopPoint, _semistaticObjectCfg.MaxTimeInTopPoint)).Append(transform.DORotate(Vector3.zero, speed).SetEase(Ease.InSine)).OnComplete((() => entity.Del<Doing>()));
            }
            foreach (var rotate in _rotateTween)
            {
                var entity = _rotateTween.GetEntity(rotate);
                entity.Get<Doing>();
                ref var transform1 = ref _rotateTween.Get1(rotate).TransformFirst;
                ref var transform2 = ref _rotateTween.Get1(rotate).TransformSecond;
                ref var speed = ref _rotateTween.Get1(rotate).Duration;
                ref var angle = ref _rotateTween.Get1(rotate).MaxAngle;
                float down = (Random.Range(_semistaticObjectCfg.MinTimeInDownPoint, _semistaticObjectCfg.MaxTimeInDownPoint));
                float up = (Random.Range(_semistaticObjectCfg.MinTimeInTopPoint, _semistaticObjectCfg.MaxTimeInTopPoint));
                speed *= Random.Range(1/_semistaticObjectCfg.MultiplyDuration, _semistaticObjectCfg.MultiplyDuration);
                DOTween.Sequence().AppendInterval(down).Append(transform1.DORotate(-angle, speed).SetEase(Ease.OutSine))
                    .AppendInterval(up).Append(transform1.DORotate(Vector3.zero, speed).SetEase(Ease.InSine))
                    .OnComplete((() => entity.Del<Doing>()));
                DOTween.Sequence().AppendInterval(down).Append(transform2.DORotate(angle, speed).SetEase(Ease.OutSine))
                    .AppendInterval(up).Append(transform2.DORotate(Vector3.zero, speed).SetEase(Ease.InSine));
            }
        }
    }
}