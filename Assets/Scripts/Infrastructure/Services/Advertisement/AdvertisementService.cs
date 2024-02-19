using System;
using Infrastructure.Services.Advertisement.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.Services.StaticData.Core;
using Zenject;

namespace Infrastructure.Services.Advertisement
{
    public class AdvertisementService : IAdvertisementService, IInitializable, IDisposable
    {
        private readonly IStaticDataService _staticDataService;
        private readonly ILogService _logService;

        public AdvertisementService(IStaticDataService staticDataService, ILogService logService)
        {
            _staticDataService = staticDataService;
            _logService = logService;
        }

        public void Initialize() { }

        public void Dispose() { }

        private void OnApplicationPause(bool pauseStatus) { }

        public void LoadBanner() { }

        public void ShowBanner() { }

        public void DestroyBanner() { }

        public void LoadInterstitial() { }

        public void ShowInterstitial() { }

        public void LoadRewardedVideo() { }

        public void ShowRewardedVideo() { }
    }
}