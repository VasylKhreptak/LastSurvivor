using System;

namespace Infrastructure.Curtain.Core
{
    public interface ILoadingScreen
    {
        public event Action OnHidden;

        public void Show();

        public void Hide();
    }
}
