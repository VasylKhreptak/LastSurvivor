using UnityEngine;
using Zenject;

namespace Plugins.AudioService.Zenject.Installer
{
    public class AudioServiceInstaller : MonoInstaller
    {
        [SerializeField] private AudioService.Preferences _preferences;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<AudioService>().AsSingle().WithArguments(_preferences);
        }
    }
}