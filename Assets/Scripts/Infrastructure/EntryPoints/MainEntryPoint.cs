using Cinemachine;
using Infrastructure.EntryPoints.Core;
using UnityEngine;
using Zenject;

namespace Infrastructure.EntryPoints
{
    public class MainEntryPoint : MonoBehaviour, IEntryPoint
    {
        [Header("References")]
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _cameraPrefab;

        private IInstantiator _instantiator;

        [Inject]
        private void Constructor(IInstantiator instantiator)
        {
            _instantiator = instantiator;
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
            Transform player = _instantiator.InstantiatePrefab(_playerPrefab).transform;
            player.position = _playerSpawnPoint.position;
            player.rotation = _playerSpawnPoint.rotation;
            return player;
        }

        private void InitializeCamera(Transform target)
        {
            GameObject cameraRoot = _instantiator.InstantiatePrefab(_cameraPrefab);
            CinemachineVirtualCamera virtualCamera = cameraRoot.GetComponentInChildren<CinemachineVirtualCamera>(true);
            virtualCamera.Follow = target;
            virtualCamera.LookAt = target;
        }
    }
}