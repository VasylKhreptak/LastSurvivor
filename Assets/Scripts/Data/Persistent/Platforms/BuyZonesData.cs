using Plugins.Banks;

namespace Data.Persistent.Platforms
{
    public class BuyZonesData
    {
        public readonly ClampedIntegerBank DumpBuyZoneBank = new ClampedIntegerBank(0, 200);
    }
}