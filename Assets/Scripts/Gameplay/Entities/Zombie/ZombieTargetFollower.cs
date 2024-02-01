using System;
using Gameplay.Entities.Health.Damages;
using Gameplay.Entities.Zombie.StateMachine.States;
using Gameplay.Entities.Zombie.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using UniRx;
using UnityEngine;
using Utilities.PhysicsUtilities.Trigger;
using Visitor;

namespace Gameplay.Entities.Zombie
{
    public class ZombieTargetFollower : IDisposable
    {
        private readonly ClosestTriggerObserver<IVisitable<ZombieDamage>> _closestTriggerObserver;
        private readonly IStateMachine<IZombieState> _stateMachine;

        public ZombieTargetFollower(ClosestTriggerObserver<IVisitable<ZombieDamage>> closestTriggerObserver,
            IStateMachine<IZombieState> stateMachine)
        {
            _closestTriggerObserver = closestTriggerObserver;
            _stateMachine = stateMachine;
        }

        private IDisposable _subscription;

        public void Dispose() => Stop();

        public void Start()
        {
            StartObserving();
        }

        public void Stop()
        {
            StopObserving();
            _stateMachine.Enter<IdleState>();
        }

        private void StartObserving()
        {
            _subscription = _closestTriggerObserver.Trigger
                .Subscribe(OnClosestTriggerChanged);
        }

        private void StopObserving() => _subscription?.Dispose();

        private void OnClosestTriggerChanged(TriggerInfo<IVisitable<ZombieDamage>> closestTrigger)
        {
            if (closestTrigger == null)
                _stateMachine.Enter<IdleState>();
            else
                _stateMachine.Enter<FollowTransformState, Transform>(closestTrigger.Transform);
        }
    }
}