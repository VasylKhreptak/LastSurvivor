using Adapters.Velocity;
using Entities;
using Entities.Animations;
using Gameplay.Entities.Health.Core;
using Gameplay.Entities.Zombie.DeathHandlers.Core;
using UnityEngine;
using UnityEngine.AI;
using Utilities.GameObjectUtilities;
using Zenject;

namespace Gameplay.Entities.Zombie
{
    public class ZombieInstaller : MonoInstaller
    {
        [SerializeField] private float _maxHealth = 100f;
        [SerializeField] private MoveAnimation.Preferences _moveAnimationPreferences;
        [SerializeField] private Ragdoll.Preferences _ragdollPreferences;
        [SerializeField] private GameObjectRandomizer.Preferences _skinRandomizerPreferences;

        public override void InstallBindings()
        {
            Container.BindInstance(gameObject).AsSingle();
            Container.Bind<Animator>().FromComponentOnRoot().AsSingle();
            Container.Bind<NavMeshAgent>().FromComponentOnRoot().AsSingle();
            Container.BindInterfacesTo<AdaptedAgentForVelocity>().AsSingle();
            Container.Bind<IHealth>().FromInstance(new Health.Health(_maxHealth)).AsSingle();

            RandomizeSkin();
            BindMoveAnimation();
            BindRagdoll();
            BindDeathHandler();
        }

        private void BindMoveAnimation() =>
            Container.BindInterfacesTo<MoveAnimation>().AsSingle().WithArguments(_moveAnimationPreferences);

        private void BindRagdoll() => Container.Bind<Ragdoll>().AsSingle().WithArguments(_ragdollPreferences);

        private void BindDeathHandler() => Container.BindInterfacesTo<ZombieDeathHandler>().AsSingle();

        private void RandomizeSkin()
        {
            GameObjectRandomizer randomizer = new GameObjectRandomizer(_skinRandomizerPreferences);
            randomizer.Randomize();
        }
    }
}