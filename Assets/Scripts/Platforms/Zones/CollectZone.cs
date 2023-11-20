using System;
using DG.Tweening;
using Entities.Player;
using Grid;
using Plugins.Banks;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Platforms.Zones
{
    public class CollectZone : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] private int _itemPrice = 10;

        [Header("Animation Preferences")]
        [SerializeField] private float _duration;

        [Header("Move Preferences")]
        [SerializeField] private AnimationCurve _moveCurve;

        [Header("Rotate Preferences")]
        [SerializeField] private AnimationCurve _rotateCurve;

        [Header("Scale Preferences")]
        [SerializeField] private Vector3 _targetScale = Vector3.one * 0.4f;
        [SerializeField] private AnimationCurve _scaleCurve;

        [Header("Transfer Preferences")]
        [SerializeField] private float _interval = 0.1f;

        private Player _player;
        private IntegerBank _bank;
        private GridStack _gridStack;

        [Inject]
        private void Constructor(Player player, IntegerBank bank, GridStack gridStack)
        {
            _player = player;
            _bank = bank;
            _gridStack = gridStack;
        }

        private IDisposable _transferSubscription;

        #region MonoBehaviour

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player _))
                StartTransferring();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player _))
                StopTransferring();
        }

        private void OnDisable()
        {
            StopTransferring();
        }

        #endregion

        private void StartTransferring()
        {
            _transferSubscription = Observable
                .Interval(TimeSpan.FromSeconds(_interval))
                .Subscribe(_ => Transfer());
        }

        private void StopTransferring() => _transferSubscription?.Dispose();

        private void Transfer()
        {
            if (_gridStack.TryPop(out GameObject item) == false)
                return;

            ThrowItem(item, () =>
            {
                _bank.Add(_itemPrice);
                Destroy(item);
            });
        }

        private void ThrowItem(GameObject item, Action onComplete = null)
        {
            Vector3 startPosition = item.transform.position;
            Vector3 startScale = item.transform.localScale;
            Quaternion startRotation = item.transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(Random.insideUnitSphere);

            float progress = 0f;
            DOTween
                .To(() => progress, x => progress = x, 1f, _duration)
                .SetEase(Ease.Linear)
                .OnUpdate(() =>
                {
                    Vector3 targetPosition = _player.InputOutputTransform.position;
                    item.transform.position = Vector3.Slerp(startPosition, targetPosition, _moveCurve.Evaluate(progress));
                    item.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, _rotateCurve.Evaluate(progress));
                    item.transform.localScale = Vector3.Slerp(startScale, _targetScale, _scaleCurve.Evaluate(progress));
                })
                .OnComplete(() => onComplete?.Invoke())
                .Play();
        }
    }
}