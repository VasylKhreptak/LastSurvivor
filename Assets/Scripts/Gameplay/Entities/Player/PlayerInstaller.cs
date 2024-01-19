using Adapters.Velocity;
using Entities.Animations;
using Entities.Health;
using Entities.Health.Core;
using Gameplay.Entities.Player.StateMachine;
using Gameplay.Entities.Player.StateMachine.States;
using Gameplay.Entities.Player.StateMachine.States.Core;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Gameplay.Entities.Player
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private float _maxHealth = 100f;
        [SerializeField] private MoveAnimation.Preferences _moveAnimationPreferences;
        [SerializeField] private MoveState.Preferences _moveStatePreferences;

        public override void InstallBindings()
        {
            Container.Bind<Animator>().FromComponentOnRoot().AsSingle();
            Container.Bind<NavMeshAgent>().FromComponentOnRoot().AsSingle();
            Container.BindInterfacesTo<AdaptedAgentForVelocity>().AsSingle();
            Container.Bind<IHealth>().FromInstance(new Health(_maxHealth)).AsSingle();

            BindDeathHandler();
            BindMoveAnimation();
            BindStateMachine();

            Container.BindInterfacesTo<PlayerController>().AsSingle();
        }

        private void BindMoveAnimation() => Container.BindInterfacesTo<MoveAnimation>().AsSingle().WithArguments(_moveAnimationPreferences);

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
        }

        private void BindDeathHandler() => Container.BindInterfacesTo<PlayerDeathHandler>().AsSingle();
    }
}