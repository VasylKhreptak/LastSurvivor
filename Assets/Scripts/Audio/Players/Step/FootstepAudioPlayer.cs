using System;
using System.Collections.Generic;
using Extensions;
using Plugins.AudioService.Core;
using Providers.Velocity.Core;
using Serialization.Collections.KeyValue;
using Terrain.Surfaces;
using Terrain.Surfaces.Core;
using UnityEngine;
using AudioSettings = Plugins.AudioService.Data.AudioSettings;

namespace Audio.Players.Step
{
    public class FootstepAudioPlayer
    {
        private readonly IAudioService _audioService;
        private readonly IVelocityProvider _velocityProvider;
        private readonly Dictionary<SurfaceType, StepAudioPreferences> _surfaceAudioMap;
        private readonly Preferences _preferences;

        public FootstepAudioPlayer(IAudioService audioService, IVelocityProvider velocityProvider, Preferences preferences)
        {
            _audioService = audioService;
            _velocityProvider = velocityProvider;
            _preferences = preferences;
            _surfaceAudioMap = preferences.SurfaceAudioPreferencesPairs.ToDictionary();
        }

        public void TryPlayLeft() => TryPlay(_preferences.LeftFoot.position);

        public void TryPlayRight() => TryPlay(_preferences.RightFoot.position);

        private void TryPlay(Vector3 position)
        {
            if (_velocityProvider.Value.magnitude < _preferences.MinSpeed)
                return;

            Ray ray = new Ray(position, Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hit, _preferences.RayDistance, _preferences.LayerMask) == false)
                return;

            if (hit.collider.TryGetComponent(out Surface surface) == false)
                return;

            SurfaceType surfaceType = surface.Type;

            if (_surfaceAudioMap.TryGetValue(surfaceType, out StepAudioPreferences stepAudio) == false)
                return;

            int id = _audioService.Play(stepAudio.Clips.Random(), position, stepAudio.AudioSettings);

            _audioService.Pitch.TrySet(id, GetPitch(stepAudio));
            _audioService.Volume.TrySet(id, GetVolume(stepAudio) * _preferences.GeneralVolume);
        }

        private float GetPitch(StepAudioPreferences stepAudioPreferences)
        {
            float speed = _velocityProvider.Value.magnitude;
            return stepAudioPreferences.PitchCurve.Evaluate(0f, _preferences.MaxSpeed, speed, stepAudioPreferences.MinPitch,
                stepAudioPreferences.MaxPitch);
        }

        private float GetVolume(StepAudioPreferences stepAudioPreferences)
        {
            float speed = _velocityProvider.Value.magnitude;
            return stepAudioPreferences.VolumeCurve.Evaluate(0f, _preferences.MaxSpeed, speed, stepAudioPreferences.MinVolume,
                stepAudioPreferences.MaxVolume);
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private Transform _leftFoot;
            [SerializeField] private Transform _rightFoot;
            [SerializeField] private LayerMask _layerMask;
            [SerializeField] private float _rayDistance = 0.5f;
            [SerializeField] private float _minSpeed = 0.2f;
            [SerializeField] private float _maxSpeed = 4f;
            [SerializeField] private float _generalVolume = 1f;
            [SerializeField] private KeyValuePairs<SurfaceType, StepAudioPreferences> _surfaceAudioPreferencesPairs;

            public Transform LeftFoot => _leftFoot;
            public Transform RightFoot => _rightFoot;
            public LayerMask LayerMask => _layerMask;
            public float RayDistance => _rayDistance;
            public float MinSpeed => _minSpeed;
            public float MaxSpeed => _maxSpeed;
            public float GeneralVolume => _generalVolume;
            public KeyValuePairs<SurfaceType, StepAudioPreferences> SurfaceAudioPreferencesPairs => _surfaceAudioPreferencesPairs;
        }

        [Serializable]
        public class StepAudioPreferences
        {
            [SerializeField] private AudioClip[] _clips;
            [SerializeField] private float _minPitch = 0.8f;
            [SerializeField] private float _maxPitch = 1.2f;
            [SerializeField] private float _minVolume = 0.2f;
            [SerializeField] private float _maxVolume = 1f;
            [SerializeField] private AudioSettings _audioSettings;
            [SerializeField] private AnimationCurve _pitchCurve;
            [SerializeField] private AnimationCurve _volumeCurve;

            public AudioClip[] Clips => _clips;
            public float MinPitch => _minPitch;
            public float MaxPitch => _maxPitch;
            public float MinVolume => _minVolume;
            public float MaxVolume => _maxVolume;
            public AudioSettings AudioSettings => _audioSettings;
            public AnimationCurve PitchCurve => _pitchCurve;
            public AnimationCurve VolumeCurve => _volumeCurve;
        }
    }
}