using System;
using DG.Tweening;
using Entities.Player;
using Infrastructure.Services.Input.Main.Core;
using Plugins.Animations.Extensions;
using Plugins.Banks;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Platforms
{
    public class ReceiveZone : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _receiveToTransform;

        [Header("Animation Preferences")]
        [SerializeField] private float _scaleDuration = 0.2f;
        [SerializeField] private float _duration = 0.4f;

        [Header("Jump Preferences")]
        [SerializeField] private float _jumpPower = 2f;
        [SerializeField] private AnimationCurve _jumpCurve;

        [Header("Rotate Preferences")]
        [SerializeField] private AnimationCurve _rotateCurve;

        [Header("Scale Preferences")]
        [SerializeField] private AnimationCurve _scaleCurve;

        [Header("Transfer Preferences")]
        [SerializeField] private float _interval = 0.1f;
        [SerializeField] private int _maxTransfer = 10;

        [Header("Receive Zone Preferences")]
        [SerializeField] private float _maxRange = 5f;

        private IntegerBank _bank;
        private ClampedIntegerBank _receiveContainer;
        private IMainInputService _inputService;
        private Player _player;
        private GameObject _itemToTransfer;

        [Inject]
        public void Constructor(IntegerBank bank, ClampedIntegerBank receiveContainer, IMainInputService inputService, Player player,
            GameObject itemToTransfer)
        {
            _bank = bank;
            _receiveContainer = receiveContainer;
            _inputService = inputService;
            _player = player;
            _itemToTransfer = itemToTransfer;
        }

        private IDisposable _inputInteractionSubscription;
        private IDisposable _transferSubscription;

        private int _currentTransferAmount;

        public event Action OnReceivedAll;
        public event Action OnReceived;

        #region MonoBehaviour

        public void OnDisable() => StopObserving();

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player _) == false)
                return;

            StartObservingInputInteraction();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player _) == false)
                return;

            StopObservingInputInteraction();
        }

        #endregion

        private void StopObserving()
        {
            StopObservingInputInteraction();
            StopTransferring();
        }

        private void StartObservingInputInteraction()
        {
            StopObservingInputInteraction();

            _inputInteractionSubscription = _inputService.IsInteracting.Subscribe(isInteracting =>
            {
                if (isInteracting)
                    StopTransferring();
                else
                    StartTransferring();
            });
        }

        private void StopObservingInputInteraction() => _inputInteractionSubscription?.Dispose();

        private void StartTransferring()
        {
            StopTransferring();

            int targetTransferCount = GetTransferCount();
            int currentTransferCount = 0;

            _transferSubscription = Observable
                .Interval(TimeSpan.FromSeconds(_interval))
                .Subscribe(_ =>
                {
                    currentTransferCount++;

                    if (currentTransferCount == targetTransferCount)
                    {
                        StopTransferring();
                        return;
                    }

                    if (TryTransfer() == false)
                        StopTransferring();
                });
        }

        private void StopTransferring() => _transferSubscription?.Dispose();

        private int GetTransferCount()
        {
            int leftToFill = _receiveContainer.LeftToFill.Value - _currentTransferAmount;

            float remainder = leftToFill % _maxTransfer;
            return remainder == 0 ? leftToFill / _maxTransfer : leftToFill / _maxTransfer + 1;
        }

        private bool TryTransfer()
        {
            if (_bank.IsEmpty.Value)
                return false;

            Transfer();
            return true;
        }

        private void Transfer()
        {
            int transferAmount = Mathf.Min(_bank.Value.Value, _maxTransfer,
                _receiveContainer.LeftToFill.Value - _currentTransferAmount);

            _bank.Spend(transferAmount);

            GameObject transferringItem =
                Instantiate(_itemToTransfer, _player.InputOutputTransform.position, Quaternion.identity);

            _currentTransferAmount += transferAmount;
            ThrowItem(transferringItem, () =>
            {
                OnReceivedAmount(transferAmount);
                Destroy(transferringItem);
                _currentTransferAmount -= transferAmount;
            });
        }

        private void ThrowItem(GameObject item, Action onComplete = null)
        {
            Vector3 initialScale = item.transform.localScale;
            item.transform.localScale = Vector3.zero;

            Tween jumpTween = item.transform
                .DOJump(GetReceivePoint(), _jumpPower, 1, _duration)
                .SetEase(_jumpCurve);

            Tween rotateTween = item.transform
                .DORotateQuaternion(Quaternion.LookRotation(Random.insideUnitSphere), _duration)
                .SetEase(_rotateCurve);

            Tween scaleTween = item.transform
                .DOScale(initialScale, _scaleDuration)
                .SetEase(_scaleCurve);

            DOTween
                .Sequence()
                .Append(jumpTween)
                .Join(rotateTween)
                .Join(scaleTween)
                .OnComplete(() => { onComplete?.Invoke(); })
                .Play();
        }

        private void OnReceivedAmount(int amount)
        {
            _receiveContainer.Add(amount);
            OnReceived?.Invoke();

            if (_receiveContainer.IsFull.Value)
            {
                _receiveContainer.Clear();
                OnReceivedAll?.Invoke();
                StopTransferring();
            }
        }

        private Vector3 GetReceivePoint()
        {
            Vector3 center = _receiveToTransform.position;

            Vector3 receivePoint = center + Random.insideUnitSphere * _maxRange;

            receivePoint.y = center.y;

            return receivePoint;
        }

        private void OnDrawGizmosSelected()
        {
            if (_receiveToTransform == null)
                return;

            Gizmos.color = Color.green.WithAlpha(0.3f);
            Gizmos.DrawSphere(_receiveToTransform.position, _maxRange);
        }
    }
}