using System;
using Data.Static.Balance;
using Grid;
using Infrastructure.Data.Static;
using Infrastructure.Data.Static.Core;
using Infrastructure.Services.StaticData.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Platforms.DumpPlatform.Workers.StateMachine.States.Core;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Platforms.DumpPlatform.Workers.StateMachine.States
{
    public class WorkState : IWorkerState, IState, IExitable
    {
        private readonly Animator _animator;
        private readonly DumpWorkerPreferences _preferences;
        private readonly GridStack _gearsGrid;
        private readonly GamePrefabs _prefabs;
        private readonly Transform _gearSpawnPoint;

        public WorkState(Animator animator, DumpWorkerPreferences preferences, GridStack gearsGrid,
            Transform gearSpawnPoint, IStaticDataService staticDataService)
        {
            _animator = animator;
            _preferences = preferences;
            _gearsGrid = gearsGrid;
            _prefabs = staticDataService.Prefabs;
            _gearSpawnPoint = gearSpawnPoint;
        }

        private readonly int _isWorkingParameter = Animator.StringToHash("IsWorking");

        private IDisposable _gearSpawnSubscription;
        private IDisposable _startDelaySubscription;

        public void Enter()
        {
            _startDelaySubscription = Observable
                .Timer(TimeSpan.FromSeconds(Random.Range(_preferences.MinStartDelay, _preferences.MaxStartDelay)))
                .Subscribe(_ =>
                {
                    SetAnimatorWorkingState(true);
                    StartSpawningGears();
                });
        }

        public void Exit()
        {
            _startDelaySubscription?.Dispose();
            SetAnimatorWorkingState(false);
            StopSpawningGears();
        }

        private void SetAnimatorWorkingState(bool isWorking) => _animator.SetBool(_isWorkingParameter, isWorking);

        private void StartSpawningGears()
        {
            StopSpawningGears();
            _gearSpawnSubscription = Observable
                .Timer(TimeSpan.FromSeconds(Random.Range(_preferences.MinGearSpawnInterval, _preferences.MaxGearSpawnInterval)))
                .Subscribe(_ =>
                {
                    SpawnGear();

                    if (_gearsGrid.Data.IsFull.Value)
                        return;

                    StartSpawningGears();
                });
        }

        private void StopSpawningGears() => _gearSpawnSubscription?.Dispose();

        private void SpawnGear()
        {
            GameObject gearInstance = Object.Instantiate(_prefabs[Prefab.Gear], _gearSpawnPoint.position, _gearSpawnPoint.rotation);
            _gearsGrid.TryPush(gearInstance);
        }
    }
}