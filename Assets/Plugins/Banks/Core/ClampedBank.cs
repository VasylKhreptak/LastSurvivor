using System;
using Plugins.Banks.Extensions;
using UniRx;
using UnityEngine;

namespace Plugins.Banks.Core
{
    public abstract class ClampedBank<T> : Bank<T> where T : IComparable<T>
    {
        protected readonly ReactiveProperty<T> _maxValue;
        protected readonly ReactiveProperty<float> _fillAmount;
        protected readonly ReactiveProperty<T> _leftToFill;
        public readonly IReadOnlyReactiveProperty<bool> IsFull;
        public readonly IReadOnlyReactiveProperty<bool> IsEmpty;

        public ClampedBank()
        {
            _maxValue = new ReactiveProperty<T>();
            _fillAmount = new ReactiveProperty<float>();
            _leftToFill = new ReactiveProperty<T>();
            IsFull = _fillAmount.Select(fillAmount => Mathf.Approximately(fillAmount, 1f)).ToReadOnlyReactiveProperty();
            IsEmpty = _fillAmount.Select(fillAmount => Mathf.Approximately(fillAmount, 0f)).ToReadOnlyReactiveProperty();
        }

        public ClampedBank(T value, T maxValue) : base(value)
        {
            maxValue = GenericComparer.Max(default, maxValue);
            value = GenericComparer.Clamp(value, default, maxValue);

            _value.Value = value;
            _maxValue = new ReactiveProperty<T>(maxValue);
            _fillAmount = new ReactiveProperty<float>(CalculateFillAmount());
            _leftToFill = new ReactiveProperty<T>(CalculateLeftToFill());
            IsFull = _fillAmount.Select(fillAmount => Mathf.Approximately(fillAmount, 1f)).ToReadOnlyReactiveProperty();
            IsEmpty = _fillAmount.Select(fillAmount => Mathf.Approximately(fillAmount, 0f)).ToReadOnlyReactiveProperty();
        }

        public IReadOnlyReactiveProperty<T> MaxValue => _maxValue;
        public IReadOnlyReactiveProperty<float> FillAmount => _fillAmount;
        public IReadOnlyReactiveProperty<T> LeftToFill => _leftToFill;

        protected abstract void UpdateFillAmount();

        protected abstract float CalculateFillAmount();

        protected abstract void ClampValue();

        public abstract void SetMaxValue(T value);

        public abstract void Fill();

        protected abstract void UpdateLeftToFill();

        protected abstract T CalculateLeftToFill();
    }
}