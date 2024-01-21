using Gameplay.Entities.Health;
using Gameplay.Entities.Health.Damages;
using Gameplay.Entities.Health.Damages.Core;
using Gameplay.Weapons.Bullets.Core;
using UnityEngine;

namespace Gameplay.Weapons.Bullets
{
    public class Bullet : MonoBehaviour, IBullet
    {
        [Header("References")]
        [SerializeField] private Transform _transform;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _defaultDamage = 10f;

        #region MonoBehaviour

        private void Awake() => Damage = new BulletDamage(_defaultDamage);

        private void OnValidate()
        {
            _transform ??= GetComponent<Transform>();
            _rigidbody ??= GetComponent<Rigidbody>();
        }

        #endregion

        public Transform Transform => _transform;
        public Rigidbody Rigidbody => _rigidbody;
        public BulletDamage Damage { get; private set; }
    }
}