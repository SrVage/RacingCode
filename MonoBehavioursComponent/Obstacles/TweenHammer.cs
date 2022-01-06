using Code.Components.Obstacles;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.MonoBehavioursComponent.Obstacles
{
    public class TweenHammer:MonoBehavioursEntity
    {
        [SerializeField] private float _duration;
        [SerializeField] private float _delay;
        [SerializeField] private Vector3 _maxAngle;
        [SerializeField] private Transform _firstTransform;
        [SerializeField] private Transform _secondTransform;
        public override void Initial(EcsEntity entity, EcsWorld world)
        {
            base.Initial(entity, world);
            entity.Get<Tween>();
            ref var rotate = ref entity.Get<Rotate>();
            rotate.Duration = _duration;
            rotate.MaxAngle = _maxAngle;
            rotate.TransformFirst = _firstTransform;
            rotate.TransformSecond = _secondTransform;
            entity.Get<Delay>().Time = _delay;
        }
    }
}