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
        [Header("References")]
        [SerializeField] private Transform _playerSpawnPoint;

        private IInstantiator _instantiator;
        private GamePrefabs _prefabs;

        [Inject]
        private void Constructor(IInstantiator instantiator, IStaticDataService staticDataService)
        {
            _instantiator = instantiator;
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
        }

        private Transform InitializePlayer()
        {
            Transform player = _instantiator.InstantiatePrefab(_prefabs.Player).transform;
            player.position = _playerSpawnPoint.position;
            player.rotation = _playerSpawnPoint.rotation;
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