using Gameplay.Data;
using Zenject;

namespace Gameplay.Entities.Loot.Variety
{
    public class Money : Core.Loot
    {
        private LevelData _levelData;

        [Inject]
        private void Constructor(LevelData levelData) => _levelData = levelData;

        public override void RegisterLoot() => _levelData.CollectedMoney.Value += Data.Count;
    }
}