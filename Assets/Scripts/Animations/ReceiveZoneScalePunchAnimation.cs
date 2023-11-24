using Platforms.Zones;
using Plugins.Animations;
using Plugins.Animations.Core;
using UnityEngine;
using Zenject;

namespace Animations
{
    public class ReceiveZoneScalePunchAnimation : MonoBehaviour
    {
        [Header("Animations")]
        [SerializeField] private ScaleAnimation _scaleUpAnimation;
        [SerializeField] private ScaleAnimation _scaleDownAnimation;

        private ReceiveZone _receiveZone;

        [Inject]
        private void Constructor(ReceiveZone receiveZone)
        {
            _receiveZone = receiveZone;
        }

        private IAnimation _scalePunchAnimation;

        #region MonoBehaviour

        private void Awake() => _scalePunchAnimation = new AnimationSequence(_scaleUpAnimation, _scaleDownAnimation);

        private void OnEnable() => StartObserving();

        private void OnDisable()
        {
            StopObserving();
            _scalePunchAnimation.SetEndState();
        }

        #endregion

        private void StartObserving() => _receiveZone.OnReceived += Punch;

        private void StopObserving() => _receiveZone.OnReceived -= Punch;

        private void Punch() => _scalePunchAnimation.PlayForward();
    }
}