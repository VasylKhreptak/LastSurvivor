using System;
using Extensions;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapons.Minigun
{
    public class BarrelSpiner : ITickable, IDisposable
    {
        private readonly Preferences _preferences;

        public BarrelSpiner(Preferences preferences)
        {
            _preferences = preferences;
        }

        private IDisposable _accelerationSubscription;

        private float _rotateSpeed;

        public float RotateSpeed => _rotateSpeed;
        public Preferences Settings => _preferences;

        public void Tick() => Rotate();

        public void Dispose() => _accelerationSubscription?.Dispose();

        private void Rotate()
        {
            _preferences.BarrelTransform.rotation *=
                Quaternion.Euler(0, 0, (!_preferences.Reverse).ToSign() * _rotateSpeed * Time.deltaTime);
        }

        public void SpinUp(Action onComplete = null)
        {
            _accelerationSubscription?.Dispose();
            _accelerationSubscription = Observable
                .EveryUpdate()
                .Subscribe(_ =>
                {
                    _rotateSpeed = Mathf.Clamp(_rotateSpeed + _preferences.Acceleration * Time.deltaTime, 0,
                        _preferences.MaxRotateSpeed);

                    if (_rotateSpeed >= _preferences.MaxRotateSpeed)
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
                    _rotateSpeed = Mathf.Clamp(_rotateSpeed - _preferences.Deceleration * Time.deltaTime, 0,
                        _preferences.MaxRotateSpeed);

                    if (_rotateSpeed <= 0)
                    {
                        _accelerationSubscription?.Dispose();
                        onComplete?.Invoke();
                    }
                });
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private Transform _barrelTransform;
            [SerializeField] private bool _reverse;
            [SerializeField] [Min(0)] private float _maxRotateSpeed;
            [SerializeField] [Min(0)] private float _acceleration;
            [SerializeField] [Min(0)] private float _deceleration;

            public Transform BarrelTransform => _barrelTransform;
            public bool Reverse => _reverse;
            public float MaxRotateSpeed => _maxRotateSpeed;
            public float Acceleration => _acceleration;
            public float Deceleration => _deceleration;
        }
    }
}