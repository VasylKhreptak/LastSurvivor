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
            Container.BindInterfacesTo<HelicopterIncomeMultiplierText>().AsSingle().WithArguments(_references.IncomeMultiplierText);
            
            Container.Bind<ToggleableManager>().AsSingle();
        }
    }
}