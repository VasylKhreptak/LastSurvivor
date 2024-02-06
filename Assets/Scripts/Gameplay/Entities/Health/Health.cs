using Gameplay.Entities.Health.Core;
using Plugins.Banks;
using UniRx;
using UnityEngine;

namespace Gameplay.Entities.Health
{
    public class Health : IHealth
    {
        private readonly ClampedFloatBank _health;

        public Health(float maxHealth) : this(maxHealth, maxHealth) { }

        public Health(float health, float maxHealth)
        {
            _health = new ClampedFloatBank(health, maxHealth);
        }

        public IReadOnlyReactiveProperty<float> Value => _health.Value;

        public IReadOnlyReactiveProperty<float> MaxValue => _health.MaxValue;

        public IReadOnlyReactiveProperty<float> FillAmount => _health.FillAmount;

        public IReadOnlyReactiveProperty<bool> IsFull => _health.IsFull;

        public IReadOnlyReactiveProperty<bool> IsDeath => _health.IsEmpty;

        public IReadOnlyReactiveProperty<float> OnDamaged =>
            _health.Value.Pairwise()
                .Where(pair => pair.Previous > pair.Current)
                .Select(pair => pair.Previous - pair.Current)
                .ToReactiveProperty();

        public void SetValue(float health) => _health.SetValue(health);

        public void SetMaxValue(float maxValue) => _health.SetMaxValue(maxValue);

        public void Add(float health) => _health.Add(health);

        public void TakeDamage(float damage) => _health.Spend(Mathf.Min(damage, _health.Value.Value));

        public void Kill() => _health.Clear();

        public void Restore() => _health.Fill();
    }
}