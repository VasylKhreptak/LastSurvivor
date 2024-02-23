using System;
using Gameplay.Aim;
using Gameplay.Weapons.Core;
using UniRx;
using Zenject;

namespace Gameplay.Weapons
{
    public class WeaponShooter : IInitializable, IDisposable
    {
        private readonly Trackpad _trackpad;
        private readonly IWeapon _weapon;

        public WeaponShooter(Trackpad trackpad, IWeapon weapon)
        {
            _trackpad = trackpad;
            _weapon = weapon;
        }

        private IDisposable _subscription;

        public void Initialize() => StartObserving();

        public void Dispose()
        {
            StopObserving();
            _weapon.StopShooting();
        }

        private void StartObserving()
        {
            _subscription = _trackpad.IsPressed
                .Skip(1)
                .Subscribe(isPressed =>
                {
                    if (isPressed)
                        _weapon.StartShooting();
                    else
                        _weapon.StopShooting();
                });
        }

        private void StopObserving() => _subscription?.Dispose();
    }
}