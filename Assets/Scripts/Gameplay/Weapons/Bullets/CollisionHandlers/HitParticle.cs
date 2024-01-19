using System;
using System.Collections.Generic;
using ObjectPoolSystem.PoolCategories;
using Plugins.ObjectPoolSystem;
using Serialization.Collections.KeyValue;
using Terrain.Surfaces.Core;
using UnityEngine;

namespace Gameplay.Weapons.Bullets.CollisionHandlers
{
    public class HitParticle
    {
        private readonly Preferences _preferences;
        private readonly IObjectPools<Particle> _particlePools;

        private readonly Dictionary<SurfaceType, Particle> _surfaceParticleMap;

        public HitParticle(Preferences preferences, IObjectPools<Particle> particlePools)
        {
            _preferences = preferences;
            _particlePools = particlePools;

            _surfaceParticleMap = _preferences.SurfaceParticlePairs.ToDictionary();
        }

        public void Spawn(Collision collision)
        {
            ISurface surface = collision.gameObject.GetComponent<ISurface>();

            if (surface == null)
                return;

            if (_surfaceParticleMap.TryGetValue(surface.Type, out Particle particle) == false)
                return;

            Spawn(collision, particle);
        }

        private void Spawn(Collision collision, Particle particle)
        {
            GameObject particleObject = _particlePools.GetPool(particle).Get();

            Vector3 hitNormal = collision.contacts[0].normal;

            particleObject.transform.position = collision.contacts[0].point;
            particleObject.transform.forward = hitNormal;
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private KeyValuePairs<SurfaceType, Particle> _surfaceParticlePairs;

            public KeyValuePairs<SurfaceType, Particle> SurfaceParticlePairs => _surfaceParticlePairs;
        }
    }
}