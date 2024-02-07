using Plugins.Animations.Adapters.Volume.Core;
using Plugins.Animations.Extensions;
using UnityEngine;
using UnityEngine.Audio;

namespace Plugins.Animations.Adapters.Volume
{
    public class AdaptedAudioMixerForVolume : VolumeAdapter
    {
        [Header("References")]
        [SerializeField] private AudioMixerGroup _audioMixerGroup;

        [Header("Preferences")]
        [SerializeField] private string _exposedParameter = "Volume";

        public override float Value
        {
            get
            {
                _audioMixerGroup.audioMixer.GetFloat(_exposedParameter, out float dbVolume);
                return AudioExtensions.To01(dbVolume);
            }
            set => _audioMixerGroup.audioMixer.SetFloat(_exposedParameter, AudioExtensions.ToDB(value));
        }
    }
}