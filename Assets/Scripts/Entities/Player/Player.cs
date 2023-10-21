using UnityEngine;

namespace Entities.Player
{
    public class Player : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _transform;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Animator _animator;

        public Transform Transform => _transform;
        public Rigidbody Rigidbody => _rigidbody;
        public Animator Animator => _animator;

        #region MonoBehaviour

        private void OnValidate()
        {
            _transform ??= GetComponent<Transform>();
            _rigidbody ??= GetComponent<Rigidbody>();
            _animator ??= GetComponent<Animator>();
        }

        #endregion
    }
}