namespace Infrastructure.Services.Advertisement.Core
{
    public interface IAdvertisementService
    {
        public void LoadBanner();

        public void ShowBanner();

        public void DestroyBanner();

        public void LoadInterstitial();

        public void ShowInterstitial();

        public void LoadRewardedVideo();

        public void ShowRewardedVideo();
    }
}