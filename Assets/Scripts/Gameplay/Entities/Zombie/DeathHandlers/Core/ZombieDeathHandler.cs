using Gameplay.Entities.Health;
using Gameplay.Entities.Health.Core;
using UnityEngine;
using UnityEngine.AI;
using Utilities.PhysicsUtilities;
using Zenject;

namespace Gameplay.Entities.Zombie.DeathHandlers.Core
{
    public class ZombieDeathHandler : DeathHandler
    {
        private readonly MonoKernel _kernel;
        private readonly Ragdoll _ragdoll;
        private readonly Animator _animator;
        private readonly Collider _collider;
        private readonly NavMeshAgent _agent;

        public ZombieDeathHandler(MonoKernel kernel, Ragdoll ragdoll, Animator animator, Collider collider, NavMeshAgent agent,
            IHealth health) :
            base(health)
        {
            _kernel = kernel;
            _ragdoll = ragdoll;
            _animator = animator;
            _collider = collider;
            _agent = agent;
        }

        protected override void OnDied()
        {
            Object.Destroy(_kernel);
            _animator.enabled = false;
            _collider.enabled = false;
            _agent.enabled = false;
            _ragdoll.Enable();
        }
    }
}