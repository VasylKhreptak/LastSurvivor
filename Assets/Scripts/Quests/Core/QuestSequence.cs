using UniRx;

namespace Quests.Core
{
    public class QuestSequence : IQuestSequence
    {
        private readonly IQuest[] _quests;

        public QuestSequence(IQuest[] quests)
        {
            _quests = quests;
        }

        private readonly BoolReactiveProperty _isCompleted = new BoolReactiveProperty();

        private readonly ReactiveProperty<IQuest> _currentQuest = new ReactiveProperty<IQuest>();

        private readonly CompositeDisposable _questCompletionSubscriptions = new CompositeDisposable();

        private int _completedQuestsCount;

        private bool _isVisualizationEnabled;

        public IReadOnlyReactiveProperty<bool> IsCompleted => _isCompleted;

        public IReadOnlyReactiveProperty<IQuest> CurrentQuest => _currentQuest;

        public void StartObserving()
        {
            if (_isCompleted.Value)
                return;

            StopObserving();

            bool foundUncompletedQuest = false;
            foreach (IQuest quest in _quests)
            {
                if (quest.IsCompleted.Value)
                {
                    OnCompletedQuest();
                    continue;
                }

                if (foundUncompletedQuest == false)
                {
                    SetCurrentQuest(quest);
                    foundUncompletedQuest = true;
                }

                quest.IsCompleted
                    .Where(x => x)
                    .Subscribe(_ =>
                    {
                        quest.StopObserving();
                        OnCompletedQuest();
                    })
                    .AddTo(_questCompletionSubscriptions);

                quest.StartObserving();
            }
        }

        public void StopObserving()
        {
            _questCompletionSubscriptions.Clear();

            foreach (IQuest quest in _quests)
            {
                if (quest.IsCompleted.Value)
                    continue;

                quest.StopObserving();
            }

            _completedQuestsCount = 0;
        }

        private void OnCompletedQuest()
        {
            if (++_completedQuestsCount == _quests.Length)
            {
                _isCompleted.Value = true;
                SetCurrentQuest(null);
                StopObserving();
                return;
            }

            foreach (IQuest quest in _quests)
            {
                if (quest.IsCompleted.Value == false)
                {
                    SetCurrentQuest(quest);
                    return;
                }
            }

            SetCurrentQuest(null);
        }

        private void SetCurrentQuest(IQuest quest)
        {
            (_currentQuest.Value as IQuestCallbackHandler)?.OnBecameActive(false);

            (quest as IQuestCallbackHandler)?.OnBecameActive(true);

            _currentQuest.Value = quest;
        }
    }
}