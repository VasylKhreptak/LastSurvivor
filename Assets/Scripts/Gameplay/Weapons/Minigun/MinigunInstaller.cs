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

        [Header("Preferences")]
        [SerializeField] private WeaponAimer.Preferences _aimPreferences;

        private HelicopterPlatformData _helicopterPlatformData;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService)
        {
            _helicopterPlatformData = persistentDataService.PersistentData.PlayerData.PlatformsData.HelicopterPlatformData;
        }

        public override void InstallBindings()
        {
            Container.BindInstance(_transform).AsSingle();
            BindMagazine();
            BindStateMachine();
            BindAimer();
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

        private void BindMagazine()
        {
            ClampedIntegerBank clampedIntegerBank =
                new ClampedIntegerBank(_helicopterPlatformData.MinigunAmmoCapacity, _helicopterPlatformData.MinigunAmmoCapacity);

            Container.BindInstance(clampedIntegerBank).AsSingle();
        }

        private void BindAimer() => Container.BindInterfacesTo<WeaponAimer>().AsSingle().WithArguments(_aimPreferences);
    }
}