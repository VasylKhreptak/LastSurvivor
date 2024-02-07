using Infrastructure.Transition.Core;
using Plugins.Animations.Adapters.Volume;
using UniRx;
using UnityEngine;
using Zenject;

namespace Audio
{
    public class VolumeFader : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AdaptedAudioMixerForVolume _volumeAdapter;

        private ITransitionScreen _transitionScreen;

        [Inject]
        private void Constructor(ITransitionScreen transitionScreen) => _transitionScreen = transitionScreen;

        #region MonoBehaviour

        private void Awake() => _transitionScreen.FadeProgress.Subscribe(x => _volumeAdapter.Value = x).AddTo(this);

        #endregion
    }
}