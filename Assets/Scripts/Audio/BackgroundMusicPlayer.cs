using System;
using DG.Tweening;
using Plugins.AudioService.Core;
using UnityEngine;
using Zenject;
using AudioSettings = Plugins.AudioService.Data.AudioSettings;

namespace Audio
{
    public class BackgroundMusicPlayer : IInitializable
    {
        private readonly IAudioService _audioService;
        private readonly Preferences _preferences;

        public BackgroundMusicPlayer(IAudioService audioService, Preferences preferences)
        {
            _audioService = audioService;
            _preferences = preferences;
        }

        public void Initialize() => Play();

        private void Play()
        {
            int id = _audioService.Play(_preferences.Clip, _preferences.AudioSettings);

            SetVolume(id, 0f);

            DOTween
                .To(() => GetVolume(id), x => SetVolume(id, x), _preferences.AudioSettings.Volume, _preferences.VolumeUpDuration)
                .SetEase(Ease.Linear)
                .SetDelay(_preferences.Delay)
                .Play();
        }

        private float GetVolume(int id)
        {
            _audioService.Volume.TryGet(id, out float volume);

            return volume;
        }

        private void SetVolume(int id, float volume) => _audioService.Volume.TrySet(id, volume);

        [Serializable]
        public class Preferences
        {
            [SerializeField] private AudioClip _clip;
            [SerializeField] private float _volumeUpDuration = 1f;
            [SerializeField] private float _delay = 0.5f;
            [SerializeField] private AudioSettings _audioSettings;

            public AudioClip Clip => _clip;
            public float VolumeUpDuration => _volumeUpDuration;
            public float Delay => _delay;
            public AudioSettings AudioSettings => _audioSettings;
        }
    }
}