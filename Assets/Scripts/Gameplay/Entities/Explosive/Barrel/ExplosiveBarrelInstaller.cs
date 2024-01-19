using Audio.Players;
using Entities.Health;
using Entities.Health.Core;
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
        [SerializeField] private ExplosionDamageApplier.Preferences _explosionDamageApplierPreferences;
        [SerializeField] private AudioPlayer.Preferences _explosionAudioPlayerPreferences;

        public override void InstallBindings()
        {
            Container.Bind<IHealth>().FromInstance(new Health(_maxHealth)).AsSingle();
            Container.BindInstance(gameObject).AsSingle();

            Container.Bind<ExplosionRigidbodyAffector>().AsSingle().WithArguments(_rigidbodyAffectorPreferences);
            Container.Bind<ExplosionDamageApplier>().AsSingle().WithArguments(_explosionDamageApplierPreferences);
            Container.Bind<AudioPlayer>().AsSingle().WithArguments(_explosionAudioPlayerPreferences);
            Container.BindInterfacesTo<BarrelExploder>().AsSingle();
        }
    }
}