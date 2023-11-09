using System;
using DG.Tweening;
using Entities.Player;
using Infrastructure.Services.Input.Main.Core;
using Plugins.Banks;
using UniRx;
using UnityEngine;
using Zenject;

namespace Platforms
{
    public abstract class TransferZone : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _transferTo;

        [Header("Jump Preferences")]
        [SerializeField] private float _jumpDuration;
        [SerializeField] private float _jumpPower = 3f;
        [SerializeField] private AnimationCurve _jumpCurve;

        [Header("Transfer Preferences")]
        [SerializeField] private float _interval = 0.1f;
        [SerializeField] private int _maxTranferrableValue = 10;

        private IntegerBank _bank;
        private ClampedIntegerBank _transferContainer;
        private IMainInputService _inputService;
        private Player _player;
        private GameObject _transferringItemPrefab;

        [Inject]
        public void Constructor(IntegerBank bank, ClampedIntegerBank upgradeContainer, IMainInputService inputService, Player player,
            GameObject transferringItemPrefab)
        {
            _bank = bank;
            _transferContainer = upgradeContainer;
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
                    StartTransferringGears();
            });
        }

        private void StopObservingInputInteraction() => _inputInteractionSubscription?.Dispose();

        private void StartTransferringGears()
        {
            StopTransferring();

            _transferSubscription = Observable
                .Interval(TimeSpan.FromSeconds(_interval))
                .DoOnSubscribe(() =>
                {
                    if (TryTransfer() == false)
                        StopTransferring();
                })
                .Subscribe(_ =>
                {
                    if (TryTransfer() == false)
                        StopTransferring();
                });
        }

        private void StopTransferring() => _transferSubscription?.Dispose();

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

            OnTransferred(transferAmount);

            GameObject transferringItem = Instantiate(_transferringItemPrefab, _player.transform.position, Quaternion.identity);

            transferringItem.transform
                .DOJump(_transferTo.position, _jumpPower, 1, _jumpDuration)
                .SetEase(_jumpCurve)
                .OnComplete(() => Destroy(transferringItem))
                .Play();
        }

        private void OnTransferred(int amount)
        {
            _transferContainer.Add(amount);

            if (_transferContainer.IsFull.Value)
            {
                OnAllTransferred();
                _transferContainer.Clear();
                StopTransferring();
            }
        }

        protected abstract void OnAllTransferred();
    }
}