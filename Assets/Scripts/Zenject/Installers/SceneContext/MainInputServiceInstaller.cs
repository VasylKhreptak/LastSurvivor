using Infrastructure.Services.Input.Main.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Zenject.Installers.SceneContext
{
    public class MainInputServiceInstaller : MonoInstaller
    {
        [Header("References")]
        [ShowInInspector] private IMainInputService _mainInputService;

        public override void InstallBindings()
        {
            Container.BindInstance(_mainInputService).AsSingle();
        }
    }
}