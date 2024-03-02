using UI.Buttons.Core;
using UI.Main.Windows;
using UI.Main.Windows.Settings;
using Zenject;

namespace UI.Main.Buttons.Settings
{
    public class CloseSettingsButton : BaseButton
    {
        private SettingsWindow _settingsWindow;

        [Inject]
        private void Constructor(SettingsWindow settingsWindow)
        {
            _settingsWindow = settingsWindow;
        }

        protected override void OnClicked()
        {
            if (_settingsWindow.IsBeingHidden)
                return;

            _settingsWindow.Hide();
        }
    }
}