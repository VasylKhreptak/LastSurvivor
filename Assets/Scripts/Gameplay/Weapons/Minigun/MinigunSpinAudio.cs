using System;
using Plugins.AudioService.Core;
using UnityEngine;
using Zenject;
using Zenject.Infrastructure.Toggleable.Core;
using AudioSettings = Plugins.AudioService.Data.AudioSettings;

namespace Gameplay.Weapons.Minigun
{
    public class MinigunSpinAudio : IEnableable, ITickable, IDisableable
    {
        private readonly IAudioService _audioService;
        private readonly BarrelSpiner _barrelSpiner;
        private readonly Preferences _preferences;

        public MinigunSpinAudio(IAudioService audioService, BarrelSpiner barrelSpiner, Preferences preferences)
        {
            _audioService = audioService;
            _barrelSpiner = barrelSpiner;
            _preferences = preferences;
        }

        private int _audioID;
        private float _spinProgress;

        public void Enable()
        {
            _audioID = _audioService.Play(_preferences.Clip, _preferences.Settings);
            _audioService.Pitch.TrySet(_audioID, _preferences.Settings.Pitch);
        }

        public void Tick()
        {
            _spinProgress = _barrelSpiner.RotateSpeed / _barrelSpiner.Settings.MaxRotateSpeed;
            _audioService.Pitch.TrySet(_audioID, _preferences.PitchCurve.Evaluate(_spinProgress) * _preferences.Settings.Pitch);
            _audioService.Position.TrySet(_audioID, _barrelSpiner.Settings.BarrelTransform.position);
            _audioService.Volume.TrySet(_audioID, _preferences.VolumeCurve.Evaluate(_spinProgress) * _preferences.Settings.Volume);
        }

        public void Disable() => _audioService.Stop(_audioID);

        [Serializable]
        public class Preferences
        {
            [SerializeField] private AudioClip _clip;
            [SerializeField] private AudioSettings _settings;
            [SerializeField] private AnimationCurve _pitchCurve;
            [SerializeField] private AnimationCurve _volumeCurve;

            public AudioClip Clip => _clip;
            public AudioSettings Settings => _settings;
            public AnimationCurve PitchCurve => _pitchCurve;
            public AnimationCurve VolumeCurve => _volumeCurve;
        }
    }
}