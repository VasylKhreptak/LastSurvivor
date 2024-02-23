using Cinemachine;
using Data.Persistent.Platforms;
using Infrastructure.Data.Static;
using Infrastructure.Data.Static.Core;
using Infrastructure.Services.Input.Main;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.StaticData.Core;
using Main.Entities.Player;
using Main.Platforms.BarracksPlatform;
using Main.Platforms.BuyZones;
using Main.Platforms.CollectorsPlatform;
using Main.Platforms.DumpPlatform;
using Main.Platforms.HelicopterPlatform;
using Main.Platforms.OilPlatform;
using UnityEngine;
using Zenject;

namespace Main
{
    public class MainInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private Joystick _joystick;

        private GamePrefabs _prefabs;
        private PlatformsData _platformsData;

        [Inject]
        private void Constructor(IStaticDataService staticDataService, IPersistentDataService persistentDataService)
        {
            _prefabs = staticDataService.Prefabs;
            _platformsData = persistentDataService.Data.PlayerData.PlatformsData;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _joystick ??= FindObjectOfType<Joystick>();
        }

        #endregion

        public override void InstallBindings()
        {
            BindInputService();
            BindPlayer();
            BindCamera();
            BindPlatforms();
        }

        private void BindInputService() => Container.BindInterfacesTo<MainInputService>().AsSingle().WithArguments(_joystick);

        private void BindPlayer()
        {
            GameObject playerObject = Container.InstantiatePrefab(_prefabs[Prefab.MainPlayer]);
            Container.Bind<Player>().FromComponentOn(playerObject).AsSingle();
        }

        private void BindCamera()
        {
            Transform playerTransform = Container.Resolve<Player>().transform;
            GameObject cameraRoot = Container.InstantiatePrefab(_prefabs[Prefab.Camera]);
            CinemachineVirtualCamera virtualCamera = cameraRoot.GetComponentInChildren<CinemachineVirtualCamera>(true);
            virtualCamera.Follow = playerTransform;
            virtualCamera.LookAt = playerTransform;

            Camera camera = cameraRoot.GetComponentInChildren<Camera>();
            Container.BindInstance(camera).AsSingle();
        }

        private void BindPlatforms()
        {
            BindHelicopterPlatform();
            BindOilPlatform();
            BindDumpPlatformOrBuyZone();
            BindBarracksPlatformOrBuyZone();
            BindCollectorsPlatformOrBuyZone();
        }

        private void BindHelicopterPlatform() => BindComponentInNewPrefabNonLazy<HelicopterPlatform>(Prefab.HelicopterPlatform);

        private void BindOilPlatform() => BindComponentInNewPrefabNonLazy<OilPlatform>(Prefab.OilPlatform);

        private void BindDumpPlatformOrBuyZone()
        {
            if (_platformsData.DumpPlatformData.BuyContainer.IsFull.Value)
                BindComponentInNewPrefabNonLazy<DumpPlatform>(Prefab.DumpPlatform);
            else
                BindComponentInNewPrefabNonLazy<DumpBuyZone>(Prefab.DumpBuyZone);
        }

        private void BindBarracksPlatformOrBuyZone()
        {
            if (_platformsData.BarracksPlatformData.BuyContainer.IsFull.Value)
                BindComponentInNewPrefabNonLazy<BarracksPlatform>(Prefab.BarracksPlatform);
            else
                BindComponentInNewPrefabNonLazy<BarracksBuyZone>(Prefab.BarracksBuyZone);
        }

        private void BindCollectorsPlatformOrBuyZone()
        {
            if (_platformsData.CollectorsPlatformData.BuyContainer.IsFull.Value)
                BindComponentInNewPrefabNonLazy<CollectorsPlatform>(Prefab.CollectorsPlatform);
            else
                BindComponentInNewPrefabNonLazy<CollectorsBuyZone>(Prefab.CollectorsBuyZone);
        }

        private void BindComponentInNewPrefabNonLazy<T>(Prefab prefab)
        {
            Container
                .Bind<T>()
                .FromComponentInNewPrefab(_prefabs[prefab])
                .AsSingle()
                .NonLazy();
        }
    }
}