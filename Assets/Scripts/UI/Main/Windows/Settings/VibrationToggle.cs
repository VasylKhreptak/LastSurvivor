﻿using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.Vibration.Core;
using Lofelt.NiceVibrations;
using Settings;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Main.Windows.Settings
{
    public class VibrationToggle : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Toggle _toggle;

        private IPersistentDataService _persistentDataService;
        private SettingsApplier _settingsApplier;
        private IVibrationService _vibrationService;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService, SettingsApplier settingsApplier,
            IVibrationService vibrationService)
        {
            _persistentDataService = persistentDataService;
            _settingsApplier = settingsApplier;
            _vibrationService = vibrationService;
        }

        #region MonoBehaviour

        private void OnValidate() => _toggle ??= GetComponent<Toggle>();

        private void Awake() => _toggle.isOn = _persistentDataService.Data.Settings.IsVibrationEnabled;

        private void OnEnable() => _toggle.onValueChanged.AddListener(SetVibrationActive);

        private void OnDisable() => _toggle.onValueChanged.RemoveListener(SetVibrationActive);

        #endregion

        private void SetVibrationActive(bool enabled)
        {
            _settingsApplier.ApplyVibration(enabled);

            if (enabled)
                _vibrationService.Vibrate(HapticPatterns.PresetType.RigidImpact);
        }
    }
}