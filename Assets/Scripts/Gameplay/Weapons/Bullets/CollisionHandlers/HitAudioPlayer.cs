using System;
using Extensions;
using Plugins.AudioService.Core;
using Serialization.Collections.Dictionary;
using Terrain.Surfaces.Core;
using UnityEngine;
using AudioSettings = Plugins.AudioService.Data.AudioSettings;

namespace Gameplay.Weapons.Bullets.CollisionHandlers
{
    public class HitAudioPlayer
    {
        private readonly IAudioService _audioService;
        private readonly Preferences _preferences;

        public HitAudioPlayer(IAudioService audioService, Preferences preferences)
        {
            _audioService = audioService;
            _preferences = preferences;
        }

        public void Play(Collision collision)
        {
            ISurface surface = collision.gameObject.GetComponent<ISurface>();

            if (surface == null)
                return;

            if (_preferences.TryGetClips(surface.Type, out AudioClip[] audioClips) == false)
                return;

            _audioService.Play(audioClips.Random(), collision.contacts[0].point, _preferences.AudioSettings);
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private SerializedDictionary<SurfaceType, AudioClip[]> _surfaceAudioMap;
            [SerializeField] private AudioSettings _audioSettings;

            public bool TryGetClips(SurfaceType surface, out AudioClip[] audioClips) =>
                _surfaceAudioMap.TryGetValue(surface, out audioClips);

            public AudioSettings AudioSettings => _audioSettings;
        }
    }
}