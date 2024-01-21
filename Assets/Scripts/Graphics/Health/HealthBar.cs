using System;
using DG.Tweening;
using Gameplay.Entities.Health.Core;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Graphics.Health
{
    public class HealthBar : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private Slider _immediateSlider;
        [SerializeField] private Slider _smoothSlider;

        [Header("Smooth Slider Preferences")]
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _delay = 0.3f;
        [SerializeField] private AnimationCurve _curve;

        private IHealth _health;

        [Inject]
        private void Constructor(IHealth health)
        {
            _health = health;
        }

        private Tween _tween;
        private IDisposable _healthSubscription;

        #region MonoBehaviour

        private void OnEnable()
        {
            _immediateSlider.value = _health.Value.Value;
            _smoothSlider.value = _health.Value.Value;
            StartObservingHealth();
        }

        private void OnDisable()
        {
            StopObservingHealth();
            _tween?.Kill();
        }

        #endregion

        private void StartObservingHealth() => _healthSubscription = _health.FillAmount.Subscribe(OnHealthChanged);

        private void StopObservingHealth() => _healthSubscription?.Dispose();

        private void OnHealthChanged(float healthFill)
        {
            if (_health.IsDeath.Value)
            {
                _gameObject.SetActive(false);
                _tween?.Kill();
                _immediateSlider.value = 0f;
                _smoothSlider.value = 0f;
                return;
            }

            if (_health.IsFull.Value)
            {
                _gameObject.SetActive(false);
                _immediateSlider.value = 1f;
                _smoothSlider.value = 1f;
                return;
            }

            _gameObject.SetActive(true);

            _immediateSlider.value = healthFill;

            _smoothSlider.gameObject.SetActive(true);
            _tween?.Kill();
            _tween = _smoothSlider
                .DOValue(healthFill, _duration)
                .SetDelay(_delay)
                .SetEase(_curve)
                .OnComplete(() => _smoothSlider.gameObject.SetActive(false))
                .Play();
        }
    }
}