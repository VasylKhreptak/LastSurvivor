using Infrastructure.Services.Advertisement.Core;
using Infrastructure.Services.StaticData.Core;
using Zenject;

namespace Infrastructure.Services.Advertisement
{
    public class AdvertisementService : IAdvertisementService, IInitializable
    {
        private readonly IStaticDataService _staticDataService;
        
        public AdvertisementService(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public void Initialize()
        {
            
        }
    }
}