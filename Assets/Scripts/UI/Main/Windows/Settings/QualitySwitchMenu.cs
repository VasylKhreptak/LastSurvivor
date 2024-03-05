using System.Collections.Generic;
using Infrastructure.Services.PersistentData.Core;
using Serialization.Collections.KeyValue;
using Settings;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using Zenject;

namespace UI.Main.Windows.Settings
{
    public class QualitySwitchMenu : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SwitchMenu.SwitchMenu _switchMenu;
        [SerializeField] private TMP_Text _qualityNameTmp;

        [Header("Preferences")]
        [SerializeField] private KeyValuePairs<int, LocalizedString> _qualityLevelsPairs;

        private IPersistentDataService _persistentDataService;
        private SettingsApplier _settingsApplier;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService, SettingsApplier settingsApplier)
        {
            _persistentDataService = persistentDataService;
            _settingsApplier = settingsApplier;
        }

        private Dictionary<int, LocalizedString> _qualityLevelsMap;

        private LocalizedString _currentLocalizedString;

        #region MonoBehaviour

        private void OnValidate() => _switchMenu ??= GetComponent<SwitchMenu.SwitchMenu>();

        private void Awake()
        {
            _qualityLevelsMap = _qualityLevelsPairs.ToDictionary();
            int qualityLevel = _persistentDataService.Data.Settings.QualityLevel;
            _switchMenu.SetIndex(qualityLevel);
            LocalizedString localizedString = _qualityLevelsMap[qualityLevel];
            StartObservingLocalizedString(localizedString);
        }

        private void OnEnable() => _switchMenu.OnIndexChanged += SetQualityLevel;

        private void OnDisable()
        {
            _switchMenu.OnIndexChanged -= SetQualityLevel;
            StopObservingLocalizedString(_currentLocalizedString);
        }

        #endregion

        private void SetQualityLevel(int qualityLevel)
        {
            _settingsApplier.ApplyQualityLevel(qualityLevel);
            LocalizedString localizedString = _qualityLevelsMap[qualityLevel];
            StopObservingLocalizedString(_currentLocalizedString);
            StartObservingLocalizedString(localizedString);
            _currentLocalizedString = localizedString;
        }

        private void StartObservingLocalizedString(LocalizedString localizedString) => localizedString.StringChanged += UpdateText;

        private void StopObservingLocalizedString(LocalizedString localizedString)
        {
            if (localizedString == null)
                return;

            localizedString.StringChanged -= UpdateText;
        }

        private void UpdateText(string text) => _qualityNameTmp.text = text;
    }
}