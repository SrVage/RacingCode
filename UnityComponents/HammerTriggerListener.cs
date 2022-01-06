using Code.Components;
using Code.Components.Obstacles;
using Code.MonoBehavioursComponent;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.UnityComponents
{
    public class HammerTriggerListener:TriggerListener
    {
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<EntityRef>())
            {
                ref var ent = ref other.GetComponentInParent<EntityRef>().Entity;
                if (ent.Has<Car>())
                {
                    ent.Get<Strike>();
                }
            }
            
        }
    }
}