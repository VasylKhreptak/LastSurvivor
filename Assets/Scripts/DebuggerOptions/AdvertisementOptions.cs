using System.ComponentModel;
using Infrastructure.Services.Advertisement.Core;
using Infrastructure.Services.Log.Core;

namespace DebuggerOptions
{
    public class AdvertisementOptions
    {
        private readonly IAdvertisementService _advertisementService;
        private readonly ILogService _logService;

        public AdvertisementOptions(IAdvertisementService advertisementService, ILogService logService)
        {
            _advertisementService = advertisementService;
            _logService = logService;
        }

        [Category("Advertisements")]
        public void ShowBanner() => _advertisementService.ShowBanner();

        [Category("Advertisements")]
        public void DestroyBanner() => _advertisementService.DestroyBanner();

        [Category("Advertisements")]
        public void LoadInterstitial() => _advertisementService.LoadInterstitial();

        [Category("Advertisements")]
        public void ShowInterstitial() => _advertisementService.ShowInterstitial();

        [Category("Advertisements")]
        public void DestroyInterstitial() => _advertisementService.DestroyInterstitial();

        [Category("Advertisements")]
        public void LoadRewardedVideo() => _advertisementService.LoadRewardedVideo();

        [Category("Advertisements")]
        public void ShowRewardedVideo() =>
            _advertisementService.ShowRewardedVideo(() => { _logService.Log($"Rewarded video shown"); });

        [Category("Advertisements")]
        public void DestroyRewardedVideo() => _advertisementService.DestroyRewardedVideo();
    }
}