using Adapters.Velocity;
using Entities.Animations;
using Entities.StateMachine.States;
using Gameplay.Entities.Health.Core;
using Gameplay.Entities.Soldier.StateMachine;
using Gameplay.Entities.Soldier.StateMachine.States;
using Tags.Gameplay;
using UnityEngine;
using UnityEngine.AI;
using Utilities.PhysicsUtilities;
using Utilities.PhysicsUtilities.Trigger;
using Zenject;

namespace Gameplay.Entities.Soldier
{
    public class SoldierInstaller : MonoInstaller
    {
        [SerializeField] private Transform _viewTransform;
        [SerializeField] private float _maxHealth = 100f;
        [SerializeField] private AgentFollowTransformState.Preferences _followTransformStatePreferences;
        [SerializeField] private PlaneMoveAnimation.Preferences _moveAnimationPreferences;
        [SerializeField] private Ragdoll.Preferences _ragdollPreferences;
        [SerializeField] private Collider _targetDetectionCollider;
        [SerializeField] private ClosestTriggerObserver<SoldierTarget>.Preferences _closestTargetObserverPreferences;
        [SerializeField] private SoldierAimer.Preferences _soldierAimerPreferences;

        private Platoon.Platoon _platoon;

        [Inject]
        private void Constructor(Platoon.Platoon platoon)
        {
            _platoon = platoon;
        }

        public override void InstallBindings()
        {
            Container.BindInstance(gameObject).AsSingle();
            Container.Bind<Animator>().FromComponentOnRoot().AsSingle();
            Container.Bind<NavMeshAgent>().FromComponentOnRoot().AsSingle();
            Container.BindInterfacesTo<AdaptedAgentForVelocity>().AsSingle();
            Container.Bind<IHealth>().FromInstance(new Health.Health(_maxHealth)).AsSingle();

            BindTargetsZone();
            BindClosestTargetObserver();
            BindSoldierAimer();
            BindMoveAnimation();
            BindRagdoll();
            BindStateMachine();
            RegisterSoldier();
            EnterIdleState();
        }

        private void BindStateMachine()
        {
            BindStates();
            Container.Bind<SoldierStateFactory>().AsSingle();
            Container.BindInterfacesTo<SoldierStateMachine>().AsSingle();
        }

        private void BindStates()
        {
            Container.Bind<IdleState>().AsSingle();
            Container.Bind<FollowTransformState>().AsSingle().WithArguments(_followTransformStatePreferences);
            Container.Bind<DeathState>().AsSingle().WithArguments(GetComponent<Collider>());
        }

        private void EnterIdleState() => Container.Resolve<IdleState>().Enter();

        private void BindMoveAnimation()
        {
            Container
                .BindInterfacesAndSelfTo<PlaneMoveAnimation>()
                .AsSingle()
                .WithArguments(_moveAnimationPreferences, _viewTransform);
        }

        private void BindRagdoll()
        {
            Container.Bind<Ragdoll>().AsSingle().WithArguments(_ragdollPreferences);
            Container.Resolve<Ragdoll>().Disable();
        }

        private void BindTargetsZone()
        {
            Container
                .BindInterfacesAndSelfTo<TriggerZone<SoldierTarget>>()
                .AsSingle()
                .WithArguments(_targetDetectionCollider);
        }

        private void BindClosestTargetObserver()
        {
            Container
                .BindInterfacesAndSelfTo<ClosestTriggerObserver<SoldierTarget>>()
                .AsSingle()
                .WithArguments(_closestTargetObserverPreferences);
        }

        private void BindSoldierAimer() =>
            Container.BindInterfacesAndSelfTo<SoldierAimer>().AsSingle().WithArguments(_viewTransform, _soldierAimerPreferences);

        private void RegisterSoldier() => _platoon.Soldiers.Add(GetComponent<Soldier>());
    }
}