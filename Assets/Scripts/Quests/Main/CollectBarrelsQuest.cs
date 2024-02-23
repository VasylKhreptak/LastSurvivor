using System;
using Quests.Core;
using Quests.Main.Core;
using UniRx;

namespace Quests.Main
{
    public class CollectBarrelsQuest : IQuest
    {
        private readonly BoolReactiveProperty _isCompleted = new BoolReactiveProperty(false);

        public QuestType Type => QuestType.CollectBarrels;

        public IReadOnlyReactiveProperty<bool> IsCompleted => _isCompleted;

        public void StartObserving()
        {
            throw new NotImplementedException();
        }

        public void StopObserving()
        {
            throw new NotImplementedException();
        }

        public void StartVisualization()
        {
            throw new NotImplementedException();
        }

        public void StopVisualization()
        {
            throw new NotImplementedException();
        }
    }
}