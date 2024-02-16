using System;
using Infrastructure.Services.Advertisement.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.Services.StaticData.Core;
using UniRx;
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

        private IDisposable _applicationPauseSubscription;

        public void Initialize()
        {
            StartObserving();

            _logService.Log("IronSource Initialization");
            IronSource.Agent.init(_staticDataService.Config.AppKey);

            IronSource.Agent.validateIntegration();
        }

        public void Dispose()
        {
            StopObserving();
        }

        private void StartObserving()
        {
            IronSourceEvents.onSdkInitializationCompletedEvent += OnSDKInitialized;
            _applicationPauseSubscription = Observable.EveryApplicationPause().Subscribe(OnApplicationPause);
        }

        private void StopObserving()
        {
            IronSourceEvents.onSdkInitializationCompletedEvent -= OnSDKInitialized;
            _applicationPauseSubscription.Dispose();
        }

        private void OnSDKInitialized() => _logService.Log("IronSource SDK Initialized");

        private void OnApplicationPause(bool pauseStatus) => IronSource.Agent.onApplicationPause(pauseStatus);

        public void LoadBanner() => IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);

        public void ShowBanner() => IronSource.Agent.displayBanner();

        public void DestroyBanner() => IronSource.Agent.destroyBanner();

        public void LoadInterstitial() => IronSource.Agent.loadInterstitial();

        public void ShowInterstitial()
        {
            if (IronSource.Agent.isInterstitialReady())
            {
                IronSource.Agent.showInterstitial();
                return;
            }

            _logService.LogWarning("Interstitial is not ready");
        }

        public void LoadRewardedVideo() => IronSource.Agent.loadRewardedVideo();

        public void ShowRewardedVideo()
        {
            if (IronSource.Agent.isRewardedVideoAvailable())
            {
                IronSource.Agent.showRewardedVideo();
                return;
            }

            _logService.LogWarning("Rewarded Video is not ready");
        }
    }
}