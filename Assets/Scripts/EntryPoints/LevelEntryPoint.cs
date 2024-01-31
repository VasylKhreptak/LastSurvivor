using EntryPoints.Core;
using Gameplay.Entities.Helicopter;
using Gameplay.Entities.Player;
using Infrastructure.Data.Static;
using Infrastructure.Data.Static.Core;
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

        [Inject]
        private void Constructor(DiContainer container, PlayerHolder playerHolder, IStaticDataService staticDataService,
            Helicopter helicopter)
        {
            _container = container;
            _playerHolder = playerHolder;
            _gamePrefabs = staticDataService.Prefabs;
            _helicopter = helicopter;
        }

        #region MonoBehaviour

        private void Start() => Enter();

        #endregion

        public void Enter()
        {
            InitializePlayer();
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
    }
}