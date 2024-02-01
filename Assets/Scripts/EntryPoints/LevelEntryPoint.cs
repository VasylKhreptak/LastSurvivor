using EntryPoints.Core;
using Gameplay.Entities.Helicopter;
using Gameplay.Entities.Platoon;
using Gameplay.Entities.Player;
using Gameplay.Entities.Soldier;
using Gameplay.Entities.Soldier.StateMachine.States;
using Infrastructure.Data.Static;
using Infrastructure.Data.Static.Core;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.StaticData.Core;
using UnityEngine;
using Zenject;

namespace EntryPoints
{
    public class LevelEntryPoint : MonoBehaviour, IEntryPoint
    {
        private DiContainer _container;
        private PlayerHolder _playerHolder;
        private GamePrefabs _gamePrefabs;
        private Helicopter _helicopter;
        private Platoon _platoon;
        private IPersistentDataService _persistentDataService;

        [Inject]
        private void Constructor(DiContainer container, PlayerHolder playerHolder, IStaticDataService staticDataService,
            Helicopter helicopter, Platoon platoon, IPersistentDataService persistentDataService)
        {
            _container = container;
            _playerHolder = playerHolder;
            _gamePrefabs = staticDataService.Prefabs;
            _helicopter = helicopter;
            _platoon = platoon;
            _persistentDataService = persistentDataService;
        }

        #region MonoBehaviour

        private void Start() => Enter();

        #endregion

        public void Enter()
        {
            InitializePlayer();
            InitializePlatoon();
            InitializeHelicopterMovement();
        }

        private void InitializePlayer()
        {
            GameObject playerObject = _container.InstantiatePrefab(_gamePrefabs[Prefab.GameplayPlayer]);
            Player player = playerObject.GetComponent<Player>();
            _playerHolder.Instance = player;
        }

        private void InitializeHelicopterMovement()
        {
            _helicopter.TargetFollower.Target = _playerHolder.Instance.transform;
            _helicopter.TargetFollower.FollowTargetImmediately();
        }

        private void InitializePlatoon()
        {
            _platoon.TargetFollower.Target = _playerHolder.Instance.transform;
            _platoon.TargetFollower.FollowTargetImmediately();
            InitializeSoldiers();
        }

        private void InitializeSoldiers()
        {
            int count = Mathf.Min(
                _persistentDataService.PersistentData.PlayerData.PlatformsData.BarracksPlatformData.SoldiersBank.Value.Value,
                _platoon.SoldierPoints.Count);

            for (int i = 0; i < count; i++)
            {
                GameObject soldierObject = _container.InstantiatePrefab(_gamePrefabs[Prefab.GameplaySoldier]);
                soldierObject.transform.position = _platoon.SoldierPoints[i].position;
                soldierObject.transform.rotation = _platoon.SoldierPoints[i].rotation;
                Soldier soldier = soldierObject.GetComponent<Soldier>();
                soldier.StateMachine.Enter<FollowTransformState, Transform>(_platoon.SoldierPoints[i]);
            }
        }
    }
}