using System.Collections.Generic;
using Gameplay.Entities.Collector.StateMachine.States.Core;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Pathfinding;
using UnityEngine;
using Utilities.PhysicsUtilities;
using Zenject;

namespace Gameplay.Entities.Collector.StateMachine.States
{
    public class DeathState : ICollectorState, IState
    {
        private readonly MonoKernel _kernel;
        private readonly Ragdoll _ragdoll;
        private readonly Animator _animator;
        private readonly Collider _collider;
        private readonly IAstarAI _ai;
        private readonly IPersistentDataService _persistentDataService;
        private readonly List<Collector> _collectors;
        private readonly Collector _collector;

        public DeathState(MonoKernel kernel, Ragdoll ragdoll, Animator animator, Collider collider, IAstarAI ai,
            IPersistentDataService persistentDataService, List<Collector> collectors, Collector collector)
        {
            _kernel = kernel;
            _ragdoll = ragdoll;
            _animator = animator;
            _collider = collider;
            _ai = ai;
            _persistentDataService = persistentDataService;
            _collectors = collectors;
            _collector = collector;
        }

        public void Enter()
        {
            Object.Destroy(_kernel);
            _animator.enabled = false;
            _collider.enabled = false;
            _ai.isStopped = true;
            _ragdoll.Enable();
            _persistentDataService.PersistentData.PlayerData.PlatformsData.CollectorsPlatformData.CollectorsBank.Spend(1);
            _collectors.Remove(_collector);
        }
    }
}