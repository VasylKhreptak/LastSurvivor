using System;
using System.Collections.Generic;
using Extensions;
using Plugins.AudioService.Core;
using Providers.Velocity.Core;
using Serialization.Collections.KeyValue;
using SRF;
using Terrain.Surfaces;
using Terrain.Surfaces.Core;
using UnityEngine;
using AudioSettings = Plugins.AudioService.Data.AudioSettings;

namespace Audio.Players.Step
{
    public class StepAudioPlayer
    {
        private readonly IAudioService _audioService;
        private readonly IVelocityProvider _velocityProvider;
        private readonly Dictionary<SurfaceType, AudioClip[]> _surfaceClipsMap;
        private readonly Preferences _preferences;

        public StepAudioPlayer(IAudioService audioService, IVelocityProvider velocityProvider, Preferences preferences)
        {
            _audioService = audioService;
            _velocityProvider = velocityProvider;
            _preferences = preferences;
            _surfaceClipsMap = preferences.SurfaceClipsPairs.ToDictionary();
        }

        public void PlayLeft() => Play(_preferences.LeftFoot.position);

        public void PlayRight() => Play(_preferences.RightFoot.position);

        private void Play(Vector3 position)
        {
            Ray ray = new Ray(position, Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hit, _preferences.RayDistance, _preferences.LayerMask) == false)
                return;

            SurfaceType surfaceType = hit.collider.GetComponent<Surface>().Type;

            if (_surfaceClipsMap.TryGetValue(surfaceType, out AudioClip[] clips) == false)
                return;

            _preferences.AudioSettings.Pitch = GetPitch();
            _audioService.Play(clips.Random(), position, _preferences.AudioSettings);
        }

        private float GetPitch()
        {
            float speed = _velocityProvider.Value.magnitude;
            return _preferences.PitchCurve.Evaluate(0f, _preferences.MaxSpeed, speed, _preferences.MinPitch, _preferences.MaxPitch);
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private Transform _leftFoot;
            [SerializeField] private Transform _rightFoot;
            [SerializeField] private LayerMask _layerMask;
            [SerializeField] private float _rayDistance = 0.5f;
            [SerializeField] private float _maxSpeed = 4f;
            [SerializeField] private float _minPitch = 1f;
            [SerializeField] private float _maxPitch = 1.2f;
            [SerializeField] private AnimationCurve _pitchCurve;
            [SerializeField] private KeyValuePairs<SurfaceType, AudioClip[]> _surfaceClipsPairs;
            [SerializeField] private AudioSettings _audioSettings;

            public Transform LeftFoot => _leftFoot;
            public Transform RightFoot => _rightFoot;
            public LayerMask LayerMask => _layerMask;
            public float RayDistance => _rayDistance;
            public float MaxSpeed => _maxSpeed;
            public float MinPitch => _minPitch;
            public float MaxPitch => _maxPitch;
            public AnimationCurve PitchCurve => _pitchCurve;
            public KeyValuePairs<SurfaceType, AudioClip[]> SurfaceClipsPairs => _surfaceClipsPairs;
            public AudioSettings AudioSettings => _audioSettings;
        }
    }
}