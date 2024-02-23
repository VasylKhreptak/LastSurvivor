using System;
using Data.Persistent.Platforms;
using Grid;
using Infrastructure.Data.Static;
using Infrastructure.Data.Static.Core;
using Infrastructure.Services.StaticData.Core;
using Plugins.Timer;
using UniRx;
using UnityEngine;
using Zenject;

namespace Main.Platforms.OilPlatform
{
    public class FuelSpawner : MonoBehaviour
    {
        private OilPlatformData _platformData;
        private GamePrefabs _gamePrefabs;
        private GridStack _gridStack;

        [Inject]
        private void Constructor(OilPlatformData platformData, IStaticDataService staticDataService, GridStack gridStack)
        {
            _platformData = platformData;
            _gamePrefabs = staticDataService.Prefabs;
            _gridStack = gridStack;
        }

        private readonly Timer _timer = new Timer();

        private IDisposable _timerCompletedSubscription;
        private IDisposable _gridSubscription;

        public IReadonlyTimer Timer => _timer;

        #region MonoBehaviour

        private void OnEnable() => StartObserving();

        private void OnDisable()
        {
            StopObserving();
            StopSpawning();
        }

        #endregion

        private void StartObserving()
        {
            _gridSubscription = _gridStack.Bank.IsFull.Subscribe(isFull =>
            {
                if (isFull)
                    StopSpawning();
                else
                    StartSpawning();
            });
        }

        private void StopObserving() => _gridSubscription?.Dispose();

        private void StartSpawning()
        {
            StopSpawning();

            _timerCompletedSubscription = _timer.OnCompleted.Subscribe(_ =>
            {
                SpawnBarrel();

                if (_platformData.GridData.IsFull.Value)
                    return;

                StartTimer();
            });

            StartTimer();
        }

        private void StopSpawning()
        {
            _timerCompletedSubscription?.Dispose();
            _timer.Reset();
        }

        private void SpawnBarrel()
        {
            GameObject barrel = Instantiate(_gamePrefabs[Prefab.FuelBarrel]);
            _gridStack.TryPush(barrel);
            barrel.transform.localScale = Vector3.zero;
            barrel.transform.localPosition =
                _gridStack.Grid.Node.GetChild(_gridStack.Grid.transform.childCount - 1).Result.TargetPosition;
        }

        private void StartTimer() => _timer.Start(_platformData.BarrelProduceDuration.Value);
    }
}