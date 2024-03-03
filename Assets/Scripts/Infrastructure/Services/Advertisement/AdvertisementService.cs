using System;
using GoogleMobileAds.Api;
using Infrastructure.Services.Advertisement.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.Services.StaticData.Core;
using Zenject;

namespace Infrastructure.Services.Advertisement
{
    public class AdvertisementService : IAdvertisementService, IInitializable
    {
        private readonly IStaticDataService _staticDataService;
        private readonly ILogService _logService;

        public AdvertisementService(IStaticDataService staticDataService, ILogService logService)
        {
            _staticDataService = staticDataService;
            _logService = logService;
        }

        private BannerView _bannerView;
        private InterstitialAd _interstitialAd;
        private RewardedAd _rewardedAd;

        public void Initialize()
        {
            MobileAds.RaiseAdEventsOnUnityMainThread = true;
            MobileAds.Initialize(_ => OnInitialized());
        }

        public void ShowBanner()
        {
            DestroyBanner();
            InitializeBannerView();

            AdRequest adRequest = new AdRequest();
            _bannerView.LoadAd(adRequest);
        }

        public void DestroyBanner()
        {
            _bannerView?.Destroy();
            _bannerView = null;
        }

        public void LoadInterstitial()
        {
            DestroyInterstitial();

            string interstitialId = _staticDataService.Config.GoogleAdsSettings.InterstitialId;

            AdRequest adRequest = new AdRequest();

            InterstitialAd.Load(interstitialId, adRequest, (ad, error) =>
            {
                if (error != null)
                {
                    _logService.LogError($"Interstitial ad failed to load: {error}");
                    return;
                }

                _interstitialAd = ad;
            });
        }

        public void ShowInterstitial()
        {
            if (_interstitialAd == null || _interstitialAd.CanShowAd() == false)
                return;

            _interstitialAd.Show();
        }

        public void DestroyInterstitial()
        {
            _interstitialAd?.Destroy();
            _interstitialAd = null;
        }

        public void LoadRewardedVideo()
        {
            DestroyRewardedVideo();

            string rewardedVideoId = _staticDataService.Config.GoogleAdsSettings.RewardedVideoId;

            AdRequest adRequest = new AdRequest();

            RewardedAd.Load(rewardedVideoId, adRequest, (ad, error) =>
            {
                if (error != null)
                {
                    _logService.LogError($"Rewarded video ad failed to load: {error}");
                    return;
                }

                _rewardedAd = ad;
            });
        }

        public bool ShowRewardedVideo(Action onRewarded)
        {
            if (_rewardedAd == null || _rewardedAd.CanShowAd() == false)
                return false;

            _rewardedAd.Show(_ =>
            {
                _logService.Log("Rewarded earned");
                onRewarded?.Invoke();
            });

            return true;
        }

        public void DestroyRewardedVideo()
        {
            _rewardedAd?.Destroy();
            _rewardedAd = null;
        }

        private void OnInitialized() => _logService.Log("Admob initialized");

        private void InitializeBannerView()
        {
            if (_bannerView != null)
                return;

            string bannerId = _staticDataService.Config.GoogleAdsSettings.BannerId;
            _bannerView = new BannerView(bannerId, AdSize.Banner, AdPosition.Bottom);
        }
    }
}