using Plugins.Banks;
using UniRx;
using UnityEngine;

namespace Gameplay.Weapons.Core
{
    public interface IWeapon
    {
        public Transform Transform { get; }
        public ClampedIntegerBank Ammo { get; }

        public IReadOnlyReactiveProperty<float> ReloadProgress { get; }

        public IReadOnlyReactiveProperty<bool> IsReloading { get; }

        public void StartShooting();

        public void StopShooting();
    }
}