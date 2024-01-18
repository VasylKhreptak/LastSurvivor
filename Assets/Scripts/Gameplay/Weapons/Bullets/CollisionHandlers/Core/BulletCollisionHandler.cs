using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapons.Bullets.CollisionHandlers.Core
{
    public class BulletCollisionHandler : IInitializable, IDisposable
    {
        private readonly GameObject _gameObject;
        private readonly Collider _collider;
        private readonly HitParticle _hitParticle;
        private readonly ImpulseTransmitter _impulseTransmitter;
        private readonly DamageApplier _damageApplier;
        private readonly HitAudioPlayer _hitAudioPlayer;

        public BulletCollisionHandler(GameObject gameObject, Collider collider, HitParticle hitParticle,
            ImpulseTransmitter impulseTransmitter, DamageApplier damageApplier, HitAudioPlayer hitAudioPlayer)
        {
            _gameObject = gameObject;
            _collider = collider;
            _hitParticle = hitParticle;
            _impulseTransmitter = impulseTransmitter;
            _damageApplier = damageApplier;
            _hitAudioPlayer = hitAudioPlayer;
        }

        private IDisposable _subscription;

        public void Initialize() => _subscription = _collider.OnCollisionEnterAsObservable().Subscribe(OnCollisionEnter);

        public void Dispose() => _subscription.Dispose();

        private void OnCollisionEnter(Collision collision)
        {
            _hitParticle.Spawn(collision);
            _impulseTransmitter.TryTransmit(collision);
            _hitAudioPlayer.Play(collision);
            _damageApplier.TryApply(collision.gameObject);
            _gameObject.SetActive(false);
        }
    }
}