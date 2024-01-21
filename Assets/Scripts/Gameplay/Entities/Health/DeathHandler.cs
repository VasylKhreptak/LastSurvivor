using System;
using Gameplay.Entities.Health.Core;
using UniRx;
using Zenject;

namespace Gameplay.Entities.Health
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