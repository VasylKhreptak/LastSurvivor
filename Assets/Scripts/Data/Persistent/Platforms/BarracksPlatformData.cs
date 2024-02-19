using Plugins.Banks;

namespace Data.Persistent.Platforms
{
    public class BarracksPlatformData
    {
        public readonly ClampedIntegerBank SoldiersBank = new ClampedIntegerBank(4, 12);
        public readonly ClampedIntegerBank HireSoldierBank = new ClampedIntegerBank(0, 100);
        public readonly ClampedIntegerBank BuyContainer = new ClampedIntegerBank(0, 200);
    }
}