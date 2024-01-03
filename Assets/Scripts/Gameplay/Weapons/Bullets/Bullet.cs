using Gameplay.Weapons.Bullets.Core;
using UnityEngine;

namespace Gameplay.Weapons.Bullets
{
    public class Bullet : MonoBehaviour, IBullet
    {
        [Header("References")]
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField, Min(0)] private float _damage;

        public Rigidbody Rigidbody => _rigidbody;

        public float Damage => _damage;
    }
}