using System;
using Code.Components;
using Leopotam.Ecs;

namespace Code.Systems.CarControlsSystems
{
    public class WheelSpeedSystem:IEcsRunSystem
    {
        private readonly EcsFilter<Car> _car;
        private readonly EcsFilter<WheelSpeed> _wheel;
        private readonly EcsWorld _world;
        public void Run()
        {
            if (_wheel.IsEmpty())
                _world.NewEntity().Get<WheelSpeed>();
            foreach (var car in _car)
            {
                ref var blc = ref _car.Get1(car).BackLeftWheelCol;
                var speed = blc.rpm * blc.radius * Math.PI / 30;
                if (speed > 200)
                {
                    blc.brakeTorque = 5000f;
                    ref var brc = ref _car.Get1(car).BackRightWheelCol;
                    brc.brakeTorque = 5000f;
                }
                foreach (var wheel in _wheel)
                {
                    ref var wSpeed = ref _wheel.Get1(wheel).Speed;
                    wSpeed = (float)speed;
                }
            }
        }
    }
}