using UnityEngine;
using Zenject;
using Zenject.Infrastructure.Toggleable;

namespace Entities.Player
{
    public class PlayerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ToggleableManager>().AsSingle();
            Container.Bind<Transform>().FromComponentOnRoot().AsSingle();
            Container.Bind<Rigidbody>().FromComponentOnRoot().AsSingle();
            Container.Bind<Animator>().FromComponentOnRoot().AsSingle();
            Container.BindInterfacesTo<PlayerMovement>().AsSingle();
            Container.BindInterfacesTo<PlayerMoveAnimation>().AsSingle();
        }
    }
}