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

        public string BannerId => _bannerId;
        public string InterstitialId => _interstitialId;
        public string RewardedVideoId => _rewardedVideoId;
    }
}