using Cinemachine;
using Data.Scenes.Main;
using Entities.Player;
using Holders.Core;
using Infrastructure.Data.Static;
using Infrastructure.EntryPoints.Core;
using Infrastructure.Services.StaticData.Core;
using UnityEngine;
using Zenject;

namespace Infrastructure.EntryPoints
{
    public class MainEntryPoint : MonoBehaviour, IEntryPoint
    {
        private IInstantiator _instantiator;
        private GamePrefabs _prefabs;
        private MainSceneData _mainSceneData;
        private Holder<Player> _playerHolder;

        [Inject]
        private void Constructor(IInstantiator instantiator, IStaticDataService staticDataService, MainSceneData mainSceneData,
            Holder<Player> playerHolder)
        {
            _instantiator = instantiator;
            _prefabs = staticDataService.Prefabs;
            _mainSceneData = mainSceneData;
            _playerHolder = playerHolder;
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
        }

        private Transform InitializePlayer()
        {
            Transform player = _instantiator.InstantiatePrefab(_prefabs.Player).transform;
            Transform playerSpawnTransform = _mainSceneData.PlayerSpawnTransform;
            player.position = playerSpawnTransform.position;
            player.rotation = playerSpawnTransform.rotation;
            
            _playerHolder.Value = player.GetComponent<Player>();
            
            return player;
        }

        private void InitializeCamera(Transform target)
        {
            GameObject cameraRoot = _instantiator.InstantiatePrefab(_prefabs.Camera);
            CinemachineVirtualCamera virtualCamera = cameraRoot.GetComponentInChildren<CinemachineVirtualCamera>(true);
            virtualCamera.Follow = target;
            virtualCamera.LookAt = target;
        }
    }
}