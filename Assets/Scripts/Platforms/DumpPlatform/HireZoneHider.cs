using System;
using Infrastructure.Services.PersistentData.Core;
using Platforms.Zones;
using Plugins.Animations;
using Plugins.Banks;
using UniRx;
using UnityEngine;
using Zenject;

namespace Platforms.DumpPlatform
{
    public class HireZoneHider : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _root;

        [Header("Animations")]
        [SerializeField] private ScaleAnimation _scaleDownAnimation;

        private ReceiveZone _receiveZone;
        private ClampedIntegerBank _hiredWorkersBank;

        [Inject]
        private void Constructor(ReceiveZone receiveZone, IPersistentDataService persistentDataService)
        {
            _receiveZone = receiveZone;
            _hiredWorkersBank = persistentDataService.PersistentData.PlayerData.PlatformsData.DumpPlatformData.WorkersBank;
        }

        private IDisposable _bankFullnessSubscription;

        #region MonoBehaviour

        private void OnValidate() => _root ??= GetComponent<GameObject>();

        private void Awake()
        {
            if (_hiredWorkersBank.IsFull.Value)
                Destroy(_root);
        }

        private void OnEnable() => StartObserving();

        private void OnDisable() => StopObserving();

        #endregion

        private void StartObserving()
        {
            _bankFullnessSubscription = _hiredWorkersBank.IsFull.Subscribe(isFull =>
            {
                if (isFull == false)
                    return;

                _receiveZone.gameObject.SetActive(false);
                _scaleDownAnimation.PlayForward(() => Destroy(_root));
                StopObserving();
            });
        }

        private void StopObserving() => _bankFullnessSubscription?.Dispose();
    }
}