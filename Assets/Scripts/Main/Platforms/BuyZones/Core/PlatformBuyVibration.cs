using System;
using Infrastructure.Services.Vibration.Core;
using Lofelt.NiceVibrations;
using UnityEngine;
using IInitializable = Zenject.IInitializable;

namespace Main.Platforms.BuyZones.Core
{
    public class PlatformBuyVibration : IInitializable, IDisposable
    {
        private readonly PlatformBuyer _platformBuyer;
        private readonly IVibrationService _vibrationService;

        public PlatformBuyVibration(PlatformBuyer platformBuyer, IVibrationService vibrationService)
        {
            _platformBuyer = platformBuyer;
            _vibrationService = vibrationService;
        }

        public void Initialize() => StartObserving();

        public void Dispose() => StopObserving();

        private void StartObserving() => _platformBuyer.OnBought += OnPlatformBought;

        private void StopObserving() => _platformBuyer.OnBought -= OnPlatformBought;

        private void OnPlatformBought(GameObject _) => Vibrate();

        private void Vibrate() => _vibrationService.Vibrate(HapticPatterns.PresetType.LightImpact);
    }
}