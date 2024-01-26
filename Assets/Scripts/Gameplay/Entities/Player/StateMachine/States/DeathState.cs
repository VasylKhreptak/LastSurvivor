using Gameplay.Entities.Player.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Levels.StateMachine.States;
using Levels.StateMachine.States.Core;
using UnityEngine;
using UnityEngine.AI;
using Utilities.PhysicsUtilities;
using Zenject;

namespace Gameplay.Entities.Player.StateMachine.States
{
    public class DeathState : IPlayerState, IState
    {
        private readonly MonoKernel _kernel;
        private readonly PlayerHolder _playerHolder;
        private readonly Animator _animator;
        private readonly NavMeshAgent _agent;
        private readonly Ragdoll _ragdoll;
        private readonly Collider _collider;
        private readonly IStateMachine<ILevelState> _levelStateMachine;

        public DeathState(MonoKernel kernel, PlayerHolder playerHolder, Animator animator, NavMeshAgent agent,
            Ragdoll ragdoll, Collider collider, IStateMachine<ILevelState> levelStateMachine)
        {
            _kernel = kernel;
            _playerHolder = playerHolder;
            _animator = animator;
            _agent = agent;
            _ragdoll = ragdoll;
            _collider = collider;
            _levelStateMachine = levelStateMachine;
        }

        public void Enter()
        {
            _playerHolder.Instance = null;
            Object.Destroy(_kernel);
            _animator.enabled = false;
            _agent.enabled = false;
            _collider.enabled = false;
            _ragdoll.Enable();
            _levelStateMachine.Enter<LevelFailedState>();
        }
    }
}