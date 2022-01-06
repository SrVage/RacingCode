using Code.Components;
using Code.Components.Obstacles;
using Code.Components.States;
using DG.Tweening;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Systems.TranslateSystems
{
    public class CheckpointTranslateSystem:IEcsRunSystem
    {
        private readonly EcsFilter<Translate, GameObjectRef>.Exclude<Spring> _translate;
        public void Run()
        {
            foreach (var translate in _translate)
            {
                ref var transform = ref _translate.Get2(translate).Transform;
                var entity = _translate.GetEntity(translate);
                entity.Del<Translate>();
                DOTween.Sequence()
                    .Append(transform.DOLocalJump((transform.localPosition), 5, 0, 1)
                    .OnComplete((() =>
                        entity.Get<NotActive>())));
                transform.DOLocalRotate(new Vector3(180, 0, 0), 1);
            }
        }
    }
}