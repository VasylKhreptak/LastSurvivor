using System.Collections.Generic;
using Gameplay.Data;
using Gameplay.Entities.Zombie.StateMachine.States.Core;
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

        public DeathState(MonoKernel kernel, Ragdoll ragdoll, Animator animator, Collider collider, IAstarAI ai,
            List<Zombie> zombies, Zombie zombie, LevelData levelData, MinMaxValue<int> priceForKill)
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
        }

        public void Enter()
        {
            _zombies.Remove(_zombie);
            Object.Destroy(_kernel);
            _animator.enabled = false;
            _collider.enabled = false;
            _ai.isStopped = true;
            _ragdoll.Enable();
            _levelData.CollectedMoney.Value += _priceForKill.GetRandom();
        }
    }
}