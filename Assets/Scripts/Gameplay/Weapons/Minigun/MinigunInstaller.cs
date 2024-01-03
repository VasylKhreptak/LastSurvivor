using Data.Persistent.Platforms;
using Gameplay.Weapons.Minigun.StateMachine;
using Gameplay.Weapons.Minigun.StateMachine.States;
using Gameplay.Weapons.Minigun.StateMachine.States.Core;
using Infrastructure.Services.PersistentData.Core;
using Plugins.Banks;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapons.Minigun
{
    public class MinigunInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private Transform _transform;
        [SerializeField] private Transform _barrelTransform;

        [Header("Preferences")]
        [SerializeField] private WeaponAimer.Preferences _aimPreferences;
        [SerializeField] private BarrelRotator.Preferences _barrelRotatorPreferences;

        private HelicopterPlatformData _helicopterPlatformData;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService)
        {
            _helicopterPlatformData = persistentDataService.PersistentData.PlayerData.PlatformsData.HelicopterPlatformData;
        }

        public override void InstallBindings()
        {
            BindMinigunAmmo();
            BindAimer();
            BindBarrelRotator();
            BindStateMachine();
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
            Container.Bind<FireState>().AsSingle();
            Container.Bind<SpinDownState>().AsSingle();
            Container.Bind<IdleState>().AsSingle();
        }

        private void BindMinigunAmmo()
        {
            int ammoCapacity = _helicopterPlatformData.MinigunAmmoCapacity;

            ClampedIntegerBank ammo = new ClampedIntegerBank(ammoCapacity, ammoCapacity);
            Container.BindInstance(ammo).AsSingle();
        }

        private void BindAimer()
        {
            Container
                .BindInterfacesTo<WeaponAimer>()
                .AsSingle()
                .WithArguments(_aimPreferences, _transform);
        }

        private void BindBarrelRotator()
        {
            Container
                .BindInterfacesAndSelfTo<BarrelRotator>()
                .AsSingle()
                .WithArguments(_barrelTransform, _barrelRotatorPreferences);
        }
    }
}