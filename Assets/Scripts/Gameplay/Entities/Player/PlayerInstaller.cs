using Adapters.Velocity;
using Entities.AI;
using Entities.Animations;
using Gameplay.Entities.Health.Core;
using Gameplay.Entities.Player.StateMachine;
using Gameplay.Entities.Player.StateMachine.States;
using Gameplay.Entities.Player.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using UnityEngine;
using UnityEngine.AI;
using Utilities.PhysicsUtilities;
using Utilities.PhysicsUtilities.Trigger;
using Zenject;
using Zenject.Infrastructure.Toggleable;

namespace Gameplay.Entities.Player
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private float _maxHealth = 100f;
        [SerializeField] private MoveAnimation.Preferences _moveAnimationPreferences;
        [SerializeField] private AgentMover.Preferences _moveStatePreferences;
        [SerializeField] private Ragdoll.Preferences _ragdollPreferences;
        [SerializeField] private Collider _lootBoxDetectionCollider;
        [SerializeField] private ClosestTriggerObserver<LootBox.LootBox>.Preferences _closestLootBoxObserverPreferences;

        public override void InstallBindings()
        {
            Container.Bind<Animator>().FromComponentOnRoot().AsSingle();
            Container.Bind<NavMeshAgent>().FromComponentOnRoot().AsSingle();
            Container.BindInterfacesTo<AdaptedAgentForVelocity>().AsSingle();
            Container.Bind<IHealth>().FromInstance(new Health.Health(_maxHealth)).AsSingle();

            BindRagdoll();
            BindDeathHandler();
            BindMoveAnimation();
            BindStateMachine();
            BindPlayerWaypointNavigator();
            BindPlayerMapNavigator();
            // BindLootBoxTriggerZone();
            // BindClosestLootBoxObserver();
            EnterIdleState();

            Container.Bind<ToggleableManager>().AsSingle();
        }

        private void BindMoveAnimation() =>
            Container.BindInterfacesTo<MoveAnimation>().AsSingle().WithArguments(_moveAnimationPreferences);

        private void BindStateMachine()
        {
            BindStates();
            Container.Bind<PlayerStateFactory>().AsSingle();
            Container.BindInterfacesTo<PlayerStateMachine>().AsSingle();
        }

        private void BindStates()
        {
            Container.Bind<IdleState>().AsSingle();
            Container.Bind<MoveState>().AsSingle().WithArguments(_moveStatePreferences);
            Container.Bind<DeathState>().AsSingle().WithArguments(GetComponent<Collider>());
        }

        private void BindDeathHandler() => Container.BindInterfacesTo<PlayerDeathHandler>().AsSingle();

        private void EnterIdleState() => Container.Resolve<IStateMachine<IPlayerState>>().Enter<IdleState>();

        private void BindRagdoll()
        {
            Container.Bind<Ragdoll>().AsSingle().WithArguments(_ragdollPreferences);
            Container.Resolve<Ragdoll>().Disable();
        }

        private void BindPlayerWaypointNavigator() => Container.BindInterfacesAndSelfTo<PlayerWaypointNavigator>().AsSingle();

        private void BindPlayerMapNavigator() => Container.BindInterfacesAndSelfTo<PlayerMapNavigator>().AsSingle();

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
    }
}