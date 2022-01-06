using System;
using Code.Components;
using Code.Enums;
using Code.GameConfigDescription;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Systems.InputSystems
{
    public class InputSystemHandler:IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsFilter<Speed, CarDirection>.Exclude<Flying> _car;
        private readonly EcsWorld _world;
        private readonly EcsFilter<SmoothVector> _input;
        private readonly EcsFilter<Steering> _steering;
        private readonly ControlCfg _controlCfg;
        private Direction _direction=Direction.Null;
        private float _heightFactor;
        private Transform _camera;
        private Transform _carTransform;

        public void Init()
        {
            _heightFactor = 500f / Screen.height;
            _camera = Camera.main.transform;
        }
        
        public void Run()
        {
            foreach (var car in _car)
            {
                ref var physics = ref _car.Get1(car).Rigidbody;
                _carTransform = physics.transform;
            }
            if (_input.IsEmpty())
            {
                _direction = Direction.Null;
                return;
            }
            foreach (var input in _input)
            {
                ref var vector = ref _input.Get1(input).Vector;
                foreach (var car in _car)
                {
                    ref var physics = ref _car.Get1(car).Rigidbody;
                    ref var direction = ref _car.Get2(car).Direction;
                    var speed = physics.velocity;
                    var angle = physics.transform.forward;
                    if (speed.sqrMagnitude > 0.2f)
                    {
                        if (Vector3.Dot(speed, angle) > 0)
                        {
                            _direction = Direction.Forward;
                        }
                        else if (Vector3.Dot(speed, angle) < 0)
                        {
                            _direction = Direction.Backward;
                        }
                        else
                        {
                            _direction = Direction.Null;
                        }
                    }
                    else
                    {
                        _direction = Direction.Null;
                    }
                    direction = _direction;
                    Vector3 endPoint3 = new Vector3(vector.x, 0, vector.y);
                    Old(endPoint3, speed.magnitude);
                }
            }
        }
        
        private void Old(Vector3 vector, float speed)
        {
            float angle=0;
            float xLength=0;
            var yVectorClamp = CreateVector(vector, true, ref xLength);
            var xVectorClamp = CreateVector(vector, false, ref angle);
            var force = _controlCfg.YSensitivity * yVectorClamp;
            var inputAngle = xVectorClamp;
            angle = Math.Abs(angle);
            if (_direction == Direction.Null && angle>_controlCfg.SideAngle)
            {
                var entity = _world.NewEntity();
                entity.Get<Accelerate>().Force = Mathf.Clamp(force, _controlCfg.MinAccelerate, Mathf.Abs(force)+Mathf.Abs(inputAngle));;
            }
            else
            {
                if (force > 0)
                {
                    var entity = _world.NewEntity();
                    if (_direction == Direction.Forward || _direction == Direction.Null)
                    {
                        entity.Get<Accelerate>().Force = force;
                    }
                    else
                    {
                        if ((Mathf.Abs(angle) > _controlCfg.FrontAngle))
                        {
                            entity.Get<Accelerate>().Force = 
                                Mathf.Clamp(force, -Mathf.Abs(force), -_controlCfg.MinAccelerate);
                        }
                        else
                        {
                            entity.Get<Brake>().Force = force;
                        }
                    }
                }
                else if (force < 0)
                {
                    var entity = _world.NewEntity();
                    if (_direction == Direction.Backward || _direction == Direction.Null)
                    {
                        entity.Get<Accelerate>().Force = force;
                    }
                    else
                    {
                        if ((Mathf.Abs(angle) > _controlCfg.BackAngle))
                        {
                            entity.Get<Accelerate>().Force =
                                Mathf.Clamp(force, _controlCfg.MinAccelerate, Mathf.Abs(force));
                        }
                        else
                        {
                            entity.Get<Brake>().Force = force;
                        }
                    }
                }
            }
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
            int dirx;
            Debug.DrawRay(_carTransform.position+Vector3.up, -10*vector, Color.yellow);
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
                float vectorProjectile = (dir)*
                                         ((Vector3.Project((vector), _carTransform.forward)) * _heightFactor).magnitude;
                lenght = vectorProjectile;
                if (vectorProjectile > 0)
                    return Mathf.Clamp(vectorProjectile, _controlCfg.MinVectorY, _controlCfg.MaxVectorY);
                if (vectorProjectile<0)
                    return Mathf.Clamp(vectorProjectile, -_controlCfg.MaxVectorY, -_controlCfg.MinVectorY);
                return 0;
            }
            else
            {
                if (Vector3.Dot(vector, _carTransform.forward) > 0)
                {
                    dir = 1;
                }
                else
                {
                    dir = -1;
                }
                if (Vector3.Dot(vector, _carTransform.right) < 0)
                {
                    dirx = 1;
                }
                else
                {
                    dirx = -1;
                }

                var angle = Vector3.Angle(_carTransform.forward, dir * vector);
                lenght = angle;
                if (angle >= _controlCfg.LimitMinAngle)
                    return dirx * Vector3.Angle(_carTransform.forward, dir * vector);
                return 0;
            }
        }
    }
}