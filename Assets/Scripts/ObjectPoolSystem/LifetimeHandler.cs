using System;
using UniRx;
using UnityEngine;
using Zenject.Infrastructure.Toggleable.Core;

namespace ObjectPoolSystem
{
    public class LifetimeHandler : IEnableable, IDisableable
    {
        private readonly GameObject _gameObject;
        private readonly Preferences _preferences;

        public LifetimeHandler(GameObject gameObject, Preferences preferences)
        {
            _gameObject = gameObject;
            _preferences = preferences;
        }

        private IDisposable _subscription;

        public void Enable() => StartTimer();

        public void Disable() => StopTimer();

        private void StartTimer()
        {
            _subscription = Observable
                .Timer(TimeSpan.FromSeconds(_preferences.Lifetime))
                .Subscribe(_ => _gameObject.SetActive(false));
        }

        private void StopTimer() => _subscription?.Dispose();

        [Serializable]
        public class Preferences
        {
            [SerializeField] private float _lifetime = 1f;

            public float Lifetime => _lifetime;
        }
    }
}