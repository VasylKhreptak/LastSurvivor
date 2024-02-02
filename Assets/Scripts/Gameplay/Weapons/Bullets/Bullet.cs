using Gameplay.Entities.Health.Damages;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapons.Bullets
{
    public class Bullet : MonoBehaviour
    {
        [Inject]
        private void Constructor(Transform transform, Rigidbody rigidbody, BulletDamage damage)
        {
            Transform = transform;
            Rigidbody = rigidbody;
            Damage = damage;
        }

        public Transform Transform { get; private set; }

        public Rigidbody Rigidbody { get; private set; }

        public BulletDamage Damage { get; private set; }
    }
}