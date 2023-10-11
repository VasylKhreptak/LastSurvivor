namespace Infrastructure.Graphics.UI.Windows.Core
{
    public interface IWindow
    {
        public bool IsActive { get; }

        public void Show();

        public void Hide();
    }
}
