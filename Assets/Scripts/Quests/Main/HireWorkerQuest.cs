using System;
using Infrastructure.Services.PersistentData.Core;
using Main.Entities.Player;
using Main.Platforms.DumpPlatform;
using Quests.Core;
using Quests.Main.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Main
{
    public class HireWorkerQuest : Quest, IQuestCallbackHandler
    {
        private readonly IPersistentDataService _persistentDataService;
        private readonly LazyInject<DumpPlatform> _dumpPlatform;
        private readonly Player _player;

        public HireWorkerQuest(IPersistentDataService persistentDataService, LazyInject<DumpPlatform> dumpPlatform, Player player)
        {
            _persistentDataService = persistentDataService;
            _dumpPlatform = dumpPlatform;
            _player = player;
            _isCompleted.Value = _persistentDataService.Data.PlayerData.CompletedQuests.Contains(QuestType.HireWorker);
        }

        private IDisposable _subscription;

        public override void StartObserving()
        {
            _subscription = _persistentDataService.Data.PlayerData.PlatformsData.DumpPlatformData.WorkersBank.Value
                .Pairwise()
                .Select(pair => pair.Previous < pair.Current)
                .Subscribe(_ =>
                {
                    Debug.Log("Hired worker!");
                    MarkAsCompleted();
                });
        }

        public override void StopObserving() => _subscription?.Dispose();

        public void OnBecameActive(bool isActive) =>
            _player.QuestArrow.Target = isActive ? _dumpPlatform.Value.HireWorkerZone.transform : null;

        public override void MarkAsCompleted()
        {
            _persistentDataService.Data.PlayerData.CompletedQuests.Add(QuestType.HireWorker);
            base.MarkAsCompleted();
        }
    }
}