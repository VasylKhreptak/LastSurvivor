using System;
using Plugins.Banks.Extensions;
using UniRx;

namespace Plugins.Banks.Core
{
    public abstract class Bank<T>
    {
        protected readonly ReactiveProperty<T> _value;

        public IReadOnlyReactiveProperty<T> Value => _value;

        public Bank() => _value = new ReactiveProperty<T>();

        public Bank(T value) => _value = new ReactiveProperty<T>(Clamp(value));

        public abstract IReadOnlyReactiveProperty<bool> IsEmpty { get; }

        protected abstract T Clamp(T value);
        
        public abstract void Add(T value);

        public abstract bool Spend(T value);

        public abstract void SetValue(T value);

        public abstract void Clear();

        public abstract bool HasEnough(T value);
    }
}