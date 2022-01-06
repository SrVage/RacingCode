using Cinemachine;
using Code.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.MonoBehavioursComponent
{
    public class Camera:MonoBehavioursEntity
    {
        [SerializeField] private CinemachineVirtualCamera _cinemachine;
        public override void Initial(EcsEntity entity, EcsWorld world)
        {
            base.Initial(entity, world);
            entity.Get<CinemaCamera>().Camera = _cinemachine;
        }
    }
}