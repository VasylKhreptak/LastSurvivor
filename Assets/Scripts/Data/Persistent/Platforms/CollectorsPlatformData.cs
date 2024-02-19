using Plugins.Banks;

namespace Data.Persistent.Platforms
{
    public class CollectorsPlatformData
    {
        public readonly ClampedIntegerBank CollectorsBank = new ClampedIntegerBank(2, 5);
        public readonly ClampedIntegerBank HireCollectorBank = new ClampedIntegerBank(0, 100);
        public readonly ClampedIntegerBank BuyContainer = new ClampedIntegerBank(0, 200);
    }
}