using Data.Scenes.Main;
using UnityEngine;

namespace Zenject.Installers.SceneContext.Data
{
    public class MainSceneDataInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private MainSceneData _mainSceneData;

        public override void InstallBindings()
        {
            Container.BindInstance(_mainSceneData).AsSingle();
        }
    }
}