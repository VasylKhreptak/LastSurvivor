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

        public ClampedIntegerBank Ammo => _weapon.Ammo;

        public void StartShooting() => _weapon.StartShooting();

        public void StopShooting() => _weapon.StopShooting();
    }
}