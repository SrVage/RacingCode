using Leopotam.Ecs;
using UnityEngine;

namespace Code.MonoBehavioursComponent
{
    public class UIEntity:MonoBehaviour
    {
        protected EcsWorld _world;
        public virtual void Initial(EcsWorld world)
        {
            _world = world;
        }
    }
}