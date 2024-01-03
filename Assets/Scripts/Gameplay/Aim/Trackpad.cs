using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Gameplay.Aim
{
    public class Trackpad : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [Header("References")]
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private RectTransform _startPoint;

        [Header("Preferences")]
        [SerializeField] private float _sensitivity = 1f;

        private Canvas _canvas;

        [Inject]
        private void Constructor(Canvas canvas)
        {
            _canvas = canvas;
        }

        private readonly Vector2ReactiveProperty _anchoredPosition = new Vector2ReactiveProperty();
        private readonly BoolReactiveProperty _isPressed = new BoolReactiveProperty();

        private Vector3[] _corners = new Vector3[4];

        public IReadOnlyReactiveProperty<Vector2> AnchoredPosition => _anchoredPosition;
        public IReadOnlyReactiveProperty<bool> IsPressed => _isPressed;

        #region MonoBehaviour

        private void Awake() => _anchoredPosition.Value = _startPoint.anchoredPosition;

        private void OnValidate()
        {
            _rectTransform ??= GetComponent<RectTransform>();
        }

        #endregion

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.pointerId != 0)
                return;

            Rect rect = _rectTransform.rect;
            Vector2 position = _anchoredPosition.Value;

            position += eventData.delta * _sensitivity / _canvas.scaleFactor;

            position.x = Mathf.Clamp(position.x, 0, rect.width);
            position.y = Mathf.Clamp(position.y, 0, rect.height);

            _anchoredPosition.Value = position;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.pointerId != 0)
                return;

            _isPressed.Value = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.pointerId != 0)
                return;

            _isPressed.Value = false;
        }

        public Vector3 GetScreenPoint()
        {
            _rectTransform.GetWorldCorners(_corners);

            return _corners[0] + (Vector3)(_anchoredPosition.Value * _canvas.scaleFactor);
        }
    }
}