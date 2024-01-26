using Gameplay.Entities.Health;
using Gameplay.Entities.Health.Core;
using UnityEngine;
using UnityEngine.AI;
using Utilities.PhysicsUtilities;
using Zenject;

namespace Gameplay.Entities.Player
{
    public class PlayerDeathHandler : DeathHandler
    {
        private readonly MonoKernel _kernel;
        private readonly PlayerHolder _playerHolder;
        private readonly Animator _animator;
        private readonly NavMeshAgent _agent;
        private readonly Ragdoll _ragdoll;
        private readonly Collider _collider;

        public PlayerDeathHandler(MonoKernel kernel, PlayerHolder playerHolder, Animator animator, NavMeshAgent agent,
            IHealth health, Ragdoll ragdoll, Collider collider) : base(health)
        {
            _kernel = kernel;
            _playerHolder = playerHolder;
            _animator = animator;
            _agent = agent;
            _ragdoll = ragdoll;
            _collider = collider;
        }

        protected override void OnDied()
        {
            Object.Destroy(_kernel);
            _playerHolder.Instance = null;
            _animator.enabled = false;
            _agent.enabled = false;
            _collider.enabled = false;
            _ragdoll.Enable();
        }
    }
}