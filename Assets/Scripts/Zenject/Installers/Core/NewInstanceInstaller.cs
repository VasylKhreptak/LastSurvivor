namespace Zenject.Installers.Core
{
    public class NewInstanceInstaller<T> : MonoInstaller
    {
        public override void InstallBindings() => Container.Bind<T>().AsSingle();
    }
}