using System;
using Code.Components;
using Code.Enums;
using Code.GameConfigDescription;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Systems.InputSystems
{
    public class InputSystemHandlerFly:IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsFilter<Speed, CarDirection, Flying> _car;
        private readonly EcsWorld _world;
        private readonly EcsFilter<SmoothVector> _input;
        private readonly EcsFilter<Steering> _steering;
        private readonly ControlCfg _controlCfg;
        private float _heightFactor;
        private float _widthFactor;
        private Transform _camera;
        private Transform _carTransform;

        public void Init()
        {
            _heightFactor = 500f / Screen.height;
            _widthFactor = 250f / Screen.width;
            _camera = Camera.main.transform;
        }
        
        public void Run()
        {
            foreach (var car in _car)
            {
                ref var physics = ref _car.Get1(car).Rigidbody;
                _carTransform = physics.transform;
            }
            foreach (var input in _input)
            {
                ref var vector = ref _input.Get1(input).Vector;
                foreach (var car in _car)
                {
                    Vector3 endPoint3 = new Vector3(vector.x, 0, vector.y);
                    Old(endPoint3);
                }
            }
        }
        
        private void Old(Vector3 vector)
        {
            float angle=0;
            float xLength=0;
            var yVectorClamp = CreateVector(vector, true, ref xLength);
            var xVectorClamp = CreateVector(vector, false, ref angle);
            var force = _controlCfg.YSensitivity * yVectorClamp;
            var inputAngle = _controlCfg.YSensitivity *xVectorClamp;
            var entity = _world.NewEntity();
            entity.Get<Accelerate>().Force = force;
            foreach (var steering in _steering)
            {
                ref var steer = ref _steering.Get1(steering).Force;
                steer = inputAngle;
            }
        }

        private float CreateVector(Vector3 vector, bool y, ref float lenght)
        {
            vector = new Quaternion(0, _camera.rotation.y, 0, _camera.rotation.w) * vector;
            int dir;
            Debug.DrawRay(_carTransform.position + Vector3.up, -10 * vector, Color.yellow);
            if (y)
            {
                if (Vector3.Dot(vector, _carTransform.forward) < 0)
                {
                    dir = 1;
                }
                else
                {
                    dir = -1;
                }

                float vectorProjectile = (dir) *
                                         ((Vector3.Project((vector), _carTransform.forward)) * _heightFactor).magnitude;
                lenght = vectorProjectile;
                if (vectorProjectile > 0)
                    return Mathf.Clamp(vectorProjectile, _controlCfg.MinVectorY, _controlCfg.MaxVectorY);
                if (vectorProjectile < 0)
                    return Mathf.Clamp(vectorProjectile, -_controlCfg.MaxVectorY, -_controlCfg.MinVectorY);
                return 0;
            }
            else
            {
                if (Vector3.Dot(vector, _carTransform.right) < 0)
                {
                    dir = 1;
                }
                else
                {
                    dir = -1;
                }
                float vectorProjectile = (dir) *
                                         ((Vector3.Project((vector), _carTransform.right)) * _widthFactor).magnitude;
                lenght = vectorProjectile;
                if (vectorProjectile > 0)
                    return Mathf.Clamp(vectorProjectile, _controlCfg.MinVectorY, _controlCfg.MaxVectorY);
                if (vectorProjectile < 0)
                    return Mathf.Clamp(vectorProjectile, -_controlCfg.MaxVectorY, -_controlCfg.MinVectorY);
                return 0;
            }
        }
    }
}