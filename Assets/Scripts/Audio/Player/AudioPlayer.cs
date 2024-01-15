using System;
using System.Collections.Generic;
using Audio.Player.Core;
using Extensions;
using Plugins.AudioPoolSystem.Core;
using Plugins.AudioPoolSystem.Facade.Core;
using UnityEngine;
using AudioSettings = Plugins.AudioPoolSystem.Data.AudioSettings;

namespace Audio.Player
{
    public class AudioPlayer : IAudioPlayer, IDisposable
    {
        private readonly IAudioPool _audioPool;
        private readonly Preferences _preferences;

        public AudioPlayer(IAudioPool audioPool, Preferences preferences)
        {
            _audioPool = audioPool;
            _preferences = preferences;
        }

        public void Play()
        {
            IAudio audio = _audioPool.Get();
            audio.ApplySettings(_preferences.AudioSettings);
            audio.Clip = _preferences.Clips.Random();
            audio.Play();
        }

        public void Stop() { }

        public void Dispose() => Stop();

        [Serializable]
        public class Preferences
        {
            public List<AudioClip> Clips;
            public AudioSettings AudioSettings;
        }
    }
}