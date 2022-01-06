using Code.Components.Obstacles;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.MonoBehavioursComponent.Obstacles
{
    public class SingleHammer:MonoBehavioursEntity
    {
        [SerializeField] private float _duration;
        [SerializeField] private float _delay;
        [SerializeField] private Vector3 _maxAngle;
        public override void Initial(EcsEntity entity, EcsWorld world)
        {
            base.Initial(entity, world);
            ref var rotate = ref entity.Get<Rotate>();
            rotate.Duration = _duration;
            rotate.MaxAngle = _maxAngle;
            rotate.TransformFirst = transform;
            entity.Get<Single>();
            entity.Get<Delay>().Time = _delay;
        }
    }
}