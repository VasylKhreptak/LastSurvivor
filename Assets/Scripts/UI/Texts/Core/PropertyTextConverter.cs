using System;
using TMPro;
using UniRx;
using Zenject;

namespace UI.Texts.Core
{
    public class PropertyTextConverter<T> : IInitializable, IDisposable
    {
        private readonly TMP_Text _tmp;
        private readonly IObservable<T> _observable;

        public PropertyTextConverter(TMP_Text tmp, IReadOnlyReactiveProperty<T> property)
        {
            _tmp = tmp;
            _observable = property;
        }

        private IDisposable _subscription;

        public void Initialize()
        {
            Dispose();
            _subscription = _observable.Subscribe(OnValueChanged);
        }

        public void Dispose() => _subscription?.Dispose();

        public void SetObservable(IObservable<T> property)
        {
            Dispose();
            _subscription = property.Subscribe(OnValueChanged);
            Initialize();
        }

        private void OnValueChanged(T value) => _tmp.text = value.ToString();
    }
}