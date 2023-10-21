using Infrastructure.Data.Core;
using Infrastructure.Data.Static;
using Infrastructure.Data.Static.Balance.Core;

namespace Infrastructure.Services.StaticData.Core
{
    public interface IStaticDataService : ILoadHandler
    {
        public GameConfig Config { get; }
        public GameBalance Balance { get; }
        public GamePrefabs Prefabs { get; }
    }
}