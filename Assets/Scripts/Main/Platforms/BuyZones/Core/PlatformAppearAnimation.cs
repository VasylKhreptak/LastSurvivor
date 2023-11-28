using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Main.Platforms.BuyZones.Core
{
    public class PlatformAppearAnimation : MonoBehaviour
    {
        [Header("Animation Preferences")]
        [SerializeField] private float _duration = 0.5f;

        [Header("Scale Up Animation Preferences")]
        [SerializeField] private Vector3 _startScale = Vector3.zero;
        [SerializeField] private AnimationCurve _scaleCurve;

        private PlatformBuyer _platformBuyer;

        [Inject]
        private void Constructor(PlatformBuyer platformBuyer)
        {
            _platformBuyer = platformBuyer;
        }

        #region MonoBehaviour

        private void OnEnable() => StartObserving();

        private void OnDisable() => StopObserving();

        #endregion

        private void StartObserving() => _platformBuyer.OnBought += OnBoughtPlatform;

        private void StopObserving() => _platformBuyer.OnBought -= OnBoughtPlatform;

        private void OnBoughtPlatform(GameObject platformObject)
        {
            Vector3 targetScale = platformObject.transform.localScale;

            platformObject.transform.localScale = _startScale;

            platformObject.transform
                .DOScale(targetScale, _duration)
                .SetEase(_scaleCurve)
                .Play();
        }
    }
}