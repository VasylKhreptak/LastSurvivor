using CameraUtilities.Shaker;
using UnityEngine;

namespace Zenject.Installers.SceneContext
{
    public class CameraShakerInstaller : MonoInstaller
    {
        [SerializeField] private CameraShaker.Preferences _preferences;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CameraShaker>().AsSingle().WithArguments(_preferences);
        }
    }
}