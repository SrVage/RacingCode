using Code.Components;
using Code.Components.Boosters;
using Code.MonoBehavioursComponent;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.UnityComponents
{
    public class N2OTriggerListener:TriggerListener
    {
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<EntityRef>())
            {
                ref var enity = ref other.GetComponentInParent<EntityRef>().Entity;
                if (enity.Has<Car>())
                {
                    enity.Get<N2OEffect>().Time = 10;
                    _entity.Get<Destroy>();
                }
            }
        }
    }
}