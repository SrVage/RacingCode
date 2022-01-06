using Code.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.MonoBehavioursComponent
{
    public abstract class MonoBehavioursEntity:MonoBehaviour
    {
        public virtual void Initial(EcsEntity entity, EcsWorld world)
        {
            gameObject.AddComponent<EntityRef>().Entity = entity;
            entity.Get<GameObjectRef>().GameObject = gameObject;
            entity.Get<GameObjectRef>().Transform = transform;
            Destroy(gameObject.GetComponent<MonoBehavioursEntity>());
        }

    }
}