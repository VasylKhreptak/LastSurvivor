using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gameplay.Aim
{
    public class Trackpad : MonoBehaviour, IDragHandler
    {
        [Header("References")]
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Canvas _canvas;

        [Header("Preferences")]
        [SerializeField] private float _sensitivity = 1f;

        private readonly Vector2ReactiveProperty _position = new Vector2ReactiveProperty();

        public IReadOnlyReactiveProperty<Vector2> Position => _position;

        #region MonoBehaviour

        private void OnValidate()
        {
            _canvas ??= FindObjectOfType<Canvas>();
            _rectTransform ??= GetComponent<RectTransform>();
        }

        #endregion

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.pointerId != 0)
                return;

            Vector2 position = _position.Value;

            position += eventData.delta * _sensitivity / _canvas.scaleFactor;

            Rect rect = _rectTransform.rect;

            position.x = Mathf.Clamp(position.x, rect.xMin, rect.xMax);
            position.y = Mathf.Clamp(position.y, rect.yMin, rect.yMax);

            _position.Value = position;
        }
    }
}