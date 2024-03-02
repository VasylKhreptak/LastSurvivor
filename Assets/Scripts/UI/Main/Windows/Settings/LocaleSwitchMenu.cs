using Infrastructure.Services.PersistentData.Core;
using Settings;
using UnityEngine;
using Zenject;

namespace UI.Main.Windows.Settings
{
    public class LocaleSwitchMenu : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SwitchMenu.SwitchMenu _switchMenu;

        private IPersistentDataService _persistentDataService;
        private SettingsApplier _settingsApplier;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService, SettingsApplier settingsApplier)
        {
            _persistentDataService = persistentDataService;
            _settingsApplier = settingsApplier;
        }

        #region MonoBehaviour

        private void OnValidate() => _switchMenu ??= GetComponent<SwitchMenu.SwitchMenu>();

        private void Awake() => _switchMenu.SetIndex(_persistentDataService.Data.Settings.QualityLevel);

        private void OnEnable() => _switchMenu.OnIndexChanged += SetLocale;

        private void OnDisable() => _switchMenu.OnIndexChanged -= SetLocale;

        #endregion

        private void SetLocale(int localeIndex)
        {
            _persistentDataService.Data.Settings.LocaleIndex = localeIndex;
            _settingsApplier.ApplyLocale();
        }
    }
}