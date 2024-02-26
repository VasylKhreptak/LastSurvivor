using UI.Main.Windows;
using UnityEngine;

namespace Zenject.Installers.SceneContext.Main
{
    public class MainUIInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private HUD _hud;
        [SerializeField] private SettingsWindow _settingsWindow;

        #region MonoBehaviour

        private void OnValidate()
        {
            _hud ??= FindObjectOfType<HUD>();
            _settingsWindow ??= FindObjectOfType<SettingsWindow>();
        }

        #endregion

        public override void InstallBindings()
        {
            Container.BindInstance(_hud).AsSingle();
            Container.BindInstance(_settingsWindow).AsSingle();
        }
    }
}