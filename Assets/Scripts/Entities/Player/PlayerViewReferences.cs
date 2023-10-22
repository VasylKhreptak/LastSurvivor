using System;
using UnityEngine;

namespace Entities.Player
{
    [Serializable]
    public class PlayerViewReferences
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Animator _animator;

        public Transform Transform => _transform;
        public Rigidbody Rigidbody => _rigidbody;
        public Animator Animator => _animator;
    }
}