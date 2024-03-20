using Data.Persistent;

namespace Infrastructure.Data.Persistent
{
    public class PersistentData
    {
        public readonly PlayerData PlayerData = new PlayerData();
        public readonly Settings Settings = new Settings();
        public readonly Metadata Metadata = new Metadata();
    }
}