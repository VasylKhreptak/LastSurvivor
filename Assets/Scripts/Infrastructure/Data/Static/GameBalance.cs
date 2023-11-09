using Data.Static.Balance;
using Data.Static.Balance.Animations.Core;
using UnityEngine;

namespace Infrastructure.Data.Static
{
    [CreateAssetMenu(fileName = "GameBalance", menuName = "ScriptableObjects/Static/GameBalance")]
    public class GameBalance : ScriptableObject
    {
        [Header("Balance")]
        [SerializeField] private PlayerPreferences _playerPreferences;
        [SerializeField] private AnimationPreferences _animationPreferences;
        [SerializeField] private HelicopterUpgradePreferences _helicopterUpgradePreferences;

        public PlayerPreferences PlayerPreferences => _playerPreferences;
        public AnimationPreferences AnimationPreferences => _animationPreferences;
        public HelicopterUpgradePreferences HelicopterUpgradePreferences => _helicopterUpgradePreferences;
    }
}