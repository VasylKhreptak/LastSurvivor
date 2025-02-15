﻿using Analytics;
using Firebase.Analytics;
using Gameplay.Entities.Player.StateMachine.States.Core;
using Gameplay.Levels.StateMachine.States;
using Gameplay.Levels.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Pathfinding;
using UnityEngine;
using Utilities.PhysicsUtilities;
using Zenject;

namespace Gameplay.Entities.Player.StateMachine.States
{
    public class DeathState : IPlayerState, IState
    {
        private readonly MonoKernel _kernel;
        private readonly Animator _animator;
        private readonly IAstarAI _ai;
        private readonly Ragdoll _ragdoll;
        private readonly Collider _collider;
        private readonly IStateMachine<ILevelState> _levelStateMachine;
        private readonly Rigidbody _rigidbody;

        public DeathState(MonoKernel kernel, Animator animator, IAstarAI ai,
            Ragdoll ragdoll, Collider collider, IStateMachine<ILevelState> levelStateMachine, Rigidbody rigidbody)
        {
            _kernel = kernel;
            _animator = animator;
            _ai = ai;
            _ragdoll = ragdoll;
            _collider = collider;
            _levelStateMachine = levelStateMachine;
            _rigidbody = rigidbody;
        }

        public void Enter()
        {
            _kernel.enabled = false;
            _animator.enabled = false;
            _ai.isStopped = true;
            _ai.canMove = false;
            _rigidbody.isKinematic = true;
            _collider.enabled = false;
            _ragdoll.Enable();
            FirebaseAnalytics.LogEvent(AnalyticEvents.PlayerDied);
            _levelStateMachine.Enter<LevelFailedState>();
        }
    }
}