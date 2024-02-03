using Data.Persistent;
using Data.Persistent.Platforms;
using UniRx;

namespace Infrastructure.Data.Persistent
{
    public class PlayerData
    {
        public readonly Resources Resources = new Resources();
        public readonly PlatformsData PlatformsData = new PlatformsData();
        public readonly IntReactiveProperty Level = new IntReactiveProperty(1);

        public bool FinishedTutorial;
    }
}