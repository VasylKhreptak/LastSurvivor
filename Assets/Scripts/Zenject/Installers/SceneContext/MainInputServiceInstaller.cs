using Infrastructure.Services.Input.Main;
using Infrastructure.Services.Input.Main.Core;
using UnityEngine;

namespace Zenject.Installers.SceneContext
{
    public class MainInputServiceInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private MainInputService _mainInputService;

        public override void InstallBindings()
        {
            Container.BindInstance(_mainInputService as IMainInputService).AsSingle();
        }
    }
}