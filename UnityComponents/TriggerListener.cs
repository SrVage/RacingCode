using Code.Components;
using Code.MonoBehavioursComponent;
using Leopotam.Ecs;
using UnityEngine;
using Event = Code.Components.Event;

namespace Code.UnityComponents
{
    public class TriggerListener:MonoBehaviour
    {
        protected EcsWorld _world; 
        protected EcsEntity _entity;
        protected bool _self;

        public void Init(EcsWorld world, EcsEntity entity, bool self)
        {
            _world = world;
            _entity = entity;
            _self = self;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (_self)
            {
                if (other.GetComponentInParent<EntityRef>())
                {
                    ref var ent = ref other.GetComponentInParent<EntityRef>().Entity; //ref other.GetComponent<EntityRef>().Entity;
                    _entity.Get<Trigger>().Entity = ent;
                    return;
                }
            }
            if (other.gameObject.GetComponentInParent<EntityRef>())
            {
                ref var triggerEntity = ref  other.gameObject.GetComponentInParent<EntityRef>().Entity;
                triggerEntity.Get<Trigger>();
            }
        }
    }
}