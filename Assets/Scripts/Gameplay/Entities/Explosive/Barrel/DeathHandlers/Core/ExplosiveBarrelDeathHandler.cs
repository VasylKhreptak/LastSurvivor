using CameraUtilities.Shaker;
using Entities.Core;
using Entities.Core.Health.Core;
using Gameplay.Entities.Explosive.Core;
using UnityEngine;

namespace Gameplay.Entities.Explosive.Barrel.DeathHandlers.Core
{
    public class ExplosiveBarrelDeathHandler : DeathHandler
    {
        private readonly GameObject _gameObject;
        private ExplosionRigidbodyAffector _rigidbodyAffector;
        private CameraShaker _cameraShaker;

        public ExplosiveBarrelDeathHandler(IHealth health, GameObject gameObject, ExplosionRigidbodyAffector rigidbodyAffector,
            CameraShaker cameraShaker) : base(health)
        {
            _gameObject = gameObject;
            _rigidbodyAffector = rigidbodyAffector;
            _cameraShaker = cameraShaker;
        }

        protected override void OnDied()
        {
            _rigidbodyAffector.Affect(_gameObject.transform.position);
            _cameraShaker.DoExplosionShake();
            Object.Destroy(_gameObject);
        }
    }
}