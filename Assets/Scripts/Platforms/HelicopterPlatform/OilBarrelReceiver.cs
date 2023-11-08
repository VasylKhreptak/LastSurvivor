using System;
using Data.Persistent;
using DG.Tweening;
using Entities.Player;
using Grid;
using UniRx;
using UnityEngine;
using Zenject;

namespace Platforms.HelicopterPlatform
{
    public class OilBarrelReceiver : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _attractorTransform;

        [Header("Preferences")]
        [SerializeField] private float _interval = 0.3f;
        [SerializeField] private float _duration = 0.5f;

        [Header("Jump Preferences")]
        [SerializeField] private float _jumpPower = 1f;
        [SerializeField] private AnimationCurve _jumpCurve;

        [Header("Rotate Preferences")]
        [SerializeField] private AnimationCurve _rotateCurve;

        private GridStack _playerGrid;
        private HelicopterData _helicopterData;

        [Inject]
        private void Constructor(Player player, HelicopterData helicopterData)
        {
            _playerGrid = player.BarrelGridStack;
            _helicopterData = helicopterData;
        }

        private IDisposable _subscription;

        #region MonoBehaviour

        private void OnValidate() => _attractorTransform ??= GetComponent<Transform>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player _))
                StartReceiving();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player _))
                StopReceiving();
        }

        private void OnDisable() => StopReceiving();

        #endregion

        private void StartReceiving()
        {
            StopReceiving();

            _subscription = Observable
                .Interval(TimeSpan.FromSeconds(_interval))
                .DoOnSubscribe(ReceiveBarrel)
                .Subscribe(_ => ReceiveBarrel());
        }

        private void StopReceiving() => _subscription?.Dispose();

        private void ReceiveBarrel()
        {
            if (_playerGrid.IsEmpty.Value || _playerGrid.TryPop(out GameObject barrel) == false)
                return;

            Tween jumpTween = barrel.transform
                .DOJump(_attractorTransform.position, _jumpPower, 1, _duration)
                .SetEase(_jumpCurve);

            Tween rotateTween = barrel.transform
                .DORotate(_attractorTransform.rotation.eulerAngles, _duration)
                .SetEase(_rotateCurve);

            DOTween
                .Sequence()
                .Append(jumpTween)
                .Join(rotateTween)
                .OnComplete(() => OnReceived(barrel))
                .Play();
        }

        private void OnReceived(GameObject barrel)
        {
            _helicopterData.FuelTank.Add(1);
            Destroy(barrel);
        }
    }
}