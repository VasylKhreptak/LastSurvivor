using Main.Entities.Player;
using Plugins.Animations.Extensions;
using UnityEngine;
using Zenject;

namespace UI.Platforms
{
    public class UpgradeZoneLeftPointsAnimation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _root;
        [SerializeField] private RectTransform _rectTransform;

        [Header("Preferences")]
        [SerializeField] private float _startY;
        [SerializeField] private float _endY;
        [SerializeField] private Vector3 _startScale;
        [SerializeField] private Vector3 _endScale;
        [SerializeField] private float _minDistance;
        [SerializeField] private float _maxDistance;

        private Player _player;

        [Inject]
        private void Constructor(Player player)
        {
            _player = player;
        }

        private float _distance;
        private float _delta;
        private Vector3 _anchoredPosition;

        #region MonoBehaviour

        private void OnValidate() => _rectTransform ??= GetComponentInParent<RectTransform>(true);

        private void Update() => UpdateState();

        #endregion

        private void UpdateState()
        {
            _distance = Vector3.Distance(_root.position, _player.transform.position);
            _delta = 1 - Mathf.InverseLerp(_minDistance, _maxDistance, _distance);

            _anchoredPosition = _rectTransform.anchoredPosition3D;
            _anchoredPosition.y = Mathf.Lerp(_startY, _endY, _delta);
            _rectTransform.anchoredPosition3D = _anchoredPosition;

            _rectTransform.localScale = Vector3.Lerp(_startScale, _endScale, _delta);
        }

        private void OnDrawGizmosSelected()
        {
            if (_root == null)
                return;

            Gizmos.color = Color.red.WithAlpha(0.3f);
            Gizmos.DrawSphere(_root.position, _minDistance);

            Gizmos.color = Color.blue.WithAlpha(0.3f);
            Gizmos.DrawSphere(_root.position, _maxDistance);
        }
    }
}