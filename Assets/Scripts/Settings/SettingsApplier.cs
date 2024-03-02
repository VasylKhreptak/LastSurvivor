using Infrastructure.Services.PersistentData.Core;
using Plugins.AudioService.Extensions;
using UnityEngine;
using UnityEngine.Audio;

namespace Settings
{
    public class SettingsApplier
    {
        private const string MusicVolumeParameter = "Music_Volume";
        private const string SfxVolumeParameter = "SFX_Volume";

        private readonly IPersistentDataService _persistentDataService;
        private readonly AudioMixer _audioMixer;

        public SettingsApplier(IPersistentDataService persistentDataService, AudioMixer audioMixer)
        {
            _persistentDataService = persistentDataService;
            _audioMixer = audioMixer;
        }

        public void Apply()
        {
            ApplyMusicVolume();
            ApplySfxVolume();
            ApplyQualityLevel();
        }

        public void ApplyMusicVolume() => ApplyVolume(MusicVolumeParameter, _persistentDataService.Data.Settings.MusicVolume);

        public void ApplySfxVolume() => ApplyVolume(SfxVolumeParameter, _persistentDataService.Data.Settings.SoundVolume);

        public void ApplyQualityLevel() => QualitySettings.SetQualityLevel(_persistentDataService.Data.Settings.QualityLevel);

        private void ApplyVolume(string parameter, float volume) => _audioMixer.SetFloat(parameter, AudioExtensions.ToDB(volume));
    }
}