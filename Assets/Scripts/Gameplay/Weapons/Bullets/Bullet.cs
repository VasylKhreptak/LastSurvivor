using Gameplay.Entities.Health.Damages;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapons.Bullets
{
    public class Bullet : MonoBehaviour
    {
        private Transform _transform;
        private Rigidbody _rigidbody;
        private BulletDamage _damage;

        [Inject]
        private void Constructor(Transform transform, Rigidbody rigidbody, BulletDamage damage)
        {
            _transform = transform;
            _rigidbody = rigidbody;
            _damage = damage;
        }

        public Transform Transform => _transform;
        public Rigidbody Rigidbody => _rigidbody;
        public BulletDamage Damage => _damage;
    }
}