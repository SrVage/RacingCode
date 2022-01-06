using System;
using Code.Components;
using Code.Components.Collectable;
using DG.Tweening;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Systems
{
    public class CollectableAnimationSystem:IEcsRunSystem
    {
        private readonly EcsFilter<IdleAnimation, GameObjectRef, Rotation> _collectableAnimationsRotation;
        private readonly EcsFilter<IdleAnimation, GameObjectRef, Slide> _collectableAnimationsSlide;
        public void Run()
        {
            foreach (var collectableAnimation in _collectableAnimationsRotation)
            {
                ref var transform = ref _collectableAnimationsRotation.Get2(collectableAnimation).Transform;
                var entity = _collectableAnimationsRotation.GetEntity(collectableAnimation);
                transform.rotation = Quaternion.Euler(0,0,0);
                ref var speed = ref _collectableAnimationsRotation.Get1(collectableAnimation).AnimationSpeed;
                _collectableAnimationsRotation.GetEntity(collectableAnimation).Del<Rotation>();
                DOTween.Sequence()
                            .Append(transform.DORotate(new Vector3(0, 180, 0), speed).SetEase(Ease.Linear))
                            .Append(transform.DORotate(new Vector3(0, 360, 0), speed).SetEase(Ease.Linear))
                            .OnComplete(() 
                            => entity.Get<Rotation>());
            }
            foreach (var collectableAnimation in _collectableAnimationsSlide)
            {
                ref var transform = ref _collectableAnimationsSlide.Get2(collectableAnimation).Transform;
                var entity = _collectableAnimationsSlide.GetEntity(collectableAnimation);
                transform.rotation = Quaternion.Euler(0,0,0);
                ref var speed = ref _collectableAnimationsSlide.Get1(collectableAnimation).AnimationSpeed;
                ref var distance = ref _collectableAnimationsSlide.Get1(collectableAnimation).Distance;
                ref var currentPosition = ref _collectableAnimationsSlide.Get1(collectableAnimation).StartPosition;
                //_collectableAnimationsSlide.GetEntity(collectableAnimation).Get<Doing>();
                _collectableAnimationsSlide.GetEntity(collectableAnimation).Del<Slide>();
                DOTween.Sequence().Append(transform.DOLocalMove(currentPosition+distance, speed))
                    .Append(transform.DOLocalMove(currentPosition-distance, speed))
                    .OnComplete(() 
                        => //entity.Del<Doing>()
                        entity.Get<Slide>());
            }
        }
    }
}