using System.Collections.Generic;
using Gameplay.Entities.Zombie.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.States.Core;
using UnityEngine;
using UnityEngine.AI;
using Utilities.PhysicsUtilities;
using Zenject;

namespace Gameplay.Entities.Zombie.StateMachine.States
{
    public class DeathState : IZombieState, IState
    {
        private readonly MonoKernel _kernel;
        private readonly Ragdoll _ragdoll;
        private readonly Animator _animator;
        private readonly Collider _collider;
        private readonly NavMeshAgent _agent;
        private readonly List<Zombie> _zombies;
        private readonly Zombie _zombie;

        public DeathState(MonoKernel kernel, Ragdoll ragdoll, Animator animator, Collider collider, NavMeshAgent agent,
            List<Zombie> zombies, Zombie zombie)
        {
            _kernel = kernel;
            _ragdoll = ragdoll;
            _animator = animator;
            _collider = collider;
            _agent = agent;
            _zombies = zombies;
            _zombie = zombie;
        }

        public void Enter()
        {
            _zombies.Remove(_zombie);
            Object.Destroy(_kernel);
            _animator.enabled = false;
            _collider.enabled = false;
            _agent.enabled = false;
            _ragdoll.Enable();
        }
    }
}