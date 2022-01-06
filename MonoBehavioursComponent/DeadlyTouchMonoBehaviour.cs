using System;
using Code.Components;
using Code.UnityComponents;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.MonoBehavioursComponent
{
    public class DeadlyTouchMonoBehaviour:MonoBehavioursEntity
    {
        private bool _gizmo = true;
        private void Start()
        {
            _gizmo = false;
        }
        
        private void OnDrawGizmos()
        {
            if (!_gizmo)
                return;
            Gizmos.DrawCube(transform.position, transform.localScale);
        }


        public override void Initial(EcsEntity entity, EcsWorld world)
        {
            base.Initial(entity, world);
            gameObject.AddComponent<TriggerListener>().Init(world, entity, true);
            entity.Get<DeadlyTouch>();
        }
    }
}