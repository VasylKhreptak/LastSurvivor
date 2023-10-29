using UnityEngine;
using Zenject;

namespace Platforms.HelicopterPlatform
{
    public class HelicopterPlatformInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private HelicopterPlatformViewReferences _references;

        public override void InstallBindings()
        {
            Container.BindInstance(_references).AsSingle();
            Container.BindInterfacesTo<HelicopterUpgradeZone>().AsSingle();
        }
    }
}