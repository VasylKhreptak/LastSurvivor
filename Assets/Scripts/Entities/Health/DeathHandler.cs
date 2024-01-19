using System;
using Entities.Health.Core;
using UniRx;
using Zenject;

namespace Entities.Health
{
    public class DeathHandler : IInitializable, IDisposable
    {
        private readonly IHealth _health;

        public DeathHandler(IHealth health)
        {
            _health = health;
        }

        private IDisposable _subscription;

        public void Initialize() => _subscription = _health.IsDeath.Where(x => x).Subscribe(_ => OnDied());

        public void Dispose() => _subscription?.Dispose();

        protected virtual void OnDied() { }
    }
}