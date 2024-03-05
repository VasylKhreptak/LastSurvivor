using Infrastructure.Data.SaveLoad.Core;

namespace Infrastructure.Services.PersistentData.Core
{
    public interface IPersistentDataService : ISaveLoadHandler
    {
        public Data.Persistent.PersistentData Data { get; }
    }
}