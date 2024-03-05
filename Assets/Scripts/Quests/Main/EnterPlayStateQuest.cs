using Analytics;
using Firebase.Analytics;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.StateMachine.Game.States;
using Quests.Core;
using Quests.Main.Core;

namespace Quests.Main
{
    public class EnterPlayStateQuest : Quest
    {
        private readonly PlayState _playState;
        private readonly IPersistentDataService _persistentDataService;

        public EnterPlayStateQuest(PlayState playState, IPersistentDataService persistentDataService)
        {
            _playState = playState;
            _persistentDataService = persistentDataService;

            _isCompleted.Value = _persistentDataService.Data.PlayerData.CompletedQuests.Contains(QuestType.EnterPlayState);
        }

        public override void StartObserving() => _playState.OnEnter += MarkAsCompleted;

        public override void StopObserving() => _playState.OnEnter -= MarkAsCompleted;

        public override void MarkAsCompleted()
        {
            _persistentDataService.Data.PlayerData.CompletedQuests.Add(QuestType.EnterPlayState);
            FirebaseAnalytics.LogEvent(AnalyticEvents.CompletedQuest,
                new Parameter(AnalyticParameters.Name, QuestType.EnterPlayState.ToString()));
            base.MarkAsCompleted();
        }
    }
}