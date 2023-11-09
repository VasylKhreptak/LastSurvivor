using Plugins.Banks;

namespace Data.Persistent
{
    public class Resources
    {
        public readonly IntegerBank Money = new IntegerBank(0);
        public readonly IntegerBank Gears = new IntegerBank(1000);
    }
}