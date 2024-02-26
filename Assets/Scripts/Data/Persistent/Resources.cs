using Plugins.Banks;

namespace Data.Persistent
{
    public class Resources
    {
        public readonly IntegerBank Money = new IntegerBank(150);
        public readonly IntegerBank Gears = new IntegerBank(200);
    }
}