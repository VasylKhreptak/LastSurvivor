using Balance;
using Data.Static.Balance;
using Data.Static.Balance.Animations.Core;
using Data.Static.Balance.Platforms;
using UnityEngine;

namespace Infrastructure.Data.Static
{
    [CreateAssetMenu(fileName = "GameBalance", menuName = "ScriptableObjects/Static/GameBalance")]
    public class GameBalance : ScriptableObject
    {
        [SerializeField] private PlayerPreferences _playerPreferences;
        [SerializeField] private AnimationPreferences _animationPreferences;
        [SerializeField] private HelicopterPlatformPreferences _helicopterPlatformPreferences;
        [SerializeField] private OilPlatformPreferences _oilPlatformPreferences;
        [SerializeField] private DumpPlatformPreferences _dumpPlatformPreferences;
        [SerializeField] private BarracksPlatformPreferences _barracksPlatformPreferences;
        [SerializeField] private CollectorsPlatformPreferences _collectorsPlatformPreferences;

        [Header("Level Depended Values")]
        [SerializeField] private RangeDependentValue _zombieHealth;
        [SerializeField] private RangeDependentValue _zombieDamage;
        [SerializeField] private RangeDependentValue _lootBoxHealth;

        public PlayerPreferences PlayerPreferences => _playerPreferences;
        public AnimationPreferences AnimationPreferences => _animationPreferences;
        public HelicopterPlatformPreferences HelicopterPlatformPreferences => _helicopterPlatformPreferences;
        public OilPlatformPreferences OilPlatformPreferences => _oilPlatformPreferences;
        public DumpPlatformPreferences DumpPlatformPreferences => _dumpPlatformPreferences;
        public BarracksPlatformPreferences BarracksPlatformPreferences => _barracksPlatformPreferences;
        public CollectorsPlatformPreferences CollectorsPlatformPreferences => _collectorsPlatformPreferences;

        public RangeDependentValue ZombieHealth => _zombieHealth;
        public RangeDependentValue ZombieDamage => _zombieDamage;
        public RangeDependentValue LootBoxHealth => _lootBoxHealth;
    }
}