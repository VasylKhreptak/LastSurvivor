using System;
using Extensions;
using Gameplay.Entities.Explosive.Data;
using UnityEngine;
using Visitor;

namespace Gameplay.Entities.Explosive.Core
{
    public class ExplosionDamageApplier
    {
        private readonly Preferences _preferences;

        public ExplosionDamageApplier(Preferences preferences)
        {
            _preferences = preferences;
        }

        public void Apply(Vector3 position)
        {
            Collider[] colliders = Physics.OverlapSphere(position, _preferences.MaxRadius, _preferences.LayerMask);

            for (int i = 0; i < colliders.Length; i++)
            {
                GameObject gameObject = colliders[i].gameObject;

                if (gameObject.TryGetComponent(out IVisitable<ExplosionDamage> visitable) == false)
                    continue;

                float distance = Vector3.Distance(position, gameObject.transform.position);

                float damage = _preferences.DamageCurve.Evaluate(_preferences.MinRadius, _preferences.MaxRadius, distance,
                    _preferences.MaxDamage, _preferences.MinDamage);

                ExplosionDamage explosionDamage = new ExplosionDamage(damage);

                visitable.Accept(explosionDamage);
            }
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private LayerMask _layerMask;
            [SerializeField] private float _minRadius = 0f;
            [SerializeField] private float _maxRadius = 15f;
            [SerializeField] private float _minDamage = 10f;
            [SerializeField] private float _maxDamage = 300f;
            [SerializeField] private AnimationCurve _damageCurve;

            public LayerMask LayerMask => _layerMask;
            public float MinRadius => _minRadius;
            public float MaxRadius => _maxRadius;
            public float MinDamage => _minDamage;
            public float MaxDamage => _maxDamage;
            public AnimationCurve DamageCurve => _damageCurve;
        }
    }
}