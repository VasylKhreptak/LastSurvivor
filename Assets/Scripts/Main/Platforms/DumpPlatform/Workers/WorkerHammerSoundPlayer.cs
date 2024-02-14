using Audio.Players;
using Plugins.AudioService.Core;
using UnityEngine;
using Zenject;

namespace Main.Platforms.DumpPlatform.Workers
{
    public class WorkerHammerSoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioPlayer.Preferences _audioPreferences;

        private IAudioService _audioService;

        [Inject]
        private void Constructor(IAudioService audioService)
        {
            _audioService = audioService;
        }

        private AudioPlayer _audioPlayer;

        #region MonoBehaviour

        private void Awake() => _audioPlayer = new AudioPlayer(_audioService, _audioPreferences);

        #endregion

        private void PlaySound() => _audioPlayer.Play(transform.position);
    }
}