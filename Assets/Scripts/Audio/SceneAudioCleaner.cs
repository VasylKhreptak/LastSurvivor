using System.Collections.Generic;
using Plugins.AudioService.Core;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Audio
{
    public class SceneAudioCleaner : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] private List<AudioMixerGroup> _targetMixerGroups;

        private HashSet<AudioMixerGroup> _targetMixerGroupsSet;

        private IAudioService _audioService;

        [Inject]
        private void Constructor(IAudioService audioService)
        {
            _audioService = audioService;
        }

        #region MonoBehaviour

        private void Awake() => _targetMixerGroupsSet = new HashSet<AudioMixerGroup>(_targetMixerGroups);

        private void OnDestroy() => Clear();

        #endregion

        private void Clear() => _audioService.Stop(x => _targetMixerGroupsSet.Contains(x.AudioMixerGroup));
    }
}