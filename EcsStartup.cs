using Code.Components;
using Code.GameConfigDescription;
using Code.LevelsLoader;
using Code.Services;
using Code.StatesSwitcher;
using Code.StatesSwitcher.States;
using Code.Systems;
using Code.Systems.Boosters;
using Code.Systems.CarControlsSystems;
using Code.Systems.InputSystems;
using Code.Systems.TranslateSystems;
using Code.Systems.UnityHelpersSystems;
using Code.UI.Systems;
using Leopotam.Ecs;
using UnityEngine;

namespace Code {
    sealed class EcsStartup : MonoBehaviour {
        EcsWorld _world;
        EcsSystems _updateSystems;
        EcsSystems _fixedupdateSystems;

        [SerializeField] private LevelList _levels;
        [SerializeField] private CarCfg _carCfg;
        [SerializeField] private UIScreen _screens;
        [SerializeField] private SemistaticObjectCfg _semistaticObjectCfg;
        [SerializeField] private N2OCfg _n2OCfg;

        [Header("MenuConfig")] 
        [SerializeField] private ControlCfg _controlCfg;
        [SerializeField] private WheelCfg _wheelCfg;
        [SerializeField] private PlayerCfg _playerCfg;


        void Start () {
            // void can be switched to IEnumerator for support coroutines.
            Application.targetFrameRate = 100;
            Time.fixedDeltaTime = 0.005f;
            var saveLoadCollectable = new SaveLoadCollectable();
            //Time.timeScale = 0.01f;
            _world = new EcsWorld ();
            _updateSystems = new EcsSystems (_world);
            _fixedupdateSystems = new EcsSystems(_world);
            ChangeGameState.World = _world;
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create (_world);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create (_updateSystems);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create (_fixedupdateSystems);
#endif
            _updateSystems
                // register your systems here, for example:
                .Add(new Initial())
                .Add(new LiveSystem())
                .Add(new DestroyLevelSystem())
                .Add (new ChangeStateSystem ())
                .Add(new WinInitializeSystem())
                .Add(new StateMachine())
                .Add(new LoadLevel(saveLoadCollectable))
                .Add(new ChangeScreenSystem())
                .Add(new WinSystem())
                .Add(new CreateCarSystem())
                
                #region InputSystems
                .Add(new InputSystem())
                .Add(new SmoothVectorSystem())
                .Add(new InputSystemHandler())
                .Add(new InputSystemHandlerFly())
                #endregion
#if UNITY_EDITOR
                .Add(new DrawGizmoSystem())
                .Add(new PauseSystem())
#endif
                #region CarControlSystem
                //.Add(new ZeroingSystem())
                .Add(new BrakeSystem())
                //.Add(new SteeringSystem())
                .Add(new AlternativeSteeringSystem())
                .Add(new AccelerateSystem())
                .Add(new PushCarSystem())
                .Add(new AntiWingSystem())
                .Add(new CarWheelsSystem())
                .Add(new FlyCarSystem())
                .Add(new WheelSpeedSystem())
                #endregion
                .Add(new N2OTimeSystem())
                .Add(new N2ODisableSystem())
                .Add(new ChangeN2OSliderSystem())
                .Add(new RotatorSystem())
                .Add(new StrikeCarSystem())
                .Add(new SpringActivateSystem())
                .Add(new ChangeSpeedTextSystem())
                .Add(new ChangeLivesSystem())
                .Add(new FinishMoveSystem())
                .Add(new CollectableAnimationSystem())
                .Add(new CollectionSystem())
                .Add(new KillSystem())
                .Add(new DestroyObjectSystem(saveLoadCollectable))
                .Add(new ChangeCoinTextSystem())
                .Add(new ChangeKeysSystem())
                .Add(new SpawnSystem(saveLoadCollectable))
                .Add(new CheckpointTranslateSystem())
                .Add(new DelaySystem())


                // register one-frame components (order is important), for example:
                .OneFrame<ChangeState> ()
                .OneFrame<LoadLevelSignal> ()
                .OneFrame<RestartLevelSignal> ()
                .OneFrame<Accelerate>()
                .OneFrame<Brake>()
                .OneFrame<InputVector>()
                .OneFrame<SmoothVector>()
                .OneFrame<Stuck>()
                // inject service instances here (order doesn't important), for example:
                .Inject (_levels)
                .Inject(_carCfg)
                .Inject(_screens)
                .Inject(_controlCfg)
                .Inject(_wheelCfg)
                .Inject(_playerCfg)
                .Inject(_semistaticObjectCfg)
                .Inject(_n2OCfg)
                // .Inject (new NavMeshSupport ())
                .Init ();
            
            _fixedupdateSystems
                .Add(new SkidmarkWheelSystem())
                
                .Init();
        }
        
        void Update () {
            _updateSystems?.Run ();
            TimeService.deltaTime = Time.deltaTime;
        }

        private void FixedUpdate()
        {
            _fixedupdateSystems?.Run();
        }

        void OnDestroy () {
            if (_updateSystems != null)
            {
                _updateSystems.Destroy();
                _updateSystems = null;
            }
            if (_fixedupdateSystems != null)
            {
                _fixedupdateSystems.Destroy();
                _fixedupdateSystems = null;
            }
            _world.Destroy ();
            _world = null;
        }
    }
}