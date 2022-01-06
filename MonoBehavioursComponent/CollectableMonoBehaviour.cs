using System;
using Code.Components.Collectable;
using Code.Enums;
using Code.Extensions;
using Code.UnityComponents;
using Leopotam.Ecs;
using UnityEngine;
using IdleAnimation = Code.Enums.IdleAnimation;

namespace Code.MonoBehavioursComponent
{
    public class CollectableMonoBehaviour:MonoBehavioursEntity
    {
        [SerializeField] public CollectableElement CollectableElement;
        [SerializeField] private CollectableOperations _collectableOperations;
        [SerializeField] private IdleAnimation _idleAnimation;
        [SerializeField] private float _timeAnimation;
        [SerializeField] private Vector3 _slideDistance;


        public override void Initial(EcsEntity entity, EcsWorld world)
        {

            base.Initial(entity, world);
            if (!gameObject.GetComponent<StartID>().IsActive)
            {
                entity.DestroyWithGameObject();
                return;
            }
            entity.Get<CollectableID>().ID = gameObject.GetComponent<StartID>().ID;
            gameObject.AddComponent<TriggerListener>().Init(world, entity, true);
            switch (CollectableElement)
            {
                case CollectableElement.Coin:
                    entity.Get<Coin>();
                    break;
                case CollectableElement.Life:
                    entity.Get<Live>();
                    break;
                case CollectableElement.Key:
                    entity.Get<Key>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            switch (_collectableOperations)
            {
                case CollectableOperations.Increase:
                    entity.Get<Increase>();
                    break;
                case CollectableOperations.Decrease:
                    entity.Get<Decrease>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //entity.Get<Components.Collectable.IdleAnimation>().AnimationType = _idleAnimation;
            switch (_idleAnimation)
            {
                case IdleAnimation.Rotate:
                    entity.Get<Rotation>();
                    break;
                case IdleAnimation.Slide:
                    entity.Get<Slide>();
                    break;
                case IdleAnimation.None:
                    break;
                case IdleAnimation.SlideAndRotate:
                    entity.Get<Rotation>();
                    entity.Get<Slide>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            entity.Get<Components.Collectable.IdleAnimation>().AnimationSpeed = _timeAnimation;
            entity.Get<Components.Collectable.IdleAnimation>().Distance = _slideDistance;
            entity.Get<Components.Collectable.IdleAnimation>().StartPosition = transform.localPosition;

        }
    }
}