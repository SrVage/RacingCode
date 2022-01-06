using System;
using Code.Components;
using Code.Components.States;
using Code.MonoBehavioursComponent;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.UnityComponents
{
    public class SpringTriggerListener:TriggerListener
    {
        private Collider _collider;
        protected void OnTriggerStay(Collider other)
        {
            Collide(other);
        }

        private void Collide(Collider other)
        {
            if (!_entity.Has<Fixed>()||_entity.Has<Active>())
            {
                return;
            }

            if (other.GetComponentInParent<EntityRef>())
            {
                ref var ent = ref other.GetComponentInParent<EntityRef>().Entity;
                if (ent.Has<Car>())
                {
                    _entity.Del<NotActive>();
                    _entity.Get<Active>();
                    _entity.Get<Translate>();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            _collider = other;
            UnCollide();
        }

        private void UnCollide()
        {
            if (!_entity.Has<Fixed>())
            {
                Invoke(nameof(UnCollide), 0.1f);
                return;
            }
            if (_collider.GetComponentInParent<EntityRef>())
            {
                ref var ent = ref _collider.GetComponentInParent<EntityRef>().Entity;
                if (ent.Has<Car>())
                {
                    _entity.Del<Active>();
                    _entity.Get<NotActive>();
                    _entity.Get<Translate>();
                }
            }
        }
    }
}