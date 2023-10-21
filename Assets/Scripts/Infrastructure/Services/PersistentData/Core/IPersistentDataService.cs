using Infrastructure.Data.Core;

namespace Infrastructure.Services.PersistentData.Core
{
    public interface IPersistentDataService : ISaveLoadHandler
    {
        public Data.Persistent.PersistentData PersistentData { get; set; }
    }
}