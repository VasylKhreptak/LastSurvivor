using System;
using Gameplay.Aim;
using UniRx;
using Zenject;

namespace Gameplay.Weapons
{
    public class WeaponShooter : IInitializable, IDisposable
    {
        private readonly Trackpad _trackpad;
        private readonly WeaponHolder _weaponHolder;

        public WeaponShooter(Trackpad trackpad, WeaponHolder weaponHolder)
        {
            _trackpad = trackpad;
            _weaponHolder = weaponHolder;
        }

        private IDisposable _subscription;

        public void Initialize() => StartObserving();

        public void Dispose()
        {
            StopObserving();
            _weaponHolder.Instance.Value?.StopShooting();
        }

        private void StartObserving()
        {
            _subscription = _trackpad.IsPressed
                .Skip(1)
                .Subscribe(isPressed =>
                {
                    if (isPressed)
                        _weaponHolder.Instance.Value?.StartShooting();
                    else
                        _weaponHolder.Instance.Value?.StopShooting();
                });
        }

        private void StopObserving() => _subscription?.Dispose();
    }
}