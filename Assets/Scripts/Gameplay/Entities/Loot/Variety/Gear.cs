using Gameplay.Data;
using Zenject;

namespace Gameplay.Entities.Loot.Variety
{
    public class Gear : Core.Loot
    {
        private LevelData _levelData;

        [Inject]
        private void Construct(LevelData levelData) => _levelData = levelData;

        public override void RegisterLoot() => _levelData.CollectedGears.Value += Data.Count;
    }
}