using System;
using Gameplay.Entities.Health.Damages;
using UniRx;
using UnityEngine;
using Visitor;

namespace Entities.AI
{
    public class MeleeAttacker : IDisposable
    {
        private readonly Transform _transform;
        private readonly AgentMover _agentMover;
        private readonly Animator _animator;
        private readonly Preferences _preferences;

        public MeleeAttacker(Transform transform, AgentMover agentMover, Animator animator, Preferences preferences)
        {
            _transform = transform;
            _agentMover = agentMover;
            _animator = animator;
            _preferences = preferences;
        }

        private IDisposable _lookSubscription;
        private IDisposable _attackSubscription;
        private IDisposable _damageApplySubscription;

        private Transform _target;
        private IVisitable<MeleeDamage> _visitableTarget;

        public void Start(Transform target, IVisitable<MeleeDamage> visitableTarget)
        {
            Stop();

            _target = target;
            _visitableTarget = visitableTarget;

            _agentMover.SetDestination(target.position, true, () => LookAt(_target,  StartAttacking));
        }

        public void Stop()
        {
            _lookSubscription?.Dispose();
            _attackSubscription?.Dispose();
            _damageApplySubscription?.Dispose();
            _animator.ResetTrigger(_preferences.AttackTrigger);
        }

        public void Dispose() => Stop();
        
        private void LookAt(Transform target, Action onComplete)
        {
            _lookSubscription?.Dispose();
            _lookSubscription = Observable
                .EveryUpdate()
                .Subscribe(_ =>
                {
                    if (target == null)
                    {
                        Stop();
                        return;
                    }

                    Vector3 targetPosition = target.position;
                    Vector3 currentPosition = _transform.position;
                    targetPosition.y = currentPosition.y;
                    Vector3 direction = (targetPosition - currentPosition);
                    Quaternion rotation = Quaternion.LookRotation(direction);
                    _transform.rotation = Quaternion.Lerp(_transform.rotation, rotation, Time.deltaTime * _preferences.LookSpeed);
                    
                    if (Quaternion.Angle(_transform.rotation, rotation) > 1f)
                        return;

                    Debug.Log("Dispose");
                    _lookSubscription?.Dispose();
                    onComplete?.Invoke();
                });
        }

        private void StartAttacking()
        {
            if (_target == null)
            {
                Stop();
                return;
            }

            _attackSubscription?.Dispose();
            _attackSubscription = Observable
                .Interval(TimeSpan.FromSeconds(_preferences.AttackInterval))
                .DoOnSubscribe(PlayAttackAnimation)
                .Subscribe(_ => PlayAttackAnimation());
        }

        private void PlayAttackAnimation()
        {
            _animator.SetTrigger(_preferences.AttackTrigger);

            _damageApplySubscription?.Dispose();
            _damageApplySubscription = Observable
                .Timer(TimeSpan.FromSeconds(_preferences.DamageApplyDelay))
                .Subscribe(_ => ApplyDamage());
        }

        private void ApplyDamage()
        {
            _visitableTarget.Accept(new MeleeDamage(_preferences.Damage));

            if (_visitableTarget == null)
                Stop();
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private string _attackTrigger = "Attack";
            [SerializeField] private float _damage = 20f;
            [SerializeField] private float _lookSpeed = 10f;
            [SerializeField] private float _attackInterval = 1f;
            [SerializeField] private float _damageApplyDelay = 0.5f;

            public string AttackTrigger => _attackTrigger;
            public float Damage => _damage;
            public float LookSpeed => _lookSpeed;
            public float AttackInterval => _attackInterval;
            public float DamageApplyDelay => _damageApplyDelay;
        }
    }
}