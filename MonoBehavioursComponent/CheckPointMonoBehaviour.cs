using Code.Components;
using Code.Components.Collectable;
using Code.Components.States;
using Code.UnityComponents;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.MonoBehavioursComponent
{
    public class CheckPointMonoBehaviour:MonoBehavioursEntity
    {
        [SerializeField] public int SpawnNumber;

        public override void Initial(EcsEntity entity, EcsWorld world)
        {
            base.Initial(entity, world);
            entity.Get<CollectableID>().ID = gameObject.GetComponent<StartID>().ID;
            entity.Get<SpawnPoint>().Point = transform.position;
            entity.Get<SpawnPoint>().Number = SpawnNumber;
            if (gameObject.GetComponent<StartID>().IsActive)
            {
                entity.Get<Active>();
                gameObject.AddComponent<TriggerListener>().Init(world, entity, true);
            }
            else
                entity.Get<Translate>();
        }
    }
}