using System;
using Gameplay.Entities.Health.Core;
using Gameplay.Entities.Health.Damages;
using UniRx;
using UnityEngine;
using Utilities.PhysicsUtilities.Trigger;
using Visitor;
using Zenject;

namespace Gameplay.Entities.Zombie
{
    public class EnemyDetectionTriggerAwakener : IInitializable, IDisposable
    {
        private readonly SphereCollider _trigger;
        private readonly IHealth _health;
        private readonly TriggerZone<IVisitable<ZombieDamage>> _targetsZone;
        private readonly Preferences _preferences;

        public EnemyDetectionTriggerAwakener(SphereCollider trigger, IHealth health, TriggerZone<IVisitable<ZombieDamage>> targetsZone,
            Preferences preferences)
        {
            _trigger = trigger;
            _health = health;
            _targetsZone = targetsZone;
            _preferences = preferences;
        }

        private IDisposable _delaySubscription;
        private IDisposable _targetCountSubscription;
        private IDisposable _sleepDelaySubscription;

        public void Initialize()
        {
            StartObservingCount();
            StartObservingDamage();
        }

        public void Dispose()
        {
            StopObservingCount();
            StopObservingDamage();
            _delaySubscription?.Dispose();
        }

        private void StartObservingCount()
        {
            StopObservingCount();
            _targetCountSubscription = _targetsZone.Triggers
                .ObserveCountChanged()
                .DoOnSubscribe(() => OnTargetCountChanged(_targetsZone.Triggers.Count))
                .Subscribe(OnTargetCountChanged);
        }

        private void StopObservingCount() => _targetCountSubscription?.Dispose();

        private void StartObservingDamage()
        {
            StopObservingDamage();
            _sleepDelaySubscription = _health.OnDamaged
                .Subscribe(_ => Awaken());
        }

        private void StopObservingDamage() => _sleepDelaySubscription?.Dispose();

        private void OnTargetCountChanged(int count)
        {
            _delaySubscription?.Dispose();

            if (count > 0)
            {
                _trigger.radius = _preferences.AwakenedRadius;
                return;
            }

            Awaken();
        }

        private void Awaken()
        {
            _trigger.radius = _preferences.AwakenedRadius;
            _delaySubscription?.Dispose();
            _delaySubscription = Observable
                .Timer(TimeSpan.FromSeconds(_preferences.SleepTime))
                .Subscribe(_ => _trigger.radius = _preferences.BaseRadius);
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private float _awakenedRadius = 10f;
            [SerializeField] private float _baseRadius = 5f;
            [SerializeField] private float _sleepTime = 5f;

            public float AwakenedRadius => _awakenedRadius;
            public float BaseRadius => _baseRadius;
            public float SleepTime => _sleepTime;
        }
    }
}