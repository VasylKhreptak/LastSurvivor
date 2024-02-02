using Adapters.Velocity;
using Audio.Players;
using Data.Persistent.Platforms;
using Gameplay.Weapons.Core;
using Gameplay.Weapons.Core.Fire;
using Gameplay.Weapons.Minigun.StateMachine;
using Gameplay.Weapons.Minigun.StateMachine.States;
using Gameplay.Weapons.Minigun.StateMachine.States.Core;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.StateMachine.Main.Core;
using ObjectPoolSystem;
using ObjectPoolSystem.PoolCategories;
using Plugins.Banks;
using UnityEngine;
using Zenject;
using Zenject.Infrastructure.Toggleable;

namespace Gameplay.Weapons.Minigun
{
    public class MinigunInstaller : MonoInstaller
    {
        [Header("Preferences")]
        [SerializeField] private BarrelSpiner.Preferences _barrelRotatorPreferences;
        [SerializeField] private ShootState.Preferences _firePreferences;
        [SerializeField] private AudioPlayer.Preferences _fireAudioPreferences;
        [SerializeField] private MinigunSpinAudio.Preferences _spinAudioPreferences;
        [SerializeField] private ObjectSpawner<Particle>.Preferences _shootParticlePreferences;
        [SerializeField] private ShellSpawner.Preferences _shellSpawnerPreferences;
        [SerializeField] private ReloadState.Preferences _reloadStatePreferences;

        private HelicopterPlatformData _helicopterPlatformData;
        private WeaponHolder _weaponHolder;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService, WeaponHolder weaponHolder)
        {
            _helicopterPlatformData = persistentDataService.PersistentData.PlayerData.PlatformsData.HelicopterPlatformData;
            _weaponHolder = weaponHolder;
        }

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<AdaptedTransformForVelocity>().AsSingle().WithArguments(transform);

            BindMinigunAmmo();
            BindBarrelSpiner();
            BindBarrelSpinAudio();
            BindShootParticle();
            BindShootAudioPlayer();
            BindShellSpawner();
            BindStateMachine();
            BindWeapon();
            RegisterWeapon();
            EnterIdleState();

            Container.Bind<ToggleableManager>().AsSingle();
        }

        private void BindStateMachine()
        {
            BindStates();
            Container.Bind<MinigunStateFactory>().AsSingle();
            Container.BindInterfacesTo<MinigunStateMachine>().AsSingle();
        }

        private void BindStates()
        {
            Container.Bind<SpinUpState>().AsSingle();
            Container.Bind<ShootState>().AsSingle().WithArguments(_firePreferences);
            Container.Bind<ReloadState>().AsSingle().WithArguments(_reloadStatePreferences);
            Container.Bind<SpinDownState>().AsSingle();
            Container.Bind<IdleState>().AsSingle();
        }

        private void BindMinigunAmmo()
        {
            int ammoCapacity = _helicopterPlatformData.MinigunAmmoCapacity;

            ClampedIntegerBank ammo = new ClampedIntegerBank(ammoCapacity, ammoCapacity);
            Container.BindInstance(ammo).AsSingle();
        }

        private void BindBarrelSpiner() =>
            Container.BindInterfacesAndSelfTo<BarrelSpiner>().AsSingle().WithArguments(_barrelRotatorPreferences);

        private void BindBarrelSpinAudio() =>
            Container.BindInterfacesTo<MinigunSpinAudio>().AsSingle().WithArguments(_spinAudioPreferences);

        private void BindShootParticle() => Container.Bind<ObjectSpawner<Particle>>().AsSingle().WithArguments(_shootParticlePreferences);

        private void BindShootAudioPlayer() => Container.Bind<AudioPlayer>().AsSingle().WithArguments(_fireAudioPreferences);

        private void BindShellSpawner() => Container.Bind<ShellSpawner>().AsSingle().WithArguments(_shellSpawnerPreferences);

        private void EnterIdleState() => Container.Resolve<IStateMachine<IMinigunState>>().Enter<IdleState>();

        private void BindWeapon()
        {
            Container.Bind<Transform>().FromComponentOnRoot().AsSingle().WhenInjectedInto<IWeapon>();
            Container.BindInterfacesTo<Minigun>().AsSingle();
        }

        private void RegisterWeapon() => _weaponHolder.Instance.Value = Container.Resolve<IWeapon>();
    }
}