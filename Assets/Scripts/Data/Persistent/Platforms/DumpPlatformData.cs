using Plugins.Banks;

namespace Data.Persistent.Platforms
{
    public class DumpPlatformData
    {
        public readonly ClampedIntegerBank WorkersBank = new ClampedIntegerBank(0, 5);
        public readonly ClampedIntegerBank HireWorkerContainer = new ClampedIntegerBank(0, 100);
        public readonly ClampedIntegerBank GridData = new ClampedIntegerBank(0, 18);
        public readonly ClampedIntegerBank BuyContainer = new ClampedIntegerBank(0, 100);
    }
}