using Plugins.Banks;
using UniRx;

namespace Data.Persistent.Platforms
{
    public class HelicopterPlatformData
    {
        public readonly IntReactiveProperty Level = new IntReactiveProperty(1);
        public readonly FloatReactiveProperty IncomeMultiplier = new FloatReactiveProperty(1f);
        public readonly ClampedIntegerBank FuelTank = new ClampedIntegerBank(0, 5);
        public readonly ClampedIntegerBank UpgradeContainer = new ClampedIntegerBank(0, 100);
    }
}