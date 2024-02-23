using System;
using Quests.Core;
using Zenject;
using IInitializable = Unity.VisualScripting.IInitializable;

namespace Quests.Main.Core
{
    public class MainQuestSequence : QuestSequence, IInitializable, IDisposable
    {
        private readonly DiContainer _container;

        public MainQuestSequence(DiContainer container) => _container = container;

        public void Initialize()
        {
            StartObserving();
            StartVisualization();
        }

        public void Dispose() => StopObserving();

        protected override IQuest[] BuildQuests()
        {
            return new IQuest[]
            {
                _container.Instantiate<CollectBarrelsQuest>(),
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