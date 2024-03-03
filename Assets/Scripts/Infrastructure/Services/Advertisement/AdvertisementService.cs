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

            _logService.Log("Banner ad shown");
        }

        public void DestroyBanner()
        {
            _bannerView?.Destroy();
            _bannerView = null;

            _logService.Log("Banner ad destroyed");
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
                    _logService.Log($"Interstitial ad failed to load: {error}");
                    return;
                }

                _logService.Log("Interstitial ad loaded");

                _interstitialAd = ad;
            });
        }

        public void ShowInterstitial()
        {
            if (_interstitialAd == null || _interstitialAd.CanShowAd() == false)
                return;

            _interstitialAd.Show();

            _logService.Log("Interstitial ad shown");
        }

        public void DestroyInterstitial()
        {
            _interstitialAd?.Destroy();
            _interstitialAd = null;

            _logService.Log("Interstitial ad destroyed");
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
                    _logService.Log($"Rewarded video ad failed to load: {error}");
                    return;
                }

                _rewardedAd = ad;

                _logService.Log("Rewarded video ad loaded");
            });
        }

        public bool ShowRewardedVideo(Action onRewarded)
        {
            if (_rewardedAd == null || _rewardedAd.CanShowAd() == false)
                return false;

            _rewardedAd.Show(_ =>
            {
                _logService.Log("Rewarded video");
                onRewarded?.Invoke();
            });

            _logService.Log("Rewarded video ad shown");
            return true;
        }

        public void DestroyRewardedVideo()
        {
            _rewardedAd?.Destroy();
            _rewardedAd = null;

            _logService.Log("Rewarded video ad destroyed");
        }

        private void OnInitialized() => _logService.Log("Google ads initialized");

        private void InitializeBannerView()
        {
            if (_bannerView != null)
                return;

            string bannerId = _staticDataService.Config.GoogleAdsSettings.BannerId;
            _bannerView = new BannerView(bannerId, AdSize.Banner, AdPosition.Bottom);
        }
    }
}