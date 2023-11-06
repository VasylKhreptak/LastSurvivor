using System;
using Entities.Player;
using Infrastructure.Services.Input.Main.Core;
using Plugins.Banks;
using UniRx;
using UnityEngine;
using Zenject;

namespace Platforms
{
    public abstract class PlatformUpgradeZone : MonoBehaviour
    {
        private IntegerBank _bank;
        private ClampedIntegerBank _upgradeContainer;
        private IMainInputService _inputService;

        [Inject]
        public void Constructor(IntegerBank bank, IMainInputService inputService)
        {
            _bank = bank;
            _inputService = inputService;
        }

        private IDisposable _inputInteractionSubscription;

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
            StopTransferringGears();
        }

        private void StartObservingInputInteraction()
        {
            StopObservingInputInteraction();

            _inputInteractionSubscription = _inputService.IsInteracting.Subscribe(isInteracting =>
            {
                if (isInteracting)
                {
                    StopTransferringGears();
                    Debug.Log("Stopped Transferring Gears");
                }
                else
                {
                    StartTransferringGears();
                    Debug.Log("Started Transferring Gears");
                }
            });
        }

        private void StopObservingInputInteraction() => _inputInteractionSubscription?.Dispose();

        private void StartTransferringGears()
        {
            StopTransferringGears();
        }

        private void StopTransferringGears() { }

        protected abstract void Upgrade();
    }
}