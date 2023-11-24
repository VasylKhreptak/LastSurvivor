using Plugins.Banks;

namespace Data.Persistent
{
    public class Resources
    {
        public readonly IntegerBank Money = new IntegerBank(10000);
        public readonly IntegerBank Gears = new IntegerBank(10000);
    }
}