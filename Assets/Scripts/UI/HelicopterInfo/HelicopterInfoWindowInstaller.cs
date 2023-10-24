using UnityEngine;
using Zenject;
using Zenject.Infrastructure.Toggleable;

namespace UI.HelicopterInfo
{
    public class HelicopterInfoWindowInstaller : MonoInstaller
    {
        [SerializeField] private HelicopterInfoWindowReferences _references;

        public override void InstallBindings()
        {
            Container.Bind<HelicopterInfoWindowReferences>().AsSingle();
            Container.Bind<HelicopterInfoWindow>().AsSingle();

            BindHelicopterIncomeMultiplierText();
            BindHelicopterFuelTankText();

            Container.Bind<ToggleableManager>().AsSingle();
        }

        private void BindHelicopterIncomeMultiplierText()
        {
            Container.BindInterfacesTo<HelicopterIncomeMultiplierText>().AsSingle().WithArguments(_references.IncomeMultiplierText);
        }

        private void BindHelicopterFuelTankText()
        {
            Container.BindInterfacesTo<HelicopterFuelTankText>().AsSingle().WithArguments(_references.FuelTankText);
        }
    }
}