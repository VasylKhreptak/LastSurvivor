using System;
using Entities.AI;
using Gameplay.Entities.Health.Damages;
using Gameplay.Entities.Zombie.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.States.Core;
using UniRx;
using UnityEngine;
using Utilities.PhysicsUtilities.Trigger;
using Visitor;

namespace Gameplay.Entities.Zombie.StateMachine.States
{
    public class NavigationState : IZombieState, IState, IExitable
    {
        private readonly ClosestTriggerObserver<IVisitable<ZombieDamage>> _closestTargetObserver;
        private readonly AgentTransformFollower _transformFollower;
        private readonly ZombieAttacker _zombieAttacker;

        public NavigationState(ClosestTriggerObserver<IVisitable<ZombieDamage>> closestTargetObserver,
            AgentTransformFollower transformFollower, ZombieAttacker zombieAttacker)
        {
            _closestTargetObserver = closestTargetObserver;
            _transformFollower = transformFollower;
            _zombieAttacker = zombieAttacker;
        }

        private IDisposable _closestTargetSubscription;

        public void Enter()
        {
            _zombieAttacker.Start();
            StartObservingClosestTarget();
        }

        public void Exit()
        {
            _zombieAttacker.Stop();
            StopObservingClosestTarget();
            _transformFollower.Stop();
        }

        private void StartObservingClosestTarget()
        {
            StopObservingClosestTarget();
            _closestTargetSubscription = _closestTargetObserver.Trigger
                .Select(x => x?.Transform)
                .Subscribe(OnClosestTargetChanged);
        }

        private void StopObservingClosestTarget() => _closestTargetSubscription?.Dispose();

        private void OnClosestTargetChanged(Transform target)
        {
            if (target == null)
            {
                _transformFollower.Stop();
                return;
            }

            _transformFollower.Start(target);
        }
    }
}