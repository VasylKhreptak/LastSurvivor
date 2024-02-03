using System;
using System.Linq;
using Extensions;
using UnityEngine;

namespace Gameplay.Entities.Explosive.Core
{
    public class ExplosionRigidbodyAffector
    {
        private readonly Preferences _preferences;

        public ExplosionRigidbodyAffector(Preferences preferences)
        {
            _preferences = preferences;
        }

        public void Affect(Vector3 position)
        {
            Collider[] colliders = Physics.OverlapSphere(position, _preferences.MaxRadius, _preferences.LayerMask);

            for (int i = 0; i < colliders.Length; i++)
            {
                Rigidbody rigidbody = colliders[i].attachedRigidbody;

                if (rigidbody == null)
                    continue;

                AffectRigidbody(rigidbody, position);
            }
        }

        private void AffectRigidbody(Rigidbody rigidbody, Vector3 position)
        {
            Vector3 explosionPosition = position - Vector3.up * _preferences.UpwardsModifier;

            float distance = Vector3.Distance(rigidbody.position, explosionPosition);

            float impulse = _preferences.ImpulseCurve.Evaluate(_preferences.MinRadius, _preferences.MaxRadius, distance,
                _preferences.MaxImpulse, _preferences.MinImpulse);

            Vector3 direction = (rigidbody.position - explosionPosition).normalized;

            rigidbody.AddForce(direction * impulse, ForceMode.Impulse);
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private LayerMask _layerMask;
            [SerializeField] private float _upwardsModifier = 1f;
            [SerializeField] private float _minRadius = 2f;
            [SerializeField] private float _maxRadius = 20f;
            [SerializeField] private float _minImpulse = 10f;
            [SerializeField] private float _maxImpulse = 30f;
            [SerializeField] private AnimationCurve _impulseCurve;

            public LayerMask LayerMask => _layerMask;
            public float UpwardsModifier => _upwardsModifier;
            public float MinRadius => _minRadius;
            public float MaxRadius => _maxRadius;
            public float MinImpulse => _minImpulse;
            public float MaxImpulse => _maxImpulse;
            public AnimationCurve ImpulseCurve => _impulseCurve;
        }
    }
}