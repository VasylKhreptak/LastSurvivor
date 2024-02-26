using UniRx;

namespace Quests.Core
{
    public abstract class Quest : IQuest
    {
        protected readonly BoolReactiveProperty _isCompleted = new BoolReactiveProperty();

        public IReadOnlyReactiveProperty<bool> IsCompleted => _isCompleted;

        public abstract void StartObserving();

        public abstract void StopObserving();

        public virtual void MarkAsCompleted() => _isCompleted.Value = true;
    }
}