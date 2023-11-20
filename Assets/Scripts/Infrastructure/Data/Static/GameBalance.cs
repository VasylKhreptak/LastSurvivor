using Data.Static.Balance;
using Data.Static.Balance.Animations.Core;
using Data.Static.Balance.Upgrade;
using UnityEngine;

namespace Infrastructure.Data.Static
{
    [CreateAssetMenu(fileName = "GameBalance", menuName = "ScriptableObjects/Static/GameBalance")]
    public class GameBalance : ScriptableObject
    {
        [Header("Balance")]
        [SerializeField] private PlayerPreferences _playerPreferences;
        [SerializeField] private AnimationPreferences _animationPreferences;

        [Header("Platforms Upgrade Preferences")]
        [SerializeField] private HelicopterPlatformUpgradePreferences _helicopterPlatformUpgradePreferences;
        [SerializeField] private OilPlatformUpgradePreferences _oilPlatformUpgradePreferences;

        [Space]
        [SerializeField] private DumpWorkerPreferences _dumpWorkerPreferences;

        public PlayerPreferences PlayerPreferences => _playerPreferences;
        public AnimationPreferences AnimationPreferences => _animationPreferences;

        public HelicopterPlatformUpgradePreferences HelicopterPlatformUpgradePreferences => _helicopterPlatformUpgradePreferences;
        public OilPlatformUpgradePreferences OilPlatformUpgradePreferences => _oilPlatformUpgradePreferences;

        public DumpWorkerPreferences DumpWorkerPreferences => _dumpWorkerPreferences;
    }
}