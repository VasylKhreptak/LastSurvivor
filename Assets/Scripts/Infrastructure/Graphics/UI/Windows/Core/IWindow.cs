using System;
using UniRx;

namespace Infrastructure.Graphics.UI.Windows.Core
{
    public interface IWindow
    {
        public void Show(Action onComplete = null);

        public void Hide(Action onComplete = null);
    }
}