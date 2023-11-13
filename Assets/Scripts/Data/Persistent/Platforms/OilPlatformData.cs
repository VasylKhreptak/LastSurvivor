using Plugins.Banks;
using UniRx;

namespace Data.Persistent.Platforms
{
    public class OilPlatformData
    {
        public readonly IntReactiveProperty Level = new IntReactiveProperty(1);
        public readonly ClampedIntegerBank GridData = new ClampedIntegerBank(0, 9);
        public readonly FloatReactiveProperty BarrelProduceDuration = new FloatReactiveProperty(2f);
        public readonly ClampedIntegerBank UpgradeContainer = new ClampedIntegerBank(0, 100);
    }
}