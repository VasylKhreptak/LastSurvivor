using Plugins.Animations.Adapters.Volume.Core;
using Plugins.Animations.Extensions;
using UnityEngine;
using UnityEngine.Audio;

namespace Plugins.Animations.Adapters.Volume
{
    public class AdaptedAudioMixerForVolume : VolumeAdapter
    {
        [Header("References")]
        [SerializeField] private AudioMixer _audioMixer;

        [Header("Preferences")]
        [SerializeField] private string _exposedParameter = "Volume";

        public override float Value
        {
            get
            {
                _audioMixer.GetFloat(_exposedParameter, out float dbVolume);
                return AudioExtensions.To01(dbVolume);
            }
            set => _audioMixer.SetFloat(_exposedParameter, AudioExtensions.ToDB(value));
        }
    }
}