using UI.Buttons.Core;
using UI.Main.Windows;
using Zenject;

namespace UI.Main.Buttons.Settings
{
    public class OpenSettingsButton : BaseButton
    {
        private SettingsWindow _settingsWindow;

        [Inject]
        private void Constructor(SettingsWindow settingsWindow)
        {
            _settingsWindow = settingsWindow;
        }

        protected override void OnClicked() => _settingsWindow.Show();
    }
}