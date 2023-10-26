using Data.Static.Balance;
using UnityEngine;

namespace Infrastructure.Data.Static
{
    [CreateAssetMenu(fileName = "GameBalance", menuName = "ScriptableObjects/Static/GameBalance")]
    public class GameBalance : ScriptableObject
    {
        [Header("Balance")]
        [SerializeField] private PlayerPreferences _playerPreferences;
        [SerializeField] private TransformLookerPreferences _transformLookerPreferences;

        public PlayerPreferences PlayerPreferences => _playerPreferences;
        public TransformLookerPreferences TransformLookerPreferences => _transformLookerPreferences;
    }
}