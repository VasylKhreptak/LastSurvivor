using System;
using ObjectPoolSystem.PoolCategories;
using Plugins.ObjectPoolSystem;
using UnityEngine;
using Zenject.Infrastructure.Toggleable.Core;

namespace Gameplay.Weapons.Core.Fire
{
    public class ShootParticle : IEnableable, IDisableable
    {
        private readonly IWeapon _weapon;
        private readonly IObjectPools<Particle> _particlePools;
        private readonly Preferences _preferences;

        public ShootParticle(IWeapon weapon, IObjectPools<Particle> particlePools, Preferences preferences)
        {
            _weapon = weapon;
            _particlePools = particlePools;
            _preferences = preferences;
        }

        public void Enable() => _weapon.OnShoot += SpawnParticle;

        public void Disable() => _weapon.OnShoot -= SpawnParticle;

        private void SpawnParticle(ShootData shootData)
        {
            GameObject particle = _particlePools.GetPool(_preferences.Particle).Get();
            particle.transform.position = shootData.Position;
            particle.transform.forward = shootData.Direction;
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private Particle _particle = Particle.Shoot;

            public Particle Particle => _particle;
        }
    }
}