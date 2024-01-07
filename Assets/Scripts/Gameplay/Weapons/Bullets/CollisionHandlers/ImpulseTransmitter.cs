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

        public void Transmit(Collision collision)
        {
            if (collision.rigidbody == null)
                return;

            Vector3 impulse = collision.impulse;

            Debug.DrawRay(collision.contacts[0].point, impulse, Color.red, 5f);

            collision.rigidbody.AddForceAtPosition(-impulse * _preferences.ImpulseMultiplier, collision.contacts[0].point,
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