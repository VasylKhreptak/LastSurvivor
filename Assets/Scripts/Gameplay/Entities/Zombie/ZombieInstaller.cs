using System.Collections.Generic;
using Adapters.Velocity;
using Entities.AI;
using Entities.Animations;
using Gameplay.Entities.Health.Core;
using Gameplay.Entities.Health.Damages;
using Gameplay.Entities.Zombie.StateMachine;
using Gameplay.Entities.Zombie.StateMachine.States;
using Gameplay.Entities.Zombie.StateMachine.States.Core;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.StaticData.Core;
using Infrastructure.StateMachine.Main.Core;
using Inspector.MinMax;
using Inspector.MinMax.Core;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;
using Utilities.GameObjectUtilities;
using Utilities.PhysicsUtilities;
using Utilities.PhysicsUtilities.Trigger;
using Utilities.TransformUtilities;
using Visitor;
using Zenject;

namespace Gameplay.Entities.Zombie
{
    public class ZombieInstaller : MonoInstaller
    {
        [SerializeField] private MoveAnimation.Preferences _moveAnimationPreferences;
        [SerializeField] private GameObjectRandomizer.Preferences _skinRandomizerPreferences;
        [SerializeField] private RotationRandomizer.Preferences _rotationRandomizerPreferences;
        [SerializeField] private AgentMover.Preferences _agentMoverPreferences;
        [SerializeField] private AgentTransformFollower.Preferences _moveStatePreferences;
        [SerializeField] private Collider _targetDetectionCollider;
        [SerializeField] private ClosestTriggerObserver<IVisitable<ZombieDamage>>.Preferences _closestTriggerObserverPreferences;
        [SerializeField] private ZombieAttacker.Preferences _zombieAttackPreferences;
        [SerializeField] private Ragdoll.Preferences _ragdollPreferences;
        [SerializeField] private EnemyDetectionTriggerAwakener.Preferences _enemyDetectionTriggerAwakenerPreferences;
        [SerializeField] private MinMaxValue<int> _priceForKill = new IntMinMaxValue(10, 30);

        private List<Zombie> _zombies;
        private IPersistentDataService _persistentDataService;
        private IStaticDataService _staticDataService;

        [Inject]
        private void Constructor(List<Zombie> zombies, IPersistentDataService persistentDataService,
            IStaticDataService staticDataService)
        {
            _zombies = zombies;
            _persistentDataService = persistentDataService;
            _staticDataService = staticDataService;
        }

        public override void InstallBindings()
        {
            Container.BindInstance(gameObject).AsSingle();
            Container.Bind<Animator>().FromComponentOnRoot().AsSingle();
            Container.Bind<IAstarAI>().FromInstance(GetComponent<IAstarAI>()).AsSingle();
            Container.Bind<Zombie>().FromComponentOnRoot().AsSingle();
            Container.BindInterfacesTo<AdaptedAgentForVelocity>().AsSingle();
            Container.Bind<IHealth>().FromInstance(new Health.Health(GetHealth())).AsSingle();

            RandomizeSkin();
            RandomizeRotation();
            BindMoveAnimation();
            BindRagdoll();
            BindStateMachine();
            BindTargetsZone();
            BindClosestTriggerObserver();
            BindZombieTargetFollower();
            BindEnemyDetectionTriggerAwakener();
            BindDeathHandler();
            BindZombieAttacker();
            BindAgentMover();
            BindAgentTransformFollower();
            EnterIdleState();
            RegisterZombie();
        }

        private float GetHealth() =>
            _staticDataService.Balance.ZombieHealth.Get(_persistentDataService.PersistentData.PlayerData.Level);

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
            Container.Bind<FollowTransformState>().AsSingle();
            Container.Bind<DeathState>().AsSingle().WithArguments(GetComponent<Collider>(), _priceForKill);
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
                .AsSingle();

        private void EnterIdleState() => Container.Resolve<IStateMachine<IZombieState>>().Enter<IdleState>();

        private void BindZombieAttacker() =>
            Container.BindInterfacesAndSelfTo<ZombieAttacker>().AsSingle().WithArguments(_zombieAttackPreferences);

        private void BindRagdoll()
        {
            Container.Bind<Ragdoll>().AsSingle().WithArguments(_ragdollPreferences);
            Container.Resolve<Ragdoll>().Disable();
        }

        private void RegisterZombie() => _zombies.Add(GetComponent<Zombie>());

        private void BindClosestTriggerObserver()
        {
            Container
                .BindInterfacesAndSelfTo<ClosestTriggerObserver<IVisitable<ZombieDamage>>>()
                .AsSingle()
                .WithArguments(_closestTriggerObserverPreferences);
        }

        private void BindAgentMover() =>
            Container.BindInterfacesAndSelfTo<AgentMover>().AsSingle().WithArguments(_agentMoverPreferences);

        private void BindAgentTransformFollower() =>
            Container.Bind<AgentTransformFollower>().AsSingle().WithArguments(_moveStatePreferences);

        private void BindEnemyDetectionTriggerAwakener()
        {
            Container
                .BindInterfacesTo<EnemyDetectionTriggerAwakener>()
                .AsSingle()
                .WithArguments(_targetDetectionCollider, _enemyDetectionTriggerAwakenerPreferences);
        }
    }
}