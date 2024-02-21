using System.Collections.Generic;
using Entities.AI;
using Entities.Animations;
using Gameplay.Entities.Health.Core;
using Gameplay.Entities.Player.StateMachine;
using Gameplay.Entities.Player.StateMachine.States;
using Gameplay.Entities.Player.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Pathfinding;
using Providers.Velocity;
using UnityEngine;
using Utilities.PhysicsUtilities;
using Utilities.PhysicsUtilities.Trigger;
using Vibration;
using Zenject;
using Zenject.Infrastructure.Toggleable;

namespace Gameplay.Entities.Player
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private float _maxHealth = 100f;
        [SerializeField] private MoveAnimation.Preferences _moveAnimationPreferences;
        [SerializeField] private AgentMover.Preferences _movePreferences;
        [SerializeField] private AgentWaypointsFollower.Preferences _waypointsFollowerPreferences;
        [SerializeField] private Ragdoll.Preferences _ragdollPreferences;
        [SerializeField] private Collider _lootBoxDetectionCollider;
        [SerializeField] private ClosestTriggerObserver<LootBox.LootBox>.Preferences _closestLootBoxObserverPreferences;
        [SerializeField] private MeleeAttacker.Preferences _meleeAttackerPreferences;
        [SerializeField] private List<Transform> _collectorFollowPoints;
        [SerializeField] private DamageVibration.Preferences _damageVibrationPreferences;

        private Waypoints.Waypoints _waypoints;

        [Inject]
        private void Constructor(Waypoints.Waypoints waypoints) => _waypoints = waypoints;

        public override void InstallBindings()
        {
            Container.BindInstance(_waypoints).AsSingle();
            Container.Bind<Animator>().FromComponentOnRoot().AsSingle();
            Container.Bind<IAstarAI>().FromInstance(GetComponent<IAstarAI>()).AsSingle();
            Container.Bind<Rigidbody>().FromComponentOnRoot().AsSingle();
            Container.BindInterfacesTo<AgentVelocityProvider>().AsSingle();
            Container.Bind<IHealth>().FromInstance(new Health.Health(_maxHealth)).AsSingle();
            Container.BindInstance(_collectorFollowPoints).AsSingle().WhenInjectedInto<Player>();

            BindRagdoll();
            BindDeathHandler();
            BindMoveAnimation();
            BindAgentMover();
            BindAgentWaypointsFollower();
            BindMeleeAttacker();
            BindLootBoxTriggerZone();
            BindClosestLootBoxObserver();
            BindDamageVibration();
            BindStateMachine();
            EnterIdleState();

            Container.Bind<ToggleableManager>().AsSingle();
        }

        private void BindMoveAnimation() =>
            Container.BindInterfacesTo<MoveAnimation>().AsSingle().WithArguments(_moveAnimationPreferences);

        private void BindAgentMover() => Container.Bind<AgentMover>().AsSingle().WithArguments(_movePreferences);

        private void BindAgentWaypointsFollower() =>
            Container.Bind<AgentWaypointsFollower>().AsSingle().WithArguments(_waypointsFollowerPreferences);

        private void BindStateMachine()
        {
            BindStates();
            Container.Bind<PlayerStateFactory>().AsSingle();
            Container.BindInterfacesTo<PlayerStateMachine>().AsSingle();
        }

        private void BindStates()
        {
            Collider collider = GetComponent<Collider>();
            Container.Bind<IdleState>().AsSingle();
            Container.Bind<NavigationState>().AsSingle();
            Container.Bind<DeathState>().AsSingle().WithArguments(collider);
            Container.Bind<ReviveState>().AsSingle().WithArguments(collider);
        }

        private void BindDeathHandler() => Container.BindInterfacesTo<PlayerDeathHandler>().AsSingle();

        private void EnterIdleState() => Container.Resolve<IStateMachine<IPlayerState>>().Enter<IdleState>();

        private void BindRagdoll()
        {
            Container.Bind<Ragdoll>().AsSingle().WithArguments(_ragdollPreferences);
            Container.Resolve<Ragdoll>().Disable();
        }

        private void BindLootBoxTriggerZone()
        {
            Container
                .BindInterfacesAndSelfTo<TriggerZone<LootBox.LootBox>>()
                .AsSingle()
                .WithArguments(_lootBoxDetectionCollider);
        }

        private void BindClosestLootBoxObserver()
        {
            Container
                .BindInterfacesAndSelfTo<ClosestTriggerObserver<LootBox.LootBox>>()
                .AsSingle()
                .WithArguments(_closestLootBoxObserverPreferences);
        }

        private void BindMeleeAttacker() =>
            Container.BindInterfacesAndSelfTo<MeleeAttacker>().AsSingle().WithArguments(_meleeAttackerPreferences);

        private void BindDamageVibration() =>
            Container.BindInterfacesTo<DamageVibration>().AsSingle().WithArguments(_damageVibrationPreferences);
    }
}