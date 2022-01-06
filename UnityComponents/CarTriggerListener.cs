using System;
using Code.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.UnityComponents
{
    public class CarTriggerListener:TriggerListener
    {

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer == 8)
                return;
            if (!_entity.Has<Stuck>()) 
                _entity.Get<Stuck>();
        }
    }
}