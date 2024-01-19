using Entities.Health;
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

        private void Awake() => Damage = new Damage(_defaultDamage);

        private void OnValidate()
        {
            _transform ??= GetComponent<Transform>();
            _rigidbody ??= GetComponent<Rigidbody>();
        }

        #endregion

        public Transform Transform => _transform;
        public Rigidbody Rigidbody => _rigidbody;
        public Damage Damage { get; private set; }
    }
}