using UnityEngine;
using Zenject;

namespace Audio.Players.Step
{
    public class AnimatorFootCallbackReceiver : MonoBehaviour
    {
        private StepAudioPlayer _audioPlayer;

        [Inject]
        private void Constructor(StepAudioPlayer audioPlayer) => _audioPlayer = audioPlayer;

        private void PlayLeftStepSound() => _audioPlayer.PlayLeft();

        private void PlayRightStepSound() => _audioPlayer.PlayRight();
    }
}