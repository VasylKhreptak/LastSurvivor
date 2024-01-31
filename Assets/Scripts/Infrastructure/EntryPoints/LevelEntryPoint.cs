using Gameplay.Entities.Player;
using Gameplay.Entities.Soldier;
using Gameplay.Entities.Soldier.StateMachine.States;
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
            InitializeSoldiers();
        }

        private void InitializePlayer()
        {
            GameObject playerObject = _container.InstantiatePrefab(_gamePrefabs[Prefab.GameplayPlayer]);
            Player player = playerObject.GetComponent<Player>();
            _playerHolder.Instance = player;
        }

        private void InitializeSoldiers()
        {
            Player player = _playerHolder.Instance;

            for (int i = 0; i < player.SoldierFollowPoints.Count; i++)
            {
                Transform followPoint = player.SoldierFollowPoints[i];
                GameObject soldierObject = _container.InstantiatePrefab(_gamePrefabs[Prefab.GameplaySoldier]);
                soldierObject.transform.position = followPoint.position;
                soldierObject.transform.rotation = followPoint.rotation;
                Soldier soldier = soldierObject.GetComponent<Soldier>();
                soldier.StateMachine.Enter<FollowTransformState, Transform>(followPoint);
            }
        }
    }
}