using System;
using Entities.Player;
using Infrastructure.Services.Input.Main.Core;
using Infrastructure.Services.PersistentData.Core;
using Plugins.Banks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Platforms.HelicopterPlatform
{
    public class HelicopterUpgradeZone : IInitializable, IDisposable
    {
        private readonly Collider _trigger;
        private readonly IntegerBank _gearsBank;
        private readonly IMainInputService _inputService;

        public HelicopterUpgradeZone(HelicopterPlatformViewReferences references, IPersistentDataService persistentDataService,
            IMainInputService inputService)
        {
            _trigger = references.UpgradeZoneTrigger;
            _gearsBank = persistentDataService.PersistentData.PlayerData.Resources.Gears;
            _inputService = inputService;
        }

        private readonly CompositeDisposable _triggerSubscriptions = new CompositeDisposable();
        private IDisposable _inputInteractionSubscription;

        public void Initialize() => StartObserving();

        public void Dispose() => StopObserving();

        private void StartObserving()
        {
            StopObserving();

            StartObservingTrigger();
        }

        private void StopObserving()
        {
            StopObservingTrigger();
            StopObservingInputInteraction();
            StopTransferringGears();
        }

        private void StartObservingTrigger()
        {
            StopObservingTrigger();

            _trigger.OnTriggerEnterAsObservable().Subscribe(OnTriggerEnter).AddTo(_triggerSubscriptions);
            _trigger.OnTriggerExitAsObservable().Subscribe(OnTriggerExit).AddTo(_triggerSubscriptions);
        }

        private void StopObservingTrigger() => _triggerSubscriptions.Clear();

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.TryGetComponent(out Player player) == false)
                return;

            StartObservingInputInteraction();
        }

        private void OnTriggerExit(Collider collider)
        {
            if (collider.TryGetComponent(out Player player) == false)
                return;

            StopObservingInputInteraction();
        }

        private void StartObservingInputInteraction()
        {
            StopObservingInputInteraction();

            _inputInteractionSubscription = _inputService.IsInteracting.Subscribe(isInteracting =>
            {
                if (isInteracting)
                {
                    StopTransferringGears();
                }
                else
                {
                    StartTransferringGears();
                }
            });
        }

        private void StopObservingInputInteraction() => _inputInteractionSubscription?.Dispose();

        private void StartTransferringGears()
        {
            StopTransferringGears();
        }

        private void StopTransferringGears() { }
    }
}