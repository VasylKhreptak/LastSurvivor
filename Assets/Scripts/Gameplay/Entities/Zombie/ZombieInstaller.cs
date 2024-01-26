using System.Collections.Generic;
using Adapters.Velocity;
using Entities.Animations;
using Entities.StateMachine.States;
using Gameplay.Entities.Health.Core;
using Gameplay.Entities.Health.Damages;
using Gameplay.Entities.Zombie.StateMachine;
using Gameplay.Entities.Zombie.StateMachine.States;
using Gameplay.Entities.Zombie.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using UnityEngine;
using UnityEngine.AI;
using Utilities.GameObjectUtilities;
using Utilities.PhysicsUtilities;
using Utilities.TransformUtilities;
using Visitor;
using Zenject;

namespace Gameplay.Entities.Zombie
{
    public class ZombieInstaller : MonoInstaller
    {
        [SerializeField] private float _maxHealth = 100f;
        [SerializeField] private MoveAnimation.Preferences _moveAnimationPreferences;
        [SerializeField] private GameObjectRandomizer.Preferences _skinRandomizerPreferences;
        [SerializeField] private RotationRandomizer.Preferences _rotationRandomizerPreferences;
        [SerializeField] private AgentMoveState.Preferences _moveStatePreferences;
        [SerializeField] private Collider _targetDetectionCollider;
        [SerializeField] private ZombieTargetFollower.Preferences _zombieTargetFollowerPreferences;
        [SerializeField] private ZombieAttacker.Preferences _zombieAttackPreferences;
        [SerializeField] private Ragdoll.Preferences _ragdollPreferences;

        private List<Zombie> _zombies;

        [Inject]
        private void Constructor(List<Zombie> zombies)
        {
            _zombies = zombies;
        }

        public override void InstallBindings()
        {
            Container.BindInstance(gameObject).AsSingle();
            Container.Bind<Animator>().FromComponentOnRoot().AsSingle();
            Container.Bind<NavMeshAgent>().FromComponentOnRoot().AsSingle();
            Container.Bind<Zombie>().FromComponentOnRoot().AsSingle();
            Container.BindInterfacesTo<AdaptedAgentForVelocity>().AsSingle();
            Container.Bind<IHealth>().FromInstance(new Health.Health(_maxHealth)).AsSingle();

            RandomizeSkin();
            RandomizeRotation();
            BindMoveAnimation();
            BindRagdoll();
            BindStateMachine();
            BindTargetsZone();
            BindZombieTargetFollower();
            BindDeathHandler();
            BindZombieAttacker();
            EnterIdleState();
            RegisterZombie();
        }

        private void BindMoveAnimation() =>
            Container.BindInterfacesTo<MoveAnimation>().AsSingle().WithArguments(_moveAnimationPreferences);

        private void BindDeathHandler() => Container.BindInterfacesTo<ZombieDeathHandler>().AsSingle();

        private void RandomizeSkin()
        {
            GameObjectRandomizer randomizer = new GameObjectRandomizer(_skinRandomizerPreferences);
            randomizer.Randomize();
        }

        private void RandomizeRotation()
        {
            RotationRandomizer randomizer = new RotationRandomizer(transform, _rotationRandomizerPreferences);
            randomizer.Randomize();
        }

        private void BindStateMachine()
        {
            BindStates();
            Container.Bind<ZombieStateFactory>().AsSingle();
            Container.BindInterfacesTo<ZombieStateMachine>().AsSingle();
        }

        private void BindStates()
        {
            Container.Bind<IdleState>().AsSingle();
            Container.Bind<MoveState>().AsSingle().WithArguments(_moveStatePreferences);
            Container.Bind<DeathState>().AsSingle().WithArguments(GetComponent<Collider>());
        }

        private void BindTargetsZone()
        {
            Container
                .BindInterfacesAndSelfTo<TriggerZone<IVisitable<ZombieDamage>>>()
                .AsSingle()
                .WithArguments(_targetDetectionCollider);
        }

        private void BindZombieTargetFollower() =>
            Container.BindInterfacesAndSelfTo<ZombieTargetFollower>()
                .AsSingle()
                .WithArguments(transform, _zombieTargetFollowerPreferences);

        private void EnterIdleState() => Container.Resolve<IStateMachine<IZombieState>>().Enter<IdleState>();

        private void BindZombieAttacker() =>
            Container.BindInterfacesTo<ZombieAttacker>().AsSingle().WithArguments(_zombieAttackPreferences);

        private void BindRagdoll()
        {
            Container.Bind<Ragdoll>().AsSingle().WithArguments(_ragdollPreferences);
            Container.Resolve<Ragdoll>().Disable();
        }

        private void RegisterZombie() => _zombies.Add(GetComponent<Zombie>());
    }
}