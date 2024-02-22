using UnityEngine;

namespace Utilities.TransformUtilities
{
    public class TransformScaler : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] private Vector3 _startScale = Vector3.one;
        [SerializeField] private Vector3 _endScale = Vector3.one * 1.1f;
        [SerializeField] private float _speed = 3f;

        private Vector3 _targetScale;

        #region MonoBehaviour

        private void Update()
        {
            _targetScale = Mathf.PingPong(Time.time * _speed, 1) > 0.5f ? _endScale : _startScale;
            transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, Time.deltaTime * _speed);
        }

        private void OnDisable() => transform.localScale = _startScale;

        #endregion
    }
}