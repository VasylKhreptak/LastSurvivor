using Data.Persistent;
using Data.Persistent.Platforms;

namespace Infrastructure.Data.Persistent
{
    public class PlayerData
    {
        public readonly Resources Resources = new Resources();
        public readonly PlatformsData PlatformsData = new PlatformsData();
        public int CompletedLevels = 26;
    }
}