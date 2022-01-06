using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Components;
using Code.Components.Collectable;
using Code.MonoBehavioursComponent;
using Code.Services;
using Code.StatesSwitcher;
using Code.UI;
using Leopotam.Ecs;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using EndLevel = Code.MonoBehavioursComponent.EndLevel;

namespace Code.LevelsLoader
{
    public class LoadLevel:IEcsRunSystem, IEcsInitSystem
    {
        private readonly LevelList _levels;
        private readonly EcsFilter<LoadLevelSignal> _signal;
        private readonly EcsFilter<RestartLevelSignal> _restartignal;
        private EcsWorld _world;
        private ChangeLevelService _changeLevelService;
        private SetSpawnOnLoad _setSpawnOnLoad;
        private AsyncOperationHandle<GameObject> _level;
        private Queue<AsyncOperationHandle<GameObject>> _levelList = new Queue<AsyncOperationHandle<GameObject>>();
        private LoadingCanvas _loadingCanvas;
        private readonly EcsFilter<CarSpawnPoint> _carSpawnPoint;
        private Vector3 _spawnPoint = Vector3.zero;
        private Vector3 _currentSpawnPoint = Vector3.zero;
        private Vector3 _nextCarPosition = Vector3.zero;
        private int _nextLevelCount;
        private Vector3 _spawn;
        private ISaveLoadCollectable _saveLoadCollectable;

        public LoadLevel(ISaveLoadCollectable saveLoadCollectable)
        {
            _saveLoadCollectable = saveLoadCollectable;
        }

        public void Init()
        {
            _loadingCanvas = Object.FindObjectOfType<LoadingCanvas>();
            _changeLevelService = new ChangeLevelService();
            _setSpawnOnLoad = new SetSpawnOnLoad();
            CreateInitLevel();
        }

        public async void Run()
        {
            if (!_signal.IsEmpty())
            {
                var uncomplete = _world.NewEntity();
                    uncomplete.Get<LoadLevelUnComplete>();
                if (_levelList.Count > 2)
                {
                    var lev = _levelList.Dequeue();
                    var entityRefs = lev.Result.GetComponentsInChildren<EntityRef>();
                    foreach (var entityRef in entityRefs)
                    {
                        entityRef.Entity.Destroy();
                    }

                    Addressables.ReleaseInstance(lev);
                }
                
                

                _changeLevelService.ChangeLevel();
                var level = Addressables.InstantiateAsync(
                    _levels.Levels[(_changeLevelService.CurrentLevel + 1) % _levels.Levels.Count], _spawnPoint,
                    Quaternion.identity);
                await level.Task;
                _levelList.Enqueue(level);
                SetSpawnPoint(level);
                InitializeCollectableSave(level);
                uncomplete.Destroy();
                ChangeGameState.Change(GameStates.StartState);
            }

            if (!_restartignal.IsEmpty())
            {
                //_world.NewEntity().Get<LevelFailed>();
                int count = _levelList.Count;
                Queue<AsyncOperationHandle<GameObject>> levelList = new Queue<AsyncOperationHandle<GameObject>>();
                for (int i = 0; i < count; i++)
                {
                    if ((i == 0 && count == 2) || (i == 1 && count == 3))
                    {

                        var levl = _levelList.Dequeue();
                        SetCurrentSpawnPoint(levl);
                        levelList.Enqueue(levl);
                    }
                    else
                    {
                        levelList.Enqueue(_levelList.Dequeue());
                    }
                }
                for (int i = 0; i < count; i++)
                {
                    _levelList.Enqueue(levelList.Dequeue());
                }
                levelList = null;
            } /*
                var uncomolete = _world.NewEntity();
                uncomolete.Get<LoadLevelUnComplete>();
                Queue<AsyncOperationHandle<GameObject>> levelList = new Queue<AsyncOperationHandle<GameObject>>();
                int count = _levelList.Count;
                for (int i = 0; i < count; i++)
                {
                    if ((i==0&&count==2) || (i==1 && count==3))
                    {
                        var lev = _levelList.Dequeue();
                        _currentSpawnPoint = lev.Result.transform.position;
                        var entityRefs = lev.Result.GetComponentsInChildren<EntityRef>();
                        foreach (var entityRef in entityRefs)
                        {
                            entityRef.Entity.Destroy();
                        }
                        Addressables.ReleaseInstance(lev);
                        var level = Addressables.InstantiateAsync(
                            _levels.Levels[(_changeLevelService.CurrentLevel) % _levels.Levels.Count], _currentSpawnPoint,
                            Quaternion.identity);
                        await level.Task;
                        levelList.Enqueue(level);
                        SetCurrentSpawnPoint(level);
                    }
                    else
                    {
                        levelList.Enqueue(_levelList.Dequeue());
                    }
                }

                for (int i = 0; i < levelList.Count; i++)
                {
                    _levelList.Enqueue(levelList.Dequeue());
                }
                levelList = null;
                uncomolete.Destroy();
                ChangeGameState.Change(GameStates.StartState);
            }*/

        }

        private void InitializeCollectableSave(AsyncOperationHandle<GameObject> level)
        {
            _saveLoadCollectable.DeleteList();
            _saveLoadCollectable.SetList(_nextLevelCount);
            _nextLevelCount = level.Result.GetComponentsInChildren<StartID>().Length;
        }

        private void SetSpawnPoint(AsyncOperationHandle<GameObject> level)
        {
            _spawnPoint = level.Result.GetComponentInChildren<EndLevel>().transform.position;
            foreach (var spawnPoint in _carSpawnPoint)
            {
                _carSpawnPoint.Get1(spawnPoint).SpawnPosition = _nextCarPosition;
            }

            _nextCarPosition =
                level.Result.transform.position + 3 * Vector3.forward;
            foreach (var spawnPoint in _carSpawnPoint)
            {
                _carSpawnPoint.Get1(spawnPoint).NextPoint = _nextCarPosition;
            }
        }
        
        private void SetCurrentSpawnPoint(AsyncOperationHandle<GameObject> level)
        {
            foreach (var spawnPoint in _carSpawnPoint)
            {
                _carSpawnPoint.Get1(spawnPoint).SpawnPosition = level.Result.transform.position + 3 * Vector3.forward;
            }
        }

        private async void CreateInitLevel()
        {
            int i = _changeLevelService.CurrentLevel - 1;
            if (i >= 0)
            {
                for (int j = i; j < i+3; j++)
                {
                    await InstantiateLevelCycle(j, i);
                }
            }
            else
            {
                for (int j = i+1; j < i+3; j++)
                {
                    await InstantiateLevelCycle(j, i);
                }
            }
            _loadingCanvas.Hide();
        }

        private async Task InstantiateLevelCycle(int j, int i)
        {
            var level = Addressables.InstantiateAsync(_levels.Levels[j % _levels.Levels.Count], _spawnPoint,
                Quaternion.identity);
            await level.Task;
            _levelList.Enqueue(level);
            _spawnPoint = level.Result.GetComponentInChildren<EndLevel>().transform.position;
            if (j == i + 1)
            {
                var entity = _world.NewEntity();
                    entity.Get<CarSpawnPoint>().SpawnPosition =
                    _setSpawnOnLoad.GetSpawnPoint(level.Result, SaveSpawnPointService.Get());
                    if (_saveLoadCollectable.IsSaved())
                    {
                        var list = _saveLoadCollectable.GetList();
                        var listObject = level.Result.GetComponentsInChildren<StartID>();
                        for (int k = 0; k < list.Length-1; k++)
                        {
                            if (list[k] == 1)
                            {
                                var a = listObject.FirstOrDefault(e => e.ID == k);
                                a.IsActive = false;
                            }
                        }
                    }
                    else
                    {
                        _saveLoadCollectable.SetList(level.Result.GetComponentsInChildren<StartID>().Length);
                    }
            }

            if (j == i + 2)
            {
                _nextCarPosition =
                        level.Result.transform.position + 3 * Vector3.forward;
                    SetSpawnPointInEntity(level);
                    _nextLevelCount = level.Result.GetComponentsInChildren<StartID>().Length;
            }
            _loadingCanvas.LoadLevel();
        }

        private void SetSpawnPointInEntity(AsyncOperationHandle<GameObject> level)
        {
            foreach (var spawnPoint in _carSpawnPoint)
            {
                if (SaveSpawnPointService.Get() == 0)
                    _carSpawnPoint.Get1(spawnPoint).NextPoint = _nextCarPosition;
                else
                {
                    _carSpawnPoint.Get1(spawnPoint).NextPoint = level.Result.transform.position + 3 * Vector3.forward;
                }
            }
        }
    }
}