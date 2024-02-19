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
        private readonly Transform _transform;
        private readonly AgentMover _agentMover;
        private readonly Animator _animator;
        private readonly Preferences _preferences;
        private readonly IAstarAI _ai;

        public MeleeAttacker(Transform transform, AgentMover agentMover, Animator animator, Preferences preferences, IAstarAI ai)
        {
            _transform = transform;
            _agentMover = agentMover;
            _animator = animator;
            _preferences = preferences;
            _ai = ai;
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
                _preferences.Weapon.SetActive(true);
                _preferences.WeaponShowAnimation.PlayForward();
                StartLooking(_target);
                StartAttacking();
                _ai.canMove = false;
            });
        }

        public void Stop()
        {
            _active = false;
            _lookSubscription?.Dispose();
            _agentMover.Stop();
            StopAttacking();
            _preferences.WeaponShowAnimation.PlayBackward(() => _preferences.Weapon.SetActive(false));
            _ai.canMove = true;
        }

        public void Initialize()
        {
            _preferences.Weapon.SetActive(false);
            _preferences.WeaponShowAnimation.SetStartState();
        }

        public void Dispose() => Stop();

        private void StartLooking(Transform target)
        {
            _lookSubscription?.Dispose();
            _lookSubscription = Observable
                .EveryUpdate()
                .Subscribe(_ =>
                {
                    Vector3 targetPosition = target.position;
                    Vector3 currentPosition = _transform.position;
                    targetPosition.y = currentPosition.y;
                    Vector3 direction = targetPosition - currentPosition;
                    Quaternion rotation = Quaternion.LookRotation(direction);
                    _transform.rotation = Quaternion.Lerp(_transform.rotation, rotation, Time.deltaTime * _preferences.LookSpeed);
                });
        }

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