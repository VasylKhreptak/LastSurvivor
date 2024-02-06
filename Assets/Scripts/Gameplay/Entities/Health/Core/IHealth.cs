using UniRx;

namespace Gameplay.Entities.Health.Core
{
    public interface IHealth
    {
        public IReadOnlyReactiveProperty<float> Value { get; }

        public IReadOnlyReactiveProperty<float> MaxValue { get; }

        public IReadOnlyReactiveProperty<float> FillAmount { get; }

        public IReadOnlyReactiveProperty<bool> IsFull { get; }

        public IReadOnlyReactiveProperty<bool> IsDeath { get; }
        
        public IReadOnlyReactiveProperty<float> OnDamaged { get; }

        public void SetValue(float health);

        public void SetMaxValue(float maxValue);

        public void Add(float health);

        public void TakeDamage(float damage);

        public void Kill();

        public void Restore();
    }
}