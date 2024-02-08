using Adapters.Velocity;
using Entities.AI;
using Entities.Animations;
using Gameplay.Entities.Health.Core;
using Gameplay.Entities.Soldier.StateMachine;
using Gameplay.Entities.Soldier.StateMachine.States;
using ObjectPoolSystem;
using ObjectPoolSystem.PoolCategories;
using Tags.Gameplay;
using UnityEngine;
using UnityEngine.AI;
using Utilities.PhysicsUtilities;
using Utilities.PhysicsUtilities.Trigger;
using Zenject;
using AudioPlayer = Audio.Players.AudioPlayer;

namespace Gameplay.Entities.Soldier
{
    public class SoldierInstaller : MonoInstaller
    {
        [SerializeField] private Transform _viewTransform;
        [SerializeField] private float _maxHealth = 100f;
        [SerializeField] private AgentMover.Preferences _agentMoverPreferences;
        [SerializeField] private AgentTransformFollower.Preferences _followTransformStatePreferences;
        [SerializeField] private PlaneMoveAnimation.Preferences _moveAnimationPreferences;
        [SerializeField] private Ragdoll.Preferences _ragdollPreferences;
        [SerializeField] private Collider _targetDetectionCollider;
        [SerializeField] private ClosestTriggerObserver<SoldierTarget>.Preferences _closestTargetObserverPreferences;
        [SerializeField] private SoldierAimer.Preferences _soldierAimerPreferences;
        [SerializeField] private SoldierShooter.Preferences _shootPreferences;
        [SerializeField] private AudioPlayer.Preferences _shootAudioPlayerPreferences;
        [SerializeField] private ObjectSpawner<Particle>.Preferences _shootParticlePreferences;

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
            BindAimer();
            BindMoveAnimation();
            BindRagdoll();
            BindShootAudio();
            BindShootParticle();
            BindShooter();
            BindDeathHandler();
            BindAgentMover();
            BindAgentTransformFollower();
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
            Container.Bind<FollowTransformState>().AsSingle();
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

        private void BindAimer() =>
            Container.BindInterfacesAndSelfTo<SoldierAimer>().AsSingle().WithArguments(_viewTransform, _soldierAimerPreferences);

        private void RegisterSoldier() => _platoon.Soldiers.Add(GetComponent<Soldier>());

        private void BindShootParticle() =>
            Container.Bind<ObjectSpawner<Particle>>().AsSingle().WithArguments(_shootParticlePreferences);

        private void BindShooter() => Container.BindInterfacesAndSelfTo<SoldierShooter>().AsSingle().WithArguments(_shootPreferences);

        private void BindShootAudio() => Container.Bind<AudioPlayer>().AsSingle().WithArguments(_shootAudioPlayerPreferences);

        private void BindDeathHandler() => Container.BindInterfacesTo<SoldierDeathHandler>().AsSingle();

        private void BindAgentMover() =>
            Container.BindInterfacesAndSelfTo<AgentMover>().AsSingle().WithArguments(_agentMoverPreferences);

        private void BindAgentTransformFollower() =>
            Container.Bind<AgentTransformFollower>().AsSingle().WithArguments(_followTransformStatePreferences);
    }
}