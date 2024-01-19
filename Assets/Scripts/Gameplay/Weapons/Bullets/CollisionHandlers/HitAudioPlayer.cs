using System;
using System.Collections.Generic;
using Extensions;
using Plugins.AudioService.Core;
using Serialization.Collections.KeyValue;
using Terrain.Surfaces.Core;
using UnityEngine;
using AudioSettings = Plugins.AudioService.Data.AudioSettings;

namespace Gameplay.Weapons.Bullets.CollisionHandlers
{
    public class HitAudioPlayer
    {
        private readonly IAudioService _audioService;
        private readonly Preferences _preferences;

        private readonly Dictionary<SurfaceType, AudioClip[]> _surfaceAudiosMap;

        public HitAudioPlayer(IAudioService audioService, Preferences preferences)
        {
            _audioService = audioService;
            _preferences = preferences;

            _surfaceAudiosMap = _preferences.SurfaceAudioPairs.ToDictionary();
        }

        public void Play(Collision collision)
        {
            ISurface surface = collision.gameObject.GetComponent<ISurface>();

            if (surface == null)
                return;

            if (_surfaceAudiosMap.TryGetValue(surface.Type, out AudioClip[] audioClips) == false)
                return;

            _audioService.Play(audioClips.Random(), collision.contacts[0].point, _preferences.AudioSettings);
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private KeyValuePairs<SurfaceType, AudioClip[]> _surfaceAudioPairs;
            [SerializeField] private AudioSettings _audioSettings;

            public KeyValuePairs<SurfaceType, AudioClip[]> SurfaceAudioPairs => _surfaceAudioPairs;
            public AudioSettings AudioSettings => _audioSettings;
        }
    }
}