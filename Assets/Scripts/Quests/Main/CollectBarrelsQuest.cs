using System;
using Infrastructure.Services.PersistentData.Core;
using Main.Entities.Player;
using Main.Platforms.OilPlatform;
using Quests.Core;
using Quests.Main.Core;
using UniRx;

namespace Quests.Main
{
    public class CollectBarrelsQuest : Quest, IQuestCallbackHandler
    {
        private readonly OilPlatform _oilPlatform;
        private readonly Player _player;
        private readonly IPersistentDataService _persistentDataService;

        public CollectBarrelsQuest(OilPlatform oilPlatform, Player player, IPersistentDataService persistentDataService)
        {
            _oilPlatform = oilPlatform;
            _player = player;
            _persistentDataService = persistentDataService;

            _isCompleted.Value = persistentDataService.Data.PlayerData.CompletedQuests.Contains(QuestType.CollectBarrels);
        }

        private IDisposable _subscription;

        public override void StartObserving()
        {
            _subscription = _player.BarrelGridStack.Bank.IsFull
                .Where(x => x)
                .Subscribe(_ => MarkAsCompleted());
        }

        public override void StopObserving() => _subscription?.Dispose();

        public void OnBecameActive(bool isActive) => _player.QuestArrow.Target = isActive ? _oilPlatform.GridStackTransform : null;

        public override void MarkAsCompleted()
        {
            _persistentDataService.Data.PlayerData.CompletedQuests.Add(QuestType.CollectBarrels);
            base.MarkAsCompleted();
        }
    }
}