using Adapters.Velocity;
using Entities.Animations;
using Gameplay.Entities.Health;
using Gameplay.Entities.Health.Core;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Gameplay.Entities.Zombie
{
    public class ZombieInstaller : MonoInstaller
    {
        [SerializeField] private float _maxHealth = 100f;
        [SerializeField] private MoveAnimation.Preferences _moveAnimationPreferences;

        public override void InstallBindings()
        {
            Container.Bind<Animator>().FromComponentOnRoot().AsSingle();
            Container.Bind<NavMeshAgent>().FromComponentOnRoot().AsSingle();
            Container.BindInterfacesTo<AdaptedAgentForVelocity>().AsSingle();
            Container.Bind<IHealth>().FromInstance(new Health.Health(_maxHealth)).AsSingle();

            BindMoveAnimation();
            BindDeathHandler();
        }

        private void BindMoveAnimation() =>
            Container.BindInterfacesTo<MoveAnimation>().AsSingle().WithArguments(_moveAnimationPreferences);

        private void BindDeathHandler() => Container.BindInterfacesTo<DeathHandler>().AsSingle();
    }
}