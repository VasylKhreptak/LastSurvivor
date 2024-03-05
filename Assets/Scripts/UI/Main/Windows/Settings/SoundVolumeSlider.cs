using Infrastructure.Services.PersistentData.Core;
using Settings;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Main.Windows.Settings
{
    public class SoundVolumeSlider : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Slider _slider;

        private IPersistentDataService _persistentDataService;
        private SettingsApplier _settingsApplier;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService, SettingsApplier settingsApplier)
        {
            _persistentDataService = persistentDataService;
            _settingsApplier = settingsApplier;
        }

        #region MonoBehaviour

        private void OnValidate() => _slider ??= GetComponent<Slider>();

        private void Awake() => _slider.value = _persistentDataService.Data.Settings.SoundVolume;

        private void OnEnable() => _slider.onValueChanged.AddListener(SetVolume);

        private void OnDisable() => _slider.onValueChanged.RemoveListener(SetVolume);

        #endregion

        private void SetVolume(float volume) => _settingsApplier.ApplySoundVolume(volume);
    }
}