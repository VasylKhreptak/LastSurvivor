using Data.Static.Balance.Animations;
using DG.Tweening;
using Infrastructure.Services.StaticData.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Animations
{
    public class ScalePressAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [Header("References")]
        [SerializeField] private Transform _transform;

        private ScalePressAnimationPreferences _preferences;

        [Inject]
        private void Constructor(IStaticDataService staticDataService)
        {
            _preferences = staticDataService.Balance.AnimationPreferences.ScalePressAnimationPreferences;
        }

        private Tween _tween;

        #region MonoBehaviour

        private void OnValidate() => _transform ??= GetComponent<Transform>();

        private void OnDisable()
        {
            KillAnimation();
            ResetScale();
        }

        #endregion

        public void OnPointerDown(PointerEventData eventData) => OnPointerDown();

        public void OnPointerUp(PointerEventData eventData) => OnPointerUp();

        private void OnPointerDown()
        {
            KillAnimation();

            _tween = _transform
                .DOScale(_preferences.PressedScale, _preferences.PressDuration)
                .SetEase(_preferences.PressCurve)
                .Play();
        }

        private void OnPointerUp()
        {
            KillAnimation();

            _tween = _transform
                .DOScale(_preferences.ReleasedScale, _preferences.ReleaseDuration)
                .SetEase(_preferences.ReleaseCurve)
                .Play();
        }

        private void KillAnimation() => _tween?.Kill();

        private void ResetScale() => _transform.localScale = _preferences.ReleasedScale;
    }
}