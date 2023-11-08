using Grid;
using Plugins.Banks;
using UniRx;

namespace Data.Persistent
{
    public class OilPlatformData
    {
        public readonly IntReactiveProperty Level = new IntReactiveProperty(1);
        public readonly GridData GridData = new GridData(3, 3, 9);
        public readonly FloatReactiveProperty BarrelProduceDuration = new FloatReactiveProperty(1f);
        public readonly ClampedIntegerBank UpgradeContainer = new ClampedIntegerBank(0, 100);
    }
}