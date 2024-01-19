using Plugins.AudioService.Core;
using UnityEngine;
using Zenject;

namespace Audio.Players
{
    public class MonoAudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioPlayer.Preferences _preferences;

        private IAudioService _audioService;

        [Inject]
        private void Constructor(IAudioService audioService)
        {
            _audioService = audioService;
        }

        private AudioPlayer _audioPlayer;

        #region MonoBehaviour

        private void Awake() => _audioPlayer = new AudioPlayer(_audioService, _preferences);

        private void OnEnable() => _audioPlayer.Play(transform.position);

        private void OnDisable() => _audioPlayer.Stop();

        #endregion
    }
}