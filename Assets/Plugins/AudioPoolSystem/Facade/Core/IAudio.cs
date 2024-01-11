using Plugins.Timer;
using UnityEngine;
using UnityEngine.Audio;
using AudioSettings = Plugins.AudioPoolSystem.Data.AudioSettings;

namespace Plugins.AudioPoolSystem.Facade.Core
{
    public interface IAudio
    {
        public void Play(float delay = 0f);
        public void Stop();
        public void Pause();
        public void Resume();
        public void ApplySettings(AudioSettings settings);

        public IReadonlyTimer Timer { get; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public AudioClip Clip { get; set; }
        public AudioMixerGroup AudioMixerGroup { get; set; }
        public bool Mute { get; set; }
        public bool BypassEffects { get; set; }
        public bool BypassListenerEffects { get; set; }
        public bool BypassReverbZones { get; set; }
        public bool Loop { get; set; }
        public bool IsPlaying { get; }
        public int Priority { get; set; }
        public float Volume { get; set; }
        public float Pitch { get; set; }
        public float StereoPan { get; set; }
        public float SpatialBlend { get; set; }
        public float ReverbZoneMix { get; set; }
        public float DopplerLevel { get; set; }
        public float Spread { get; set; }
        public AudioRolloffMode RolloffMode { get; set; }
        public float MinDistance { get; set; }
        public float MaxDistance { get; set; }
    }
}