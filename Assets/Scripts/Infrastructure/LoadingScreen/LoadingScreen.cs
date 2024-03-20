using System;
using DG.Tweening;
using Infrastructure.LoadingScreen.Core;
using TMPro;
using UnityEngine;

namespace Infrastructure.LoadingScreen
{
    public class LoadingScreen : MonoBehaviour, ILoadingScreen
    {
        [Header("References")]
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private TMP_Text _infoTmp;

        [Header("Preferences")]
        [SerializeField] private float _duration;
        [SerializeField] private Ease _ease;

        private Tween _moveTween;

        #region MonoBehaviour

        private void OnDestroy()
        {
            KillTween();
        }

        #endregion

        public void Show()
        {
            KillTween();

            _rectTransform.anchoredPosition = Vector2.zero;
            gameObject.SetActive(true);
        }

        public void Hide(Action onComplete = null)
        {
            if (gameObject.activeSelf == false)
                return;

            KillTween();

            _moveTween = _rectTransform
                .DOAnchorPosY(_rectTransform.rect.height, _duration)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    onComplete?.Invoke();
                })
                .SetEase(_ease)
                .Play();
        }

        public void SetInfoText(string text) => _infoTmp.text = text;

        private void KillTween()
        {
            _moveTween.Kill();
        }
    }
}