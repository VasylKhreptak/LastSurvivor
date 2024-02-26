using UniRx;

namespace Quests.Core
{
    public interface IQuestSequence
    {
        public IReadOnlyReactiveProperty<bool> IsCompleted { get; }

        public IReadOnlyReactiveProperty<IQuest> CurrentQuest { get; }

        public void StartObserving();

        public void StopObserving();
    }
}