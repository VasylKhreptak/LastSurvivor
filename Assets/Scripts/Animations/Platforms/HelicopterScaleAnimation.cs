using System;
using Data.Persistent;
using Plugins.Animations;
using Plugins.Animations.Core;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;

namespace Animations.Platforms
{
    public class HelicopterScaleAnimation : MonoBehaviour
    {
        [Header("Animations")]
        [SerializeField] private ScaleAnimation _scaleUpAnimation;
        [SerializeField] private ScaleAnimation _scaleDownAnimation;

        private HelicopterData _helicopterData;

        [Inject]
        private void Constructor(HelicopterData helicopterData)
        {
            _helicopterData = helicopterData;
        }

        private IAnimation _scaleAnimation;

        private IDisposable _fuelSubscription;

        #region MonoBehaviour

        private void Awake() => _scaleAnimation = new AnimationSequence(_scaleUpAnimation, _scaleDownAnimation);

        private void OnEnable() => StartObserving();

        private void OnDisable() => StopObserving();

        #endregion

        private void StartObserving()
        {
            _fuelSubscription = _helicopterData.FuelTank.Value
                .Pairwise()
                .Subscribe(pair =>
                {
                    if (pair.Current > pair.Previous)
                        Punch();
                });
        }

        private void StopObserving() => _fuelSubscription?.Dispose();

        [Button]
        private void Punch() => _scaleAnimation.PlayForward();
    }
}