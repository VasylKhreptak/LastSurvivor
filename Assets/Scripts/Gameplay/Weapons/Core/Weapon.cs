using System;
using Plugins.Banks;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapons.Core
{
    public class Weapon : MonoBehaviour, IWeapon
    {
        private IWeapon _weapon;

        [Inject]
        private void Constructor(IWeapon weapon)
        {
            _weapon = weapon;
        }

        public event Action<ShootData> OnShoot { add => _weapon.OnShoot += value; remove => _weapon.OnShoot -= value; }

        public ClampedIntegerBank Ammo => _weapon.Ammo;

        public void StartShooting() => _weapon.StartShooting();

        public void StopShooting() => _weapon.StopShooting();
    }
}