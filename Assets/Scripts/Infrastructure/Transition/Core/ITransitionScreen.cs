using System;

namespace Infrastructure.Transition.Core
{
    public interface ITransitionScreen
    {
        public event Action OnShown;
        public event Action OnHidden;

        public void Show();

        public void Hide();

        public void ShowImmediately();

        public void HideImmediately();
    }
}