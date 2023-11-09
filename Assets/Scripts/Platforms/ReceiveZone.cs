using System;
using DG.Tweening;
using Entities.Player;
using Infrastructure.Services.Input.Main.Core;
using Plugins.Banks;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Platforms
{
    public abstract class ReceiveZone : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _receiveTo;

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
        [SerializeField] private int _maxTranferrableValue = 10;

        private IntegerBank _bank;
        private ClampedIntegerBank _receiveContainer;
        private IMainInputService _inputService;
        private Player _player;
        private GameObject _transferringItemPrefab;

        [Inject]
        public void Constructor(IntegerBank bank, ClampedIntegerBank upgradeContainer, IMainInputService inputService, Player player,
            GameObject transferringItemPrefab)
        {
            _bank = bank;
            _receiveContainer = upgradeContainer;
            _inputService = inputService;
            _player = player;
            _transferringItemPrefab = transferringItemPrefab;
        }

        private IDisposable _inputInteractionSubscription;
        private IDisposable _transferSubscription;

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
            StopReceiving();
        }

        private void StartObservingInputInteraction()
        {
            StopObservingInputInteraction();

            _inputInteractionSubscription = _inputService.IsInteracting.Subscribe(isInteracting =>
            {
                if (isInteracting)
                    StopReceiving();
                else
                    StartReceiving();
            });
        }

        private void StopObservingInputInteraction() => _inputInteractionSubscription?.Dispose();

        private void StartReceiving()
        {
            StopReceiving();

            _transferSubscription = Observable
                .Interval(TimeSpan.FromSeconds(_interval))
                .DoOnSubscribe(() =>
                {
                    if (TryTransfer() == false)
                        StopReceiving();
                })
                .Subscribe(_ =>
                {
                    if (TryTransfer() == false)
                        StopReceiving();
                });
        }

        private void StopReceiving() => _transferSubscription?.Dispose();

        private bool TryTransfer()
        {
            if (_bank.IsEmpty.Value)
                return false;

            Transfer();
            return true;
        }

        private void Transfer()
        {
            int transferAmount = Mathf.Min(_bank.Value.Value, _maxTranferrableValue);

            _bank.Spend(transferAmount);

            OnReceived(transferAmount);

            GameObject transferringItem =
                Instantiate(_transferringItemPrefab, _player.InputOutputTransform.position, Quaternion.identity);

            Vector3 initialScale = transferringItem.transform.localScale;
            transferringItem.transform.localScale = Vector3.zero;

            Tween jumpTween = transferringItem.transform.DOJump(_receiveTo.position, _jumpPower, 1, _duration).SetEase(_jumpCurve);
            Tween rotateTween = transferringItem.transform
                .DORotateQuaternion(Quaternion.LookRotation(Random.insideUnitSphere), _duration)
                .SetEase(_rotateCurve);
            Tween scaleTween = transferringItem.transform.DOScale(initialScale, _scaleDuration).SetEase(_scaleCurve);

            DOTween
                .Sequence()
                .Append(jumpTween)
                .Join(rotateTween)
                .Join(scaleTween)
                .OnComplete(() => Destroy(transferringItem))
                .Play();
        }

        private void OnReceived(int amount)
        {
            _receiveContainer.Add(amount);

            if (_receiveContainer.IsFull.Value)
            {
                OnReceivedAll();
                _receiveContainer.Clear();
                StopReceiving();
            }
        }

        protected abstract void OnReceivedAll();
    }
}