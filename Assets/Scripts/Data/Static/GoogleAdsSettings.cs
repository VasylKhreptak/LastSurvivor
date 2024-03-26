using System;
using UnityEngine;

namespace Data.Static
{
    [Serializable]
    public class GoogleAdsSettings
    {
        [SerializeField] private string _bannerId;
        [SerializeField] private string _interstitialId;
        [SerializeField] private string _rewardedVideoId;

        [Header("Test mode")]
        [SerializeField] private string _testBannerId = "ca-app-pub-3940256099942544/6300978111";
        [SerializeField] private string _testInterstitialId = "ca-app-pub-3940256099942544/1033173712";
        [SerializeField] private string _testRewardedVideoId = "ca-app-pub-3940256099942544/5224354917";

        [Header("Preferences")]
        [SerializeField] private bool _testModeEnabled = true;

        public string BannerId => _testModeEnabled ? _testBannerId : _bannerId;
        public string InterstitialId => _testModeEnabled ? _testInterstitialId : _interstitialId;
        public string RewardedVideoId => _testModeEnabled ? _testRewardedVideoId : _rewardedVideoId;
    }
}