using Data.Persistent;
using Data.Persistent.Platforms;

namespace Infrastructure.Data.Persistent
{
    public class PlayerData
    {
        public readonly Resources Resources = new Resources();

        public readonly PlatformsData PlatformsData = new PlatformsData();
        public readonly BuyZonesData BuyZonesData = new BuyZonesData();

        public bool FinishedTutorial;
    }
}