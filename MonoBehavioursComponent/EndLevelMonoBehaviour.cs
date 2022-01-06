using Code.Components;
using Code.UnityComponents;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.MonoBehavioursComponent
{
    public class EndLevelMonoBehaviour:MonoBehavioursEntity
    {
        public override void Initial(EcsEntity entity, EcsWorld world)
        {
            base.Initial(entity, world);
            entity.Get<Finish>();
            gameObject.AddComponent<TriggerListener>().Init(world, entity, true);
        }
    }
}