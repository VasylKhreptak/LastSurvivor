using System;
using Quests.Core;
using UniRx;
using Zenject;

namespace Quests.Main.Core
{
    public class MainQuestSequence : IQuestSequence, IInitializable, IDisposable
    {
        private readonly DiContainer _container;
        private readonly IQuestSequence _questSequence;

        public MainQuestSequence(DiContainer container)
        {
            _container = container;
            _questSequence = new QuestSequence(BuildQuests());
        }

        public IReadOnlyReactiveProperty<bool> IsCompleted => _questSequence.IsCompleted;

        public IReadOnlyReactiveProperty<IQuest> CurrentQuest => _questSequence.CurrentQuest;

        public void Initialize() => StartObserving();

        public void Dispose() => StopObserving();

        public void StartObserving() => _questSequence.StartObserving();

        public void StopObserving() => _questSequence.StopObserving();

        private IQuest[] BuildQuests()
        {
            return new IQuest[]
            {
                _container.Instantiate<CollectBarrelsQuest>(), _container.Instantiate<FuelUpHelicopterQuest>(),
                _container.Instantiate<EnterPlayStateQuest>(), _container.Instantiate<BuyDumpPlatformQuest>(),
                _container.Instantiate<HireWorkerQuest>()
            };
        }
    }
}