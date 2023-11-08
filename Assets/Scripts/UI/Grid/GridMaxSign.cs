using System;
using Grid;
using Plugins.Animations;
using Plugins.Animations.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.Grid
{
    public class GridMaxSign : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _gameObject;

        [Header("Animations")]
        [SerializeField] private FadeAnimation _fadeShowAnimation;
        [SerializeField] private ScaleAnimation _scaleShowAnimation;

        private GridData _gridData;

        [Inject]
        private void Constructor(GridData gridData)
        {
            _gridData = gridData;
        }

        private IAnimation _showAnimation;

        private IDisposable _gridFullSubscription;

        #region MonoBehaviour

        private void Awake() => _showAnimation = new AnimationGroup(_fadeShowAnimation, _scaleShowAnimation);

        private void OnValidate()
        {
            if (transform.childCount > 0)
                _gameObject = transform.GetChild(0).gameObject;
        }

        private void OnEnable() => StartObserving();

        private void OnDisable()
        {
            StopObserving();
            _showAnimation.Stop();
        }

        #endregion

        private void StartObserving()
        {
            _gridFullSubscription = _gridData.IsFull.Subscribe(isFull =>
            {
                if (isFull)
                    Show();
                else
                    Hide();
            });
        }

        private void StopObserving() => _gridFullSubscription?.Dispose();

        private void Show()
        {
            _gameObject.SetActive(true);
            _showAnimation.PlayForward();
        }

        private void Hide() => _showAnimation.PlayBackward(() => _gameObject.SetActive(false));
    }
}