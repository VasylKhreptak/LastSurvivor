using Audio.Players;
using Gameplay.Entities.Explosive.Core;
using Gameplay.Entities.Health;
using Gameplay.Entities.Health.Core;
using Infrastructure.Services.Vibration.Core;
using Lofelt.NiceVibrations;
using ObjectPoolSystem.PoolCategories;
using Plugins.ObjectPoolSystem;
using UnityEngine;
using Utilities.CameraUtilities.Shaker;

namespace Gameplay.Entities.Explosive.Barrel
{
    public class BarrelExploder : DeathHandler
    {
        private readonly GameObject _gameObject;
        private readonly ExplosionRigidbodyAffector _rigidbodyAffector;
        private readonly CameraShaker _cameraShaker;
        private readonly IObjectPools<Particle> _particlePools;
        private readonly ExplosionDamageApplier _explosionDamageApplier;
        private readonly AudioPlayer _explosionAudioPlayer;
        private readonly IVibrationService _vibrationService;

        public BarrelExploder(IHealth health, GameObject gameObject, ExplosionRigidbodyAffector rigidbodyAffector,
            CameraShaker cameraShaker, IObjectPools<Particle> particlePools, ExplosionDamageApplier explosionDamageApplier,
            AudioPlayer explosionAudioPlayer, IVibrationService vibrationService) : base(health)
        {
            _gameObject = gameObject;
            _rigidbodyAffector = rigidbodyAffector;
            _cameraShaker = cameraShaker;
            _particlePools = particlePools;
            _explosionDamageApplier = explosionDamageApplier;
            _explosionAudioPlayer = explosionAudioPlayer;
            _vibrationService = vibrationService;
        }

        protected override void OnDied()
        {
            Vector3 position = _gameObject.transform.position;

            _explosionDamageApplier.Apply(position);
            _rigidbodyAffector.Affect(position);
            _cameraShaker.DoExplosionShake();
            _explosionAudioPlayer.Play(position);
            SpawnExplosionParticle();
            _vibrationService.Vibrate(HapticPatterns.PresetType.HeavyImpact);
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