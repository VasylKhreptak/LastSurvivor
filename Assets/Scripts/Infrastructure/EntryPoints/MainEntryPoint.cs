using Cinemachine;
using Infrastructure.Data.Static;
using Infrastructure.EntryPoints.Core;
using Infrastructure.Services.StaticData.Core;
using UnityEngine;
using Zenject;

namespace Infrastructure.EntryPoints
{
    public class MainEntryPoint : MonoBehaviour, IEntryPoint
    {
        private DiContainer _container;
        private GamePrefabs _prefabs;

        [Inject]
        private void Constructor(DiContainer container, IStaticDataService staticDataService)
        {
            _container = container;
            _prefabs = staticDataService.Prefabs;
        }

        #region MonoBehaviour

        private void Start()
        {
            Enter();
        }

        #endregion

        public void Enter()
        {
            Transform player = InitializePlayer();
            InitializeCamera(player);
            InitializePlatforms();
        }

        private Transform InitializePlayer()
        {
            Transform player = _container.InstantiatePrefab(_prefabs.Player).transform;
            return player;
        }

        private void InitializeCamera(Transform target)
        {
            GameObject cameraRoot = _container.InstantiatePrefab(_prefabs.Camera);
            CinemachineVirtualCamera virtualCamera = cameraRoot.GetComponentInChildren<CinemachineVirtualCamera>(true);
            virtualCamera.Follow = target;
            virtualCamera.LookAt = target;

            Camera camera = cameraRoot.GetComponentInChildren<Camera>(true);
            _container.BindInstance(camera).AsSingle();
        }

        private void InitializePlatforms()
        {
            InitializeHelicopterPlatform();
        }

        private void InitializeHelicopterPlatform()
        {
            _container.InstantiatePrefab(_prefabs.HelicopterPlatform);
        }
    }
}