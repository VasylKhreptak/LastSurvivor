using System;
using Quests.Core;
using Zenject;

namespace Quests.Main.Core
{
    public class MainQuestSequence : IInitializable, IDisposable
    {
        private readonly DiContainer _container;
        private readonly IQuestSequence _questSequence;

        public MainQuestSequence(DiContainer container)
        {
            _container = container;
            _questSequence = new QuestSequence(BuildQuests());
        }

        public void Initialize()
        {
            _questSequence.StartObserving();
            (_questSequence as IQuestVisualization)?.StartVisualization();
        }

        public void Dispose() => _questSequence.StopObserving();

        private IQuest[] BuildQuests()
        {
            return new IQuest[]
            {
                _container.Instantiate<CollectBarrelsQuest>()
                // new FuelUpHelicopterQuest(),
                // new BuyDumPlatformQuest(),
                // new BuySoldiersPlatformQuest(),
                // new HireSoldierQuest(),
                // new BuyCollectorsPlatformQuest(),
                // new HireCollectorQuest()
            };
        }
    }
}