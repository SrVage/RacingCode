using Code.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Extensions
{
    public static class EntityExtensions
    {
        public static void DestroyWithGameObject(this EcsEntity entity)
        {
            if (entity.Has<GameObjectRef>())
            {
                GameObject.Destroy(entity.Get<GameObjectRef>().GameObject);
            }
            entity.Destroy();
        }
    }
}