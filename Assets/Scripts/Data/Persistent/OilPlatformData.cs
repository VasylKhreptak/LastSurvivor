using Plugins.Banks;
using UniRx;

namespace Data.Persistent
{
    public class OilPlatformData
    {
        public readonly IntReactiveProperty Level = new IntReactiveProperty(1);
        public readonly FloatReactiveProperty BarrelProduceDuration = new FloatReactiveProperty(1f);
        public readonly ClampedIntegerBank UpgradeContainer = new ClampedIntegerBank(0, 100);
        public readonly ReactiveProperty<int> GridRows = new ReactiveProperty<int>(3);
        public readonly ReactiveProperty<int> GridColumns = new ReactiveProperty<int>(3);
        public readonly ReactiveProperty<int> BarrelsCapacity = new ReactiveProperty<int>(10);
        public readonly ReactiveProperty<int> BarrelsCount = new ReactiveProperty<int>(0);
    }
}