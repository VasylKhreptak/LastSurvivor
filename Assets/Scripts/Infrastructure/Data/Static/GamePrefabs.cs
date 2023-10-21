using UnityEngine;

namespace Infrastructure.Data.Static
{
    [CreateAssetMenu(fileName = "GamePrefabs", menuName = "ScriptableObjects/Static/GamePrefabs")]
    public class GamePrefabs : ScriptableObject
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject _player;
        [SerializeField] private GameObject _camera;

        public GameObject Player => _player;
        public GameObject Camera => _camera;
    }
}