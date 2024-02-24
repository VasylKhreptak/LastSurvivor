using UniRx;

namespace Quests.Core
{
    public interface IQuest
    {
        public IReadOnlyReactiveProperty<bool> IsCompleted { get; }

        public void StartObserving();

        public void StopObserving();
    }
}