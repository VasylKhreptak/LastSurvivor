using System;
using Infrastructure.Data.Static;
using Infrastructure.Data.Static.Core;
using Infrastructure.Services.StaticData.Core;
using Infrastructure.Services.Vibration.Core;
using Main.Platforms.Zones;
using UnityEngine;
using Zenject;

namespace Main.Platforms.BuyZones.Core
{
    public class PlatformBuyer : IInitializable, IDisposable
    {
        private readonly Prefab _prefabType;
        private readonly ReceiveZone _receiveZone;
        private readonly IInstantiator _instantiator;
        private readonly GamePrefabs _gamePrefabs;
        private readonly DiContainer _container;

        [Inject]
        public PlatformBuyer(Prefab prefabType, ReceiveZone receiveZone, IInstantiator instantiator,
            IStaticDataService staticDataService, IVibrationService vibrationService, DiContainer container)
        {
            _prefabType = prefabType;
            _receiveZone = receiveZone;
            _instantiator = instantiator;
            _gamePrefabs = staticDataService.Prefabs;
            _container = container;
        }

        public event Action<GameObject> OnBought;

        #region MonoBehaviour

        public void Initialize() => StartObserving();

        public void Dispose() => StopObserving();

        #endregion

        private void StartObserving() => _receiveZone.OnReceivedAll += BuyPlatform;

        private void StopObserving() => _receiveZone.OnReceivedAll -= BuyPlatform;

        private void BuyPlatform()
        {
            GameObject prefab = _gamePrefabs[_prefabType];

            GameObject platformObject = _instantiator.InstantiatePrefab(_gamePrefabs[_prefabType]);
            platformObject.transform.SetParent(null);
            platformObject.transform.position = prefab.transform.position;
            platformObject.transform.localScale = prefab.transform.localScale;

            OnBought?.Invoke(platformObject);
        }
    }
}