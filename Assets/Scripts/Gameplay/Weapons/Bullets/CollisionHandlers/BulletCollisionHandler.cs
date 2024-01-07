using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapons.Bullets.CollisionHandlers
{
    public class BulletCollisionHandler : IInitializable, IDisposable
    {
        private readonly GameObject _gameObject;
        private readonly Collider _collider;
        private readonly HitParticle _hitParticle;
        private readonly ImpulseTransmitter _impulseTransmitter;

        public BulletCollisionHandler(GameObject gameObject, Collider collider, HitParticle hitParticle, ImpulseTransmitter impulseTransmitter)
        {
            _gameObject = gameObject;
            _collider = collider;
            _hitParticle = hitParticle;
            _impulseTransmitter = impulseTransmitter;
        }

        private IDisposable _subscription;

        public void Initialize() => _subscription = _collider.OnCollisionEnterAsObservable().Subscribe(OnCollisionEnter);

        public void Dispose() => _subscription.Dispose();

        private void OnCollisionEnter(Collision collision)
        {
            _hitParticle.Spawn(collision);
            _impulseTransmitter.Transmit(collision);
            _gameObject.SetActive(false);
        }
    }
}