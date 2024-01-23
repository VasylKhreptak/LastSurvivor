using Audio.Players;
using Gameplay.Entities.Explosive.Core;
using Gameplay.Entities.Health;
using Gameplay.Entities.Health.Core;
using ObjectPoolSystem.PoolCategories;
using Plugins.ObjectPoolSystem;
using UnityEngine;
using Utilities.CameraUtilities.Shaker;

namespace Gameplay.Entities.Explosive.Barrel.DeathHandlers.Core
{
    public class BarrelExploder : DeathHandler
    {
        private readonly GameObject _gameObject;
        private readonly ExplosionRigidbodyAffector _rigidbodyAffector;
        private readonly CameraShaker _cameraShaker;
        private readonly IObjectPools<Particle> _particlePools;
        private readonly ExplosionDamageApplier _explosionDamageApplier;
        private readonly AudioPlayer _explosionAudioPlayer;

        public BarrelExploder(IHealth health, GameObject gameObject, ExplosionRigidbodyAffector rigidbodyAffector,
            CameraShaker cameraShaker, IObjectPools<Particle> particlePools, ExplosionDamageApplier explosionDamageApplier,
            AudioPlayer explosionAudioPlayer) : base(health)
        {
            _gameObject = gameObject;
            _rigidbodyAffector = rigidbodyAffector;
            _cameraShaker = cameraShaker;
            _particlePools = particlePools;
            _explosionDamageApplier = explosionDamageApplier;
            _explosionAudioPlayer = explosionAudioPlayer;
        }

        protected override void OnDied()
        {
            Vector3 position = _gameObject.transform.position;

            _explosionDamageApplier.Apply(position);
            _rigidbodyAffector.Affect(position);
            _cameraShaker.DoExplosionShake();
            _explosionAudioPlayer.Play(position);
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