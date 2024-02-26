using System;
using DG.Tweening;
using Infrastructure.Data.Static;
using Infrastructure.Data.Static.Core;
using Infrastructure.Services.Input.Main.Core;
using Infrastructure.Services.StaticData.Core;
using Plugins.Animations.Extensions;
using Plugins.Banks;
using UniRx;
using UnityEngine;
using Zenject;
using Player = Main.Entities.Player.Player;
using Random = UnityEngine.Random;

namespace Main.Platforms.Zones
{
    public class ReceiveZone : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _receiveToTransform;
        [SerializeField] private Prefab _prefabType;

        [Header("Preferences")]
        [SerializeField] private bool _clearOnReceivedAll = true;

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
        private GamePrefabs _gamePrefabs;
        private Player _player;

        [Inject]
        public void Constructor(IntegerBank bank, ClampedIntegerBank receiveContainer, IMainInputService inputService,
            IStaticDataService staticDataService, Player player)
        {
            _bank = bank;
            _receiveContainer = receiveContainer;
            _inputService = inputService;
            _gamePrefabs = staticDataService.Prefabs;
            _player = player;
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
            if (other.TryGetComponent(out Player _))
                StartObservingInputInteraction();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player _))
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

            _inputInteractionSubscription = _inputService.IsInteracting
                .Skip(1)
                .Subscribe(isInteracting =>
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

            if (targetTransferCount == 0)
                return;

            int currentTransferCount = 0;

            _transferSubscription = Observable
                .Interval(TimeSpan.FromSeconds(_interval))
                .Subscribe(_ =>
                {
                    if (_bank.IsEmpty.Value || currentTransferCount++ == targetTransferCount)
                    {
                        StopTransferring();
                        return;
                    }

                    Transfer();
                });
        }

        private void StopTransferring() => _transferSubscription?.Dispose();

        private int GetTransferCount()
        {
            int leftToFill = _receiveContainer.LeftToFill.Value - _currentTransferAmount;

            if (leftToFill == 0)
                return 0;

            float remainder = leftToFill % _maxTransfer;
            return remainder == 0 ? leftToFill / _maxTransfer : leftToFill / _maxTransfer + 1;
        }

        private void Transfer()
        {
            int transferAmount = Mathf.Min(_bank.Value.Value, _maxTransfer,
                _receiveContainer.LeftToFill.Value - _currentTransferAmount);

            _bank.Spend(transferAmount);

            GameObject transferringItem =
                Instantiate(_gamePrefabs[_prefabType], _player.InputOutputTransform.position, Quaternion.identity);

            _currentTransferAmount += transferAmount;
            ThrowItem(transferringItem, () => //callback is stopping after calling _bank.Add(value) in OnReceivedAmount(...)
            {
                Destroy(transferringItem);
                _currentTransferAmount -= transferAmount;
                OnReceivedAmount(transferAmount);
            });
        }

        private void ThrowItem(GameObject item, Action onComplete)
        {
            Vector3 initialScale = item.transform.localScale;
            item.transform.localScale = Vector3.zero;

            Vector3 targetPosition = GetReceivePoint();

            Tween jumpTween = item.transform
                .DOJump(targetPosition, _jumpPower, 1, _duration)
                .SetEase(_jumpCurve);

            Tween rotateTween = item.transform
                .DORotateQuaternion(Quaternion.LookRotation(Random.insideUnitSphere), _duration)
                .SetEase(_rotateCurve);

            Tween scaleTween = item.transform
                .DOScale(initialScale, _scaleDuration)
                .SetEase(_scaleCurve);

            DOTween
                .Sequence()
                .Join(jumpTween)
                .Join(rotateTween)
                .Join(scaleTween)
                .OnComplete(() => onComplete?.Invoke())
                .Play();
        }

        private void OnReceivedAmount(int amount) // here is crunch(
        {
            OnReceived?.Invoke();

            // if (_receiveContainer.IsFull.Value)
            
            if (Mathf.Clamp(_receiveContainer.Value.Value + amount, 0, _receiveContainer.MaxValue.Value) ==
                _receiveContainer.MaxValue.Value)
            {
                if (_clearOnReceivedAll)
                    _receiveContainer.Clear();

                StopTransferring();
                OnReceivedAll?.Invoke();
            }

            _receiveContainer.Add(amount);
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