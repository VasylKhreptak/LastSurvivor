using UnityEngine;
using Zenject;
using Zenject.Infrastructure.Toggleable;

namespace Entities.Player
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private PlayerViewReferences _playerViewReferences;

        public override void InstallBindings()
        {
            Container.Bind<PlayerViewReferences>().FromInstance(_playerViewReferences).AsSingle();
            Container.BindInterfacesTo<PlayerMovement>().AsSingle();
            Container.BindInterfacesTo<PlayerMovementAnimation>().AsSingle();

            Container.Bind<ToggleableManager>().AsSingle();
        }
    }
}