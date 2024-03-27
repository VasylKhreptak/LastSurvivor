using UniRx;
using UnityEngine;

namespace Plugins.Banks.Core
{
    public abstract class ClampedBank<T> : Bank<T>
    {
        protected readonly ReactiveProperty<T> _maxValue;
        protected readonly ReactiveProperty<float> _fillAmount;
        protected readonly ReactiveProperty<T> _leftToFill;

        public ClampedBank()
        {
            _maxValue = new ReactiveProperty<T>();
            _fillAmount = new ReactiveProperty<float>();
            _leftToFill = new ReactiveProperty<T>();
        }

        public ClampedBank(T value, T maxValue) : base(value)
        {
            maxValue = ClampMaxValue(maxValue);
            value = Clamp(value, maxValue);

            _value.Value = value;
            _maxValue = new ReactiveProperty<T>(maxValue);
            _fillAmount = new ReactiveProperty<float>(CalculateFillAmount());
            _leftToFill = new ReactiveProperty<T>(CalculateLeftToFill());
        }

        public IReadOnlyReactiveProperty<T> MaxValue => _maxValue;
        public IReadOnlyReactiveProperty<float> FillAmount => _fillAmount;
        public IReadOnlyReactiveProperty<T> LeftToFill => _leftToFill;

        public IReadOnlyReactiveProperty<bool> IsFull =>
            _fillAmount.Select(fillAmount => Mathf.Approximately(fillAmount, 1f)).ToReadOnlyReactiveProperty();

        protected abstract T ClampMaxValue(T maxValue);

        protected abstract T Clamp(T value, T maxValue);

        protected abstract void UpdateFillAmount();

        protected abstract float CalculateFillAmount();

        protected abstract void ClampValue();

        public abstract void SetMaxValue(T value);

        public abstract void Fill();

        protected abstract void UpdateLeftToFill();

        protected abstract T CalculateLeftToFill();
    }
}