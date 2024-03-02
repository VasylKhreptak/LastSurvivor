using System.Collections;
using Infrastructure.Coroutines.Runner.Core;
using Infrastructure.Services.PersistentData.Core;
using Plugins.AudioService.Extensions;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;

namespace Settings
{
    public class SettingsApplier
    {
        private const string MusicVolumeParameter = "Music_Volume";
        private const string SfxVolumeParameter = "SFX_Volume";

        private readonly IPersistentDataService _persistentDataService;
        private readonly AudioMixer _audioMixer;
        private readonly ICoroutineRunner _coroutineRunner;

        public SettingsApplier(IPersistentDataService persistentDataService, AudioMixer audioMixer, ICoroutineRunner coroutineRunner)
        {
            _persistentDataService = persistentDataService;
            _audioMixer = audioMixer;
            _coroutineRunner = coroutineRunner;
        }

        public void Apply()
        {
            ApplyMusicVolume();
            ApplySfxVolume();
            ApplyQualityLevel();
            ApplyLocale();
        }

        public void ApplyMusicVolume() => ApplyVolume(MusicVolumeParameter, _persistentDataService.Data.Settings.MusicVolume);

        public void ApplySfxVolume() => ApplyVolume(SfxVolumeParameter, _persistentDataService.Data.Settings.SoundVolume);

        public void ApplyQualityLevel() => QualitySettings.SetQualityLevel(_persistentDataService.Data.Settings.QualityLevel);

        public void ApplyLocale() => _coroutineRunner.StartCoroutine(ApplyLocaleRoutine());

        private IEnumerator ApplyLocaleRoutine()
        {
            yield return LocalizationSettings.InitializationOperation;

            LocalizationSettings.SelectedLocale =
                LocalizationSettings.AvailableLocales.Locales[_persistentDataService.Data.Settings.LocaleIndex];
        }

        private void ApplyVolume(string parameter, float volume) => _audioMixer.SetFloat(parameter, AudioExtensions.ToDB(volume));
    }
}