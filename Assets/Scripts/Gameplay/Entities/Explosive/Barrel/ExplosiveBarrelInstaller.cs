using Entities.Core.Health;
using Entities.Core.Health.Core;
using Gameplay.Entities.Explosive.Barrel.DeathHandlers.Core;
using Gameplay.Entities.Explosive.Core;
using UnityEngine;
using Zenject;

namespace Gameplay.Entities.Explosive.Barrel
{
    public class ExplosiveBarrelInstaller : MonoInstaller
    {
        [SerializeField] private float _maxHealth = 100f;
        [SerializeField] private ExplosionRigidbodyAffector.Preferences _rigidbodyAffectorPreferences;

        public override void InstallBindings()
        {
            Container.Bind<IHealth>().FromInstance(new Health(_maxHealth)).AsSingle();
            Container.BindInstance(gameObject).AsSingle();

            Container.Bind<ExplosionRigidbodyAffector>().AsSingle().WithArguments(_rigidbodyAffectorPreferences);
            Container.BindInterfacesTo<ExplosiveBarrelDeathHandler>().AsSingle();
        }
    }
}