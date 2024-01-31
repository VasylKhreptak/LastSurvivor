using System.Collections.Generic;
using Gameplay.Entities.Soldier.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.States.Core;
using UnityEngine;
using UnityEngine.AI;
using Utilities.PhysicsUtilities;
using Zenject;

namespace Gameplay.Entities.Soldier.StateMachine.States
{
    public class DeathState : ISoldierState, IState
    {
        private readonly MonoKernel _kernel;
        private readonly Ragdoll _ragdoll;
        private readonly Animator _animator;
        private readonly Collider _collider;
        private readonly NavMeshAgent _agent;

        public DeathState(MonoKernel kernel, Ragdoll ragdoll, Animator animator, Collider collider, NavMeshAgent agent)
        {
            _kernel = kernel;
            _ragdoll = ragdoll;
            _animator = animator;
            _collider = collider;
            _agent = agent;
        }

        public void Enter()
        {
            Object.Destroy(_kernel);
            _animator.enabled = false;
            _collider.enabled = false;
            _agent.enabled = false;
            _ragdoll.Enable();
        }
    }
}