using Infrastructure.Services.PersistentData.Core;

namespace Gameplay.Levels
{
    public class LevelManager
    {
        private readonly IPersistentDataService _persistentDataService;

        public LevelManager(IPersistentDataService persistentDataService)
        {
            _persistentDataService = persistentDataService;
        }

        public int GetCurrentLevel() => _persistentDataService.Data.PlayerData.CompletedLevelsCount + 1;
    }
}