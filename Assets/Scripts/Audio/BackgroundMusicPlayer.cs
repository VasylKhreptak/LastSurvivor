using System;
using DG.Tweening;
using Plugins.AudioService.Core;
using UnityEngine;
using AudioSettings = Plugins.AudioService.Data.AudioSettings;

namespace Audio
{
    public class BackgroundMusicPlayer
    {
        private readonly IAudioService _audioService;
        private readonly Preferences _preferences;

        public BackgroundMusicPlayer(IAudioService audioService, Preferences preferences)
        {
            _audioService = audioService;
            _preferences = preferences;
        }

        private int _id;

        public void Play()
        {
            Stop();

            _id = _audioService.Play(_preferences.Clip, _preferences.AudioSettings);

            SetVolume(_id, 0f);

            DOTween
                .To(() => GetVolume(_id), x => SetVolume(_id, x), _preferences.AudioSettings.Volume, _preferences.VolumeUpDuration)
                .SetEase(Ease.Linear)
                .Play();
        }

        public void Stop() => _audioService.Stop(_id);

        public bool IsPlaying() => _audioService.IsActive(_id);

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
            [SerializeField] private AudioSettings _audioSettings;

            public AudioClip Clip => _clip;
            public float VolumeUpDuration => _volumeUpDuration;
            public AudioSettings AudioSettings => _audioSettings;
        }
    }
}