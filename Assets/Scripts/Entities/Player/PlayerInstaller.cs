using Data.Static.Balance;
using Infrastructure.Services.StaticData.Core;
using UnityEngine;
using Zenject;
using Zenject.Infrastructure.Toggleable;

namespace Entities.Player
{
    public class PlayerInstaller : MonoInstaller
    {
        private IStaticDataService _staticDataService;

        [Inject]
        private void Constructor(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public override void InstallBindings()
        {
            Container.Bind<Transform>().FromComponentOnRoot().AsSingle();
            Container.Bind<Animator>().FromComponentOnRoot().AsSingle();
            Container.Bind<CharacterController>().FromComponentOnRoot().AsSingle();
            Container.Bind<PlayerPreferences>().FromInstance(_staticDataService.Balance.PlayerPreferences).AsSingle();
            Container.BindInterfacesTo<PlayerMovement>().AsSingle();
            Container.Bind<ToggleableManager>().AsSingle();
        }
    }
}