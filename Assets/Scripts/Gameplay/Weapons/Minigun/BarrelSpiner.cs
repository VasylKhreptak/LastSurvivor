using System;
using Extensions;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapons.Minigun
{
    public class BarrelSpiner : ITickable, IDisposable
    {
        public readonly Preferences Settings;

        public BarrelSpiner(Preferences preferences)
        {
            Settings = preferences;
        }

        private IDisposable _accelerationSubscription;

        public float RotateSpeed { get; private set; }

        public void Tick() => Rotate();

        public void Dispose() => _accelerationSubscription?.Dispose();

        private void Rotate()
        {
            Settings.BarrelTransform.rotation *=
                Quaternion.Euler(0, 0, (!Settings.Reverse).ToSign() * RotateSpeed * Time.deltaTime);
        }

        public void SpinUp(Action onComplete = null)
        {
            _accelerationSubscription?.Dispose();
            _accelerationSubscription = Observable
                .EveryUpdate()
                .Subscribe(_ =>
                {
                    RotateSpeed = Mathf.Clamp(RotateSpeed + Settings.Acceleration * Time.deltaTime, 0,
                        Settings.MaxRotateSpeed);

                    if (RotateSpeed >= Settings.MaxRotateSpeed)
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
                    RotateSpeed = Mathf.Clamp(RotateSpeed - Settings.Deceleration * Time.deltaTime, 0,
                        Settings.MaxRotateSpeed);

                    if (RotateSpeed <= 0)
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