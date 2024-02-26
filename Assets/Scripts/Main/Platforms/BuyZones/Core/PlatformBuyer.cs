using System;
using Infrastructure.Data.Static;
using Infrastructure.Data.Static.Core;
using Infrastructure.Services.StaticData.Core;
using Main.Platforms.Zones;
using UnityEngine;
using Zenject;

namespace Main.Platforms.BuyZones.Core
{
    public class PlatformBuyer : IInitializable, IDisposable
    {
        private readonly Prefab _prefab;
        private readonly ReceiveZone _receiveZone;
        private readonly IInstantiator _instantiator;
        private readonly GamePrefabs _gamePrefabs;

        [Inject]
        public PlatformBuyer(Prefab prefab, ReceiveZone receiveZone, IInstantiator instantiator, IStaticDataService staticDataService)
        {
            _prefab = prefab;
            _receiveZone = receiveZone;
            _instantiator = instantiator;
            _gamePrefabs = staticDataService.Prefabs;
        }

        public event Action<GameObject> OnBought;
        
        public void Initialize() => StartObserving();

        public void Dispose() => StopObserving();
        
        private void StartObserving() => _receiveZone.OnReceivedAll += BuyPlatform;

        private void StopObserving() => _receiveZone.OnReceivedAll -= BuyPlatform;

        private void BuyPlatform()
        {
            GameObject prefab = _gamePrefabs[_prefab];

            GameObject platformObject = _instantiator.InstantiatePrefab(prefab);
            platformObject.transform.SetParent(null);
            platformObject.transform.position = prefab.transform.position;
            platformObject.transform.localScale = prefab.transform.localScale;
            
            OnBought?.Invoke(platformObject);
        }
    }
}