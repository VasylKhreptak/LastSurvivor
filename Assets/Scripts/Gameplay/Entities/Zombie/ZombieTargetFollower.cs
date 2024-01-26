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

namespace Gameplay.Entities.Zombie
{
    public class ZombieTargetFollower : IDisposable
    {
        private readonly Transform _transform;
        private readonly IStateMachine<IZombieState> _stateMachine;
        private readonly Preferences _preferences;
        private readonly TriggerZone<IVisitable<ZombieDamage>> _targetsZone;

        public ZombieTargetFollower(Transform transform, IStateMachine<IZombieState> stateMachine, Preferences preferences,
            TriggerZone<IVisitable<ZombieDamage>> targetsZone)
        {
            _transform = transform;
            _stateMachine = stateMachine;
            _preferences = preferences;
            _targetsZone = targetsZone;
        }

        private IDisposable _intervalSubscription;

        public void Dispose() => Stop();

        public void Start() => StartInterval();

        public void Stop()
        {
            StopInterval();
            _stateMachine.Enter<IdleState>();
        }

        private void StartInterval()
        {
            StopInterval();
            _intervalSubscription = Observable
                .Interval(TimeSpan.FromSeconds(_preferences.UpdateInterval))
                .DoOnSubscribe(OnIntervalCallback)
                .Subscribe(_ => OnIntervalCallback());
        }

        private void StopInterval() => _intervalSubscription?.Dispose();

        private void OnIntervalCallback() => UpdateState();

        private void UpdateState()
        {
            if (_targetsZone.Triggers.Count == 0)
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

            foreach (TriggerInfo<IVisitable<ZombieDamage>> triggerInfo in _targetsZone.Triggers)
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
            [Header("Preferences")]
            [SerializeField] private float _updateInterval = 0.3f;

            public float UpdateInterval => _updateInterval;
        }
    }
}