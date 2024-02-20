using System;
using Gameplay.Entities.Health.Core;
using Infrastructure.Services.Vibration.Core;
using Lofelt.NiceVibrations;
using UniRx;
using UnityEngine;
using IInitializable = Zenject.IInitializable;

namespace Vibration
{
    public class DamageVibration : IInitializable, IDisposable
    {
        private readonly IHealth _health;
        private readonly IVibrationService _vibrationService;
        private readonly Preferences _preferences;

        public DamageVibration(IHealth health, IVibrationService vibrationService, Preferences preferences)
        {
            _health = health;
            _vibrationService = vibrationService;
            _preferences = preferences;
        }

        private IDisposable _subscription;

        public void Initialize() => StartObserving();

        public void Dispose() => StopObserving();

        private void StartObserving() => _subscription = _health.OnDamaged.Subscribe(_ => Vibrate());

        private void StopObserving() => _subscription?.Dispose();

        private void Vibrate() => _vibrationService.Vibrate(_preferences.Preset);

        [Serializable]
        public class Preferences
        {
            [SerializeField] private HapticPatterns.PresetType _preset = HapticPatterns.PresetType.RigidImpact;

            public HapticPatterns.PresetType Preset => _preset;
        }
    }
}