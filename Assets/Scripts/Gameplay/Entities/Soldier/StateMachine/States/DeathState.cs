using Gameplay.Entities.Soldier.StateMachine.States.Core;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Pathfinding;
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
        private readonly IAstarAI _ai;
        private readonly IPersistentDataService _persistentDataService;

        public DeathState(MonoKernel kernel, Ragdoll ragdoll, Animator animator, Collider collider, IAstarAI ai,
            IPersistentDataService persistentDataService)
        {
            _kernel = kernel;
            _ragdoll = ragdoll;
            _animator = animator;
            _collider = collider;
            _ai = ai;
            _persistentDataService = persistentDataService;
        }

        public void Enter()
        {
            Object.Destroy(_kernel);
            _animator.enabled = false;
            _collider.enabled = false;
            _ai.isStopped = true;
            _ragdoll.Enable();
            _persistentDataService.PersistentData.PlayerData.PlatformsData.BarracksPlatformData.SoldiersBank.Spend(1);
        }
    }
}