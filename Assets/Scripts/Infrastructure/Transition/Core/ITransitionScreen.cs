using System;
using UniRx;

namespace Infrastructure.Transition.Core
{
    public interface ITransitionScreen
    {
        public IReadOnlyReactiveProperty<float> FadeProgress { get; }

        public void Show(Action onComplete = null);

        public void Hide(Action onComplete = null);

        public void ShowImmediately();

        public void HideImmediately();
    }
}