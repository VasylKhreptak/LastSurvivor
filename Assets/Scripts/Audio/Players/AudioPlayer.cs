using System;
using Extensions;
using Plugins.AudioService.Core;
using UnityEngine;
using AudioSettings = Plugins.AudioService.Data.AudioSettings;

namespace Audio.Players
{
    public class AudioPlayer
    {
        private readonly IAudioService _audioService;
        private readonly Preferences _preferences;

        public AudioPlayer(IAudioService audioService, Preferences preferences)
        {
            _audioService = audioService;
            _preferences = preferences;
        }

        private int _lastPlayIndex = -1;

        public void Play() => Play(Vector3.zero);

        public void Play(Vector3 position)
        {
            _lastPlayIndex = _audioService.Play(_preferences.Clips.Random(), position, _preferences.AudioSettings);
        }

        public void Stop() => _audioService.Stop(_lastPlayIndex);

        [Serializable]
        public class Preferences
        {
            [SerializeField] private AudioClip[] _clips;
            [SerializeField] private AudioSettings _audioSettings;

            public AudioClip[] Clips => _clips;
            public AudioSettings AudioSettings => _audioSettings;
        }
    }
}