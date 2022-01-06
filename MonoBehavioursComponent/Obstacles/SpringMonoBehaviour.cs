using Code.Components.Obstacles;
using Code.Components.States;
using Code.UnityComponents;
using Leopotam.Ecs;
using TMPro;
using UnityEngine;

namespace Code.MonoBehavioursComponent.Obstacles
{
    public class SpringMonoBehaviour:MonoBehavioursEntity
    {
        [SerializeField]private float _liftTime;
        [SerializeField]private float _descentTime;
        [SerializeField]private float _distance;

        public override void Initial(EcsEntity entity, EcsWorld world)
        {
            base.Initial(entity, world);
            entity.Get<Spring>().LiftTime = _liftTime;
            entity.Get<Spring>().DescentTime = _descentTime;
            entity.Get<Spring>().Transform = transform;
            entity.Get<Spring>().StartPosition = transform.position;
            entity.Get<Spring>().Distance = _distance;
            entity.Get<NotActive>();
            entity.Get<Fixed>();
            gameObject.AddComponent<SpringTriggerListener>().Init(world, entity, true);
        }
    }
}