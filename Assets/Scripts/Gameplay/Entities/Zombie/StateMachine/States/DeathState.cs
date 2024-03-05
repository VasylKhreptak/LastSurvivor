using System.Collections.Generic;
using Analytics;
using Firebase.Analytics;
using Gameplay.Data;
using Gameplay.Entities.Zombie.StateMachine.States.Core;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Inspector.MinMax.Core;
using Pathfinding;
using UnityEngine;
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
        private readonly IAstarAI _ai;
        private readonly List<Zombie> _zombies;
        private readonly Zombie _zombie;
        private readonly LevelData _levelData;
        private readonly MinMaxValue<int> _priceForKill;
        private readonly Rigidbody _rigidbody;
        private readonly IPersistentDataService _persistentDataService;

        public DeathState(MonoKernel kernel, Ragdoll ragdoll, Animator animator, Collider collider, IAstarAI ai,
            List<Zombie> zombies, Zombie zombie, LevelData levelData, MinMaxValue<int> priceForKill, Rigidbody rigidbody,
            IPersistentDataService persistentDataService)
        {
            _kernel = kernel;
            _ragdoll = ragdoll;
            _animator = animator;
            _collider = collider;
            _ai = ai;
            _zombies = zombies;
            _zombie = zombie;
            _levelData = levelData;
            _priceForKill = priceForKill;
            _rigidbody = rigidbody;
            _persistentDataService = persistentDataService;
        }

        public void Enter()
        {
            _zombies.Remove(_zombie);
            Object.Destroy(_kernel);
            _animator.enabled = false;
            _collider.enabled = false;
            _ai.isStopped = true;
            _ai.canMove = false;
            _rigidbody.isKinematic = true;
            _ragdoll.Enable();
            FirebaseAnalytics.LogEvent(AnalyticEvents.ZombieDied);
            _levelData.CollectedMoney.Value += (int)(_priceForKill.GetRandom() *
                                                     _persistentDataService.Data.PlayerData.PlatformsData
                                                         .HelicopterPlatformData
                                                         .IncomeMultiplier.Value);
        }
    }
}