using Data.Static.Balance;
using Flexalon;
using Grid;
using Infrastructure.Services.StaticData.Core;
using UnityEngine;
using Zenject;
using Zenject.Infrastructure.Toggleable;

namespace Entities.Player
{
    public class PlayerInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private FlexalonGridLayout _barrelLayout;

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

            BindFuelGrid();
        }

        private void BindFuelGrid()
        {
            GridData gridData = new GridData(1, 1, 10);
            Container.BindInstance(gridData).AsSingle();
            Container.BindInstance(_barrelLayout).AsSingle();
            Container.Bind<GridStack>().AsSingle();
        }
    }
}