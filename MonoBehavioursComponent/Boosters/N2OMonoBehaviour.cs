using Code.Components.Boosters;
using Code.UnityComponents;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.MonoBehavioursComponent.Boosters
{
    public class N2OMonoBehaviour:MonoBehavioursEntity
    {
        [SerializeField] private Collider _collider;
        public override void Initial(EcsEntity entity, EcsWorld world)
        {
            base.Initial(entity, world);
            entity.Get<N2OEntity>().Collider = _collider;
            gameObject.AddComponent<N2OTriggerListener>().Init(world, entity, true);
        }
    }
}