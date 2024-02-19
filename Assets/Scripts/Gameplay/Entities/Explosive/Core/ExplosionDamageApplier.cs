using System;
using Extensions;
using Gameplay.Entities.Health.Damages;
using Inspector.MinMax;
using UnityEngine;
using Visitor;

namespace Gameplay.Entities.Explosive.Core
{
    public class ExplosionDamageApplier
    {
        private readonly IVisitable<ExplosionDamage> _visitable;
        private readonly Preferences _preferences;

        public ExplosionDamageApplier(IVisitable<ExplosionDamage> visitable, Preferences preferences)
        {
            _visitable = visitable;
            _preferences = preferences;
        }

        public void Apply(Vector3 position)
        {
            Collider[] colliders = Physics.OverlapSphere(position, _preferences.Radius.Max, _preferences.LayerMask);

            for (int i = 0; i < colliders.Length; i++)
            {
                GameObject gameObject = colliders[i].gameObject;

                if (gameObject.TryGetComponent(out IVisitable<ExplosionDamage> visitable) == false)
                    continue;

                if (_visitable == visitable)
                    continue;

                float distance = Vector3.Distance(position, gameObject.transform.position);

                float damage = _preferences.DamageCurve.Evaluate(_preferences.Radius.Min, _preferences.Radius.Max, distance,
                    _preferences.Damage.Max, _preferences.Damage.Min);

                ExplosionDamage explosionDamage = new ExplosionDamage(damage);

                visitable.Accept(explosionDamage);
            }
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private LayerMask _layerMask;
            [SerializeField] private FloatMinMaxValue _radius = new FloatMinMaxValue(0f, 10f);
            [SerializeField] private FloatMinMaxValue _damage = new FloatMinMaxValue(10f, 300f);
            [SerializeField] private AnimationCurve _damageCurve;

            public LayerMask LayerMask => _layerMask;
            public FloatMinMaxValue Radius => _radius;
            public FloatMinMaxValue Damage => _damage;
            public AnimationCurve DamageCurve => _damageCurve;
        }
    }
}