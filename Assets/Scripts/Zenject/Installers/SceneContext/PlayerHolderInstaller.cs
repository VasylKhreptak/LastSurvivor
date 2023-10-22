using Entities.Player;
using Holders;
using Holders.Core;

namespace Zenject.Installers.SceneContext
{
    public class PlayerHolderInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<Holder<Player>>().FromInstance(new PlayerHolder()).AsSingle();
        }
    }
}