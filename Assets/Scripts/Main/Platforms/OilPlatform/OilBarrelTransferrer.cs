using System;
using Grid;
using Main.Entities.Player;
using UniRx;
using UnityEngine;
using Zenject;

namespace Main.Platforms.OilPlatform
{
    public class OilBarrelTransferrer : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] private float _interval = 0.4f;

        private GridStack _gridStack;
        private GridStack _playerGridStack;

        [Inject]
        private void Constructor(GridStack gridStack, Player player)
        {
            _gridStack = gridStack;
            _playerGridStack = player.BarrelGridStack;
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

        private void OnDisable() => StopTransferring();

        #endregion

        private void StartTransferring()
        {
            StopTransferring();

            _transferSubscription = Observable
                .Interval(TimeSpan.FromSeconds(_interval))
                .DoOnSubscribe(TryTransfer)
                .Subscribe(_ => TryTransfer());
        }

        private void StopTransferring() => _transferSubscription?.Dispose();

        private void TryTransfer()
        {
            if (_gridStack.Data.IsEmpty.Value || _playerGridStack.Data.IsFull.Value)
                return;

            if (_gridStack.TryPop(out GameObject barrelGameObject) == false)
            {
                _gridStack.TryPush(barrelGameObject);
                return;
            }

            _playerGridStack.TryPush(barrelGameObject);
        }
    }
}