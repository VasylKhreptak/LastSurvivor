using Plugins.Animations.Adapters.Volume.Core;
using UnityEngine;

namespace Plugins.Animations.Adapters.Volume
{
    public class AdaptedAudioSourceForVolume : VolumeAdapter
    {
        [Header("References")]
        [SerializeField] private AudioSource _audioSource;

        #region MonoBehaviour

        private void OnValidate() => _audioSource ??= GetComponent<AudioSource>();

        #endregion

        public override float Value { get => _audioSource.volume; set => _audioSource.volume = value; }
    }
}