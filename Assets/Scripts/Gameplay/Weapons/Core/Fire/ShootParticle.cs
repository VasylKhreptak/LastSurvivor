using System;
using ObjectPoolSystem.PoolCategories;
using Plugins.ObjectPoolSystem;
using UnityEngine;
using Zenject.Infrastructure.Toggleable.Core;

namespace Gameplay.Weapons.Core.Fire
{
    public class ShootParticle
    {
        private readonly IObjectPools<Particle> _particlePools;
        private readonly Preferences _preferences;

        public ShootParticle(IObjectPools<Particle> particlePools, Preferences preferences)
        {
            _particlePools = particlePools;
            _preferences = preferences;
        }

        public void Spawn(Vector3 position, Vector3 direction)
        {
            GameObject particle = _particlePools.GetPool(_preferences.Particle).Get();
            particle.transform.position = position;
            particle.transform.forward = direction;
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private Particle _particle = Particle.Shoot;

            public Particle Particle => _particle;
        }
    }
}