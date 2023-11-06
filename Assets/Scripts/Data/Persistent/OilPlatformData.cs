using Plugins.Banks;
using UniRx;

namespace Data.Persistent
{
    public class OilPlatformData
    {
        public readonly FloatReactiveProperty ProduceDuration = new FloatReactiveProperty(1f);
        public readonly ClampedIntegerBank UpgradeContainer = new ClampedIntegerBank(0, 100);
    }
}