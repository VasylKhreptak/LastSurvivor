using System;
using Analytics;
using Firebase.Analytics;
using Infrastructure.Services.PersistentData.Core;
using Main.Entities.Player;
using Main.Platforms.HelicopterPlatform;
using Quests.Core;
using Quests.Main.Core;
using UniRx;

namespace Quests.Main
{
    public class FuelUpHelicopterQuest : Quest, IQuestCallbackHandler
    {
        private readonly HelicopterPlatform _helicopterPlatform;
        private readonly IPersistentDataService _persistentDataService;
        private readonly Player _player;

        public FuelUpHelicopterQuest(HelicopterPlatform helicopterPlatform, IPersistentDataService persistentDataService,
            Player player)
        {
            _helicopterPlatform = helicopterPlatform;
            _persistentDataService = persistentDataService;
            _player = player;

            _isCompleted.Value = _persistentDataService.Data.PlayerData.CompletedQuests.Contains(QuestType.FuelUpHelicopter);
        }

        private IDisposable _subscription;

        public override void StartObserving()
        {
            _subscription = _persistentDataService.Data.PlayerData.PlatformsData.HelicopterPlatformData.FuelTank.IsFull
                .Where(x => x)
                .Subscribe(_ => MarkAsCompleted());
        }

        public override void StopObserving() => _subscription?.Dispose();

        public void OnBecameActive(bool isActive) => _player.QuestArrow.Target = isActive ? _helicopterPlatform.transform : null;

        public override void MarkAsCompleted()
        {
            _persistentDataService.Data.PlayerData.CompletedQuests.Add(QuestType.FuelUpHelicopter);
            FirebaseAnalytics.LogEvent(AnalyticEvents.CompletedQuest,
                new Parameter(AnalyticParameters.Name, QuestType.FuelUpHelicopter.ToString()));
            base.MarkAsCompleted();
        }
    }
}