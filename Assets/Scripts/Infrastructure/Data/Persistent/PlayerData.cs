using Data.Persistent;

namespace Infrastructure.Data.Persistent
{
    public class PlayerData
    {
        public readonly Resources Resources = new Resources();
        public readonly HelicopterPlatformData HelicopterPlatformData = new HelicopterPlatformData();
        public readonly OilPlatformData OilPlatformData = new OilPlatformData();

        public bool FinishedTutorial;
    }
}