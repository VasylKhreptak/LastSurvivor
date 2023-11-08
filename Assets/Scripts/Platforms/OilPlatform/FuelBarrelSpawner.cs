using System;
using Data.Persistent;
using Flexalon;
using Grid;
using Infrastructure.Data.Static;
using Infrastructure.Services.StaticData.Core;
using Plugins.Timer;
using UniRx;
using UnityEngine;
using Zenject;

namespace Platforms.OilPlatform
{
    public class FuelBarrelSpawner : MonoBehaviour
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

        #region MonoBehaviour

        private void OnEnable() => StartObserving();

        private void OnDisable() => StopObserving();

        #endregion

        private void StartObserving()
        {
            _gridSubscription = _gridStack.IsFull.Subscribe(isFull =>
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
                StartTimer();
            });
            StartTimer();
        }

        private void StopSpawning()
        {
            _timer.Stop();
            _timer.Reset();
            _timerCompletedSubscription?.Dispose();
        }

        private void SpawnBarrel()
        {
            GameObject barrel = Instantiate(_gamePrefabs.FuelBarrel);
            barrel.transform.position = _gridStack.Root.position;
            barrel.transform.localScale = Vector3.zero;
            _gridStack.TryPush(barrel);
        }

        private void StartTimer() => _timer.Start(_platformData.BarrelProduceDuration.Value);
    }
}