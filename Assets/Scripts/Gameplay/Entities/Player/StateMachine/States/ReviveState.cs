using Analytics;
using Firebase.Analytics;
using Gameplay.Entities.Health.Core;
using Gameplay.Entities.Player.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Pathfinding;
using UnityEngine;
using Utilities.PhysicsUtilities;
using Zenject;

namespace Gameplay.Entities.Player.StateMachine.States
{
    public class ReviveState : IPlayerState, IState
    {
        private readonly MonoKernel _kernel;
        private readonly Animator _animator;
        private readonly IAstarAI _ai;
        private readonly Ragdoll _ragdoll;
        private readonly Collider _collider;
        private readonly IStateMachine<IPlayerState> _playerStateMachine;
        private readonly Rigidbody _rigidbody;
        private readonly IHealth _health;

        public ReviveState(MonoKernel kernel, Animator animator, IAstarAI ai,
            Ragdoll ragdoll, Collider collider, IStateMachine<IPlayerState> playerStateMachine, Rigidbody rigidbody, IHealth health)
        {
            _kernel = kernel;
            _animator = animator;
            _ai = ai;
            _ragdoll = ragdoll;
            _collider = collider;
            _playerStateMachine = playerStateMachine;
            _rigidbody = rigidbody;
            _health = health;
        }

        public void Enter()
        {
            _kernel.enabled = true;
            _animator.enabled = true;
            _ai.isStopped = false;
            _ai.canMove = true;
            _rigidbody.isKinematic = false;
            _collider.enabled = true;
            _ragdoll.Disable();
            _health.Restore();
            FirebaseAnalytics.LogEvent(AnalyticEvents.PlayerRevived);
            _playerStateMachine.Enter<IdleState>();
        }
    }
}