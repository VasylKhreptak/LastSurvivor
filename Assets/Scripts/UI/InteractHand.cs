using Plugins.Animations;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Splines;

namespace UI
{
    public class InteractHand : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private UIBehaviour _interactionTarget;
        [SerializeField] private Transform _hand;
        [SerializeField] private SplineContainer _splineContainer;

        [Header("Preferences")]
        [SerializeField] private float _speed = 5f;
        [SerializeField] private FadeAnimation _fadeAnimation;

        private bool _wasPressed;

        private float _time;
        private Vector3 _evaluatedPosition;

        #region MonoBehaviour

        private void Update()
        {
            _time += Time.deltaTime * _speed;
            _time = Mathf.Repeat(_time, 1f);
            _evaluatedPosition = _splineContainer.Spline.EvaluatePosition(_time);
            _hand.position = _splineContainer.transform.TransformPoint(_evaluatedPosition);
        }

        private void OnEnable() => _interactionTarget.OnPointerDownAsObservable().Subscribe(_ => OnPointerDown()).AddTo(this);

        #endregion

        private void OnPointerDown()
        {
            if (_wasPressed)
                return;

            _fadeAnimation.PlayForward(() => Destroy(gameObject));

            _wasPressed = true;
        }
    }
}