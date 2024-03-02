using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Transition.Core;
using Plugins.Animations.Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Audio
{
    public class SfxVolumeFader : MonoBehaviour
    {
        private const string SfxVolumeParameter = "SFX_Volume";

        private ITransitionScreen _transitionScreen;
        private IPersistentDataService _persistentDataService;
        private AudioMixer _audioMixer;

        [Inject]
        private void Constructor(ITransitionScreen transitionScreen, IPersistentDataService persistentDataService,
            AudioMixer audioMixer)
        {
            _transitionScreen = transitionScreen;
            _persistentDataService = persistentDataService;
            _audioMixer = audioMixer;
        }

        #region MonoBehaviour

        private void Awake() => _transitionScreen.FadeProgress.Subscribe(OnFadeProgressChanged).AddTo(this);

        #endregion

        private void OnFadeProgressChanged(float progress)
        {
            _audioMixer.SetFloat(SfxVolumeParameter,
                AudioExtensions.ToDB(progress * _persistentDataService.Data.Settings.SoundVolume));
        }
    }
}