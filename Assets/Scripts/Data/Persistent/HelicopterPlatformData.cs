using Plugins.Banks;

namespace Data.Persistent
{
    public class HelicopterPlatformData
    {
        public HelicopterData HelicopterData = new HelicopterData();
        public ClampedIntegerBank UpgradeContainer = new ClampedIntegerBank(0, 100);
    }
}