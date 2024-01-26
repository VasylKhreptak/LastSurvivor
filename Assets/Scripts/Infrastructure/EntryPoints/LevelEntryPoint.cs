using Gameplay.Entities.Player;
using Infrastructure.Data.Static;
using Infrastructure.Data.Static.Core;
using Infrastructure.EntryPoints.Core;
using Infrastructure.Services.StaticData.Core;
using UnityEngine;
using Zenject;

namespace Infrastructure.EntryPoints
{
    public class LevelEntryPoint : MonoBehaviour, IEntryPoint
    {
        private DiContainer _container;
        private PlayerHolder _playerHolder;
        private GamePrefabs _gamePrefabs;

        [Inject]
        private void Constructor(DiContainer container, PlayerHolder playerHolder, IStaticDataService staticDataService)
        {
            _container = container;
            _playerHolder = playerHolder;
            _gamePrefabs = staticDataService.Prefabs;
        }

        #region MonoBehaviour

        private void Start() => Enter();

        #endregion

        public void Enter()
        {
            InitializePlayer();
        }

        private void InitializePlayer()
        {
            GameObject playerObject = _container.InstantiatePrefab(_gamePrefabs[Prefab.GameplayPlayer]);
            Player player = playerObject.GetComponent<Player>();
            _playerHolder.Instance = player;
        }
    }
}