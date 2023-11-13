using UnityEngine;

namespace Infrastructure.Data.Static
{
    [CreateAssetMenu(fileName = "GamePrefabs", menuName = "ScriptableObjects/Static/GamePrefabs")]
    public class GamePrefabs : ScriptableObject
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject _player;
        [SerializeField] private GameObject _camera;

        [Header("Platforms")]
        [SerializeField] private GameObject _helicopterPlatform;
        [SerializeField] private GameObject _oilPlatform;
        [SerializeField] private GameObject _dumpPlatform;

        [Header("Other")]
        [SerializeField] private GameObject _fuelBarrel;

        [Header("Resources")]
        [SerializeField] private GameObject _gear;

        public GameObject Player => _player;
        public GameObject Camera => _camera;

        public GameObject HelicopterPlatform => _helicopterPlatform;
        public GameObject OilPlatform => _oilPlatform;
        public GameObject DumpPlatform => _dumpPlatform;

        public GameObject FuelBarrel => _fuelBarrel;

        public GameObject Gear => _gear;
    }
}