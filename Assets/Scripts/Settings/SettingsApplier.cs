using System;
using System.Collections;
using Analytics;
using Extensions;
using Firebase.Analytics;
using Infrastructure.Coroutines.Runner.Core;
using Infrastructure.Services.PersistentData.Core;
using Plugins.AudioService.Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;

namespace Settings
{
    public class SettingsApplier
    {
        private const float ThrottleTime = 0.5f;

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

        private IDisposable _musicVolumeEventDelaySubscription;
        private IDisposable _soundVolumeEventDelaySubscription;

        public void Apply(Infrastructure.Data.Persistent.Settings settings)
        {
            ApplyMusicVolume(settings.MusicVolume);
            ApplySoundVolume(settings.SoundVolume);
            ApplyVibration(settings.IsVibrationEnabled);
            ApplyQualityLevel(settings.QualityLevel);
            ApplyLocale(settings.LocaleIndex);
            ApplyScreenSleepTimeout(settings.SleepTimeout);
        }

        public void ApplyMusicVolume(float volume)
        {
            _persistentDataService.Data.Settings.MusicVolume = volume;
            ThrottleCallback(ref _musicVolumeEventDelaySubscription, () => LogVolumeChangedEvent(MusicVolumeParameter, volume));
            ApplyVolume(MusicVolumeParameter, volume);
        }

        public void ApplySoundVolume(float volume)
        {
            _persistentDataService.Data.Settings.SoundVolume = volume;
            ThrottleCallback(ref _soundVolumeEventDelaySubscription, () => LogVolumeChangedEvent(SfxVolumeParameter, volume));
            ApplyVolume(SfxVolumeParameter, volume);
        }

        public void ApplyVibration(bool enabled)
        {
            _persistentDataService.Data.Settings.IsVibrationEnabled = enabled;
            FirebaseAnalytics.LogEvent(AnalyticEvents.SetVibration, new Parameter(AnalyticParameters.Value, enabled.ToInt()));
        }

        public void ApplyQualityLevel(int level)
        {
            _persistentDataService.Data.Settings.QualityLevel = level;
            QualitySettings.SetQualityLevel(level);
            FirebaseAnalytics.LogEvent(AnalyticEvents.SetQuality, new Parameter(AnalyticParameters.Value, level));
        }

        public void ApplyLocale(int localeIndex)
        {
            _persistentDataService.Data.Settings.LocaleIndex = localeIndex;
            _coroutineRunner.StartCoroutine(ApplyLocaleRoutine(localeIndex));
        }

        public void ApplyScreenSleepTimeout(int timeout)
        {
            _persistentDataService.Data.Settings.SleepTimeout = timeout;
            Screen.sleepTimeout = timeout;
        }

        private IEnumerator ApplyLocaleRoutine(int localeIndex)
        {
            yield return LocalizationSettings.InitializationOperation;

            LocalizationSettings.SelectedLocale =
                LocalizationSettings.AvailableLocales.Locales[localeIndex];

            FirebaseAnalytics.LogEvent(AnalyticEvents.SetLanguage, new Parameter(AnalyticParameters.Value, localeIndex));
        }

        private void ApplyVolume(string parameter, float volume)
        {
            _audioMixer.SetFloat(parameter, AudioExtensions.ToDB(volume));
        }

        private void LogVolumeChangedEvent(string name, float volume)
        {
            FirebaseAnalytics.LogEvent(AnalyticEvents.SetVolume,
                new Parameter(AnalyticParameters.Name, name),
                new Parameter(AnalyticParameters.Value, volume));
        }

        private void ThrottleCallback(ref IDisposable subscription, Action action)
        {
            subscription?.Dispose();
            subscription = Observable
                .Timer(TimeSpan.FromSeconds(ThrottleTime))
                .Subscribe(_ => action?.Invoke());
        }
    }
}