using System;
using Gameplay.Entities.Health.Core;
using UniRx;
using Zenject;

namespace Gameplay.Entities.Health
{
    public abstract class DamageObserver : IInitializable, IDisposable
    {
        private readonly IHealth _health;

        protected DamageObserver(IHealth health)
        {
            _health = health;
        }

        private IDisposable _subscription;

        public void Initialize() => StartObserving();

        public void Dispose() => StopObserving();

        private void StartObserving()
        {
            _health.Value
                .Pairwise()
                .Where(pair => pair.Previous > pair.Current)
                .Subscribe(pair => OnTookDamage(pair.Previous - pair.Current));
        }

        private void StopObserving() => _subscription?.Dispose();

        protected abstract void OnTookDamage(float damage);
    }
}