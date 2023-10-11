using Infrastructure.Data.Core;

namespace Infrastructure.Services.SaveLoadHandler.Core
{
    public interface ISaveLoadHandlerService : ISaveLoadHandler
    {
        public void AddSaveHandler<T>(T t) where T : ISaveHandler;
        
        public void RemoveSaveHandler<T>(T t) where T : ISaveHandler;
        
        public void AddLoadHandler<T>(T t) where T : ILoadHandler;
        
        public void RemoveLoadHandler<T>(T t) where T : ILoadHandler;

        public void Add<T>(T t) where T : ISaveLoadHandler;
        
        public void Remove<T>(T t) where T : ISaveLoadHandler;
    }
}
