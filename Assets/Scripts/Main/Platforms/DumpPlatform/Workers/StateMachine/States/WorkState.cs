using System;
using Data.Static.Balance.Platforms;
using DG.Tweening;
using Grid;
using Infrastructure.Data.Static;
using Infrastructure.Data.Static.Core;
using Infrastructure.Services.StaticData.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Main.Platforms.DumpPlatform.Workers.StateMachine.States.Core;
using UniRx;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Main.Platforms.DumpPlatform.Workers.StateMachine.States
{
    public class WorkState : IWorkerState, IInitializable, IExitable
    {
        private readonly Animator _animator;
        private readonly DumpPlatformPreferences _preferences;
        private readonly GridStack _gearsGrid;
        private readonly GamePrefabs _prefabs;
        private readonly WorkerReferences _workerReferences;

        public WorkState(Animator animator, DumpPlatformPreferences preferences, GridStack gearsGrid,
            IStaticDataService staticDataService, WorkerReferences workerReferences)
        {
            _animator = animator;
            _preferences = preferences;
            _gearsGrid = gearsGrid;
            _prefabs = staticDataService.Prefabs;
            _workerReferences = workerReferences;
        }

        private readonly int _isWorkingParameter = Animator.StringToHash("IsWorking");

        private Vector3 _initialToolScale;

        private IDisposable _gearSpawnSubscription;
        private IDisposable _startDelaySubscription;

        public void Initialize()
        {
            _initialToolScale = _workerReferences.ToolTransform.localScale;
            _workerReferences.ToolTransform.localScale = Vector3.zero;
            _workerReferences.ToolTransform.gameObject.SetActive(false);
        }

        public void Enter()
        {
            _startDelaySubscription = Observable
                .Timer(TimeSpan.FromSeconds(Random.Range(_preferences.MinWorkerStartDelay, _preferences.MaxWorkerStartDelay)))
                .Subscribe(_ =>
                {
                    SetToolState(true);
                    SetAnimatorWorkingState(true);
                    StartSpawningGears();
                });
        }

        public void Exit()
        {
            _startDelaySubscription?.Dispose();
            SetAnimatorWorkingState(false);
            StopSpawningGears();
            SetToolState(false);
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
            Transform gearSpawnPoint = _workerReferences.GearSpawnPoint;
            GameObject gearInstance = Object.Instantiate(_prefabs[Prefab.Gear], gearSpawnPoint.position, gearSpawnPoint.rotation);
            _gearsGrid.TryPush(gearInstance);
        }

        private void SetToolState(bool enabled)
        {
            Transform toolTransform = _workerReferences.ToolTransform;

            Vector3 targetScale = enabled ? _initialToolScale : Vector3.zero;

            if (enabled)
                toolTransform.gameObject.SetActive(true);

            toolTransform
                .DOScale(targetScale, _preferences.WorkerToolAnimationDuration)
                .SetEase(_preferences.WorkerToolAnimationCurve)
                .OnComplete(() =>
                {
                    if (enabled == false)
                        toolTransform.gameObject.SetActive(false);
                })
                .Play();
        }
    }
}