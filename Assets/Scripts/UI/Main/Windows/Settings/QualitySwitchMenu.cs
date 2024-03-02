using System.Collections.Generic;
using Infrastructure.Services.PersistentData.Core;
using Serialization.Collections.KeyValue;
using Settings;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.Main.Windows.Settings
{
    public class QualitySwitchMenu : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SwitchMenu.SwitchMenu _switchMenu;
        [SerializeField] private TMP_Text _qualityNameTmp;

        [Header("Preferences")]
        [SerializeField] private KeyValuePairs<int, string> _qualityLevelsPairs;

        private IPersistentDataService _persistentDataService;
        private SettingsApplier _settingsApplier;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService, SettingsApplier settingsApplier)
        {
            _persistentDataService = persistentDataService;
            _settingsApplier = settingsApplier;
        }

        private Dictionary<int, string> _qualityLevelsMap;

        #region MonoBehaviour

        private void Awake()
        {
            _qualityLevelsMap = _qualityLevelsPairs.ToDictionary();
            _switchMenu.SetIndex(_persistentDataService.Data.Settings.QualityLevel);
            UpdateText();
        }

        private void OnEnable() => _switchMenu.OnIndexChanged += SetQualityLevel;

        private void OnDisable() => _switchMenu.OnIndexChanged -= SetQualityLevel;

        #endregion

        private void SetQualityLevel(int qualityLevel)
        {
            _persistentDataService.Data.Settings.QualityLevel = qualityLevel;
            _settingsApplier.ApplyQualityLevel();
            UpdateText();
        }

        private void UpdateText() => _qualityNameTmp.text = _qualityLevelsMap[_switchMenu.CurrentIndex];
    }
}