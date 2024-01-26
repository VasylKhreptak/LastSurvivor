using Plugins.Banks;
using UnityEngine;

namespace Gameplay.Weapons.Core
{
    public interface IWeapon
    {
        public Transform Transform { get; }
        public ClampedIntegerBank Ammo { get; }

        public void StartShooting();

        public void StopShooting();
    }
}