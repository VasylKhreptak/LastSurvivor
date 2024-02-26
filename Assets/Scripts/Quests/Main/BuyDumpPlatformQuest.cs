using System;
using Infrastructure.Services.PersistentData.Core;
using Main.Entities.Player;
using Main.Platforms.BuyZones;
using Quests.Core;
using Quests.Main.Core;
using UniRx;
using Zenject;

namespace Quests.Main
{
    public class BuyDumpPlatformQuest : Quest, IQuestCallbackHandler
    {
        private readonly Player _player;
        private readonly IPersistentDataService _persistentDataService;
        private readonly DumpBuyZone _dumpBuyZone;

        public BuyDumpPlatformQuest(Player player, IPersistentDataService persistentDataService,
            [InjectOptional] DumpBuyZone dumpBuyZone)
        {
            _player = player;
            _persistentDataService = persistentDataService;
            _dumpBuyZone = dumpBuyZone;

            _isCompleted.Value = _persistentDataService.Data.PlayerData.CompletedQuests.Contains(QuestType.BuyDumpPlatform);
        }

        private IDisposable _subscription;

        public override void StartObserving()
        {
            _subscription = _persistentDataService.Data.PlayerData.PlatformsData.DumpPlatformData.BuyContainer.IsFull
                .Where(x => x)
                .Subscribe(_ => MarkAsCompleted());
        }

        public override void StopObserving() => _subscription?.Dispose();

        public void OnBecameActive(bool isActive) => _player.QuestArrow.Target = isActive ? _dumpBuyZone.transform : null;

        public override void MarkAsCompleted()
        {
            _persistentDataService.Data.PlayerData.CompletedQuests.Add(QuestType.BuyDumpPlatform);
            base.MarkAsCompleted();
        }
    }
}