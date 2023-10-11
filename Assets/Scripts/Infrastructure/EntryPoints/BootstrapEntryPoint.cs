using Infrastructure.Curtain.Core;
using Infrastructure.EntryPoints.Core;
using Infrastructure.SceneManagement.Core;
using Infrastructure.Services.RuntimeData.Core;
using Infrastructure.Services.SaveLoadHandler.Core;
using Infrastructure.Services.StaticData.Core;
using UnityEngine;
using Zenject;

namespace Infrastructure.EntryPoints
{
    public class BootstrapEntryPoint : MonoBehaviour, IEntryPoint
    {
        private ISceneLoader _sceneLoader;
        private IStaticDataService _staticDataService;
        private ILoadingScreen _loadingScreen;
        private ISaveLoadHandlerService _saveLoadHandlerService;
        private IRuntimeDataService _runtimeDataService;

        [Inject]
        private void Constructor(ISceneLoader sceneLoader,
            IStaticDataService staticDataService,
            ILoadingScreen loadingScreen,
            ISaveLoadHandlerService saveLoadHandlerService,
            IRuntimeDataService runtimeDataService)
        {
            _sceneLoader = sceneLoader;
            _staticDataService = staticDataService;
            _loadingScreen = loadingScreen;
            _saveLoadHandlerService = saveLoadHandlerService;
            _runtimeDataService = runtimeDataService;
        }

        #region MonoBehaviour

        private void Start()
        {
            Enter();
        }

        #endregion

        public void Enter()
        {
            DisableScreenSleep();
            LoadData();
            UpdateAnalyticSessionsCount();
            LoadScene();
        }

        private void LoadScene()
        {
            _sceneLoader.LoadAsync(_staticDataService.Config.MainScene, _loadingScreen.Hide);
        }

        private void DisableScreenSleep()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        private void LoadData()
        {
            _saveLoadHandlerService.Load();
        }

        private void UpdateAnalyticSessionsCount()
        {
            _runtimeDataService.RuntimeData.AnalyticsData.SessionsCount++;
        }
    }
}
