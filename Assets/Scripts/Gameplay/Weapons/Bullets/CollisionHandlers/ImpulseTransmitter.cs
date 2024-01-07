using System;
using UnityEngine;

namespace Gameplay.Weapons.Bullets.CollisionHandlers
{
    public class ImpulseTransmitter
    {
        private readonly Preferences _preferences;

        public ImpulseTransmitter(Preferences preferences)
        {
            _preferences = preferences;
        }

        private Vector3 _impulse;

        public void Transmit(Collision collision)
        {
            if (collision.rigidbody == null)
                return;

            _impulse = collision.impulse;

            collision.rigidbody.AddForceAtPosition(-_impulse * _preferences.ImpulseMultiplier, collision.contacts[0].point,
                ForceMode.Impulse);
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private float _impulseMultiplier = 5f;

            public float ImpulseMultiplier => _impulseMultiplier;
        }
    }
}