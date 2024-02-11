using System.Collections.Generic;
using Adapters.Velocity;
using Entities.AI;
using Entities.Animations;
using Gameplay.Entities.Collector.StateMachine;
using Gameplay.Entities.Collector.StateMachine.States;
using Gameplay.Entities.Collector.StateMachine.States.Core;
using Gameplay.Entities.Health.Core;
using Gameplay.Entities.Player;
using Infrastructure.StateMachine.Main.Core;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;
using Utilities.GameObjectUtilities;
using Utilities.PhysicsUtilities;
using Utilities.PhysicsUtilities.Trigger;
using Zenject;

namespace Gameplay.Entities.Collector
{
    public class CollectorInstaller : MonoInstaller
    {
        [SerializeField] private float _health = 100f;
        [SerializeField] private MoveAnimation.Preferences _moveAnimationPreferences;
        [SerializeField] private Collider _lootBoxDetectionTrigger;
        [SerializeField] private ClosestTriggerObserver<LootBox.LootBox>.Preferences _closestLootBoxObserverPreferences;
        [SerializeField] private AgentMover.Preferences _agentMoverPreferences;
        [SerializeField] private AgentTransformFollower.Preferences _agentTransformFollowerPreferences;
        [SerializeField] private MeleeAttacker.Preferences _meleeAttackerPreferences;
        [SerializeField] private Ragdoll.Preferences _ragdollPreferences;
        [SerializeField] private GameObjectRandomizer.Preferences _skinRandomizerPreferences;

        private Transform _followPoint;
        private List<Collector> _collectors;

        [Inject]
        private void Constructor(PlayerHolder playerHolder, List<Collector> collectors)
        {
            _collectors = collectors;

            _followPoint = playerHolder.Instance.CollectorFollowPoints[_collectors.Count];
        }

        public override void InstallBindings()
        {
            Container.BindInstance(gameObject).AsSingle();
            Container.Bind<Animator>().FromComponentOnRoot().AsSingle();
            Container.Bind<IAstarAI>().FromInstance(GetComponent<IAstarAI>()).AsSingle();
            Container.BindInterfacesTo<AdaptedAgentForVelocity>().AsSingle();
            Container.Bind<IHealth>().FromInstance(new Health.Health(_health)).AsSingle();
            Container.Bind<Collector>().FromComponentOnRoot().AsSingle();

            RandomizeSkin();
            BindRagdoll();
            BindMoveAnimation();
            BindLootBoxTriggerZone();
            BindClosestLootBoxObserver();
            BindDeathHandler();
            BindAgentMover();
            BindAgentTransformFollower();
            BindMeleeAttacker();
            BindStateMachine();
            EnterIdleState();
            RegisterCollector();
        }

        private void RandomizeSkin()
        {
            GameObjectRandomizer randomizer = new GameObjectRandomizer(_skinRandomizerPreferences);
            randomizer.Randomize();
        }

        private void BindRagdoll()
        {
            Container.Bind<Ragdoll>().AsSingle().WithArguments(_ragdollPreferences);
            Container.Resolve<Ragdoll>().Disable();
        }

        private void BindMoveAnimation() =>
            Container.BindInterfacesTo<MoveAnimation>().AsSingle().WithArguments(_moveAnimationPreferences);

        private void BindLootBoxTriggerZone() =>
            Container.BindInterfacesAndSelfTo<TriggerZone<LootBox.LootBox>>().AsSingle().WithArguments(_lootBoxDetectionTrigger);

        private void BindClosestLootBoxObserver()
        {
            Container
                .BindInterfacesAndSelfTo<ClosestTriggerObserver<LootBox.LootBox>>()
                .AsSingle()
                .WithArguments(_closestLootBoxObserverPreferences);
        }

        private void BindDeathHandler() => Container.BindInterfacesTo<CollectorDeathHandler>().AsSingle();

        private void BindAgentMover() =>
            Container.BindInterfacesAndSelfTo<AgentMover>().AsSingle().WithArguments(_agentMoverPreferences);

        private void BindAgentTransformFollower() =>
            Container.BindInterfacesAndSelfTo<AgentTransformFollower>().AsSingle().WithArguments(_agentTransformFollowerPreferences);

        private void BindMeleeAttacker() =>
            Container.BindInterfacesAndSelfTo<MeleeAttacker>().AsSingle().WithArguments(transform, _meleeAttackerPreferences);

        private void BindStateMachine()
        {
            BindStates();
            Container.Bind<CollectorStateFactory>().AsSingle();
            Container.BindInterfacesTo<CollectorStateMachine>().AsSingle();
        }

        private void BindStates()
        {
            Container.Bind<IdleState>().AsSingle();
            Container.Bind<MapNavigationState>().AsSingle().WithArguments(_followPoint);
            Container.Bind<DeathState>().AsSingle().WithArguments(GetComponent<Collider>());
        }

        private void EnterIdleState() => Container.Resolve<IStateMachine<ICollectorState>>().Enter<IdleState>();

        private void RegisterCollector() => _collectors.Add(GetComponent<Collector>());
    }
}