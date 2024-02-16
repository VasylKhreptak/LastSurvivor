using Infrastructure.Services.Advertisement;
using Infrastructure.Services.Advertisement.Core;
using JetBrains.Annotations;

namespace DebuggerOptions
{
    public class AdvertisementOptionsContainer
    {
        private readonly IAdvertisementService _advertisementService;

        public AdvertisementOptionsContainer(IAdvertisementService advertisementService)
        {
            _advertisementService = advertisementService;
        }

        [UsedImplicitly]
        public void LoadBanner() => _advertisementService.LoadBanner();

        [UsedImplicitly]
        public void ShowBanner() => _advertisementService.ShowBanner();

        [UsedImplicitly]
        public void DestroyBanner() => _advertisementService.DestroyBanner();

        [UsedImplicitly]
        public void LoadInterstitial() => _advertisementService.LoadInterstitial();

        [UsedImplicitly]
        public void ShowInterstitial() => _advertisementService.ShowInterstitial();

        [UsedImplicitly]
        public void LoadRewardedVideo() => _advertisementService.LoadRewardedVideo();

        [UsedImplicitly]
        public void ShowRewardedVideo() => _advertisementService.ShowRewardedVideo();
    }
}