namespace EntryPoints
{
    public class LevelEntryPoint // : IEntryPoint
    {
        // private readonly Transform _playerSpawnPoint;
        // private readonly DiContainer _container;
        // private readonly PlayerHolder _playerHolder;
        // private readonly GamePrefabs _gamePrefabs;
        // private readonly Helicopter _helicopter;
        // private readonly Platoon _platoon;
        // private readonly IPersistentDataService _persistentDataService;
        //
        // private LevelEntryPoint(Transform playerSpawnPoint, DiContainer container, PlayerHolder playerHolder, IStaticDataService staticDataService,
        //     Helicopter helicopter, Platoon platoon, IPersistentDataService persistentDataService)
        // {
        //     _playerSpawnPoint = playerSpawnPoint;
        //     _container = container;
        //     _playerHolder = playerHolder;
        //     _gamePrefabs = staticDataService.Prefabs;
        //     _helicopter = helicopter;
        //     _platoon = platoon;
        //     _persistentDataService = persistentDataService;
        // }
        //
        // public void Enter()
        // {
        //     InitializePlayer();
        //     InitializePlatoon();
        //     InitializeCollectors();
        //     InitializeHelicopterMovement();
        // }
        //
        // private void InitializePlayer()
        // {
        //     GameObject playerObject = _container.InstantiatePrefab(_gamePrefabs[Prefab.GameplayPlayer], _playerSpawnPoint.position,
        //         _playerSpawnPoint.rotation, null);
        //     playerObject.transform.SetParent(null, true);
        //     Player player = playerObject.GetComponent<Player>();
        //     _playerHolder.Instance = player;
        // }
        //
        // private void InitializeHelicopterMovement()
        // {
        //     _helicopter.TargetFollower.Target = _playerHolder.Instance.transform;
        //     _helicopter.TargetFollower.FollowTargetImmediately();
        // }
        //
        // private void InitializePlatoon()
        // {
        //     Debug.Log(_platoon.TargetFollower == null);
        //     _platoon.TargetFollower.Target = _playerHolder.Instance.transform;
        //     _platoon.TargetFollower.FollowTargetImmediately();
        //     InitializeSoldiers();
        // }
        //
        // private void InitializeSoldiers()
        // {
        //     int count = Mathf.Min(_persistentDataService.Data.PlayerData.PlatformsData.BarracksPlatformData.SoldiersBank.Value.Value,
        //         _platoon.SoldierPoints.Count);
        //
        //     for (int i = 0; i < count; i++)
        //     {
        //         GameObject soldierObject = _container.InstantiatePrefab(_gamePrefabs[Prefab.GameplaySoldier]);
        //         Rigidbody rigidbody = soldierObject.GetComponent<Rigidbody>();
        //         rigidbody.position = _platoon.SoldierPoints[i].position;
        //         rigidbody.rotation = _platoon.SoldierPoints[i].rotation;
        //     }
        // }
        //
        // private void InitializeCollectors()
        // {
        //     int count = Mathf.Min(
        //         _persistentDataService.Data.PlayerData.PlatformsData.CollectorsPlatformData.CollectorsBank.Value.Value,
        //         _playerHolder.Instance.CollectorFollowPoints.Count);
        //
        //     for (int i = 0; i < count; i++)
        //     {
        //         GameObject collectorObject = _container.InstantiatePrefab(_gamePrefabs[Prefab.GameplayCollector]);
        //         Rigidbody rigidbody = collectorObject.GetComponent<Rigidbody>();
        //         rigidbody.position = _playerHolder.Instance.CollectorFollowPoints[i].position;
        //         rigidbody.rotation = _playerHolder.Instance.CollectorFollowPoints[i].rotation;
        //     }
        // }
    }
}