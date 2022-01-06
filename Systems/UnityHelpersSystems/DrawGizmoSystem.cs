using Code.Components;
using Code.GameConfigDescription;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Systems.UnityHelpersSystems
{
    public class DrawGizmoSystem:IEcsRunSystem
    {
        private readonly EcsFilter<Speed> _car;
        private Transform _carTransform;
        private readonly ControlCfg _controlCfg;

        public void Run()
        {
            foreach (var car in _car)
            {
                ref var physics = ref _car.Get1(car).Rigidbody;
                var speed = physics.velocity;
                var angle = physics.transform.forward;
                _carTransform = physics.transform;
                if (speed.sqrMagnitude > 0.2f)
                {
                    if (Vector3.Dot(speed, angle) > 0)
                    {
                        Draw(true);
                    }
                    else if (Vector3.Dot(speed, angle) < 0)
                    {
                        Draw(false);
                    }
                    else
                    {
                        DrawSide();
                    }
                }
                else
                {
                    DrawSide();
                }
            }
        }
        
        private void Draw(bool forward)
        {
            Vector3 vector = Vector3.zero;
            if (forward)
                vector = new Vector3(Mathf.Tan(_controlCfg.BackAngle*Mathf.Deg2Rad), 0, -1);
            else
                vector = new Vector3(Mathf.Tan(_controlCfg.FrontAngle*Mathf.Deg2Rad), 0, 1);
            var vector1 = _carTransform.rotation * vector;
            Debug.DrawRay(_carTransform.position+Vector3.up, 10*vector1, Color.red);
            if (forward)
                vector = new Vector3(-Mathf.Tan(_controlCfg.BackAngle*Mathf.Deg2Rad), 0, -1);
            else
                vector = new Vector3(-Mathf.Tan(_controlCfg.FrontAngle*Mathf.Deg2Rad), 0, 1);
            vector1 = _carTransform.rotation * vector;
            Debug.DrawRay(_carTransform.position+Vector3.up, 10*vector1, Color.red);
        }

        private void DrawSide()
        {
            Vector3 vector = Vector3.zero;
            var rotation = _carTransform.rotation;
            var position = _carTransform.position;
            vector = new Vector3(Mathf.Tan(_controlCfg.SideAngle*Mathf.Deg2Rad), 0, 1);
            var vector1 = rotation * vector;
            Debug.DrawRay(position+Vector3.up, 10*vector1, Color.green);
            vector = new Vector3(-Mathf.Tan(_controlCfg.SideAngle*Mathf.Deg2Rad), 0, 1);
            vector1 = rotation * vector;
            Debug.DrawRay(position+Vector3.up, 10*vector1, Color.green);
            vector = new Vector3(-Mathf.Tan(_controlCfg.SideAngle*Mathf.Deg2Rad), 0, -1);
            vector1 = rotation * vector;
            Debug.DrawRay(position+Vector3.up, 10*vector1, Color.green);
            vector = new Vector3(Mathf.Tan(_controlCfg.SideAngle*Mathf.Deg2Rad), 0, -1);
            vector1 = rotation * vector;
            Debug.DrawRay(position+Vector3.up, 10*vector1, Color.green);
        }
    }
}