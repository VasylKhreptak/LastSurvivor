using System;
using Plugins.Banks;

namespace Gameplay.Weapons.Core
{
    public interface IWeapon
    {
        public ClampedIntegerBank Ammo { get; }

        public void StartShooting();

        public void StopShooting();
    }
}