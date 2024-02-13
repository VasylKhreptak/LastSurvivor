using System;
using Infrastructure.Services.Vibration.Core;
using Lofelt.NiceVibrations;
using IInitializable = Zenject.IInitializable;

namespace Main.Platforms.Zones
{
    public class ReceiveZoneVibration : IInitializable, IDisposable
    {
        private readonly ReceiveZone _receiveZone;
        private readonly IVibrationService _vibrationService;

        public ReceiveZoneVibration(ReceiveZone receiveZone, IVibrationService vibrationService)
        {
            _receiveZone = receiveZone;
            _vibrationService = vibrationService;
        }

        public void Initialize() => StartObserving();

        public void Dispose() => StopObserving();

        private void StartObserving() => _receiveZone.OnReceivedAll += Vibrate;

        private void StopObserving() => _receiveZone.OnReceivedAll -= Vibrate;

        private void Vibrate() => _vibrationService.Vibrate(HapticPatterns.PresetType.LightImpact);
    }
}