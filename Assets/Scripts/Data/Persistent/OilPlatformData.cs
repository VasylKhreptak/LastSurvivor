using Plugins.Banks;
using UniRx;

namespace Data.Persistent
{
    public class OilPlatformData
    {
        public readonly FloatReactiveProperty BarrelProduceDuration = new FloatReactiveProperty(1f);
        public readonly ClampedIntegerBank UpgradeContainer = new ClampedIntegerBank(0, 100);
        public readonly ReactiveProperty<uint> GridRows = new ReactiveProperty<uint>(3);
        public readonly ReactiveProperty<uint> GridColumns = new ReactiveProperty<uint>(3);
        public readonly ReactiveProperty<uint> GridLayers = new ReactiveProperty<uint>(2);
    }
}