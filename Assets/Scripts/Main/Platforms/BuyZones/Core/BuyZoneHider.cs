using DG.Tweening;
using Main.Platforms.Zones;
using UnityEngine;
using Zenject;

namespace Main.Platforms.BuyZones.Core
{
    public class BuyZoneHider : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _root;

        [Header("Animation Preferences")]
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private AnimationCurve _scaleDownCurve;

        private PlatformBuyer _platformBuyer;
        private ReceiveZone _receiveZone;

        [Inject]
        private void Constructor(PlatformBuyer platformBuyer, ReceiveZone receiveZone)
        {
            _platformBuyer = platformBuyer;
            _receiveZone = receiveZone;
        }

        #region MonoBehaviour

        private void OnValidate() => _root ??= GetComponentInParent<GameObject>(true);

        private void OnEnable() => StartObserving();

        private void OnDisable() => StopObserving();

        #endregion

        private void StartObserving() => _platformBuyer.OnBought += OnBoughtPlatform;

        private void StopObserving() => _platformBuyer.OnBought -= OnBoughtPlatform;

        private void OnBoughtPlatform(GameObject _) => Hide();

        private void Hide()
        {
            StopObserving();

            _receiveZone.enabled = false;

            _root.transform
                .DOScale(Vector3.zero, _duration)
                .SetEase(_scaleDownCurve)
                .OnComplete(() => Destroy(_root))
                .Play();
        }
    }
}