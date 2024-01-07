using Gameplay.Weapons.Bullets.Core;
using UnityEngine;

namespace Gameplay.Weapons.Bullets
{
    public class Bullet : MonoBehaviour, IBullet
    {
        [Header("References")]
        [SerializeField] private Transform _transform;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] [Min(0)] private float _damage;

        #region MonoBehaviour

        private void OnValidate()
        {
            _transform ??= GetComponent<Transform>();
            _rigidbody ??= GetComponent<Rigidbody>();
        }

        #endregion

        public Transform Transform => _transform;
        public Rigidbody Rigidbody => _rigidbody;

        public float Damage
        {
            get => _damage;
            set => _damage = Mathf.Max(0, value);
        }
    }
}