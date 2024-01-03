using System;
using Extensions;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapons.Minigun
{
    public class BarrelRotator : ITickable, IDisposable
    {
        private readonly Transform _barrelTransform;
        private readonly Preferences _preferences;

        public BarrelRotator(Transform barrelTransform, Preferences preferences)
        {
            _barrelTransform = barrelTransform;
            _preferences = preferences;
        }

        private IDisposable _accelerationSubscription;

        public void Tick() => Rotate();

        public void Dispose() => _accelerationSubscription?.Dispose();

        private void Rotate()
        {
            _barrelTransform.rotation *= Quaternion.Euler(0, 0, (!_preferences.Reverse).ToSign() * _preferences.RotateSpeed);
        }

        public void SpinUp(Action onComplete = null)
        {
            _accelerationSubscription?.Dispose();
            _accelerationSubscription = Observable
                .EveryUpdate()
                .Subscribe(_ =>
                {
                    _preferences.RotateSpeed = Mathf.Clamp(_preferences.RotateSpeed + _preferences.Acceleration * Time.deltaTime, 0,
                        _preferences.MaxRotateSpeed);

                    if (_preferences.RotateSpeed >= _preferences.MaxRotateSpeed)
                    {
                        _accelerationSubscription?.Dispose();
                        onComplete?.Invoke();
                    }
                });
        }

        public void SpinDown(Action onComplete = null)
        {
            _accelerationSubscription?.Dispose();
            _accelerationSubscription = Observable
                .EveryUpdate()
                .Subscribe(_ =>
                {
                    _preferences.RotateSpeed = Mathf.Clamp(_preferences.RotateSpeed - _preferences.Acceleration * Time.deltaTime, 0,
                        _preferences.MaxRotateSpeed);

                    if (_preferences.RotateSpeed <= 0)
                    {
                        _accelerationSubscription?.Dispose();
                        onComplete?.Invoke();
                    }
                });
        }

        [Serializable]
        public class Preferences
        {
            public bool Reverse;
            [ReadOnly] public float RotateSpeed;
            public float MaxRotateSpeed;
            public float Acceleration;
        }
    }
}