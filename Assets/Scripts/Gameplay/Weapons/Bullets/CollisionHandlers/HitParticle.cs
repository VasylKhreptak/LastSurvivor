using System;
using ObjectPoolSystem.PoolCategories;
using Plugins.ObjectPoolSystem;
using Terrain.Surfaces.Core;
using UnityEngine;
using UnityEngine.Rendering;

namespace Gameplay.Weapons.Bullets.CollisionHandlers
{
    public class HitParticle
    {
        private readonly Preferences _preferences;
        private readonly IObjectPools<Particle> _particlePools;

        public HitParticle(Preferences preferences, IObjectPools<Particle> particlePools)
        {
            _preferences = preferences;
            _particlePools = particlePools;
        }

        public void Spawn(Collision collision)
        {
            ISurface surface = collision.gameObject.GetComponent<ISurface>();

            if (surface == null)
                return;

            if (_preferences.TryGetParticle(surface.Type, out Particle particle) == false)
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
            [SerializeField] private SerializedDictionary<SurfaceType, Particle> _surfaceParticleMap;

            public bool TryGetParticle(SurfaceType surfaceType, out Particle particle) =>
                _surfaceParticleMap.TryGetValue(surfaceType, out particle);
        }
    }
}