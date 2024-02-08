using System;
using Extensions;
using Inspector.MinMax;
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
            Collider[] colliders = Physics.OverlapSphere(position, _preferences.Radius.Max, _preferences.LayerMask);

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

            float impulse = _preferences.ImpulseCurve.Evaluate(_preferences.Radius.Min, _preferences.Radius.Max, distance,
                _preferences.Impulse.Max, _preferences.Impulse.Min);

            Vector3 direction = (rigidbody.position - explosionPosition).normalized;

            rigidbody.AddForce(direction * impulse, ForceMode.Impulse);
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private LayerMask _layerMask;
            [SerializeField] private float _upwardsModifier = 1f;
            [SerializeField] private FloatMinMaxValue _radius = new FloatMinMaxValue(0f, 7f);
            [SerializeField] private FloatMinMaxValue _impulse = new FloatMinMaxValue(20f, 70f);
            [SerializeField] private AnimationCurve _impulseCurve;

            public LayerMask LayerMask => _layerMask;
            public float UpwardsModifier => _upwardsModifier;
            public FloatMinMaxValue Radius => _radius;
            public FloatMinMaxValue Impulse => _impulse;
            public AnimationCurve ImpulseCurve => _impulseCurve;
        }
    }
}