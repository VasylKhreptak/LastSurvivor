using Cinemachine;
using Data.Persistent.Platforms;
using EntryPoints.Core;
using Infrastructure.Data.Static;
using Infrastructure.Data.Static.Core;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.StaticData.Core;
using Main.Entities.Player;
using UnityEngine;
using Zenject;

namespace EntryPoints
{
    public class MainEntryPoint : MonoBehaviour, IEntryPoint
    {
        private DiContainer _container;
        private GamePrefabs _prefabs;
        private PlatformsData _platformsData;

        [Inject]
        private void Constructor(DiContainer container, IStaticDataService staticDataService,
            IPersistentDataService persistentDataService)
        {
            _container = container;
            _prefabs = staticDataService.Prefabs;
            _platformsData = persistentDataService.Data.PlayerData.PlatformsData;
        }

        #region MonoBehaviour

        private void Start() => Enter();

        #endregion

        public void Enter()
        {
            Transform playerTransform = InitializePlayer();
            InitializeCamera(playerTransform);
            SpawnPlatforms();
        }

        private Transform InitializePlayer()
        {
            GameObject playerObject = _container.InstantiatePrefab(_prefabs[Prefab.MainPlayer]);
            _container.Bind<Player>().FromComponentOn(playerObject).AsSingle();
            return playerObject.transform;
        }

        private void InitializeCamera(Transform target)
        {
            GameObject cameraRoot = _container.InstantiatePrefab(_prefabs[Prefab.Camera]);
            CinemachineVirtualCamera virtualCamera = cameraRoot.GetComponentInChildren<CinemachineVirtualCamera>(true);
            virtualCamera.Follow = target;
            virtualCamera.LookAt = target;

            Camera camera = cameraRoot.GetComponentInChildren<Camera>(true);
            _container.BindInstance(camera).AsSingle();
        }

        private void SpawnPlatforms()
        {
            SpawnHelicopterPlatform();
            SpawnOilPlatform();
            SpawnDumpPlatform();
            SpawnBarracksPlatform();
            SpawnCollectorsPlatform();
        }

        private void SpawnHelicopterPlatform() => _container.InstantiatePrefab(_prefabs[Prefab.HelicopterPlatform]);

        private void SpawnOilPlatform() => _container.InstantiatePrefab(_prefabs[Prefab.OilPlatform]);

        private void SpawnDumpPlatform()
        {
            Prefab prefab = _platformsData.DumpPlatformData.BuyContainer.IsFull.Value
                ? Prefab.DumpPlatform
                : Prefab.DumpBuyZone;
            _container.InstantiatePrefab(_prefabs[prefab]);
        }

        private void SpawnBarracksPlatform()
        {
            Prefab prefab = _platformsData.BarracksPlatformData.BuyContainer.IsFull.Value
                ? Prefab.BarracksPlatform
                : Prefab.BarracksBuyZone;
            _container.InstantiatePrefab(_prefabs[prefab]);
        }

        private void SpawnCollectorsPlatform()
        {
            Prefab prefab = _platformsData.CollectorsPlatformData.BuyContainer.IsFull.Value
                ? Prefab.CollectorsPlatform
                : Prefab.CollectorsBuyZone;
            _container.InstantiatePrefab(_prefabs[prefab]);
        }
    }
}