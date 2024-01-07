using CameraUtilities.Shaker;
using Entities.Core;
using Entities.Core.Health.Core;
using Gameplay.Entities.Explosive.Core;
using ObjectPoolSystem.PoolCategories;
using Plugins.ObjectPoolSystem;
using UnityEngine;

namespace Gameplay.Entities.Explosive.Barrel.DeathHandlers.Core
{
    public class ExplosiveBarrelDeathHandler : DeathHandler
    {
        private readonly GameObject _gameObject;
        private ExplosionRigidbodyAffector _rigidbodyAffector;
        private CameraShaker _cameraShaker;
        private IObjectPools<Particle> _particlePools;

        public ExplosiveBarrelDeathHandler(IHealth health, GameObject gameObject, ExplosionRigidbodyAffector rigidbodyAffector,
            CameraShaker cameraShaker, IObjectPools<Particle> particlePools) : base(health)
        {
            _gameObject = gameObject;
            _rigidbodyAffector = rigidbodyAffector;
            _cameraShaker = cameraShaker;
            _particlePools = particlePools;
        }

        protected override void OnDied()
        {
            _rigidbodyAffector.Affect(_gameObject.transform.position);
            _cameraShaker.DoExplosionShake();
            SpawnExplosionParticle();
            Object.Destroy(_gameObject);
        }

        private void SpawnExplosionParticle()
        {
            GameObject particle = _particlePools.GetPool(Particle.BarrelExplosion).Get();
            particle.transform.position = _gameObject.transform.position;
            particle.transform.up = Vector3.up;
        }
    }
}