using Audio.Players.Step;
using Data.Static.Balance;
using Flexalon;
using Grid;
using Infrastructure.Services.StaticData.Core;
using Plugins.Banks;
using Providers.Velocity;
using UnityEngine;
using Zenject;

namespace Main.Entities.Player
{
    public class PlayerInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private FlexalonGridLayout _barrelLayout;
        [SerializeField] private FootstepAudioPlayer.Preferences _stepAudioPreferences;

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
            Container.BindInterfacesTo<TransformVelocityProvider>().AsSingle();

            BindPlayerMovement();
            BindFuelGrid();
            BindStepAudioPlayer();
        }

        private void BindPlayerMovement() => Container.BindInterfacesTo<PlayerMovement>().AsSingle();

        private void BindFuelGrid()
        {
            ClampedIntegerBank clampedBank = new ClampedIntegerBank(0, 10);
            GridStack gridStack = new GridStack(_barrelLayout, clampedBank);
            Container.BindInstance(gridStack).AsSingle();
        }

        private void BindStepAudioPlayer() => Container.Bind<FootstepAudioPlayer>().AsSingle().WithArguments(_stepAudioPreferences);
    }
}