using System.ComponentModel;
using Infrastructure.Services.Advertisement.Core;

namespace DebuggerOptions
{
    public class AdvertisementOptions
    {
        private readonly IAdvertisementService _advertisementService;

        public AdvertisementOptions(IAdvertisementService advertisementService)
        {
            _advertisementService = advertisementService;
        }

        [Category("Advertisements")]
        public void LoadBanner() => _advertisementService.LoadBanner();

        [Category("Advertisements")]
        public void ShowBanner() => _advertisementService.ShowBanner();

        [Category("Advertisements")]
        public void DestroyBanner() => _advertisementService.DestroyBanner();

        [Category("Advertisements")]
        public void LoadInterstitial() => _advertisementService.LoadInterstitial();

        [Category("Advertisements")]
        public void ShowInterstitial() => _advertisementService.ShowInterstitial();

        [Category("Advertisements")]
        public void LoadRewardedVideo() => _advertisementService.LoadRewardedVideo();

        [Category("Advertisements")]
        public void ShowRewardedVideo() => _advertisementService.ShowRewardedVideo();
    }
}