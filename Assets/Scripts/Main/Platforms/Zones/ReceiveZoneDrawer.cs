using System;
using DG.Tweening;
using Plugins.Banks;
using UniRx;
using UnityEngine;
using Zenject;

namespace Main.Platforms.Zones
{
    public class ReceiveZoneDrawer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _root;

        [Header("Animation Preferences")]
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private AnimationCurve _scaleCurve;

        private ReceiveZone _receiveZone;
        private ClampedIntegerBank _dependentBank;

        [Inject]
        private void Constructor(ReceiveZone receiveZone, ClampedIntegerBank dependentBank)
        {
            _receiveZone = receiveZone;
            _dependentBank = dependentBank;
        }

        private Vector3 _initialScale;

        private IDisposable _bankFullnessSubscription;

        #region MonoBehaviour

        private void OnValidate() => _root ??= GetComponent<Transform>();

        private void Awake()
        {
            _initialScale = _root.localScale;
            UpdateStateImmediately();
            StartObserving();
        }

        private void OnDestroy() => StopObserving();

        #endregion

        private void StartObserving() => _bankFullnessSubscription = _dependentBank.IsFull.Skip(1).Subscribe(_ => UpdateState());

        private void StopObserving() => _bankFullnessSubscription?.Dispose();

        private void UpdateState()
        {
            bool isFull = _dependentBank.IsFull.Value;
            Action onComplete = isFull ? () => _root.gameObject.SetActive(false) : null;

            if (isFull == false)
                _root.gameObject.SetActive(true);

            _receiveZone.enabled = isFull == false;

            SetScaleSmooth(isFull ? Vector3.zero : _initialScale, onComplete);
        }

        private void UpdateStateImmediately()
        {
            bool isFull = _dependentBank.IsFull.Value;
            _root.gameObject.SetActive(isFull == false);
            _root.localScale = isFull ? Vector3.zero : _initialScale;
            _receiveZone.enabled = isFull == false;
        }

        private void SetScaleSmooth(Vector3 scale, Action onComplete = null)
        {
            _root.transform
                .DOScale(scale, _duration)
                .SetEase(_scaleCurve)
                .OnComplete(() => onComplete?.Invoke())
                .Play();
        }
    }
}