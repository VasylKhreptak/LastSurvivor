using System;
using Infrastructure.Services.PersistentData.Core;
using Main.Entities.Player;
using Main.Platforms.OilPlatform;
using Quests.Core;
using Quests.Main.Core;
using UniRx;

namespace Quests.Main
{
    public class CollectBarrelsQuest : IQuest, IQuestVisualization
    {
        private readonly OilPlatform _oilPlatform;
        private readonly Player _player;
        private readonly IPersistentDataService _persistentDataService;

        public CollectBarrelsQuest(OilPlatform oilPlatform, Player player, IPersistentDataService persistentDataService)
        {
            _oilPlatform = oilPlatform;
            _player = player;
            _persistentDataService = persistentDataService;

            _isCompleted.Value = _persistentDataService.Data.PlayerData.CompletedQuests.Contains(QuestType.CollectBarrels);
        }

        private IDisposable _playerGridSubscription;

        private readonly BoolReactiveProperty _isCompleted = new BoolReactiveProperty(false);

        public IReadOnlyReactiveProperty<bool> IsCompleted => _isCompleted;

        public void StartObserving()
        {
            _playerGridSubscription = _player.BarrelGridStack.Bank.IsFull
                .Where(x => x)
                .Subscribe(_ =>
                {
                    _persistentDataService.Data.PlayerData.CompletedQuests.Add(QuestType.CollectBarrels);
                    _isCompleted.Value = true;
                });
        }

        public void StopObserving() => _playerGridSubscription?.Dispose();

        public void StartVisualization() => _player.QuestArrow.Target = _oilPlatform.GridStackTransform;

        public void StopVisualization() => _player.QuestArrow.Target = null;
    }
}