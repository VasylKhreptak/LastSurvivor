using System;
using Gameplay.Entities.Health.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.Entities.Explosive.Barrel
{
    public class BarrelFireBehaviour : IInitializable, IDisposable
    {
        private readonly IHealth _health;
        private readonly Preferences _preferences;

        public BarrelFireBehaviour(IHealth health, Preferences preferences)
        {
            _health = health;
            _preferences = preferences;
        }

        private IDisposable _healthSubscription;
        private IDisposable _damageSubscription;

        public void Initialize()
        {
            StartObservingHealth();
        }

        public void Dispose()
        {
            StopObservingHealth();
            StopDamaging();
        }

        private void StartObservingHealth()
        {
            _healthSubscription = _health.FillAmount
                .Subscribe(fillAmount =>
                {
                    if (fillAmount <= _preferences.HealthPercentageThreshold)
                    {
                        EnableFire();
                        StartDamaging();
                        StopObservingHealth();
                    }
                });
        }

        private void StopObservingHealth() => _healthSubscription?.Dispose();

        private void EnableFire() => _preferences.FireParticle.SetActive(true);

        private void StartDamaging()
        {
            _damageSubscription = Observable
                .EveryUpdate()
                .Subscribe(_ => ApplyDamage(_preferences.DamagePerSecond * Time.deltaTime));
        }

        private void StopDamaging() => _damageSubscription?.Dispose();

        private void ApplyDamage(float damage) => _health.TakeDamage(damage);

        [Serializable]
        public class Preferences
        {
            [SerializeField] private GameObject _fireParticle;
            [SerializeField] private float _healthPercentageThreshold = 0.4f;
            [SerializeField] private float _damagePerSecond = 10f;

            public GameObject FireParticle => _fireParticle;
            public float DamagePerSecond => _damagePerSecond;
            public float HealthPercentageThreshold => _healthPercentageThreshold;
        }
    }
}