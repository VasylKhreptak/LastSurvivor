using UnityEngine;
using Zenject;

namespace Plugins.AudioPoolSystem.Zenject
{
    public class AudioPoolInstaller : MonoInstaller
    {
        [SerializeField] private AudioPool.Preferences _preferences;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<AudioPool>().AsSingle().WithArguments(_preferences);
        }
    }
}