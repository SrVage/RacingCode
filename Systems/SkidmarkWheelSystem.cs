using Code.Components;
using Leopotam.Ecs;
using Other.Skidmarks.Skidmarks_Essentials;
using UnityEngine;

namespace Code.Systems
{
    public class SkidmarkWheelSystem:IEcsRunSystem, IEcsInitSystem
    {
        
        private float startSlipValue = 0.2f;
        private Skidmarks skidmarks;
        private int[] lastSkidmark = new int[4];
        private readonly EcsFilter<Car, Speed> _car;
        public void Init()
        {
            skidmarks = Object.FindObjectOfType(typeof(Skidmarks)) as Skidmarks;
        }

        public void Run()
        {
            foreach (var car in _car)
            {
                ref var carPhysic = ref _car.Get2(car).Rigidbody;
                var blc = _car.Get1(car).BackLeftWheelCol;
                SkidMark(blc,carPhysic, 0);
                var brc = _car.Get1(car).BackRightWheelCol;
                SkidMark(brc,carPhysic, 1);
                var flc = _car.Get1(car).FrontLeftWheelCol;
                SkidMark(flc,carPhysic, 2);
                var frc = _car.Get1(car).FrontRightWheelCol;
                SkidMark(frc,carPhysic, 3);
            }
        }

        private void SkidMark(WheelCollider wheel, Rigidbody carPhysic, int i)
        {
            WheelHit GroundHit; //variable to store hit data
            wheel.GetGroundHit(out GroundHit );//store hit data into GroundHit
            var wheelSlipAmount = Mathf.Abs(GroundHit.sidewaysSlip);
            if (wheelSlipAmount > startSlipValue) //if sideways slip is more than desired value
            {
                Vector3 skidPoint  = GroundHit.point + 2*(carPhysic.velocity) * Time.deltaTime;
                lastSkidmark[i] = skidmarks.AddSkidMark(skidPoint, GroundHit.normal, wheelSlipAmount/2.0f, lastSkidmark[i]);	
            }
            else
            {
                lastSkidmark[i] = -1;
            }
        }
    }
}