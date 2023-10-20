namespace Infrastructure.Services.SaveLoad.Core
{
    public interface ISaveLoadService
    {
        public void Save<T>(T data, string key);

        public T Load<T>(string key, T defaultValue = default);

        public bool HasKey(string key);
    }
}