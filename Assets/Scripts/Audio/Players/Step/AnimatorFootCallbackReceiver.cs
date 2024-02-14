using UnityEngine;
using Zenject;

namespace Audio.Players.Step
{
    public class AnimatorFootCallbackReceiver : MonoBehaviour
    {
        private FootstepAudioPlayer _audioPlayer;

        [Inject]
        private void Constructor(FootstepAudioPlayer audioPlayer) => _audioPlayer = audioPlayer;

        private void PlayLeftStepSound() => _audioPlayer.TryPlayLeft();

        private void PlayRightStepSound() => _audioPlayer.TryPlayRight();
    }
}