using System;
using System.Collections.Generic;
using DebuggerOptions;
using Gameplay.Aim;
using Gameplay.Data;
using Gameplay.Entities.Collector;
using Gameplay.Entities.Helicopter;
using Gameplay.Entities.Platoon;
using Gameplay.Entities.Player;
using Gameplay.Entities.Zombie;
using Gameplay.Levels;
using Gameplay.Levels.Analytics;
using Gameplay.Levels.StateMachine;
using Gameplay.Levels.StateMachine.States;
using Gameplay.Levels.StateMachine.States.Core;
using Gameplay.Levels.ZombieSpawner;
using Gameplay.Waypoints;
using Gameplay.Weapons;
using Gameplay.Weapons.Core;
using Infrastructure.Data.Static;
using Infrastructure.Data.Static.Core;
using Infrastructure.Services.Advertisement.Core;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.StaticData.Core;
using ObjectPoolSystem.PoolCategories;
using Plugins.ObjectPoolSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Splines;
using Utilities.CameraUtilities.Shaker;

namespace Zenject.Installers.SceneContext.Gameplay
{
    public class LevelInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private Trackpad _trackpad;
        [SerializeField] private Transform _playerSpawnPoint;

        [Header("Preferences")]
        [SerializeField] private CameraShaker.Preferences _cameraShakerPreferences;
        [SerializeField] private Transform[] _playerWaypoints;

        [Header("Object Pool Preferences")]
        [SerializeField] private ObjectPoolPreference<Particle>[] _particlePoolsPreferences;
        [SerializeField] private ObjectPoolPreference<GeneralPool>[] _generalPoolsPreferences;

        [Header("Weapons")]
        [SerializeField] private WeaponAimer.Preferences _weaponAimPreferences;

        [Header("Splines")]
        [SerializeField] private SplineContainer _helicopterMovementSpline;
        [SerializeField] private SplineContainer _platoonMovementSpline;

        private GamePrefabs _gamePrefabs;
        private IPersistentDataService _persistentDataService;
        private IAdvertisementService _advertisementService;

        [Inject]
        private void Constructor(IStaticDataService staticDataService, IPersistentDataService persistentDataService,
            IAdvertisementService advertisementService)
        {
            _gamePrefabs = staticDataService.Prefabs;
            _persistentDataService = persistentDataService;
            _advertisementService = advertisementService;
        }

        #region MonoBehaviour

        [Button("Validate")]
        private void OnValidate()
        {
            _trackpad ??= FindObjectOfType<Trackpad>(true);
        }

        #endregion

        public override void InstallBindings()
        {
            Container.BindInstance(_trackpad).AsSingle();
            Container.BindInstance(_helicopterMovementSpline).WhenInjectedInto<HelicopterInstaller>();
            Container.BindInstance(_platoonMovementSpline).WhenInjectedInto<PlatoonInstaller>();

            BindLevelManager();
            BindObjectPools();
            BindZombiesList();
            BindCollectorsList();
            GameObject helicopterObject = CreateHelicopter();
            BindWeapon(helicopterObject);
            BindCamera(helicopterObject);
            BindCameraShaker();
            BindPlayerWaypoints();
            BindPlayer();
            BindHelicopter(helicopterObject);
            BindPlatoon();
            InitializeCollectors();
            BindZombieSpawnerList();
            BindLevelData();
            BindWeaponAimer();
            BindWeaponShooter();
            BindLevelStateMachine();
            BindLevelDebugger();
            PreloadVideoAd();
            BindLevelDurationEventLogger();
        }

        private void BindLevelManager() => Container.Bind<LevelManager>().AsSingle();

        private void BindCameraShaker() =>
            Container.BindInterfacesAndSelfTo<CameraShaker>().AsSingle().WithArguments(_cameraShakerPreferences);

        private void BindPlayerWaypoints()
        {
            Container
                .Bind<Waypoints>()
                .AsSingle()
                .WithArguments(_playerWaypoints)
                .WhenInjectedInto<PlayerInstaller>();
        }

        private void BindPlayer()
        {
            GameObject playerObject = Container.InstantiatePrefab(_gamePrefabs[Prefab.GameplayPlayer], _playerSpawnPoint.position,
                _playerSpawnPoint.rotation, null);
            Container.Bind<Player>().FromComponentOn(playerObject).AsSingle();
        }

        private void BindPlatoon()
        {
            Container.Bind<Platoon>().FromComponentInNewPrefab(_gamePrefabs[Prefab.Platoon]).AsSingle();
            Platoon platoon = Container.Resolve<Platoon>();
            platoon.TargetFollower.Target = Container.Resolve<Player>().transform;
            platoon.TargetFollower.FollowTargetImmediately();
            InitializeSoldiers();
        }

        private GameObject CreateHelicopter() => Container.InstantiatePrefab(_gamePrefabs[Prefab.GameplayHelicopter]);

        private void BindWeapon(GameObject helicopterObject) =>
            Container.BindInstance(helicopterObject.GetComponentInChildren<IWeapon>()).AsSingle();

        private void BindCamera(GameObject helicopterObject)
        {
            Camera camera = helicopterObject.GetComponentInChildren<Camera>(true);
            Container.BindInstance(camera).AsSingle();
        }

        private void BindHelicopter(GameObject helicopterObject)
        {
            Helicopter helicopter = helicopterObject.GetComponent<Helicopter>();
            Container.BindInstance(helicopter).AsSingle();
            helicopter.TargetFollower.Target = Container.Resolve<Player>().transform;
            helicopter.TargetFollower.FollowTargetImmediately();
        }

        private void InitializeSoldiers()
        {
            Platoon platoon = Container.Resolve<Platoon>();

            int count = Mathf.Min(_persistentDataService.Data.PlayerData.PlatformsData.BarracksPlatformData.SoldiersBank.Value.Value,
                platoon.SoldierPoints.Count);

            for (int i = 0; i < count; i++)
            {
                GameObject soldierObject = Container.InstantiatePrefab(_gamePrefabs[Prefab.GameplaySoldier]);
                Rigidbody rigidbody = soldierObject.GetComponent<Rigidbody>();
                rigidbody.position = platoon.SoldierPoints[i].position;
                rigidbody.rotation = platoon.SoldierPoints[i].rotation;
            }
        }

        private void InitializeCollectors()
        {
            Player player = Container.Resolve<Player>();

            int count = Mathf.Min(_persistentDataService.Data.PlayerData.PlatformsData.CollectorsPlatformData.CollectorsBank.Value
                .Value, player.CollectorFollowPoints.Count);

            for (int i = 0; i < count; i++)
            {
                GameObject collectorObject = Container.InstantiatePrefab(_gamePrefabs[Prefab.GameplayCollector]);
                Rigidbody rigidbody = collectorObject.GetComponent<Rigidbody>();
                rigidbody.position = player.CollectorFollowPoints[i].position;
                rigidbody.rotation = player.CollectorFollowPoints[i].rotation;
            }
        }

        private void BindObjectPools()
        {
            BindObjectPools(_particlePoolsPreferences, new GameObject("Particles").transform);
            BindObjectPools(_generalPoolsPreferences, new GameObject("General Pool").transform);
        }

        private void BindObjectPools<T>(ObjectPoolPreference<T>[] poolPreferences, Transform parent) where T : Enum
        {
            foreach (ObjectPoolPreference<T> preference in poolPreferences)
            {
                preference.CreateFunc = () => Container.InstantiatePrefab(preference.Prefab, parent);
            }

            ObjectPools<T> objectPools = new ObjectPools<T>(poolPreferences);

            Container.Bind<IObjectPools<T>>().FromInstance(objectPools).AsSingle();
        }

        private void BindLevelStateMachine()
        {
            BindLevelStates();
            Container.Bind<LevelStateFactory>().AsSingle();
            Container.BindInterfacesTo<LevelStateMachine>().AsSingle();
        }

        private void BindLevelStates()
        {
            Container.Bind<LevelStartState>().AsSingle();
            Container.Bind<LevelCompletedState>().AsSingle();
            Container.Bind<PauseLevelState>().AsSingle();
            Container.Bind<ResumeLevelState>().AsSingle();
            Container.Bind<LevelFailedState>().AsSingle();
            Container.Bind<LevelLoopState>().AsSingle();
            Container.Bind<FinalizeProgressAndLoadMenuState>().AsSingle();
        }

        private void BindZombiesList() => Container.Bind<List<Zombie>>().AsSingle();

        private void BindCollectorsList() => Container.Bind<List<Collector>>().AsSingle();

        private void BindZombieSpawnerList() => Container.Bind<List<ZombieSpawner>>().AsSingle();

        private void BindLevelData() => Container.Bind<LevelData>().AsSingle();

        private void BindWeaponAimer() =>
            Container.BindInterfacesAndSelfTo<WeaponAimer>().AsSingle().WithArguments(_weaponAimPreferences);

        private void BindWeaponShooter() => Container.BindInterfacesAndSelfTo<WeaponShooter>().AsSingle();

        private void BindLevelDebugger() => Container.BindInterfacesTo<LevelOptions>().AsSingle();

        private void PreloadVideoAd() => _advertisementService.LoadRewardedVideo();

        private void BindLevelDurationEventLogger() => Container.BindInterfacesTo<LevelLifetimeEventLogger>().AsSingle();
    }
}