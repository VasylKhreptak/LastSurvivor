using Platforms;
using Platforms.HelicopterPlatform;
using Plugins.Animations;
using Plugins.Animations.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Animations.Platforms
{
    public class HelicopterScaleAnimation : MonoBehaviour
    {
        [Header("Animations")]
        [SerializeField] private ScaleAnimation _scaleUpAnimation;
        [SerializeField] private ScaleAnimation _scaleDownAnimation;

        private OilBarrelReceiver _oilBarrelReceiver;
        private ReceiveZone _receiveZone;

        [Inject]
        private void Constructor(OilBarrelReceiver oilBarrelReceiver, ReceiveZone receiveZone)
        {
            _oilBarrelReceiver = oilBarrelReceiver;
            _receiveZone = receiveZone;
        }

        private IAnimation _scaleAnimation;

        #region MonoBehaviour

        private void Awake() => _scaleAnimation = new AnimationSequence(_scaleUpAnimation, _scaleDownAnimation);

        private void OnEnable() => StartObserving();

        private void OnDisable()
        {
            StopObserving();
            _scaleAnimation.SetEndState();
        }

        #endregion

        private void StartObserving()
        {
            _oilBarrelReceiver.OnReceivedBarrel += Punch;
            _receiveZone.OnReceivedAll += Punch;
        }

        private void StopObserving()
        {
            _oilBarrelReceiver.OnReceivedBarrel -= Punch;
            _receiveZone.OnReceivedAll -= Punch;
        }

        [Button]
        private void Punch() => _scaleAnimation.PlayForward();
    }
}