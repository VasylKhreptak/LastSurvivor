using System;
using Plugins.ObjectPoolSystem;
using Plugins.ObjectPoolSystem.Test;
using Terrain.Surfaces.Core;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace Gameplay.Weapons.Bullets.CollisionHandlers
{
    public class HitParticle
    {
        private readonly Preferences _preferences;
        private readonly IObjectPools<MainPool> _objectPools;

        public HitParticle(Preferences preferences)
        {
            _preferences = preferences;
        }

        public void Play(Collision collision)
        {
            ISurface surface = collision.gameObject.GetComponent<ISurface>();

            if (surface == null)
                return;

            MainPool particle = _preferences.GetParticle(surface.Type);

            GameObject particleObject = _objectPools.GetPool(particle).Get();

            Vector3 hitNormal = collision.contacts[0].normal;

            particleObject.transform.position = collision.contacts[0].point;
            particleObject.transform.forward = hitNormal;
            particleObject.transform.rotation *= Quaternion.AngleAxis(Random.Range(0, 360), hitNormal);
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private SerializedDictionary<SurfaceType, MainPool> _surfaceParticleMap;

            public MainPool GetParticle(SurfaceType surfaceType) => _surfaceParticleMap[surfaceType];
        }
    }
}