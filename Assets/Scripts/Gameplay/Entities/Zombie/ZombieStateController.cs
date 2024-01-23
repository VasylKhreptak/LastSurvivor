using System;
using Entities.StateMachine.States;
using Gameplay.Entities.Health.Damages;
using Gameplay.Entities.Zombie.StateMachine.States;
using Gameplay.Entities.Zombie.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using UniRx;
using UnityEngine;
using Utilities.PhysicsUtilities;
using Visitor;
using Zenject;

namespace Gameplay.Entities.Zombie
{
    public class ZombieStateController : IInitializable, IDisposable
    {
        private readonly Transform _transform;
        private readonly IStateMachine<IZombieState> _stateMachine;
        private readonly Preferences _preferences;

        private readonly TriggerZone<IVisitable<ZombieDamage>> _triggerZone;

        public ZombieStateController(Transform transform, IStateMachine<IZombieState> stateMachine, Preferences preferences)
        {
            _transform = transform;
            _stateMachine = stateMachine;
            _preferences = preferences;

            _triggerZone = new TriggerZone<IVisitable<ZombieDamage>>(_preferences.Trigger);
        }

        private IDisposable _intervalSubscription;

        public void Initialize()
        {
            _triggerZone.Initialize();
            StartInterval();
        }

        public void Dispose()
        {
            _triggerZone.Dispose();
            StopInterval();
        }

        private void StartInterval()
        {
            _intervalSubscription = Observable
                .Interval(TimeSpan.FromSeconds(_preferences.UpdateInterval))
                .Subscribe(_ => OnIntervalCallback());
        }

        private void StopInterval() => _intervalSubscription?.Dispose();

        private void OnIntervalCallback() => UpdateState();

        private void UpdateState()
        {
            if (_triggerZone.Triggers.Count == 0)
            {
                _stateMachine.Enter<IdleState>();

                return;
            }

            Transform closestTransform = GetClosestTransform();

            AgentMoveState.Payload payload = new AgentMoveState.Payload
            {
                Position = closestTransform.position
            };

            _stateMachine.Enter<MoveState, AgentMoveState.Payload>(payload);
        }

        private Transform GetClosestTransform()
        {
            Transform closestTransform = null;
            float closestDistance = float.MaxValue;

            foreach (TriggerInfo<IVisitable<ZombieDamage>> triggerInfo in _triggerZone.Triggers)
            {
                float distance = Vector3.Distance(triggerInfo.Transform.position, _transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTransform = triggerInfo.Transform;
                }
            }

            return closestTransform;
        }

        [Serializable]
        public class Preferences
        {
            [Header("References")]
            [SerializeField] private Collider _trigger;

            [Header("Preferences")]
            [SerializeField] private float _updateInterval = 0.3f;

            public Collider Trigger => _trigger;
            public float UpdateInterval => _updateInterval;
        }
    }
}