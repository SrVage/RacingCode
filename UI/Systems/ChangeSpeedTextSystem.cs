using Code.Components;
using Code.UI.Components;
using Leopotam.Ecs;

namespace Code.UI.Systems
{
    public class ChangeSpeedTextSystem:IEcsRunSystem
    {
        private readonly EcsFilter<SpeedText> _text;
        private readonly EcsFilter<Speed> _speed;
        private readonly EcsFilter<WheelSpeed> _wheelSpeed;
        public void Run()
        {
            foreach (var speed in _speed)
            {
                ref var carPhysic = ref _speed.Get1(speed).Rigidbody;
                var carSpeed = carPhysic.velocity.magnitude;
                foreach (var wheel in _wheelSpeed)
                {
                    ref var wheelSpeed = ref _wheelSpeed.Get1(wheel).Speed;
                    foreach (var text in _text)
                    {
                        ref var textSpeed = ref _text.Get1(text).Speed;
                        ref var textWheelSpeed = ref _text.Get1(text).WheelSpeed;
                        textSpeed.text = $"Speed: {carSpeed:F2}";
                        textWheelSpeed.text = $"Wheel speed: {wheelSpeed:F2}";
                    }
                }
            }
        }
    }
}