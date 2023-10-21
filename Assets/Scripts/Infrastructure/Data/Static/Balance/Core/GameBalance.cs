using UnityEngine;

namespace Infrastructure.Data.Static.Balance.Core
{
    [CreateAssetMenu(fileName = "GameBalance", menuName = "ScriptableObjects/Static/GameBalance")]
    public class GameBalance : ScriptableObject
    {
        [Header("Balance")]
        [SerializeField] private PlayerPreferences _playerPreferences;

        public PlayerPreferences PlayerPreferences => _playerPreferences;
    }
}