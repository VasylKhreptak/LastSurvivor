using System;
using Gameplay.Entities.Health.Core;
using Gameplay.Entities.Health.Damages;
using Pathfinding;
using Plugins.Animations;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using Visitor;

namespace Entities.AI
{
    public class MeleeAttacker : IInitializable, IDisposable
    {
        private readonly Rigidbody _rigidbody;
        private readonly AgentMover _agentMover;
        private readonly Animator _animator;
        private readonly IAstarAI _ai;
        private readonly Preferences _preferences;

        public MeleeAttacker(Rigidbody rigidbody, AgentMover agentMover, Animator animator, IAstarAI ai, Preferences preferences)
        {
            _rigidbody = rigidbody;
            _agentMover = agentMover;
            _animator = animator;
            _ai = ai;
            _preferences = preferences;
        }

        private IDisposable _lookSubscription;

        private Transform _target;
        private IHealth _health;
        private IVisitable<MeleeDamage> _visitableTarget;

        private bool _active;

        public void Start(Vector3 position, Transform target, IHealth health, IVisitable<MeleeDamage> visitableTarget)
        {
            Stop();

            _target = target;
            _health = health;
            _visitableTarget = visitableTarget;

            _active = true;

            _agentMover.SetDestination(position, () =>
            {
                _ai.canMove = false;
                _preferences.Weapon.SetActive(true);
                _preferences.WeaponShowAnimation.PlayForward();
                StartLooking(_target);
                StartAttacking();
            });
        }

        public void Stop()
        {
            _active = false;
            StopLooking();
            _agentMover.Stop();
            StopAttacking();
            _ai.canMove = true;
            _preferences.WeaponShowAnimation.PlayBackward(() => _preferences.Weapon.SetActive(false));
        }

        public void Initialize()
        {
            _preferences.Weapon.SetActive(false);
            _preferences.WeaponShowAnimation.SetStartState();
        }

        public void Dispose() => Stop();

        private void StartLooking(Transform target)
        {
            StopLooking();
            _lookSubscription = Observable
                .EveryUpdate()
                .Subscribe(_ =>
                {
                    Vector3 targetPosition = target.position;
                    Vector3 currentPosition = _rigidbody.position;
                    targetPosition.y = currentPosition.y;
                    Vector3 direction = targetPosition - currentPosition;
                    Quaternion rotation = Quaternion.LookRotation(direction);
                    _rigidbody.rotation = Quaternion.Lerp(_rigidbody.rotation, rotation, Time.deltaTime * _preferences.LookSpeed);
                });
        }

        private void StopLooking() => _lookSubscription?.Dispose();

        private void StartAttacking() => _animator.SetBool(_preferences.IsAttackingProperty, true);

        private void StopAttacking() => _animator.SetBool(_preferences.IsAttackingProperty, false);

        public void ApplyDamage()
        {
            if (_active == false)
                return;

            _visitableTarget.Accept(new MeleeDamage(_preferences.Damage));

            if (_health.IsDeath.Value)
                Stop();
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private GameObject _weapon;
            [SerializeField] private ScaleAnimation _weaponShowAnimation;
            [SerializeField] private string _isAttackingProperty = "IsAttacking";
            [SerializeField] private float _damage = 20f;
            [SerializeField] private float _lookSpeed = 10f;

            public GameObject Weapon => _weapon;
            public ScaleAnimation WeaponShowAnimation => _weaponShowAnimation;
            public string IsAttackingProperty => _isAttackingProperty;
            public float Damage => _damage;
            public float LookSpeed => _lookSpeed;
        }
    }
}