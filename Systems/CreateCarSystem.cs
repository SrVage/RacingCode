using Code.Components;
using Code.Enums;
using Code.GameConfigDescription;
using Code.MonoBehavioursComponent;
using Code.StatesSwitcher.Events;
using Code.StatesSwitcher.States;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Systems
{
    public class CreateCarSystem:IEcsRunSystem
    {
        private readonly EcsFilter<Car, GameObjectRef> _car;
        private readonly EcsFilter<PlayState> _play;
        private readonly EcsFilter<ResetGame> _reset;
        private readonly EcsFilter<CinemaCamera> _camera;
        private readonly EcsFilter<CarSpawnPoint> _spawn;
        private readonly EcsFilter<LoadLevelUnComplete> _loadLevel;
        private readonly EcsWorld _world;
        private readonly CarCfg _carCfg;
        public void Run()
        {
            if (_car.IsEmpty() && _loadLevel.IsEmpty())
            {
                foreach (var spawn in _spawn)
                {
                    ref var spawnPoint = ref _spawn.Get1(spawn).SpawnPosition;
                    var entit = _world.NewEntity();
                    var car = GameObject.Instantiate(_carCfg.CarPrefab, spawnPoint, Quaternion.identity);
                    car.GetComponent<MonoBehavioursEntity>().Initial(entit, _world);
                    car.GetComponent<Rigidbody>().mass = _carCfg.Mass;
                    car.GetComponentInChildren<BoxCollider>().sharedMaterial.dynamicFriction = _carCfg.Friction;
                    car.GetComponentInChildren<BoxCollider>().sharedMaterial.bounciness = _carCfg.Jumping;
                    Vector3 massPosition = new Vector3(0, _carCfg.CenterOfMassTop, _carCfg.CenterOfMassForward);
                    entit.Get<Car>().CenterOfMass.localPosition = massPosition;
                    car.GetComponent<Rigidbody>().centerOfMass = massPosition;
                    entit.Get<CarDirection>().Direction = Direction.Null;
                    foreach (var i in _camera)
                    {
                        ref var camera = ref _camera.Get1(i).Camera;
                        camera.Follow = car.transform;
                        camera.LookAt = car.transform;
                    }
                }
            }
        }
    }
}