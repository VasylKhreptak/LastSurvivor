using System;
using UnityEngine;

namespace Data.Scenes.Main
{
    [Serializable]
    public class MainSceneData
    {
        [Header("References")]
        [SerializeField] private Transform _playerSpawnTransform;

        public Transform PlayerSpawnTransform => _playerSpawnTransform;
    }
}