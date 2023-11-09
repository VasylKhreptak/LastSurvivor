using Plugins.Banks;
using UniRx;

namespace Data.Persistent
{
    public class HelicopterData
    {
        public readonly IntReactiveProperty Level = new IntReactiveProperty(1);
        public readonly FloatReactiveProperty IncomeMultiplier = new FloatReactiveProperty(1f);
        public readonly ClampedIntegerBank FuelTank = new ClampedIntegerBank(0, 5);
    }
}