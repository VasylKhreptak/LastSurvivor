using Data.Persistent;
using Data.Persistent.Platforms;

namespace Infrastructure.Data.Persistent
{
    public class PlayerData
    {
        public readonly Resources Resources = new Resources();

        public readonly HelicopterPlatformData HelicopterPlatformData = new HelicopterPlatformData();
        public readonly OilPlatformData OilPlatformData = new OilPlatformData();
        public readonly DumpPlatformData DumpPlatformData = new DumpPlatformData();

        public bool FinishedTutorial;
    }
}