using Infrastructure.Data.Core;

namespace Infrastructure.Services.RuntimeData.Core
{
    public interface IRuntimeDataService : ISaveLoadHandler
    {
        public Data.Runtime.RuntimeData RuntimeData { get; set; }
    }
}
