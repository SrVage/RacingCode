using System;
using Code.Components;
using DG.Tweening;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Systems
{
    public class FinishMoveSystem:IEcsRunSystem
    {
        private readonly EcsFilter<Car, MoveTo, GameObjectRef>.Exclude<Doing> _car;
        public void Run()
        {
            foreach (var car in _car)
            {
                var entity = _car.GetEntity(car);
                ref var carGo = ref _car.Get3(car).GameObject;
                var physic = carGo.GetComponent<Rigidbody>();
                physic.isKinematic = true;
                var carTransform = carGo.transform;
                ref var target = ref _car.Get2(car).Target;
                entity.Get<Doing>();
                   carTransform.DOMove(target, 2f);
                   carTransform.DORotate(Vector3.forward, 2f)
                    .OnComplete(() =>
                {
                    entity.Del<MoveTo>();
                    entity.Del<Doing>();
                    physic.isKinematic = false;
                });
            }
        }
    }
}